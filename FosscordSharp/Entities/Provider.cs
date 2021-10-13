using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Provider
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}