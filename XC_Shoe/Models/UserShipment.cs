using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XC_Shoe.Models
{
    public class UserShipment
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecificAddress { get; set; }
        public string AdministrativeBoundaries { get; set; }
        public bool IsDefault { get; set; }
    }
}