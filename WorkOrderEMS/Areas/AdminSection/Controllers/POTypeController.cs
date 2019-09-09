using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces.Accounts;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class POTypeController : Controller
    {
        // GET: AdminSection/_IPOType
        private readonly IPOType _IPOType;
        public POTypeController(IPOType _IPOType)
        {
            this._IPOType = _IPOType;
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
            return View("~/Areas/AdminSection/Views/POType/Index.cshtml");
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
        public JsonResult GetPOTypes(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var poTypeList = _IPOType.GetPOTypeList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var contracttype in poTypeList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(contracttype.POT_Id), true);
                    row.cell = new string[2];
                    row.cell[0] = Convert.ToString(contracttype.POT_POName);
                    row.cell[1] = Convert.ToString(contracttype.POT_IsActive);
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

        public ActionResult AddPOType()
        {
            try
            {
                POTypeModel objPOTypeModel = new POTypeModel();
                return PartialView("AddPOType", objPOTypeModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SavePOType(POTypeModel objPOTypeModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                var savedStatus = _IPOType.SavePOType(objPOTypeModel);
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
        /// Created Date : 20-Nov-2018
        /// Created For : To active PO type
        /// </summary>
        /// <param name="POTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivePOType(string POTypeId, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                long _POTypeId = 0;
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
                if (!string.IsNullOrEmpty(POTypeId))
                {
                    POTypeId = Cryptography.GetDecryptedData(POTypeId, true);
                    long.TryParse(POTypeId, out _POTypeId);
                }
                result = _IPOType.ActivePOTypeById(_POTypeId, UserId, IsActive);
                if (result == true)
                {
                    string message = "";
                    if(IsActive == "Y")
                    {
                        message = "Activeted.";
                        ViewBag.Message = message;
                    }
                    else
                    {
                        message = "Deactiveted.";
                        ViewBag.Message = message;
                    }
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating PO Type.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
    }
}