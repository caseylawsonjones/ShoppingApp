﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class ShippingAddress {
        public string ID { get; set; }
        public string AddressDesc { get; set; }
        public string Recipient { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecInstructions { get; set; }



    }
}