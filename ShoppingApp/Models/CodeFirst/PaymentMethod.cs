using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class PaymentMethod {
        public int Id { get; set; }
        public Boolean Default { get; set; }
        public string CardNameFirst { get; set; }
        public string CardNameLast { get; set; }
        public string CardBrand { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode {get; set;}
        public DateTime ExpirationDate { get; set; }
        // Billing Address
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}