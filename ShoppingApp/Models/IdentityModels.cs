using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShoppingApp.Models.CodeFirst;
using System.Collections.Generic;

namespace ShoppingApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // PROPERTIES
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {  // just a Get, so it won't be in the database, but will be available for coding purposes
            get {
                return FirstName = " " + LastName;
            }
        }
        public string EmailAddress { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }

        // CONSTRUCTORS
        public ApplicationUser() {
            this.Orders = new HashSet<Order>();
            this.CartItems = new HashSet<CartItem>();
            this.OrderItems = new HashSet<OrderItem>();
            this.ShippingAddresses = new HashSet<ShippingAddress>();
            this.PaymentMethods = new HashSet<PaymentMethod>();
        }

        // METHODS
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        // This builds the table, based on the classes below
        // instantiate this class everytime you access the database
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false) {
        }

        public static ApplicationDbContext Create() { 
            return new ApplicationDbContext();
        }
        // You have to add all classes
        public DbSet<Item> Items { get; set; } //You pluralize the name here because it does it itself somewhere else
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }



    }
}