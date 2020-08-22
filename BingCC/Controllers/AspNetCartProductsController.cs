using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BingCC.Models;

namespace BingCC.Controllers
{
    public class AspNetCartProductsController : Controller
    {
        private BingCC_Entities db = new BingCC_Entities();

        // GET: AspNetCartProducts
        public ActionResult Index()
        {
            var aspNetCartProducts = db.AspNetCartProducts.Include(a => a.AspNetProducts).Include(a => a.AspNetUsers);
            return View(aspNetCartProducts.ToList());
        }

        // GET: AspNetCartProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCartProducts aspNetCartProducts = db.AspNetCartProducts.Find(id);
            if (aspNetCartProducts == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCartProducts);
        }

        //public ActionResult SucceedAddToCart()
        //{
        //    return View();
        //}

        public ActionResult SucceedAddToCart(string userId, int? productId)
        {
            
            AspNetCartProducts aspNetCartProducts = new AspNetCartProducts();
            aspNetCartProducts.UserId = userId;
            aspNetCartProducts.ProductId = (int)productId;
            if (db.AspNetCartProducts.SingleOrDefault(entry => entry.UserId == userId && entry.ProductId == productId) == null)
            {
                db.AspNetCartProducts.Add(aspNetCartProducts);
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("FailAddToCart");
            }
        }

        public ActionResult FailAddToCart()
        {
            return View();
        }

        public ActionResult ConfirmOrder()
        {
            var aspNetCartProducts = db.AspNetCartProducts.Include(a => a.AspNetProducts).Include(a => a.AspNetUsers);
            return View(aspNetCartProducts.ToList());
        }


        // GET: AspNetCartProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetCartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "CartProductId,UserId,ProductId")] AspNetCartProducts aspNetCartProducts)
        {
            if (ModelState.IsValid)
            {
                db.AspNetCartProducts.Add(aspNetCartProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.AspNetProducts, "ProductId", "ProductName", aspNetCartProducts.ProductId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetCartProducts.UserId);
            return View(aspNetCartProducts);
        }

        // GET: AspNetCartProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCartProducts aspNetCartProducts = db.AspNetCartProducts.Find(id);
            if (aspNetCartProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.AspNetProducts, "ProductId", "ProductName", aspNetCartProducts.ProductId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetCartProducts.UserId);
            return View(aspNetCartProducts);
        }

        // POST: AspNetCartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartProductId,UserId,ProductId")] AspNetCartProducts aspNetCartProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetCartProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.AspNetProducts, "ProductId", "ProductName", aspNetCartProducts.ProductId);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetCartProducts.UserId);
            return View(aspNetCartProducts);
        }

        // GET: AspNetCartProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCartProducts aspNetCartProducts = db.AspNetCartProducts.Find(id);
            if (aspNetCartProducts == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCartProducts);
        }

        // POST: AspNetCartProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetCartProducts aspNetCartProducts = db.AspNetCartProducts.Find(id);
            db.AspNetCartProducts.Remove(aspNetCartProducts);
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
