using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot.Core
{
    public static class TicTacToeProvider
    {
        //______________Error messages______________//
        public static readonly string errGameInProgress = "ERR_GameInProgress";
        public static readonly string errGameNotInProgress = "ERR_GameNotInProgress";

        public static readonly string errUserNotPlaying = "ERR_UserNotPlaying";
        public static readonly string errUserAlreadyPlaying = "ERR_UserAlreadyPlaying";

        public static readonly string errUserMarkerEmpty = "ERR_UserMarkerEmpty";


        //______________Success messages______________//
        public static readonly string sucPlayer1Joined = "SUC_Player1Joined";
        public static readonly string sucPlayer2Joined = "SUC_Player2Joined";

        public static readonly string sucGameStopped = "SUC_GameStopped";


        //The 2 active players
        public static IGuildUser player1 = null;
        public static IGuildUser player2 = null;

        public static bool isFirstPlayersTurn;


        public static bool UserIsInGame(IGuildUser player)
        {
            return player != null && (player1 == player || player2 == player);
        }

        public static bool GameIsInProgress()
        {
            return player1 != null && player2 != null;
        }

        public static bool PlayerMarkerIsEmpty(IGuildUser player)
        {
            var userAccount = UserAccounts.UserAccounts.GetAccount(player.Id);

            return userAccount == null || string.IsNullOrWhiteSpace(userAccount.TTTMarker);
        }

        public static string AttemptPlayerJoin(IGuildUser player)
        {
            if (GameIsInProgress())
            {
                return errGameInProgress;
            }

            if (player1 == player)
            {
                return errUserAlreadyPlaying;
            }

            if (player1 == null)
            {
                player1 = player;
                return sucPlayer1Joined;
            }
            
            if (player2 == null)
            {
                player2 = player;

                //StartGame method that draws board and mentions player 1 perhaps.
                return sucPlayer2Joined;
                
            }

            return "";
        }

        public static string StartGame()
        {
            return
                (string.Concat
                    (
                        "New game started!\n",
                        $"(P1){player1.Mention} vs (P2){player2.Mention}\n",
                        TicTacToe.DrawBoard()
                    )
                );
        }

        public static string StopPlaying(IGuildUser user)
        {
            if (!GameIsInProgress())
            {
                return errGameNotInProgress;
            }
            
            if (!UserIsInGame(user))
            {
                return errUserNotPlaying;
            }

            player1 = null;
            player2 = null;
            TicTacToe.ResetGame();
            return sucGameStopped;
        }

        public static void ForceGameRestart()
        {
            player1 = null;
            player2 = null;
            TicTacToe.ResetGame();
        }

        //This need a better translation layer I think that kinda check everything but I'm too tired atm
        public static string PlaceMarker(IGuildUser player, int x, int y)
        {
            if (!UserIsInGame(player))
            {
                //"Please join the game first"
                return errUserNotPlaying;
            }

            if (!GameIsInProgress())
            {
                //"Wait for an opponent"
                return errGameNotInProgress;
            }

            if (PlayerMarkerIsEmpty(player))
            {
                return errUserMarkerEmpty;
            }

            //By now I know that this is not ever null thanks to PlayerMarkerIsEmpty
            var userAccount = UserAccounts.UserAccounts.GetAccount((SocketGuildUser)player);

            //Probably need more checks but yolo
            return TicTacToe.TicTacToeMove(userAccount.TTTMarker, player.Username, x, y);

            //return tic tac thing at the very end
        }
    }
}
