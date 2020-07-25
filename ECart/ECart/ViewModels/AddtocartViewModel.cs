using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECart.ViewModels
{
    public class AddtocartViewModel
    {
        public List<CartTable> CartTables { get; set; }
        public int Subtotal { get; set; }
    }
    public class CartTable
    {
        public int SelectedQuantity { get; set; }
        public string LaptopName { get; set; }
        public Nullable<int> Price { get; set; }       
    }
}