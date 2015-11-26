using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        public ActionResult Game(String gameName)
        {
            if (gameName == null)
                return RedirectToAction("Browse", "Browse");
            Games currentGame = DBManager.GetGameByName(gameName);

            double actualRating = 0;
            if (currentGame.Rating != null && currentGame.Wins != null && currentGame.Wins != 0)
            {
                actualRating = Math.Round(((double)currentGame.Rating / (double)currentGame.Wins), 2);
            }

            Session["Gamestring"] = currentGame.Game;
            Session["Gamename"] = currentGame.Name;
            Session["Uploadername"] = currentGame.Uploader;
            Session["Rating"] = actualRating;
            Session["Mistakes"] = currentGame.Mistakes;
            Session["Wins"] = currentGame.Wins;
            return View();
        }

        public ActionResult Rating(string gameName)
        {
            if (gameName == null)
                return RedirectToAction("Browse", "Browse");
            return View();
        }

        [HttpPost]
        public PartialViewResult CheckGameSolution(String gameName, String gameSolution, String userName)
        {

            var game = DBManager.GetGameByName(gameName);
            var user = DBManager.GetUserByName(userName);
            if (gameSolution.Equals(game.Game))
            {
                DBManager.UpdateGameWins(game);
                DBManager.UpdateUserWins(user);
                return PartialView("_Rating");
            }
            else
            {
                DBManager.UpdateGameMistakes(game);
                DBManager.UpdateUserMistakes(user);
                Session["Mistakes"] = game.Mistakes;
                return PartialView("_Mistake");
            }
        }

        [HttpPost]
        public ActionResult PostGameRating(String gameName, String rating)
        {
            var game = DBManager.GetGameByName(gameName);

            if (rating != "")
            {
                int rate = int.Parse(rating);
                DBManager.UpdateGameRating(game, rate);
            }

            return Json("asd");
        }
    }
}