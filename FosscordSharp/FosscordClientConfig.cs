using System.Globalization;

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
    }

    public class RegistrationOptions
    {
        public string Username { get; set; } = "Unknown bot";
        public string DateOfBirth { get; set; } = "1970-01-01";
    }
}