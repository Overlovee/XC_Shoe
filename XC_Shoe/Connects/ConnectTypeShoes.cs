using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectTypeShoes
    {
        DbContext db = new DbContext();
        public List<Models.TypeShoes> getTypeShoesData()
        {
            List<Models.TypeShoes> listEmployee = new List<Models.TypeShoes>();
            string sql = "SELECT * FROM Type_Shoes";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Models.TypeShoes emp = new Models.TypeShoes();
                emp.TypeShoesID = Convert.ToInt32(rdr.GetValue(0).ToString());
                emp.NameTS = rdr.GetValue(1).ToString();

                listEmployee.Add(emp);
            }
            return (listEmployee);
        }
    }
}