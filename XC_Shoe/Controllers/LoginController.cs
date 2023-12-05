using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using GoogleAuthentication.Services;
using System.Security.Policy;
using Newtonsoft.Json;
using System.Threading.Tasks;
using XC_Shoe.Models;
using XC_Shoe.Connects;
using System.Text;

namespace XC_Shoe.Controllers
{
    public class LoginController : Controller
    {
        // GET: DangNhap

        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            var ClientID = "1069009233830-kdj02hne53t722c7hha3d4a81qd0fs61.apps.googleusercontent.com";
            var url = "http://localhost:57352/Login/SignInWithGoogle";
            var response = GoogleAuth.GetAuthUrl(ClientID, url);
            ViewBag.response = response;
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> SignInWithGoogle(string code)
        {
            try
            {
                var ClientSecret = "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";
                var ClientID = "1069009233830-kdj02hne53t722c7hha3d4a81qd0fs61.apps.googleusercontent.com";
                var url = "http://localhost:57352/Login/SignInWithGoogle";
                var token = await GoogleAuth.GetAuthAccessToken(code, ClientID, ClientSecret, url);
                var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
                var googleUser = JsonConvert.DeserializeObject<GoogleProfile>(userProfile);

                //Session["Account"] = googleUser.Name;
                //Session["Email"] = googleUser.Email;
                //Session["Account_Img"] = googleUser.Picture;


                //Kiểm tra nếu Email dưới db
                if (googleUser.Email != "")
                {
                    HttpCookie cookie = new HttpCookie("UserLogin");
                    string encodedName = HttpUtility.UrlEncode(googleUser.Name, Encoding.UTF8);
                    cookie["UserName"] = encodedName;
                    cookie["Email"] = googleUser.Email;
                    cookie["Account_Img"] = googleUser.Picture;

                    //Session["UserID"] = id;
                    ConnectUsers connectUsers = new ConnectUsers();
                    User user = new User();
                    user = connectUsers.getUserData(googleUser.Email);
                    if (user != null)
                    {
                        cookie["UserID"] = user.UserID;
                    }
                    else
                    {
                        // Đệ gọi hàm đăng kí cho email này 
                        cookie["UserID"] = "";

                    }
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("ShowHomePage", "User");
                }

                return RedirectToAction("SignIn", "Login");

            }
            catch (Exception ex)
            {
                return RedirectToAction("SignIn", "Login");
            }
            
        }

        public ActionResult SignInWithAccount(string email, string password)
        {
            //Đệ xử lí

            return RedirectToAction("ShowHomePage", "User");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            var ClientID = "1069009233830-kdj02hne53t722c7hha3d4a81qd0fs61.apps.googleusercontent.com";
            var url = "http://localhost:57352/Login/SignInWithGoogle";
            var response = GoogleAuth.GetAuthUrl(ClientID, url);
            ViewBag.response = response;
            return View();
            //Đệ xử lí

            return View();
        }


        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("UserLogin");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("ShowHomePage", "User");
        }
    }
}