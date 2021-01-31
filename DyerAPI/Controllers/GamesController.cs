using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DyerAPI.Models;
using DyerAPI.Models.Data;
using Microsoft.Extensions.Logging;
using DyerAPI.Models.Service;

namespace DyerAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<GamesController> _logger;
        private readonly IGameService _gameService;

        public GamesController(ILogger<GamesController> logger, IGameService gameService)
        {
            _logger = logger;
            _gameService = gameService;
        }

        [Route("games")]
        [HttpGet]
        public ActionResult<IEnumerable<Game>> GetGames()
        {
            return _gameService.GetAllGames();
        }

        [Route("games/{gameId:int}")]
        [HttpGet("{id}")]
        public ActionResult<Game> GetGame(int gameId)
        {
            var game = _gameService.GetGameById(gameId);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }


        [Route("games/{gameId:int}")]
        [HttpPut]
        public IActionResult PutGame(int gameId, Game game)
        {
            _gameService.UpdateGame(game);
            return NoContent();
        }

        [Route("games/{gameId:int}/celebs")]
        [HttpPost]
        public ActionResult<Celeb> PostCeleb(int gameId, Celeb celeb)
        {
            if (gameId != celeb.GameId)
            {
                throw new ArgumentException($"gameId: {gameId} doesn't equal game id in celeb: {celeb.GameId}");
            }
            return _gameService.AddCeleb(celeb);
        }

        [Route("games/{gameId:int}/celebs")]
        [HttpGet]
        public ActionResult<IEnumerable<Celeb>> GetCelebs(int gameId)
        {
            return _gameService.GetGameById(gameId).Celebs;
        }


        [Route("games")]
        [HttpPost]
        public ActionResult<Game> PostGame(Game game)
        {
            var createdGame = _gameService.CreateGame(game);

            return CreatedAtAction("GetGame", new { id = createdGame.Id }, createdGame);
        }


        [Route("games/{gameId:int}")]
        [HttpDelete]
        public ActionResult<Game> DeleteGame(int gameId)
        {
            return _gameService.DeleteGame(gameId);
        }

        private bool GameExists(int id)
        {
            return _gameService.GetGameById(id) != null;
        }


    }
}
