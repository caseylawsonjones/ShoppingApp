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
using System.IO;
using Microsoft.AspNet.Identity;

namespace ShoppingApp.Controllers
{
    public class ItemsController : Universal
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        //This was commented out because the "Universal" controller, that was created as a parent controller for all
        //controllers in this project, contains the statement there and is inherited by all child controllers
        
            
        // GET: Items
        // INDEX
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Items/Details/5
        // DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            string tempPath = HttpContext.Request.Url.AbsoluteUri;
            return View(item);
        }

        //***************************************************************************************
        //***************************************************************************************
        // GET: Items/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }



        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaURL,Description")] Item item, HttpPostedFileBase image)
        {
            bool noImage = false;
            if (image != null && image.ContentLength > 0) {
                var ext = Path.GetExtension(image.FileName).ToLower(); // Gets image's extension and then sets it to lower case
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp") {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            else {
                noImage = true;
            }
            if (ModelState.IsValid) {
                var filePath = "/Assets/Images/";
                var absPath = Server.MapPath("~" + filePath);
                if (noImage) {
                    item.MediaURL = "/Assets/Images/Coming Soon.jpg";
                }
                else {
                    item.MediaURL = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
                item.CreationDate = System.DateTime.Now;
                item.UpdatedDate = System.DateTime.Now;
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Create"); //Returns to Create action to facilitate multiple item creations
            }
            return View(item);
        }

        //***************************************************************************************
        //***************************************************************************************
        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }


        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaURL,Description")] Item item, string mediaURL, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0) {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp") {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            if (ModelState.IsValid) {
                db.Entry(item).State = EntityState.Modified;
                if (image != null) {
                    var filePath = "/Assets/Images/";
                    var absPath = Server.MapPath("~" + filePath);
                    item.MediaURL = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
            else {
                //If the image value is null, insert the previous image rather than throw an error
                //This sort of handling could be very nice if making small edits on multiple items.
                //It would keep the user from having to re-establish the image for every single item.
                item.MediaURL = mediaURL;
            }

                item.UpdatedDate = System.DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //***************************************************************************************
        //***************************************************************************************
        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            var absPath = Server.MapPath("~" + item.MediaURL);
            System.IO.File.Delete(absPath);
            db.Items.Remove(item);
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
