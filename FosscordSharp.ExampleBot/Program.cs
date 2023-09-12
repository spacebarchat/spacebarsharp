using System;
using System.Collections.Generic;
using System.IO;
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
        private static Dictionary<string, FosscordClient> clients = new();
        static void Main(string[] args)
        {
            if(!File.Exists("instances.txt")) File.WriteAllText("instances.txt", "https://dev.fosscord.com\nhttps://fosscord.thearcanebrony.net\n");
            foreach (var inst in File.ReadAllLines("instances.txt"))
            {
                Run(inst);
            }
            Thread.Sleep(int.MaxValue);
        }

        private static Random rnd = new Random();
        static async void Run(string endpoint)
        {
            FosscordClient client;
            clients.Add(endpoint, client = new FosscordClient(new()
            {
                // Email = $"FosscordSharp{Environment.TickCount64}@example.com",
                Email = $"FosscordSharpDev@example.com",
                Password = "FosscordSharp",
                // Endpoint = "https://dev.fosscord.com",
                // Endpoint = "https://fosscord.thearcanebrony.net",
                Endpoint = endpoint,
                Verbose = false,
                ShouldRegister = true,
                RegistrationOptions =
                {
                    Username = "FosscordSharp Example Bot",
                    DateOfBirth = "1969-01-01",
                    CreateBotGuild = true
                },
                PollMessages = true
            }));
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
                    new Thread(() => {
                        while (true) {
                            channel.SendMessage("hi hello");
                            Thread.Sleep(rnd.Next(2000));
                        }
                    }).Start();
                }
            }
        }

        private static async void ClientOnMessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message received: "+e.Message.Content);
            if (e.Message.Content.StartsWith("!"))
            {
                var guild = await e.Client.GetGuild(e.Message.GuildId);
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
                                                  " - botinfo: show info about the bot and its environment.\n" +
                                                  " - joinguild: join guild by invite on current instance.\n" +
                                                  " - joininstance: join instance.\n" +
                                                  "```");
                        break;
                    case "ping":
                        await channel.SendMessage("pong!");
                        break;
                    case "avatar":
                        await channel.SendMessage($"Your avatar url: {e.Message.Author.AvatarUrl}");
                        break;
                    case "guildlist":
                        string msg = "Guild list:\n";
                        foreach (var gui in await e.Client.GetGuilds())
                        {
                            msg += $"- {gui.Name}\n";
                            foreach (var c in await gui.GetChannels())
                            {
                                msg += $"  - {c.Name}: {(c.Type == 0 ? (await c.CreateInvite()).FullUrl : "<no invite, not a text channel>")}\n";
                            }
                        }

                        await channel.SendMessage(msg);
                        break;
                    case "globaltree":
                        foreach (var client in clients)
                        {
                            string globaltree = "Guild list:\n";
                            foreach (var gu in await client.Value.GetGuilds())
                            {
                                globaltree += $"- {gu.Name}\n";
                                foreach (var c in await gu.GetChannels())
                                {
                                    globaltree += $"  - {c.Name}: {(c.Type == 0 ? (await c.CreateInvite()).FullUrl : "<no invite, not a text channel>")}\n";
                                }
                            }

                            await channel.SendMessage(globaltree);
                        }
                        
                        break;
                    case "botinfo":
                        await channel.SendMessage($"Bot info:\n" +
                                                  $".NET version: {Environment.Version}\n" +
                                                  $"FosscordSharp version: {RuntimeInfo.LibVersion}\n" +
                                                  $"Guild count: {(await e.Client.GetGuilds()).Length} (instance), {clients.Sum(x=>(x.Value.GetGuilds().Result.Length))} (bot)\n" +
                                                  $"Instance count: {clients.Count}");
                        break;
                    case "joinguild":
                        if (args.Length != 1)
                        {
                            await channel.SendMessage("This command takes one argument!");
                            break;
                        }
                        await e.Client.JoinGuild(args[0].Split("/").Last());
                        break;
                    case "joininstance":
                        if (args.Length != 1)
                        {
                            await channel.SendMessage("This command takes one argument!");
                            break;
                        }
                        File.AppendAllText("instances.txt", args[0]+"\n");
                        Run(args[0]);
                        break;
                    case "testguild":
                        var g = await e.Client.CreateGuild("Test Guild - Delete me");
                        await channel.SendMessage((await (await g.GetChannels())[0].CreateInvite()).Code);
                        List<Channel> categories = new();
                        for (int i = 0; i < 4; i++)
                        {
                            categories.Add(await g.CreateChannel("Category "+i));
                        }

                        foreach (var cat in categories)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                g.CreateChannel($"channel" + i, parent: cat.Id);
                            }
                        }
                        break;
                    default: break;
                }
                
            }
        }
    }
}