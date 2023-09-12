using FosscordSharp.Core;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Provider : FosscordObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}