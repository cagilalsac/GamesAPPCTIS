using APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.Session.MVC;

namespace APP.Services
{
    public class FavoriteService : IFavoriteService
    {
        const string SESSIONKEY = "favorites";

        private readonly SessionServiceBase _sessionService;

        private readonly IService<GameRequest, GameResponse> _gameService;

        public FavoriteService(SessionServiceBase sessionService, IService<GameRequest, GameResponse> gameService)
        {
            _sessionService = sessionService;
            _gameService = gameService;
        }

        public void AddToFavorites(int userId, int gameId)
        {
            var favorites = GetFavorites(userId);
            favorites.Add(new FavoriteItem
            {
                GameId = gameId,
                UserId = userId,
                GameName = _gameService.Item(gameId)?.Title
            });
            _sessionService.SetSession(SESSIONKEY, favorites);
        }

        public void ClearFavorites(int userId)
        {
            var favorites = GetFavorites(userId);
            favorites.RemoveAll(f => f.UserId == userId);
            _sessionService.SetSession(SESSIONKEY, favorites);
        }

        public List<FavoriteItem> GetFavorites(int userId)
        {
            var favorites = _sessionService.GetSession<List<FavoriteItem>>(SESSIONKEY) ?? new List<FavoriteItem>();
            return favorites.Where(f => f.UserId == userId).ToList();
        }

        public void RemoveFromFavorites(int userId, int gameId)
        {
            var favorites = GetFavorites(userId);
            var favorite = favorites.FirstOrDefault(f => f.GameId == gameId);
            if (favorite is not null)
                favorites.Remove(favorite);
            _sessionService.SetSession(SESSIONKEY, favorites);
        }
    }
}
