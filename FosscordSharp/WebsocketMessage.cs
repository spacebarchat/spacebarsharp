using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FosscordSharp
{
    public class WebsocketMessage
    {
        [JsonProperty("op")] public int OpCode { get; set; } = 0;
        [JsonProperty("d")] public object? EventData { get; set; }

        [JsonProperty("s")] public int? SequenceNum { get; set; }

        [JsonProperty("t")] public string? EventName { get; set; }
    }

    public class ExplainedWebsocketMessage
    {
        public WebsocketMessage _msg;

        public ExplainedWebsocketMessage(WebsocketMessage msg)
        {
            _msg = msg;
        }
        public string EventDesc => _msg.OpCode switch
        {
            0 => "Dispatch",
            1 => "Heartbeat",
            2 => "Identify",
            3 => "Presence Update",
            4 => "Voice State Update",
            6 => "Resume",
            7 => "Reconnect",
            8 => "Request Guild Members (send)",
            9 => "Invalid Session",
            10 => "Hello",
            11 => "Heartbeat ACK",
            _ => "Unknown"
        };
    }
}