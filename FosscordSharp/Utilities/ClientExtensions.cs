using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
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
            if(cli._config.Verbose) cli.debugLog.Log(resp);
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
                if (cli._config.Verbose) cli.debugLog.Log(resp);
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

                try {
                    var obj = JsonConvert.DeserializeObject<T>(resp);
                    if (obj.GetType().IsArray) {
                        // _client.log.Log("obj is array");
                        foreach (var a in obj as T[]) {
                            // _client.log.Log("obj[] setting client");
                            ((a as FosscordObject)!)._client = cli;
                        }
                    }
                    else {
                        // _client.log.LogDebugStdout("obj is not array");
                        ((obj as FosscordObject)!)._client = cli;
                        // _client.log.LogDebugStdout("Set _client");
                    }

                    return obj;
                }
                catch (Exception e) {
                    throw new Exception($"Post request failed on {cli._config.Endpoint}/{url}: ", e);
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(new Exception($"Post request failed on {cli._config.Endpoint}/{url}: ", e));
                throw;
            }
        }
        public static async Task<OneOf<T, ErrorResponse>> DeleteAsync<T>(this FosscordClient cli, string url) where T : class
        {
            var _resp = await cli._httpClient.DeleteAsync(url);
            var resp = await _resp.Content.ReadAsStringAsync();
            if(cli._config.Verbose) cli.log.Log(resp);
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
        public static async Task<OneOf<T, ErrorResponse>> PostFileWithDataAsync<T>(this FosscordClient cli, string url, object data, object file)
        {
            try {
                var f = new MultipartFormDataContent();
                // f.Add(new StringContent());
                
                
                var _resp = await cli._httpClient.PostAsJsonAsync(url, data);
                var resp = await _resp.Content.ReadAsStringAsync();
                if (cli._config.Verbose) cli.debugLog.Log(resp);
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
                    // _client.log.Log("obj is array");
                    foreach (var a in obj as T[])
                    {
                        // _client.log.Log("obj[] setting client");
                        ((a as FosscordObject)!)._client = cli;
                    }
                }
                else
                {
                    // _client.log.LogDebugStdout("obj is not array");
                    ((obj as FosscordObject)!)._client = cli;
                    // _client.log.LogDebugStdout("Set _client");
                }

                return obj;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}