using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using XC_Shoe.Models;

namespace XC_Shoe.Connects
{
    public class ConnectOrders
    {
        DbContext db = new DbContext();
        public List<Order> getBasicOrderData(string status = "Wait for confirmation", string sort = "ASC", string search = "")
        {
            List<Order> list = new List<Order>();
            string sql = "SELECT " +
                "O.OrderID, " +
                "O.UserID, " +
                "O.PaymentInfo, " +
                "O.EstimatedDeliveryHandlingFee, " +
                "O.Email, " +
                "O.Total, " +
                "O.PaymentStatus, " +
                "O.RecipientAddress, " +
                "O.RecipientName, " +
                "O.RecipientPhoneNumber, " +
                "O.OrderDate, " +
                "OS.EmployeeID, " +
                "OS.Status " +
                "FROM ORDERS O " +
                "JOIN OrderSystem OS ON O.OrderID = OS.OrderID " +
                "Where OS.Status like N'" + status + "' ";
            if (status == "All")
            {
                sql = "SELECT " +
                "O.OrderID, " +
                "O.UserID, " +
                "O.PaymentInfo, " +
                "O.EstimatedDeliveryHandlingFee, " +
                "O.Email, " +
                "O.Total, " +
                "O.PaymentStatus, " +
                "O.RecipientAddress, " +
                "O.RecipientName, " +
                "O.RecipientPhoneNumber, " +
                "O.OrderDate, " +
                "OS.EmployeeID, " +
                "OS.Status " +
                "FROM ORDERS O " +
                "JOIN OrderSystem OS ON O.OrderID = OS.OrderID ";
            }
            
            if (search != "")
            {
                sql += "And " +
                    "(O.RecipientName like N'%" + search + "%' " +
                    "Or O.Email like N'%" + search + "%' " +
                    "Or O.RecipientPhoneNumber like '%" + search + "%') ";
            }
            if (sort == "DESC")
            {
                sql += "Order by O.OrderID DESC";
            }
            else
            {
                sql += "Order by O.OrderID ASC";
            }

            SqlDataReader rdr = db.ExcuteQuery(sql);

            while (rdr.Read())
            {
                Order emp = new Order();
                emp.OrderID = rdr.GetValue(0).ToString();
                emp.UserID = rdr.GetValue(1).ToString();
                emp.PaymentInfo = rdr.GetValue(2).ToString();
                emp.EstimatedDeliveryHandlingFee = decimal.Parse(rdr.GetValue(3).ToString());
                emp.Email = rdr.GetValue(4).ToString();
                emp.Total = decimal.Parse(rdr.GetValue(5).ToString());
                emp.PaymentStatus = rdr.GetValue(6).ToString();
                emp.RecipientAddress = rdr.GetValue(7).ToString();
                emp.RecipientName = rdr.GetValue(8).ToString();
                emp.RecipientPhoneNumber = rdr.GetValue(9).ToString();
                emp.OrderDate = DateTime.Parse(rdr.GetValue(10).ToString());
                emp.orderSystem.EmployeeID = rdr.GetValue(11).ToString();
                emp.orderSystem.Status = rdr.GetValue(12).ToString();

                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }
        public List<OrderDetails> getOrderDetailsByID(string id)
        {
            List<OrderDetails> list = new List<OrderDetails>();
            string sql = "Select " +
                "OrderID, " +
                "OD.ShoesID, " +
                "Quantity, " +
                "Size, " +
                "StyleType, " +
                "OD.ColourID, " +
                "Price,SD.Name, " +
                "C.Name from Order_Detail OD " +
                "join Shoes_Details SD ON OD.ShoesID = SD.ShoesID " +
                "join Colours C ON OD.ColourID = C.ColourID " +
                "Where OD.OrderID = '"+id+"'";

            SqlDataReader rdr = db.ExcuteQuery(sql);

            while (rdr.Read())
            {
                OrderDetails emp = new OrderDetails();
                emp.OrderID = rdr.GetValue(0).ToString();
                emp.ShoesID = rdr.GetValue(1).ToString();
                emp.Quantity = int.Parse(rdr.GetValue(2).ToString());
                emp.Size = int.Parse(rdr.GetValue(3).ToString());
                emp.StyleType = rdr.GetValue(4).ToString();
                emp.ColourID = int.Parse(rdr.GetValue(5).ToString());
                emp.Price = decimal.Parse(rdr.GetValue(6).ToString());
                emp.ProductName = rdr.GetValue(7).ToString();
                emp.ColourName = rdr.GetValue(8).ToString();

                list.Add(emp);
            }
            rdr.Close();
            return (list);
        }

        public List<Order> getFullOrderData(string status = "Wait for confirmation", string sort = "ASC", string search = "")
        {
            List<Order> list = new List<Order>();
            list = getBasicOrderData(status, sort, search);
            foreach(Order order in list)
            {
                order.orderDetails = getOrderDetailsByID(order.OrderID);
            }
            return (list);
        }

        public int AddToOrder(string UserID, string RecipientName, string RecipPhone, string RecipientAddress, float HandingFee, float Total, DateTime OrderDate)
        {
            int rs = 0;
            string sql = "EXEC dbo.AddToOrder '" + UserID + "'," + HandingFee + "," + Total + ",'" + RecipientAddress + "','" + RecipientName + "','" + RecipPhone + "','" + OrderDate + "'";
            rs = db.ExcuteNonQuery(sql);
            db.close();
            return (rs);
        }

        public string getOrderID(string UserID, string RecipientName, string RecipPhone, string RecipientAddress, float HandingFee, float total)
        {
            string id = "";
            string sql = "SELECT dbo.Get_OrderID('"+ UserID + "', '"+ HandingFee + "', '"+ total + "', " +
                "N'"+ RecipientAddress + "', N'"+ RecipientName + "', '"+ RecipPhone + "') AS ResultOrderID;";

            SqlDataReader rdr = db.ExcuteQuery(sql);

            if (rdr.Read())
            {
                id = rdr.GetValue(0).ToString();
            }
            rdr.Close();
            return (id);
        }
        
        public int AddToOrderDetail(string OrderID, string shoesId, int quantity, int size, string styleType, string colourName, float price)
        {
            int rs = 0;
            string sql = "EXEC dbo.AddOrderDetail '" + OrderID + "', '" + shoesId + "', '" + quantity + "', '" + size + "', '" + styleType + "', '" + colourName + "', '" + price + "'";
            rs = db.ExcuteNonQuery(sql);
            db.close();
            return (rs);
        }

    }
}