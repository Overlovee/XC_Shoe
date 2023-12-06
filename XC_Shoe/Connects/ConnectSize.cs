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
        public List<Size> getSizeShoesData(string colourName, string shoesID)
        {
            List<Size> listEmployee = new List<Size>();
            string sql = "SELECT sizeID FROM size_Detail SD,Colours C WHERE C.ColourID = SD.ColourID AND  SD.shoesID = '" + shoesID + "' AND C.Name = '" + colourName + "'";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                Size emp = new Size();
                emp.SizeName = Convert.ToInt32(rdr.GetValue(0).ToString());
                listEmployee.Add(emp);
            }
            rdr.Close();
            return (listEmployee);
        }
    }
}