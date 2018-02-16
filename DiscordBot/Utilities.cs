using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace DiscordBot
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        

        static Utilities()
        {
            //Reads .json file and converts into Dictionary<string, string> = "alerts"
            string json = File.ReadAllText("SystemLang/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetAlert(string key)
        {
            //If the dictionary contains the string that is passed in, return it's pair from the .json file.
            if (alerts.ContainsKey(key)) return alerts[key];
            return "";
            
        }


        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key], parameter);
            }

            return "";
        }

        public static string GetFormattedAlert(string key, object parameter)
        {
            return GetFormattedAlert(key, new object[] { parameter });
        }

        public static string UppercaseFirstLetter(string message)
        {
            return char.ToUpper(message[0]) + message.Substring(1);
        }

        //Is this string a url?
        public static bool ValidateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        //validate that file exists. Could move this into Utilities.
        public static bool ValidateStorageFile(string path)
        {
            //If file doesn't exist
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
                DataStorage.SaveData();
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
