using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.ManagerModels;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    [Authorize]
    public class NewAdminController : Controller
    {
        // GET: NewAdmin
        UserRepository ObjUserRepository;
        private readonly IDepartment _IDepartment;
        private readonly IGlobalAdmin _GlobalAdminManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IQRCSetup _IQRCSetup;
        private readonly IePeopleManager _IePeopleManager;
        private readonly IApplicantManager _IApplicantManager;
        private readonly IGuestUser _IGuestUserRepository;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];


        DBUtilities DB = new DBUtilities();
        public NewAdminController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager, IApplicantManager _IApplicantManager, IGuestUser _IGuestUserRepository)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
            this._IApplicantManager = _IApplicantManager;
            this._IGuestUserRepository = _IGuestUserRepository;
        }
        public ActionResult Index()
        {

            //if(Session["eTrac"]==null)
            //{
            //  string usedd=  HttpContext.User.Identity.Name;
            //    ObjUserRepository = new UserRepository();
            //    UserRegistration loginUser = ObjUserRepository.GetAll(u => u.AlternateEmail == usedd).FirstOrDefault();

            //    Session["eTrac"] = loginUser;
            // }
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
        public JsonResult GetListLocation(string _search, long? UserId, int? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            txtSearch = string.IsNullOrEmpty(_search) ? "" : _search; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
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
        public ActionResult DisplayLocationData(int LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, String sidx = null, string txtSearch = null)
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
                    var serivces = obj_Common_B.GetLocationServicesByLocationID(LocationId, 0);
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
                var ContractDetailsModel = new ContractDetailsModel();
                var LocationRuleMappingModel = new LocationRuleMappingModel();
                details.ContractDetailsModel = GetContractInfo(LocationId).First();
                details.LocationRuleMappingModel = GetLocationRules(LocationId).First();
            }
            catch (Exception ex)
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
        public JsonResult GetWorkOrderList(long LocationId, long workRequestProjectId, string filterwrtype, string filterqrc, string filter)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<WorkRequestAssignmentModelList>();
            long UserId = 0, RequestedBy = 0;
            int? rows = 20; int? page = 1;
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
            var data = _GlobalAdminManager.GetAllWorkRequestAssignmentList(workRequestProjectId, RequestedBy, "GetAllWorkRequestAssignment", page, rows, sidx, sord, txtSearch, LocationId, UserId, StartDate, EndDate, (filter == "All" ? "" : filter), (filterqrc == "All" ? "" : filterqrc), (filterwrtype == "All" ? "" : filterwrtype), paramTotalRecords);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.id = Cryptography.GetEncryptedData(item.WorkRequestAssignmentID.ToString(), true);
                    item.QRCType = String.IsNullOrEmpty(item.QRCType) ? ((item.eFleetVehicleID != null && item.eFleetVehicleID != "" ? "Shuttle Bus" : "N/A")) : item.QRCType + " (" + item.QRCodeID + ")";
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
            return PartialView("~/Views/NewAdmin/ePeople/_EmployeeManagement.cshtml");
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
                {
                    isUTCDay = false;
                }
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
            var data = objDARDetailsList.GetDARDetails(UserId, LocationId, EmployeeId, TastType, page, rows, sord, sord, txtSearch, paramTotalRecords, fromDate, toDate);
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
        public ActionResult userAssessmentView(string Id, string Assesment, string Name, string Image, string JobTitle, string Department, string LocationName)
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

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment,null);
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
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
                    ITAdmin.Status=ITAdmin.Status=="S"? "Assessment Submitted" : ITAdmin.Status == "Y" ? "Assessment Drafted" : "Assessment Pending";
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
            try
            {
                result = _GlobalAdminManager.saveSelfAssessment(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

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
        public ActionResult userEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string Department, string LocationName)
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
            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment=="30"?"31":Assesment=="60"?"61":"91",null);
            ViewData["employeeInfo"] = new GWCQUestionModel(){ EmployeeName=Name,AssessmentType=Assesment,Image=Image, JobTitle=JobTitle,Department=Department,LocationName=LocationName }; 
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
                    ITAdmin.Status = ITAdmin.Status=="C"? "Expectations Submitted" : ITAdmin.Status == "S" ? "Expectations Submitted" : ITAdmin.Status == "Y" ? "Expectations Drafted" : "Expectations Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult userExpectationsView(string Id, string Assesment,string Name,string Image,string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
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

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment,"Expectation");
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;
                item.EmployeeId = Employee_Id;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeId= Employee_Id, EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department= Department, LocationName= LocationName };
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

        [HttpGet]
        public ActionResult HiringOnBoardingDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_HiringOnBoardingDashboardNew.cshtml");
        }
        [HttpGet]
        public ActionResult MyOpenings(long PostingId)
        {
            //var cont = new RecruiteeAPIController();
            //string url = "";
            //var tt = cont.Index();
            var myOpenings = _GlobalAdminManager.GetMyOpenings(PostingId);
            return Json(myOpenings, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult MyInterviews()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var myOpenings = _GlobalAdminManager.GetMyInterviews(ObjLoginModel.UserId);
            return Json(myOpenings, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetJobPostong()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var jobPostings = _GlobalAdminManager.GetJobPostong(ObjLoginModel.UserId);
            return Json(jobPostings, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult InfoFactSheet(MyOpeningModel model)
        {
            InfoFactSheet sheet = new InfoFactSheet { ResumePath = "" };
            sheet.model = model;
                                         
            return PartialView("ePeople/_infoFactSheet", sheet);
        }
        [HttpGet]
        public PartialViewResult GetInterviewers(long applicantId)
        {
            Session["eTrac_questions_number"] = null;
            Session["eTrac_questions"] = null;
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var interviewersList = _GlobalAdminManager.GetInterviewersList(applicantId, ObjLoginModel.UserId);
            return PartialView("ePeople/_interviewers", interviewersList);
        }
        //[HttpGet]
        //public PartialViewResult GetInterviewQuestionView()
        //{
        //    //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
        //    var questions = _GlobalAdminManager.GetInterviewQuestions().ToList();//.Where(x => x.INQ_Id == 1).ToList();
        //    Session["eTrac_questions"] = questions;
        //    return PartialView("ePeople/_questionsview");
        //}
        [HttpGet]
        public PartialViewResult GetInterviewQuestionView(string isExempt)
        {
            //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
            var questions = _GlobalAdminManager.GetInterviewQuestions(isExempt).ToList();//.Where(x => x.INQ_Id == 1).ToList();
            Session["eTrac_questions"] = questions;
            return PartialView("ePeople/_questionsview");
        }
        [HttpGet]
        public PartialViewResult CheckForTypeInterview(long id)
        {
            if(id > 0)
            {
                var getDataForIsExempt = _IApplicantManager.GetRateOfPayInfo(id, null);
                return PartialView("ePeople/OnBoarding/_CheckForDOT", getDataForIsExempt);
            }
            else
            {
                return PartialView("ePeople/OnBoarding/_CheckForDOT");
            }
            //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
            // var questions = _GlobalAdminManager.GetInterviewQuestions("Y").ToList();//.Where(x => x.INQ_Id == 1).ToList();
            //Session["eTrac_questions"] = questions;
            
        }

        [HttpPost]
        public PartialViewResult GetInterviewQuestions(int? id, int Applicant)
        {
            IEnumerable<InterviewQuestionMaster> Masterquestions = (List<InterviewQuestionMaster>)Session["eTrac_questions"];
            var questions = new InterviewQuestionAnswerModel();
            int num = 0;
            if (Applicant > 0 && id != 6)
            {
                if (Masterquestions != null)
                {
                    if (Session["eTrac_questions_number"] != null)
                    {
                        num = (int)Session["eTrac_questions_number"];
                        num += 1;
                        if (num <= Masterquestions.Count() - 1)
                        {

                            Session["eTrac_questions_number"] = num;
                            var currentQus1 = Masterquestions.Skip(num).Take(1).FirstOrDefault();
                            if (id > 0)
                            {
                                num = Convert.ToInt32(id) + 1;
                            }
                            var getLst = _GlobalAdminManager.GetInterviewChildQuestions(num).ToList();
                            if (getLst.Count() > 0)
                            {
                                questions.ChildrenQuestionModel = getLst;
                                questions.MasterId = currentQus1.IQM_Id;
                                questions.MasterQuestion = currentQus1.IQM_Question;
                            }

                            //    MasterId = currentQus1.IQM_Id,
                            //    MasterQuestion = currentQus1.IQM_Question,
                            //     ChildrenQuestionModel = 
                            //}).ToList();
                            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", questions);
                        }
                        //return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", Masterquestions.Where(x => x.INQ_QuestionType == "LastQuestion").FirstOrDefault());
                    }
                    else
                    {
                        var getLst = _GlobalAdminManager.GetInterviewChildQuestions(1).ToList();
                        if (getLst.Count() > 0)
                        {
                            questions.ChildrenQuestionModel = getLst;
                        }
                        var currentQus = Masterquestions.Skip(0).Take(1).Select(x => new InterviewQuestionAnswerModel()
                        {
                            MasterId = x.IQM_Id,
                            MasterQuestion = x.IQM_Question,
                            ChildrenQuestionModel = getLst
                        }).FirstOrDefault();
                        Session["eTrac_questions_number"] = 0;
                        return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
                    }
                }
            }
            else
            {
                var getLst = _GlobalAdminManager.GetInterviewAnswerByApplicantId(Applicant);
                return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_ViewAnswerQuestion.cshtml", getLst);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", null);
        }

        //[HttpPost]
        //public PartialViewResult GetInterviewQuestions(int? id)
        //{
        //    IEnumerable<spGetInterviewQuestion_Result1> questions = (List<spGetInterviewQuestion_Result1>)Session["eTrac_questions"];
        //    int num = 0;
        //    if (questions != null)
        //    {
        //        if (Session["eTrac_questions_number"] != null)
        //        {
        //            num = (int)Session["eTrac_questions_number"];
        //            num += 1;
        //            if (num <= questions.Count() - 1)
        //            {
        //                Session["eTrac_questions_number"] = num;
        //                var currentQus = questions.Skip(num).Take(1).FirstOrDefault();

        //                return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
        //            }
        //            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", questions.Where(x => x.INQ_QuestionType == "LastQuestion").FirstOrDefault());
        //        }
        //        else
        //        {
        //            var currentQus = questions.Skip(0).Take(1).FirstOrDefault();
        //            Session["eTrac_questions_number"] = 0;
        //            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
        //        }
        //    }
        //    return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", null);
        //}
        [HttpPost]
        public JsonResult SaveAnswers(InterviewQuestionAnswerModel model, List<AnswerArr> AnswerArr)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var isAnsSaveSuccess = _GlobalAdminManager.SaveInterviewAnswers(model, AnswerArr, ObjLoginModel.UserId);
            return Json(true ? true : false, JsonRequestBehavior.AllowGet);
        }
        public FileStreamResult GetPDF()
        {
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/resume.pdf"), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }
        #region Hiring On Boarding
        [HttpGet]
        public JsonResult GetApplicantInfo()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var data = _GlobalAdminManager.GetApplicantInfo(ObjLoginModel.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveApplicantInfo(OnboardingDetailRequestModel onboardingDetailRequestModel)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            onboardingDetailRequestModel.CreatedBy = ObjLoginModel.UserId;
            var data = _GlobalAdminManager.SaveApplicantInfo(onboardingDetailRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 05-Nov-2019
        /// Created for : to verify user
        /// </summary>
        /// <param name="onboardingDetailRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyEmployeeAfterGenerate(OnboardingDetailRequestModel onboardingDetailRequestModel)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            onboardingDetailRequestModel.CreatedBy = ObjLoginModel.UserId;
            var data = _GlobalAdminManager.VerifyEmployee(onboardingDetailRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveGuestEmployeeBasicInfo(GuestEmployeeBasicInfoRequestModel guestEmployeeBasicInfoRequestModel)
        {

            var data = _GlobalAdminManager.SaveGuestEmployeeBasicInfo(guestEmployeeBasicInfoRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStateList()
        {
            return Json(_ICommonMethod.GetStateByCountryId(1), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CanInterviewerIsOnline(long ApplicantId, string IsAvailable, string Comment)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var res = _GlobalAdminManager.IsInterviewerOnline(ApplicantId, ObjLoginModel.UserId, IsAvailable, Comment);
            return Json(res, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetScore(long ApplicantId)
        {
            var res = _GlobalAdminManager.GetScore(ApplicantId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckNextQuestion(long ApplicantId, long QusId)
        {
            var res = _GlobalAdminManager.CheckIfAllRespondedForQuestion(ApplicantId, QusId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to  validate the duplicate employee id ie alternativeemail in db col.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidateEmployeeID(string empId)
        {
            var response = _ICommonMethod.CheckEmployeeIdExist(empId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuestEmployee()
        {
            return View("~/Views/NewAdmin/GuestEmployee/GuestEmployee.cshtml");
        }
        [HttpPost]
        public JsonResult AssessmentStatusChange(string Status, string IsActive, long ApplicantId)
        {
            string message = string.Empty;
            long UserId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (Status != null && IsActive != null && ApplicantId > 0)
                {
                    var sendForAssessment = _IePeopleManager.SendForAssessment(Status, IsActive, ApplicantId, UserId);
                    message = CommonMessage.SendAssessment();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = CommonMessage.WrongParameterMessage();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult BackgroundStatusChange(string Status, string IsActive, long ApplicantId)
        {
            string message = string.Empty;
            long UserId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (Status != null && IsActive != null && ApplicantId > 0)
                {
                    var sendForAssessment = _IePeopleManager.SendForBackgroundCheck(Status, IsActive, ApplicantId, UserId);
                    message = CommonMessage.SendAssessment();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = CommonMessage.WrongParameterMessage();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult InterviewAcceptCancel(string status, long ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if(status != null && ApplicantId > 0)
                {
                    string IsActive = "Y";
                    var isAccept = _GlobalAdminManager.SetInterviewAcceptCancel(status, ApplicantId, IsActive);
                }
            }
            catch(Exception ex)
            {

            }
            return PartialView("~/Views/NewAdmin/ePeople/_HiringOnBoardingDashboard.cshtml");
        }

        [HttpPost]
        public PartialViewResult ShowAssetsForApplicant(long ApplicantId)
        {
            var model  = new AssetsAllocationModel();
            model.ApplicantId = ApplicantId;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_AssetsAllocation.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Dec-2019
        /// Created For : To save Assets
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveShowAssetsForApplicant(AssetsAllocationModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);               
            }
            try
            {
                if(model != null)
                {
                    if (model.AssetsId == 0)
                    {
                        model.Action = "I";
                        isSaved = _IApplicantManager.SaveAssets(model);
                    }
                    else
                    {
                        model.Action = "U";
                        isSaved = _IApplicantManager.SaveAssets(model);
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult ShwoOfferLetter(long ApplicantId)
        {
            var model = new OfferModel();
            model.ApplicantId = ApplicantId;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_OfferLetter.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To send offer letter to applicant
        /// created Date : 24-Dec-2019
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendOfferToApplicant(OfferModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (model != null)
                {                    
                   model.Action = "I";
                   model.UserId = ObjLoginModel.UserId;
                   isSaved = _IApplicantManager.SendOffer(model);                                        
                }
            }
            catch (Exception ex)
            {
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveApplicant(CommonApplicantModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            W4FormModel returnModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (model != null)
                {
                    //if (model.ApplicantId == 0)
                    //{
                    ////    model.Action = "I";
                    ////    isSaved = _IApplicantManager.SaveAssets(model);
                    //}
                    //else
                    //{
                        returnModel = _IGuestUserRepository.GetW4Form(ObjLoginModel.UserId);
                        //    model.Action = "U";
                        //    isSaved = _IApplicantManager.SaveAssets(model);

                    //}
                }
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Guest/Index1.cshtml", returnModel);
            }
            return PartialView("~/Views/Guest/_W4Form.cshtml", returnModel);
        }
        #endregion Applicant
        [HttpPost]
        public ActionResult QEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
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

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment,"Evaluation");
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
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
                    ITAdmin.Status = ITAdmin.Status == "C"? "Evaluation Submitted" : ITAdmin.Status == "S" ? "Expectations Submitted" : ITAdmin.Status == "Y" ? "Expectations Drafted" : "Expectations Pending";
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

        #region Location wise Deal Master
        /// <summary>
        /// Created By : Jemin Vasoya
        /// Created Date : 19-Oct-2019
        /// Created For : To Manage Location wise Deal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListDealsSpecific(int id)
        {
            List<ListDealsSpecificToLocation> LDSTL = new List<ListDealsSpecificToLocation>();
            try
            {
                LDSTL = new List<ListDealsSpecificToLocation>();
                LDSTL = GetDealsSpecificList(id);
                //This is for JSGrid
                var tt = LDSTL.ToArray();
                return Json(tt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public List<ContractDetailsModel> GetContractInfo(int LocationId)
        {
            string QueryString = "exec Usp_GetContractInfo '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ContractDetailsModel> ItemList = DataRowToObject.CreateListFromTable<ContractDetailsModel>(dataTable);
            return ItemList;
        }

        public List<ListDealsSpecificToLocation> GetDealsSpecificList(int id)
        {
            string QueryString = "exec Usp_GetDealsSpecificList '" + id + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ListDealsSpecificToLocation> ItemList = DataRowToObject.CreateListFromTable<ListDealsSpecificToLocation>(dataTable);
            return ItemList;
        }

        public List<LocationRuleMappingModel> GetLocationRules(int LocationId)
        {
            string QueryString = "exec Usp_GetContractInfo '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<LocationRuleMappingModel> ItemList = DataRowToObject.CreateListFromTable<LocationRuleMappingModel>(dataTable);
            return ItemList;
        }
        #endregion
        public ActionResult GetCompanyOpening()
        {
            eTracLoginModel ObjLoginModel = null;
            var _workorderems = new workorderEMSEntities();
            var data = new List<spGetJobPosting_ForCompanyOpening_Result>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getEMPID = _workorderems.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId).FirstOrDefault().EmployeeID;
            if(getEMPID != null)
            {

            }
             data = _GlobalAdminManager.GetJobPostingForCompanyOpening(getEMPID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created by  : Ashwajit Bansod
        /// Created Date : 20-Nov-2019
        /// Created For : To get hiring manager count
        /// </summary>
        /// <param name="postingId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHringChartData(long postingId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                var getChartData = _GlobalAdminManager.HiringGraphCount(postingId);
                return Json(getChartData, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult QEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    string Employee_Id = string.Empty;
        //    GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
        //    var details = new LocationDetailsModel();
        //    if (Session["eTrac"] != null)
        //    {
        //        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //    }
        //    List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
        //    try
        //    {
        //        Employee_Id = Cryptography.GetDecryptedData(Id, true);
        //    }
        //    catch (Exception e)
        //    {
        //        Employee_Id = Id;
        //    }

        //    ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment);
        //    foreach (var item in ListQuestions)
        //    {
        //        item.EEL_FinencialYear = FinYear;
        //        item.EEL_FinQuarter = FinQuarter;
        //        item.AssessmentType = Assesment;


        //    }
        //    ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle };
        //    return PartialView("QEvaluationView", ListQuestions);
        //}
        [HttpPost]
        public JsonResult SetupMeeting(SetupMeeting SetupMeeting)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                SetupMeeting.ReceipientEmailId = Cryptography.GetDecryptedData(SetupMeeting.ReceipientEmailId, true);
                result = _GlobalAdminManager.SetupMeetingEmail(SetupMeeting);
            }
            catch (Exception ex)
            {
                result = _GlobalAdminManager.SetupMeetingEmail(SetupMeeting);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMeetingDetail(string Id, string FinYear, string FinQuarter)
        {
            eTracLoginModel ObjLoginModel = null;
            string result =string.Empty;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                Id = Cryptography.GetDecryptedData(Id, true);
                result = _GlobalAdminManager.GetMeetingDetail(Id, FinYear, FinQuarter);
            }
            catch (Exception ex)
            {
                result = _GlobalAdminManager.GetMeetingDetail(Id, FinYear, FinQuarter);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        ///Get Scheduled Meeting List
        ///17/11/2019
        public JsonResult GetMeetingList()
        {
            List<ReviewMeeting> MeetingList;
            try
            {
                MeetingList = _GlobalAdminManager.GetMeetingList();
                foreach (var ITAdmin in MeetingList)
                {
                    ITAdmin.ManagerPhoto = (ITAdmin.ManagerPhoto == "" || ITAdmin.ManagerPhoto == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.ManagerPhoto;
                    ITAdmin.EmployeePhoto = (ITAdmin.EmployeePhoto == "" || ITAdmin.EmployeePhoto == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EmployeePhoto;
                    ITAdmin.MeetingDate = ITAdmin.PRMeetingDateTime.HasValue ? ITAdmin.PRMeetingDateTime.Value.ToShortDateString() : "";
                    ITAdmin.MeetingTime = ITAdmin.PRMeetingDateTime.HasValue ? ITAdmin.PRMeetingDateTime.Value.ToShortTimeString() : "";
                    MeetingList.Add(ITAdmin);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return Json(MeetingList, JsonRequestBehavior.AllowGet);
        }
        #region Scheduler
        public ActionResult GetMyEvents(string start, string end, string _)
        {
            eTracLoginModel ObjLoginModel = null;
            List<EventModel> result = new List<EventModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.GetMyEvents(ObjLoginModel.UserId, start, end);
                var eventList = from e in result
                                select new
                                {
                                    id = e.id,
                                    title = e.title,
                                    start = e.start,
                                    end = e.end,
                                    color = e.StatusColor,
                                    //className = e.ClassName,
                                    //someKey = e.SomeImportantKeyID,
                                    allDay = false
                                };

                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(
     new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)Math.Truncate(tspan.TotalSeconds);
        }
        public string SaveEvent(string Title, string NewEventDate, string  NewEventTime, string NewEventDuration,string JobId, string ApplicantName,string ApplicantEmail,string selectedManagers)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                return _GlobalAdminManager.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration, JobId, ApplicantName, ApplicantEmail, ObjLoginModel.UserId, selectedManagers);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            _GlobalAdminManager.UpdateEvent(id, NewEventStart, NewEventEnd);
        }

        public ActionResult GetBookedSlots(string ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            List<EventModel> result = new List<EventModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.GetBookedSlots(ObjLoginModel.UserId).OrderByDescending(x=>x.start).ToList();
                var eventList = from e in result
                                select new 
                                {
                                    id = e.id,
                                    title = e.title,
                                    startDate = Convert.ToDateTime(e.start).ToShortDateString(),
                                    startTime = Convert.ToDateTime(e.start).ToShortTimeString(),
                                    end = Convert.ToDateTime(e.end).ToShortTimeString(),
                                    color = e.StatusColor,
                                    allDay = false
                                } ;
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetManagerList()
        {
            eTracLoginModel ObjLoginModel = null;
            List<ManagerModel> result = new List<ManagerModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _ICommonMethod.GetManagerList();
                var eventList = from e in result
                                select new
                                {
                                    label=e.UserName,
                                    value=e.UserID.ToString()
                                };
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSlotTimings(string date) {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<CustomSlotTime> list = new List<CustomSlotTime>();
            try
            {
                list= _ICommonMethod.GetSlotTimings(ObjLoginModel.UserId,date);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public void GetOutlookMeeting()
        {
            _GlobalAdminManager.GetOutlookMeetingDetails("","");
        }


        #endregion
		public ActionResult MySchedules() {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return View();
        
        }
        [HttpGet]
        public ActionResult GetManagerAssessmentDetails()
        {
            eTracLoginModel ObjLoginModel = null;
            PerformanceModel model = new PerformanceModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                model = _GlobalAdminManager.GetManagerAssessmentDetails(ObjLoginModel.UserName);
            }
            catch (Exception ex)
            {
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult insertChangedExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveChangedExpectations(data, null, ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult updateChangedExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveChangedExpectations(data, "S", ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult SelfAssessmentView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
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

            ListQuestions = _GlobalAdminManager.GetSelfAssessmentView(Employee_Id, Assesment);
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;
                item.EmployeeId = Employee_Id;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
            return PartialView("SelfAssessmentView", ListQuestions);
        }
    }
}