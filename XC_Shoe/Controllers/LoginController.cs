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
using System.Data.SqlClient;
using System.Web.WebPages;

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
                    string role = "user";
                    if (user.Role == 1)
                    {
                        role = "admin";
                    }
                    cookie["Role"] = role;

                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("CheckRole", new { role = role });
                    
                }

                return RedirectToAction("ShowHomePage", "User");

            }
            catch (Exception ex)
            {
                return RedirectToAction("SignIn", "Login");
            }

        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string emailSignup, string passwordSignup, string firstName, string lastName)
        {
            emailSignup = Request.Form["emailSignup"];
            passwordSignup = Request.Form["passwordSignup"];
            firstName = Request.Form["firstName"];
            lastName = Request.Form["lastName"];
            if (IsValidRegistrationData(emailSignup, passwordSignup, firstName, lastName))
            {
                return RedirectToAction("SignIn");
            }
            else
                return View();
        }
        private bool IsValidRegistrationData(string email, string password, string firstName, string lastName)
        {
            if (email != null || email != null || password != null || firstName != null || lastName != null)
                ViewBag.ThongBao = "Invalid registration data. Please check your input.";
            return !string.IsNullOrWhiteSpace(email)
                && !string.IsNullOrWhiteSpace(password)
                && !string.IsNullOrWhiteSpace(firstName)
                && !string.IsNullOrWhiteSpace(lastName);
        }
        [HttpPost]
        public ActionResult SignIn(string emailSignin, string passwordSignin)
        {
            emailSignin = Request.Form["emailSignin"];
            passwordSignin = Request.Form["passwordSignin"];
            if (IsValidLogin(emailSignin, passwordSignin))
            {
                ConnectUsers connectUsers = new ConnectUsers();
                User user = new User();
                user = connectUsers.getUserData(emailSignin);
                HttpCookie cookie = new HttpCookie("UserLogin");
                string encodedName = HttpUtility.UrlEncode(user.UserName, Encoding.UTF8);
                cookie["UserID"] = user.UserID;
                cookie["UserName"] = encodedName;
                cookie["Email"] = emailSignin;
                cookie["Account_Img"] = user.Image;
                string role = "user";
                if (user.Role == 1)
                {
                    role = "admin";
                }
                cookie["Role"] = role;
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cookie);
                return RedirectToAction("CheckRole", new {role = role });
            }
            else
            {
                ViewBag.ThongBao = "Invalid email or password.";
                return RedirectToAction("Sigin", "Login");
            }
        }
        private bool IsValidLogin(string email, string password)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DB_XC_Shoes_Store;Integrated Security=True";
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
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

        public ActionResult CheckRole(string role)
        {
            if (role == "admin")
            {
                return RedirectToAction("ShowStartAdminPage", "Admin");
            }
            return RedirectToAction("ShowHomePage", "Login");
        }
    }
}