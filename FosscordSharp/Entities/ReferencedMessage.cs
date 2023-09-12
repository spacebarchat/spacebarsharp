using System;
using System.Collections.Generic;
using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class ReferencedMessage : FosscordObject
    {
        public string Id { get; set; }
        public long Type { get; set; }
        public string Content { get; set; }
        public string ChannelId { get; set; }
        public Author Author { get; set; }
        public List<Attachment> Attachments { get; set; }
        // public List<ReferencedMessageEmbed> Embeds { get; set; }
        public List<Author> Mentions { get; set; }
        public List<dynamic> MentionRoles { get; set; }
        public bool Pinned { get; set; }
        public bool MentionEveryone { get; set; }
        public bool Tts { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public dynamic EditedTimestamp { get; set; }
        public long Flags { get; set; }
        public List<dynamic> Components { get; set; }
        public MessageReference MessageReference { get; set; }
    }
}