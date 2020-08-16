using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECart.ViewModels
{
    public class AccountViewModel
    {
        public LoginModel LoginModel { get; set; }
        public RegisterModel RegisterModel { get; set; }      

    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }        
    }
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
    }
}