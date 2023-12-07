using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XC_Shoe.Connects;
using XC_Shoe.Models;
using System.IO;
namespace XC_Shoe.Controllers
{
    public class ShoesController : Controller
    {
        // GET: Shoes
        public ActionResult TypeShoesPartial()
        {
            ConnectTypeShoes connectShoes = new ConnectTypeShoes();
            List<TypeShoes> TypeShoesList = connectShoes.getTypeShoesData();
            return View(TypeShoesList);
        }
        public ActionResult IconsPartial()
        {
            ConnectIcons connectShoes = new ConnectIcons();
            List<Icons> IconList = connectShoes.getIconShoesData();
            return View(IconList);
        }
        [HttpPost]
        public ActionResult UploadImages()
        {
            if (Request.Files.Count > 0)
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];

                    if (file != null && file.ContentLength > 0)
                    {
                        // Specify the path where you want to save the file
                        var filePath = Path.Combine(Server.MapPath("~/YourTargetFolder"), Path.GetFileName(file.FileName));

                        file.SaveAs(filePath);
                    }
                }
            }

            return Content("Images uploaded successfully");
        }
    }
}