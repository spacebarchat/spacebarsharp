using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
        public FosscordClient(FosscordClientConfig config)
        {
            _config = config;
            _httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri(_config.Endpoint)
            };
        }

        public async Task Login()
        {
            Util.Log("Attempting login");
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/auth/login", new {Login = _config.Email, password = _config.Password, undelete = false}, CancellationToken.None);
            string _resp = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(_resp);
            if (_resp.Contains("E-Mail or Phone not found"))
            {
                if (_config.ShouldRegister)
                {
                    Util.Log("Account doesn't exist, registering");
                    await Register();                    
                }
                else
                {
                    Util.Log("Account doesn't exist, registering disabled!");
                }
            }
            else
            {
                Util.Log("Successfully logged in!");
                _loginResponse = JsonConvert.DeserializeObject<LoginResponse>(_resp);
                _httpClient.DefaultRequestHeaders.Add("Authorization", _loginResponse.Token);
                Util.LogDebug(_loginResponse.Token);
                Util.Log("Set token!");
            }

            return;
        }

        private async Task Register()
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/auth/register", new {email = _config.Email, password = _config.Password, consent = true, date_of_birth = _config.RegistrationOptions.DateOfBirth, username = _config.RegistrationOptions.Username}, CancellationToken.None);
            string _resp = await resp.Content.ReadAsStringAsync();
            // Console.WriteLine(_resp);
            Util.Log("Successfully registered!");
            await Login();
            Guild defaultGuild = await CreateGuild(_config.RegistrationOptions.Username + "'s Official Discord!");
            Util.Log($"Created default guild '{_config.RegistrationOptions.Username + "'s Official Discord!"}'");
            
            Channel[] channels = await defaultGuild.GetChannels();
            Util.Log("Got channels");
            Console.WriteLine(JsonConvert.SerializeObject(channels));
            Util.Log($"Fetched {channels.Length} channels in default guild!");
            Util.Log($"Default guild invite: {_config.Endpoint}/invite/{(await channels[0].CreateInvite(temporary_membership:false)).Code}");
        }

        public async Task<Guild[]> GetGuilds()
        {
            var res = await _httpClient.GetStringAsync("/api/v9/users/@me/guilds");
            Util.Log(res);
            var a = JsonConvert.DeserializeObject<JArray>(res);
            List < Guild > guilds = new();
            Util.Log(a.Count+"");
            foreach (var jToken in a)
            {
                var b = jToken.ToObject<Guild>();
                var c = b.Name;
                guilds.Add(b);
                Util.Log(c);
            }
            Util.Log(a + "");
            // foreach (var guild in a)
            // {
            //     guild._client = this;
            // }
            return new []{new Guild()};
            // return await _httpClient.GetFromJsonAsync<Guild[]>("/api/v9/users/@me/guilds");
        }
        public async Task<Guild> GetGuild(ulong id)
        {
            return new Guild(this);
        }

        public async Task<Guild> CreateGuild(string name)
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/guilds",
                new { name = name });
            string _resp = await resp.Content.ReadAsStringAsync();
            var gr = JsonConvert.DeserializeObject<GuildCreatedResponse>(_resp);
            string guildid = gr.id;
            Util.Log("Got guild id " + guildid);
        
            Guild guild = new Guild(this)
            {
                id = gr.Id
            };
            Util.Log("Instantiated guild on creation");
            return guild;
        }
    }
}