using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECart.Controllers
{
    public class OrderController : Controller
    {
        ECartEntities1 eCart = new ECartEntities1();
        public ActionResult Index()
        {
            return View();
        }
        
    }
}