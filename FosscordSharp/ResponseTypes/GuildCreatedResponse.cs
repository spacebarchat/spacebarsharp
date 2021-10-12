namespace FosscordSharp.ResponseTypes
{
    public class GuildCreatedResponse
    {
        public string id { get; set; } = "";
        public ulong Id => ulong.Parse(id);
    }
}