using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyerAPI.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DyerAPI.Models.Service
{
    public class GameService : IGameService
    {

        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PutAllGuessedCelebsBackInHat(Game game)
        {
            foreach (Celeb celeb in game.getGuessedCelebs())
            {
                celeb.PutBackIntoHat();
                //_context.Celeb.Update(celeb);
            }
        }

        public Game MoveGameToNextRound(int GameID)
        {
            Game game = GetGameById(GameID);
            PutAllGuessedCelebsBackInHat(game);
            game.NextRound();
            _context.Game.Update(game);
            _context.SaveChanges();
            return game;
        }

        public List<Game> GetAllGames()
        {
            return _context.Game.ToList();
        }

        public Game GetGameById(int Id)
        {
            return _context.Game.Include(game => game.Celebs)
                                .Single(g => g.Id == Id);
        }

        public Game CreateGame(Game game)
        {
            _context.Game.Add(game);
            _context.SaveChanges();
            return game;
        }

        public Game DeleteGame(int id)
        {
            var game = _context.Game.Find(id);
            if (game == null)
            {
                return null; // yikes! hide error
            }

            _context.Game.Remove(game);
            _context.SaveChanges();
            return game;
        }

        public Game GetGameByCelebId(int CelebId)
        {
            Celeb celeb = _context.Celeb.Single(c => c.Id == CelebId);
            return _context.Game.Include(g => g.Celebs)
                                .Single(g => g.Id == celeb.GameId);
        }

        public void UpdateGame(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;

            _context.SaveChanges();

        }


        public void CelebGuessed(int celebId)
        {
            Celeb celeb = _context.Celeb.Single(c => c.Id == celebId);
            celeb.Guess();
            _context.Celeb.Update(celeb);
            _context.SaveChanges();
        }

        public void CelebBurned(int celebId)
        {
            Celeb celeb = _context.Celeb.Single(c => c.Id == celebId);
            celeb.Burn();
            _context.Celeb.Update(celeb);
            _context.SaveChanges();
        }

        public Celeb AddCeleb(Celeb celeb)
        {
            _context.Celeb.Add(celeb);
            _context.SaveChanges();
            return celeb;
        }

        public Celeb GetCeleb(int id)
        {
            return _context.Celeb.Single(c => c.Id == id);
        }
    }
}
