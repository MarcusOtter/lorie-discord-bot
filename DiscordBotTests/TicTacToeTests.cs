using System;
using DiscordBot.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace DiscordBotTests
{
    
    public class TicTacToeTests
    {
        //Board layout
        //[0,0][1,0][2,0]
        //[0,1][1,1][2,1]
        //[0,2][1,2][2,2]

        [Test]
        public void TicTacToeTest_Player1Win()
        {
            const string expected = "PLAYER_1_WIN";

            TicTacToe.TicTacToeMove("x", "player1", 0, 0);
            TicTacToe.TicTacToeMove("o", "player2", 2, 2);
            TicTacToe.TicTacToeMove("x", "player1", 1, 0);
            TicTacToe.TicTacToeMove("o", "player2", 2, 1);
            string actual = TicTacToe.TicTacToeMove("x", "player1", 2, 0); //Winning move

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_Player2Win()
        {
            const string expected = "PLAYER_2_WIN";

            TicTacToe.TicTacToeMove("x", "player1", 0, 0);
            TicTacToe.TicTacToeMove("o", "player2", 2, 2);
            TicTacToe.TicTacToeMove("x", "player1", 1, 0);
            TicTacToe.TicTacToeMove("o", "player2", 2, 1);
            TicTacToe.TicTacToeMove("x", "player1", 0, 1);
            string actual = TicTacToe.TicTacToeMove("o", "player2", 2, 0);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_GameTied()
        {
            const string expected = "GAME_TIED";
            const string okMove = "ACK";

            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("x", "player1", 0, 0));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("o", "player2", 0, 2));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("x", "player1", 1, 1));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("o", "player2", 1, 0));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("x", "player1", 2, 0));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("o", "player2", 2, 1));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("x", "player1", 1, 2));
            Assert.AreEqual(okMove, TicTacToe.TicTacToeMove("o", "player2", 2, 2));
            string actual = TicTacToe.TicTacToeMove("x", "player1", 0, 1);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_TileAlreadyContainsMarker()
        {
            const string expected = "INVALID_MOVE";

            TicTacToe.TicTacToeMove("x", "A", 0, 0);

            string actual = TicTacToe.TicTacToeMove("o", "B", 0, 0);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_UnknownPlayer()
        {
            const string expected = "ERR01:UnknownPlayer";

            TicTacToe.TicTacToeMove("x", "A", 0, 0);
            TicTacToe.TicTacToeMove("o", "B", 0, 1);
            string actual = TicTacToe.TicTacToeMove("q", "C", 1, 0);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_MoveOutOfBounds(
            [Values(-10, 50)] int x,
            [Values(-10, 50)] int y)
        {
            const string expected = "INVALID_MOVE";

            string actual = TicTacToe.TicTacToeMove("q", "C", x, y);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_PlayerCheating()
        {
            const string expected = "INVALID_MOVE";

            // Tests if the player can go twice...
            TicTacToe.TicTacToeMove("x", "PlayerA", 0, 0);
            string actual = TicTacToe.TicTacToeMove("x", "PlayerA", 1, 1);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

    }
}
