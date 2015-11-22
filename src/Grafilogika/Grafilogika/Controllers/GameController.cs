using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers
{
    public class GameController : Controller
    {
        public ActionResult Game()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckGameSolution(String gameName, String gameSolution, String userName) {

            var game = DBManager.GetGameByName(gameName);
            var user = DBManager.GetUserByName(userName);
            if (gameSolution.Equals(game.Game))
                return View(); //TODO Wineltél
            else {
                game.Mistakes++;
                user.Mistakes++;
                //TODO Update
                return View(); //TODO Elrontottad
            }
        }

        [HttpPost]
        public ActionResult CheckGameSolution(String gameName, String gameSolution) {

            var game = DBManager.GetGameByName(gameName);
            if (gameSolution.Equals(game.Game))
                return View(); //TODO Wineltél
            else {
                game.Mistakes++;

                return View(); //TODO Elrontottad
            }
        }

        [HttpPost]
        public ActionResult PostGameRating(String gameName, int rating, String userName) {

            var game = DBManager.GetGameByName(gameName);
            var user = DBManager.GetUserByName(userName);

            game.Wins++;
            game.Rating += rating;
            game.Rating /= game.Wins;
            //TODO update game

            user.Wins++;
            //TODO update user

            return View(); //TODO thxforratingbics
        }

        [HttpPost]
        public ActionResult PostGameRating(String gameName, int rating) {

            var game = DBManager.GetGameByName(gameName);

            game.Wins++;
            game.Rating += rating;
            game.Rating /= game.Wins;
            //TODO update game

            return View(); //TODO thxforratingbics
        }
    }
}