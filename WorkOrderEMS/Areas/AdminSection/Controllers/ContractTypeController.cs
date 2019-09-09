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
    public class ContractTypeController : Controller
    {
        // GET: AdminSection/ContractType
        private readonly IContractType _IContractType;
        public ContractTypeController(IContractType _IContractType)
        {
            this._IContractType = _IContractType;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        public ActionResult Index()
        {
            try
            {
                ViewBag.AccountSection = true;
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/ContractType/Index.cshtml");
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
        public JsonResult GetContractTypes(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var contractTypeList = _IContractType.GetContractTypeList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var contracttype in contractTypeList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(contracttype.CTT_Id), true);
                    row.cell = new string[3];
                    row.cell[0] = Convert.ToString(contracttype.CTT_ContractType);
                    row.cell[1] = Convert.ToString(contracttype.CTT_Discription);
                    row.cell[2] = Convert.ToString(contracttype.CTT_IsActive);
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

        public ActionResult AddContractType()
        {
            try
            {
                ViewBag.AccountSection = true;
                ContractTypeModel objContractTypeModel = new ContractTypeModel();
                return PartialView("AddContractType", objContractTypeModel);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SaveContractType(ContractTypeModel objContractTypeModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                var savedStatus = _IContractType.SaveContractType(objContractTypeModel);
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
                return View("~/Areas/AdminSection/Views/ContractType/Index.cshtml");
                //return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
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
        /// Created For : To activate contract Type as per contract Id.
        /// </summary>
        /// <param name="ContractTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveContractType(string ContractTypeId, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                long _COntractTypeId = 0;
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
                if (!string.IsNullOrEmpty(ContractTypeId))
                {
                    ContractTypeId = Cryptography.GetDecryptedData(ContractTypeId, true);
                    long.TryParse(ContractTypeId, out _COntractTypeId);
                }
                result = _IContractType.ActiveContractTypeById(_COntractTypeId, UserId, IsActive);
                if (result == true)
                {
                    string Message = "";
                    if (IsActive == "Y")
                    {
                        Message = "Activeted.";
                        ViewBag.Message = "Activeted.";
                    }
                    else
                    {
                        Message = "Deactiveted.";
                        ViewBag.Message = "Deactiveted.";
                    }
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating Contract Type.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
    }
}