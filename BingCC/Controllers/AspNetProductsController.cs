using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BingCC.Models;

namespace BingCC.Controllers
{
    public class AspNetProductsController : Controller
    {
        private BingCC_Entities db = new BingCC_Entities();

        // GET: AspNetProducts
        public ActionResult Index()
        {
            return View(db.AspNetProducts.ToList());
        }

        public ActionResult IndexAdmin()
        {
            return View(db.AspNetProducts.ToList());
        }

        // GET: AspNetProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProducts aspNetProducts = db.AspNetProducts.Find(id);
            if (aspNetProducts == null)
            {
                return HttpNotFound();
            }
            return View(aspNetProducts);
        }

        // GET: AspNetProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductPrice,ProductIntro,ProductImage")] AspNetProducts aspNetProducts)
        {
            if (ModelState.IsValid)
            {
                db.AspNetProducts.Add(aspNetProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetProducts);
        }

        // GET: AspNetProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProducts aspNetProducts = db.AspNetProducts.Find(id);
            if (aspNetProducts == null)
            {
                return HttpNotFound();
            }
            return View(aspNetProducts);
        }

        // POST: AspNetProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductPrice,ProductIntro,ProductImage")] AspNetProducts aspNetProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(aspNetProducts);
        }

        // GET: AspNetProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetProducts aspNetProducts = db.AspNetProducts.Find(id);
            if (aspNetProducts == null)
            {
                return HttpNotFound();
            }
            return View(aspNetProducts);
        }

        // POST: AspNetProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetProducts aspNetProducts = db.AspNetProducts.Find(id);
            db.AspNetProducts.Remove(aspNetProducts);
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
