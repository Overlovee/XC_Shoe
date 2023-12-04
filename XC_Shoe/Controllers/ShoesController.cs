using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;

namespace XC_Shoe.Controllers
{
    public class ShoesController : Controller
    {
        // GET: Shoes
        public ActionResult TypeShoesPartial()
        {
            ConnectTypeShoes connectShoes = new ConnectTypeShoes();
            List<TypeShoes> TypeShoesList = connectShoes.getTypeShoesData();
            return View(TypeShoesList);
        }
        public ActionResult IconsPartial()
        {
            ConnectIcons connectShoes = new ConnectIcons();
            List<Icons> IconList = connectShoes.getIconShoesData();
            return View(IconList);
        }
    }
}