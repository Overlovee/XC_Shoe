using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class OrderDetails
    {
        public string OrderID { get; set; }
        public string ShoesID { get; set; }
        public int Quantity { get; set; }
        public int Size { get; set; }
        public string StyleType { get; set; }
        public string ProductName { get; set; }
        public string ColourName { get; set; }
        public int ColourID { get; set; }
        public decimal Price { get; set; }
    }
}