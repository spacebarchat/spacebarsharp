using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FosscordSharp.ResponseTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("settings")]
        public Settings Settings;
    }


}