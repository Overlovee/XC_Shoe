using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;
using System.IO;

namespace XC_Shoe.Connects
{
    public class ConnectShoes
    {
        DbContext db = new DbContext();
        //string projectDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
            
        public List<Models.Shoe> GetRepresentData(string Gender)
        {
            List<Shoe> list = new List<Shoe>();
            string sql = "SELECT * FROM dbo.GetFirstShoeInfo('" + Gender + "')";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Shoe shoes = new Shoe();
                shoes.IconID = rdr.GetValue(0).ToString();
                shoes.ShoesID = rdr.GetValue(1).ToString();
                shoes.TypeShoesID = Convert.ToInt32(rdr.GetValue(2).ToString());
                shoes.NameShoes = rdr.GetValue(3).ToString();
                shoes.StyleType = rdr.GetValue(4).ToString();
                shoes.TypeShoesName = rdr.GetValue(5).ToString();
                shoes.NameColor = rdr.GetValue(6).ToString();
                shoes.NumberColor = Convert.ToInt32(rdr.GetValue(7).ToString());
                shoes.Price = float.Parse(rdr.GetValue(8).ToString());
                shoes.Discount = float.Parse(rdr.GetValue(9).ToString());
                shoes.Url = rdr.GetValue(10).ToString();

                list.Add(shoes);
            }
            rdr.Close();
            return (list);
        }
        public List<Models.Shoe> getShoesData(string Gender)
        {
            List<Models.Shoe> list = new List<Shoe>();
            string sql = "SELECT S.IconID, " +
                "S.ShoesID, " +
                "SD.TypeShoesID, " +
                "SD.Name, " +
                "S.StyleType, " +
                "TS.Name, " +
                "count(CD.ColourID) as 'Number_Colour', " +
                "S.Price,S.Discount " +
                "FROM Shoes S, Shoes_Details SD, Type_Shoes TS, Colour_Detail CD " +
                "WHERE " +
                "S.ShoesID = SD.ShoesID " +
                "AND SD.TypeShoesID = TS.TypeShoesID " +
                "AND S.ShoesID = CD.ShoesID " +
                "GROUP BY S.IconID, S.ShoesID, SD.TypeShoesID, SD.Name, S.StyleType, TS.Name, S.Price, S.Discount " +
                "HAVING StyleType = '" + Gender + "'";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Shoe emp = new Shoe();
                emp.IconID = rdr.GetValue(0).ToString();
                emp.ShoesID = rdr.GetValue(1).ToString();
                emp.TypeShoesID = Convert.ToInt32(rdr.GetValue(2).ToString());
                emp.NameShoes = rdr.GetValue(3).ToString();
                emp.StyleType = rdr.GetValue(4).ToString();
                emp.TypeShoesName = rdr.GetValue(5).ToString();
                emp.NumberColor = Convert.ToInt32(rdr.GetValue(6).ToString());
                emp.Price = float.Parse(rdr.GetValue(7).ToString());
                emp.Discount = float.Parse(rdr.GetValue(8).ToString());
                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }
        public List<Models.Shoe> getShoesDataByStyleType(string Gender, string sort, string search)
        {
            List<Models.Shoe> list = new List<Shoe>();
            string sql = "SELECT S.ShoesID, SD.Name, TS.Name, C.Name, S.Price, S.Discount, I.Url, S.StyleType " +
                "FROM Shoes S " +
                "join Shoes_Details SD ON S.ShoesID = SD.ShoesID " +
                "join Type_Shoes TS ON SD.TypeShoesID = TS.TypeShoesID " +
                "join Colour_Detail CD ON S.ShoesID = CD.ShoesID " +
                "join Colours C ON CD.ColourID = C.ColourID " +
                "join Images I ON S.ShoesID = I.ShoesID AND CD.ColourID = I.ColourID " +
                "Where S.StyleType like '" + Gender + "' ";
            if (search != "")
            {
                sql = "SELECT S.ShoesID, SD.Name, TS.Name, C.Name, S.Price, S.Discount, I.Url, S.StyleType " +
                   "FROM Shoes S " +
                   "join Shoes_Details SD ON S.ShoesID = SD.ShoesID " +
                   "join Type_Shoes TS ON SD.TypeShoesID = TS.TypeShoesID " +
                   "join Colour_Detail CD ON S.ShoesID = CD.ShoesID " +
                   "join Colours C ON CD.ColourID = C.ColourID " +
                   "join Images I ON S.ShoesID = I.ShoesID AND CD.ColourID = I.ColourID " +
                   "Where S.StyleType like '" + Gender + "' " +
                   "And SD.Name like '%"+search+"%' ";
            }
            if (sort == "DESC")
            {
                sql += "Order by SD.Name DESC";
            }
            else
            {
                sql += "Order by SD.Name ASC";
            }
            
            SqlDataReader rdr = db.ExcuteQuery(sql);
            
            while (rdr.Read())
            {
                Shoe emp = new Shoe();
                emp.ShoesID = rdr.GetValue(0).ToString();
                emp.NameShoes = rdr.GetValue(1).ToString();
                emp.TypeShoesName = rdr.GetValue(2).ToString();
                emp.NameColor = rdr.GetValue(3).ToString();
                emp.Price = float.Parse(rdr.GetValue(4).ToString());
                emp.Discount = float.Parse(rdr.GetValue(5).ToString());
                emp.Url = rdr.GetValue(6).ToString();
                emp.StyleType = rdr.GetValue(7).ToString();
                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }
        public Shoe getShoesDetailData(string ShoesID, string ColourName)
        {
            string sql = "SELECT * FROM dbo.ShowDetailShoes('" + ShoesID +"',N'" + ColourName + "')";
            Shoe shoes = new Shoe();
            SqlDataReader rdr = db.ExcuteQuery(sql);
            if (rdr.Read())
            {
                shoes.IconID = rdr.GetValue(0).ToString();
                shoes.ShoesID = rdr.GetValue(1).ToString();
                shoes.TypeShoesID = Convert.ToInt32(rdr.GetValue(2).ToString());
                shoes.NameShoes = rdr.GetValue(3).ToString();
                shoes.StyleType = rdr.GetValue(4).ToString();
                shoes.TypeShoesName = rdr.GetValue(5).ToString();
                shoes.NameColor = rdr.GetValue(6).ToString();
                shoes.NumberColor = Convert.ToInt32(rdr.GetValue(7).ToString());
                shoes.Price = float.Parse(rdr.GetValue(8).ToString());
                shoes.Discount = float.Parse(rdr.GetValue(9).ToString());
                shoes.Url = rdr.GetValue(10).ToString();
            }
            rdr.Close();
            return (shoes);
        }
        public List<Models.Shoe> getShoesByShoesIDData(string ShoesID)
        {
            List<Models.Shoe> list = new List<Shoe>();
            string sql = "SELECT * FROM dbo.ShowDetailShoesWithShoesID('"+ShoesID+"')";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Shoe shoes = new Shoe();
                shoes.IconID = rdr.GetValue(0).ToString();
                shoes.ShoesID = rdr.GetValue(1).ToString();
                shoes.TypeShoesID = Convert.ToInt32(rdr.GetValue(2).ToString());
                shoes.NameShoes = rdr.GetValue(3).ToString();
                shoes.StyleType = rdr.GetValue(4).ToString();
                shoes.TypeShoesName = rdr.GetValue(5).ToString();
                shoes.NameColor = rdr.GetValue(6).ToString();
                shoes.NumberColor = Convert.ToInt32(rdr.GetValue(7).ToString());
                shoes.Price = float.Parse(rdr.GetValue(8).ToString());
                shoes.Discount = float.Parse(rdr.GetValue(9).ToString());
                shoes.Url = rdr.GetValue(10).ToString();
                list.Add(shoes);
            }
            rdr.Close();
            return (list);
        }
    }
}