using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECart.Controllers
{
    public class LaptopController : Controller
    {
        ECartEntities1 eCart = new ECartEntities1();
        public ActionResult Index()
        {
            var categories = eCart.LaptopCategories;
            return View(categories);
        }
        public ActionResult NavBar()
        {
            
            List<LaptopCategory> categories = eCart.LaptopCategories.ToList();
            LaptopViewModel laptopViewModel = new LaptopViewModel();
            laptopViewModel.LaptopCategories = categories;
            return PartialView(laptopViewModel);
        }
        
        public ActionResult List(int categoryid)
        {
           
            List<Laptop> laptops = eCart.Laptops.Where(c => c.LapCategoryId == categoryid).ToList();
            LaptopViewModel laptopView = new LaptopViewModel();
            laptopView.Laptops = laptops;
            return View(laptopView);
        }
    }
}