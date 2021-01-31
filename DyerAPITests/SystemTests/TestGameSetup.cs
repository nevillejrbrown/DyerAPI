using Microsoft.VisualStudio.TestTools.UnitTesting;
using DyerAPI.Models;
using RestSharp;
using FluentAssertions;
using DyerAPITests.SystemTests.API;

namespace DyerAPITests.SystemTests
{
    [TestClass]
    public class TestGameSetup
    {

        [TestMethod]
        public void TestCreateGame()
        {
            Game game = GameAPI.CreateGame("New Game");
            game.Should().NotBeNull();
            game.Id.Should().BeGreaterThan(0);
            GameAPI.DeleteGame(game.Id);
        }

        [TestMethod]
        public void TestCelebGuessed()
        {
            Game newGame = GameAPI.CreateGame("New Game");
            Celeb celeb = GameAPI.AddCelebToGame(new Celeb() { Name = "Name 1", GameId = newGame.Id }, newGame);
            Celeb celeb2 = GameAPI.AddCelebToGame(new Celeb() { Name = "Name 2", GameId = newGame.Id }, newGame);
            Celeb celeb3 = GameAPI.AddCelebToGame(new Celeb() { Name = "Name 3", GameId = newGame.Id }, newGame);

            celeb2.Guess();

            GameAPI.UpdateCeleb(celeb2);

            Celeb retrievedCeleb = GameAPI.GetCeleb(celeb2.Id);

            retrievedCeleb.State.Should().Be(CelebState.GUESSED);


        }

        [TestMethod]
        public void TestAddCeleb()
        {
            Game newGame = GameAPI.CreateGame("New Game");
            Celeb celeb = GameAPI.AddCelebToGame(new Celeb() { Name = "Name 1", GameId = newGame.Id }, newGame);
            Celeb celeb2 = GameAPI.AddCelebToGame(new Celeb() { Name = "Name 2", GameId = newGame.Id }, newGame);

            Game gameRetrieved = GameAPI.GetGameById(newGame.Id);
            gameRetrieved.Celebs.Should().HaveCount(2);
            gameRetrieved.Celebs.Should().ContainEquivalentOf(celeb);
            gameRetrieved.Celebs.Should().ContainEquivalentOf(celeb2);
        }



    }





}
