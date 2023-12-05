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
        public string userID { get; set; }
        public BagController()
        {
            userID = "";
        }
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

        [HttpPost]
        public ActionResult AddToBag(string shoesId = "", string colour = "")
        {
            if(userID != "")
            {

            }
            // Trả về một JSON object để xử lý ở phía client
            return Json(new { success = true, message = "Added " + shoesId + " Colour: " + colour +" to cart successfully" + " for "});
        }
    }
}