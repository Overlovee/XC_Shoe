using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class Order
    {
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string PaymentInfo { get; set; } = "Payment in cash";
        public decimal? EstimatedDeliveryHandlingFee { get; set; }
        public string Email { get; set; }
        public decimal? Total { get; set; }
        public string PaymentStatus { get; set; } = "Unpaid";
        public string RecipientAddress { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhoneNumber { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderDetails> orderDetails { get; set; }
        public OrderSystem orderSystem { get; set; }
        public Order()
        {
            orderDetails = new List<OrderDetails>();
            orderSystem = new OrderSystem();
        }
    }
}