using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DiscordBot
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();

            //Assembly stuff that I didn't have to understand hehe (and I do not)
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        //This is called whenever the bot receives a message in whatever channel or in DM
        private async Task HandleCommandAsync(SocketMessage s)
        {
            //The message
            var msg = s as SocketUserMessage;
            if (msg == null)
            {
                return;
            }
            //Information about which channel it was posted in, etc.
            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            //If the message is prefixed with the prefix in BotConfig or if the bot is mentioned
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                //Executes the command
                var result = await _service.ExecuteAsync(context, argPos);

                //If the command has errors and it is a known command
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    //Write the error reason in console
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
