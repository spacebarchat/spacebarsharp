using Newtonsoft.Json;

namespace FosscordSharp.ResponseTypes
{
    public class RateLimitResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("retry_after")]
        public double RetryAfter { get; set; }

        [JsonProperty("global")]
        public bool Global { get; set; }
    }
}