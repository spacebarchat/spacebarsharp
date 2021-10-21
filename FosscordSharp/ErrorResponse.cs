using Newtonsoft.Json;

namespace FosscordSharp
{
    public class Error
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }

    public class Errors
    {
        [JsonProperty("")]
        public Error[] errors { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public Errors Errors { get; set; }
    }
}