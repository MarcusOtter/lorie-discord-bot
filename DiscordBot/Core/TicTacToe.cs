using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Core.UserAccounts;

namespace DiscordBot.Core
{
    public static class TicTacToe
    {
        internal static List<Tile> boardTiles = new List<Tile>();
        internal static List<Player> players = new List<Player>();
        internal static bool isFirstPlayersTurn = true;

        private static string templateBoard = string.Concat
        (
            "|:black_medium_square:|:zero:|:one:|:two:|\n",
            "|:zero:|{0}|{1}|{2}|\n",
            "|:one:|{3}|{4}|{5}|\n",
            "|:two:|{6}|{7}|{8}|\n"
        );

        public static string DrawBoard()
        {

            return string.Format
            (
                templateBoard,
                boardTiles[0].GetEmoteMarker(), boardTiles[1].GetEmoteMarker(), boardTiles[2].GetEmoteMarker(),
                boardTiles[3].GetEmoteMarker(), boardTiles[4].GetEmoteMarker(), boardTiles[5].GetEmoteMarker(),
                boardTiles[6].GetEmoteMarker(), boardTiles[7].GetEmoteMarker(), boardTiles[8].GetEmoteMarker(), boardTiles[0].GetEmoteMarker());
        }

        public static string[] SetMarker(SocketUser user, string marker)
        {
            //if (marker.Length > 1)
            //{
            //    //await SendEmbeddedMessage("Error", "Please do not enter more than 1 character.");
            //    return new string[] {"Error", "**Please do not enter more than 1 character.**\n\nA list of emojis can be found here: https://apps.timwhitlock.info/emoji/tables/unicode#block-2-dingbats. \nAll emojis under \"2. Dingbats\" and the first half of the emojis under \n\"5. Uncategorized\" work." };
            //}

            var account = UserAccounts.UserAccounts.GetAccount(user);
            account.TTTMarker = marker;
            return new string[] { "Marker updated!", $"Marker for {user.Username} was updated to {marker}" };
        }

        //Main function
        internal static string TicTacToeMove(SocketUser player, int x, int y)
        {
            if (boardTiles.Count == 0)
            {
                InitializeBoard();
            }

            //If this is the first TicTacToeMove
            if (players.Count == 0)
            {
                //Create player 1 and assign given player name.
                Player p1 = new Player(1, player);
                players.Add(p1);
            }
            else if (players.Count == 1)
            {
                //Create player 2 and assign given player name.
                Player p2 = new Player(2, player);
                players.Add(p2);
            }

            //If player doesn't exist by now, return.
            if (!PlayerAlreadyExists(player.Username))
            {
                return "ERR01:UnknownPlayer";
            }

            //If player 1 tries to play and it isn't player 1's turn
            if (GetPlayer(player.Username).Number == 1 && !isFirstPlayersTurn)
            {
                return "INVALID_MOVE";
            }

            //If player 2 tries to play and it isn't player 2's turn
            if (GetPlayer(player.Username).Number == 2 && isFirstPlayersTurn)
            {
                return "INVALID_MOVE";
            }

            //If the passed in x position is out of bounds
            if (x < 0 || x > 2)
            {
                return "INVALID_MOVE";
            }

            //If the passed in y position is out of bounds
            if (y < 0 || y > 2)
            {
                return "INVALID_MOVE";
            }

            //If the tile is already occupied
            if (TileIsOccupied(x, y))
            {
                return "INVALID_MOVE";
            }

            //Mark the tile
            MarkTile(x, y, player.Username);

            //Flip the turns
            isFirstPlayersTurn = !isFirstPlayersTurn;

            //Check the game state and return appropriate strings.
            switch (GetGameState())
            {
                case -1:
                    ResetGame();
                    return "Game tied.";

                case 0:
                    return "ACK";

                case 1:
                    ResetGame();
                    return "PLAYER_1_WIN";

                case 2:
                    ResetGame();
                    return "PLAYER_2_WIN";
            }
            return null;
        }

        internal static void ResetGame()
        {
            players.Clear();
            boardTiles.Clear();
            isFirstPlayersTurn = true;
        }

        //Returns one of the following:
        // -1   = Tie
        // 0    = Game still in progress
        // 1    = Player 1 won
        // 2    = Player 2 won
        internal static int GetGameState()
        {
            if (GetWinState() == 1)
            {
                return 1;
            }

            if (GetWinState() == 2)
            {
                return 2;
            }

            if (CheckTie())
            {
                return -1;
            }
            return 0;
        }

        //Is the game tied?
        internal static bool CheckTie()
        {
            for (int i = 0; i < boardTiles.Count; i++)
            {
                if (boardTiles[i].MarkedByPlayer != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //Has somebody won the game?
        internal static int GetWinState()
        {
            //Checks if the upper horizontal or the left vertical row is filled with the same mark.
            if (IsRowFilled(GetMarkFromTile(0, 0), GetMarkFromTile(0, 1), GetMarkFromTile(0, 2)) ||
                 IsRowFilled(GetMarkFromTile(0, 0), GetMarkFromTile(1, 0), GetMarkFromTile(2, 0)))
            {
                //Returns the mark in the tile that they have in common.
                return GetMarkFromTile(0, 0);
            }

            //Checks if the middle horizontal or middle vertical row is filled with the same mark.
            if (IsRowFilled(GetMarkFromTile(0, 1), GetMarkFromTile(1, 1), GetMarkFromTile(2, 1)) ||
                 IsRowFilled(GetMarkFromTile(1, 0), GetMarkFromTile(1, 1), GetMarkFromTile(1, 2)))
            {
                //Returns the mark in the tile that they have in common.
                return GetMarkFromTile(1, 1);
            }

            //Checks if the bottom horizontal or the right vertical row is filled with the same mark.
            if (IsRowFilled(GetMarkFromTile(0, 2), GetMarkFromTile(1, 2), GetMarkFromTile(2, 2)) ||
                 IsRowFilled(GetMarkFromTile(2, 0), GetMarkFromTile(2, 1), GetMarkFromTile(2, 2)))
            {
                //Returns the mark in the tile that they have in common.
                return GetMarkFromTile(2, 2);
            }

            //Checks if the any of the diagonal rows are filled with the same mark
            if (IsRowFilled(GetMarkFromTile(0, 0), GetMarkFromTile(1, 1), GetMarkFromTile(2, 2)) ||
                 IsRowFilled(GetMarkFromTile(2, 0), GetMarkFromTile(1, 1), GetMarkFromTile(0, 2)))
            {
                //Returns the mark in the tile that they have in common.
                return GetMarkFromTile(1, 1);
            }

            //Game still playing
            return 0;
        }

        //Are the 3 marks that are passed in the of the number?
        internal static bool IsRowFilled(int first, int second, int third)
        {
            return (first != 0 && first == second && second == third);
        }

        //What is this tile marked with?
        internal static int GetMarkFromTile(int x, int y)
        {
            return GetTile(x, y).MarkedByPlayer;
        }

        //Mark the tile for player with 'name'
        internal static void MarkTile(int x, int y, string name)
        {
            GetTile(x, y).MarkedByPlayer = GetPlayer(name).Number;
        }

        //Returns a tile from x and y position
        internal static Tile GetTile(int x, int y)
        {
            foreach (var tile in boardTiles)
            {
                if (tile.Pos.x == x && tile.Pos.y == y)
                {
                    return tile;
                }
            }
            return null;
        }

        //Does this tile have a marker in it?
        internal static bool TileIsOccupied(int x, int y)
        {
            return GetTile(x, y).MarkedByPlayer != 0;
        }

        //Does this player already exist?
        internal static bool PlayerAlreadyExists(string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        //Returns a player by name.
        internal static Player GetPlayer(string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Name == name)
                {
                    return players[i];
                }
            }
            return new Player();
        }

        //Player struct
        internal struct Player
        {
            private int number;
            private string name;
            private SocketUser socketUsr;

            internal Player(int playerNumber, SocketUser player)
            {
                number = playerNumber;
                name = player.Username;
                socketUsr = player;
            }

            internal SocketUser SocketUsr
            {
                get { return socketUsr; }
            }

            internal int Number
            {
                get { return number; }

            }

            internal string Name
            {
                get { return name; }
            }
        }

        //Initializes the board
        internal static void InitializeBoard()
        {
            //Creates 9 tiles. Their position is in a 3x3 layout. Tile.Postion(0,0) is top left corner.
            //Order is (0,0)  (0,1)  (0,2)  (1,0)  (1,1)...
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    boardTiles.Add(new Tile(i, j));
                }
            }
        }

        //Position struct that holds information about where a tile is.
        internal struct Position
        {
            internal int x, y;

            internal Position(int xPos, int yPos)
            {
                x = xPos;
                y = yPos;
            }
        }

        //The tile class
        internal class Tile
        {
            //Constructor for a Tile
            internal Tile(int x, int y)
            {
                //Simplifying this will result in code like x=x and y=y
                pos = new Position();
                pos.x = x;
                pos.y = y;
            }

            //Holds information about where the tile is on the board.
            Position pos;

            internal Position Pos
            {
                get { return pos; }
            }

            //Holds information about who's put a marker in this tile.
            private int markedByPlayer;

            internal int MarkedByPlayer
            {
                get { return markedByPlayer; }
                set { markedByPlayer = value; }
            }

            internal string GetEmoteMarker()
            {
                if (MarkedByPlayer == 0)
                {
                    return ":black_medium_square:";
                }
                else if (MarkedByPlayer == 1)
                {
                    return UserAccounts.UserAccounts.GetAccount(players[0].SocketUsr).TTTMarker;
                }
                else if (MarkedByPlayer == 2)
                {
                    return UserAccounts.UserAccounts.GetAccount(players[1].SocketUsr).TTTMarker;
                }
                return "InvalidMarker!";
            }

        }
    }
}