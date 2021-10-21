using FosscordSharp.Core;

namespace FosscordSharp.Entities
{
    public partial class Emoji : FosscordObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}