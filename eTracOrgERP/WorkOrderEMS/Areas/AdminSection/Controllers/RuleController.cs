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

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class RuleController : Controller
    {
        private readonly IRuleManager _IRuleManager;
        public RuleController(IRuleManager _IRuleManager)
        {
            this._IRuleManager = _IRuleManager;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        // GET: AdminSection/Rule
        public ActionResult Index()
        {
            long LocationId = 0;
            long UserId = 0;
            var ObjLoginModel = new eTracLoginModel();
            ViewBag.AccountSection = true;
            ViewBag.UpdateMode = false;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                LocationId = ObjLoginModel.LocationID;
                
                UserId = ObjLoginModel.UserId;
            }
            ViewBag.UserList = _IRuleManager.GetUserDataList(LocationId, UserId);
            return View("~/Areas/AdminSection/Views/Rule/Index.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Jan-2019
        /// Created For : To get all rules 
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
        public JsonResult GetRuleList(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var ruleList = _IRuleManager.GetRuleList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var rule in ruleList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(rule.RuleId), true);
                    row.cell = new string[7];
                    row.cell[0] = rule.RuleName;
                    row.cell[1] = rule.ModuleName;
                    row.cell[2] = Convert.ToString(rule.Level);
                    row.cell[3] = Convert.ToString(rule.Date);
                    row.cell[4] = Convert.ToString(rule.SlabFrom);
                    row.cell[5] = Convert.ToString(rule.SlabTo);
                    row.cell[6] = rule.IsActive == "Y"?"Active":"Deactive";
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

        /// <summary>
        /// Created By : ashwajit bansod
        /// Created Date : 22-jan-2019
        /// Created for : To save Rule into database
        /// </summary>
        /// <param name="objRuleModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveRule(RuleModel objRuleModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            ViewBag.UpdateMode = false;
            long LocationId = 0;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                LocationId = ObjLoginModel.LocationID;

                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (objRuleModel.RuleId == 0)
                {
                    var savedStatus = _IRuleManager.SaveRule(objRuleModel);
                    if (savedStatus == true)
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
                }
                else
                {
                    var savedStatus = _IRuleManager.SaveRule(objRuleModel);
                    if (savedStatus == true)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }
                return View("~/Areas/AdminSection/Views/Rule/Index.cshtml");
                //return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
            finally
            {
                ViewBag.UserList = _IRuleManager.GetUserDataList(LocationId, UserId);
            }
        }

        /// <summary>
        /// Created By : ashwajit bansod
        /// Created Date : 22-Jan-2019
        /// Created For : To get all user list 
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserList(long LocationId)
        {
            var result = new List<EmployeeListModel>();
            try
            {
                eTracLoginModel ObjLoginModel = null;
                long UserId = 0;
                ViewBag.AccountSection = true;
                
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
                        UserId = ObjLoginModel.UserId;
                    }
                }
                result = _IRuleManager.GetUserDataList(LocationId, UserId);  
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 24-Jan-2019
        /// Created For : To get rule data by Id for edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditRule(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            long LocationId = 0;
            long UserId = 0;
            var objData = new RuleModel();
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.UpdateMode = true;
                    id = Cryptography.GetDecryptedData(id, true);
                    long _ruleId = 0;
                    long.TryParse(id, out _ruleId);
                    var _RuleModel = _IRuleManager.GetRuleDetailsById(_ruleId);
                    LocationId = ObjLoginModel.LocationID;
                    UserId = ObjLoginModel.UserId;
                    ViewBag.UserList = _IRuleManager.GetUserDataList(LocationId, UserId);
                    return View("~/Areas/AdminSection/Views/Rule/Index.cshtml", _RuleModel);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/Rule/Index.cshtml");
        }
    }
}