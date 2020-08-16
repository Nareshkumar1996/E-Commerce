using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECart.ViewModels
{
    public class CheckoutViewModel
    {
        public ShippingModel ShippingModel { get; set; }
        public int TotalSelectedQuantity { get; set; }
        public int SubTotal { get; set; }
        public static IEnumerable<SelectListItem> GetState()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states.Add(new SelectListItem
            {
                Value = "TamilNadu",
                Text = "TamilNadu"
            });
            states.Add(new SelectListItem
            {
                Value = "Kerala",
                Text = "Kerala"
            });
            states.Add(new SelectListItem
            {
                Value = "Karnataka",
                Text = "Karnataka"
            });
            return states;
        }
        public static IEnumerable<SelectListItem> GetPaymentMethod()
        {
            List<SelectListItem> payment = new List<SelectListItem>();
            payment.Add(new SelectListItem
            {
                Value = "Debit Card",
                Text = "Debit Card"
            });
            payment.Add(new SelectListItem
            {
                Value = "Credit Card",
                Text = "Credit Card"
            });
            payment.Add(new SelectListItem
            {
                Value = "Cash On Delivery",
                Text = "Cash On Delivery"
            });
            
            return payment;
        }
    }
    public class ShippingModel
    {
        public string Address { get; set; }
        public string Locality { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Pincode { get; set; }
        public string PaymentMethod { get; set; }

    }

    
}