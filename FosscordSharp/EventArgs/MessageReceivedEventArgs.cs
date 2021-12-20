using FosscordSharp.Entities;

namespace FosscordSharp.EventArgs
{
    public class MessageReceivedEventArgs : System.EventArgs
    {
        public FosscordClient Client;
        public Message Message;
    }
}