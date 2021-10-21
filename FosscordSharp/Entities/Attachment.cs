using System;
using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class Attachment : FosscordObject
    {
        public string Id { get; set; }
        public string Filename { get; set; }
        public long Size { get; set; }
        public Uri Url { get; set; }
        public Uri ProxyUrl { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
        public string ContentType { get; set; }
    }
}