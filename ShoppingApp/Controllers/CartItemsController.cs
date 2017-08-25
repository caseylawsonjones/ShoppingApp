using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingApp.Models;
using ShoppingApp.Models.CodeFirst;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ShoppingApp.Controllers
{
    public class CartItemsController : Universal
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        //This was commented out because the "Universal" controller, that was created as a parent controller for all
        //controllers in this project, contains the statement there and is inherited by all child controllers

            
        // GET: CartItems
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            decimal CartTotal = 0;
            foreach(var i in user.CartItems) {
                CartTotal += (i.Count * i.Item.Price);
            }
            ViewBag.CartTotal = CartTotal;
            return View(user.CartItems.ToList());
        }

        // GET: CartItems/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }


        // POST: CartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int? itemIDin) {
            if (itemIDin == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item itemIn = db.Items.Find(itemIDin);
            if (itemIn == null) {
                return HttpNotFound();
            }
            // If item exist in cart already, increment counter, do not add
            bool itemInCart = false;  //Boolean value to determine if item in in cart.  DB cannot be saved within foreach loop.
            foreach (var item1 in db.CartItems) {
                if (item1.CustomerID == User.Identity.GetUserId()) {
                    if (item1.ItemID == itemIDin) {
                        item1.Count++;
                        itemInCart = true;
                    }
                }
            }
            if (itemInCart) {
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
            else {
                CartItem newItem = new CartItem();
                newItem.CustomerID = User.Identity.GetUserId();
                newItem.ItemID = itemIn.ID;
                newItem.Count = 1;
                newItem.CreationDate = DateTime.Now;
                db.CartItems.Add(newItem);
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult IncrementQuant(int itemIDin) {
            CartItem itemIn = db.CartItems.Find(itemIDin);
            itemIn.Count++;
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DecrementQuant(int itemIDin) {
            CartItem itemIn = db.CartItems.Find(itemIDin);
            itemIn.Count--;
            if(itemIn.Count >= 0) { RemoveItem(itemIDin); }
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult RemoveItem(int itemIDin) {
            CartItem itemIn = db.CartItems.Find(itemIDin);
            if (itemIn == null) {
                return HttpNotFound();
            }
            db.CartItems.Remove(itemIn);
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }























        // GET: CartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerID,ItemID,Count,CreationDate")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
