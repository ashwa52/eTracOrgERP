using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.GlobalSettings;
using WorkOrderEMS.Service;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class GlobalSettingsController : BaseController
    {
        // GET: NewAdmin
        private readonly IDepartment _IDepartment;
        private readonly IGlobalAdmin _GlobalAdminManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IQRCSetup _IQRCSetup;
        private readonly IePeopleManager _IePeopleManager;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public GlobalSettingsController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
        }

        ARService ARS = new ARService();

        #region AR Rules

        public ActionResult RuleList()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }

                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_RuleList.cshtml");
                //return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetListRuleJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string RuleStastus = null)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }

            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "Name" : sidx;
            txtSearch = string.IsNullOrEmpty(_search) ? "" : _search;            
            List<ARRules> RuleList = new List<ARRules>();
            try
            {
                RuleList = ARS.GetAllARRules(rows, page, TotalRecords, sord, txtSearch, sidx, null, 0);
                if(!string.IsNullOrEmpty(RuleStastus) && RuleStastus != "All")
                {
                    bool RStastus = RuleStastus.Trim() ==  "1" ? true : false;
                    RuleList = RuleList.Where(c => c.Status == RStastus).ToList();
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(RuleList, JsonRequestBehavior.AllowGet);
        }

      //  [HttpGet]
        public ActionResult NewARRule(int id)
        {
            ViewBag.EntryType = "A";
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                ARRules ARR = new ARRules();

                if (id > 0)
                {
                    ViewBag.EntryType = "U";
                    ARR = ARS.GetAllARRules(null, null, null, null, "", null, "", id).ToList().FirstOrDefault();
                }


                return PartialView("~/Views/GlobalSettings/_AddNewARRule.cshtml", ARR);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }
      //  [HttpPost]
        public ActionResult NewARRuleSubmit(ARRules ARRs, string EntryType)
        {
            try
            {
                if (EntryType == "A")
                {
                    ARRs.EntryBy = HttpContext.User.Identity.Name;
                    ARRs.EntryDate = System.DateTime.Now;
                }
                else
                {
                    ARRs.UpdateBy = HttpContext.User.Identity.Name;
                    ARRs.UpdateDate = System.DateTime.Now;
                }

                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                XmlDocument xmlDoc1 = new XmlDocument();
                XmlSerializer xmlSerializer1 = new XmlSerializer(ARRs.GetType());

                using (MemoryStream xmlStream1 = new MemoryStream())
                {
                    xmlSerializer1.Serialize(xmlStream1, ARRs);
                    xmlStream1.Position = 0;
                    xmlDoc1.Load(xmlStream1);
                }
                DataTable DT = ARS.InsertUpdateARRules(xmlDoc1.InnerXml, EntryType);
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_RuleList.cshtml");
                //return null;
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        #endregion


        #region Bank Account

        public ActionResult BankAccountList()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }

                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_BankAccountList.cshtml");
                //return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetListBankAccountJSGrid(string _search, long? UserId, long? locationId, int BankId = 0, int BankUserId = 0, int Id=0)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }

            
            List<BankAccount> ItemList = new List<BankAccount>();
            try
            {

                ItemList = ARS.GetAllBankAccount(BankUserId,Id,BankId);

                

            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewBankAccount(int id)
        {
            ViewBag.EntryType = "A";
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                BankAccount BAcc = new BankAccount();
                ViewBag.GlobalCodes = ARS.GetGlobalCodes(0, "BANK");
                if (id > 0)
                {
                    ViewBag.EntryType = "U";
                    BAcc = ARS.GetAllBankAccount(0,0, id).ToList().FirstOrDefault();
                }


                return PartialView("~/Views/GlobalSettings/_AddBankAccount.cshtml", BAcc);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }
        //  [HttpPost]
        public ActionResult NewBankAccountSubmit(BankAccount BAcc, string EntryType)
        {
            try
            {
                if (EntryType == "A")
                {
                    BAcc.EntryBy = HttpContext.User.Identity.Name;
                    BAcc.EntryDate = System.DateTime.Now;
                }
                else
                {
                    BAcc.UpdateBy = HttpContext.User.Identity.Name;
                    BAcc.UpdateDate = System.DateTime.Now;
                }

                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                XmlDocument xmlDoc1 = new XmlDocument();
                XmlSerializer xmlSerializer1 = new XmlSerializer(BAcc.GetType());

                using (MemoryStream xmlStream1 = new MemoryStream())
                {
                    xmlSerializer1.Serialize(xmlStream1, BAcc);
                    xmlStream1.Position = 0;
                    xmlDoc1.Load(xmlStream1);
                }
                DataTable DT = ARS.BankAccountMapping_InsertUpdate(xmlDoc1.InnerXml, EntryType);
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_BankAccountList.cshtml");
                //return null;
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        #endregion

        #region Customer Invoice 

        public ActionResult InvoicetList()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }

                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_InvoiceList.cshtml");
                //return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetListInvoiceJSGrid(DateTime? FromDate, DateTime? ToDate, string _search, long? UserId, long? locationId, string InvoiceNo = "", int InvoiceType = 0, int InvoiceCriteria = 0,
             int Id = 0)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            List<Invoices> ItemList = new List<Invoices>();
            try
            {

                ItemList = ARS.GetAllInvoice(FromDate, ToDate, InvoiceNo, InvoiceType, InvoiceCriteria, Id);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetInvoice(int id)
        {
            var getData = new Invoices();
            try
            {
                //eTracLoginModel ObjLoginModel = null;

                //long UserId = 0;
                //long LocationId = 0;
            
                //if (Session != null && Session["eTrac"] != null)
                //{
                //    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //    if (LocationId == null)
                //    {
                //        LocationId = ObjLoginModel.LocationID;
                //    }
                //    UserId = ObjLoginModel.UserId;
                //}
                
                if (id > 0)
                {
                    getData = ARS.GetAllInvoice(null, null, "0", 0, 0, id).ToList().FirstOrDefault();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InvoicesApproveReject(int id, int AppRejType)
        {            
            try
            {                 
                if (id > 0)
                {
                    DataTable DT = ARS.InvoicesApproveReject(id, AppRejType,HttpContext.User.Identity.Name);
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewInvoice(int id)
        {
            ViewBag.EntryType = "A";
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                Invoices Inv = new Invoices();
                ViewBag.InvoiceType = ARS.GetGlobalCodes(0, "InvoiceType");
                ViewBag.PuposeOfBilling = ARS.GetGlobalCodes(0, "PuposeOfBilling");
                ViewBag.InvoiceCriteria = ARS.GetGlobalCodes(0, "InvoiceCriteria");
                ViewBag.Customer = ARS.Get_CustomerList(0);
                if (id > 0)
                {
                    ViewBag.EntryType = "U";
                    Inv = ARS.GetAllInvoice(null, null, "0", 0, 0, id).ToList().FirstOrDefault();
                }


                return PartialView("~/Views/GlobalSettings/_NewInvoice.cshtml", Inv);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }
        //  [HttpPost]
        public ActionResult NewInvoiceSubmit(Invoices Inv, string EntryType)
        {
            try
            {
                if (EntryType == "A")
                {
                    Inv.EntryBy = HttpContext.User.Identity.Name;
                    Inv.EntryDate = System.DateTime.Now;
                }
                else
                {
                    Inv.UpdateBy = HttpContext.User.Identity.Name;
                    Inv.UpdateDate = System.DateTime.Now;
                }

                if (Inv.Amount == null)
                    Inv.Amount = 0;

                Inv.InvoiceCriteria = 2;

                if (Inv.PendingAmount == null)
                    Inv.PendingAmount = 0;
                if (Inv.InvoiceDate == null)
                    Inv.InvoiceDate = System.DateTime.Now;
                if (Inv.DueDate == null)
                    Inv.DueDate = System.DateTime.Now;

                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];

                XmlDocument xmlDoc1 = new XmlDocument();
                XmlSerializer xmlSerializer1 = new XmlSerializer(Inv.GetType());

                using (MemoryStream xmlStream1 = new MemoryStream())
                {
                    xmlSerializer1.Serialize(xmlStream1, Inv);
                    xmlStream1.Position = 0;
                    xmlDoc1.Load(xmlStream1);
                }
                DataTable DT = ARS.BankAccountMapping_InsertUpdate(xmlDoc1.InnerXml, EntryType);
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = false;
                return View("~/Views/GlobalSettings/_InvoiceList.cshtml");
                //return null;
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }

        }

        #endregion

    }
}