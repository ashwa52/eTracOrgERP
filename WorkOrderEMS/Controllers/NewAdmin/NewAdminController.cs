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

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class NewAdminController : Controller
    {
        // GET: NewAdmin
        private readonly IDepartment _IDepartment;
        private readonly IGlobalAdmin _GlobalAdminManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        
        public NewAdminController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
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
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);               
            }
            UserType _UserType = (WorkOrderEMS.Helper.UserType)ObjLoginModel.UserRoleId;
            if (_UserType == UserType.Administrator)
                ViewBag.Location = _ICommonMethod.GetLocationByAdminId(ObjLoginModel.UserId);
            else if (_UserType == UserType.Manager)
                ViewBag.Location = _ICommonMethod.GetLocationByManagerId(ObjLoginModel.UserId);
            else
                ViewBag.Location = _ICommonMethod.GetAllLocation();
            ViewBag.AssignToUserWO = _GlobalAdminManager.GetLocationEmployeeWO(ObjLoginModel.LocationID);
            ViewBag.AssignToUser = _GlobalAdminManager.GetLocationEmployee(ObjLoginModel.LocationID);
            ViewBag.Asset = _ICommonMethod.GetAssetList(ObjLoginModel.LocationID);
            ViewBag.GetAssetListWO = _ICommonMethod.GetAssetListWO(ObjLoginModel.LocationID);
            ViewBag.UpdateMode = false;
            ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
            ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
            ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
            ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
            ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);
            return PartialView("_OperationDashboard");
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
    }
}