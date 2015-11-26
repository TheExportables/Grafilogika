using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Grafilogika.Models;

namespace Grafilogika.Controllers {
    [Authorize]
    public class HomeController : Controller {
        public ActionResult Index() {

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(user.UserName, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "A bejelentkezési adatok nem megfelelőek");
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginGuest()
        {
            var guestUser = DBManager.GetUserByName("Vendég");
            if(guestUser == null)
            {
                try
                {
                    DBManager.AddUser("Vendég", "", 0);
                    FormsAuthentication.SetAuthCookie("Vendég", false);
                    return RedirectToAction("Index", "Home");
                }
                catch(MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", e.StatusCode.ToString());
                }
                return RedirectToAction("Login");
            } 
            FormsAuthentication.SetAuthCookie("Vendég", false);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            var allUser = DBManager.GetAllUsers();
            foreach (var item in allUser)
            {
                if (item.Name.Equals(model.UserName))
                {
                    ModelState.AddModelError("nameError","Ilyen nevű felhasználó már létezik");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    DBManager.AddUser(model.UserName, Encoder.Encode(model.Password), 0);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", e.StatusCode.ToString());
                }
            }

            return View(model);
        }

        public ActionResult Browse()
        {
            return RedirectToAction("BrowseController/Browse");
        }

        public ActionResult Upload()
        {
            return RedirectToAction("UploadController/Upload");
        }

        public ActionResult MyProfile()
        {
            if (User.Identity.Name.Equals("Vendég"))
                return RedirectToAction("GuestProfile", "Home");
            var user = DBManager.GetUserByName(User.Identity.Name);
            if (user.Isadmin!=0)
            {
                Session["Isadmin"] = true;
                var userGames = DBManager.GetAllGames();
                return View(userGames);
            }
            else
            {
                Session["Isadmin"] = false;
                var userGames = DBManager.GetGamesByUploader(User.Identity.Name);
                return View(userGames);
            }
        }

        [HttpPost]
        public ActionResult DeleteGame(Games selectedGame)
        {
            DBManager.DeleteGameByName(selectedGame.Name);
            return RedirectToAction("MyProfile", "Home");
        }

        public ActionResult GuestProfile()
        {
            return View();
        }

        public bool IsValid(string _username, string _password)
        {
            Users loginUser = DBManager.GetUserByNameAndPassword(_username, Encoder.Encode(_password));

            if (loginUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
