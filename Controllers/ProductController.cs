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
using System.Data.Entity.Validation;

namespace ProductionHoursLosses.Controllers
{
    public class ProductController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        // GET: Product
        public ActionResult Index()
        {
            return View(db.PRODUCT.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCT.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CODE,DESCRIPTION")] PRODUCT pRODUCT)
        {
            
            if (ModelState.IsValid)
            {
                var exception = new ExceptionError();
                try
                {
                    db.PRODUCT.Add(pRODUCT);
                    db.SaveChanges();
                    exception.Result = true;
                }
                catch(DbEntityValidationException e)
                {
                    
                    exception.Name = string.Format("Entity has the following validation errors: {0} {1}\n", e.Message, e.InnerException);

                    foreach (var x in e.EntityValidationErrors)
                    {
                        exception.Name += string.Format("Property name: {0}, Message {1}\n", x.ValidationErrors.FirstOrDefault().PropertyName, x.ValidationErrors.FirstOrDefault().ErrorMessage);
                    }
                    exception.Result = false;
                }
                
                if(exception.Result)
                {
                    SharedHelp.CommonFunctions.CreateLog("CREATE", "PRODUCT", db.PRODUCT.Max(x => x.ID), null, string.Concat(pRODUCT.CODE, " / ", pRODUCT.DESCRIPTION), User.Identity.Name);
                }


                    return RedirectToAction("Index");
            }

            return View(pRODUCT);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCT.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CODE,DESCRIPTION")] PRODUCT pRODUCT)
        {
            if (ModelState.IsValid)
            {
                PRODUCT old_prod = db.PRODUCT.AsNoTracking().FirstOrDefault(x => x.ID == pRODUCT.ID);
                db.Entry(pRODUCT).State = EntityState.Modified;
                db.SaveChanges();

                SharedHelp.CommonFunctions.CreateLog("EDIT", "PRODUCT", pRODUCT.ID, string.Concat(old_prod.CODE, " / ", old_prod.DESCRIPTION), string.Concat(pRODUCT.CODE, " / ", pRODUCT.DESCRIPTION), User.Identity.Name);

                return RedirectToAction("Index");
            }
            return View(pRODUCT);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCT.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PRODUCT pRODUCT = db.PRODUCT.Find(id);
            db.PRODUCT.Remove(pRODUCT);
            db.SaveChanges();

            SharedHelp.CommonFunctions.CreateLog("DELETE", "PRODUCT", pRODUCT.ID, string.Concat(pRODUCT.CODE, " / ", pRODUCT.DESCRIPTION), null, User.Identity.Name);

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
