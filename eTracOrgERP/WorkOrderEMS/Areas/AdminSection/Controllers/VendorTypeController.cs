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
    public class VendorTypeController : Controller
    {
        // GET: AdminSection/_IVendorType
        private readonly IVendorType _IVendorType;
        public VendorTypeController(IVendorType _IVendorType)
        {
            this._IVendorType = _IVendorType;
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
            return View("~/Areas/AdminSection/Views/VendorType/Index.cshtml");
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : 19-Oct-2018
        /// Created For : Fetching vendor type jqgrid.
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
        public JsonResult GetVendorTypes(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var vendorTypeList = _IVendorType.GetVendorTypeList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var contracttype in vendorTypeList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(contracttype.Vendor_Id), true);
                    row.cell = new string[2];
                    row.cell[0] = Convert.ToString(contracttype.VendorType);
                    row.cell[1] = Convert.ToString(contracttype.Vendor_IsActive);
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

        public ActionResult AddVendorType()
        {
            try
            {
                VendorTypeModel objVendorTypeModel = new VendorTypeModel();
                return PartialView("AddVendorType", objVendorTypeModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult SaveVendorType(VendorTypeModel objVendorTypeModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                var savedStatus = _IVendorType.SaveVendorType(objVendorTypeModel);
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
        /// Created By Ashutosh Dwivedi 10/19/2017
        /// Deletion of Vendor Type
        /// </summary>
        /// <param name="VendorID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteVendorType(string VendorID)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null; long loggedInUser = 0, vendorId = 0;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                loggedInUser = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                if (!string.IsNullOrEmpty(VendorID))
                {
                    VendorID = Cryptography.GetDecryptedData(VendorID, true);
                }
                vendorId = Convert.ToInt64(VendorID);
                Result result = _IVendorType.DeleteVendorType(vendorId, loggedInUser, ObjLoginModel.Location);
                if (result == Result.Delete)
                {
                    ViewBag.Message = CommonMessage.DeleteSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (result == Result.Failed)
                {
                    ViewBag.Message = "Can't Delete ";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
            }
            catch (Exception ex)
            { throw ex; }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 20-Nov-2018
        /// Created For : To Active Vendor type as per Vemdor id.
        /// </summary>
        /// <param name="VendorTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveVendorType(string VendorTypeId,string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                long _VendorTypeId = 0;
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
                if (!string.IsNullOrEmpty(VendorTypeId))
                {
                    VendorTypeId = Cryptography.GetDecryptedData(VendorTypeId, true);
                    long.TryParse(VendorTypeId, out _VendorTypeId);
                }
                result = _IVendorType.ActiveVendorTypeById(_VendorTypeId, UserId, IsActive);
                if (result == true)
                {
                    if(IsActive == "Y")
                    {
                        ViewBag.Message = "Activeted.";
                    }
                    else
                    {
                        ViewBag.Message = "Deactiveted.";
                    }
                   
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
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