using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static ECart.ViewModels.LaptopViewModel;

namespace ECart.Controllers
{

    public static class InitializeOnce
    {
        
       public static AddtocartViewModel addtocartViewModel = new AddtocartViewModel();
       public static List<CartTable> cartTables = new List<CartTable>();
       public static Dictionary<int, int> map = new Dictionary<int, int>();
       public static CheckoutViewModel checkout = new CheckoutViewModel();
       
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
            LaptopViewModel laptopViewModel = new LaptopViewModel
            {
                LaptopCategories = categories
            };
            return PartialView(laptopViewModel);
        }
        
        public ActionResult List(int categoryid)
        {
           
            List<Laptop> laptops = eCart.Laptops.Where(c => c.LapCategoryId == categoryid).ToList();
            LaptopViewModel laptopView = new LaptopViewModel();
            laptopView.Laptops = laptops;
            return View(laptopView);
        }
        public ActionResult Details(int laptopid)
        {
            var laptop = eCart.Laptops.FirstOrDefault(l => l.LaptopId == laptopid);
            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Laptop = laptop
            };
            return View(detailsViewModel);
        }
        public ActionResult AddToShoppingCart(int laptopid)
        {
            //Adding laptopid to the map if the item is new to cart
            //Checking the laptop id is present in the map, if present im only changing the quantity and row subtotal
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
                        cart.RowSubtotal = cart.RowSubtotal + (int)laptop.Price;
                        InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;
                        //Adding total selected quantity and subtotal to checkout page
                        //InitializeOnce.checkout.TotalSelectedQuantity = cart.SelectedQuantity;
                        InitializeOnce.checkout.SubTotal = InitializeOnce.addtocartViewModel.Subtotal;
                    }
                }
            }
            else {
                int initialcount = 1;
                InitializeOnce.map.Add(laptopid, initialcount);

                CartTable cart = new CartTable
                {
                    SelectedQuantity = InitializeOnce.map[laptopid],
                    LaptopName = laptop.LaptopName,
                    Price = laptop.Price,
                    RowSubtotal = (int)laptop.Price
                };

                InitializeOnce.cartTables.Add(cart);
                InitializeOnce.addtocartViewModel.CartTables = InitializeOnce.cartTables;
                InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;
                //Adding total selected quantity and subtotal to checkout page
                //InitializeOnce.checkout.TotalSelectedQuantity = cart.SelectedQuantity;
                InitializeOnce.checkout.SubTotal = InitializeOnce.addtocartViewModel.Subtotal;
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

        [Authorize]        
        public ActionResult Checkout()
        {
            foreach (var total in InitializeOnce.cartTables)
            {
                InitializeOnce.checkout.TotalSelectedQuantity = InitializeOnce.checkout.TotalSelectedQuantity + total.SelectedQuantity;
            }
            var username = User.Identity.Name;
            var user = eCart.UserTables.FirstOrDefault(u => u.UserName == username);
            ViewBag.DeliveryAddress = user.ShippingAddress;
            return View(InitializeOnce.checkout);
        }
        [Authorize]
        public ActionResult PlaceOrder(CheckoutViewModel checkoutViewModel)
        {
            string shippingAddress = checkoutViewModel.ShippingModel.Address + "," +
                checkoutViewModel.ShippingModel.Locality + "," +
                checkoutViewModel.ShippingModel.City + "," +
                checkoutViewModel.ShippingModel.State + "-" +
                checkoutViewModel.ShippingModel.Pincode + ",";


            var name = User.Identity.Name;
            var user = eCart.UserTables.FirstOrDefault(m => m.UserName == name);
            user.ShippingAddress = shippingAddress;

            OrderDetail order = new OrderDetail
            {
                UserId = user.UserId,
                TotalItems = InitializeOnce.checkout.TotalSelectedQuantity,
                TotalPrice = InitializeOnce.checkout.SubTotal,
                DateTime = DateTime.Now
            };
            eCart.OrderDetails.Add(order);
            eCart.SaveChanges();
            return View();
        }
        public ActionResult CartIcon()
        {
            //if(InitializeOnce.addtocartViewModel == null)
            //{
            //    InitializeOnce.addtocartViewModel = null;
            //    return View("AddToShoppingCart", InitializeOnce.addtocartViewModel);
            //}
            return View("AddToShoppingCart",InitializeOnce.addtocartViewModel);
        }
    }
}