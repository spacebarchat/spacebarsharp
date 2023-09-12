using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FosscordSharp.Entities;

namespace FosscordSharp.Services
{
    internal class MessagePoller
    {
        internal FosscordClient _client;

        public MessagePoller(FosscordClient client)
        {
            _client = client;
        }

        internal Dictionary<string, ulong> LastCheckedMessages = new();
        internal void Start()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Guild[] guilds = await _client.GetGuilds();
                    foreach (var guild in guilds)
                    {
                        Channel[] channels = await guild.GetChannels();
                        foreach (var channel in channels)
                        {
                            string key = $"{guild.Id}/{channel.Id}";
                            if (!LastCheckedMessages.ContainsKey(key))
                                LastCheckedMessages.Add(key, channel.LastMessageId??0);
                            foreach (var message in await channel.GetMessages(after: LastCheckedMessages[key]))
                            {
                                _client.OnMessageReceived(new()
                                {
                                    Message = message
                                });
                                LastCheckedMessages[key] = Math.Max(message.Id, LastCheckedMessages[key]);
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
        }
    }
}