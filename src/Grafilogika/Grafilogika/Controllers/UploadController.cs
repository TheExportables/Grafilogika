using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        public ActionResult Upload()
        {
            if (User.Identity.Name.Equals("Vendég"))
                return RedirectToAction("GuestProfile", "Home");
            return View();
        }

        public ActionResult CreateOwnGame()
        {
            if (User.Identity.Name.Equals("Vendég"))
                return RedirectToAction("GuestProfile", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult CreateOwnGameProcess(String gameName, String gameSolution)
        {
            var everyGame = DBManager.GetAllGames();
            foreach (var item in everyGame)
            {
                if (item.Name.Equals(gameName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Json("Ezzel a névvel már létezik játék. Válassz másik nevet!");
                }
            }
            if (gameName == "")
            {
                return Json("Nem adtál meg nevet a játékodnak!");
            }
            if (gameName != "")
            {
                DBManager.AddGame(gameName, User.Identity.Name, gameSolution);
                return Json("Sikeres feltöltés.");
            }
            return Json("Hiba történt! A játék feltöltése nem sikerült.");
        }

        public ActionResult UploadGame()
        {
            if (User.Identity.Name.Equals("Vendég"))
                return RedirectToAction("GuestProfile", "Home");
            return View();
        }

        public ActionResult UploadGameProcess()
        {
            return View("UploadGameProcess");
        }

        [HttpPost]
        public ActionResult UploadGame(HttpPostedFileBase file, String gameName)
        {
            StringBuilder game = null;
            if (file != null && file.ContentLength > 0)
            {
                if (file.ContentType != "text/plain")
                {
                    ViewBag.Error = "A kiválasztott fájl nem txt.";
                    return View("UploadGame");
                }
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Upload")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Upload"));
                }
                file.SaveAs(path);
                //ASCII feldolgozás
                int length = 0;
                int counter = 1;
                string line;
                game = new StringBuilder();

                System.IO.StreamReader txtfile = new System.IO.StreamReader(path);
                while ((line = txtfile.ReadLine()) != null)
                {
                    if (counter == 1)
                    {
                        length = line.Length;
                        if (length > 60)
                        {
                            txtfile.Close();
                            if (!Directory.Exists(Server.MapPath("~/Upload")))
                            {
                                Directory.Delete(Server.MapPath("~/Upload"));
                            }
                            ViewBag.Error = "Túl nagy a tábla mérete! 60 egységnél rövidebb sort adj meg!";
                            return View("UploadGame");
                        }
                    }
                    else
                    {
                        if (line.Length != length)
                        {
                            txtfile.Close();
                            if (!Directory.Exists(Server.MapPath("~/Upload")))
                            {
                                Directory.Delete(Server.MapPath("~/Upload"));
                            }
                            ViewBag.Error = "Nem egyenlő hosszúak a sorok! Egyenlő hosszú sorokat adj meg!";
                            return View("UploadGame");
                        }
                        if (counter > 60)
                        {
                            txtfile.Close();
                            if (!Directory.Exists(Server.MapPath("~/Upload")))
                            {
                                Directory.Delete(Server.MapPath("~/Upload"));
                            }
                            ViewBag.Error = "Túl nagy a tábla mérete! 60 sornál kevesebbet adj meg.";
                            return View("UploadGame");
                        }
                    }
                    for (int i = 0; i < line.Length; ++i)
                    {
                        if (line[i].ToString().ToUpper().Equals("X"))
                        {
                            game.Append("1");
                        }
                        else if (line[i].ToString().Equals(" "))
                        {
                            game.Append("0");
                        }
                        else
                        {
                            txtfile.Close();
                            if (!Directory.Exists(Server.MapPath("~/Upload")))
                            {
                                Directory.Delete(Server.MapPath("~/Upload"));
                            }
                            ViewBag.Error = "A fájl nem megfelelő karaktereket tartalmaz.";
                            return View("UploadGame");
                        }
                    }
                    game.Append(";");
                    ++counter;
                }
                //TODO adatbázisba mentés
                //feltöltő user neve, Játék neve, win/lose/rating default =0, StringBuilder game változó toString

                if (gameName != null && gameName != "")
                {
                    var everyGame = DBManager.GetAllGames();
                    foreach (var item in everyGame)
                    {
                        if (item.Name.Equals(gameName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            txtfile.Close();
                            if (!Directory.Exists(Server.MapPath("~/Upload")))
                            {
                                Directory.Delete(Server.MapPath("~/Upload"));
                            }
                            ViewBag.Error = "Ezzel a névvel már létezik játék. Válassz másik nevet!";
                            return View();
                        }
                    }

                    DBManager.AddGame(gameName, User.Identity.Name, game.ToString());
                }
                else
                {
                    txtfile.Close();
                    if (!Directory.Exists(Server.MapPath("~/Upload")))
                    {
                        Directory.Delete(Server.MapPath("~/Upload"));
                    }
                    ViewBag.Error = "Adj meg nevet a fájlhoz.";
                    return View("UploadGame");
                }

                txtfile.Close();
                if (!Directory.Exists(Server.MapPath("~/Upload")))
                {
                    Directory.Delete(Server.MapPath("~/Upload"));
                }

                return View("UploadGameProcess");
            }

            ViewBag.Error = "Nem választottál fájlt!";
            return View();
        }
    }
}