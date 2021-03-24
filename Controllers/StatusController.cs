using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductionHoursLosses.Models;

namespace ProductionHoursLosses.Controllers
{
    public class StatusController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        // GET: Status
        public ActionResult Index()
        {
            return View(db.STATUS.ToList());
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            STATUS sTATUS = db.STATUS.Find(id);
            if (sTATUS == null)
            {
                return HttpNotFound();
            }
            return View(sTATUS);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME")] STATUS sTATUS)
        {
            if (ModelState.IsValid)
            {
                db.STATUS.Add(sTATUS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sTATUS);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            STATUS sTATUS = db.STATUS.Find(id);
            if (sTATUS == null)
            {
                return HttpNotFound();
            }
            return View(sTATUS);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME")] STATUS sTATUS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sTATUS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sTATUS);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            STATUS sTATUS = db.STATUS.Find(id);
            if (sTATUS == null)
            {
                return HttpNotFound();
            }
            return View(sTATUS);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            STATUS sTATUS = db.STATUS.Find(id);
            db.STATUS.Remove(sTATUS);
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
