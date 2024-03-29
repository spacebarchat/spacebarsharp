﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ArcaneLibs.Logging;
using ArcaneLibs.Logging.LogEndpoints;
using FosscordSharp.Entities;
using FosscordSharp.EventArgs;
using FosscordSharp.ResponseTypes;
using FosscordSharp.Services;
using FosscordSharp.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FosscordSharp
{
    public class FosscordClient
    {
        internal FosscordClientConfig _config;
        internal HttpClient _httpClient;
        internal RegisterResponse _loginResponse;
        internal FosscordWebsocketClient _wscli;
        internal LogManager log, debugLog;
        
        /// <summary>
        /// Constructor, requires a configuration to be passed
        /// </summary>
        /// <param name="config">Library configuration</param>
        public FosscordClient(FosscordClientConfig config)
        {
            _config = config;
            _httpClient = config.Verbose
                ? new HttpClient(new LoggingHandler(new HttpClientHandler()))
                {
                    BaseAddress = new Uri(_config.Endpoint)
                }
                : new HttpClient()
                {
                    BaseAddress = new Uri(_config.Endpoint)
                };
            if(_config.LogManager != null) log = _config.LogManager;
            else
            {
                log = new LogManager();
                log.AddEndpoint(new ConsoleEndpoint());
            }
            if(_config.DebugLogManager != null) debugLog = _config.DebugLogManager;
            else
            {
                log = new LogManager();
                log.AddEndpoint(new DebugEndpoint());
            }
            log.Log("Initialised new Fosscord client: FosscordSharp/"+RuntimeInfo.LibVersion);
        }
        /// <summary>
        /// Connect to the instance and log in
        /// </summary>
        public async Task Login()
        {
            log.Log("Attempting login");
            var loginResp = await this.PostJsonAsync<LoginResponse>("/api/v9/auth/login", new { Login = _config.Email, password = _config.Password, undelete = false });
            if (loginResp.IsT1)
            {
                log.Log("Login failed: " + loginResp.AsT1.Errors.First().Value.Errors[0].Message);
                if (loginResp.AsT1.Errors.First().Value.Errors[0].Code == "INVALID_LOGIN")
                {
                    if (_config.ShouldRegister) await Register();
                    else throw new UnauthorizedAccessException("User does not exist!");
                    return;
                }

                throw new Exception(loginResp.AsT1.ToString());
            }
            _loginResponse = loginResp.AsT0;
            _httpClient.DefaultRequestHeaders.Add("Authorization", _loginResponse.Token);
            log.Log("Logged in on REST API!");
            if (!_config.PollMessages) {
                _wscli = new FosscordWebsocketClient(this);
                await _wscli.Start();
            }

            PostLogin();
        }
        /// <summary>
        /// Register a new account
        /// </summary>
        private async Task Register()
        {
            var resp = await this.PostJsonAsync<RegisterResponse>("/api/v9/auth/register", new
            {
                email = _config.Email, password = _config.Password, consent = true,
                date_of_birth = _config.RegistrationOptions.DateOfBirth,
                username = _config.RegistrationOptions.Username
            });
            if (resp.IsT1)
            {
                throw new Exception(_config.Endpoint + ":\n" + resp.AsT1.ToString());
            }
            _loginResponse = resp.AsT0;
            _httpClient.DefaultRequestHeaders.Add("Authorization", _loginResponse.Token);
            log.Log("Registered and logged in on REST API!");
            if(_config.RegistrationOptions.CreateBotGuild){
                Guild defaultGuild = await CreateGuild(_config.RegistrationOptions.Username + "'s Official Discord!");
                log.LogDebug($"Created default guild '{defaultGuild.Name}'");

                Channel[] channels = await defaultGuild.GetChannels();
                log.LogDebug($"Default guild invite: {_config.Endpoint}/invite/{(await channels[0].CreateInvite(temporary_membership: false)).Code}");
            }
            
            // _wscli = new FosscordWebsocketClient(this);
            // await _wscli.Start();
            await new FosscordWebsocketClient(this).Start();
            // _client.log.Log("Logged in on WS API!");
            PostLogin();
        }

        internal void PostLogin()
        {
            if (_config.PollMessages)
            {
                log.Log("Starting poller");
                new MessagePoller(this).Start();
                log.Log("Started poller");
            }
        }
        /// <summary>
        /// Gets all guilds the bot is in
        /// </summary>
        /// <returns>List of guilds the bot is in</returns>
        public async Task<Guild[]> GetGuilds()
        {
            var res = await this.GetAsync<Guild[]>("/api/v9/users/@me/guilds");
            if (!res.IsT0) throw new Exception(res.AsT1 + "");
            return res.AsT0;
        }

        /// <summary>
        /// Get a specific guild
        /// </summary>
        /// <param name="id">Guild ID</param>
        /// <returns>Guild</returns>
        public async Task<Guild> GetGuild(ulong id)
        {
            Guild[] guilds = await GetGuilds();
            if (!guilds.Any(x => x.Id == id)) throw new NullReferenceException("Not a member of this guild!");
            return (guilds).First(x => x.Id == id);
        }
        
        /// <summary>
        /// Create a guild
        /// </summary>
        /// <param name="name">Guild name</param>
        /// <returns>Newly created guild</returns>
        public async Task<Guild> CreateGuild(string name)
        {
            var res = await this.PostJsonAsync<GuildCreatedResponse>("/api/v9/guilds", new { name = name });
            if (res.IsT1) throw new Exception(res.AsT1 + "");
            return await GetGuild(res.AsT0.id);
        }

        public async Task<Guild> JoinGuild(string code)
        {
            var g = await this.PostJsonAsync<Invite>($"/api/v9/invites/{code}", new { });
            if (g.IsT1) throw new Exception(g.AsT1 + "");
            Thread.Sleep(100);
            return await GetGuild(g.AsT0.GuildId);
        }

        public async Task<User> GetUser(int id = 0)
        {
            var g = await this.GetAsync<User>($"/api/v9/users/" + (id == 0 ? "@me" : id));
            if (g.IsT1) throw new Exception(g.AsT1 + "");
            g.AsT0.SetClientInTree(this);
            return g.AsT0;
        }

        public async Task<User> GetCurrentUser()
        {
            return await GetUser();
        }
        
        
        #region events

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
            e.Message.SetClientInTree(this);
            handler.Invoke(this, e);
        }

        #endregion
    }
}