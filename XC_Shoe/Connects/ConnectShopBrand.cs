using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectShopBrand
    {
        DbContext db = new DbContext();
        public List<ShopBrand> getShopBrandsData()
        {
            List<ShopBrand> listEmployee = new List<ShopBrand>();
            string sql = "SELECT *FROM Shop_Branchs";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                ShopBrand emp = new ShopBrand();
                emp.ID = Convert.ToInt32(rdr.GetValue(0).ToString());
                emp.ShopID = rdr.GetValue(1).ToString();
                emp.ShopBranchAddress = rdr.GetValue(2).ToString();
                emp.BranchManagement = rdr.GetValue(3).ToString();
                listEmployee.Add(emp);
            }
            return (listEmployee);
        }
    }
}