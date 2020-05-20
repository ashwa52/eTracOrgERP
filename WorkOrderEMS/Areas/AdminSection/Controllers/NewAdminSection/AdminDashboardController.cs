using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminSection/AdminDashboard
        private readonly IAdminDashboard _IAdminDashboard;
        private readonly IDepartment _IDepartment;
        public AdminDashboardController(IAdminDashboard _IAdminDashboard, IDepartment _IDepartment)
        {
            this._IAdminDashboard = _IAdminDashboard;
            this._IDepartment = _IDepartment;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public ActionResult Index()
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
            //Put nothing for this to get Department list

            return View();
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get Dropdown list because we use partial view so will bind dropdown using ajax
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetList()
        {
            var lst = new BindDropDownList();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                var lstDept = _IDepartment.ListAllDepartment("", 0, 0);
                var lstSuperiour = _IAdminDashboard.ListSuperiour();
                lst.listDepartment = lstDept.ToList();
                lst.listSuperiour = lstSuperiour;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveVCS(AddChartModel Obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                if (Obj != null && Obj.SeatingName != null)
                {
                    if (Obj.RolesAndResponsibility != null)
                    {
                        var ex = System.Text.RegularExpressions.Regex.Replace(Obj.RolesAndResponsibility, @"<[^>]+>|&nbsp;", "").Trim();
                        var removepTag = Obj.RolesAndResponsibility.Replace("<p>", "");
                        var removeendTag = removepTag.Replace("</p>", ",");
                        var removeSpace = removeendTag.Replace("&nbsp;", " ");
                        System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        Obj.RolesAndResponsibility = removeSpace;//rx.Replace(Obj.RolesAndResponsibility, "");
                    }
                    if (Obj.Id == null)
                    {
                        Obj.Action = "I";
                        Obj.IsActive = "Y";
                    }
                    var isSaved = _IAdminDashboard.SaveVSC(Obj);
                    if (isSaved != null)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            var newModel = new AddChartModel();
            return PartialView("_AddChart", newModel);
            //return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 29-Aug-2019
        /// Created For : To get VSC list 
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetChartDisplayData(long? LocationId)
        {
            //var lstChart = new List<AddChartModel();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                var lstChart = _IAdminDashboard.ListVehicleSeatingChart(LocationId);
                if (lstChart.Count() > 0)
                {
                    return Json(lstChart, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Message = ex.Message }, JsonRequestBehavior.AllowGet);
                // ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : To save Job Title
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveJobTitle(AddChartModel Obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                if (Obj != null && Obj.JobTitleDesc != null)
                {
                    //Obj.Id = Obj.parentId;
                    var isSaved = _IAdminDashboard.SaveJobTitleVSC(Obj);
                    if (isSaved == true)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : To get Job title data
        /// </summary>
        /// <param name="CSVChartId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetJobTitleById(long CSVChartId)
        {
            eTracLoginModel ObjLoginModel = null;
            var lst = new List<AddChartModel>();
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                if (CSVChartId > 0)
                {
                    lst = _IAdminDashboard.GetJobTitleData(CSVChartId);
                    if (lst != null)
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetChartDetailsById(long CSVChartId)
        {
            eTracLoginModel ObjLoginModel = null;
            var lst = new AddChartModel();
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                if (CSVChartId > 0)
                {
                    lst = _IAdminDashboard.GetChartData(CSVChartId);
                    if (lst != null)
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By  : Ashwajit Bansod
        /// Created Date: 20-05-2020
        /// Created For : TO soft delete job title
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteJobTitleById(long JobId)
        {
            eTracLoginModel ObjLoginModel = null;
            bool delete = false;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                delete = _IAdminDashboard.DeleteJobTitleById(JobId);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(delete, JsonRequestBehavior.AllowGet);
        }
    }
}