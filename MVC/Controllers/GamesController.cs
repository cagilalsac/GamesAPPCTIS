#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    public class GamesController : Controller
    {
        // Service injections:
        private readonly IService<GameRequest, GameResponse> _gameService;
        private readonly IService<PublisherRequest, PublisherResponse> _publisherService;

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        private readonly IService<TagRequest, TagResponse> _tagService;

        public GamesController(
			IService<GameRequest, GameResponse> gameService
            , IService<PublisherRequest, PublisherResponse> publisherService

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            , IService<TagRequest, TagResponse> tagService
        )
        {
            _gameService = gameService;
            _publisherService = publisherService;

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            _tagService = tagService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["PublisherId"] = new SelectList(_publisherService.List(), "Id", "Name");

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            ViewBag.TagIds = new MultiSelectList(_tagService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Games
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _gameService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Games/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _gameService.Item(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Games/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(GameRequest game)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _gameService.Create(game);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(game); // return request as model to the Create view
        }

        // GET: Games/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _gameService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Games/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(GameRequest game)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _gameService.Update(game);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(game); // return request as model to the Edit view
        }

        // GET: Games/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _gameService.Item(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Games/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _gameService.Delete(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
    }
}
