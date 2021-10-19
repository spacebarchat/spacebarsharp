using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Author
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("avatar")]
        public object Avatar { get; set; }

        [JsonProperty("accent_color")]
        public object AccentColor { get; set; }

        [JsonProperty("banner")]
        public object Banner { get; set; }

        [JsonProperty("bot")]
        public bool Bot { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("public_flags")]
        public int PublicFlags { get; set; }
    }
}