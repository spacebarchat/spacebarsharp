using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;
using FosscordSharp.ResponseTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FosscordSharp
{
    public class FosscordClient
    {
        internal FosscordClientConfig _config;
        internal HttpClient _httpClient;
        internal LoginResponse _loginResponse;
        internal FosscordWebsocketClient _wscli;
        
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
        }
        /// <summary>
        /// Connect to the instance and log in
        /// </summary>
        public async Task Login()
        {
            Util.Log("Attempting login");
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/auth/login",
                new { Login = _config.Email, password = _config.Password, undelete = false }, CancellationToken.None);
            string _resp = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(_resp);
            if (_resp.Contains("E-Mail or Phone not found"))
            {
                if (_config.ShouldRegister)
                {
                    Util.LogDebug("Account doesn't exist, registering");
                    await Register();
                }
                else
                {
                    Util.LogDebug("Account doesn't exist, registering disabled!");
                }
            }
            else
            {
                Util.LogDebug("Successfully logged in!");
                _loginResponse = JsonConvert.DeserializeObject<LoginResponse>(_resp);
                _httpClient.DefaultRequestHeaders.Add("Authorization", _loginResponse.Token);
                Util.LogDebug(_loginResponse.Token);
                Util.LogDebug("Set token!");
                Util.Log("Logged in on REST API!");
                // _wscli = new FosscordWebsocketClient(this);
                // await _wscli.Start();
                // Util.Log("Logged in on WS API!");
            }

            return;
        }
        /// <summary>
        /// Register a new account
        /// </summary>
        private async Task Register()
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/auth/register",
                new
                {
                    email = _config.Email, password = _config.Password, consent = true,
                    date_of_birth = _config.RegistrationOptions.DateOfBirth,
                    username = _config.RegistrationOptions.Username
                }, CancellationToken.None);
            string _resp = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(_resp);
            Util.LogDebug("Successfully registered!");
            await Login();
            Guild defaultGuild = await CreateGuild(_config.RegistrationOptions.Username + "'s Official Discord!");
            Util.LogDebug($"Created default guild '{_config.RegistrationOptions.Username + "'s Official Discord!"}'");

            Channel[] channels = await defaultGuild.GetChannels();
            Util.LogDebug("Got channels");
            Util.LogDebug($"Fetched {channels.Length} channels in default guild!");
            Util.LogDebug(
                $"Default guild invite: {_config.Endpoint}/invite/{(await channels[0].CreateInvite(temporary_membership: false)).Code}");
        }
        /// <summary>
        /// Gets all guilds the bot is in
        /// </summary>
        /// <returns>List of guilds the bot is in</returns>
        public async Task<Guild[]> GetGuilds()
        {
            var res = JsonConvert.DeserializeObject<Guild[]>(
                await _httpClient.GetStringAsync("/api/v9/users/@me/guilds"));
            foreach (var temp in res)
            {
                temp._client = this;
            }

            return res;
            // return await _httpClient.GetFromJsonAsync<Guild[]>("/api/v9/users/@me/guilds");
        }

        /// <summary>
        /// Get a specific guild
        /// </summary>
        /// <param name="id">Guild ID</param>
        /// <returns>Guild</returns>
        public async Task<Guild> GetGuild(ulong id)
        {
            Guild[] guilds = await GetGuilds();
            return (guilds).First(x => x.Id == id);
            // return new Guild(this);
        }
        
        /// <summary>
        /// Create a guild
        /// </summary>
        /// <param name="name">Guild name</param>
        /// <returns>Newly created guild</returns>
        public async Task<Guild> CreateGuild(string name)
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/guilds",
                new { name = name });
            string _resp = await resp.Content.ReadAsStringAsync();
            var gr = JsonConvert.DeserializeObject<GuildCreatedResponse>(_resp);
            Util.LogDebug("Got guild id " + gr.id);

            return await GetGuild(gr.id);
        }

        public async Task<Guild> JoinGuild(string code)
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync($"/api/v9/invites/{code}",
                new { });
            // Util.Log(await resp.Content.ReadAsStringAsync());
            var gr = JsonConvert.DeserializeObject<Invite>(await resp.Content.ReadAsStringAsync());
            Util.LogDebug("Got guild id " + gr.GuildId);
            
            return await GetGuild(gr.GuildId);
        }
    }
}