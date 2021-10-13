using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Channel
    {
        internal FosscordClient _client;
        
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("last_message_id")]
        public object LastMessageId { get; set; }

        [JsonProperty("guild_id")]
        public string GuildId { get; set; }

        [JsonProperty("parent_id")]
        public object ParentId { get; set; }

        [JsonProperty("owner_id")]
        public object OwnerId { get; set; }

        [JsonProperty("last_pin_timestamp")]
        public object LastPinTimestamp { get; set; }

        [JsonProperty("default_auto_archive_duration")]
        public object DefaultAutoArchiveDuration { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public List<object> PermissionOverwrites { get; set; }

        [JsonProperty("video_quality_mode")]
        public object VideoQualityMode { get; set; }

        [JsonProperty("bitrate")]
        public object Bitrate { get; set; }

        [JsonProperty("user_limit")]
        public object UserLimit { get; set; }

        [JsonProperty("nsfw")]
        public object Nsfw { get; set; }

        [JsonProperty("rate_limit_per_user")]
        public object RateLimitPerUser { get; set; }

        [JsonProperty("topic")]
        public object Topic { get; set; }
        
        public async Task<Invite> CreateInvite(int duration = 0, int max_uses = 0, bool temporary_membership = true)
        {
            Util.LogDebug($"Creating invite for {Id}/{Name}");
            HttpResponseMessage resp = await _client._httpClient.PostAsJsonAsync($"/api/v9/channels/{Id}/invites",
                new { max_age = duration, max_uses = max_uses, temporary = temporary_membership  });
            Invite invite = await resp.Content.ReadFromJsonAsync<Invite>();
            invite._client = _client;
            return invite;
        }

        public async Task<Message[]> GetMessages(ulong? before = null, ulong? after = null, int? amount = 100)
        {
            if(before == null && after == null) before = UInt64.MaxValue;
            Util.LogDebug($"Fetching {amount} messages {(after == null ? $"before {before}" : $"after {after}")}");
            Util.LogDebug(await _client._httpClient.GetStringAsync($"/api/v9/channels/{Id}/messages?amount={amount}&{(after == null ? "before="+before : "after="+after)}"));
            return await _client._httpClient.GetFromJsonAsync<Message[]>($"/api/v9/channels/{Id}/messages?amount={amount}&{(after == null ? "before="+before : "after="+after)}");
        }

        public async Task<Message> SendMessage(string content)
        {
            return JsonConvert.DeserializeObject<Message>(await (await _client._httpClient.PostAsJsonAsync($"/api/v9/channels/{Id}/messages", new
            {
                content = content
            })).Content.ReadAsStringAsync());
        }
    }
}