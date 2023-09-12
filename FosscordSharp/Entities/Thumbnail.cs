using System;
using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class Thumbnail : FosscordObject
    {
        public Uri Url { get; set; }
        public Uri ProxyUrl { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
    }
}