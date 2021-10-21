using System.Text.Json.Serialization;
using FosscordSharp.Core;
using Newtonsoft.Json;

namespace FosscordSharp.ResponseTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class LoginResponse : FosscordObject
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("settings")]
        public Settings Settings;
    }


}