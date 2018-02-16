using DiscordBot.Core.UserAccounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public static class DataStorage
    {
        private const string dataFolder = "Resources";
        private const string animalsFile = "cute.json";
        private const string scheduleFile = "schoolSchedule.json";

        //Could be of <int, Animal> so it can contain information like who posted it, when, etc. Also maybe upvotes how many times it has gotten a heart or a thumbs up or w/e
        private static List<string> animols = new List<string>();

        //public static void FixAnimals()
        //{
        //    animols = animals.Values.ToList();
        //    for (int i = 0; i < animols.Count; i++)
        //    {
        //        Console.Write(animols[i] + "\n");
        //    }
        //}

        static DataStorage()
        {
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            //If file doesn't exist, create it and return.
            if (!Utilities.ValidateStorageFile(dataFolder + "/" + animalsFile))
            {
                return;
            }

            if (!Utilities.ValidateStorageFile(dataFolder + "/" + scheduleFile))
            {
                return;
            }

            string json = File.ReadAllText(dataFolder + "/" + animalsFile);
            animols = JsonConvert.DeserializeObject<List<string>>(json);
        }

        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts);
            //xdoupt
            Utilities.ValidateStorageFile(filePath);

            File.WriteAllText(filePath, json);
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Couldn't load user accounts: File does not exist.");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }

        public static void AddAnimal(string value)
        {
            animols.Add(value);
            SaveData();
        }

        public static void RemovePairFromAnimals(int animalNumber)
        {
            animols.RemoveAt(animalNumber);
            SaveData();
        }

        public static int GetAnimalsCount()
        {
            return animols.Count();
        }

        public static bool URLAlreadyExists(string url)
        {
            if (animols.Contains(url))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetAnimalURL(int animalNumber)
        {
            return animols[animalNumber];
        }

        public static void SaveData()
        {
            string json = JsonConvert.SerializeObject(animols, Formatting.Indented);
            File.WriteAllText(dataFolder + "/" + animalsFile, json);
        }
    }
}
