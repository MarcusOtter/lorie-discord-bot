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
        [Command("ttt clearall")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ClearAll()
        {
            //This should be a function in TicTacToeProvider.
            TicTacToeProvider.player1 = null;
            TicTacToeProvider.player2 = null;
            TicTacToe.ResetGame();
            await SendEmbeddedMessage("Games cleared", "An admin has cleared all of the tic tac toe games.");
        }

        [Command("ttt join")]
        public async Task TicTacToeStart()
        {
            string resultString = TicTacToeProvider.AttemptPlayerJoin((SocketGuildUser)Context.User);
            string title = "Error";
            string description = "Something is wrong in the code..";

            if (resultString == TicTacToeProvider.sucPlayer2Joined)
            {
                title = "Success!";
                description = $"{Context.User.Mention} is now player 2!";
                await SendEmbeddedMessage(title, description);
                await SendEmbeddedMessage("", TicTacToeProvider.StartGame());
                return;
            }

            if (resultString == TicTacToeProvider.errGameInProgress)
            {
                title = "Failed to join game";
                description = "Please wait for the current game to finish before starting a new one.";
            }

            if (resultString == TicTacToeProvider.errUserAlreadyPlaying)
            {
                title = "Failed to join game";
                description = "You've already joined the game. The game will start when another player joins.";
            }

            if (resultString == TicTacToeProvider.sucPlayer1Joined)
            {
                title = "Success!";
                description = $"{Context.User.Mention} is now player 1!";
            }

            await SendEmbeddedMessage(title, description);
        }

        [Command("ttt setmarker")]
        public async Task TicTacToeSetMarker([Remainder] string message)
        {
            string marker = message.Replace(" ", "");

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

        [Command("ttt currentmarker")]
        public async Task TicTacToeGetCurrentmarker()
        {
            var account = UserAccounts.GetAccount(Context.User);
            await SendEmbeddedMessage($"The current marker for {Context.User.Username} is {account.TTTMarker}", "");
        }

        [Command("ttt")]
        public async Task TicTacToeHelp()
        {
                                          //_____________OUTDATED_____________//
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
