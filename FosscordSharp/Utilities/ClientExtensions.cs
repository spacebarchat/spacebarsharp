using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Core;
using FosscordSharp.ResponseTypes;
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
            if (!_resp.IsSuccessStatusCode)
            {
                if (_resp.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var ratelimit = JsonConvert.DeserializeObject<RateLimitResponse>(resp);
                    Console.WriteLine($"Ratelimited, trying again in {ratelimit.RetryAfter} seconds (endpoint: {url})");
                    Thread.Sleep((int)(ratelimit.RetryAfter*1000));
                    return await GetAsync<T>(cli, url);
                }
                return JsonConvert.DeserializeObject<ErrorResponse>(resp);
            }
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
            try
            {
                var _resp = await cli._httpClient.PostAsJsonAsync(url, data);
                var resp = await _resp.Content.ReadAsStringAsync();
                if (cli._config.Verbose) Util.Log(resp);
                if (!_resp.IsSuccessStatusCode)
                {
                    if (_resp.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        var ratelimit = JsonConvert.DeserializeObject<RateLimitResponse>(resp);
                        Console.WriteLine($"Ratelimited, trying again in {ratelimit.RetryAfter} seconds (endpoint: {url})");
                        Thread.Sleep((int)(ratelimit.RetryAfter*1000));
                        return await PostJsonAsync<T>(cli, url, data);
                    }
                    try
                    {
                        return JsonConvert.DeserializeObject<ErrorResponse>(resp);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        
                    }
                }
                var obj = JsonConvert.DeserializeObject<T>(resp);
                if (obj.GetType().IsArray)
                {
                    // Util.Log("obj is array");
                    foreach (var a in obj as T[])
                    {
                        // Util.Log("obj[] setting client");
                        ((a as FosscordObject)!)._client = cli;
                    }
                }
                else
                {
                    // Util.LogDebugStdout("obj is not array");
                    ((obj as FosscordObject)!)._client = cli;
                    // Util.LogDebugStdout("Set _client");
                }

                return obj;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static async Task<OneOf<T, ErrorResponse>> DeleteAsync<T>(this FosscordClient cli, string url) where T : class
        {
            var _resp = await cli._httpClient.DeleteAsync(url);
            var resp = await _resp.Content.ReadAsStringAsync();
            if(cli._config.Verbose) Util.Log(resp);
            if (!_resp.IsSuccessStatusCode)
            {
                if (_resp.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var ratelimit = JsonConvert.DeserializeObject<RateLimitResponse>(resp);
                    Console.WriteLine($"Ratelimited, trying again in {ratelimit.RetryAfter} seconds (endpoint: {url})");
                    Thread.Sleep((int)(ratelimit.RetryAfter*1000));
                    return await GetAsync<T>(cli, url);
                }
                return JsonConvert.DeserializeObject<ErrorResponse>(resp);
            }
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
    }
}