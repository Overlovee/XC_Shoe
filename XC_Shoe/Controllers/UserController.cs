using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;

namespace XC_Shoe.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        //GoogleDriveAPI googleDriveAPI = new GoogleDriveAPI();
        //public async Task<ActionResult> Index()
        //{
        //    return View();
        //}
        public User user { get; set; }
        public ActionResult ShowHomePage()
        {
            return View();
        }

        public ActionResult ShoesPage(string gender)
        {
            ConnectShoes connectShoes = new ConnectShoes();
            List<Shoe> ListShoes = connectShoes.GetRepresentData(gender);
            return View(ListShoes);
        }
        public ActionResult ShowShoesDetail(string shoesID, String colourName)
        {
            ConnectShoes connectShoes = new ConnectShoes();
            ConnectSize connectSize = new ConnectSize();
            Shoe shoes = connectShoes.getShoesDetailData(shoesID, colourName);
            List<Size> SizeList = connectSize.getSizeShoesData();
            List<Shoe> shoesList = connectShoes.getShoesByShoesIDData(shoesID);
            ViewBag.UserID = "US3";
            ViewBag.ShoesColor = shoesList;
            ViewBag.size = SizeList;
            return View(shoes);
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        public ActionResult SignIn(string email, string password)
        {
            return View();
        }
        public ActionResult UserProfile(String Email = "hoang2011@gmail.com")
        {
            ConnectUsers connectUser = new ConnectUsers();
            User User = connectUser.getUserData(Email);
            List<UserShipment> List = connectUser.getUserShipmentDetails(User.UserID);
            ViewBag.UserShipment = List;
            return View(User);
        }
    }
}