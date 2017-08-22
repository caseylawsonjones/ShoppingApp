using Microsoft.AspNet.Identity;
using ShoppingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingApp.Controllers {
    public class Universal : Controller{
        public ApplicationDbContext db = new ApplicationDbContext();


        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            if (User.Identity.IsAuthenticated) {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;
                ViewBag.CartItems = user.CartItems.ToList();
            }

            base.OnActionExecuting(filterContext); //This needs to be here


        }





    }
}