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
                //ViewBag.CartItems = db.CartItems.AsNoTracking().Where(c => c.CustomerID == user.Id).ToList();
                //ViewBag.ItemTypes = db.ItemTypes.AsNoTracking().OrderBy(testc => testc.TypeName).ToList();
                //db.CartItems.AsNoTracking().whatever - this can be used to read the db and then close behind it
                //I'm not sure what he was doing as it never was an issue for me.
                //The Lambda statement "=>" basically translatest to grab every item that meets the requirement and put in a list.
            }

            base.OnActionExecuting(filterContext); //This needs to be here


        }





    }
}