using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Guild
    {
        internal FosscordClient _client;
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("afk_channel_id")]
        public object AfkChannelId { get; set; }

        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; set; }

        [JsonProperty("banner")]
        public object Banner { get; set; }

        [JsonProperty("default_message_notifications")]
        public int DefaultMessageNotifications { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("discovery_splash")]
        public object DiscoverySplash { get; set; }

        [JsonProperty("explicit_content_filter")]
        public int ExplicitContentFilter { get; set; }

        [JsonProperty("features")]
        public List<object> Features { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("large")]
        public object Large { get; set; }

        [JsonProperty("max_members")]
        public int MaxMembers { get; set; }

        [JsonProperty("max_presences")]
        public int MaxPresences { get; set; }

        [JsonProperty("max_video_channel_users")]
        public int MaxVideoChannelUsers { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("presence_count")]
        public int PresenceCount { get; set; }

        [JsonProperty("template_id")]
        public object TemplateId { get; set; }

        [JsonProperty("mfa_level")]
        public int MfaLevel { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty("preferred_locale")]
        public string PreferredLocale { get; set; }

        [JsonProperty("premium_subscription_count")]
        public int PremiumSubscriptionCount { get; set; }

        [JsonProperty("premium_tier")]
        public int PremiumTier { get; set; }

        [JsonProperty("public_updates_channel_id")]
        public object PublicUpdatesChannelId { get; set; }

        [JsonProperty("rules_channel_id")]
        public object RulesChannelId { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("splash")]
        public object Splash { get; set; }

        [JsonProperty("system_channel_id")]
        public object SystemChannelId { get; set; }

        [JsonProperty("system_channel_flags")]
        public int SystemChannelFlags { get; set; }

        [JsonProperty("unavailable")]
        public bool Unavailable { get; set; }

        [JsonProperty("verification_level")]
        public int VerificationLevel { get; set; }

        // [JsonProperty("welcome_screen")]
        // public WelcomeScreen WelcomeScreen { get; set; }

        [JsonProperty("widget_channel_id")]
        public object WidgetChannelId { get; set; }

        [JsonProperty("widget_enabled")]
        public bool WidgetEnabled { get; set; }

        [JsonProperty("nsfw_level")]
        public int NsfwLevel { get; set; }

        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }

        public Guild()
        {
            
        }
        public Guild(FosscordClient client)
        {
            _client = client;
        }

        public ulong id = 0;
        public async Task<Channel[]> GetChannels()
        {
            Util.Log("GetChannels called on guild " + id);
            // await _client._httpClient.GetAsync($"");
            // Util.LogDebug($"/guilds/{id}/channels");
            // Util.LogDebug(await _client._httpClient.GetStringAsync($"/guilds/{id}/channels"));
            var a = await _client._httpClient.GetFromJsonAsync<Channel[]>($"/api/guilds/{id}/channels");
            Util.Log(a.Length+ " channels");
            return a;
        }
    }
}