using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;
using FosscordSharp.Utilities;
using FosscordSharp.WebsocketData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket = WebSocket4Net.WebSocket;
using WebSocketState = WebSocket4Net.WebSocketState;

namespace FosscordSharp
{
    public class FosscordWebsocketClient
    {
        internal FosscordClient _client;
        internal WebSocket ws;
        private int? seq_id = null;
        private string ident;

        public FosscordWebsocketClient(FosscordClient client)
        {
            _client = client;
            var ide = new WebsocketMessage()
            {
                OpCode = 2,
                EventData = new Identify()
                {
                    token = _client._loginResponse.Token,
                    properties = new()
                    {
                        browser = "FosscordSharp/"+RuntimeInfo.LibVersion,
                        device = "idk",
                        os = "no idea"
                    },
                    compress = false,
                    // large_treshold = 250
                }
            };
            ident = JsonConvert.SerializeObject(ide);
        }

        public async Task Start()
        {
            ws = new WebSocket($"wss://{_client._config.Endpoint.Replace("https://", "").Replace("http://", "")}?encoding=json&v=9");
            ws.MessageReceived += (sender, args) =>
            {
                // Console.WriteLine("Websocket message received: " + args.Message);
                HandleWSMessage(JsonConvert.DeserializeObject<WebsocketMessage>(args.Message));
            };
            ws.Opened += (sender, args) =>
            {
                _client.log.Log("WebSocket opened");
            };
            ws.Closed += (sender, args) =>
            {
                _client.log.Log("WebSocket closed");
                try
                {
                    ws.Open();
                }
                catch{}
            };
            ws.Error += (sender, args) =>
            {
                _client.log.Log("WebSocket errored");
            };
            Console.WriteLine("Connecting to websocket!");
            ws.Open();
        }

        private async Task HandleWSMessage(WebsocketMessage msg)
        {
            // Console.WriteLine(JsonConvert.SerializeObject(new ExplainedWebsocketMessage(msg)));
            switch (msg.OpCode)
            {
                case 0: //Dispatch
                    // _client.log.Log("WS: Event dispatched!");
                    switch (msg.EventName)
                    {
                        case "MESSAGE_CREATE":
                            var m = JsonConvert.DeserializeObject<Message>(JsonConvert.SerializeObject(msg.EventData));
                            m._client = _client;
                            m.SetClientInTree(_client);
                            _client.OnMessageReceived(new()
                            {
                                Message = m,
                                Client = _client
                            });
                            break;
                        default:
                            Directory.CreateDirectory("dispatch");
                            File.WriteAllText("dispatch/"+msg.EventName+".txt", JsonConvert.SerializeObject(msg, Formatting.Indented));
                            _client.log.LogDebug("Unknown dispatch event: " + JsonConvert.SerializeObject(new ExplainedWebsocketMessage(msg)));
                            break;
                    }
                    // File.WriteAllText("dispatch.txt", JsonConvert.SerializeObject(msg, Formatting.Indented));
                    break;
                case 1: //Heartbeat
                    // _client.log.Log("Sending heartbeat..");
                    ws.Send(JsonConvert.SerializeObject(new WebsocketMessage(){OpCode = 1, EventData = seq_id}));
                    // _client.log.Log("Heartbeat success!");
                    break;
                case 2: //Identify
                    _client.log.Log("WS: invalid msg identify");
                    break;
                case 3: //Presence Update
        
                    break;
                case 4: //Voice state update
        
                    break;
                case 6: //resume
                    _client.log.Log("WS: invalid msg resume");
                    break;
                case 7: //Reconnect
                    _client.log.Log("WS: Reconnect!");
                    ws.Close();
                    // ws.Open();
                    // _client.log.LogDebug("WS: Reconnected");
                    break;
                case 8: //Request Guild Members (send)
                    break;
                case 9: //Invalid session
                    _client.log.LogDebug("WS: invalid session!");
                    break;
                case 10: //Hello
                    _client.log.LogDebug("WebSocket: Hello!");
                    var a = ((JObject)msg.EventData)?.Property("heartbeat_interval")?.Value.ToObject<int>();
                    _client.log.LogDebug($"Heartbeat interval: {a}");
                    
                     ws.Send(ident);
                    _client.log.LogDebug("Sent: " + ident);
                    
                    var t = new System.Timers.Timer((double)a! / 2d);
                    t.Enabled = true;
                    t.Elapsed += (_, _) =>
                    {
                        if (ws.State != WebSocketState.Open) HandleWSMessage(new WebsocketMessage() { OpCode = 7 });
                        else HandleWSMessage(new() { OpCode = 1 });
                    };
                    t.Start();
                    break;
                case 11: //Heartbeat ACK
                    // _client.log.Log("Heartbeat ACK");
                    break;
                default:
                    _client.log.Log($"Unknown opcode {msg.OpCode}! Report this!");
                    break;
            }
        
            // _client.log.Log("Passed switch block!");
        }
    }
}