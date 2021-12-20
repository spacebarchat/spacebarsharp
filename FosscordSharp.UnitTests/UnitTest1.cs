using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FosscordSharp.UnitTests
{
    public class UnitTest1
    {
        private static Random rnd = new();
        int id = rnd.Next(int.MaxValue);
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        public async Task<FosscordClient> GetClient()
        {
            return new FosscordClient(new()
            {
                Endpoint = "https://fosscord.thearcanebrony.net",
                Email = $"fosscordsharp.{id}@fosscord.bot",
                Password = "fosscordsharp",
                Verbose = false,
                RegistrationOptions = new()
                {
                    Username = "F#Bot" + id,
                    DateOfBirth = "2000-11-11"
                }
            });
        }

        [Fact]
        public async Task RegisterAndLogin()
        {
            (await GetClient()).Login();
        }

        [Fact]
        public async Task FetchGuilds()
        {
            FosscordClient cli = await GetClient();
            await cli.Login();
            var guilds = await cli.GetGuilds();
            output.WriteLine(guilds.Length+"");
            foreach (var guild in guilds)
            {
                output.WriteLine(guild.Id + "");
                Assert.Equal((await cli.GetGuild(guild.Id)).Id, guild.Id);
            }
        }
    }
}