using System.Collections.Generic;

namespace DyerAPI.Models.Service
{
    public interface IGameService
    {
        List<Game> GetAllGames();
        Game CreateGame(Game game);
        void UpdateGame(Game game);

        Game DeleteGame(int id);
        Celeb GetCeleb(int id);
        Game GetGameById(int Id);
        Game GetGameByCelebId(int CelebId);
        Celeb AddCeleb(Celeb celeb);
        void CelebBurned(int celebId);
        void CelebGuessed(int celebId);
        Game MoveGameToNextRound(int GameID);
    }
}