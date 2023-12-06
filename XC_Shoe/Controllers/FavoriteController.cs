using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                ConnectFavorite connectFavorite = new ConnectFavorite();
                list = connectFavorite.getFavoriteData(userID);
            }
            else
            {
                if (Session["FavoriteList"] != null)
                {
                    List<Favorite> favorites = (List<Favorite>)Session["FavoriteList"];
                    ConnectShoes connectShoes = new ConnectShoes();

                    foreach (Favorite favorite in favorites)
                    {
                        Shoe shoe = connectShoes.getShoesDetailData(favorite.ShoesID, favorite.ColorName);
                        favorite.ImageUrl = shoe.Url;
                        favorite.Number_Colour = shoe.NumberColor;
                        favorite.Price = shoe.Price;
                        favorite.NameShoes = shoe.NameShoes;
                        favorite.TypeShoesID = shoe.TypeShoesID;
                        favorite.TypeName = shoe.TypeShoesName;
                    }

                    return View(favorites);
                }
            }

            return View(list);

        }

        [HttpPost]
        public ActionResult AddToFavorite(string userID = "", string shoesId = "", string colour = "", string styleType = "")
        {
            if (userID != "")
            {
                ConnectFavorite connectFavorite = new ConnectFavorite();
                List<Favorite> favorites = connectFavorite.getFavoriteData(userID);
                int kt = 0;
                foreach (Favorite favorite in favorites)
                {
                    if (favorite.ShoesID == shoesId && favorite.ColorName == colour && favorite.StyleType == styleType)
                    {
                        kt = connectFavorite.DeleteShoesInFavorite(favorite.favoriteID, shoesId, colour, styleType);
                        return Json(new { success = false, message = "Removed from favorites successfully" });
                    }
                }
                kt = connectFavorite.AddtoFavorite(userID, shoesId, colour, styleType);
                return Json(new { success = true, message = "Added to favorites successfully" });
            }
            else
            {
                List<Favorite> favoriteList = Session["FavoriteList"] as List<Favorite>;

                // Nếu danh sách chưa tồn tại, tạo mới
                if (favoriteList == null)
                {
                    favoriteList = new List<Favorite>();
                }

                // Kiểm tra xem đã tồn tại phần tử có shoesId, colour, và styleType đã cho chưa
                Favorite existingFavorite = favoriteList.FirstOrDefault(fav => fav.ShoesID == shoesId && fav.ColorName == colour && fav.StyleType == styleType);

                if (existingFavorite == null)
                {
                    // Nếu không tồn tại, thêm mới vào danh sách
                    Favorite newFavorite = new Favorite
                    {
                        ShoesID = shoesId,
                        ColorName = colour,
                        StyleType = styleType
                    };

                    favoriteList.Add(newFavorite);

                    // Cập nhật danh sách vào Session
                    Session["FavoriteList"] = favoriteList;

                    return Json(new { success = true, message = "Added to favorites successfully" });
                }
                else
                {
                    // Nếu tồn tại, xoá phần tử đó khỏi danh sách
                    favoriteList.Remove(existingFavorite);

                    // Cập nhật danh sách vào Session
                    Session["FavoriteList"] = favoriteList;

                    return Json(new { success = false, message = "Removed from favorites successfully" });
                }

            }
        }
    }
}