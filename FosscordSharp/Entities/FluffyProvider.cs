using System;
using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class FluffyProvider : FosscordObject
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
    }
}