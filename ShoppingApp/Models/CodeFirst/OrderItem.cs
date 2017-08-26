using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class OrderItem {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreationDate { get; set; } // This will be used in the order archive database, when implemented

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public decimal unitTotal {
            get {
                return Count * Item.Price;
            }

        }
    }