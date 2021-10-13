namespace FosscordSharp.Entities
{
    public partial class Reaction
    {
        public Emoji Emoji { get; set; }
        public long Count { get; set; }
        public bool Me { get; set; }
    }
}
