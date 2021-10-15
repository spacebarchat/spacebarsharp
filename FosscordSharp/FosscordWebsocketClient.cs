using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Utilities;
using FosscordSharp.WebsocketData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FosscordSharp
{
    public class FosscordWebsocketClient
    {
        internal FosscordClient _client;
        internal ClientWebSocket ws = new();
        private int? seq_id = null;
        private ArraySegment<Byte> identpk;
        
        public FosscordWebsocketClient(FosscordClient client)
        {
            _client = client;
            Util.Log("a");
            var ide = new WebsocketMessage()
            {
                OpCode = 2,
                EventData = JObject.FromObject(new Identify()
                {
                    token = _client._loginResponse.Token,
                    properties = new()
                    {
                        browser = "fosscordsharp",
                        device = "something",
                        os = "windows"
                    },
                    compress = false,
                    large_treshold = 250
                })
            };
            Util.Log("aaa");
            identpk = JsonConvert.SerializeObject(ide).ToArraySegment();
            // identpk = JsonConvert.SerializeObject(new WebsocketMessage()
            // {
            //     OpCode = 2,
            //     EventData = new JObject(new
            //     {
            //         token = _client._loginResponse.Token,
            //         properties = new
            //         {
            //             os = "windows",
            //             browser = "fosscordsharp",
            //             device = "something"
            //         },
            //         compress = false,
            //         large_treshold = 250,
            //         presence = new
            //         {
            //             activities = new
            //                 object[]
            //                 {
            //                     new
            //                     {
            //                         name = "FosscordSharp bot",
            //                         type = 0
            //                     }
            //                 },
            //             status = "online",
            //             afk = false
            //         }
            //     })
            // }).ToArraySegment();
            Util.Log("b");
        }

        public async Task Start()
        {
            Util.Log("initialising websocket");
            await ws.ConnectAsync(
                new Uri(
                    $"wss://{_client._config.Endpoint.Replace("https://", "").Replace("http://", "")}?encoding=json&v=9"),
                CancellationToken.None);
            Util.LogDebug("Connected to websocket!");
            var rcvBytes = new byte[128];
            var rcvBuffer = new ArraySegment<byte>(rcvBytes);
            WebSocketReceiveResult rcvResult;
            Task.Run(async () =>
            {
                while (true)
                {
                    rcvResult = await ws.ReceiveAsync(rcvBuffer, CancellationToken.None);
                    byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
                    string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                    Util.LogDebug($"Received: {rcvMsg}");
                    WebsocketMessage msg = JsonConvert.DeserializeObject<WebsocketMessage>(rcvMsg);
                    Util.Log($"Deserialized msg: {msg != null}!");
                    if (msg != null)
                    {
                        Util.LogDebug(JsonConvert.SerializeObject(msg));
                        Util.LogDebug($"rcv opcode {msg.OpCode}");
                        seq_id = msg.SequenceNum ?? seq_id;
                        await HandleWSMessage(msg);
                    }
                    else
                    {
                        Util.LogDebug($"WS state: {ws.State}");
                        Util.LogDebug($"WS close msg {ws.CloseStatus}");
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Received invalid message",
                            CancellationToken.None);
                        await ws.ConnectAsync(
                            new Uri(
                                $"wss://{_client._config.Endpoint.Replace("https://", "").Replace("http://", "")}?encoding=json&v=9"),
                            new CancellationToken());
                        Util.LogDebug("WS: Reconnected");
                    }
                }
            }, CancellationToken.None);
        }

        private async Task HandleWSMessage(WebsocketMessage msg)
        {
            switch (msg.OpCode)
            {
                case 0: //Dispatch
                    Util.Log("WS: Event dispatched!");
                    break;
                case 1: //Heartbeat
                    Util.Log("Sending heartbeat..");
                    await ws.SendAsync(("{\"op\": 1, \"d\": " + seq_id + "}").ToArraySegment(), WebSocketMessageType.Text, false, CancellationToken.None);
                    Util.Log("Heartbeat success!");
                    break;
                case 2: //Identify
                    Util.Log("WS: invalid msg identify");
                    break;
                case 3: //Presence Update

                    break;
                case 4: //Voice state update

                    break;
                case 6: //resume
                    Util.Log("WS: invalid msg resume");
                    break;
                case 7: //Reconnect
                    Util.Log("WS: Reconnect!");
                    break;
                case 8: //Request Guild Members (send)
                    break;
                case 9: //Invalid session
                    Util.Log("WS: invalid session!");
                    break;
                case 10: //Hello
                    Util.Log("WebSocket: Hello!");
                    var a = msg.EventData?.Property("heartbeat_interval")?.Value.ToObject<int>();
                    Util.Log($"Heartbeat interval: {a}");

                    Util.Log("Sending ident..");
                    await ws.SendAsync(identpk, WebSocketMessageType.Text, false, CancellationToken.None);

                    var t = new System.Timers.Timer((double)a! / 2d);
                    t.Enabled = true;
                    t.Elapsed += (_, _) =>
                    {
                        HandleWSMessage(new() { OpCode = 1 });
                    };
                    t.Start();
                    break;
                case 11: //Heartbeat ACK
                    Util.Log("Heartbeat ACK");
                    break;
                default:
                    Util.Log($"Unknown opcode {msg.OpCode}! Report this!");
                    break;
            }

            Util.Log("Passed switch block!");
        }
    }
}