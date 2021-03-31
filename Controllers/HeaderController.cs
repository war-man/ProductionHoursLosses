﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductionHoursLosses.Models;
using ProductionHoursLosses.Models.Enum;
using ProductionHoursLosses.Models.ViewModels;
using ProductionHoursLosses.Helper;
using System.Data.Entity.Validation;

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
            {
                model = new HeaderViewModel();
                model.HeaderModel = new HEADER();
                model.DetailsList = new List<DetailExtended>();
                model.DetailLossesList = new List<DETAIL_LOSSES>();
            }
                

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
        public ActionResult Create(HeaderViewModel model, string save, string saveAndSubmit, string addDetail)//([Bind(Include = "ID,DATE,FACTORY_ID,ROOM_ID,AVAIL_HRS,STATUS_ID")] HEADER hEADER) 
        {
            ModelState.Clear();

            var dbPRD_HRS = new PRD_HRS_DBEntities();

            var errorList = new List<string>();

            model.SelectedFactory = dbPRD_HRS.FACTORY.FirstOrDefault(x => x.ID == model.SelectedFactoryID);
            if (model.SelectedFactory == null)
                model.SelectedFactory = new FACTORY();

            model.SelectedRoom = dbPRD_HRS.ROOM.FirstOrDefault(x => x.ID == model.SelectedRoomID);
            if (model.SelectedRoom == null)
                model.SelectedRoom = new ROOM();

            model.SelectedStatus = dbPRD_HRS.STATUS.FirstOrDefault(x => x.ID == model.SelectedStatusID);
            if (model.SelectedStatus == null)
                model.SelectedStatus = new STATUS();

            if (!model.SelectedDate.HasValue && model.HeaderModel.DATE == null)
                errorList.Add("Enter a Date.");
            if (!model.SelectedFactoryID.HasValue)
                errorList.Add("Select a Factory.");
            if (!model.SelectedRoomID.HasValue)
                errorList.Add("Select a Factory Room.");
            if (!model.SelectedAvailHours.HasValue)
                errorList.Add("Select the available hours.");

            if (!errorList.Any() && model.HeaderModel == null)
            {
                model.HeaderModel = new HEADER();
                model.HeaderModel.DATE = model.SelectedDate.Value;
                model.HeaderModel.FACTORY_ID = (int)model.SelectedFactoryID;
                model.HeaderModel.ROOM_ID = (int)model.SelectedRoomID;
                model.HeaderModel.STATUS_ID = (int)model.SelectedStatusID;
                model.HeaderModel.AVAIL_HRS = model.SelectedAvailHours;
            }

            if (!string.IsNullOrEmpty(model.SelectedDetailToBeDeleted))
                DeleteDetail(model);

            if (!string.IsNullOrEmpty(model.SelectedDetailToAddLossAA))
                AddDetailLoss(model, dbPRD_HRS, ref errorList);

            if (!string.IsNullOrEmpty(addDetail))
                AddDetails(model,dbPRD_HRS,ref errorList);

            if (model.DetailsList == null || !model.DetailsList.Any())
                errorList.Add("Enter at least one detail.");


            UpdateDetails(model,dbPRD_HRS);

            if (!errorList.Any() && (!string.IsNullOrWhiteSpace(save) || !string.IsNullOrWhiteSpace(saveAndSubmit)))
            {
                var submitForReview = false;
                if (!string.IsNullOrWhiteSpace(save))
                    submitForReview = false;
                else if (!string.IsNullOrWhiteSpace(saveAndSubmit))
                    submitForReview = true;

                var exception = Save(model, submitForReview);
                if (exception.Result)
                {
                    Session["SaveMessage"] = "true";
                    return RedirectToAction("Index", "Header");
                }
                else
                {
                    Session["SaveMessage"] = "false";
                    errorList.Add(exception.Name);
                }
            }

            Session["SaveMessage"] = "false";
            ViewBag.ErrorList = errorList;
            return View(model);
            //if (ModelState.IsValid)
            //  {
            //      db.HEADER.Add(hEADER);
            //      db.SaveChanges();
            //      return RedirectToAction("Index");
            //  }

            //  ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
            //  ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
            //  ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
            //  return View(hEADER);
        }


        private ExceptionError Save(HeaderViewModel model, bool submitForReview)
        {
            //using (new Impersonator(ProductionHoursLosses.Helper.Helper.GetUserNameWithoutDomain(User.Identity.Name), StaticVariables.DomainName, model.Password))
            //{
                using (var dbPRD_HRS = new PRD_HRS_DBEntities())
                {
                    using (DbContextTransaction transactionNewRec = dbPRD_HRS.Database.BeginTransaction())
                    {
                        if (model.IsUpdate)
                        {

                        }
                        else
                        {
                            var newHeader = new HEADER();
                            //newHeader.DATE = model.SelectedDate.Value;
                            //newHeader.FACTORY_ID = (int)model.SelectedFactoryID;
                            //newHeader.ROOM_ID = (int)model.SelectedRoomID;
                            //newHeader.AVAIL_HRS = model.SelectedAvailHours;

                            newHeader.DATE = model.HeaderModel.DATE;
                            newHeader.FACTORY_ID = (int)model.HeaderModel.FACTORY_ID;
                            newHeader.ROOM_ID = (int)model.HeaderModel.ROOM_ID;
                            newHeader.AVAIL_HRS = model.HeaderModel.AVAIL_HRS;
                            newHeader.STATUS_ID = (int)model.HeaderModel.STATUS_ID;

                            foreach (var detailExtended in model.DetailsList.OrderBy(x => x.AA))
                            {
                                var newDetail = new DETAIL();
                                newDetail.START_TIME = detailExtended.START_TIME;
                                newDetail.END_TIME = detailExtended.END_TIME;
                                newDetail.PRODUCT_ID = detailExtended.PRODUCT_ID;
                                newDetail.BATCH_NO = detailExtended.BATCH_NO;
                                newDetail.WORK_ORDER = detailExtended.WORK_ORDER;
                                newDetail.SHIFT = detailExtended.SHIFT;
                                newDetail.ACTUAL_HRS = detailExtended.ACTUAL_HRS;
                                newDetail.UNIT_WEIGHT = detailExtended.UNIT_WEIGHT;
                                newDetail.SPEED_MACHINE_RPM = detailExtended.SPEED_MACHINE_RPM;
                                newDetail.ACTUAL_QTY = detailExtended.ACTUAL_QTY;
                                newDetail.NUM_PEOPLE = detailExtended.NUM_PEOPLE;
                                newDetail.UNITS = detailExtended.UNITS;

                            foreach(var detaiLoss in detailExtended.DetailLossesList)
                            {
                                var newDetailLoss = new DETAIL_LOSSES();
                                newDetailLoss.LOSSES_ID = detaiLoss.LOSSES_ID;
                                newDetailLoss.DURATION = detaiLoss.DURATION;

                                newDetail.DETAIL_LOSSES.Add(newDetailLoss);
                            }

                                newHeader.DETAIL.Add(newDetail);
                            }

                            dbPRD_HRS.HEADER.Add(newHeader);
                        }

                        try
                        {
                            dbPRD_HRS.SaveChanges();
                            transactionNewRec.Commit();

                            return new ExceptionError { Result = true };
                        }
                        catch (DbEntityValidationException e)
                        {
                            transactionNewRec.Rollback();

                            var exception = new ExceptionError();
                            exception.Name = string.Format("Entity has the following validation errors: {0} {1}\n", e.Message, e.InnerException);

                            foreach (var x in e.EntityValidationErrors)
                            {
                                exception.Name += string.Format("Property name: {0}, Message {1}\n", x.ValidationErrors.FirstOrDefault().PropertyName, x.ValidationErrors.FirstOrDefault().ErrorMessage);
                            }
                            exception.Result = false;

                            return exception;
                        }
                    }
                }
            //}
        }
        private void AddDetails(HeaderViewModel model, PRD_HRS_DBEntities dbPRD_HRS, ref List<string> error)
        {
            model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
            int count = 0;
            if (!model.SelectedStartTime.HasValue)
                error.Add("Enter Start Time.");
            else
                ++count;

            if (!model.SelectedEndTime.HasValue)
                error.Add("Enter End Time.");
            else
                ++count;

            if (!model.SelectedProductId.HasValue)
                error.Add("Select Product.");
            else
            {
                model.SelectedProduct = dbPRD_HRS.PRODUCT.FirstOrDefault(x => x.ID == model.SelectedProductId);
                ++count;
            }
                
            if (model.SelectedProduct == null)
                model.SelectedProduct = new PRODUCT();

            ViewBag.SelectedProduct = model.SelectedProduct;

            if (string.IsNullOrEmpty(model.SelectedBatchNo))
                error.Add("Enter Batch Number.");
            else
                ++count;

            if (string.IsNullOrEmpty(model.SelectedWorkOrder))
                error.Add("Enter Work Order.");
            else
                ++count;

            if (!model.SelectedShift.HasValue)
                error.Add("Select Shift.");
            else
                ++count;

            if (!model.SelectedActualHours.HasValue)
                error.Add("Select Actual Hours.");
            else
                ++count;

            if (!model.SelectedUnitWeight.HasValue)
                error.Add("Enter Unit Weight (mg).");
            else
                ++count;

            if (!model.SelectedSpeedMachineRpm.HasValue)
                error.Add("Enter Speed Machine RPM.");
            else
                ++count;

            if (!model.SelectedActualQuantity.HasValue)
                error.Add("Enter Actual Quantity (Kg).");
            else
                ++count;

            if (!model.SelectedNumPeople.HasValue)
                error.Add("Enter number of people.");
            else
                ++count;

            if (!model.SelectedUnits.HasValue)
                error.Add("Enter units.");
            else
                ++count;

            if(count == 12)
            {
                var detailToAdd = new DetailExtended();
                detailToAdd.START_TIME = model.SelectedStartTime.Value;
                detailToAdd.END_TIME = model.SelectedEndTime.Value;
                detailToAdd.PRODUCT_ID = model.SelectedProductId.Value;
                detailToAdd.BATCH_NO = model.SelectedBatchNo;
                detailToAdd.WORK_ORDER = model.SelectedWorkOrder;
                detailToAdd.SHIFT = Convert.ToByte(model.SelectedShift);
                detailToAdd.ACTUAL_HRS = Convert.ToByte(model.SelectedActualHours);
                detailToAdd.UNIT_WEIGHT = model.SelectedUnitWeight;
                detailToAdd.SPEED_MACHINE_RPM = Convert.ToByte(model.SelectedSpeedMachineRpm);
                detailToAdd.ACTUAL_QTY = model.SelectedActualQuantity;
                detailToAdd.NUM_PEOPLE = Convert.ToByte(model.SelectedNumPeople);
                detailToAdd.UNITS = model.SelectedUnits;

                detailToAdd.PRODUCT = dbPRD_HRS.PRODUCT.FirstOrDefault(x => x.ID == model.SelectedProductId);
                detailToAdd.AA = Guid.NewGuid();

                if (model.DetailsList == null)
                    model.DetailsList = new List<DetailExtended>();

                model.DetailsList.Add(detailToAdd);

                model.SelectedStartTime = new DateTime?();
                model.SelectedEndTime = new DateTime?();
                model.SelectedProductId = new int?();
                model.SelectedBatchNo = string.Empty;
                model.SelectedWorkOrder = string.Empty;
                model.SelectedShift = new int?();
                model.SelectedActualHours = new int?();
                model.SelectedUnitWeight = new decimal?();
                model.SelectedSpeedMachineRpm = new int?();
                model.SelectedActualQuantity = new decimal?();
                model.SelectedNumPeople = new int?();
                model.SelectedUnits = new int?();

                ViewBag.SelectedProduct = null;

            }


        }


        private void AddDetailLoss(HeaderViewModel model, PRD_HRS_DBEntities dbPRD_HRS, ref List<string> error)
        {
            model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;

            if (!model.SelectedLossesMins.HasValue)
                error.Add("Enter Loss Duration (mins).");

            if (!model.SelectedLossesId.HasValue)
                error.Add("Select Loss.");
            else
                model.SelectedLosses = dbPRD_HRS.LOSSES.FirstOrDefault(x => x.ID == model.SelectedLossesId);

            if (model.SelectedLosses == null)
                model.SelectedLosses = new LOSSES();

            ViewBag.SelectedLosses = model.SelectedLosses;

            if(model.SelectedLossesMins.HasValue && model.SelectedLossesId.HasValue)
            {
                foreach (var det in model.DetailsList.Where(x => x.AA.ToString() == model.SelectedDetailToAddLossAA))
                {
                    if (det.DETAIL_LOSSES == null)
                        det.DetailLossesList = new List<DETAIL_LOSSES>();

                    var detailLossToAdd = new DETAIL_LOSSES();

                    detailLossToAdd.LOSSES_ID = (int)model.SelectedLossesId;
                    detailLossToAdd.LOSSES = dbPRD_HRS.LOSSES.FirstOrDefault(x => x.ID == model.SelectedLossesId);
                    detailLossToAdd.DURATION = (int)model.SelectedLossesMins;

                    det.DetailLossesList.Add(detailLossToAdd);

                    model.SelectedDetailToAddLossAA = string.Empty;
                    model.SelectedLossesId = new int?();
                    model.SelectedLossesMins = new int?();
                    model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
                }
            }
        }


        private static void UpdateDetails(HeaderViewModel model, PRD_HRS_DBEntities dbPRD_HRS)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedDetailToUpdateAA) || 
                (!model.SelectedDetailToUpdateStartTime.HasValue || !model.SelectedDetailToUpdateEndTime.HasValue || !model.SelectedDetailToUpdateProductId.HasValue
                || string.IsNullOrEmpty(model.SelectedDetailToUpdateBatchNo) ||  string.IsNullOrEmpty(model.SelectedDetailToUpdateWorkOrder) || !model.SelectedDetailToUpdateShift.HasValue
                || !model.SelectedDetailToUpdateActualHours.HasValue || !model.SelectedDetailToUpdateUnitWeight.HasValue || !model.SelectedDetailToUpdateSpeedMachineRpm.HasValue
                || !model.SelectedDetailToUpdateActualQuantity.HasValue || !model.SelectedDetailToUpdateNumPeople.HasValue || !model.SelectedDetailToUpdateUnits.HasValue))
                return;

            foreach (var det in model.DetailsList.Where(x => x.AA.ToString() == model.SelectedDetailToUpdateAA))
            {
                det.START_TIME = model.SelectedDetailToUpdateStartTime.Value;
                det.END_TIME = model.SelectedDetailToUpdateEndTime.Value;
                det.PRODUCT_ID = (int)model.SelectedDetailToUpdateProductId;
                det.BATCH_NO = model.SelectedDetailToUpdateBatchNo;
                det.WORK_ORDER = model.SelectedDetailToUpdateWorkOrder;
                det.SHIFT = Convert.ToByte(model.SelectedDetailToUpdateShift);
                det.ACTUAL_HRS = Convert.ToByte(model.SelectedDetailToUpdateActualHours);
                det.UNIT_WEIGHT = model.SelectedDetailToUpdateUnitWeight;
                det.SPEED_MACHINE_RPM = Convert.ToByte(model.SelectedDetailToUpdateSpeedMachineRpm);
                det.ACTUAL_QTY = model.SelectedDetailToUpdateActualQuantity;
                det.NUM_PEOPLE = Convert.ToByte(model.SelectedDetailToUpdateNumPeople);
                det.UNITS = model.SelectedDetailToUpdateUnits;


                model.SelectedDetailToUpdateAA = string.Empty;
                model.SelectedDetailToUpdateStartTime = new DateTime?();
                model.SelectedDetailToUpdateEndTime = new DateTime?();
                model.SelectedDetailToUpdateProductId = new int?();
                model.SelectedDetailToUpdateBatchNo = string.Empty;
                model.SelectedDetailToUpdateWorkOrder = string.Empty;
                model.SelectedDetailToUpdateShift = new int?();
                model.SelectedDetailToUpdateActualHours = new int?();
                model.SelectedDetailToUpdateUnitWeight = new decimal?();
                model.SelectedDetailToUpdateSpeedMachineRpm = new int?();
                model.SelectedDetailToUpdateActualQuantity = new decimal?();
                model.SelectedDetailToUpdateNumPeople = new int?();
                model.SelectedDetailToUpdateUnits = new int?();
                model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
            }
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

        public JsonResult GetLossesList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

            List<ItemIdText> list = new List<ItemIdText>();

                list = db.LOSSES
                .Where(x => x.DESCRIPTION.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.DESCRIPTION,
                            itemDescription = y.CODE
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

        private ActionResult DeleteDetail(HeaderViewModel model)
        {
            if (model.DetailsList != null)
                model.DetailsList.RemoveAll(x => x.AA.ToString() == model.SelectedDetailToBeDeleted);

            model.SelectedDetailToBeDeleted = string.Empty;
            model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
            TempData["objectFromAction"] = model;

            return RedirectToAction("Create", "Header", new { model, button = "delete" });
        }

    }
}
