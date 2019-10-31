using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class NewAdminController : Controller
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
        
        public NewAdminController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
        }
        public ActionResult Index()
        {
            return View("~/Views/Shared/_NewDashboard.cshtml");
        }
        public ActionResult ListLocation()
        {
            return View();
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-August-2019
        /// Created For : To get location List
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
        public JsonResult GetListLocation(string _search, long? UserId, int? locationId , int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //if (locationId == null)
                //{
                //    locationId = Convert.ToInt32(ObjLoginModel.LocationID);
                //}
                UserId = ObjLoginModel.UserId;
            }

            JQGridResults result = new JQGridResults();
            var result1 = new List<ListLocationModel>();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "LocationId" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var data = _GlobalAdminManager.ListAllLocation(locationId, page, rows, sidx, sord, txtSearch, paramTotalRecords);

                foreach (var locList in data)
                {
                    //Convert Id to Encrypted data
                    var id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                    locList.Id = id;
                    result1.Add(locList);
                }
                //This is for JSGrid
                var tt = result1.ToArray();
                //foreach (var locList in result1)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                //    row.cell = new string[11];
                //    row.cell[0] = locList.LocationName;
                //    row.cell[1] = locList.Address + "," + locList.City + ", " + locList.State + ", " + locList.ZipCode + "," + locList.Country;
                //    row.cell[2] = locList.LocationAdministrator;
                //    row.cell[3] = locList.LocationManager;
                //    row.cell[4] = locList.LocationEmployee;
                //    row.cell[5] = locList.City;
                //    row.cell[6] = locList.State;
                //    row.cell[7] = locList.Country;
                //    row.cell[8] = locList.PhoneNo + " / " + locList.Mobile;
                //    row.cell[9] = locList.Description;
                //    row.cell[10] = Convert.ToString(locList.QRCID);
                //    rowss.Add(row);
                //}
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
                return Json(tt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }           
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-August-2019
        /// Created For : To get Location details by Location Id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="sidx"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DisplayLocationData(int LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, String sidx = null, string txtSearch =null)
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            try
            {
                sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
                sidx = string.IsNullOrEmpty(sidx) ? "LocationId" : sidx;
                var obj_Common_B = new Common_B();
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var data = _GlobalAdminManager.LocationDetailByLocationID(LocationId);
                if (data.Count() > 0)
                {
                    var ListLocationModel = new ListLocationModel();
                    var serivces  = obj_Common_B.GetLocationServicesByLocationID(LocationId, 0);
                    foreach (var locList in data)
                    {
                        //var id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                        ListLocationModel.LocationName = locList.LocationName;
                        ListLocationModel.Address = locList.Address1 + "," + locList.Address2;
                        ListLocationModel.City = locList.City;
                        ListLocationModel.State = locList.LocationState;
                        ListLocationModel.Country = locList.LocationCountry;
                        ListLocationModel.Mobile = locList.PhoneNo + " / " + locList.Mobile;
                        ListLocationModel.Description = locList.Description;
                        ListLocationModel.ZipCode = locList.ZipCode;
                        ListLocationModel.LocationSubTypeDesc = locList.LocationSubTypeDesc;
                        //ListLocationModel.LocationCode = locList.Loc;
                        ListLocationModel.LocationServices = serivces;
                    }
                    details.ListLocationModel = ListLocationModel;
                }

            }
            catch(Exception ex)
            {

            }
            return View(details);
        }
        [HttpGet]
        public ActionResult AddNewLocation()
        {
            return View();
        }
        #region Operation
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-August-2019
        /// Created For : To Show Operation Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OperationDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            long Totalrecords = 0;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);               
            }
            //#region WO
            //UserType _UserType = (WorkOrderEMS.Helper.UserType)ObjLoginModel.UserRoleId;
            //if (_UserType == UserType.Administrator)
            //    ViewBag.Location = _ICommonMethod.GetLocationByAdminId(ObjLoginModel.UserId);
            //else if (_UserType == UserType.Manager)
            //    ViewBag.Location = _ICommonMethod.GetLocationByManagerId(ObjLoginModel.UserId);
            //else
            //ViewBag.Location = _ICommonMethod.GetAllLocation();
            //ViewBag.AssignToUserWO = _GlobalAdminManager.GetLocationEmployeeWO(ObjLoginModel.LocationID);
            //ViewBag.AssignToUser = _GlobalAdminManager.GetLocationEmployee(ObjLoginModel.LocationID);
            //ViewBag.Asset = _ICommonMethod.GetAssetList(ObjLoginModel.LocationID);
            //ViewBag.GetAssetListWO = _ICommonMethod.GetAssetListWO(ObjLoginModel.LocationID);
            //ViewBag.UpdateMode = false;
            //ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
            //ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
            //ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
            //ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
            //ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);
            //#endregion WO        
            return PartialView("_OperationDashboard");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View eMaiantanace
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EMaintananceDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            } 
            return PartialView("~/Views/NewAdmin/WorkOrderView/_WorkOrderDashboard.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View eScan
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EScanDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/QRCView/_eScanDashboard.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View DAR
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DARDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/DAR/_DARDashboard.cshtml");
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 26-Sept-2019
        /// Created For : To View Report of WO, DAR, QRC
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OperationReports()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/_Reports.cshtml");
        }
        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 01-Sept-2019
        /// Created For : TO get Work order List
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="workRequestAssignmentId"></param>
        /// <param name="OperationName"></param>
        /// <param name="UserId"></param>
        /// <param name="RequestedBy"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="filter"></param>
        /// <param name="filterqrc"></param>
        /// <param name="filterwrtype"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="sidx"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>

        //public JsonResult GetWorkOrderList(int LocationId,long? workRequestProjectId, long UserId, long?RequestedBy ,  string filter, string filterqrc, string filterwrtype,int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, String sidx = null, string txtSearch = null)        
        [HttpGet]
        public JsonResult GetWorkOrderList(long LocationId, long workRequestProjectId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<WorkRequestAssignmentModelList>();
            long UserId = 0, RequestedBy = 0;
            string filter = ""; string filterqrc = ""; string filterwrtype = ""; int? rows = 20; int? page = 1;
            int? TotalRecords = 10; string sord = null; String sidx = null; string txtSearch = "";
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "CreatedDate" : sidx;
            DateTime StartDate = DateTime.UtcNow;
            DateTime EndDate = DateTime.UtcNow;
            var obj_Common_B = new Common_B();
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            //var data = _GlobalAdminManager.GetAllWorkRequestAssignmentList(workRequestAssignmentId, RequestedBy, OperationName, page, rows, sord, sidx, txtSearch, LocationId, UserId, StartDate, EndDate, filter, filterqrc, filterwrtype, paramTotalRecords);
            var data = _GlobalAdminManager.GetAllWorkRequestAssignmentList(workRequestProjectId, RequestedBy, "GetAllWorkRequestAssignment", page, rows, sidx, sord, txtSearch, LocationId, UserId, StartDate, EndDate, (filter == "All" ? "" : filter), (filterqrc == "All" ? "" : filterqrc), (filterwrtype == "All" ? "" : filterwrtype), paramTotalRecords);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.id = Cryptography.GetEncryptedData(item.WorkRequestAssignmentID.ToString(), true);
                    item.QRCType =  String.IsNullOrEmpty(item.QRCType) ? ((item.eFleetVehicleID != null && item.eFleetVehicleID != "" ? "Shuttle Bus" : "N/A")) : item.QRCType + " (" + item.QRCodeID + ")";
                    item.FacilityRequestType = (item.FacilityRequestType == null || item.FacilityRequestType.TrimWhiteSpace() == "" || item.FacilityRequestType.Trim() == "") ? "N/A" : item.FacilityRequestType;
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                    item.AssignedWorkOrderImage = item.AssignedWorkOrderImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkRequestImagepath.Replace("~", "") + item.AssignedWorkOrderImage;
                    details.Add(item);
                }
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(details, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Operation

        #region ePeople
        public ActionResult ePeopleDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }            
            return PartialView("_ePeopleDashboard");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-Sept-2019
        /// Created For : To get All user List by location
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserList(long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var data = _IePeopleManager.GetUserList(LocationId);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {                   
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;                    
                    details.Add(item);
                }
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(details, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ePeople
        [HttpGet]
        public JsonResult GeDARList(long? LocationId, int? TastType, long? EmployeeId, string FromDate, string ToDate, string FromTime, string ToTime)
        {
            eTracLoginModel ObjLoginModel = null;
            DARManager objDARDetailsList = new DARManager();
            var details = new List<WorkRequestAssignmentModelList>();
            long UserId = 0;
            string sord = null; String sidx = null; string txtSearch = "";
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                    UserId = ObjLoginModel.UserId;
                }
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            //Fetching record like 2017-06-11T00:00:00-04:00 to 2017-06-12T00:0000-04:00
            string fromDate = (FromDate == null || FromDate == " " || FromDate == "") ? clientdt.Date.ToString() : FromDate;
            string toDate = (ToDate == null || ToDate == " " || ToDate == "") ? clientdt.AddDays(1).Date.ToString() : ToDate;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null && ToDate != "" && FromDate != "null")
            {
                DateTime tt = Convert.ToDateTime(toDate);
                if (tt.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }

            if (fromDate != null && toDate != null)
            {
                DateTime frmd = Convert.ToDateTime(fromDate);
                DateTime tod = Convert.ToDateTime(toDate);
                ////if interval date come then need to fetch record till midnight of todate day
                if ((frmd.Date != tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
                if ((frmd.Date == tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM"))
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
            }
            //Converting datetime from userTZ to UTC
            fromDate = Convert.ToDateTime(fromDate).ConvertClientTZtoUTC().ToString();
            toDate = Convert.ToDateTime(toDate).ConvertClientTZtoUTC().ToString();
            int? rows = 20; int? page = 1;
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "CreatedDate" : sidx;
            var obj_Common_B = new Common_B();
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
           var data  = objDARDetailsList.GetDARDetails(UserId, LocationId, EmployeeId, TastType, page, rows, sord, sord, txtSearch, paramTotalRecords, fromDate, toDate);           
            if (data.Count() > 0)
            {               
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PerformanceManagement()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("_PerformanceManagement");
        }
        [HttpPost]
        public ActionResult userAssessmentView(string Id,string Assesment)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch(Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment);
            return PartialView("userAssessmentView", ListQuestions);
        }
        public ActionResult PerformanceManagementGrid()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("_306090Grid");
        }
        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
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
        public JsonResult GetListOf306090ForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<PerformanceModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                
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
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOf306090ForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;
                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status=ITAdmin.Status=="S"?"Review Submitted": ITAdmin.Status == "Y" ? "Review Draft":"Assessment Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult draftSelfAssessment(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try {
                result=_GlobalAdminManager.saveSelfAssessment(data,"D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result,JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveSelfAssessment(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveSelfAssessment(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult userEvaluationView(string Id, string Assesment,string Name,string Image,string JobTitle)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }
            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment=="30"?"31":Assesment=="60"?"61":"91");
            ViewData["employeeInfo"] = new GWCQUestionModel(){ EmployeeName=Name,AssessmentType=Assesment,Image=Image, JobTitle=JobTitle }; 
            return PartialView("userEvaluationView", ListQuestions);
        }

        [HttpPost]
        public JsonResult draftEvaluation(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveEvaluation(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveEvaluation(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveEvaluation(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
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
        public JsonResult GetListOfQExpectationsForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<PerformanceModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }

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
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOfExpectationsForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;

                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status=="C"?"Review Submitted":ITAdmin.Status == "S" ? "Review Submitted" : ITAdmin.Status == "Y" ? "Review Draft" : "Assessment Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult userExpectationsView(string Id, string Assesment,string Name,string Image,string JobTitle, string FinYear, string FinQuarter)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment);
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle };
            return PartialView("userExpectationsView", ListQuestions);
        }

        [HttpPost]
        public JsonResult draftExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveExpectations(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveExpectations(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult QEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment);
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle };
            return PartialView("QEvaluationView", ListQuestions);
        }

        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
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
        public JsonResult GetListOfQEvaluationsForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<PerformanceModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }

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
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOfQEvaluationsForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;

                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status == "C"? "Review Submitted":ITAdmin.Status == "S" ? "Review Submitted" : ITAdmin.Status == "Y" ? "Review Draft" : "Assessment Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult draftQEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveQEvaluations(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveQEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                foreach (var item in data)
                {
                    item.EEL_EMP_EmployeeIdManager = ObjLoginModel.UserName; 


                }
                result = _GlobalAdminManager.saveQEvaluations(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult saveFinalEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                foreach (var item in data)
                {
                    item.EEL_EMP_EmployeeIdManager = ObjLoginModel.UserName;


                }
                result = _GlobalAdminManager.saveQEvaluations(data, "C");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }

}