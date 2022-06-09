using ECart.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ECart.Data;
using Microsoft.Ajax.Utilities;
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
        private readonly IUnitOfWork _unitOfWork;
        public LaptopController()
        {

        }

        public LaptopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            var categories = _unitOfWork.GetEntities<LaptopCategory>();
            return View(categories);
        }
        public ActionResult NavBar()
        {            
            var categories = _unitOfWork.GetEntities<LaptopCategory>().ToList();
            var laptopViewModel = new LaptopViewModel
            {
                LaptopCategories = categories
            };
            return PartialView(laptopViewModel);
        }
        
        public ActionResult List(int categoryId)
        {
           
           var laptops = _unitOfWork.GetEntities<Laptop>().Where(c => c.LapCategoryId == categoryId).ToList();
            var laptopView = new LaptopViewModel
            {
                Laptops = laptops
            };
            return View(laptopView);
        }
        public ActionResult Details(int laptopId)
        {
            var laptop = _unitOfWork.GetEntities<Laptop>().FirstOrDefault(l => l.LaptopId == laptopId);
            var detailsViewModel = new DetailsViewModel()
            {
                Laptop = laptop
            };
            return View(detailsViewModel);
        }
        public ActionResult AddToShoppingCart(int laptopId)
        {
            //Adding laptopId to the map if the item is new to cart
            //In If condition checking the laptop id is present in the map, if present im only changing the quantity and row subtotal
            var laptop = _unitOfWork.GetEntities<Laptop>().Find(laptopId);

            if (InitializeOnce.map.ContainsKey(laptopId))
            {
                int value;
                InitializeOnce.map.TryGetValue(laptopId, out value);
                InitializeOnce.map[laptopId] = value + 1;

                foreach (var cart in InitializeOnce.cartTables)
                {
                    if (cart.LaptopName == laptop.LaptopName)
                    {
                        cart.SelectedQuantity = InitializeOnce.map[laptopId];
                        cart.RowSubtotal = cart.RowSubtotal + (int)laptop.Price;
                        InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;

                        //Adding subtotal to checkout page
                        InitializeOnce.checkout.SubTotal = InitializeOnce.addtocartViewModel.Subtotal;
                    }
                }
            }
            else 
            {
                int initialcount = 1;
                InitializeOnce.map.Add(laptopId, initialcount);

                var cart = new CartTable
                {
                    SelectedQuantity = InitializeOnce.map[laptopId],
                    LaptopName = laptop.LaptopName,
                    Price = laptop.Price,
                    RowSubtotal = (int)laptop.Price
                };

                InitializeOnce.cartTables.Add(cart);
                InitializeOnce.addtocartViewModel.CartTables = InitializeOnce.cartTables;
                InitializeOnce.addtocartViewModel.Subtotal = InitializeOnce.addtocartViewModel.Subtotal + (int)laptop.Price;

                //Adding subtotal to checkout page
                InitializeOnce.checkout.SubTotal = InitializeOnce.addtocartViewModel.Subtotal;
            }
             
            return View(InitializeOnce.addtocartViewModel);
        }
        public ActionResult AllLaptops()
        {
            var laptops = _unitOfWork.GetEntities<Laptop>().ToList();
            var laptopViewModel = new LaptopViewModel
            {
                Laptops = laptops
            };
            return View("List",laptopViewModel);
        }

        [Authorize]        
        public ActionResult Checkout()
        {
            foreach (var total in InitializeOnce.cartTables)
            {
                InitializeOnce.checkout.TotalSelectedQuantity = InitializeOnce.checkout.TotalSelectedQuantity + total.SelectedQuantity;
            }

            InitializeOnce.checkout.ShippingModel = new ShippingModel();
            var username = User.Identity.Name;
            var user = _unitOfWork.GetEntities<UserTable>().FirstOrDefault(u => u.UserName == username);
            ViewBag.DeliveryAddress = user.ShippingAddress;
            return View(InitializeOnce.checkout);
        }
        [Authorize]
        public ActionResult PlaceOrder(CheckoutViewModel checkoutViewModel)
        {
            var name = User.Identity.Name;
            var user = _unitOfWork.GetEntities<UserTable>().FirstOrDefault(m => m.UserName == name);

            if (!checkoutViewModel.ShippingModel.Address.IsNullOrWhiteSpace())
            {
                var shippingAddress = checkoutViewModel.ShippingModel.Address + "," +
                                      checkoutViewModel.ShippingModel.Locality + "," +
                                      checkoutViewModel.ShippingModel.City + "," +
                                      checkoutViewModel.ShippingModel.State + "-" +
                                      checkoutViewModel.ShippingModel.Pincode + ",";
                user.ShippingAddress = shippingAddress;
            }
            
            var order = new OrderDetail
            {
                UserId = user.UserId,
                TotalItems = InitializeOnce.checkout.TotalSelectedQuantity,
                TotalPrice = InitializeOnce.checkout.SubTotal,
                DateTime = DateTime.Now
            };
            _unitOfWork.Add(order);
            _unitOfWork.Commit();
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