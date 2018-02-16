using System;
using DiscordBot.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace DiscordBotTests
{
    public class TicTacToeTests
    {
        [Test]
        public void TicTacToeTest_UnknownPlayer()
        {
            string expected = "ERR01:UnknownPlayer";

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
            string expected = "INVALID_MOVE";

            string actual = TicTacToe.TicTacToeMove("q", "C", x, y);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TicTacToeTest_PlayerCheating()
        {
            string expected = "INVALID_MOVE";

            // Tests if the player can go twice...
            TicTacToe.TicTacToeMove("x", "PlayerA", 0, 0);
            string actual = TicTacToe.TicTacToeMove("x", "PlayerA", 1, 1);

            TicTacToe.ResetGame();
            Assert.AreEqual(expected, actual);
        }

    }
}
