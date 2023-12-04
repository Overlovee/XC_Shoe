using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectFavorite
    {
        DbContext db = new DbContext();
        public List<Favorite> getFavoriteData(string userID)
        {
            List<Favorite> listEmployee = new List<Favorite>();
            string sql = "SELECT * FROM dbo.GetFavorite('" + userID + "')";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Favorite emp = new Favorite();
                emp.ShoesID = rdr.GetValue(0).ToString();
                emp.TypeShoesID = Convert.ToInt32(rdr.GetValue(1).ToString());
                emp.NameShoes = rdr.GetValue(2).ToString();
                emp.StyleType = rdr.GetValue(3).ToString();
                emp.TypeName = rdr.GetValue(4).ToString();
                emp.Number_Colour = Convert.ToInt32(rdr.GetValue(5).ToString());
                emp.Price = float.Parse(rdr.GetValue(6).ToString());
                emp.ImageUrl = rdr.GetValue(7).ToString();
                emp.ColorName = rdr.GetValue(8).ToString();
                emp.favoriteID = Convert.ToInt32(rdr.GetValue(9).ToString());
                listEmployee.Add(emp);
            }
            rdr.Close();
            return (listEmployee);
        }
        public int AddtoFavorite(string userID,string ShoesID,string colourName,string Styletype)
        {
            int rs = 0;
            string sql = "EXEC dbo.AddFavorite'" + userID + "','" + ShoesID + "','"+ colourName + "','" + Styletype +"'";
            rs = db.ExcuteNonQuery(sql); ;
            
            return (rs);
        }
        public int DeleteShoesInFavorite(int favoriteID,string ShoesID, string colourName, string Styletype)
        {
            int rs = 0;
            string sql = "EXEC dbo.DeletoShoesInFavorite "+ favoriteID + ",'" + ShoesID + "','" + colourName + "','" +Styletype +"'";
            rs = db.ExcuteNonQuery(sql); ;
            return (rs);
        }      
    }
}