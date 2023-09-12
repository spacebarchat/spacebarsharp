using System.Globalization;
using ArcaneLibs.Logging;

namespace FosscordSharp
{
    public class FosscordClientConfig
    {
        public string Endpoint { get; set; } = "";
        public bool ShouldRegister { get; set; } = true;
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public RegistrationOptions RegistrationOptions { get; set; } = new();
        public bool Verbose { get; set; } = false;
        public bool PollMessages { get; set; } = false;
        public bool JoinDmInvites { get; set; } = false;
        public LogManager? LogManager { get; set; } = null;
        public LogManager? DebugLogManager { get; set; } = null;
    }

    public class RegistrationOptions
    {
        public string Username { get; set; } = "Unknown bot";
        public string DateOfBirth { get; set; } = "1970-01-01";
        public bool CreateBotGuild { get; set; } = false;
    }
}