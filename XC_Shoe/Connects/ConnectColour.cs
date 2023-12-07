using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;
namespace XC_Shoe.Connects
{
    public class ConnectColour
    {
        DbContext db = new DbContext();
        public List<Colours> getColourShoesData()
        {
            List<Colours> list = new List<Colours>();
            string sql = "SELECT * FROM Colours";
           
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Colours emp = new Colours();
                emp.ColourID = Convert.ToInt32(rdr.GetValue(0).ToString());
                emp.ColourName = rdr.GetValue(1).ToString();

                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }
        public int EditShoes(string OldShoesID, string NewShoesID, int TypeShoesID, string StyleType, float Price,float discount)
        {
            int rs = 0;
            string sql = "EXEC dbo.UpdateShoes '"+ OldShoesID+"','"+NewShoesID+"',"+TypeShoesID+",'"+StyleType+"',"+Price+","+discount;
            rs = db.ExcuteNonQuery(sql);
            db.close();
            return (rs);
        }
        
    }
}