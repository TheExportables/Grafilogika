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
        public ActionResult CheckGameSolution(String gameName, String gameSolution) {

            //TODO játékok adatbázisból get a gameName-el egyenlő nevűt, aztán 
            //if gameSolution.Equals(adatbázisban levő megoldás) akkor
            //return faszavagy; else return elbasztad.

            //adatbázisba mentés a hibát, wint majd az értékeléskor adjuk hozzá

            return View();
        }

        [HttpPost]
        public ActionResult PostGameRating(int rating) {

            //TODO adatbázisból getRating és getWins. ++Wins; Rating+=rating; Rating/=Wins;

            return View();
        }
    }
}
