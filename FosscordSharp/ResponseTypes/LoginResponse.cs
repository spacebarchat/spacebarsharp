using System.Text.Json.Serialization;
using FosscordSharp.Core;
using FosscordSharp.Entities;
using Newtonsoft.Json;

namespace FosscordSharp.ResponseTypes
{
    public class RegisterResponse : FosscordObject
    {
        [JsonProperty("token")]
        public string Token;
    }
    public class LoginResponse : RegisterResponse
    {
        [JsonProperty("settings")]
        public Settings Settings;
    }
}