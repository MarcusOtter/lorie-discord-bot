using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Core.UserAccounts;
using DiscordBot.Core;

namespace DiscordBot.Modules
{
    public class Games : ModuleBase<SocketCommandContext>
    {
        [Command("ttt start")]
        [Alias("tictactoe start", "tictac start", "tic start")]
        [Priority(2)]
        public async Task TicTacToeStart()
        {
            await SendEmbeddedMessage("Tic Tac Toe board for USER_1 and USER_2", TicTacToe.DrawBoard());
            //set active game bool in global and store users probably. Then check in commandhandler if these people are in tictactoe game, then write without prefix.
        }

        [Command("ttt setmarker")]
        [Priority(2)]
        public async Task TicTacToeSetMarker([Remainder] string message)
        {
            string marker = message.Replace(" ", "");

            Console.WriteLine(marker);

            if (!TicTacToe.allowedEmojis.Contains(marker))
            {
                await SendEmbeddedMessage("Setting marker failed!", "Please enter a valid marker.");
                return;
            }

            string[] output = SetMarker(Context.User, marker);

            await SendEmbeddedMessage(output[0], output[1]);
        }

        public string[] SetMarker(SocketUser user, string marker)
        {
            var account = UserAccounts.GetAccount(user);
            account.TTTMarker = marker;
            return new string[] { "Marker updated!", $"Marker for {user.Username} was updated to {marker}" };
        }


        [Command("ttt")]
        [Alias("tictactoe", "tictac", "tic")]
        [Priority(1)]
        public async Task TicTacToeCommands([Remainder] string message)
        {
            string tttCommand = message.ToLower();

            //if (tttCommand.Contains("setmarker"))
            //{
            //    string marker = tttCommand.Substring(9).Replace(" ", "");

            //    if (!TicTacToe.allowedEmojis.Contains(marker))
            //    {
            //        await SendEmbeddedMessage("Setting marker failed!", "Please enter a valid marker.");
            //        return;
            //    }

            //    string[] output = TicTacToe.SetMarker(Context.User, marker);

            //    await SendEmbeddedMessage(output[0], output[1]);
            //}

            if (tttCommand.Contains("currentmarker"))
            {
                var account = UserAccounts.GetAccount(Context.User);
                await SendEmbeddedMessage($"Marker for {Context.User.Username} is {account.TTTMarker}", "");

            }
        }

        [Command("ttt")]
        [Alias("tictactoe", "tictac", "tic")]
        public async Task TicTacToeHelp()
        {
            string description = string.Concat("```\n",
                                                "!ttt <@UserToChallenge#1234>\n\n",
                                                "Challenges a user to a game of Tic Tac Toe.\n",
                                                "```\n",
                                                "```\n",
                                                "!ttt SetMarker <character>\n\n",
                                                "Sets your individual tic tac toe marker.\n",
                                                "```");

            await SendEmbeddedMessage("==== Tic Tac Toe commands ==== Exclude <> ====", description);
        }

        private async Task DeleteMessage(IUserMessage msgToDelete)
        {
            var botUser = Context.Guild.GetUser(Context.Client.CurrentUser.Id);

            //If the bot doesn't have permissions to remove the message, return.
            if (!botUser.GuildPermissions.ManageMessages)
            {
                return;
            }

            await msgToDelete.DeleteAsync();
        }

        private async Task DeleteMessages(int i)
        {
            var botUser = Context.Guild.GetUser(Context.Client.CurrentUser.Id);

            //If the bot doesn't have permissions to remove the message, return.
            if (!botUser.GuildPermissions.ManageMessages)
            {
                return;
            }

            var messages = await Context.Channel.GetMessagesAsync(i).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
        }

        private async Task SendEmbeddedMessage(string title, string description)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(description);
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        private async Task SendEmbeddedMessage(string title, string description, string message)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(description);
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync(message, false, embed);
        }
    }
}
