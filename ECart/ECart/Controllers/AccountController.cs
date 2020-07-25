using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ECart.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl = null)
        {
            string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(loginModel.Password, "SHA1");
            returnUrl = returnUrl ?? Url.Content("~/");
            ECartEntities1 eCart = new ECartEntities1();
            var user = eCart.UserTables.FirstOrDefault(m => m.Email == loginModel.Email & m.Password == EncryptedPassword);
            
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return Redirect(returnUrl);
            }
            AccountViewModel accountViewModel = new AccountViewModel();
            return View(accountViewModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(registerModel.Password, "SHA1");
            UserTable user = new UserTable() {
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.PhoneNumber,
                Email = registerModel.Email,
                Password = EncryptedPassword
            };
            ECartEntities1 eCart = new ECartEntities1();
            eCart.UserTables.Add(user);            
            eCart.SaveChanges();
            return Redirect(returnUrl);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}