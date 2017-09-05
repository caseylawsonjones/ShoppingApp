using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class Order {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public bool Completed { get; set; }

        // Address information should initially be gathered at the checkout.
        // After functionality is verified, create address input page for user account.
        // Then implement a choice during check out of which address to use, or add new.
        // At that time, the selected/entered address will be copied here for order archiving purposes.
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        //public string Phone { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }


    }
}