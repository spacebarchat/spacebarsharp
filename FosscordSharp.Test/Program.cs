using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace FosscordSharp.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            run();
            
            Thread.Sleep(int.MaxValue);
        }

        static async Task run()
        {
            Random rnd = new Random();
            // FosscordClient client = new(new()
            // {
            //     Email = $"FosscordSharp{rnd.Next()}@test.bot",
            //     Password = "SomePassword",
            //     Endpoint = "https://fosscord.thearcanebrony.net",
            //     RegistrationOptions =
            //     {
            //         Username = "FosscordSharp Test Bot"
            //     }
            // });
            // await client.Login();

            FosscordClient fc = new FosscordClient(new()
            {
                Email = "FosscordSharp@test.bot",
                // Password = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"/.ssh/id_rsa.pub"),
                Password = "SomePassword",
                Endpoint = "https://fosscord.thearcanebrony.net",
                // Endpoint = "http://localhost:3001",
                RegistrationOptions =
                {
                    Username = "FosscordSharp Example Bot",
                    DateOfBirth = "1969-01-01"
                },
                Verbose = false
            });
            await fc.Login();
            var gs = await fc.GetGuilds();
            Util.Log("Got guilds!");
            foreach (var g in gs)
            {
                Util.Log($"{g.Name}: ");
                var cs = await g.GetChannels();
                foreach (var c in await g.GetChannels())
                {
                    Util.Log($"- {c.Name}: {(await c.CreateInvite()).FullUrl} ({(await c.GetMessages()).Length} messages)");
                    Util.Log((await c.SendMessage("Hi from FosscordSharp!")).Id);
                }
            }
            Util.Log(fc.GetGuilds().Result.Length + " guilds");
        }
    }
}