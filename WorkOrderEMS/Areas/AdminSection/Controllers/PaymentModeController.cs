using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces.Accounts;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class PaymentModeController : Controller
    {
        // GET: AdminSection/PaymentTerms
        private readonly IPaymentMode _IPaymentMode;
        public PaymentModeController(IPaymentMode _IPaymentMode)
        {
            this._IPaymentMode = _IPaymentMode;
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
            return View("~/Areas/AdminSection/Views/PaymentMode/Index.cshtml");
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
        public JsonResult GetPaymentMode(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var paymentModeList = _IPaymentMode.GetPaymentModeList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var paymentTerm in paymentModeList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(paymentTerm.PMD_Id), true);
                    row.cell = new string[2];
                    row.cell[0] = Convert.ToString(paymentTerm.PMD_PaymentMode);
                    row.cell[1] = Convert.ToString(paymentTerm.PMD_IsActive);
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

        public ActionResult AddPaymentMode()
        {
            try
            {
                ViewBag.AccountSection = true;
                PaymentModeModel objPaymentModeModel = new PaymentModeModel();
                return PartialView("AddPaymentMode", objPaymentModeModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SavePaymentMode(PaymentModeModel objPaymentModesModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                var savedStatus = _IPaymentMode.SavePaymentMode(objPaymentModesModel);
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
        /// Created By : Ashwajit bansod
        /// Created Date : 19-Nov-2018
        /// Created For : To Activate payment mode by payment id.
        /// </summary>
        /// <param name="PaymentModeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivePaymentMode(string PaymentModeId, string IsActive)
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
                if (!string.IsNullOrEmpty(PaymentModeId))
                {
                    PaymentModeId = Cryptography.GetDecryptedData(PaymentModeId, true);
                    long.TryParse(PaymentModeId, out _PaymentModeId);
                }
                result = _IPaymentMode.ActivePaymentModeById(_PaymentModeId, UserId, IsActive);
                if (result == true)
                {
                    string Message = "";
                    if (IsActive == "Y")
                    {
                        Message = "Payment Mode is Activeted.";
                    }
                    else
                    {
                        Message = "Payment Mode is Deactiveted.";
                    }
                    ViewBag.Message = Message;
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating payment mode.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
    }
}