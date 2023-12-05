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
        public ActionResult ShowBagPage(string userID = "")
        {

            List<Bag> List = new List<Bag>();
            if (userID != "")
            {
                ConnectBag connectBag = new ConnectBag();
                List = connectBag.getBagData(userID);
            }
            else
            {

            }
            return View(List);
        }
    }
}