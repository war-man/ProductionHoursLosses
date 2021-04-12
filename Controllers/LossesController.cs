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
    public class LossesController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        // GET: losses
        public ActionResult Index()
        {
            return View(db.LOSSES.ToList());
        }

        // GET: losses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOSSES losses = db.LOSSES.Find(id);
            if (losses == null)
            {
                return HttpNotFound();
            }
            return View(losses);
        }

        // GET: losses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: losses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CODE,DESCRIPTION")] LOSSES losses)
        {
            if (ModelState.IsValid)
            {
                db.LOSSES.Add(losses);
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("CREATE", "LOSSES", db.LOSSES.Max(x => x.ID), null, string.Concat(losses.CODE, " / ", losses.DESCRIPTION), User.Identity.Name);

                return RedirectToAction("Index");
            }

            return View(losses);
        }

        // GET: losses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOSSES losses = db.LOSSES.Find(id);
            if (losses == null)
            {
                return HttpNotFound();
            }
            return View(losses);
        }

        // POST: losses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CODE,DESCRIPTION")] LOSSES losses)
        {
            if (ModelState.IsValid)
            {
                LOSSES OLD_VAL = db.LOSSES.AsNoTracking().FirstOrDefault(x => x.ID == losses.ID);
                db.Entry(losses).State = EntityState.Modified;
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("EDIT", "LOSSES", losses.ID, string.Concat(OLD_VAL.CODE, " / ", OLD_VAL.DESCRIPTION), string.Concat(losses.CODE, " / ", losses.DESCRIPTION), User.Identity.Name);

                return RedirectToAction("Index");
            }
            return View(losses);
        }

        // GET: losses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOSSES losses = db.LOSSES.Find(id);
            if (losses == null)
            {
                return HttpNotFound();
            }
            return View(losses);
        }

        // POST: losses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LOSSES losses = db.LOSSES.Find(id);
            db.LOSSES.Remove(losses);
            db.SaveChanges();

            SharedHelp.CommonFunctions.CreateLog("DELETE", "LOSSES", losses.ID, string.Concat(losses.CODE, " / ", losses.DESCRIPTION), null, User.Identity.Name);

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
