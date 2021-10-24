using System;
using Newtonsoft.Json;

namespace FosscordSharp.WebsocketData
{
    internal class Identify
    {
        public string token;

        public Properties properties;
        public bool compress;
        // public int large_treshold;

        internal class Properties
        {
            [JsonProperty("$os")] public string os;
            [JsonProperty("$browser")] public string browser;
            [JsonProperty("$device")] public string device;
        }
    }
}