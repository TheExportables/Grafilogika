using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                if (!Directory.Exists(Server.MapPath("~/Upload")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Upload"));
                }
                file.SaveAs(path);
            }

            //TODO ascii feldolgozás

            //TODO adatbázisba mentés

            //TODO upload mappa törlése

            return View();
        }
    }
}