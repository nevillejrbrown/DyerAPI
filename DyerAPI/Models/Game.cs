using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
namespace DyerAPI.Models
{
    public enum RoundState
    {
        ROUND_READY_TO_START,
        ROUND_IN_PROGRESS,
        ROUND_COMPLETE
    }

    public enum GameRound
    {
        DESCRIBE,
        DESCRIBE_3_WORDS,
        MIME,
        GAME_OVER
    }

    public static class FriendlyStringMethods
    {
        public static string StateString(this RoundState state)
        {
            switch (state)
            {
                case RoundState.ROUND_READY_TO_START: return "Read to start";
                case RoundState.ROUND_IN_PROGRESS: return "Round in progress";
                case RoundState.ROUND_COMPLETE: return "Round complete";
                default: return "Unknown";
            }
        }

        public static string GameRoundString(this GameRound round)
        {
            switch (round)
            {
                case GameRound.DESCRIBE: return "Describe...";
                case GameRound.DESCRIBE_3_WORDS: return "Describe in 3 words...";
                case GameRound.MIME: return "Mime...";
                case GameRound.GAME_OVER: return "Game over";
                default: return "Unknown";
            }
        }
    }

    public class GameStats
    {
        public int NumberInHat { get; set; }
        public int NumberGuessed { get; set; }
        public int NumberBurned { get; set; }
    }

    public class Game
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public List<Celeb> Celebs { get; set; }

        private static Random random = new Random();

        public GameRound Round { get; set; }

        public GameStats GetStats()
        {
            return new GameStats
            {
                NumberInHat = Celebs.Count(c => c.State == CelebState.IN_HAT),
                NumberGuessed = Celebs.Count(c => c.State == CelebState.GUESSED),
                NumberBurned = Celebs.Count(c => c.State == CelebState.BURNED)
            };
        }

        public void NextRound()  
        {
            if (Round < GameRound.GAME_OVER)
            {
                Round++;
            }
            else throw new InvalidOperationException("Game is already over");
        }

        public Game()
        {
            this.Celebs = new List<Celeb>();
            Round = GameRound.DESCRIBE;
        }

 
        public void AddCeleb(Celeb celeb)
        {
            this.Celebs.Add(celeb);
        }

        [NotMapped]
        public RoundState State
        {
            get {
                if (Celebs.All(c => c.State == CelebState.IN_HAT)) return RoundState.ROUND_READY_TO_START;
                return Celebs.All(c => c.State != CelebState.IN_HAT) ? RoundState.ROUND_COMPLETE : RoundState.ROUND_IN_PROGRESS;
            }

        }

        public Celeb GetCelebById(int id)
        {
            return Celebs.Where(c => c.Id == id).Single();
        }

        public IEnumerable<Celeb> getCelebsInHat()
        {
            return Celebs.Where(c => c.State == CelebState.IN_HAT);
        }

        public IEnumerable<Celeb> getGuessedCelebs()
        {
            return Celebs.Where(c => c.State == CelebState.GUESSED);
        }

        public Celeb GetRandomCelebFromHat()
        {
            return getCelebsInHat().GetRandom<Celeb>(random) ;
        }



    }

    static class RandomizerExtensions
    {
        public static Celeb GetRandom<T>(this IEnumerable<Celeb> enumerable, Random random)
        {
            return enumerable.Count() > 0 ?
                enumerable.ElementAt(random.Next(0, enumerable.Count())) :
                null;
        }
    }
}
