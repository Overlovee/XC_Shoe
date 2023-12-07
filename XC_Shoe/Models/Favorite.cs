using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class Favorite
    {
        public int favoriteID { get; set; }
        public string ShoesID { get; set; }
        public int TypeShoesID { get; set; }
        public string NameShoes { get; set; }
        public string StyleType { get; set; }
        public string TypeName { get; set; }
        public int Number_Colour { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }
        public string ColorName { get; set; }

    }
}