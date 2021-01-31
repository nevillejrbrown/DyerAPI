using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using DyerAPI.Models;
using FluentAssertions;

namespace DyerAPITests.SystemTests.API
{
    class GameAPI
    {
        private const string BASE_URL = "https://localhost:44325";
        private const string GAMES_RESOURCE = "games";
        private const string CELEBS_RESOURCE = "celebs";

        private static RestClient Client { get
            {
                return new RestClient(BASE_URL);
            } }

        public static Celeb AddCelebToGame(Celeb celeb, Game game)
        {
            var request = new RestRequest($"{GAMES_RESOURCE}/{game.Id}/{CELEBS_RESOURCE}");
            request.AddJsonBody(celeb);
            var response = Client.Post<Celeb>(request);
            return response.Data;
        }

        public static void DeleteGame(int gameId)
        {
            Client.Delete(new RestRequest($"{GAMES_RESOURCE}/{gameId}/{CELEBS_RESOURCE}"));
        }

        public static Game GetGameById(int gameId)
        {
            return Client.Get<Game>(new RestRequest($"{GAMES_RESOURCE}/{gameId}")).Data;
        }

        public static Game CreateGame(String gameName)
        {
            var request = new RestRequest(GAMES_RESOURCE);
            request.AddJsonBody(new Game() { Name = gameName });
            var response = Client.Post<Game>(request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            return response.Data;
        }

        public static void UpdateCeleb(Celeb celeb)
        {
            var request = new RestRequest($"{CELEBS_RESOURCE}/{celeb.Id}");
            request.AddJsonBody(celeb);
            var response = Client.Patch(request);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        public static Celeb GetCeleb(int id)
        {
            var request = new RestRequest($"{CELEBS_RESOURCE}/{id}");
            var response = Client.Get<Celeb>(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            return response.Data;
        }
    }
}
