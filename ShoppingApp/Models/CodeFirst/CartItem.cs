using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingApp.Models.CodeFirst {
    public class CartItem {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public int ItemID { get; set; }  // Foreign key
        public int Count { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual Item Item { get; set; }  // Has to have the same name as the Foreign key - the ID part - apparently they system automatically reads the virtual Item based off of the other.
        public virtual ApplicationUser Customer { get; set; }

        public decimal unitTotal {
            get {
                return Count * Item.Price;
            }
        }

    }
}