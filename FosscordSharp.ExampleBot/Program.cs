using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;
using FosscordSharp.EventArgs;

namespace FosscordSharp.ExampleBot
{
    class Program
    {
        private static FosscordClient client;
        static void Main(string[] args)
        {
            Run();
            Thread.Sleep(int.MaxValue);
        }
        static async void Run()
        {
            client = new FosscordClient(new()
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
                },
                PollMessages = true
            });
            await client.Login();
            User botUser = await client.GetCurrentUser();
            Console.WriteLine($"Logged in as {botUser.Username}#{botUser.Discriminator} ({botUser.Id})!");
            client.MessageReceived += ClientOnMessageReceived;
            Guild[] guilds = await client.GetGuilds();
            Console.WriteLine($"I am in {guilds.Length} guilds");
            await client.JoinGuild("qFlTsl");
            guilds = await client.GetGuilds();
            Console.WriteLine($"I am in {guilds.Length} guilds*");
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

        private static async void ClientOnMessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message received: "+e.Message.Content);
            if (e.Message.Content.StartsWith("!"))
            {
                var guild = await client.GetGuild(e.Message.GuildId);
                var channel = await guild.GetChannel(e.Message.ChannelId);
                var _ = e.Message.Content.Split(" ");
                var command = _[0][1..];
                var args = _.Skip(1).ToArray();
                switch (command)
                {
                    case "ping":
                        await channel.SendMessage("pong!");
                        break;
                    default: break;
                }
                
            }
        }
    }
}