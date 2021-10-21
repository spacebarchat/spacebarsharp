using System.Collections.Generic;
using FosscordSharp.Core;
using FosscordSharp.ResponseTypes;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class User : FosscordObject
    {
        internal FosscordClient _client;
        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("username")] public string? Username { get; set; }

        [JsonProperty("discriminator")] public string? Discriminator { get; set; }

        [JsonProperty("avatar")] public string? Avatar { get; set; }

        [JsonProperty("accent_color")] public int? AccentColor { get; set; }

        [JsonProperty("banner")] public string? Banner { get; set; }

        [JsonProperty("phone")] public string? Phone { get; set; }

        [JsonProperty("premium")] public bool? Premium { get; set; }

        [JsonProperty("premium_type")] public int? PremiumType { get; set; }

        [JsonProperty("bot")] public bool? Bot { get; set; }

        [JsonProperty("bio")] public string? Bio { get; set; }

        [JsonProperty("nsfw_allowed")] public bool? NsfwAllowed { get; set; }

        [JsonProperty("mfa_enabled")] public bool? MfaEnabled { get; set; }

        [JsonProperty("verified")] public bool? Verified { get; set; }

        [JsonProperty("disabled")] public bool? Disabled { get; set; }

        [JsonProperty("email")] public string? Email { get; set; }

        [JsonProperty("flags")] public string? Flags { get; set; }

        [JsonProperty("public_flags")] public int? PublicFlags { get; set; }

        [JsonProperty("settings")] public Settings? Settings { get; set; }
    }

    public class Settings
    {
        [JsonProperty("afk_timeout")] public int? AfkTimeout;

        [JsonProperty("allow_accessibility_detection")]
        public bool? AllowAccessibilityDetection;

        [JsonProperty("animate_emoji")] public bool? AnimateEmoji;

        [JsonProperty("animate_stickers")] public int? AnimateStickers;

        [JsonProperty("contact_sync_enabled")] public bool? ContactSyncEnabled;

        [JsonProperty("convert_emoticons")] public bool? ConvertEmoticons;

        [JsonProperty("custom_status")] public CustomStatus? CustomStatus;

        [JsonProperty("default_guilds_restricted")]
        public bool? DefaultGuildsRestricted;

        [JsonProperty("detect_platform_accounts")]
        public bool? DetectPlatformAccounts;

        [JsonProperty("developer_mode")] public bool? DeveloperMode;

        [JsonProperty("disable_games_tab")] public bool? DisableGamesTab;

        [JsonProperty("enable_tts_command")] public bool? EnableTtsCommand;

        [JsonProperty("explicit_content_filter")]
        public int? ExplicitContentFilter;

        [JsonProperty("friend_source_flags")] public FriendSourceFlags? FriendSourceFlags;

        [JsonProperty("gateway_connected")] public bool? GatewayConnected;

        [JsonProperty("gif_auto_play")] public bool? GifAutoPlay;

        // [JsonProperty("guild_folders")] public List<object>? GuildFolders;
        //
        // [JsonProperty("guild_positions")] public List<object>? GuildPositions;

        [JsonProperty("inline_attachment_media")]
        public bool? InlineAttachmentMedia;

        [JsonProperty("inline_embed_media")] public bool? InlineEmbedMedia;

        [JsonProperty("locale")] public string? Locale;

        [JsonProperty("message_display_compact")]
        public bool? MessageDisplayCompact;

        [JsonProperty("native_phone_integration_enabled")]
        public bool? NativePhoneIntegrationEnabled;

        [JsonProperty("render_embeds")] public bool? RenderEmbeds;

        [JsonProperty("render_reactions")] public bool? RenderReactions;

        [JsonProperty("restricted_guilds")] public List<object>? RestrictedGuilds;

        [JsonProperty("show_current_game")] public bool? ShowCurrentGame;

        [JsonProperty("status")] public string? Status;

        [JsonProperty("stream_notifications_enabled")]
        public bool? StreamNotificationsEnabled;

        [JsonProperty("theme")] public string? Theme;

        [JsonProperty("timezone_offset")] public int? TimezoneOffset;
    }
}