using Grafilogika.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult CreateOwnGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOwnGameProcess()
        {
            return View();
        }

        public ActionResult UploadGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadGameProcess(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0) {
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
                StringBuilder game = new StringBuilder();

                System.IO.StreamReader txtfile = new System.IO.StreamReader(path);
                while ((line = txtfile.ReadLine()) != null) {
                    if (counter == 1) {
                        length = line.Length;
                        //TODO length > mint a max pályaméret, akkor error fuck you, legyen kevesebb oszlop, max X
                    } else {
                        if (line.Length != length) {
                            //TODO nem egyenlők a sorok hosszai bazeg error 1. és counteredik sor nem egyenlő hosszú
                        }
                        //TODO if counter > max 
                        if(true){
                            //TODO error több sor van mint a max pályaméret, legyen kevesebb sor, max Y
                        }
                    }
                    for (int i = 0; i < line.Length; ++i) {
                        if(line[i].ToString().ToUpper().Equals("X")) {
                            game.Append("1");
                        } else if (line[i].ToString().Equals(" ")) {
                            game.Append("0");
                        } else {
                            //TODO wrong characters in ascii file error üzenet, száll a szélben
                        }
                    }
                    game.Append(";");
                    ++counter;
                }
            }

            //TODO adatbázisba mentés
            //feltöltő user neve, Játék neve, win/lose/rating default =0, StringBuilder game változó toString

            using (var dbCtx = new GrafilogikaDBEntities())
            {
                Games addthis = new Games();
                addthis.Name = "Testgame";
                addthis.Uploader = "Tarotth";
                addthis.Game = "1234";
                dbCtx.Games.Add(addthis);

                dbCtx.SaveChanges();
            }

            if (!Directory.Exists(Server.MapPath("~/Upload"))) {
                Directory.Delete(Server.MapPath("~/Upload"));
            }

            return View();
        }
    }
}