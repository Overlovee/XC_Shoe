using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class Bag
    {
        public string ShoesID { get; set; }
        public string ShoesName { get; set; }
        public string StyleType { get; set; }
        public string TypeName { get; set; }
        public string ColorName { get; set; }
        public int Size { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public bool BuyingSelectionStatus { get; set; }
        public string Url { get; set; }
    }
}