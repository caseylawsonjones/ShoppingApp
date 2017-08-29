using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class OrderItem {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int? OrderId { get; set; }

        public int ItemId { get; set; }
        public int Count { get; set; }
        // UnitPrice needs to be here for archival purposes.
        // The actual item price may change and/or special pricing may be in effect.
        public decimal UnitPrice { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        // unitTotal needs to be based on the UnitPrice, not Item.Price.
        // This would allow for special pricing and for accurate archival records, 
        // based on the price at the time of purchase.
        public decimal unitTotal {
            get {
                return Count * UnitPrice;
            }

        }
    }
}