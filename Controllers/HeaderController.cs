using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductionHoursLosses.Models;
using ProductionHoursLosses.Models.ViewModels;

namespace ProductionHoursLosses.Controllers
{
    public class HeaderController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        private HeaderViewModel InitializeModel(HeaderViewModel model)
        {

            return model;
        }

        // GET: Header
        public ActionResult Index(HeaderViewModel model)
        {
            if (model == null)
                model = new HeaderViewModel();

            //model = InitializeModel(model);
            var hEADER = db.HEADER.Include(h => h.FACTORY).Include(h => h.ROOM).Include(h => h.STATUS);
            return View(hEADER.ToList());
        }

        // GET: Header/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HEADER hEADER = db.HEADER.Find(id);
            if (hEADER == null)
            {
                return HttpNotFound();
            }
            return View(hEADER);
        }

        // GET: Header/Create
        public ActionResult Create(HeaderViewModel model)
        {
            if (model == null)
                model = new HeaderViewModel();

            //model = InitializeModel(model);
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME");
            ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME");
            ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME");
            return View("Create", model);
        }

        // POST: Header/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DATE,FACTORY_ID,ROOM_ID,AVAIL_HRS,STATUS_ID")] HEADER hEADER)
        {
            if (ModelState.IsValid)
            {
                db.HEADER.Add(hEADER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
            ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
            ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
            return View(hEADER);
        }

        // GET: Header/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HEADER hEADER = db.HEADER.Find(id);
            if (hEADER == null)
            {
                return HttpNotFound();
            }
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
            ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
            ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
            return View(hEADER);
        }

        // POST: Header/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DATE,FACTORY_ID,ROOM_ID,AVAIL_HRS,STATUS_ID")] HEADER hEADER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hEADER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
            ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
            ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
            return View(hEADER);
        }

        // GET: Header/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HEADER hEADER = db.HEADER.Find(id);
            if (hEADER == null)
            {
                return HttpNotFound();
            }
            return View(hEADER);
        }

        // POST: Header/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HEADER hEADER = db.HEADER.Find(id);
            db.HEADER.Remove(hEADER);
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

        public JsonResult GetFactoryList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            var list = db.FACTORY
                .Where(x => x.NAME.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.NAME,
                            itemDescription = y.ADDRESS
                        })
                        .Distinct()
                        .ToList();

            if (page == 1)
                results.results.AddRange(list.Take(30));
            else
                results.results.AddRange(list.Skip(page * 30).Take(30));

            if (page * 30 < list.Count)
                results.pagination.more = true;
            else
                results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRoomList(string q, int page, int FactoryId)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            if (FactoryId == 0)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            var dbItem = db.FACTORY.Find(FactoryId);
            if (dbItem == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            List<ItemIdText> list = new List<ItemIdText>();

            if (FactoryId != 0)
            {
                list = db.ROOM
                    .Where(x => x.NAME.ToLower().StartsWith(q) && x.FACTORY_ID == FactoryId)
                        .Select(y =>
                            new ItemIdText
                            {
                                id = y.ID.ToString(),
                                text = y.NAME,
                                itemDescription = y.FACTORY.NAME
                            })
                            .Distinct()
                            .ToList();
            }
            else
            {
                list = db.ROOM
                .Where(x => x.NAME.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.NAME
                        })
                        .Distinct()
                        .ToList();
            }


            if (page == 1)
                results.results.AddRange(list.Take(30));
            else
                results.results.AddRange(list.Skip(page * 30).Take(30));

            if (page * 30 < list.Count)
                results.pagination.more = true;
            else
                results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailHrsList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            List<ItemIdText> list = new List<ItemIdText>
            {
                new ItemIdText{id = "8", text = "8 hrs"},
                new ItemIdText{id = "16", text = "16 hrs"},
                new ItemIdText{id = "24", text = "24 hrs"},
            };

            results.results.AddRange(list.Take(30));
            results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatusList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            var list = db.STATUS
                .Where(x => x.NAME.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.NAME
                        })
                        .Distinct()
                        .ToList();

            if (page == 1)
                results.results.AddRange(list.Take(30));
            else
                results.results.AddRange(list.Skip(page * 30).Take(30));

            if (page * 30 < list.Count)
                results.pagination.more = true;
            else
                results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            var list = db.PRODUCT
                .Where(x => x.DESCRIPTION.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.DESCRIPTION
                        })
                        .Distinct()
                        .ToList();

            if (page == 1)
                results.results.AddRange(list.Take(30));
            else
                results.results.AddRange(list.Skip(page * 30).Take(30));

            if (page * 30 < list.Count)
                results.pagination.more = true;
            else
                results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShiftList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            List<ItemIdText> list = new List<ItemIdText>
            {
                new ItemIdText{id = "1", text = "1st"},
                new ItemIdText{id = "2", text = "2nd"},
                new ItemIdText{id = "3", text = "3rd"},
            };

            results.results.AddRange(list.Take(30));
            results.pagination.more = false;

            return Json(results, JsonRequestBehavior.AllowGet);
        }

    }
}
