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


namespace ShoppingApp.Controllers
{
    public class OrderItemsController : Universal
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            decimal OrderTotal = 0;
            OrderItem newOrderItem = null;
            if (ModelState.IsValid) {
                // An Order is never created until a successfull checkout.
                // Any previous clicks of Proceed to Checkout that did not complete an order
                // will leave remainders and result in duplicates or unwanted items.
                // Items that were successfully ordered in the past will have an associated 
                // order number assigned to the OrderID property.  Any items that were not 
                // successfully ordered will have a null value for OrderId.  This foreach loop
                // will clear those items to avoid the problem.  Items in the car will be added
                // in the second foreach loop.
                foreach (OrderItem p in user.OrderItems.ToList()) {
                    if (p.OrderId == null) {
                        db.OrderItems.Remove(p);
                        db.SaveChanges();
                    }
                }
                foreach (var c in user.CartItems) {
                    newOrderItem = new OrderItem();
                    newOrderItem.CustomerId = User.Identity.GetUserId();
                    newOrderItem.ItemId = c.Item.Id;
                    newOrderItem.Count = c.Count;
                    newOrderItem.UnitPrice = c.Item.Price;
                    OrderTotal += (newOrderItem.Count * newOrderItem.UnitPrice);
                    db.OrderItems.Add(newOrderItem);
                    db.SaveChanges();
                }
            }
            // Determines the default shipping address and returns a false boolean if no
            // shipping addresses are present for the user.
            if (user.ShippingAddresses.Count > 0) {
                foreach (var a in user.ShippingAddresses) {
                    if (a.Default) {
                        ViewBag.ShippingAddress = a;
                        ViewBag.ShippingAddress.IsEmpty = false;
                    }
                    else {
                        ViewBag.ShippingAddress.IsEmpty = true;
                    }
                }
            }
            // Determines the default payment method and returns a false boolean if no
            // payment methods are present for the user.
            if (user.PaymentMethods.Count > 0) {
                foreach (var m in user.PaymentMethods) {
                    if (m.Default) {
                        ViewBag.PaymentMethods = m;
                        ViewBag.PaymentMethods.IsEmpty = false;
                    }
                    else {
                        ViewBag.PaymentMethods.IsEmpty = true;
                    }
                }
            }

            ViewBag.OrderTotal = OrderTotal;
            ViewBag.User = user;
            return View(user.OrderItems.ToList());
        }

        //// GET: OrderItems/Details/5
        //[Authorize]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OrderItem orderItem = db.OrderItems.Find(id);
        //    if (orderItem == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(orderItem);
        //}

        // GET: OrderItems/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    var user = db.Users.Find(User.Identity.GetUserId());
        //    decimal OrderTotal = 0;
        //    OrderItem newOrderItem = null;
        //    foreach (var c in user.CartItems) {
        //        newOrderItem = new OrderItem();
        //        newOrderItem.CustomerId = User.Identity.GetUserId();
        //        newOrderItem.ItemId = c.Item.Id;
        //        newOrderItem.Count = c.Count;
        //        newOrderItem.UnitPrice = c.Item.Price;
        //        OrderTotal += (newOrderItem.Count * newOrderItem.UnitPrice);
        //        db.OrderItems.Add(newOrderItem);
        //        db.SaveChanges();
        //    }
        //    ViewBag.OrderTotal = OrderTotal;
        //    return View(user.OrderItems.ToList());

        //    //ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
        //    //ViewBag.OrderId = new SelectList(db.Orders, "Id", "Address");
        //    //return View();
        //}

        // POST: OrderItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create() {
        //public ActionResult Create([Bind(Include = "Id,OrderId,ItemId,Count,UnitPrice")] OrderItem orderItem)
            var user = db.Users.Find(User.Identity.GetUserId());
            decimal OrderTotal = 0;
            OrderItem newOrderItem = null;
            if (ModelState.IsValid) {
                foreach (var c in user.CartItems) {
                    newOrderItem = new OrderItem();
                    newOrderItem.CustomerId = User.Identity.GetUserId();
                    newOrderItem.ItemId = c.Item.Id;
                    newOrderItem.Count = c.Count;
                    newOrderItem.UnitPrice = c.Item.Price;
                    OrderTotal += (newOrderItem.Count * newOrderItem.UnitPrice);
                    db.OrderItems.Add(newOrderItem);
                    db.SaveChanges();
                }
            }
                ViewBag.OrderTotal = OrderTotal;
                return Index();

            //ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", orderItem.ItemId);
            //ViewBag.OrderId = new SelectList(db.Orders, "Id", "Address", orderItem.OrderId);
            //return View(orderItem);
        }

        // GET: OrderItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", orderItem.ItemId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Address", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,ItemId,Quantity,UnitPrice")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", orderItem.ItemId);
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "Address", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderItem orderItem = db.OrderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderItem orderItem = db.OrderItems.Find(id);
            db.OrderItems.Remove(orderItem);
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
