using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectPurchased
    {
        DbContext db = new DbContext();
        //string projectDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
        public List<Purchased> getData(string sort = "DESC")
        {
            List<Purchased> list = new List<Purchased>();
            string sql = "SELECT " +
                "S.ShoesID, " +
                "SD.Name, " +
                "C.Name, " +
                "S.StyleType, " +
                "SUM(OD.Quantity) AS Purchased, " +
                "S.Price, " +
                "S.Price * SUM(OD.Quantity) AS Total, " +
                "I.Url " +
                "FROM " +
                "Shoes S " +
                "JOIN Shoes_Details SD ON S.ShoesID = SD.ShoesID " +
                "JOIN Colour_Detail CD ON S.ShoesID = CD.ShoesID " +
                "JOIN Colours C ON CD.ColourID = C.ColourID " +
                "LEFT JOIN Order_Detail OD ON S.ShoesID = OD.ShoesID AND C.ColourID = OD.ColourID AND S.StyleType = OD.StyleType " +
                "Join Images I ON S.ShoesID = I.ShoesID AND C.ColourID = I.ColourID " +
                "Join OrderSystem OS ON OD.OrderID = OS.OrderID " +
                "Where OS.Status Like N'Done' " +
                "GROUP BY S.ShoesID, SD.Name, C.Name, S.StyleType, S.Price, OD.Quantity, I.Url " +
                "Order by Purchased DESC, S.ShoesID ASC";

            if (sort == "ASC")
            {
                sql = "SELECT " +
                "S.ShoesID, " +
                "SD.Name, " +
                "C.Name, " +
                "S.StyleType, " +
                "SUM(OD.Quantity) AS Purchased, " +
                "S.Price, " +
                "S.Price * SUM(OD.Quantity) AS Total, " +
                "I.Url " +
                "FROM " +
                "Shoes S " +
                "JOIN Shoes_Details SD ON S.ShoesID = SD.ShoesID " +
                "JOIN Colour_Detail CD ON S.ShoesID = CD.ShoesID " +
                "JOIN Colours C ON CD.ColourID = C.ColourID " +
                "LEFT JOIN Order_Detail OD ON S.ShoesID = OD.ShoesID AND C.ColourID = OD.ColourID AND S.StyleType = OD.StyleType " +
                "Join Images I ON S.ShoesID = I.ShoesID AND C.ColourID = I.ColourID " +
                "Join OrderSystem OS ON OD.OrderID = OS.OrderID " +
                "Where OS.Status Like N'Done' " +
                "GROUP BY S.ShoesID, SD.Name, C.Name, S.StyleType, S.Price, OD.Quantity, I.Url " +
                "Order by Purchased ASC, S.ShoesID ASC";
            }
            SqlDataReader rdr = db.ExcuteQuery(sql);

            while (rdr.Read())
            {
                Purchased emp = new Purchased();
                emp.ShoesID = rdr.GetValue(0).ToString();
                emp.ShoesName = rdr.GetValue(1).ToString();
                emp.ColorName = rdr.GetValue(2).ToString();
                emp.StyleType = rdr.GetValue(3).ToString();
                emp.PurchasedQuantity = int.Parse(rdr.GetValue(4).ToString());
                emp.Price = decimal.Parse(rdr.GetValue(5).ToString());
                emp.Total = decimal.Parse(rdr.GetValue(6).ToString());
                emp.Url = rdr.GetValue(7).ToString();
                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }
        public List<Models.Purchased> getPurchased(string userID = "")
        {
            List<Models.Purchased> list = new List<Purchased>();
            if (userID != "")
            {
                string sql = "Select OD.ShoesID, SD.Name, TS.TypeShoesID, TS.Name, OD.ColourID, C.Name, OD.Size,  OD.StyleType, I.Url " +
                    "from Orders O " +
                    "join Order_Detail OD ON O.OrderID = OD.OrderID " +
                    "join OrderSystem OS ON O.OrderID = OS.OrderID " +
                    "join Shoes_Details SD ON OD.ShoesID = SD.ShoesID " +
                    "join Type_Shoes TS ON TS.TypeShoesID = SD.TypeShoesID " +
                    "join Colours C ON C.ColourID = OD.ColourID " +
                    "join Images I ON I.ShoesID = OD.ShoesID And I.ColourID = OD.ColourID " +
                    "where UserID = '" + userID + "' AND OS.Status = 'Confirmed'";
                SqlDataReader rdr = db.ExcuteQuery(sql);
                while (rdr.Read())
                {
                    Purchased emp = new Purchased();
                    emp.ShoesID = rdr.GetValue(0).ToString();
                    emp.ShoesName = rdr.GetValue(1).ToString();
                    emp.TypeShoesID = Convert.ToInt32(rdr.GetValue(2).ToString());
                    emp.TypeShoesName = rdr.GetValue(3).ToString();
                    emp.ColorID = Convert.ToInt32(rdr.GetValue(4).ToString());
                    emp.ColorName = rdr.GetValue(5).ToString();
                    emp.Size = Convert.ToInt32(rdr.GetValue(6).ToString());
                    emp.StyleType = rdr.GetValue(7).ToString();
                    emp.Url = rdr.GetValue(8).ToString();
                    list.Add(emp);
                }
                rdr.Close();

                return list;
            }

            return (list);
        }
        public decimal getTotalIncome()
        {
            string sql = "SELECT SUM(Price*Quantity) " +
                "FROM Order_Detail OD " +
                "Join OrderSystem OS ON OD.OrderID = OS.OrderID " +
                "Where OS.Status Like N'Done'";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            decimal emp = 0;
            if (rdr.Read())
            {
                emp = decimal.Parse(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return (emp);
        }
        public decimal getThisMonthIncome()
        {
            string sql = "SELECT SUM(Price*Quantity) " +
                "FROM Order_Detail OD " +
                "Join OrderSystem OS ON OD.OrderID = OS.OrderID " +
                "Where OS.Status Like N'Done' " +
                "And MONTH(OS.OrderDate) = Month(GETDATE())";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            decimal emp = 0;
            if (rdr.Read())
            {
                emp = decimal.Parse(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return (emp);
        }
        public int getTotalOrder()
        {
            string sql = "SELECT Count(OrderID) " +
                "FROM OrderSystem OS " +
                "Where OS.Status Not Like N'Canceled' ";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            int emp = 0;
            if (rdr.Read())
            {
                emp = int.Parse(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return (emp);
        }
        public int getTotalOrderByMonth(int month = 12)
        {
            string sql = "SELECT Count(OrderID) " +
                "FROM OrderSystem OS " +
                "Where OS.Status Not Like N'Canceled' " +
                "And Month(OS.OrderDate) = '"+month+"'";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            int emp = 0;
            if (rdr.Read())
            {
                emp = int.Parse(rdr.GetValue(0).ToString());
            }
            rdr.Close();
            return (emp);
        }
    }
}