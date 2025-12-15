using APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        public int GetUserId() => Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value); 

        public IActionResult Index()
        {
            return View(_favoriteService.GetFavorites(GetUserId()));
        }

        public IActionResult AddToFavorites(int gameId)
        {
            _favoriteService.AddToFavorites(GetUserId(), gameId);
            return RedirectToAction("Index", "Games");
        }

        public IActionResult ClearFavorites()
        {
            _favoriteService.ClearFavorites(GetUserId());
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromFavorites(int gameId)
        {
            _favoriteService.RemoveFromFavorites(GetUserId(), gameId);
            return RedirectToAction(nameof(Index));
        }
    }
}
