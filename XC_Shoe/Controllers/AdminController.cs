using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;

namespace XC_Shoe.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        ConnectShoes connectShoes = new ConnectShoes();
        ConnectShopBrand connectShopBrand = new ConnectShopBrand();
        ConnectUsers connectUsers = new ConnectUsers();
        ConnectOrders connectOrders = new ConnectOrders();
        ConnectTypeShoes connectTypeShoes = new ConnectTypeShoes();
        ConnectPurchased connectPurchased = new ConnectPurchased();
        ConnectColour ConnectColour = new ConnectColour();
        ConnectIcons ConnectIcons = new ConnectIcons();
        User user = new User();
        public AdminController()
        {
            var httpContext = System.Web.HttpContext.Current;
            string userID = "";
            if (httpContext.Request.Cookies["UserLogin"] != null)
            {
                var cookieValue = httpContext.Request.Cookies["UserLogin"].Value;

                userID = httpContext.Request.Cookies["UserLogin"]["UserID"];

            }

            user = connectUsers.getDataByID(userID);
            string resourcesPath = "~/Resources/Account/";
            resourcesPath = Path.Combine(resourcesPath, user.Image);
            ViewBag.ThisAccountImage = resourcesPath;
            ViewBag.UserName = user.UserName;
        }
        public ActionResult Home(string sort = "DESC")
        {
            List<Purchased> list = connectPurchased.getData(sort);
            ViewBag.Title = "Home";
            ViewBag.MainTitle = "Home";
            ViewBag.ThisMonthIncome = connectPurchased.getThisMonthIncome();
            ViewBag.TotalOrder = connectPurchased.getTotalOrder();
            int curOrder = connectPurchased.getTotalOrderByMonth(DateTime.Now.Month);
            int preOrder = connectPurchased.getTotalOrderByMonth(DateTime.Now.Month - 1);
            decimal OGP = 0;
            if (preOrder != 0)
            {
                OGP = (curOrder - preOrder) / preOrder;
            }
            else
            {
                OGP = 100;
            }
            ViewBag.OrderGrowthPercent = Math.Round(OGP, 2);
            ViewBag.TotalIncome = connectPurchased.getTotalIncome();
            return View(list);
        }
        public ActionResult ManageProduct(string styleStyle = "Men", string sort = "ASC", string search = "", int? page = null)
        {
            List<Icons> icons = ConnectIcons.getIconShoesData();
            List<TypeShoes> list1 = connectTypeShoes.getTypeShoesData();
            List<Colours> list2 = ConnectColour.getColourShoesData();
            List<Shoe> list = connectShoes.getShoesDataByStyleType(styleStyle, sort, search);
            //List<Shoe> list3 = connectShoes.GetRepresentData(styleStyle, sort, search);
            ViewBag.Icons = new SelectList(icons, "IconID", "NameIcon");
            ViewBag.lstTypeShoes = new SelectList(list1, "TypeShoesID", "NameTS");
            ViewBag.lstColor = new SelectList(list2, "ColourID", "ColourName");
            ViewBag.ShoesName = new SelectList(list, "ShoesID", "NameShoes");
            ViewBag.Title = "Manage Products";
            ViewBag.MainTitle = "Manage Products";
            ViewBag.Style = styleStyle;
            ViewBag.Sort = sort;
            ViewBag.Total = list.Count;
            ViewBag.SearchValue = search;
            //
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 8;

            //
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ManageUser(string role = "0", string sort = "ASC", string search = "")
        {
            List<User> list = connectUsers.getData(role, sort, search);
            ViewBag.Title = "Manage Users";
            ViewBag.MainTitle = "Manage Users";
            ViewBag.Role = role;
            ViewBag.Sort = sort;
            ViewBag.Total = list.Count;
            ViewBag.SearchValue = search;
            return View(list);
        }
        public ActionResult ManageAdmin(string role = "1", string sort = "ASC", string search = "")
        {
            List<ShopBrand> list1 = connectShopBrand.getShopBrandsData();
            List<User> list = connectUsers.getData(role, sort, search);
            ViewBag.ShopBranches = new SelectList(list1, "ShopID", "ShopBranchAddress");
            ViewBag.Title = "Manage Admins";
            ViewBag.MainTitle = "Manage Admins";
            ViewBag.Role = role;
            ViewBag.Sort = sort;
            ViewBag.SearchValue = search;
            ViewBag.Total = list.Count;
            return View(list);
        }
        public ActionResult MyAccount(User userUpdate = null)
        {
            ViewBag.Title = "My Account";
            ViewBag.MainTitle = user.UserName;
            return View(user);
        }
        public ActionResult ManageOrder(string status = "Wait for confirmation", string sort = "ASC", string search = "")
        {
            List<Order> list = connectOrders.getFullOrderData(status, sort, search);
            ViewBag.Title = "Manage Orders";
            ViewBag.MainTitle = "Manage Orders";
            ViewBag.Sort = sort;
            ViewBag.Status = status;
            ViewBag.SearchValue = search;
            ViewBag.Total = list.Count;
            ViewBag.NumberProductInPage = 0;
            return View(list);
        }
        public ActionResult ConfirmOrder(string Orderid,string userID, string status = "Wait for confirmation", string sort = "ASC", string search = "")
        {
            ViewBag.UserID = user.UserID;
            int rs = connectOrders.ConfirmOrder(Orderid,userID,status);
            return RedirectToAction("ManageOrder");
        }
        [HttpPost]
        public ActionResult AdminProcessSignUp(FormCollection form)
        {

            // Retrieve form data from the collection
            var name = form["name"];
            var email = form["Email"];
            var password = form["Password"];
            var phone = form["Phone"];
            var address = form["Address"];
            var shopBranch = form["ShopBranch"];
            int rs = connectUsers.AddAdmin(name, email, password, phone, address, shopBranch);

            // Process the data as needed, for example, save it to a database

            // Redirect or return a different view as needed
            return RedirectToAction("ManageAdmin"); // Redirect to the Index action, replace with your actual action
        }
        [HttpPost]
        public ActionResult EditShoes(FormCollection form)
        {
            var OldShoesID = form["OldShoesID"];
            var NewShoesID = form["NewShoesID"];
            var TypeShoesID = int.Parse(form["TypeShoes"]);
            var styleType = form["styleType"];
            //var OldColour = form["OldColour"];
            //var newcolour = form["NewColour"];
            var Price = float.Parse(form["Price"]);
            var Discount = float.Parse(form["Discount"]);
            int rs = ConnectColour.EditShoes(OldShoesID, NewShoesID, TypeShoesID, styleType, Price, Discount);
            return RedirectToAction("ManageProduct");

        }
        [HttpPost]
        public ActionResult AddIcon(FormCollection form)
        {
            var IconID = form["IconID"];
            var IconName = form["IconName"];

            int rs = ConnectIcons.AddIcon(IconID, IconName);
            return RedirectToAction("ManageProduct");

        }
        [HttpPost]
        public ActionResult AddNewshoes(FormCollection form)
        {
            var IconID = form["Icon"];
            var TypeShoesID = int.Parse(form["NewTypeShoes"]);
            var NameShoes = form["nameNS"];
            var Price = float.Parse(form["price"]);
            var StyleType = form["Styletype"];
            var Colour = form["Colours"];
            int rs = 0;
            rs = connectShoes.AddNewShoes(IconID, TypeShoesID, NameShoes, Price, StyleType, Colour);
            return RedirectToAction("ManageProduct");
        }

        public ActionResult ShowStartAdminPage()
        {
            return View();
        }
        public ActionResult DeleteShoes(string ShoesID,string ColourName)
        {
            int rs = connectShoes.DeleteShoes(ShoesID, ColourName);
            return RedirectToAction("ManageProduct");
        }
    }
}