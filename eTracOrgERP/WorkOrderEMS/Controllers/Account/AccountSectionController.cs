using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class AccountSectionController : Controller
    {
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For: For Account section
        /// Created Date : July-07-2018
        /// </summary>
        /// <returns></returns>
        // GET: AccountSection
        //    private readonly ICostCode _ICostCode;
        //    public AccountSectionController(ICostCode _ICostCode)
        //    {
        //        this._ICostCode = _ICostCode;
        //    }
        //    AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        //    public ActionResult CostCodeAndRule()
        //    {
        //        ViewBag.AccountSection = true;
        //        return View("CostCodeandRules");
        //    }
        //    /// <summary>
        //    /// Created By : Ashwajit Bansod
        //    /// Created Date : 18 July 2018
        //    /// Created For : To Save Cost Code 
        //    /// </summary>
        //    /// <param name="objCostCodeModel"></param>
        //    /// <returns></returns>
        //    public ActionResult SaveCostCode(CostCodeModel objCostCodeModel)
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        ViewBag.AccountSection = true;
        //        bool isUpdate = false;
        //        if (Session != null)
        //        {
        //            if (Session["eTrac"] != null)
        //            {
        //                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //                if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
        //                {
        //                    (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
        //                }
        //            }
        //        }
        //        try
        //        {
        //            if(objCostCodeModel.CostCodeId == 0)  //Save cost code
        //            {
        //                objCostCodeModel.CreatedBy = ObjLoginModel.UserId;
        //                objCostCodeModel.CreatedDate = DateTime.UtcNow;
        //                objCostCodeModel.IsDeleted = false;
        //                objCostCodeModel.LocationId = ObjLoginModel.LocationID;
        //                var SaveCostCodeData = _ICostCode.SaveCostCode(objCostCodeModel);
        //                if (objCostCodeModel.Result == Result.Completed)
        //                {
        //                    ModelState.Clear();
        //                    ViewBag.Message = CommonMessage.eFleetDriverSaveSuccessMessage();
        //                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //                }
        //            }
        //            else  //edit cost code data update
        //            {
        //                isUpdate = true;
        //                objCostCodeModel.ModifiedBy = ObjLoginModel.UserId;
        //                objCostCodeModel.ModifiedDate = DateTime.UtcNow;
        //                objCostCodeModel.LocationId = ObjLoginModel.LocationID;
        //                var SaveCostCodeData = _ICostCode.SaveCostCode(objCostCodeModel);
        //                if (objCostCodeModel.Result == Result.UpdatedSuccessfully)
        //                {
        //                    ModelState.Clear();
        //                    ViewBag.Message = CommonMessage.eFleetDriverUpdateSuccessMessage();
        //                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
        //        }
        //        return View("CostCodeList");
        //    }
        //    /// <summary>
        //    /// Created By : Ashwajit Bansod
        //    /// Created Date : July-17-2018
        //    /// Created For : To show List of Cost code list
        //    /// </summary>
        //    /// <returns></returns>
        //    public ActionResult ListCostCode()
        //    {
        //        try
        //        {
        //            eTracLoginModel ObjLoginModel = null;
        //            ViewBag.AccountSection = true;
        //            //var returnModel = new eFleetDriverModel();
        //            if (Session != null)
        //            {
        //                if (Session["eTrac"] != null)
        //                {
        //                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //                    if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
        //                    {
        //                        (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
        //                    }
        //                }
        //            }
        //            return View("CostCodeList");
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.Message = ex.Message;
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
        //        }
        //        return View();
        //    }
        //    /// <summary>
        //    /// Created By : Ashwajit Bansod
        //    /// Created Date : July-17-2018
        //    /// Created For : To fetch Cost data from database and display in JQ Grid.
        //    /// </summary>
        //    /// <param name="_search"></param>
        //    /// <param name="UserId"></param>
        //    /// <param name="locationId"></param>
        //    /// <param name="rows"></param>
        //    /// <param name="page"></param>
        //    /// <param name="TotalRecords"></param>
        //    /// <param name="sord"></param>
        //    /// <param name="txtSearch"></param>
        //    /// <param name="sidx"></param>
        //    /// <param name="UserType"></param>
        //    /// <returns></returns>
        //    [HttpGet]
        //    public JsonResult GetListCostCode(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        ViewBag.AccountSection = true;
        //        if (Session["eTrac"] != null)
        //        {
        //            ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //            if (locationId == null)
        //            {
        //                locationId = ObjLoginModel.LocationID;
        //            }
        //            UserId = ObjLoginModel.UserId;
        //        }
        //        CostCodeModel objCostCodeModel = new CostCodeModel();
        //        JQGridResults result = new JQGridResults();
        //        List<JQGridRow> rowss = new List<JQGridRow>();
        //        sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
        //        sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
        //        txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
        //        try
        //        {
        //            var costCodeList = _ICostCode.GetListCostCode(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
        //            foreach (var costCode in costCodeList.rows)
        //            {
        //                if (costCode.IsDeleted == false)
        //                {
        //                    JQGridRow row = new JQGridRow();
        //                    row.id = Cryptography.GetEncryptedData(Convert.ToString(costCode.CostCode), true);
        //                    row.cell = new string[3];
        //                    row.cell[0] = costCode.CostCode.ToString();
        //                    row.cell[1] = costCode.Description;
        //                    row.cell[2] = costCode.IsActive;
        //                    rowss.Add(row);
        //                }
        //            }
        //            result.rows = rowss.ToArray();
        //            result.page = Convert.ToInt32(page);
        //            result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
        //            result.records = Convert.ToInt32(TotalRecords.Value);
        //        }
        //        catch (Exception ex)
        //        { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //    /// <summary>
        //    /// Created By : Ashwajit Bansod
        //    /// Created Date : 20 July 2018
        //    /// Created For : To fetch sub cost code data as per Master cost code Id
        //    /// </summary>
        //    /// <param name="CostCodeId"></param>
        //    /// <param name="_search"></param>
        //    /// <param name="UserId"></param>
        //    /// <param name="locationId"></param>
        //    /// <param name="rows"></param>
        //    /// <param name="page"></param>
        //    /// <param name="TotalRecords"></param>
        //    /// <param name="sord"></param>
        //    /// <param name="txtSearch"></param>
        //    /// <param name="sidx"></param>
        //    /// <param name="UserType"></param>
        //    /// <returns></returns>
        //    [HttpGet]
        //    public JsonResult GetListOfSubCostCode(string id, string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        ViewBag.AccountSection = true;
        //        long _CostCodeId = 0;
        //        if (Session["eTrac"] != null)
        //        {
        //            ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //            if (locationId == null)
        //            {
        //                locationId = ObjLoginModel.LocationID;
        //            }
        //            UserId = ObjLoginModel.UserId;
        //        }
        //        if (!string.IsNullOrEmpty(id))
        //        {
        //            id = Cryptography.GetDecryptedData(id, true);
        //            long.TryParse(id, out _CostCodeId);
        //        }
        //        CostCodeModel objCostCodeModel = new CostCodeModel();
        //        JQGridResults result = new JQGridResults();
        //        List<JQGridRow> rowss = new List<JQGridRow>();
        //        sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
        //        sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
        //        txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
        //        try
        //        {
        //            var costCodeList = _ICostCode.GetListOfSubCostCode(_CostCodeId, UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
        //            foreach (var costCode in costCodeList.rows)
        //            {
        //                if (costCode.IsDeleted == false)
        //                {
        //                    JQGridRow row = new JQGridRow();
        //                    row.id = Cryptography.GetEncryptedData(Convert.ToString(costCode.CostCode), true);
        //                    row.cell = new string[5];
        //                    row.cell[0] = costCode.CostCode.ToString();
        //                    row.cell[2] = costCode.Description;
        //                    row.cell[3] = costCode.IsActive;
        //                    rowss.Add(row);
        //                }
        //            }
        //            result.rows = rowss.ToArray();
        //            result.page = Convert.ToInt32(page);
        //            result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
        //            result.records = Convert.ToInt32(TotalRecords.Value);
        //        }
        //        catch (Exception ex)
        //        { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //}
    }
}