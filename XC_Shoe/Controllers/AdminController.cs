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
        ConnectUsers connectUsers = new ConnectUsers();
        ConnectOrders connectOrders = new ConnectOrders();
        ConnectPurchased connectPurchased = new ConnectPurchased();
        User user = new User();
        public AdminController()
        {
            user = connectUsers.getDataByID("US1");
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
        public ActionResult ManageProduct(string styleStyle = "Men", string sort = "ASC", string search = "")
        {
            List<Shoe> list = connectShoes.getShoesDataByStyleType(styleStyle, sort, search);
            ViewBag.Title = "Manage Products";
            ViewBag.MainTitle = "Manage Products";
            ViewBag.Style = styleStyle;
            ViewBag.Sort = sort;
            ViewBag.Total = list.Count;
            ViewBag.SearchValue = search;
            return View(list);
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
            List<User> list = connectUsers.getData(role, sort, search);
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
        public ActionResult ConfirmOrder(string id, string status = "Wait for confirmation", string sort = "ASC", string search = "")
        {
            return RedirectToAction("ManageOrder");
        }
    }
}