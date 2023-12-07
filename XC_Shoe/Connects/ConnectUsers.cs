using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectUsers
    {
        DbContext db = new DbContext();
        //string projectDirectory = System.Web.Hosting.HostingEnvironment.MapPath("~");
        public List<User> getData(string role = "0", string sort = "ASC", string search = "")
        {
            List<User> list = new List<User>();
            string sql = "SELECT UserID, UserName, Email, PhoneNumber, Image " +
                "FROM Users " +
                "Where Role = '" + role + "' ";
            if (search != "")
            {
                sql += "And UserName like N'%" + search + "%' ";
            }
            if (sort == "DESC")
            {
                sql += "Order by UserName DESC";
            }
            else
            {
                sql += "Order by UserName ASC";
            }

            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                User emp = new User();
                emp.UserID = rdr.GetValue(0).ToString();
                emp.UserName = rdr.GetValue(1).ToString();
                emp.Email = rdr.GetValue(2).ToString();
                emp.PhoneNumber = rdr.GetValue(3).ToString();
                emp.Image = rdr.GetValue(4).ToString();
                list.Add(emp);
            }
            rdr.Close ();
            return (list);
        }
        public User getDataByID(string id = "")
        {
            string sql = "SELECT UserID, UserName, Email, PhoneNumber, Image " +
                "FROM Users " +
                "Where UserID = '" + id + "' ";

            SqlDataReader rdr = db.ExcuteQuery(sql);
            User emp = new User();
            if (rdr.Read())
            {
                emp.UserID = rdr.GetValue(0).ToString();
                emp.UserName = rdr.GetValue(1).ToString();
                emp.Email = rdr.GetValue(2).ToString();
                emp.PhoneNumber = rdr.GetValue(3).ToString();
                emp.Image = rdr.GetValue(4).ToString();
            }
            rdr.Close();
            return emp;
        }
        public List<UserShipment> getUserShipmentDetails(string id)
        {
            List<UserShipment> list = new List<UserShipment>();
            string sql = "SELECT * FROM Users_ShipmentDetails WHERE UserID = '" + id + "' order by IsDefault DESC";
            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                UserShipment emp = new UserShipment();
                emp.UserID = rdr.GetValue(0).ToString();
                emp.UserName = rdr.GetValue(1).ToString();
                emp.PhoneNumber = rdr.GetValue(2).ToString();
                emp.SpecificAddress = rdr.GetValue(3).ToString();
                emp.AdministrativeBoundaries = rdr.GetValue(4).ToString();
                emp.IsDefault = bool.Parse(rdr.GetValue(5).ToString());
                list.Add(emp);
            }
            return (list);
        }
        public User getUserData(string Email)
        {
            string sql = "SELECT dbo.GetBeforeMailString(u.Email) as  'NameTag', u.UserID,u.UserName,u.Email,u.Password,u.PhoneNumber,u.Image,u.Role from Users u where u.Email = N'" + Email + "'";
            User emp = new User();

            SqlDataReader rdr = db.ExcuteQuery(sql);
            while (rdr.Read())
            {
                emp.NameTag = rdr.GetValue(0).ToString();
                emp.UserID = rdr.GetValue(1).ToString();
                emp.UserName = rdr.GetValue(2).ToString();
                emp.Email = rdr.GetValue(3).ToString();
                emp.Password = rdr.GetValue(4).ToString();
                emp.PhoneNumber = rdr.GetValue(5).ToString();
                emp.Image = rdr.GetValue(6).ToString();
                emp.Role = int.Parse(rdr.GetValue(7).ToString());

            }
            rdr.Close();
            return (emp);
        }
    }
}