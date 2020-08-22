﻿using System;
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
            AspNetOrders aspNetOrders = new AspNetOrders();
            aspNetOrders.OrderDate = orderDate;
            aspNetOrders.OrderFreight = orderFreight;
            aspNetOrders.OrderTotalPrice = orderTotal;
            aspNetOrders.UserId = userId;
            if (ModelState.IsValid)
            {
                // add order record to order lisr
                db.AspNetOrders.Add(aspNetOrders);
                db.SaveChanges();
                // TODO clean user's cart
                while (db.AspNetCartProducts.FirstOrDefault(entry => entry.UserId == userId) != null)
                {
                    AspNetCartProducts itemInCart = db.AspNetCartProducts.FirstOrDefault(entry => entry.UserId == userId);
                    db.AspNetCartProducts.Remove(itemInCart);
                    db.SaveChanges();
                }
                return View();
            }

            //ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", aspNetOrders.UserId);
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
            return View(aspNetOrders);
        }

        // POST: AspNetOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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
