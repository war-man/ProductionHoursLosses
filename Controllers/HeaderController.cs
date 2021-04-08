using System;
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
using ProductionHoursLosses.Module;
using ProductionHoursLosses.Helper;
using ProductionHoursLosses.SharedHelp;
using System.Data.Entity.Validation;
using PagedList;


namespace ProductionHoursLosses.Controllers
{
    public class HeaderController : Controller
    {
        private PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

        private HeaderListViewModel InitializeListViewModel(HeaderListViewModel model)
        {
            //if (model.SelectedStartDate == null)
            //    model.SelectedStartDate = new DateTime();

            //if (model.SelectedEndDate == null)
            //    model.SelectedEndDate = new DateTime();

            if (model.SelectedHeaderStatusList == null)
                model.SelectedHeaderStatusList = new List<ItemIdName>();

            if (!string.IsNullOrWhiteSpace(model.SelectedHeaderStatusIds))
            {
                foreach (var selectedHeaderStatusId in model.SelectedHeaderStatusIds.Split(',').ToList())
                {
                    var itemName = new ItemIdName();

                    var itemToAdd = db.STATUS.FirstOrDefault(x => x.ID.ToString() == selectedHeaderStatusId);
                    if (itemToAdd == null)
                        continue;

                    itemName.Id = selectedHeaderStatusId;
                    itemName.Name = itemToAdd.NAME;

                    model.SelectedHeaderStatusList.Add(itemName);
                }
            }

            if (model.SelectedHeaderFactoryList == null)
                model.SelectedHeaderFactoryList = new List<ItemIdName>();

            if (!string.IsNullOrWhiteSpace(model.SelectedHeaderFactoryIds))
            {
                foreach (var selectedHeaderFactoryId in model.SelectedHeaderFactoryIds.Split(',').ToList())
                {
                    var itemName = new ItemIdName();

                    var itemToAdd = db.FACTORY.FirstOrDefault(x => x.ID.ToString() == selectedHeaderFactoryId);
                    if (itemToAdd == null)
                        continue;

                    itemName.Id = selectedHeaderFactoryId;
                    itemName.Name = itemToAdd.NAME;

                    model.SelectedHeaderFactoryList.Add(itemName);
                }
            }

            if (model.SelectedHeaderRoomList == null)
                model.SelectedHeaderRoomList = new List<ItemIdName>();

            if (!string.IsNullOrWhiteSpace(model.SelectedHeaderRoomIds))
            {
                foreach (var selectedHeaderRoomId in model.SelectedHeaderRoomIds.Split(',').ToList())
                {
                    var itemName = new ItemIdName();

                    var itemToAdd = db.ROOM.FirstOrDefault(x => x.ID.ToString() == selectedHeaderRoomId);
                    if (itemToAdd == null)
                        continue;

                    itemName.Id = selectedHeaderRoomId;
                    itemName.Name = itemToAdd.NAME;

                    model.SelectedHeaderRoomList.Add(itemName);
                }
            }

            return model;
        }

        // GET: Header
        public ActionResult Index(HeaderListViewModel model)
        {
            if (model == null)
                model = new HeaderListViewModel();

            model = InitializeListViewModel(model);

            model.PageNumber = model.PageNumber ?? 1;
            model.PageSize = model.PageSize ?? 10;

            model.HeaderList = SearchInList(model);

            model.IsNextPageRequest = false;

            if (!string.IsNullOrWhiteSpace((string)TempData["errorMessage"]))
                ViewBag.PasswordMessage = "Wrong user authentication password.";

            return View("Index", model);


            //var hEADER = db.HEADER.Include(h => h.FACTORY).Include(h => h.ROOM).Include(h => h.STATUS);
            //return View(hEADER.ToList());
        }

        private IPagedList<HEADER> SearchInList(HeaderListViewModel model)
        {
            var areSearchCriteriaEmpty = !model.SelectedStartDate.HasValue
                && !model.SelectedEndDate.HasValue
                && !model.SelectedHeaderStatusList.Any()
                && !model.SelectedHeaderFactoryList.Any()
                && !model.SelectedHeaderRoomList.Any();

            if (areSearchCriteriaEmpty)
            {
                return db.HEADER
                        .OrderBy(x => x.ID)
                        //.Select(x => new ProductionHelperModel { ProductionMaster = x })
                        .AsQueryable()
                        .ToPagedList(model.PageNumber.Value, model.PageSize.Value);
            }
            else
            {
                var list = db.HEADER.OrderBy(x => x.ID);

                if (model.SelectedStartDate.HasValue && model.SelectedEndDate.HasValue)
                {
                    if (model.SelectedStartDate.Value <= model.SelectedEndDate.Value)
                        list = list.Where(x => x.DATE >= model.SelectedStartDate.Value && x.DATE <= model.SelectedEndDate.Value).OrderBy(x => x.ID);
                }


                if (model.SelectedHeaderStatusList.Any())
                {
                    var listOfHeaderStatus = new HashSet<string>(model.SelectedHeaderStatusList.Select(x => x.Id));
                    list = list.Where(x => listOfHeaderStatus.Contains(x.STATUS_ID.ToString())).OrderBy(X => X.ID);
                }

                if (model.SelectedHeaderFactoryList.Any())
                {
                    var listOfHeaderFactory = new HashSet<string>(model.SelectedHeaderFactoryList.Select(x => x.Id));
                    list = list.Where(x => listOfHeaderFactory.Contains(x.FACTORY_ID.ToString())).OrderBy(X => X.ID);
                }

                if (model.SelectedHeaderRoomList.Any())
                {
                    var listOfHeaderRoom = new HashSet<string>(model.SelectedHeaderRoomList.Select(x => x.Id));
                    list = list.Where(x => listOfHeaderRoom.Contains(x.ROOM_ID.ToString())).OrderBy(X => X.ID);
                }

                return list.AsQueryable()
                         .ToPagedList(model.PageNumber.Value, model.PageSize.Value);
            }
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
        public ActionResult Create(int? headerId)
        {
            HeaderViewModel model = new HeaderViewModel();

            if (model == null)
            {
                model.HeaderModel = new HEADER();
                model.DetailsList = new List<DetailExtended>();
                //model.DetailLossesList = new List<DETAIL_LOSSES>();
            }

            ModelState.Clear();

            if(headerId.HasValue)
            {
                model = RetrieveHeader(headerId.Value);
                model.IsUpdate = true;
                model.HeaderModel.ID = headerId.Value;

                return View(model);
            }

            model = InitializeViewModel(model);
            model.DetailsList = new List<DetailExtended>();
            //ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME");
            //ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME");
            //ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME");
            return View("Create", model);
        }

        // POST: Header/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HeaderViewModel model, string save, string addDetail)//([Bind(Include = "ID,DATE,FACTORY_ID,ROOM_ID,AVAIL_HRS,STATUS_ID")] HEADER hEADER) 
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

            if (!errorList.Any())
            {
                if(model.HeaderModel == null)
                    model.HeaderModel = new HEADER();
                if(model.SelectedDate.HasValue)
                    model.HeaderModel.DATE = model.SelectedDate.Value;
                if(model.SelectedFactoryID.HasValue)
                    model.HeaderModel.FACTORY_ID = (int)model.SelectedFactoryID;
                if (model.SelectedRoomID.HasValue)
                    model.HeaderModel.ROOM_ID = (int)model.SelectedRoomID;
                if (model.SelectedStatusID.HasValue)
                    model.HeaderModel.STATUS_ID = (int)model.SelectedStatusID;
                if (model.SelectedAvailHours.HasValue)
                    model.HeaderModel.AVAIL_HRS = model.SelectedAvailHours;
            }

            if (!string.IsNullOrEmpty(model.SelectedDetailToBeDeleted) && string.IsNullOrEmpty(model.SelectedDetailLossToBeDeleted))
                DeleteDetail(model);

            if (!string.IsNullOrEmpty(model.SelectedDetailLossToBeDeleted) && !string.IsNullOrEmpty(model.SelectedDetailToBeDeleted))
                DeleteDetailLoss(model);

            if (!string.IsNullOrEmpty(model.SelectedDetailToAddLossAA))
                AddDetailLoss(model, dbPRD_HRS, ref errorList);

            if (!string.IsNullOrEmpty(model.SelectedDetailLossToUpdateAA) && !string.IsNullOrEmpty(model.SelectedDetailToUpdateAA))
                UpdateDetailLoss(model, dbPRD_HRS);

            if (!string.IsNullOrEmpty(addDetail))
                AddDetails(model,dbPRD_HRS,ref errorList);

            if (model.DetailsList == null || !model.DetailsList.Any())
                errorList.Add("Enter at least one detail.");


            UpdateDetails(model,dbPRD_HRS);

            //if (string.IsNullOrWhiteSpace(model.Password))
            //{
            //    errorList.Add("Step 3: Please enter your password.");
            //}
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(save) )
            //    {
            //        if (!CheckPassword(model.Password))
            //            errorList.Add("Step 3: Wrong user authentication password.");
            //    }
            //}

            if (!errorList.Any() && !string.IsNullOrWhiteSpace(save))
            {
                var exception = Save(model);
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

        public static HeaderViewModel RetrieveHeader(int headerId)
        {
            PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();
            var itemToEdit = db.HEADER.FirstOrDefault(x => x.ID == headerId);
            if (itemToEdit == null)
                return null;

            var model = new HeaderViewModel();
            model.IsUpdate = true;

            model.HeaderModel = itemToEdit;
            model.SelectedDate = itemToEdit.DATE;
            model.SelectedAvailHours = itemToEdit.AVAIL_HRS;
            model.SelectedFactoryID = itemToEdit.FACTORY_ID;
            model.SelectedRoomID = itemToEdit.ROOM_ID;
            model.SelectedStatusID = itemToEdit.STATUS_ID;

            model = InitializeViewModel(model);

            model.DetailsList = new List<DetailExtended>();

            if (itemToEdit.DETAIL != null)
            {   
                foreach (var det in itemToEdit.DETAIL.OrderBy(x => x.ID))
                {
                    var detToAdd = new DetailExtended();
                    detToAdd.AA = Guid.NewGuid();
                    detToAdd.ID = det.ID;
                    detToAdd.START_TIME = det.START_TIME;
                    detToAdd.END_TIME = det.END_TIME;
                    detToAdd.PRODUCT_ID = det.PRODUCT_ID;
                    detToAdd.PRODUCT = new PRODUCT();
                    detToAdd.PRODUCT = db.PRODUCT.FirstOrDefault(x => x.ID == det.PRODUCT_ID);
                    detToAdd.BATCH_NO = det.BATCH_NO;
                    detToAdd.WORK_ORDER = det.WORK_ORDER;
                    detToAdd.SHIFT = det.SHIFT;
                    detToAdd.ACTUAL_HRS = det.ACTUAL_HRS;
                    detToAdd.UNIT_WEIGHT = det.UNIT_WEIGHT;
                    detToAdd.SPEED_MACHINE_RPM = det.SPEED_MACHINE_RPM;
                    detToAdd.ACTUAL_QTY = det.ACTUAL_QTY;
                    detToAdd.NUM_PEOPLE = det.NUM_PEOPLE;
                    detToAdd.UNITS = det.UNITS;

                    if (det.DETAIL_LOSSES != null)
                    {
                        detToAdd.DetailLossesList = new List<DetailLossesExtended>();
                        foreach (var loss in det.DETAIL_LOSSES.OrderBy(x => x.ID))
                        {
                            var lossToAdd = new DetailLossesExtended();
                            lossToAdd.AA = Guid.NewGuid();
                            lossToAdd.ID = loss.ID;
                            lossToAdd.LOSSES_ID = loss.LOSSES_ID;
                            lossToAdd.DURATION = loss.DURATION;
                            lossToAdd.LOSSES = new LOSSES();
                            lossToAdd.LOSSES = db.LOSSES.FirstOrDefault(x => x.ID == loss.LOSSES_ID);

                            detToAdd.DetailLossesList.Add(lossToAdd);
                        }
                        detToAdd.DetailLossesList.OrderBy(x => x.ID);
                    }

                    model.DetailsList.Add(detToAdd);
                }
                model.DetailsList.OrderBy(x => x.ID);
            }

            return model;
        }

        public static HeaderViewModel InitializeViewModel(HeaderViewModel model)
        {
            PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();

            if (model == null)
                model = new HeaderViewModel();

            if(model.SelectedDate.HasValue || model.SelectedStatusID.HasValue || model.SelectedFactoryID.HasValue || model.SelectedRoomID.HasValue || model.SelectedAvailHours.HasValue)
            {
                if (model.HeaderModel == null)
                {
                    model.HeaderModel = new HEADER();
                }
            }


            if (model.DetailsList == null)
                model.DetailsList = new List<DetailExtended>();

            if (model.SelectedStatusID.HasValue)
            {
                var status = db.STATUS.FirstOrDefault(x => x.ID == model.SelectedStatusID);
                if (status != null)
                {
                    model.SelectedStatus = new STATUS();
                    model.SelectedStatus = status;
                    model.HeaderModel.STATUS = new STATUS();
                    model.HeaderModel.STATUS = status;
                    model.HeaderModel.STATUS_ID = model.SelectedStatusID.Value;
                }
            }

            if (model.SelectedFactoryID.HasValue)
            {
                var factory = db.FACTORY.FirstOrDefault(x => x.ID == model.SelectedFactoryID);
                if (factory != null)
                {
                    model.SelectedFactory = new FACTORY();
                    model.SelectedFactory = factory;
                    model.HeaderModel.FACTORY = new FACTORY();
                    model.HeaderModel.FACTORY = factory;
                    model.HeaderModel.FACTORY_ID = model.SelectedFactoryID.Value;
                }
            }

            if (model.SelectedRoomID.HasValue)
            {
                var room = db.ROOM.FirstOrDefault(x => x.ID == model.SelectedRoomID);
                if (room != null)
                {
                    model.SelectedRoom = new ROOM();
                    model.SelectedRoom = room;
                    model.HeaderModel.ROOM = new ROOM();
                    model.HeaderModel.ROOM = room;
                    model.HeaderModel.ROOM_ID = model.SelectedRoomID.Value;
                }
            }

            if (model.SelectedDate.HasValue)
            {
                model.HeaderModel.DATE = new DateTime();
                model.HeaderModel.DATE = model.SelectedDate.Value;
            }

            if (model.SelectedAvailHours.HasValue)
            {
                model.HeaderModel.AVAIL_HRS = new int();
                model.HeaderModel.AVAIL_HRS = model.SelectedAvailHours.Value;
            }

            return model;
        }

        private bool CheckPassword(string password)
        {
            return SecurityModule.VerifyUser(Request.LogonUserIdentity.Name, password);
        }


        private ExceptionError Save(HeaderViewModel model)
        {
            //using (new Impersonator(ProductionHoursLosses.Helper.Helper.GetUserNameWithoutDomain(User.Identity.Name), StaticVariables.DomainName, model.Password))
            //{
                using (var dbPRD_HRS = new PRD_HRS_DBEntities())
                {
                    using (DbContextTransaction transactionNewRec = dbPRD_HRS.Database.BeginTransaction())
                    {
                        if (model.IsUpdate)
                        {
                            var dbHdr = dbPRD_HRS.HEADER.FirstOrDefault(x => x.ID == model.HeaderModel.ID);
                            if (dbHdr == null)
                            {
                                var exception = new ExceptionError();
                                exception.Name = string.Format("Edit Header: During save cannot find the header id {0}", model.HeaderModel.ID);
                                return exception;
                            }

                            //Option 1: delete the existing record details and losses and modify the existing one

                            if (dbHdr.DETAIL != null && dbHdr.DETAIL.Any())
                            {
                                foreach (var det in dbHdr.DETAIL.ToList())
                                {
                                    if (det.DETAIL_LOSSES != null && det.DETAIL_LOSSES.Any())
                                    {
                                        foreach (var loss in det.DETAIL_LOSSES.ToList())
                                        {
                                            dbPRD_HRS.DETAIL_LOSSES.Remove(loss);
                                        }
                                    }
                                    dbPRD_HRS.DETAIL.Remove(det);
                                }
                            }

                            dbHdr.DATE = model.HeaderModel.DATE;
                            dbHdr.FACTORY_ID = (int)model.HeaderModel.FACTORY_ID;
                            dbHdr.ROOM_ID = (int)model.HeaderModel.ROOM_ID;
                            dbHdr.AVAIL_HRS = model.HeaderModel.AVAIL_HRS;
                            dbHdr.STATUS_ID = (int)model.HeaderModel.STATUS_ID;

                            if (model.DetailsList != null && model.DetailsList.Any())
                            {
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

                                    if (detailExtended.DetailLossesList != null && detailExtended.DetailLossesList.Any())
                                    {
                                        foreach (var detaiLoss in detailExtended.DetailLossesList.OrderBy(y => y.AA))
                                        {
                                            var newDetailLoss = new DETAIL_LOSSES();
                                            newDetailLoss.LOSSES_ID = detaiLoss.LOSSES_ID;
                                            newDetailLoss.DURATION = detaiLoss.DURATION;

                                            newDetail.DETAIL_LOSSES.Add(newDetailLoss);
                                        }
                                    }
                                    dbHdr.DETAIL.Add(newDetail);
                                }

                            }

                        dbPRD_HRS.Entry(dbHdr).State = EntityState.Modified;


                        //Option 2: modify the existing one


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

                            if(model.DetailsList != null && model.DetailsList.Any())
                            {
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

                                    if (detailExtended.DetailLossesList != null && detailExtended.DetailLossesList.Any())
                                    {
                                        foreach (var detaiLoss in detailExtended.DetailLossesList.OrderBy(y => y.AA))
                                        {
                                            var newDetailLoss = new DETAIL_LOSSES();
                                            newDetailLoss.LOSSES_ID = detaiLoss.LOSSES_ID;
                                            newDetailLoss.DURATION = detaiLoss.DURATION;

                                            newDetail.DETAIL_LOSSES.Add(newDetailLoss);
                                        }
                                    }
                                    newHeader.DETAIL.Add(newDetail);
                                }

                            }

                            dbPRD_HRS.HEADER.Add(newHeader);
                        }

                        try
                        {
                            dbPRD_HRS.SaveChanges();
                            transactionNewRec.Commit();

                            if(model.IsUpdate)
                            {
                                SharedHelp.CommonFunctions.CreateLog("EDIT", "HEADER", model.HeaderModel.ID, null, null, User.Identity.Name);
                            }
                            else
                            {
                                SharedHelp.CommonFunctions.CreateLog("CREATE", "HEADER", dbPRD_HRS.HEADER.Max(x => x.ID), null, null, User.Identity.Name);
                            }

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
                    if (det.DetailLossesList == null)
                        det.DetailLossesList = new List<DetailLossesExtended>();

                    var detailLossToAdd = new DetailLossesExtended();

                    detailLossToAdd.AA = Guid.NewGuid();
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

        private static void UpdateDetailLoss(HeaderViewModel model, PRD_HRS_DBEntities dbPRD_HRS)
        {
            if (string.IsNullOrWhiteSpace(model.SelectedDetailToUpdateAA) || string.IsNullOrWhiteSpace(model.SelectedDetailLossToUpdateAA) ||
                (!model.SelectedDetailLossToUpdateDuration.HasValue || !model.SelectedDetailLossToUpdateLossID.HasValue ))
                return;

            foreach (var det in model.DetailsList.Where(x => x.AA.ToString() == model.SelectedDetailToUpdateAA))
            {
                foreach(var detlos in det.DetailLossesList.Where(y => y.AA.ToString()==model.SelectedDetailLossToUpdateAA))
                {
                    detlos.LOSSES_ID = (int)model.SelectedDetailLossToUpdateLossID;
                    detlos.DURATION = (int)model.SelectedDetailLossToUpdateDuration;
                }

                model.SelectedDetailToUpdateAA = string.Empty;
                model.SelectedDetailLossToUpdateAA = string.Empty;
                model.SelectedDetailLossToUpdateDuration = new int?();
                model.SelectedDetailLossToUpdateLossID = new int?();

                model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
            }
        }

        // GET: Header/Edit/5
        public ActionResult Edit(int headerId)
        {
            if (headerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new HeaderViewModel();
            model = RetrieveHeader((int)headerId);

            if (model == null)
                return RedirectToAction("GeneralError", "Error", new { error = "Header Edit: Header id does not exist" });

            return RedirectToAction("Create", "Header", new { headerId = headerId });

            //HEADER hEADER = db.HEADER.Find(id);
            //if (hEADER == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
            //ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
            //ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
            //return View(hEADER);
        }

        // POST: Header/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,DATE,FACTORY_ID,ROOM_ID,AVAIL_HRS,STATUS_ID")] HEADER hEADER)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(hEADER).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.FACTORY_ID = new SelectList(db.FACTORY, "ID", "NAME", hEADER.FACTORY_ID);
        //    ViewBag.ROOM_ID = new SelectList(db.ROOM, "ID", "NAME", hEADER.ROOM_ID);
        //    ViewBag.STATUS_ID = new SelectList(db.STATUS, "ID", "NAME", hEADER.STATUS_ID);
        //    return View(hEADER);
        //}

        // GET: Header/Delete/5
        public ActionResult Delete(int? headerId)
        {
            if (headerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new HeaderViewModel();
            model = RetrieveHeader((int)headerId);

            if (model == null)
                return RedirectToAction("GeneralError", "Error", new { error = "Header Edit: Header id does not exist" });

            return View("Delete", model);

            //HEADER hEADER = db.HEADER.Find(id);
            //if (hEADER == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(hEADER);
        }

        // POST: Header/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int headerId)
        {
            var errorList = new List<string>();
            var exception = new ExceptionError();
            //using (new Impersonator(ProductionHoursLosses.Helper.Helper.GetUserNameWithoutDomain(User.Identity.Name), StaticVariables.DomainName, model.Password))
            //{
            using (var dbPRD_HRS = new PRD_HRS_DBEntities())
            {
                using (DbContextTransaction transactionNewRec = dbPRD_HRS.Database.BeginTransaction())
                {
                    
                        var dbHdr = dbPRD_HRS.HEADER.FirstOrDefault(x => x.ID == headerId);
                        if (dbHdr == null)
                        {
                            exception.Result = false;
                            exception.Name = string.Format("Edit Header: During save cannot find the header id {0}", headerId);
                        }
                        else 
                        {
                            if (dbHdr.DETAIL != null && dbHdr.DETAIL.Any())
                            {
                                foreach (var det in dbHdr.DETAIL.ToList())
                                {
                                    if (det.DETAIL_LOSSES != null && det.DETAIL_LOSSES.Any())
                                    {
                                        foreach (var loss in det.DETAIL_LOSSES.ToList())
                                        {
                                            dbPRD_HRS.DETAIL_LOSSES.Remove(loss);
                                        }
                                    }
                                    dbPRD_HRS.DETAIL.Remove(det);
                                }
                            }

                            dbPRD_HRS.HEADER.Remove(dbHdr);

                            try
                            {
                                dbPRD_HRS.SaveChanges();
                                transactionNewRec.Commit();

                                SharedHelp.CommonFunctions.CreateLog("DELETE", "HEADER", headerId, null, null, User.Identity.Name);

                                exception.Result = true;
                            }
                            catch (DbEntityValidationException e)
                            {
                                transactionNewRec.Rollback();

                                exception.Name = string.Format("Entity has the following validation errors: {0} {1}\n", e.Message, e.InnerException);

                                foreach (var x in e.EntityValidationErrors)
                                {
                                    exception.Name += string.Format("Property name: {0}, Message {1}\n", x.ValidationErrors.FirstOrDefault().PropertyName, x.ValidationErrors.FirstOrDefault().ErrorMessage);
                                }
                                exception.Result = false;

                            }
                    }
                        
                }
            }
            //}

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

            ViewBag.ErrorList = errorList;
            return RedirectToAction("Index", "Header");

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


        public JsonResult GetAllRoomList(string q, int page)
        {
            var results = new ResultSelect2<ItemIdText>();
            results.results = new List<ItemIdText>();
            results.pagination = new Pagination();

            if (!string.IsNullOrWhiteSpace(q))
                q = q.ToLower();
            else
                q = string.Empty;

     

            List<ItemIdText> list = new List<ItemIdText>();

                list = db.ROOM
                .Where(x => x.NAME.ToLower().StartsWith(q))
                    .Select(y =>
                        new ItemIdText
                        {
                            id = y.ID.ToString(),
                            text = y.NAME,
                            itemDescription = y.FACTORY.NAME
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

        private ActionResult DeleteDetailLoss(HeaderViewModel model)
        {
            if (model.DetailsList != null)
            {
                foreach(var det in model.DetailsList.Where(x => x.AA.ToString() == model.SelectedDetailToBeDeleted))
                {
                    if(det.DetailLossesList != null && det.DetailLossesList.Any())
                        det.DetailLossesList.RemoveAll(y => y.AA.ToString() == model.SelectedDetailLossToBeDeleted);
                }
     
            }

            model.SelectedDetailToBeDeleted = string.Empty;
            model.SelectedDetailLossToBeDeleted = string.Empty;
            model.SelectedStep = (int)SmartWizardStepEnum.AddDetails;
            TempData["objectFromAction"] = model;

            return RedirectToAction("Create", "Header", new { model, button = "delete" });
        }

    }
}
