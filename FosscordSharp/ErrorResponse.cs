using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FosscordSharp
{
    public class ErrorResponse
    {
        [JsonProperty("code")] public int Code { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("errors")] public Dictionary<string, ErrorList> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
            // return $"{Code}: {Message} => {Errors.Values.Sum(x=>x.Errors.Length)} errors:\n" + string.Join("\n", Errors.Select(x=>$"{x.Key}:\n - {string.Join("\n - ", String.Join("---", x.Value.Errors.Select(y=>y.Code + ": " + y.Message)))}"));
        }
    }
    public class ErrorList
    {
        [JsonProperty("_errors")] public Error[] Errors { get; set; }
    }
    public class Error
    {
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("code")] public string Code { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }
}