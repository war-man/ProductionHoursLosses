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
    public class RoomController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        // GET: Room
        public ActionResult Index()
        {
            var rOOM = db.ROOM.Include(r => r.FACTORY);
            return View(rOOM.ToList());
        }

        // GET: Room/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOM.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            return View(rOOM);
        }

        // GET: Room/Create
        public ActionResult Create()
        {
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME");
            return View();
        }

        // POST: Room/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NAME,FACTORY_ID")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                rOOM.FACTORY = db.FACTORY.FirstOrDefault(X => X.ID == rOOM.FACTORY_ID);
                db.ROOM.Add(rOOM);
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("CREATE", "ROOM", db.ROOM.Max(x => x.ID), null, string.Concat(rOOM.NAME, " / ", rOOM.FACTORY.NAME), User.Identity.Name);

                return RedirectToAction("Index");
            }

            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", rOOM.FACTORY_ID);
            return View(rOOM);
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOM.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", rOOM.FACTORY_ID);
            return View(rOOM);
        }

        // POST: Room/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NAME,FACTORY_ID")] ROOM rOOM)
        {
            if (ModelState.IsValid)
            {
                ROOM old_val = db.ROOM.AsNoTracking().FirstOrDefault(x => x.ID == rOOM.ID);
                rOOM.FACTORY = db.FACTORY.FirstOrDefault(X => X.ID == rOOM.FACTORY_ID);
                db.Entry(rOOM).State = EntityState.Modified;
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("EDIT", "ROOM", rOOM.ID, string.Concat(old_val.NAME, " / ", old_val.FACTORY.NAME), string.Concat(rOOM.NAME, " / ", rOOM.FACTORY.NAME), User.Identity.Name);

                return RedirectToAction("Index");
            }
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", rOOM.FACTORY_ID);
            return View(rOOM);
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ROOM rOOM = db.ROOM.Find(id);
            if (rOOM == null)
            {
                return HttpNotFound();
            }
            return View(rOOM);
        }

        // POST: Room/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ROOM rOOM = db.ROOM.Find(id);
            db.ROOM.Remove(rOOM);
            db.SaveChanges();

            rOOM.FACTORY = db.FACTORY.FirstOrDefault(X => X.ID == rOOM.FACTORY_ID);
            SharedHelp.CommonFunctions.CreateLog("DELETE", "ROOM", rOOM.ID, string.Concat(rOOM.NAME, " / ", rOOM.FACTORY.NAME), null, User.Identity.Name);

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
