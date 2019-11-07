using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class eTimeController : BaseController
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

        public eTimeController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
        }
        public ActionResult AttandanceManager()
        {
            return View("~/Views/Shared/_AttandanceManager.cshtml");
        }

        /// <summary>
        /// Created By : ND
        /// Created Date : 21-Oct-2019
        /// Created For : To Get all verified user List
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
        public JsonResult GetListEmployeeForAttendanceJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            if (UserType == null)
            {
                UserType = "All Users";
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "Name" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            long TotalRows = 0;
            try
            {
                long paramTotalRecords = 0;
                string SQRY = "EXEC SP_GetAllActiveEmployeeForAttandance N'" + UserId + "','" + page + "', '" + sidx + "','" + sord + "', '" + rows + "','" + txtSearch + "','" + Convert.ToInt64(locationId) + "', '" + UserType + "','" + paramTotalRecords + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<UserModelList> Emp = new List<UserModelList>();
                List<UserModelList> ITAdministratorList = DataRowToObject.CreateListFromTable<UserModelList>(DT);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.ProfileImage = (ITAdmin.ProfileImage == "" || ITAdmin.ProfileImage == null) ? "" : HostingPrefix + ProfileImagePath.Replace("~/", "") + ITAdmin.ProfileImage;
                    ITAdmin.id = Cryptography.GetEncryptedData(ITAdmin.UserId.ToString(), true);
                    detailsList.Add(ITAdmin);
                }

                //List<UserModelList> ITAdministratorList = _IGlobalAdmin.GetAllITAdministratorList(UserId, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
               
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By  : ND 
        /// Created Date : 21-Oct-2019
        /// Created For : To get VSC list 
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAttendanceDataUserwise(long? LocationId, long? UserId, int? Month)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
                try
                {
                    if (Month == null || Month < 1 || Month > 12)
                    {
                        Month = DateTime.Now.Month;
                    }
                    string SQRY = "EXEC SP_GetEmployeeAttendanceList N'" + LocationId + "','" + UserId + "', '" + Month + "'";
                    DataTable DT = DBUtilities.GetDTResponse(SQRY);
                    List<Employee_AttendanceModel> lstChart = DataRowToObject.CreateListFromTable<Employee_AttendanceModel>(DT);
                    if (lstChart.Count() > 0)
                    {
                        foreach (var item in lstChart)
                        {
                            item.AttendanceDateString = item.AttendanceDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                            item.DatumString = item.Datum.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                            item.AttendanceDayString = item.AttendanceDate.ToString("dddd");
                            item.InTimeString = item.InTime == null ? "00:00" : item.InTime.ToString("hh:mm tt");
                            item.OutTimeString = item.OutTime == null ? "00:00" : item.InTime.ToString("hh:mm tt");
                            item.TotalHoursString = item.TotalHoursString == null ? "00:00" : item.TotalHours.ToString("HH:mm");
                            item.Present = item.LeaveStatus == false ? item.Present : "L";
                        }
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
            }else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddPunchTimerequest()
        {
            Employee_PunchTimeRequestModel Model = new Employee_PunchTimeRequestModel();
            Model.AttendanceDate = DateTime.Now;
            return PartialView("~/Views/NewAdmin/ePeople/_PunchTimeRequest.cshtml", Model);
        }

        [HttpPost]
        public ActionResult AddPunchTimerequest(Employee_PunchTimeRequestModel Model)
        {
            long LocationId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                Model.UserId = ObjLoginModel.UserId;
                Model.CreatedBy = ObjLoginModel.UserId;
            }

            string SQRY = "EXEC SP_PunchTimeRequest N'" + Model.UserId + "','" + LocationId + "','" + Model.AttendanceDate.ToString("dd MMM yyyy") + "','" + Model.InTimeString + "','" + Model.OutTimeString + "', N'" + Model.Remarks + "'";
            DataTable DT = DBUtilities.GetDTResponse(SQRY);
            List<UserModelList> Emp = new List<UserModelList>();

            return PartialView("~/Views/NewAdmin/ePeople/_PunchTimeRequest.cshtml", Model);
            //return RedirectToAction("AttandanceManager", "eTime");
        }
    }
}