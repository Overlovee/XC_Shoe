using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using GoogleAuthentication.Services;
using Newtonsoft.Json;
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
        public UserController() {
            
        }
        public ActionResult ShowHomePage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ShoesPage(string gender = "", string icon = "", string type = "", string search = "")
        {
            ConnectShoes connectShoes = new ConnectShoes();
            search = HttpUtility.UrlDecode(search);
            List<Shoe> ListShoes = connectShoes.GetRepresentData(gender, icon, type, search);
            ViewBag.gender = gender;
            ViewBag.icon = icon;
            ViewBag.type = type;
            ViewBag.search = search;

            return View(ListShoes);
        }
        public ActionResult ShowShoesDetail(string shoesID, string colourName)
        {
            ConnectShoes connectShoes = new ConnectShoes();
            ConnectSize connectSize = new ConnectSize();
            Shoe shoes = connectShoes.getShoesDetailData(shoesID, colourName);
            List<Size> SizeList = connectSize.getSizeShoesData(colourName, shoesID);
            List<Shoe> shoesList = connectShoes.getShoesByShoesIDData(shoesID);
            ViewBag.ShoesColor = shoesList;
            ViewBag.size = SizeList;
            return View(shoes);
        }
        public ActionResult CompleteOrder(string orderID = "")
        {
            
            ViewBag.orderID = orderID;
            return View();
        }
        public ActionResult CheckOut(string userID = "")
        {
            List<Bag> list = new List<Bag>();
            List<UserShipment> userShipmentList = new List<UserShipment>();
            if (userID != "")
            {
                ConnectUsers connectUser = new ConnectUsers();

                userShipmentList = connectUser.getUserShipmentDetails(userID);

                ConnectBag connectBag = new ConnectBag();
                list = connectBag.getSelectedItemBags(userID);

                ViewBag.userShipmentList = userShipmentList;
                return View(list);
            }
            else
            {
                if (Session["BagList"] != null)
                {
                    List<Bag> bags = (List<Bag>)Session["BagList"];
                    ConnectShoes connectShoes = new ConnectShoes();
                    List<Bag> checkoutList = new List<Bag>();
                    foreach (Bag bag in bags)
                    {
                        Shoe shoe = connectShoes.getShoesDetailData(bag.ShoesID, bag.ColorName);
                        bag.Url = shoe.Url;
                        bag.Price = shoe.Price;
                        bag.ShoesName = shoe.NameShoes;
                        if (bag.BuyingSelectionStatus)
                        {
                            checkoutList.Add(bag);
                        }
                    }
                    ViewBag.userShipmentList = userShipmentList;
                    return View(checkoutList);
                }
            }
            ViewBag.userShipmentList = userShipmentList;
            return View(list);
        }
        [HttpPost]
        public ActionResult MakeOrder(string userID = "", List<Dictionary<string, string>> infor = null, float total = 0)
        {
            string Username = "";
            string PhoneNumber = "";
            string SpecificAddress = "";
            string AdministrativeBoundaries = "";
            string orderID = "0";
            foreach (var shipmentInfo in infor)
            {
                if (shipmentInfo.ContainsKey("shipmentUsername"))
                {
                    Username = shipmentInfo["shipmentUsername"];
                }
                if (shipmentInfo.ContainsKey("shipmentPhoneNumber"))
                {
                    PhoneNumber = shipmentInfo["shipmentPhoneNumber"];
                }
                if (shipmentInfo.ContainsKey("shipmentSpecificAddress"))
                {
                    SpecificAddress = shipmentInfo["shipmentSpecificAddress"];
                }
                if (shipmentInfo.ContainsKey("shipmentAdministrativeBoundaries"))
                {
                    AdministrativeBoundaries = shipmentInfo["shipmentAdministrativeBoundaries"];
                }
            }

            if (userID != "")
            {
                ConnectOrders connectOrders = new ConnectOrders();
                int kt = connectOrders.AddToOrder(userID, Username, PhoneNumber, SpecificAddress + "," + AdministrativeBoundaries, 250000, total, DateTime.Now);
                if(kt != 0)
                {
                    orderID = connectOrders.getOrderID(userID, Username, PhoneNumber, SpecificAddress + "," + AdministrativeBoundaries, 250000, total);
                }
            }
            else
            {
                if (Session["BagList"] != null)
                {
                    List<Bag> bags = (List<Bag>)Session["BagList"];
                    ConnectShoes connectShoes = new ConnectShoes();
                    ConnectOrders connectOrders = new ConnectOrders();
                    int kt = connectOrders.AddToOrder(userID, Username, PhoneNumber, SpecificAddress + "," + AdministrativeBoundaries, 250000, total, DateTime.Now);
                    
                    if (kt != 0)
                    {
                        orderID = connectOrders.getOrderID(userID, Username, PhoneNumber, SpecificAddress + "," + AdministrativeBoundaries, 250000, total);
                        
                        for (int i = bags.Count - 1; i >= 0; i--)
                        {
                            Bag bag = bags[i];
                            Shoe shoe = connectShoes.getShoesDetailData(bag.ShoesID, bag.ColorName);
                            bag.Url = shoe.Url;
                            bag.Price = shoe.Price;
                            bag.ShoesName = shoe.NameShoes;
                            
                            if (bag.BuyingSelectionStatus)
                            {
                                int rs = connectOrders.AddToOrderDetail(orderID, bag.ShoesID, bag.Quantity, bag.Size, bag.StyleType, bag.ColorName, bag.Price);
                                if(rs != 0)
                                {
                                    bags.RemoveAt(i);
                                }
                            }
                        }
                        Session["BagList"] = bags;
                    }
                    return Json(new { success = true, message = "Order successfully", orderID = orderID});
                }
            }

            return Json(new { success = true, message = "Order successfully" , orderID = orderID });
        }
        public ActionResult UserProfile(string Email = "")
        {
            ConnectUsers connectUser = new ConnectUsers();
            User User = connectUser.getUserData(Email);
            List<UserShipment> List = connectUser.getUserShipmentDetails(User.UserID);
            ViewBag.UserShipment = List;
            return View(User);
        }
        [HttpPost]
        public ActionResult GetImages(string url, string colourName, string shoesID)
        {
            string projectDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~"); ;
            string resourcesPath = "/Resources/Shoes/";
            string directoryPath = Path.Combine(projectDirectory, "Resources", "Shoes", url);
            resourcesPath = Path.Combine(resourcesPath, url);

            string[] imageFiles = Directory.GetFiles(directoryPath, "*-AVT.jpg").Union(Directory.GetFiles(directoryPath, "*-AVT.png")).ToArray();

            string imagePath = imageFiles.Length > 0 ? Path.Combine(directoryPath, Path.GetFileName(imageFiles[0])) : "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/u_126ab356-44d8-4a06-89b4-fcdcc8df0245,c_scale,fl_relative,w_1.0,h_1.0,fl_layer_apply/b207c97d-1d63-4e43-9339-375b26222ae2/air-jordan-xxxviii-fiba-pf-basketball-shoes-XnhFhP.png";
            string[] images = Directory.GetFiles(directoryPath, "*.jpg")
                     .Union(Directory.GetFiles(directoryPath, "*.png"))
                     .ToArray();
            string fileNameOnly = Path.GetFileName(imagePath);
            string avt_img = Path.Combine(resourcesPath, fileNameOnly);

            List<string> updatedImagePaths = new List<string>();
            ConnectSize connectSize = new ConnectSize();
            List<Size> SizeList = connectSize.getSizeShoesData(colourName, shoesID);

            List<int> listSize = new List<int>();
            foreach (Size size in SizeList)
            {
                listSize.Add(size.SizeName);
            }
            foreach (string img in images)
            {
                fileNameOnly = Path.GetFileName(img);
                string updatedImagePath = Path.Combine(resourcesPath, fileNameOnly);
                updatedImagePaths.Add(updatedImagePath);
            }
            return Json(new { avtImg = avt_img, imagesFile = updatedImagePaths, sizes = listSize});
        }
    }
}