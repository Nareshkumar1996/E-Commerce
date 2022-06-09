using ECart.ViewModels;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using ECart.Data;

namespace ECart.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController()
        {

        }
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            
            var user = _unitOfWork.GetEntities<UserTable>().FirstOrDefault(m => m.Email == loginModel.Email & m.Password == encryptedPassword);
            
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
            
            _unitOfWork.Add(user);
            _unitOfWork.Commit();
            return Redirect(returnUrl);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}