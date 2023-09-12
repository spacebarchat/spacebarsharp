using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class MessageReference : FosscordObject
    {
        public string ChannelId { get; set; }
        public string GuildId { get; set; }
        public string MessageId { get; set; }
    }
}