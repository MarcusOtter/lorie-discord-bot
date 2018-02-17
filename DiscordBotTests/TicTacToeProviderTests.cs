using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordBot.Core;
using Moq;
using NUnit.Framework;

namespace DiscordBotTests
{
    public class TicTacToeProviderTests
    {
        [Test]
        public void UserIsInGame_NullRef()
        {
            const bool expected = false;

            bool actual = TicTacToeProvider.UserIsInGame(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserIsInGame_NonPlayingUser()
        {
            var mockNonPlayingUser = new Mock<IGuildUser>();
            mockNonPlayingUser.Setup(u => u.Id).Returns(120);

            const bool expected = false;

            bool actual = TicTacToeProvider.UserIsInGame(mockNonPlayingUser.Object);

            Assert.NotNull(mockNonPlayingUser.Object);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserIsInGame_ValidPlayer1()
        {
            var mockPlayer = new Mock<IGuildUser>();
            mockPlayer.Setup(u => u.Id).Returns(121);

            const bool expected = true;

            string joinResult = TicTacToeProvider.AttemptPlayerJoin(mockPlayer.Object);
            bool actual = TicTacToeProvider.UserIsInGame(mockPlayer.Object);

            TicTacToeProvider.ForceGameRestart();

            Assert.AreEqual(TicTacToeProvider.sucPlayer1Joined, joinResult);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UserIsInGame_ValidPlayer2()
        {
            var mockPlayer1 = new Mock<IGuildUser>();
            mockPlayer1.Setup(u => u.Id).Returns(121);

            var mockPlayer2 = new Mock<IGuildUser>();
            mockPlayer2.Setup(u => u.Id).Returns(122);

            const bool expectedUserIsPlaying = true;

            string joinResult1 = TicTacToeProvider.AttemptPlayerJoin(mockPlayer1.Object);
            string joinResult2 = TicTacToeProvider.AttemptPlayerJoin(mockPlayer2.Object);
            bool actualPlayer1Playing = TicTacToeProvider.UserIsInGame(mockPlayer1.Object);
            bool actualPlayer2Playing = TicTacToeProvider.UserIsInGame(mockPlayer2.Object);
            bool actualGameInProgress = TicTacToeProvider.GameIsInProgress();

            TicTacToeProvider.ForceGameRestart();

            Assert.AreEqual(true, actualGameInProgress);
            Assert.AreEqual(TicTacToeProvider.sucPlayer1Joined, joinResult1);
            Assert.AreEqual(TicTacToeProvider.sucPlayer2Joined, joinResult2);
            Assert.AreEqual(expectedUserIsPlaying, actualPlayer1Playing);
            Assert.AreEqual(expectedUserIsPlaying, actualPlayer2Playing);
        }
    }
}
