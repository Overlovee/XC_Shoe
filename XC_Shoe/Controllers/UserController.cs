using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public ActionResult ShoesPage(string gender = "Men", string search = "")
        {
            ConnectShoes connectShoes = new ConnectShoes();
            List<Shoe> ListShoes = connectShoes.GetRepresentData(gender);
            return View(ListShoes);
        }
        public ActionResult ShowShoesDetail(string shoesID, string colourName)
        {
            ConnectShoes connectShoes = new ConnectShoes();
            ConnectSize connectSize = new ConnectSize();
            Shoe shoes = connectShoes.getShoesDetailData(shoesID, colourName);
            List<Size> SizeList = connectSize.getSizeShoesData();
            List<Shoe> shoesList = connectShoes.getShoesByShoesIDData(shoesID);
            ViewBag.ShoesColor = shoesList;
            ViewBag.size = SizeList;
            return View(shoes);
        }
        public ActionResult UserProfile(string Email = "hoang2011@gmail.com")
        {
            ConnectUsers connectUser = new ConnectUsers();
            User User = connectUser.getUserData(Email);
            List<UserShipment> List = connectUser.getUserShipmentDetails(User.UserID);
            ViewBag.UserShipment = List;
            return View(User);
        }
        [HttpPost]
        public ActionResult GetImages(string url)
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

            foreach (string img in images)
            {
                fileNameOnly = Path.GetFileName(img);
                string updatedImagePath = Path.Combine(resourcesPath, fileNameOnly);
                updatedImagePaths.Add(updatedImagePath);
            }
            return Json(new { avtImg = avt_img, imagesFile = updatedImagePaths});
        }
    }
}