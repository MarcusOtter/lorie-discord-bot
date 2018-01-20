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
    }
}
