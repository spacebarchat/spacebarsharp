using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;
using FosscordSharp.EventArgs;
using Microsoft.VisualBasic;

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
                // Email = $"FosscordSharp{Environment.TickCount64}@example.com",
                Email = $"FosscordSharpDev@example.com",
                Password = "FosscordSharp",
                Endpoint = "https://dev.fosscord.com",
                // Endpoint = "https://fosscord.thearcanebrony.net",
                Verbose = false,
                ShouldRegister = true,
                RegistrationOptions =
                {
                    Username = "FosscordSharp Example Bot",
                    DateOfBirth = "1969-01-01",
                    CreateBotGuild = true
                },
                PollMessages = false
            });
            await client.Login();
            User botUser = await client.GetCurrentUser();
            Console.WriteLine($"Logged in as {botUser.Username}#{botUser.Discriminator} ({botUser.Id})!");
            client.MessageReceived += ClientOnMessageReceived;
            Guild[] guilds = await client.GetGuilds();
            Console.WriteLine($"I am in {guilds.Length} guilds");
            try
            {
             
                await client.JoinGuild("qFlTsl");   
            } catch { }
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
                    case "help":
                        await channel.SendMessage("Fosscord Example Bot command list:\n" +
                                                  "```diff\n" +
                                                  " - ping: Pong!\n" +
                                                  " - avatar: Get avatar url.\n" +
                                                  " - guildlist: Get guild + channel list.\n" +
                                                  "```");
                        break;
                    case "ping":
                        await channel.SendMessage("pong!");
                        break;
                    case "avatar":
                        await channel.SendMessage($"Your avatar url: {e.Message.Author.AvatarUrl}");
                        break;
                    case "guildlist":
                        await channel.SendMessage("Guild list:");
                        foreach (var g in await client.GetGuilds())
                        {
                            await channel.SendMessage(g.Name);
                            foreach (var c in await g.GetChannels())
                            {
                                await channel.SendMessage($" - {c.Name}: {(await c.CreateInvite()).FullUrl}");
                            }
                        }
                        break;
                    default: break;
                }
                
            }
        }
    }
}