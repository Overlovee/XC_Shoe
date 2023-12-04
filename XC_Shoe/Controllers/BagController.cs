using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;

namespace XC_Shoe.Controllers
{
    public class BagController : Controller
    {
        // GET: Bag
        public ActionResult ShowBagPage(string userID = "US3")
        {
            ConnectBag connectBag = new ConnectBag();
            List<Bag> bagList = connectBag.getBagData(userID);
            return View(bagList);
        }
    }
}