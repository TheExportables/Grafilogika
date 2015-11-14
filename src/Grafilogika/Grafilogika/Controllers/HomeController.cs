using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grafilogika.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Browse()
        {
            return RedirectToAction("BrowseController/Browse");
        }

        public ActionResult Upload()
        {
            return RedirectToAction("UploadController/Upload");
        }
    }
}
