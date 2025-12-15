using APP.Models;

namespace APP.Services
{
    public interface IFavoriteService
    {
        public List<FavoriteItem> GetFavorites(int userId);
        
        public void AddToFavorites(int userId, int gameId);

        
        public void RemoveFromFavorites(int userId, int gameId);

        
        public void ClearFavorites(int userId);
    }
}