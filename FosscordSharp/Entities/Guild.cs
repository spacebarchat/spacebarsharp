using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FosscordSharp.Core;
using FosscordSharp.Utilities;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Guild : FosscordObject
    {
        [JsonProperty("id")] public ulong Id { get; set; }

        [JsonProperty("afk_channel_id")] public object AfkChannelId { get; set; }

        [JsonProperty("afk_timeout")] public long AfkTimeout { get; set; }

        [JsonProperty("banner")] public object Banner { get; set; }

        [JsonProperty("default_message_notifications")]
        public long DefaultMessageNotifications { get; set; }

        [JsonProperty("description")] public object Description { get; set; }

        [JsonProperty("discovery_splash")] public object DiscoverySplash { get; set; }

        [JsonProperty("explicit_content_filter")]
        public long ExplicitContentFilter { get; set; }

        [JsonProperty("features")] public List<object> Features { get; set; }

        [JsonProperty("icon")] public object Icon { get; set; }

        [JsonProperty("large")] public object Large { get; set; }

        [JsonProperty("max_members")] public long MaxMembers { get; set; }

        [JsonProperty("max_presences")] public long MaxPresences { get; set; }

        [JsonProperty("max_video_channel_users")]
        public long MaxVideoChannelUsers { get; set; }

        [JsonProperty("member_count")] public long MemberCount { get; set; }

        [JsonProperty("presence_count")] public long PresenceCount { get; set; }

        [JsonProperty("template_id")] public object TemplateId { get; set; }

        [JsonProperty("mfa_level")] public long MfaLevel { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("owner_id")] public string OwnerId { get; set; }

        [JsonProperty("preferred_locale")] public string PreferredLocale { get; set; }

        [JsonProperty("premium_subscription_count")]
        public long PremiumSubscriptionCount { get; set; }

        [JsonProperty("premium_tier")] public long PremiumTier { get; set; }

        [JsonProperty("public_updates_channel_id")]
        public object PublicUpdatesChannelId { get; set; }

        [JsonProperty("rules_channel_id")] public object RulesChannelId { get; set; }

        [JsonProperty("region")] public string Region { get; set; }

        [JsonProperty("splash")] public object Splash { get; set; }

        [JsonProperty("system_channel_id")] public object SystemChannelId { get; set; }

        [JsonProperty("system_channel_flags")] public long SystemChannelFlags { get; set; }

        [JsonProperty("unavailable")] public bool Unavailable { get; set; }

        [JsonProperty("verification_level")] public long VerificationLevel { get; set; }

        // [JsonProperty("welcome_screen")]
        // public WelcomeScreen WelcomeScreen { get; set; }

        [JsonProperty("widget_channel_id")] public object WidgetChannelId { get; set; }

        [JsonProperty("widget_enabled")] public bool WidgetEnabled { get; set; }

        [JsonProperty("nsfw_level")] public long NsfwLevel { get; set; }

        [JsonProperty("nsfw")] public bool Nsfw { get; set; }

        [JsonConstructor]
        public Guild()
        {
        }

        public Guild(FosscordClient client)
        {
            _client = client;
        }

        public async Task<Channel[]> GetChannels()
        {
            var res = await _client.GetAsync<Channel[]>($"/api/guilds/{Id}/channels");
            if (res.IsT1)
            {
                throw new Exception(res.AsT1.ToString());
            }

            return res.AsT0;
        }

        public async Task<Channel> GetChannel(ulong id)
        {
            var channels = await GetChannels();
            if (!channels.Any(x => x.Id == id)) throw new NullReferenceException("Channel doesn't exist!");
            return channels.First(x => x.Id == id);
        }

        public async Task<Channel> CreateChannel(string name, int type = 0, ulong parent = 0)
        {
            if (type == 4 && parent != 0)
            {
                throw new ArgumentException("You cannot create categories in categories!");
            }
            var resp = await _client.PostJsonAsync<Channel>($"/api/v9/guilds/{Id}/channels", new
            {
                name = name,
                type = type,
                topic = "",
                rate_limit_per_user = 0,
                position = int.MaxValue,
                nsfw = false
            });
            if (resp.IsT1)
            {
                throw new Exception(resp.AsT1.ToString());
            }

            return resp.AsT0;
        }
    }

    public class GuildTemp
    {
        [JsonProperty("id")] public ulong Id { get; set; }
    }
}