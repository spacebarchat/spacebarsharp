using FosscordSharp.Core;

namespace FosscordSharp.ResponseTypes
{
    public class GuildCreatedResponse : FosscordObject
    {
        public ulong id { get; set; } = 0;
    }
}