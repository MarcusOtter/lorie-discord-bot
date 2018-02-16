using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class Maths
    {
        public static bool MessageContainsHowWhatMistake(string message)
        {
            string lowerMessage = message.ToLower();
            if (lowerMessage.Contains("what"))
            {
                if (lowerMessage.Contains("like"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (lowerMessage.Contains("how"))
            {
                int howPos = lowerMessage.IndexOf("how");
                int likePos = lowerMessage.IndexOf("like");

                if (lowerMessage.Contains("like") && likePos > howPos)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        //----------------------------------------------------------------------------------------------------
        //Add users to a list of groups. Create groups. If they exist, don't add. Format !report command properly.
        public static Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

        public static string RunGroupCommand(string command)
        {
            string[] splitCommand = command.Split(' ');

            if (command.ToLower().Contains("adduser"))
            {
                if (splitCommand.Length != 4)
                {
                    //return "Wrong format";
                }

                return (AddUser(splitCommand[1], splitCommand[3]));
            }
            else if (command.ToLower().Contains("creategroup"))
            {
                if (splitCommand.Length != 2)
                {
                    //return "Wrong format";
                }

                return (CreateGroup(splitCommand[1]));
            }
            else if (command.ToLower().Contains("report"))
            {
                if (splitCommand.Length != 1)
                {
                    //return "Wrong format";
                }

                return Report();

            }
            return "";
        }

        public static string AddUser(string name, string groupName)
        {
            if (!groups.ContainsKey(groupName))
            {
                return "(!) Group doesn't exist.";
            }
            else
            {
                List<string> names;
                groups.TryGetValue(groupName, out names);

                if (names.Contains(name))
                {
                    return "(!) User is already in that group.";
                }
                else
                {
                    groups[groupName].Add(name);
                    return "(i) DONE";
                }
            }
        }

        public static string CreateGroup(string groupName)
        {
            if (groups.ContainsKey(groupName))
            {
                return "(!) Group already exists.";
            }
            else
            {
                groups.Add(groupName, new List<string>());
                return "(i) DONE";
            }
        }

        public static string Report()
        {
            string output = "";

            foreach (var item in groups)
            {
                item.Value.Sort();
            }

            for (int i = 0; i < groups.Count; i++)
            {
                output += groups.ElementAt(i).Key + " : ";

                for (int j = 0; j < groups.ElementAt(i).Value.Count; j++)
                {
                    //If this is --not-- the last name in the users
                    if (groups.ElementAt(i).Value.ElementAt(j) != groups.ElementAt(i).Value.Last())
                    {
                        output += groups.ElementAt(i).Value.ElementAt(j) + ", ";
                    }
                    else
                    {
                        output += groups.ElementAt(i).Value.ElementAt(j) + " ";

                        if (i != groups.Count - 1)
                        {
                            output += "- ";
                        }
                    }
                }
            }
            return output;
        }
        //-----------------------------------------------------------------------------------------------------------------------




        //----------------------------------------------------------
        //Calculate sum of the numbers in a string.
        public static int CalculateSumOfDigits(string digitString)
        {
            char[] array = digitString.ToCharArray();
            int result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                int charVal = (int)Char.GetNumericValue(array[i]);

                if (charVal > 0)
                {
                    result += charVal;
                }
            }
            return result;
        }
        //------------------------------------------------------




        //----------------------------------------------------------
        //Convert HEX to RGB
        public static Color GetRGBFromHEX(string hexString)
        {
            string input = hexString;

            input = input.Replace("#", "");

            char[] chars = input.ToLower().ToCharArray();

            double[] doubles = HexToDecimal(chars);

            Color rgbColor = new Color()
            {
                Red = (doubles[0] * 16) + (doubles[1] * 1),
                Green = (doubles[2] * 16) + (doubles[3] * 1),
                Blue = (doubles[4] * 16) + (doubles[5] * 1)
            };

            return rgbColor;
        }

        public static double[] HexToDecimal(char[] hexValues)
        {
            double[] decimalDoubles = new double[hexValues.Length];

            for (int i = 0; i < hexValues.Length; i++)
            {
                decimalDoubles[i] = (double)Convert.ToInt32(hexValues[i].ToString(), 16);
            }
            return decimalDoubles;
        }

        public struct Color
        {
            public double Red;
            public double Green;
            public double Blue;
        }
        //---------------------------------------------------------------
    }
}
