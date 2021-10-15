using System;
using Newtonsoft.Json;

namespace FosscordSharp.WebsocketData
{
    internal class Identify
    {
        internal string token;

        internal Properties properties;
        internal bool compress;
        internal int large_treshold;

        public Identify()
        {
            Util.Log("aa");
        }

        internal class Properties
        {
            [JsonProperty("$os")]
            internal string os;
            [JsonProperty("$browser")]
            internal string browser;
            [JsonProperty("$device")]
            internal string device;
        }
    }
}