namespace FosscordSharp.Entities
{
    public partial class Author
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Discriminator { get; set; }
        public long PublicFlags { get; set; }
    }
}