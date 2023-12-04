using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectSize
    {
        DbContext db = new DbContext();
        public List<Size> getSizeShoesData()
        {
            List<Size> listEmployee = new List<Size>();
            string sql = "SELECT * FROM Size";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Size emp = new Size();
                emp.SizeName = Convert.ToInt32(rdr.GetValue(0).ToString());
                listEmployee.Add(emp);
            }
            return (listEmployee);
        }
    }
}