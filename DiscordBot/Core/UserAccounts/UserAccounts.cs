using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> userAccounts;
        private static string accountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (DataStorage.FileExists(accountsFile))
            {
                userAccounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }
            else
            {
                userAccounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(userAccounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        public static UserAccount GetAccount(ulong id)
        {
            return GetOrCreateAccount(id);
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in userAccounts
                         where a.ID == id
                         select a;

            var account = result.FirstOrDefault();

            if (account == null)
            {
                account = CreateUserAccount(id);
            }

            return account;

        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount(id);
            userAccounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}
