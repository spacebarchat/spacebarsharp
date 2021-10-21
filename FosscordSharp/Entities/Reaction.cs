using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class Reaction : FosscordObject
    {
        public Emoji Emoji { get; set; }
        public long Count { get; set; }
        public bool Me { get; set; }
    }
}
