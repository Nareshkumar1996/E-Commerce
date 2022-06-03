﻿using ECart.ViewModels;
using System.Linq;
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
        [HttpGet]
        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string ReturnUrl)
        {
            string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(loginModel.Password, "SHA1");           
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            ECartEntities eCart = new ECartEntities();
            var user = eCart.UserTables.FirstOrDefault(m => m.Email == loginModel.Email & m.Password == EncryptedPassword);
            
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return Redirect(ReturnUrl);
            }
            AccountViewModel accountViewModel = new AccountViewModel();
            return View(accountViewModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string ReturnUrl)
        {
            ReturnUrl = ReturnUrl ?? Url.Content("~/");
            string EncryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(registerModel.Password, "SHA1");
            UserTable user = new UserTable() {
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.PhoneNumber,
                Email = registerModel.Email,
                Password = EncryptedPassword
            };
            ECartEntities eCart = new ECartEntities();
            eCart.UserTables.Add(user);            
            eCart.SaveChanges();
            return Redirect(ReturnUrl);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}