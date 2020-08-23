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
    public class AspNetOrdersController : Controller
    {
        private BingCC_Entities db = new BingCC_Entities();

        // OrderDate,OrderFreight,OrderTotalPrice,UserId
        public ActionResult SucceedOrder(
            DateTime orderDate, float orderFreight, float orderTotal, string userId)
        {
            AspNetOrders aspNetOrder = new AspNetOrders();
            aspNetOrder.OrderDate = orderDate;
            aspNetOrder.OrderFreight = orderFreight;
            aspNetOrder.OrderTotalPrice = orderTotal;
            aspNetOrder.UserId = userId;
            if (ModelState.IsValid)
            {
                // add order record to order list
                db.AspNetOrders.Add(aspNetOrder);
                db.SaveChanges();
                // clean user's cart
                // and add items to ordered item list
                while (db.AspNetCartProducts.FirstOrDefault(entry => entry.UserId == userId) != null)
                {                   
                    AspNetCartProducts itemInCart = db.AspNetCartProducts.FirstOrDefault(entry => entry.UserId == userId);
                    db.AspNetCartProducts.Remove(itemInCart);

                    AspNetOrderProducts itemInOrder = new AspNetOrderProducts();
                    itemInOrder.OrderId = aspNetOrder.OrderId;
                    itemInOrder.ProductId = itemInCart.ProductId;
                    itemInOrder.UserId = userId;
                    db.AspNetOrderProducts.Add(itemInOrder);

                    db.SaveChanges();
                }
                return View();
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetOrder.UserId);
            return View();
        }

        //public ActionResult SucceedOrder()
        //{
        //    return View();
        //}
        
        // GET: AspNetOrders
        public ActionResult Index()
        {
            var aspNetOrders = db.AspNetOrders.Include(a => a.AspNetUsers);
            return View(aspNetOrders.ToList());
        }

        // GET: AspNetOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetOrders aspNetOrders = db.AspNetOrders.Find(id);
            if (aspNetOrders == null)
            {
                return HttpNotFound();
            }

            // get bought items in the order, and store them in ViewBag
            // -----
            // first, get OrderProducts entries
            IEnumerable<AspNetOrderProducts> itemIdInOrder =
                db.AspNetOrderProducts.Where(entry => entry.OrderId == id);
            // then get Products entries according to OrderProducts entries
            IList<AspNetProducts> itemsInOrder = new List<AspNetProducts>();
            foreach (var item in itemIdInOrder)
            {
                AspNetProducts product = db.AspNetProducts.FirstOrDefault(
                    entry => entry.ProductId == item.ProductId);
                itemsInOrder.Add(product);
            }
            ViewBag.itemsInOrder = itemsInOrder;

            return View(aspNetOrders);
        }

        // GET: AspNetOrders/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: AspNetOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult Create([Bind(Include = "OrderId,OrderDate,OrderFreight,OrderTotalPrice,UserId")] AspNetOrders aspNetOrders)
        {
            if (ModelState.IsValid)
            {
                db.AspNetOrders.Add(aspNetOrders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetOrders.UserId);
            return View(aspNetOrders);
        }

        // GET: AspNetOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetOrders aspNetOrders = db.AspNetOrders.Find(id);
            if (aspNetOrders == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetOrders.UserId);
            return View(aspNetOrders);
        }

        // POST: AspNetOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderDate,OrderFreight,OrderTotalPrice,UserId")] AspNetOrders aspNetOrders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetOrders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetOrders.UserId);
            return View(aspNetOrders);
        }

        // GET: AspNetOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetOrders aspNetOrders = db.AspNetOrders.Find(id);
            if (aspNetOrders == null)
            {
                return HttpNotFound();
            }


            // in order to show items in order on delete confirm page
            // store items in order into ViewBag
            IEnumerable<AspNetOrderProducts> itemIdInOrder =
                db.AspNetOrderProducts.Where(entry => entry.OrderId == id);
            IList<AspNetProducts> itemsInOrder = new List<AspNetProducts>();
            foreach (var item in itemIdInOrder)
            {
                AspNetProducts product = db.AspNetProducts.FirstOrDefault(
                    entry => entry.ProductId == item.ProductId);
                itemsInOrder.Add(product);
            }
            ViewBag.itemsInOrder = itemsInOrder;


            return View(aspNetOrders);
        }

        // POST: AspNetOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // delete items stored in AspNetOrderProducts
            while (db.AspNetOrderProducts.FirstOrDefault(
                entry => entry.OrderId == id) != null)
            {
                AspNetOrderProducts itemToDelete =
                    db.AspNetOrderProducts.FirstOrDefault(entry => entry.OrderId == id);
                db.AspNetOrderProducts.Remove(itemToDelete);
                db.SaveChanges();
            }

            AspNetOrders aspNetOrders = db.AspNetOrders.Find(id);
            db.AspNetOrders.Remove(aspNetOrders);
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
