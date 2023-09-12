using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FosscordSharp.Core;
using FosscordSharp.Utilities;
using Newtonsoft.Json;
using OneOf;

namespace FosscordSharp.Entities
{
    public class Channel : FosscordObject
    {
        [JsonProperty("id")] public ulong Id { get; set; }

        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("icon")] public object Icon { get; set; }

        [JsonProperty("type")] public int Type { get; set; }

        [JsonProperty("last_message_id")] public ulong? LastMessageId { get; set; }

        [JsonProperty("guild_id")] public string GuildId { get; set; }

        [JsonProperty("parent_id")] public object ParentId { get; set; }

        [JsonProperty("owner_id")] public object OwnerId { get; set; }

        [JsonProperty("last_pin_timestamp")] public object LastPinTimestamp { get; set; }

        [JsonProperty("default_auto_archive_duration")]
        public object DefaultAutoArchiveDuration { get; set; }

        [JsonProperty("position")] public int Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public List<object> PermissionOverwrites { get; set; }

        [JsonProperty("video_quality_mode")] public object VideoQualityMode { get; set; }

        [JsonProperty("bitrate")] public object Bitrate { get; set; }

        [JsonProperty("user_limit")] public object UserLimit { get; set; }

        [JsonProperty("nsfw")] public object Nsfw { get; set; }

        [JsonProperty("rate_limit_per_user")] public object RateLimitPerUser { get; set; }

        [JsonProperty("topic")] public object Topic { get; set; }

        public async Task<Invite> CreateInvite(int duration = 0, int max_uses = 0, bool temporary_membership = true)
        {
            _client.log.LogDebug($"Creating invite for {Id}/{Name}");
            var resp = await _client.PostJsonAsync<Invite>($"/api/v9/channels/{Id}/invites",
                new { max_age = duration, max_uses = max_uses, temporary = temporary_membership });
            if (resp.IsT1)
            {
                throw new Exception(resp.AsT1.ToString());
            }
            
            return resp.AsT0;
        }

        public async Task<Message[]> GetMessages(ulong? before = null, ulong? after = null, int? amount = 100)
        {
            if (before == null && after == null) before = UInt64.MaxValue;
            _client.log.LogDebug($"Fetching {amount} messages {(after == null ? $"before {before}" : $"after {after}")}");
            var msgs = await _client.GetAsync<Message[]>($"/api/v9/channels/{Id}/messages?amount={amount}&{(after == null ? "before=" + before : "after=" + after)}");
            if (msgs.IsT1)
            {
                throw new Exception(msgs.AsT1.ToString());
            }

            return msgs.AsT0;
        }

        public async Task<Message> SendMessage(string content)
        {
            var resp = await _client.PostJsonAsync<Message>($"/api/v9/channels/{Id}/messages", new
            {
                content = content
            });
            
            if (resp.IsT1)
            {
                Console.WriteLine("");
                throw new Exception(resp.AsT1.ToString());
            }

            return resp.AsT0;
        }
    }
}