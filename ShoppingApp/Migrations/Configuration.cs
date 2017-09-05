namespace ShoppingApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ShoppingApp.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingApp.Models.ApplicationDbContext>
    {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context) {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin")) {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "caseylawsonjones@yahoo.com")) {
                userManager.Create(new ApplicationUser {
                    UserName = "caseylawsonjones@yahoo.com",
                    Email = "caseylawsonjones@yahoo.com",
                }, "CoderFoundry1!");
            }
            var userID = userManager.FindByEmail("caseylawsonjones@yahoo.com").Id;
            userManager.AddToRole(userID, "Admin");
        }
    }
}
