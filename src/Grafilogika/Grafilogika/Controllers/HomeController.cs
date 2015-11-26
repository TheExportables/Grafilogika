using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Grafilogika.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

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
                    var u = DBManager.GetUserByName(user.UserName);
                    if (u.Isverified != 0) { 
                        FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Az e-mail címed még nincs verifikálva!");
                    }

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
                    DBManager.AddUser("Vendég", "", 0, "", 0);
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
        public async Task<ActionResult> Register(RegisterModel model)
        {
            var allUser = DBManager.GetAllUsers();
            foreach (var item in allUser)
            {
                if (item.Name.Equals((model.UserName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ModelState.AddModelError("nameError","Ilyen nevű felhasználó már létezik");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    DBManager.AddUser(model.UserName, Encoder.Encode(model.Password), 0, model.Email, 0);
                    var body = "<p>Kedves {0}!</p><p>Köszönjük, hogy regisztráltál a Grafilogika játékunkra! Az alábbi linkre kattintva érvényesítheted e-mail címed, és kezdheted a játékot!</p><p>{1}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.Email));
                    message.Subject = "Grafilogika Email cím érvényesítése";
                    string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
                    string verifyUrl = "/Home/Verify?Name=" + model.UserName.ToString();
                    string fullPath = urlBase + verifyUrl;
                    message.Body = string.Format(body, model.UserName, fullPath);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);
                        return RedirectToAction("VerificationSent", "Home");
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", e.StatusCode.ToString());
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Verify(string Name)
        {
            DBManager.UpdateUserVerification(Name);
            return View();
        }

        [AllowAnonymous]
        public ActionResult VerificationSent()
        {
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

        public ActionResult MyProfile()
        {
            if (User.Identity.Name.Equals("Vendég"))
                return RedirectToAction("GuestProfile", "Home");
            var user = DBManager.GetUserByName(User.Identity.Name);
            ProfileModel pm = new ProfileModel();
            if (user.Isadmin!=0)
            {
                Session["Isadmin"] = true;
                var userGames = DBManager.GetAllGames();
                pm.games = userGames;
                return View(pm);
            }
            else
            {
                Session["Isadmin"] = false;
                var userGames = DBManager.GetGamesByUploader(User.Identity.Name);
                pm.games = userGames;
                return View(pm);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsValid(User.Identity.Name, model.OldPassword))
                {
                    DBManager.UpdateUserPassword(User.Identity.Name, Encoder.Encode(model.ConfirmPassword));
                    ViewBag.Success = "Jelszó sikeresen megváltoztatva!";
                    return Json("Jelszó sikeresen megváltoztatva!");
                }
                else
                {
                    return Json("Az eredeti jelszó hibás!");
                }
            }
            else if (model.OldPassword == null || model.OldPassword == "")
            {
                return Json("Nem adtad meg a régi jelszavad!");
            }
            else if (model.NewPassword == null || model.NewPassword == "")
            {
                return Json("Nem adtál meg új jelszót!");
            }
            else if (model.ConfirmPassword == null || model.ConfirmPassword == "")
            {
                return Json("Nem adtál meg ellenőrző jelszót!");
            }
            else if (model.NewPassword.Length < 6)
            {
                    return Json("A megadott jelszó túl rövid!");
            }
            else {
                return Json("Hiba!");
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

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string userName)
        {
            if (userName == null || userName == "")
            {
                ViewBag.Message = "Írd be a felhasználóneved!";
                return View();
            }

            var user = DBManager.GetUserByName(userName);
            if (user.Isverified != 0)
            {
                var body = "<p>Kedves {0}!</p><p>Sikeresen resetelted a régi jelszavadat!</p><p>Az új jelszó: {1}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress(user.Email));
                message.Subject = "Grafilogika új jelszó kérése";
                Random rnd = new Random();
                int num = rnd.Next(0, 999999);
                string code = Encoder.Encode(userName);
                string newpass = Encoder.Encode(num + code);
                message.Body = string.Format(body, user.Name, newpass);
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    await smtp.SendMailAsync(message);
                }
                DBManager.UpdateUserPassword(user.Name, Encoder.Encode(newpass));
                ViewBag.Message = "Elküldtük a jelszót a regisztrációkor verifikált e-mail címre!";
                return View();
            }
            else
            {
                ViewBag.Message = "A felhasználónévhez regisztrált e-mail cím nincs verifikálva!";
                return View();
            }
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
