using System;
using System.Threading;
using System.Threading.Tasks;

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
            FosscordClient client = new(new()
            {
                Email = $"FosscordSharp{rnd.Next()}@test.bot",
                Password = "SomePassword",
                Endpoint = "https://fosscord.thearcanebrony.net",
                RegistrationOptions =
                {
                    Username = "FosscordSharp Test Bot"
                }
            });
            // await client.Login();

            FosscordClient fc = new FosscordClient(new()
            {
                Email = "FosscordSharp@test.bot",
                Password = "SomePassword",
                Endpoint = "https://fosscord.thearcanebrony.net"
            });
            await fc.Login();
            var gs = await fc.GetGuilds();
            Util.Log("Got guilds!");
            foreach (var g in gs)
            {
                Util.Log($"{g.Name}: ");
                foreach (var c in await g.GetChannels())
                {
                    Util.Log($"- {c.Name}");
                    
                }
            }
            Util.Log(client.GetGuilds().Result.Length + " guilds");
        }
    }
}