using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class PaymentTermsController : Controller
    {
        // GET: AdminSection/PaymentTerms
        private readonly IPaymentTerms _IPaymentTerms;
        public PaymentTermsController(IPaymentTerms _IPaymentTerms)
        {
            this._IPaymentTerms = _IPaymentTerms;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        public ActionResult Index()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/PaymentTerms/Index.cshtml");
        }

        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : 25-Aug-2018
        /// Created For : Fetching payment terms for jqgrid.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPaymentTerms(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }           
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var paymentTermsList = _IPaymentTerms.GetPaymentTermsList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var paymentTerm in paymentTermsList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(paymentTerm.PTM_Id), true);
                    row.cell = new string[3];
                    row.cell[0] = Convert.ToString(paymentTerm.PTM_Term);
                    row.cell[1] = Convert.ToString(paymentTerm.PTM_GracePeriod);
                    row.cell[2] = Convert.ToString(paymentTerm.PTM_IsActive);
                    rowss.Add(row);
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPaymentTerms()
        {
            try
            {
                ViewBag.AccountSection = true;
                PaymentTermsModel objPaymentTermsModel = new PaymentTermsModel();
                return PartialView("AddPaymentTerms", objPaymentTermsModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SavePaymentTerms(PaymentTermsModel objPaymentTermsModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                var savedStatus = _IPaymentTerms.SavePaymentTerms(objPaymentTermsModel);
                if (savedStatus.Result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Nov-2018
        /// Created For : To active Payment term by Payment Id
        /// </summary>
        /// <param name="PaymentModeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivePaymentTerm(string PaymentTermId, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                long _PaymentModeId = 0;
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
                long UserId = ObjLoginModel.UserId;
                if (!string.IsNullOrEmpty(PaymentTermId))
                {
                    PaymentTermId = Cryptography.GetDecryptedData(PaymentTermId, true);
                    long.TryParse(PaymentTermId, out _PaymentModeId);
                }
                result = _IPaymentTerms.ActivePaymentTermById(_PaymentModeId, UserId, IsActive);
                if (result == true)
                {
                    string Message = "";
                    if (IsActive == "Y")
                    {
                        Message = "Activeted.";
                        ViewBag.Message = Message;
                    }
                    else
                    {
                        Message = "Deactiveted.";
                        ViewBag.Message = Message;
                    }
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating payment term.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
    }
}