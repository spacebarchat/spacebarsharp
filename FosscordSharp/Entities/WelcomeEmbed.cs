using System;

namespace FosscordSharp.Entities
{
    public partial class WelcomeEmbed
    {
        public string Type { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? Color { get; set; }
        public string ReferenceId { get; set; }
        public PurpleProvider Provider { get; set; }
        public Thumbnail Thumbnail { get; set; }
    }
}