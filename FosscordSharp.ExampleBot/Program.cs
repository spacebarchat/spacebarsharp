using System;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;

namespace FosscordSharp.ExampleBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
            Thread.Sleep(int.MaxValue);
        }

        static async void Run()
        {
            FosscordClient client = new FosscordClient(new()
            {
                Email = $"FosscordSharp{Environment.TickCount64}@example.com",
                Password = "FosscordSharp",
                Endpoint = "https://dev.fosscord.com",
                Verbose = false,
                ShouldRegister = true,
                RegistrationOptions =
                {
                    Username = "FosscordSharp Example Bot",
                    DateOfBirth = "1969-01-01",
                    CreateBotGuild = true
                }
            });
            await client.Login();
            User botUser = await client.GetCurrentUser();
            Console.WriteLine($"Logged in as {botUser.Username}#{botUser.Discriminator} ({botUser.Id})!");
            Guild[] guilds = await client.GetGuilds();
            Console.WriteLine($"I am in {guilds.Length} guilds");
            foreach (var guild in guilds)
            {
                Console.WriteLine($" - {guild.Name} ({guild.Id})");
                Channel[] channels = await guild.GetChannels();
                foreach (var channel in channels)
                {
                    Console.WriteLine($"   - {channel.Name} ({channel.Id})");
                    try
                    {
                        Message msg = await channel.SendMessage("Hi from the FosscordSharp example bot!");
                        Task.Run(async () =>
                        {
                            Thread.Sleep(3000);
                            await msg.Delete();
                        });
                    }
                    catch
                    {
                        Console.WriteLine($"Could not send message in {guild.Name}#{channel.Name}");
                    }
                }
            }
        }
    }
}