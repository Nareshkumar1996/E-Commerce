using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECart.ViewModels
{
    public class LaptopViewModel
    {
        public List<Laptop> Laptops { get; set; }
        public List<LaptopCategory> LaptopCategories { get; set; }
        public class DetailsViewModel
        {
            public Laptop Laptop { get; set; }
        }        

    }
}