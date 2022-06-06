using ECart.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ECart.Controllers
{
    public class AccountController : Controller
    {
        private readonly ECartEntities _eCartEntities;

        public AccountController()
        {
            
        }
        public AccountController(ECartEntities eCartEntities)
        {
            _eCartEntities = eCartEntities;
        }
 
        [HttpGet]
        public ActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel loginModel, string returnUrl)
        {
            var encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(loginModel.Password, "SHA1");           
            returnUrl = returnUrl ?? Url.Content("~/");
            
            var user = _eCartEntities.UserTables.FirstOrDefault(m => m.Email == loginModel.Email & m.Password == encryptedPassword);
            
            if(user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return Redirect(returnUrl);
            }
            var accountViewModel = new AccountViewModel();
            return View(accountViewModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(registerModel.Password, "SHA1");
            var user = new UserTable() {
                UserName = registerModel.UserName,
                PhoneNumber = registerModel.PhoneNumber,
                Email = registerModel.Email,
                Password = encryptedPassword
            };
            
            _eCartEntities.UserTables.Add(user);            
            _eCartEntities.SaveChanges();
            return Redirect(returnUrl);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}