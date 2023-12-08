using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class Purchased
    {
        public string ShoesID { get; set; }
        public string ShoesName { get; set; }
        public string ColorName { get; set; }
        public int ColorID { get; set; }
        public string StyleType { get; set; }
        public string TypeShoesName { get; set; }
        public int TypeShoesID { get; set; }
        public int Size { get; set; }
        public int PurchasedQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string Url { get; set; }
    }
}