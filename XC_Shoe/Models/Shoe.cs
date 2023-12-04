using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class Shoe
    {
        public string IconID { get; set; }
        public string ShoesID { get; set; }
        public int TypeShoesID { get; set; }
        public string NameShoes { get; set; }
        public string StyleType { get; set; }
        public string TypeShoesName { get; set; }
        public int NumberColor { get; set; }
        public string NameColor { get; set; }
        public float Price { get; set; }
        public float Discount { get; set; }
        public string Url { get; set; }
    }
}