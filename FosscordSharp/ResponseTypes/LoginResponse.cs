using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FosscordSharp.ResponseTypes
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class CustomStatus
    {
    }

    public class FriendSourceFlags
    {
        [JsonProperty("all")]
        public bool All;
    }

    public class Settings
    {
        [JsonProperty("afk_timeout")]
        public int AfkTimeout;

        [JsonProperty("allow_accessibility_detection")]
        public bool AllowAccessibilityDetection;

        [JsonProperty("animate_emoji")]
        public bool AnimateEmoji;

        [JsonProperty("animate_stickers")]
        public int AnimateStickers;

        [JsonProperty("contact_sync_enabled")]
        public bool ContactSyncEnabled;

        [JsonProperty("convert_emoticons")]
        public bool ConvertEmoticons;

        [JsonProperty("custom_status")]
        public CustomStatus CustomStatus;

        [JsonProperty("default_guilds_restricted")]
        public bool DefaultGuildsRestricted;

        [JsonProperty("detect_platform_accounts")]
        public bool DetectPlatformAccounts;

        [JsonProperty("developer_mode")]
        public bool DeveloperMode;

        [JsonProperty("disable_games_tab")]
        public bool DisableGamesTab;

        [JsonProperty("enable_tts_command")]
        public bool EnableTtsCommand;

        [JsonProperty("explicit_content_filter")]
        public int ExplicitContentFilter;

        [JsonProperty("friend_source_flags")]
        public FriendSourceFlags FriendSourceFlags;

        [JsonProperty("gateway_connected")]
        public bool GatewayConnected;

        [JsonProperty("gif_auto_play")]
        public bool GifAutoPlay;

        [JsonProperty("guild_folders")]
        public List<object> GuildFolders;

        [JsonProperty("guild_positions")]
        public List<object> GuildPositions;

        [JsonProperty("inline_attachment_media")]
        public bool InlineAttachmentMedia;

        [JsonProperty("inline_embed_media")]
        public bool InlineEmbedMedia;

        [JsonProperty("locale")]
        public string Locale;

        [JsonProperty("message_display_compact")]
        public bool MessageDisplayCompact;

        [JsonProperty("native_phone_integration_enabled")]
        public bool NativePhoneIntegrationEnabled;

        [JsonProperty("render_embeds")]
        public bool RenderEmbeds;

        [JsonProperty("render_reactions")]
        public bool RenderReactions;

        [JsonProperty("restricted_guilds")]
        public List<object> RestrictedGuilds;

        [JsonProperty("show_current_game")]
        public bool ShowCurrentGame;

        [JsonProperty("status")]
        public string Status;

        [JsonProperty("stream_notifications_enabled")]
        public bool StreamNotificationsEnabled;

        [JsonProperty("theme")]
        public string Theme;

        [JsonProperty("timezone_offset")]
        public int TimezoneOffset;
    }

    public class LoginResponse
    {
        [JsonProperty("token")]
        public string Token;

        [JsonProperty("settings")]
        public Settings Settings;
    }


}