using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class ShippingAddress {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string AddressDesc { get; set; }
        public string Recipient { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecInstructions { get; set; }
        public virtual ApplicationUser Customer { get; set; }
    }
}