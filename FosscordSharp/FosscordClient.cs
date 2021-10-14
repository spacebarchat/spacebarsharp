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
                ClientWebSocket ws = new();
                await ws.ConnectAsync(
                    new Uri(
                        $"ws://{_config.Endpoint.Replace("https://", "").Replace("http://", "")}?encoding=json&v=9"),
                    new CancellationToken());
                Util.LogDebug("Connected to websocket!");
                var rcvBytes = new byte[128];
                var rcvBuffer = new ArraySegment<byte>(rcvBytes);
                var cts = new CancellationTokenSource();
                while (true)
                {
                    WebSocketReceiveResult rcvResult = await ws.ReceiveAsync(rcvBuffer, cts.Token);
                    byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
                    string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                    Console.WriteLine("Received: {0}", rcvMsg);
                    WebsocketMessage msg = JsonConvert.DeserializeObject<WebsocketMessage>(rcvMsg);
                    Util.Log($"Deserialized msg: {msg != null}!");
                    Util.Log(JsonConvert.SerializeObject(msg));
                    Util.Log($"rcv opcode {msg.OpCode}");
                    switch (msg.OpCode)
                    {
                        case 0: //Dispatch

                            break;
                        case 1: //Heartbeat

                            break;
                        case 2: //Identify

                            break;
                        case 3: //Presence Update

                            break;
                        case 4: //Voice state update

                            break;
                        case 5: //Resume

                            break;
                        case 6: //resume

                            break;
                        case 7: //Reconnect

                            break;
                        case 8: //Request Guild Members (send)

                            break;
                        case 9: //Invalid session

                            break;
                        case 10: //Hello
                            var x = msg.EventData?.GetType().GetField("heartbeat_interval").GetValue(msg.EventData);
                            Util.Log($"Heartbeat interval: {x as int?}");
                            break;
                        case 11: //Heartbeat ACK

                            break;
                        default:
                            Util.Log($"Unknown opcode {msg.OpCode}! Report this!");
                            break;
                    }
                }
            }

            return;
        }

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

        public async Task<Guild> GetGuild(ulong id)
        {
            return (await GetGuilds()).First(x => x.Id == id);
            // return new Guild(this);
        }

        public async Task<Guild> CreateGuild(string name)
        {
            HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("/api/v9/guilds",
                new { name = name });
            string _resp = await resp.Content.ReadAsStringAsync();
            var gr = JsonConvert.DeserializeObject<GuildCreatedResponse>(_resp);
            Util.LogDebug("Got guild id " + gr.Id);

            return await GetGuild(gr.Id);
        }
    }
}