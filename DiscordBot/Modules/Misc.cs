using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;
using Newtonsoft.Json;

namespace DiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("RareParrot")]
        public async Task Parrot()
        {
            await DeleteMessages(1);
            await Context.Channel.SendMessageAsync("<a:RareParrot:394551061144403968>");
        }
        
         [Command("cs")]
         public async Task CSharpChallenge([Remainder]string str)
         {
             //await SendEmbeddedMessage("new color", Math.GetRGBFromHEX(str).Red.ToString());

             //await SendEmbeddedMessage(Calculate(first, operatorSign, second).ToString(), "");

             //string outputString = Calculate().ToString();
         }
        
        [Command("lit")]
        public async Task Lit()
        {
            string title = ":ok_hand: :point_left: :tired_face: :100: :fire: :sweat_drops:";
            string description = "";
            await DeleteMessages(1);
            await SendEmbeddedMessage(title, description, "", "", $"By {Context.User.Username}");
        }

        [Command("help")]
        [Alias("commands")]
        public async Task Help()
        {
        string title =  $"All {Context.Guild.CurrentUser.Nickname} commands - Page 1 :book:";
        string description =    "**=================================**\n" +
                                "!help  **|**  *Displays all the commands (this command).*\n" +
                                "!rules  **|**  *Shows the rules for the server.*\n" +
                                "!request  **|**  *Sends a command request to <@199969241531678720>.*\n" +
                                "!lit  **|**  *It's lit*\n" +
                                "!echo  **|**  *The bot repeats your message.*\n" +
                                "!randomBetween  **|**  *Picks a random number between the provided numbers.*\n" +
                                "!pick  **|**  *Picks one of the provided options at random.*\n" +
                                "!removeMessages  **|**  *Deletes a number of the latest messages in this channel.*\n" +
                                "!cheeky  **|**  *;)*\n" +
                                "!rareParrot  **|**  *Sends a very rare parrot*\n" +
                                "!playing  **|**  *Sets the game that Lorie is playing.*\n" +
                                "!cute count  **|**  *Shows the amount of animals in the list.*\n" +
                                "!cute add  **|**  *Adds a cute animal to the list :)*\n" +
                                "!cute nr  **|**  *Displays a specific number from the list of cute animals.*\n" +
                                "!cute r  **|**  *Displays a random cute animal.*\n" +
                                "!cute remove  **|**  *Removes a cute animal from the list :(*\n"
                                ;

            await SendEmbeddedMessage(title, description);
        }

        [Command("rules")]
        public async Task Rules([Remainder]string inputString = "")
        {
            string title = $"The rules of the {Context.Guild.Name} server";
            string description = "";

            List<string> rules = new List<string>
            {
                ":hammer:<:Tanta:370508620464521226> Don't spam",
                ":hammer:<:Tanta:370508620464521226> Don't overuse memes in main",
                ":hammer:<:Tanta:370508620464521226> Don't copy paste school things",
                ":hammer:<:Tanta:370508620464521226> Don't be an asshole",
                ":hammer:<:Tanta:370508620464521226> Don't be racist",
                ":hammer:<:Tanta:370508620464521226> Don't be sexist"
            };

            if (string.IsNullOrWhiteSpace(inputString))
            {
                for (int i = 0; i < rules.Count; i++)
                {
                    description += rules[i];
                    description += "\n";
                }
            }
            else
            {
                for (int i = 0; i < rules.Count; i++)
                {
                    //Is this the rule that the user passed in?
                    if ((i +1).ToString() == inputString)
                    {
                        //Makes the rule in bold and adds a warning
                        rules[i] = $"**{rules[i]} :warning:**";
                    }
                }

                for (int i = 0; i < rules.Count; i++)
                {
                    description += rules[i];
                    description += "\n";
                }
            }

            await SendEmbeddedMessage(title, description);
        }

        [Command("request")]
        public async Task Request([Remainder]string message = "")
        {
            string title = "";
            string description = "";

            if (string.IsNullOrWhiteSpace(message))
            {
                title = "";
                //Description should be stored somewhere
                description =   "```" +
                                "!request <!CommandName> , <Description>\n\n" +
                                "Request a command for Lorie.\n\n" +
                                "Example: !request !randomSong , This command would send random songs." +
                                "```";
                await SendEmbeddedMessage(title, description);

            }

            string[] separatedInput = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var embed = new EmbedBuilder();

            title = $"Requested command by {Context.User.Username}: '{separatedInput[0]}'";
            description = $"{separatedInput[0]}: {separatedInput[1]}";

            await DeleteMessages(1);
            await SendEmbeddedMessage(title, description, "<@199969241531678720>");
        }

       

        [Command("cute add")]
        public async Task CuteAdd([Remainder]string url)
        {
            //If the input has too few parameters, print what it does. Should be a thing for every command. 

            //If the user is not a !cute Moderator
            if (!UserHasRole((SocketGuildUser)Context.User, "!cute Moderator"))
            {
                string title1 = "Operation failed";
                string description1 = $"Sorry {Context.User.Username}, you need the \"!cute Moderator\" role to use this command.";
                await SendEmbeddedMessage(title1, description1);
                return;
            }

            //If this is not a valid URL
            if (!Utilities.ValidateURL(url))
            {
                string title1 = "Operation failed";
                string description1 = "That is not a valid URL.";
                await SendEmbeddedMessage(title1, description1);
                return;
            }

            //Animal already exists in the .json file.
            if (DataStorage.URLAlreadyExists(url))
            {
                string title1 = "Operation failed";
                string description1 = "Your cute friend(s) already exists in the list.";
                await SendEmbeddedMessage(title1, description1);
                return;
            }

            DataStorage.AddAnimal(url);
            string title = "Success!";
            string description = "Your cute friend(s) was added to the list! :purple_heart:";
            await SendEmbeddedMessage(title, description);
        }

        //TODO: Post this whenever someone posts :( or "sad" or something
        [Command("cute r")]
        public async Task CuteRandom()
        {
            int max = DataStorage.GetAnimalsCount();

            Random random = new Random();
            int randomNum = random.Next(0, max);

            await Context.Channel.SendMessageAsync
            (
                $" {Context.User.Mention} requested a random cute image/video! This is animal #{randomNum}.\n" +
                $"{DataStorage.GetAnimalURL(randomNum)}"
            );
        }

        //throw errors
        [Command("cute nr")]
        public async Task CuteAt(int i)
        {
            int nr = i;
            if (i > (DataStorage.GetAnimalsCount() - 1))
            {
                await SendEmbeddedMessage("Error!", "That animal doesn't exist yet.\nRemember that the list is 0-index based, meaning that the latest animal is !cute_count **-1**");
            }

            await Context.Channel.SendMessageAsync($"{Context.User.Mention} picked cute animal #{i} \n {DataStorage.GetAnimalURL(nr)}");
        }

        //throw errors
        [Command("cute remove")]
        public async Task CuteRemove(int i)
        {
            int removeNumber = i;
            string title = $"{Context.User.Username} removed {DataStorage.GetAnimalURL(removeNumber)} from the list.";

            DataStorage.RemovePairFromAnimals(removeNumber);

            await SendEmbeddedMessage(title, "");
        }
        
        [Command("cute count")]
        public async Task CuteCount()
        {
            string title = $"The cutebank contains {DataStorage.GetAnimalsCount()} animals!";
            await SendEmbeddedMessage(title, "");
        }

        //fult command
        [Command("cute")]
        public async Task Cute()
        {
            string title = "!Cute commands";
            string description =
              "```"
            + "!Cute r \n"
            + "\nSends a random cute animal. \n"
            + "```"
            + "```"
            + "!Cute count \n"
            + "\nTells you how many cute animals are in the list. \n"
            + "```"
            + "```"
            + "!Cute nr <numberToDisplay> \n"
            + "\nDisplays any cute animal from anywhere in the list. \n"
            + "```"
            + "```"
            + "!Cute add <http://cuteanimallinkgoeshere.com> \n"
            + "\nAdds another cute animal to the list of cute animals. \n"
            + "```"
            + "```"
            + "!Cute remove <numberToRemove> \n"
            + "\nRemoves a cute animal from anywhere in the list :( \n"
            + "```"
            + "\n";

            await SendEmbeddedMessage(title, description);
        }

        [Command("test")]
        public async Task test()
        {
            if (Context.Channel.Name != "tempbotchannel")
            {
                return;
            }
            if (UserHasRole((SocketGuildUser)Context.User, "Bot Banned"))
            {
                //Add this line to json alers
                await Context.Channel.SendMessageAsync($"Sorry {Context.User.Mention}, you do not have access to that command.");
                return;
            } 
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync(Utilities.GetAlert("TEST"));
            //await Context.Channel.SendMessageAsync(Utilities.GetAlert("TEST"));
        }

        [Command("echo")]
        [Alias("me")]
        public async Task Echo([Remainder]string message)
        {
            string title = Utilities.GetFormattedAlert("MESSAGEBY_&USER", Context.User.Username);
            string description = message;
            
            await DeleteMessages(1);
            await SendEmbeddedMessage(title, description);
        }

        [Command("randomBetween")]
        public async Task RandomBetween([Remainder]string message)
        {
            string[] numbers = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (numbers.Length != 2)
            {
                //replace incorrect message with a syntax.json. Or actually, just the last part. First part can be title. Description is syntax.json (commandName)
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

            if (lowest > highest)
            {
                string title1 = "Incorrect command usage.";
                string description1 = "The second number must be greater than the first.";
                await SendEmbeddedMessage(title1, description1);
            }

            //int lowest = Convert.ToInt32(numbers[0]);
            //int highest = Convert.ToInt32(numbers[1]);
            Random r = new Random();

            int randomNum = r.Next(lowest, highest + 1);

            string title = Utilities.GetFormattedAlert("RANDOMNUM_&MIN_&MAX", numbers[0], numbers[1]);
            string description = randomNum.ToString();
            await SendEmbeddedMessage(title, description);
        }

        [Command("cheeky")]
        public async Task Cheeky()
        {
            string title = ";)";
            string footer = $"By {Context.User.Username}";
            await DeleteMessages(1);
            await SendEmbeddedMessage(title, "", "", "", footer);
        }

        [Command("pick")]
        public async Task PickOne([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            int i = r.Next(0, options.Length);
            string selection = options[i];

            string title = "I pick";
            string description = "**" + selection + "**";
            string thumbnailURL = "https://cdn.betterttv.net/emote/56e9f494fff3cc5c35e5287e/1x";
            await SendEmbeddedMessage(title, description, "", thumbnailURL);
        }

        [Command("removeMessages")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveMessages([Remainder]string message)
        {
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

            
            await DeleteMessages(amount);

            string title = "Messages removed!";
            string description = $"{Context.User.Username} removed {amount} message(s).";
            await SendEmbeddedMessage(title, description);
        }

        [Command("playing")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetPlaying([Remainder] string message)
        {
            await Context.Client.SetGameAsync(message);
        }

        //lots of todo on this
        [Command("person")]
        public async Task GeneratePerson([Remainder] string message = "")
        {

            string json = "";
            using (WebClient webClient = new WebClient())
            {
                json = webClient.DownloadString("https://randomuser.me/api/?gender=male");
            }

            var person = JsonConvert.DeserializeObject<dynamic>(json);

            string firstName = person.results[0].name.first.ToString();
            await SendEmbeddedMessage(firstName, "");
        }
        
        [Command("school")]
        public async Task SchoolCommand([Remainder] string message = "")
        {
            /*
            string startTime;
            startTime = DateTime.Now.ToString("HH:mm");
            Console.WriteLine(startTime);
            */

            //Console.WriteLine(DateTime.Now.DayOfWeek.ToString());
        }

        //add a bot banned role to check whenever a message is sent to bot
        private bool UserHasRole(SocketGuildUser user, string roleName)
        {
            string targetRoleName = roleName;

            var result = from r in user.Guild.Roles
                         where r.Name == targetRoleName
                         select r.Id;

            ulong roleID = result.FirstOrDefault();

            //The role wasn't found
            if (roleID == 0)
            {
                Console.WriteLine($"The Role \"{targetRoleName}\" could not be found. Did you misspell it?");
                return false;
            }
            var targetRole = user.Guild.GetRole(roleID);
            return user.Roles.Contains(targetRole);
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

        private async Task SendEmbeddedMessage(string title, string description, string message, string imageURL)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(description);
            embed.WithThumbnailUrl(imageURL);
            //Color could be a variable. 
            embed.WithColor(new Color(255, 165, 0));
            await Context.Channel.SendMessageAsync(message, false, embed);
        }

        private async Task SendEmbeddedMessage(string title, string description, string message, string imageURL, string footer)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(title);
            embed.WithDescription(description);
            embed.WithThumbnailUrl(imageURL);
            //Color could be a variable. 
            embed.WithColor(new Color(255, 165, 0));
            embed.WithFooter(footer);
            await Context.Channel.SendMessageAsync(message, false, embed);
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

    }
}
