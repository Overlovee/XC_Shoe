using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;

namespace XC_Shoe.Controllers
{
    public class FavoriteController : Controller
    {
        // GET: Favorite
        public ActionResult ShowFavoritePage(string userID = "")
        {
            List<Favorite> list = new List<Favorite>();
            if (userID != "")
            {
                ConnectFavorite conF = new ConnectFavorite();
                list = conF.getFavoriteData(userID);
            }
            else
            {
                //Tim kiem session => list
            }
            
            return View(list);
        }
        public ActionResult AddToFavorite(string userID, string Shoesid, string colourName, string styletype)
        {
            ConnectFavorite conF = new ConnectFavorite();
            int rs = conF.AddtoFavorite(userID, Shoesid, colourName, styletype);
            return RedirectToAction("ShowFavoritePage");
        }
        public ActionResult DeleteShoesInFavorite(int favoriteID, string ShoesID, string colourName, string Styletype)
        {
            ConnectFavorite conF = new ConnectFavorite();
            int rs = conF.DeleteShoesInFavorite(favoriteID, ShoesID, colourName, Styletype);
            return RedirectToAction("ShowFavoritePage");
        }
    }
}