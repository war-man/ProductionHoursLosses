using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductionHoursLosses.Models;
using ProductionHoursLosses.SharedHelp;

namespace ProductionHoursLosses.Controllers
{
    public class FactoryController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        // GET: Factory
        public ActionResult Index()
        {
            return View(db.FACTORY.ToList());
        }

        // GET: Factory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTORY fACTORY = db.FACTORY.Find(id);
            if (fACTORY == null)
            {
                return HttpNotFound();
            }
            return View(fACTORY);
        }

        // GET: Factory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Factory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,ADDRESS")] FACTORY fACTORY)
        {
            if (ModelState.IsValid)
            {
                db.FACTORY.Add(fACTORY);
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("CREATE", "FACTORY", db.FACTORY.Max(x => x.ID), null, string.Concat(fACTORY.NAME, " / ", fACTORY.ADDRESS), User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(fACTORY);
        }

        // GET: Factory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTORY fACTORY = db.FACTORY.Find(id);
            if (fACTORY == null)
            {
                return HttpNotFound();
            }
            return View(fACTORY);
        }

        // POST: Factory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,ADDRESS")] FACTORY fACTORY)
        {
            if (ModelState.IsValid)
            {
                FACTORY old_fact = db.FACTORY.AsNoTracking().FirstOrDefault(x => x.ID == fACTORY.ID);
                db.Entry(fACTORY).State = EntityState.Modified;
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("EDIT", "FACTORY", fACTORY.ID, string.Concat(old_fact.NAME, " / ", old_fact.ADDRESS), string.Concat(fACTORY.NAME, " / ", fACTORY.ADDRESS), User.Identity.Name);

                return RedirectToAction("Index");
            }
            return View(fACTORY);
        }

        // GET: Factory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FACTORY fACTORY = db.FACTORY.Find(id);
            if (fACTORY == null)
            {
                return HttpNotFound();
            }
            return View(fACTORY);
        }

        // POST: Factory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FACTORY fACTORY = db.FACTORY.Find(id);
            db.FACTORY.Remove(fACTORY);
            db.SaveChanges();

            SharedHelp.CommonFunctions.CreateLog("DELETE", "FACTORY", fACTORY.ID, string.Concat(fACTORY.NAME, " / ", fACTORY.ADDRESS), null, User.Identity.Name);

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
