using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class DataStorage
    {
        public static Dictionary<int, string> animals = new Dictionary<int, string>();

        static DataStorage()
        {
            //If file doesn't exist, create it and return.
            if (!ValidateStorageFile("Animals.json"))
            {
                return;
            }

            string json = File.ReadAllText("Animals.json");
            animals = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(animals, Formatting.Indented);
            File.WriteAllText("Animals.json", json);
        }

        //validate that file exists. Could move this into Utilities.
        private static bool ValidateStorageFile(string file)
        {
            //If file doesn't exist
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            else
            {
                return true;
            }
            
        }
    }
}
