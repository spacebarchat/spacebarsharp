using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FosscordSharp.Entities
{
    public class Message
    {
        internal FosscordClient _client;

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("channel_id")] public string ChannelId { get; set; }

        [JsonProperty("guild_id")] public string GuildId { get; set; }

        [JsonProperty("author_id")] public string AuthorId { get; set; }

        [JsonProperty("member_id")] public object MemberId { get; set; }

        [JsonProperty("webhook_id")] public object WebhookId { get; set; }

        [JsonProperty("application_id")] public object ApplicationId { get; set; }

        [JsonProperty("content")] public string Content { get; set; }

        [JsonProperty("timestamp")] public DateTime Timestamp { get; set; }

        [JsonProperty("edited_timestamp")] public object EditedTimestamp { get; set; }

        [JsonProperty("tts")] public object Tts { get; set; }

        [JsonProperty("mention_everyone")] public bool MentionEveryone { get; set; }

        [JsonProperty("embeds")] public List<object> Embeds { get; set; }

        [JsonProperty("reactions")] public List<object> Reactions { get; set; }

        [JsonProperty("nonce")] public object Nonce { get; set; }

        [JsonProperty("pinned")] public bool Pinned { get; set; }

        [JsonProperty("type")] public int Type { get; set; }

        [JsonProperty("activity")] public object Activity { get; set; }

        [JsonProperty("flags")] public object Flags { get; set; }

        [JsonProperty("message_reference")] public object MessageReference { get; set; }

        [JsonProperty("interaction")] public object Interaction { get; set; }

        [JsonProperty("components")] public object Components { get; set; }

        [JsonProperty("author")] public Author Author { get; set; }

        [JsonProperty("mentions")] public List<object> Mentions { get; set; }

        [JsonProperty("mention_roles")] public List<object> MentionRoles { get; set; }

        [JsonProperty("mention_channels")] public List<object> MentionChannels { get; set; }

        [JsonProperty("attachments")] public List<object> Attachments { get; set; }
    }
}