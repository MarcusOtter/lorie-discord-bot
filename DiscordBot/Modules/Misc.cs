using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("rules")]
        public async Task rules()
        {
            
            var embed = new EmbedBuilder();
            embed.WithTitle("The rules for the SPE16 server");

            embed.WithDescription(":hammer:  <:Tanta:370508620464521226> Dont spam " + " \n "
                                + ":hammer: <:Tanta:370508620464521226> Dont overuse memes in main " + " \n "
                                + ":hammer: <:Tanta:370508620464521226> Dont copy paste school things " + " \n "
                                + ":hammer: <:Tanta:370508620464521226> Dont be an asshole ");

            embed.WithColor(new Color(255, 165, 0));
            
            await Context.Channel.SendMessageAsync("", false, embed);

            //Find the emote and write out it's ID
            //Console.WriteLine(Context.Guild.Emotes.ElementAt(34).ToString());
        }


        [Command("request")]
        //Syntax: !request commandName , description
        public async Task request([Remainder]string message)
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }

            string[] separatedInput = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var embed = new EmbedBuilder();
            embed.WithTitle("Requested command by " + Context.User.Username + ": " + "'" + separatedInput[0] + "'");
            embed.WithDescription(separatedInput[0] + ": " + separatedInput[1]);
            embed.WithColor(new Color(255, 165, 0));
            await DeleteMessages(1);
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("cute")]
        public async Task cute()
        {
            var embed = new EmbedBuilder();
            embed.WithTitle("Cute commands");
            embed.WithDescription
                                ("\n"
                                + "**!Cute_R** \n"
                                + " *Sends a random cute animal.* \n"
                                + "\n"
                                + "**!Cute_Count** \n"
                                + " *Tells you how many cute animals are in the list.* \n"
                                + "\n"
                                + "**!Cute_Display `numberToDisplay`** \n"
                                + " *Displays any cute animal from anywhere in the list.* \n"
                                + "\n"
                                + "**!Cute_Add `http://cuteanimallinkgoeshere.com`** \n" 
                                + " *Adds another cute animal to the list of cute animals.* \n"
                                + "\n"
                                + "**!Cute_Remove `numberToRemove`** \n"
                                + " *Removes a cute animal from anywhere in the list :(* \n"
                                + "\n"
                                + "If there is something you'd like to see added to this command, talk to <@199969241531678720>"
                                );
            //So this needs to be formatted xd
            Console.WriteLine(Context.User.Mention.ToString());
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
        }


        [Command("test")]
        public async Task test()
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            if (UserIsBotBanned((SocketGuildUser)Context.User))
            {
                await Context.Channel.SendMessageAsync("Sorry " + Context.User.Mention + ", you do not have access to that command.");
                return;
            } 
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("TEST"));
            //await Context.Channel.SendMessageAsync(Utilities.GetAlert("TEST"));
        }

        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            var embed = new EmbedBuilder();
            embed.WithTitle(Utilities.GetFormattedAlert("MESSAGEBY_&USER",Context.User.Username));
            embed.WithDescription(message);
            embed.WithColor(new Color(255, 165, 0));
            //this may be problematic if a user posts the same time as someone posts !echo
            //possibly overload it to a message instead of int
            await DeleteMessages(1);
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("randomBetween")]
        public async Task RandomBetween([Remainder]string message)
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            string[] numbers = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (numbers.Length != 2)
            {
                //replace incorrect message with a alert.json
                await Context.Channel.SendMessageAsync("Incorrect commnad usage." + "\n" + "Command syntax: !randomBetween `minValue` `,` `maxValue`");
                return;
            }
            int lowest;
            int highest;
            try
            {
                 lowest = Int32.Parse(numbers[0]);
                 highest = Int32.Parse(numbers[1]);

            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                await Context.Channel.SendMessageAsync("Incorrect commnad usage." + "\n" + "Command syntax: !randomBetween `minValue` `,` `maxValue`");
                return;
            }
            //int lowest = Convert.ToInt32(numbers[0]);
            //int highest = Convert.ToInt32(numbers[1]);
            Random r = new Random();

            int randomNum = r.Next(lowest, highest + 1);


            var embed = new EmbedBuilder();
            embed.WithTitle(Utilities.GetFormattedAlert("RANDOMNUM_&MIN_&MAX", numbers[0], numbers[1]));
            embed.WithDescription(randomNum.ToString());
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("cheeky")]
        public async Task Cheeky()
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            var embed = new EmbedBuilder();
            embed.WithTitle(";)");
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("pick")]
        public async Task PickOne([Remainder]string message)
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            string[] options = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            int i = r.Next(0, options.Length);
            string selection = options[i];
            

            var embed = new EmbedBuilder();
            embed.WithTitle("I pick");
            embed.WithDescription("**" + selection + "**");
            embed.WithColor(new Color(255, 165, 0));
            embed.WithThumbnailUrl("https://cdn.betterttv.net/emote/56e9f494fff3cc5c35e5287e/1x");
            await Context.Channel.SendMessageAsync("", false, embed);
        }

        [Command("removeMessages")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveMessages([Remainder]string message)
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            int amount;

            try
            {
                amount = Int32.Parse(message);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
                await Context.Channel.SendMessageAsync("Incorrect commnad usage." + "\n" + "Command syntax: !removeMessages `amount`");
                return;
            }

            Console.WriteLine(message);
            await DeleteMessages(amount);
        }

        private bool UserIsBotBanned(SocketGuildUser user)
        {
            //All roles = user.Guild.Roles

            //this should be called in start or whatever
            //replace from here
            string targetRoleName = "Bot Banned";

            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;
            //to here with something better

            ulong roleID = result.FirstOrDefault();
            //The role wasn't found
            if (roleID == 0)
            {
                Console.WriteLine("The -Bot Banned- could not be found.");
                return true;
            }
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
            
        }

        private async Task DeleteMessages(int i)
        {
            var messages = await Context.Channel.GetMessagesAsync(i).Flatten();
            await Context.Channel.DeleteMessagesAsync(messages);
        }
    }
}
