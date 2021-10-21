using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FosscordSharp.Core;
using Newtonsoft.Json;
using OneOf;

namespace FosscordSharp.Utilities
{
    public static class ClientExtensions
    {
        public static async Task<OneOf<T, ErrorResponse>> GetAsync<T>(this FosscordClient cli, string url) where T : class
        {
            var _resp = await cli._httpClient.GetAsync(url);
            var resp = await _resp.Content.ReadAsStringAsync();
            if(cli._config.Verbose) Util.Log(resp);
            if (!_resp.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ErrorResponse>(resp);
            object obj = JsonConvert.DeserializeObject<T>(resp);
            if (obj.GetType().IsArray)
            {
                foreach (var a in (IEnumerable)obj)
                {
                    ((a as FosscordObject)!)._client = cli;    
                }
            }
            else
            {
                ((obj as FosscordObject)!)._client = cli;                
            }
            return (T)obj;
        }
        public static async Task<OneOf<T, ErrorResponse>> PostJsonAsync<T>(this FosscordClient cli, string url, object data)
        {
            int i = 0;
            Util.Log("Post step " + ++i); //1
            var _resp = await cli._httpClient.PostAsJsonAsync(url, data);
            Util.Log("Post step " + ++i); //2
            var resp = await _resp.Content.ReadAsStringAsync();
            Util.Log("Post step " + ++i);//3
            if(cli._config.Verbose) Util.Log(resp);
            Util.Log("Post step " + ++i);//4
            if(!_resp.IsSuccessStatusCode) return JsonConvert.DeserializeObject<ErrorResponse>(resp);
            Util.Log("Post step " + ++i);//5
            var obj = JsonConvert.DeserializeObject<T>(resp);
            Util.Log("Post step " + ++i);//6
            if (obj.GetType().IsArray)
            {
                Util.Log("obj is array");
                foreach (var a in obj as T[])
                {
                    Util.Log("obj[] setting client");
                    ((a as FosscordObject)!)._client = cli; 
                    
                }                
            }
            else
            {
                Util.Log("obj is not array");
                ((obj as FosscordObject)!)._client = cli;    
                Util.Log("Set _client");
            }
            Util.Log("Post step " + ++i);//7

            
            return obj;
        }
    }
}