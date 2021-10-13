using System;

namespace FosscordSharp.Entities
{
    public partial class Thumbnail
    {
        public Uri Url { get; set; }
        public Uri ProxyUrl { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
    }
}