using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers
{
    [Authorize]
    public class BrowseController : Controller
    {
        public ActionResult Browse()
        {
            List<Games> allGame = DBManager.GetAllGames();
            return View(allGame);
        }

        public JsonResult GetGames()
        {
            var allGame = DBManager.GetAllGames();
            var games = (from g in allGame
                             select new
                             {
                                 g.Name,
                                 g.Uploader,
                                 g.Wins,
                                 g.Mistakes,
                                 g.Rating,
                                 g.Game
                             });
            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LoadGame (Games selectedGame)
        {
            //Games selectedGame = DBManager.GetGameByName(name);

            return RedirectToAction("Game", "Game", new { gameName = selectedGame.Name});
        }
    }
}