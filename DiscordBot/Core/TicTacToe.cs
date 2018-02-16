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

        internal static List<string> allowedEmojis = new List<string>
        {
            "😀",
            ":grinning:",
            "😬",
            ":grimacing:",
            "😁",
            ":grin:",
            "😂",
            ":joy:",
            "😃",
            ":smiley:",
            "😄",
            ":smile:",
            "😅",
            ":sweat_smile:",
            "😆",
            ":laughing:",
            "😇",
            ":innocent:",
            "😉",
            ":wink:",
            "<:Wutface:370271201832075275>",
            "<:Wut:369748932785471490>",
            "<:Toxic:403559839659458581>",
            "<:ThumbsUpR:369783106904784896>",
            "<:ThumbsUpL:370329030144163840>",
            "<:TantaTriggered:364752742389907466>",
            "<:TantaFanta:364424683396792322>",
            "<:Tanta:370508620464521226>",
            "<:Sweaty:369447013810569216>",
            "<:SMOrc:394600819531579392>",
            "<:SatMarit:370492369620959232>",
            "<:RollSafe:370482487241211905>",
            "<:ResidentSleeper:378960240541958144>",
            "<:Reee:370837066906468353>",
            "<:PogChamp:372270237946478592>",
            "<:PimpAlex:376032933166841857>",
            "<:Phil:369770726774931466>",
            "<:Peter:368317157496586242>",
            "<:PagChomp:384648710354894848>",
            "<:OMEGALUL:376027024999055360>",
            "<:NotLikeThis:369774226569166850>",
            "<:Nazi:369990956008865793>",
            "<:MonkaS:369824860328951819>",
            "<:Mikael:370199314707775488>",
            "<:Marre:369465023590039553>",
            "<:Marit:368313260274876426>",
            "<:Maria:371948510075027456>",
            "<:LUL:360520799263784961>",
            "<:Kreygasm:378904041331097600>",
            "<:Kinky:370546790199590913>",
            "<:KappaPride:369992730576814081>",
            "<:Kappa:361828821848162315>",
            "<:Jebaited:370272065602715658>",
            "<:Jackie:410063061257420811>",
            "<:Irme:369796165585207306>",
            "<:Harold:369772400188981248>",
            "<:Gun:382126239831293953>",
            "<:FeelsTiredMan:370822096483516417>",
            "<:FeelsGoodMan:370476291893231616>",
            "<:FeelsBadMan:362129085813489664>",
            "<:FailFish:370210961174429696>",
            "<:Ducky:404985688174624771>",
            "<:Cough:367231620207607808>",
            "<:CoolStoryBob:370330125855883278>",
            "<:Communism:369825812905590785>",
            "<:CmonBruh:369783076496080907>",
            "<:Carried:367242474567237632>",
            "<:Bowe:370504128755138560>",
            "<:BibleThump:369773282368552962>",
            "<:Benjasas:370506920118976512>",
            "<a:RareParrot:394551061144403968>"
        };

        private static string templateBoard = string.Concat
        (
            "|:black_medium_square:|:zero:|:one:|:two:|\n",
            "|:zero:|{0}|{1}|{2}|\n",
            "|:one:|{3}|{4}|{5}|\n",
            "|:two:|{6}|{7}|{8}|\n"
        );

        public static string DrawBoard()
        {
            if(boardTiles.Count == 0)
            {
                InitializeBoard();
            }

            return string.Format
            (
                templateBoard,
                boardTiles[0].GetEmoteMarker(), boardTiles[1].GetEmoteMarker(), boardTiles[2].GetEmoteMarker(),
                boardTiles[3].GetEmoteMarker(), boardTiles[4].GetEmoteMarker(), boardTiles[5].GetEmoteMarker(),
                boardTiles[6].GetEmoteMarker(), boardTiles[7].GetEmoteMarker(), boardTiles[8].GetEmoteMarker(), boardTiles[0].GetEmoteMarker());
        }

        
        //Main function
        public static string TicTacToeMove(string playerMarker, string playerName, int x, int y)
        {
            if (boardTiles.Count == 0)
            {
                InitializeBoard();
            }

            //If this is the first TicTacToeMove
            if (players.Count == 0)
            {
                //Create player 1 and assign given player name.
                Player p1 = new Player(1, playerMarker, playerName);
                players.Add(p1);
            }
            else if (players.Count == 1)
            {
                //Create player 2 and assign given player name.
                Player p2 = new Player(2, playerMarker, playerName);
                players.Add(p2);
            }

            //If player doesn't exist by now, return.
            if (!PlayerAlreadyExists(playerName))
            {
                return "ERR01:UnknownPlayer";
            }

            //If player 1 tries to play and it isn't player 1's turn
            if (GetPlayer(playerName).Number == 1 && !isFirstPlayersTurn)
            {
                return "INVALID_MOVE";
            }

            //If player 2 tries to play and it isn't player 2's turn
            if (GetPlayer(playerName).Number == 2 && isFirstPlayersTurn)
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
            MarkTile(x, y, playerName);

            //Flip the turns
            isFirstPlayersTurn = !isFirstPlayersTurn;

            //Check the game state and return appropriate strings.
            switch (GetGameState())
            {
                case -1:
                    ResetGame();
                    return "GAME_TIED";

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

        public static void ResetGame()
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

            if (GameIsTied())
            {
                return -1;
            }
            return 0;
        }

        //Is the game tied?
        internal static bool GameIsTied()
        {
            for (int i = 0; i < boardTiles.Count; i++)
            {
                if (boardTiles[i].MarkedByPlayer == 0)
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
            private string marker;

            internal Player(int playerNumber, string playerMarker, string playerName)
            {
                number = playerNumber;
                marker = playerMarker;
                name = playerName;
            }

            internal string Marker
            {
                get { return marker; }
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
                    return players[0].Marker;
                }
                else if (MarkedByPlayer == 2)
                {
                    return players[1].Marker;
                }
                return "InvalidMarker!";
            }

        }
    }
}