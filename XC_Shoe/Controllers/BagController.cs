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
                if (Session["BagList"] != null)
                {
                    List<Bag> bags = (List<Bag>)Session["BagList"];
                    ConnectShoes connectShoes = new ConnectShoes();

                    foreach (Bag bag in bags)
                    {
                        Shoe shoe = connectShoes.getShoesDetailData(bag.ShoesID, bag.ColorName);
                        bag.Url = shoe.Url;
                        bag.Price = shoe.Price;
                        bag.ShoesName = shoe.NameShoes;
                    }

                    return View(bags);
                }
            }
            return View(List);
        }

        [HttpPost]
        public ActionResult AddToBag(string userID = "", string shoesId = "", string colour = "", string styleType = "", int size = 40)
        {
            if (userID != "")
            {
                ConnectBag connectBag = new ConnectBag();
                List<Bag> list = connectBag.getBagData(userID);
                int kt = 0;
                foreach (Bag item in list)
                {
                    if (item.ShoesID == shoesId && item.ColorName == colour && item.StyleType == styleType && item.Size == size)
                    {
                        kt = connectBag.UpdateBag(userID, shoesId, colour, styleType, size, item.Quantity + 1, item.BuyingSelectionStatus);

                        return Json(new { success = true, message = "Added to cart successfully" });
                    }
                }
                kt = connectBag.AddtoBag(userID, shoesId, colour, styleType, size, 1);

                return Json(new { success = true, message = "Added to cart successfully" });
            }
            else
            {
                List<Bag> bagList = Session["BagList"] as List<Bag>;

                // Nếu danh sách chưa tồn tại, tạo mới
                if (bagList == null)
                {
                    bagList = new List<Bag>();
                    Bag newBag = new Bag
                    {
                        ShoesID = shoesId,
                        ColorName = colour,
                        StyleType = styleType,
                        Size = size,
                        Quantity = 1,
                        BuyingSelectionStatus = true
                    };
                    bagList.Add(newBag);
                }
                else
                {
                    Bag existingBag = bagList.FirstOrDefault(bag => bag.ShoesID == shoesId && bag.ColorName == colour && bag.StyleType == styleType && bag.Size == size);
                    if (existingBag == null)
                    {
                        Bag newBag = new Bag
                        {
                            ShoesID = shoesId,
                            ColorName = colour,
                            StyleType = styleType,
                            Size = size,
                            Quantity = 1,
                            BuyingSelectionStatus = true
                        };
                        bagList.Add(newBag);
                        Session["BagList"] = bagList;

                    }
                    else
                    {
                        existingBag.Quantity += 1;

                        Session["BagList"] = bagList;

                    }
                }
                Session["BagList"] = bagList;
            }
            // Trả về một JSON object để xử lý ở phía client
            return Json(new { success = true, message = "Added to cart successfully" });
        }

        [HttpPost]
        public ActionResult UpdateBag(string userID = "", string shoesId = "", string colour = "", string styleType = "", int size = 40, int quantity = 1, bool selection = true)
        {
            if (userID != "")
            {
                ConnectBag connectBag = new ConnectBag();
                List<Bag> list = connectBag.getBagData(userID);
                int kt = 0;
                foreach (Bag item in list)
                {
                    if (item.ShoesID == shoesId && item.ColorName == colour && item.StyleType == styleType && item.Size == size)
                    {
                        kt = connectBag.UpdateBag(userID, shoesId, colour, styleType, size, quantity, selection);

                        return Json(new { success = true, message = "Added to cart successfully" });
                    }
                }
            }
            else
            {
                List<Bag> bagList = Session["BagList"] as List<Bag>;

                // Nếu danh sách chưa tồn tại, tạo mới
                if (bagList == null)
                {
                    bagList = new List<Bag>();
                    Bag newBag = new Bag
                    {
                        ShoesID = shoesId,
                        ColorName = colour,
                        StyleType = styleType,
                        Size = size,
                        Quantity = quantity,
                        BuyingSelectionStatus = selection
                    };
                    bagList.Add(newBag);
                }
                else
                {
                    Bag existingBag = bagList.FirstOrDefault(bag => bag.ShoesID == shoesId && bag.ColorName == colour && bag.StyleType == styleType && bag.Size == size);
                    if (existingBag == null)
                    {
                        Bag newBag = new Bag
                        {
                            ShoesID = shoesId,
                            ColorName = colour,
                            StyleType = styleType,
                            Size = size,
                            Quantity = quantity,
                            BuyingSelectionStatus = selection
                        };
                        bagList.Add(newBag);
                        Session["BagList"] = bagList;
                    }
                    else
                    {
                        existingBag.Quantity = quantity;
                        existingBag.BuyingSelectionStatus = selection;
                        Session["BagList"] = bagList;
                    }
                }
                Session["BagList"] = bagList;
            }
            // Trả về một JSON object để xử lý ở phía client
            return Json(new { success = true, message = "Added to cart successfully" });
        }
        [HttpPost]
        public ActionResult DeleteItemFromBag(string userID = "", string shoesId = "", string colour = "", string styleType = "", int size = 40)
        {
            if (userID != "")
            {
                ConnectBag connectBag = new ConnectBag();
                List<Bag> list = connectBag.getBagData(userID);
                int kt = 0;
                foreach (Bag item in list)
                {
                    if (item.ShoesID == shoesId && item.ColorName == colour && item.StyleType == styleType && item.Size == size)
                    {
                        kt = connectBag.DeleteShoesInBag(userID, shoesId, colour, styleType, size);

                        return Json(new { success = true, message = "Delete from cart successfully" });
                    }
                }
            }
            else
            {
                List<Bag> bagList = Session["BagList"] as List<Bag>;

                if (bagList == null)
                {
                    return Json(new { success = false, message = "Nothing!" });
                }
                else
                {
                    Bag existingBag = bagList.FirstOrDefault(bag => bag.ShoesID == shoesId && bag.ColorName == colour && bag.StyleType == styleType && bag.Size == size);
                    if (existingBag != null)
                    {
                        bagList.Remove(existingBag);
                        Session["BagList"] = bagList;
                    }
                }
                Session["BagList"] = bagList;
            }
            // Trả về một JSON object để xử lý ở phía client
            return Json(new { success = true, message = "Delete from  cart successfully" });
        }
    }
}