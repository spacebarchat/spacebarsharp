using System;
using FosscordSharp.Core;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Invite : FosscordObject
    {
        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("temporary")] public bool Temporary { get; set; }

        [JsonProperty("uses")] public int Uses { get; set; }

        [JsonProperty("max_uses")] public int MaxUses { get; set; }

        [JsonProperty("max_age")] public int MaxAge { get; set; }

        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }

        [JsonProperty("expires_at")] public DateTime ExpiresAt { get; set; }

        [JsonProperty("guild_id")] public ulong GuildId { get; set; }

        [JsonProperty("channel_id")] public ulong ChannelId { get; set; }

        [JsonProperty("inviter_id")] public ulong InviterId { get; set; }

        //
        // [JsonProperty("target_user_id")]
        // public object TargetUserId { get; set; }
        //
        // [JsonProperty("target_user_type")]
        // public object TargetUserType { get; set; }
        //
        // [JsonProperty("vanity_url")]
        // public object VanityUrl { get; set; }
        public string FullUrl => _client._httpClient.BaseAddress + "invite/" + Code;
    }
}