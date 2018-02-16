using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Core.UserAccounts
{
    public class UserAccount
    {
        //Constructor for a new account
        public UserAccount(ulong userID)
        {
            ID = userID;
            TotalBalance = 0;
            TTTMarker = "";
        }

        public ulong ID { get; set; }

        private uint money;

        public uint TotalBalance
        {
            get { return money; }
            set
            {
                money = value;
                UserAccounts.SaveAccounts();
            }
        }

        //Tic tac toe marker.
        private string _TTTMarker;

        public string TTTMarker
        {
            get { return _TTTMarker; }
            set
            {
                _TTTMarker = value;
                UserAccounts.SaveAccounts();
            }
        }
    }
}
