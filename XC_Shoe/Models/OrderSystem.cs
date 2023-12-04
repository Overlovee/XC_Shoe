using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class OrderSystem
    {
        public string OrderID { get; set; }
        public string EmployeeID { get; set; } = "";
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Wait for confirmation";

        public OrderSystem() { 
            OrderID = "Order0";
            EmployeeID = "";
            OrderDate = DateTime.Now;
            Status = "Wait for confirmation";
        }
    }
}