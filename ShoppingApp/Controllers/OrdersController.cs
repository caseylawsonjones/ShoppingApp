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
using System.Collections;

namespace ShoppingApp.Controllers
{
    public class OrdersController : Universal
    {
        //private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        //public ActionResult Index()
        //{
        //    return View(db.Orders.ToList());
        //}

        // GET: Orders/Details/5

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        // GET: Orders/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}




        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int orderShippingAddressId, int orderPaymentMethodId, decimal orderTotal) {
            if (ModelState.IsValid) {
                // orderPaymentMethodIdString is not actually used until the payment processing method is created.
                Order order = new Order();
                var user = db.Users.Find(User.Identity.GetUserId());
                var tempShippingAddress = db.ShippingAddresses.Find(orderShippingAddressId);

                order.Address = tempShippingAddress.StreetAddress;
                order.City = tempShippingAddress.City;
                order.State = tempShippingAddress.State;
                order.ZipCode = tempShippingAddress.ZipCode;
                order.Total = orderTotal;
                order.OrderDate = System.DateTime.Now;
                order.CustomerId = user.Id;
                order.Country = "USA";
                order.Completed = true;
                db.Orders.Add(order);
                db.SaveChanges();

                var tempPaymentMethod = db.PaymentMethods.Find(orderPaymentMethodId);
                ViewBag.CardBrand = tempPaymentMethod.CardBrand;

                // Add the OrderId to each of the OrderItems. Only current OrderItems will have a null value for this
                foreach (var i in user.OrderItems) {
                    if (i.OrderId == null) {
                        i.OrderId = order.Id;
                    }
                }

                // Clear the CartItems
                foreach(CartItem c in user.CartItems.ToList()) {
                    db.CartItems.Remove(c);
                }
                    db.SaveChanges();

                return View("Index", order);
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }
        // UNUSED METHODS
        // GET: Orders/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Completed,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        // GET: Orders/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        // POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
