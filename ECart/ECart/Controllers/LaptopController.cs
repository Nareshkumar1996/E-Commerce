using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECart.Controllers
{

    public static class InitializeOnce
    {
        
       public static AddtocartViewModel addtocartViewModel = new AddtocartViewModel();
       public static List<CartTable> cartTables = new List<CartTable>();
       public static Dictionary<int, int> map = new Dictionary<int, int>();
    }    
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
        public ActionResult AddToShoppingCart(int laptopid)
        {
            var laptop = eCart.Laptops.Find(laptopid);

            if (InitializeOnce.map.ContainsKey(laptopid))
            {
                int value;
                InitializeOnce.map.TryGetValue(laptopid, out value);
                InitializeOnce.map[laptopid] = value + 1;

                foreach (var cart in InitializeOnce.cartTables)
                {
                    if (cart.LaptopName == laptop.LaptopName)
                    {
                        cart.SelectedQuantity = InitializeOnce.map[laptopid];
                        InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;
                    }
                }
            }
            else {
                int initialcount = 1;
                InitializeOnce.map.Add(laptopid, initialcount);

                CartTable cart = new CartTable();
                cart.SelectedQuantity = InitializeOnce.map[laptopid];
                cart.LaptopName = laptop.LaptopName;
                cart.Price = laptop.Price;

                InitializeOnce.cartTables.Add(cart);
                InitializeOnce.addtocartViewModel.CartTables = InitializeOnce.cartTables;
                InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;
            }
             
            return View(InitializeOnce.addtocartViewModel);
        }
        public ActionResult AllLaptops()
        {
            List<Laptop> laptops = eCart.Laptops.ToList();
            LaptopViewModel laptopViewModel = new LaptopViewModel();
            laptopViewModel.Laptops = laptops;
            return View("List",laptopViewModel);
        }
    }
}