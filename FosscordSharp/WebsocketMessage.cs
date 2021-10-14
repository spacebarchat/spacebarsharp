using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FosscordSharp
{
    public class WebsocketMessage
    {
        [JsonProperty("op")] public int OpCode { get; set; } = 0;
        [JsonProperty("d")] public JObject? EventData { get; set; }

        [JsonProperty("s")] public int? SequenceNum { get; set; }

        [JsonProperty("t")] public string? EventName { get; set; }
    }
}