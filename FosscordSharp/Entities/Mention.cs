using FosscordSharp.Core;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Mention : FosscordObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("public_flags")]
        public int PublicFlags { get; set; }
    }
}