using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class EPeopleController : BaseController
    {
        // GET: EPeople
        private readonly IePeopleManager _IePeopleManager;
        private readonly IGuestUserRepository _IGuestUserRepository;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IAdminDashboard _IAdminDashboard;
        private readonly IGuestUser _IGuestUser;
        private readonly IDepartment _IDepartment;
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string FilePath = ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"];
        DBUtilities DBUtilitie = new DBUtilities();
        public EPeopleController(IePeopleManager _IePeopleManager, IAdminDashboard _IAdminDashboard, IGuestUserRepository _IGuestUserRepository, ICommonMethod _ICommonMethod, IDepartment _IDepartment, IGuestUser _IGuestUser, IFillableFormManager _IFillableFormManager)
        {
            this._IePeopleManager = _IePeopleManager;
            this._IGuestUserRepository = _IGuestUserRepository;
            this._ICommonMethod = _ICommonMethod;
            this._IDepartment = _IDepartment;
            this._IGuestUser = _IGuestUser;
            this._IFillableFormManager = _IFillableFormManager;
        }
        public ActionResult Index()
        {
            return View("~/Views/NewAdmin/ePeople/_EmployeeManagement.cshtml");
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 23-Sept-2019
        /// Created for : To get User Employee manager
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserHeirachyList(string Id, long? LocationId)
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
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserHeirarchyList(LocationId, _UserId);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                    details.Add(item);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #region Employee Management

        [HttpPost]
        public JsonResult GetEmpChartData()
        {
            var lstChart = new List<AddChartModel>();
            var _manager = new VehicleSeatingChartManager();
            lstChart = _manager.ListVehicleSeatingChart(1);
            return Json(lstChart, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChartDetailsViewDemo(string Id)
        {
            Session["EmployeeId"] = Id;
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            if (_UserId > 0)
            {
                ViewBag.userId = _UserId.ToString();
            }
            return View("~/Views/NewAdmin/ePeople/NewViewForEMP/EmployeeChartList.cshtml");
        }

        public ActionResult ChartDetailsView(string Id)
        {//D:\Project\eTrac\eTracOrgERP\WorkOrderEMS\Views\NewAdmin\ePeople\_VSCPointingChartDemo.cshtml
            //return PartialView("~/Views/NewAdmin/ePeople/_VSCPointingChart.cshtml");
            Session["EmployeeId"] = Id;
            return PartialView("~/Views/NewAdmin/ePeople/_VSCPointingChartDemo.cshtml");
        }
        //public ActionResult ChartDetailsViewDemo(string Id)
        //{//D:\Project\eTrac\eTracOrgERP\WorkOrderEMS\Views\NewAdmin\ePeople\_VSCPointingChartDemo.cshtml
        //    //return PartialView("~/Views/NewAdmin/ePeople/_VSCPointingChart.cshtml");
        //    Session["EmployeeId"] = Id;
        //    var lstChart = new List<AddChartModel>();
        //    var _manager = new VehicleSeatingChartManager();
        //    lstChart = _manager.ListVehicleSeatingChart(0);
        //    //return View("~/Views/NewAdmin/ePeople/NewViewForEMP/_NewTreeView.cshtml");
        //    return PartialView("~/Views/NewAdmin/ePeople/Requisition/_Chart.cshtml");
        //}
        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 12-OCT-2019
        /// Created For : To get details of vehicle chart
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetChartDisplayDataForEmployee(long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var lstChart = new List<AddChartModel>();
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                var _manager = new VehicleSeatingChartManager();
                lstChart = _manager.ListVehicleSeatingChart(LocationId);
                if (lstChart.Count() > 0)
                {
                    return Json(lstChart, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(lstChart, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(lstChart, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult GetUserTreeViewList(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserTreeViewList(_UserId);
            if (data.Count() > 0)
            {
                //foreach (var item in data)
                //{
                //    item.ProfilePhoto = item.ProfilePhoto == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfilePhoto;
                //    details.Add(item);
                //}
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetUserTreeViewList1(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }

            //ViewBag.UserIdFirstTime = Id;
            ViewBag.UserIdFirstTime = Cryptography.GetEncryptedData(ObjLoginModel.UserId.ToString(), true);
            //var id = Cryptography.GetDecryptedData(Id, true);
            //long _UserId = 0;
            //long.TryParse(id, out _UserId);
            //var data = _IePeopleManager.GetUserTreeViewList(_UserId);
            var data = _IePeopleManager.GetUserTreeViewList(ObjLoginModel.UserId);
            if (data.Count() > 0)
            {
                return PartialView("~/Views/NewAdmin/ePeople/_TreeViewUser1.cshtml", data);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/_TreeViewUser1.cshtml", data);
            }
        }

        [HttpPost]
        public JsonResult GetUserTreeViewListById(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserTreeViewListTesting(_UserId);
            if (data.Count() > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetUserTreeViewListTesting(string Id, long? LocationId, List<UserListViewEmployeeManagementModel> model)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserTreeViewListTesting(_UserId);
            if (data.Count() > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get user details by userid
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserListByUserId(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserListByUserId(LocationId, _UserId);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.ProfilePhoto = item.ProfilePhoto == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfilePhoto;
                    details.Add(item);
                }
                return PartialView("~/Views/NewAdmin/ePeople/_ListUserEMP.cshtml", data);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/_ListUserEMP.cshtml", data);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-oct-2019
        /// Created For : To get VCS Position of User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVehicleSeatingChartPositionedUser(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            long LocationId = 0;
            var details = new UserListViewEmployeeManagementModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ObjLoginModel != null)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetVCSPositionByUserId(_UserId);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewVCS()
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_VSCPintingChart.cshtml");
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created For : To get User Data for edit by s=User Id
        /// Created Date : 10-Oct-2019
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEmployeeDetailsForEdit(string Id)
        {
            var model = new EmployeeVIewModel();
            try
            {
                if (Id != null)
                {
                    var id = Cryptography.GetDecryptedData(Id, true);
                    long _UserId = 0;
                    long.TryParse(id, out _UserId);
                    var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
                    //ObjLoginModel.UserId remove this and add selected Id 
                    model = _IGuestUserRepository.GetEmployee(_UserId);
                    model.Image = model.Image == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + model.Image;
                    return PartialView("~/Views/NewAdmin/ePeople/_EditEmployeeInfo.cshtml", model);
                }
                else
                {
                    return PartialView("~/Views/NewAdmin/ePeople/_EditEmployeeInfo.cshtml", new EmployeeVIewModel());
                }
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/NewAdmin/ePeople/_EditEmployeeInfo.cshtml", new EmployeeVIewModel());
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 11-Oct-2019
        /// Created For : To update editable use details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserEditableInfo(EmployeeVIewModel model)
        {
            bool isSaveSuccess = false;
            try
            {
                isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfoEMPMangemnt(model);
                if (isSaveSuccess)
                    return Json(isSaveSuccess, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(isSaveSuccess, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(isSaveSuccess, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Oct-2019
        /// Created For : To get List of employee management
        /// </summary>
        /// <param name="locationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmployeeManagementList(long locationId)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = ObjLoginModel.UserId;
                }
            }
            try
            {
                var getVSCList = _IePeopleManager.GetEmployeeMgmList(locationId, UserId);
                //if(getVSCList.Count > 0)
                //{
                //    Cryptography.GetEncryptedData(ITAdmin.UserId.ToString(), true);
                //}
                return Json(getVSCList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Employee Management

        #region Requisition

        public ActionResult OpenVSCFormForRequistion()
        {
            return PartialView("~/Views/NewAdmin/ePeople/_AddVSCForRequisition.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created date : 14-Oct-2019
        /// Created For : To Get dropdown list for Requisition 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListRequisition()
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
                var _manager = new VehicleSeatingChartManager();
                var lstDept = _IDepartment.ListAllDepartment("", 0, 0);
                var lstSuperiour = _manager.ListSuperiour();
                lst.listDepartment = lstDept.ToList();
                lst.listSuperiour = lstSuperiour;
            }
            catch (Exception ex)
            {
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 14-Oct-2019
        /// Craeted For :  To send Requistion for approval
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendVCSForApproval(AddChartModel Obj)
        {
            eTracLoginModel ObjLoginModel = null;
            Obj.IsDeleted = false;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    Obj.UserId = ObjLoginModel.UserId;
                }
            }
            try
            {
                if (Obj != null && Obj.SeatingName != null)
                {
                    if (Obj.RolesAndResponsibility != null)
                    {
                        ////will use this when our client want to use tinymce
                        //var ex = System.Text.RegularExpressions.Regex.Replace(Obj.RolesAndResponsibility, @"<[^>]+>|&nbsp;", "").Trim();                       
                        //var removeNR = Obj.RolesAndResponsibility.Replace("\r\n", "");
                        //var removepTag = removeNR.Replace("<p>", "");
                        //var removeendTag = removepTag.Replace("</p>", ",");
                        //var removeSpace = removeendTag.Replace("&nbsp;", " ");
                        //System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        //Obj.RolesAndResponsibility = removeSpace;//rx.Replace(Obj.RolesAndResponsibility, "");
                    }
                    var _manager = new VehicleSeatingChartManager();
                    if (Obj.Id == null)
                    {
                        Obj.Action = "I";
                        Obj.IsActive = "N";
                    }
                    var SavedData = _manager.SaveVSC(Obj);
                    if (SavedData.Id > 0)
                    {
                        var data = _IePeopleManager.ApprovalRequisition(SavedData);
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
                //ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            var newModel = new AddChartModel();
            ///return PartialView("_AddChart", newModel);
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2019
        /// Created For : To get VSC list to delete
        /// </summary>
        /// <returns></returns>
        public JsonResult GetListOfVSCChart()
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
                var getVSCList = _IePeopleManager.GetVSCList();
                return Json(getVSCList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Oct-2019
        /// Created For : TO get VSC details by Id
        /// </summary>
        /// <param name="VSTID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVCSDetailsById(long VSCId)
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
                var getVSCList = _IePeopleManager.GetVSCDetailsById(VSCId);
                getVSCList.Id = VSCId;
                return PartialView("~/Views/NewAdmin/ePeople/_ViewVSCDetails.cshtml", getVSCList);
                //return Json(getVSCList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/NewAdmin/ePeople/_ViewVSCDetails.cshtml", new AddChartModel());
                // return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RequisitionList()
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var data = _IePeopleManager.GetRequisitionlist();
            if (data.Count() > 0)
            {
                //foreach (var item in data)
                //{
                //    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                //    details.Add(item);
                //}
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created by  :Ashwajit Bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To approve reject requisition
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveRejectRequisition(long Id, string Status)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            if (Id > 0 && Status != null)
            {
                var data = _IePeopleManager.ApproveRejectAction(Id, Status, UserId);
                if (data == true)
                {
                    if (Status == "A")
                    {
                        ViewBag.Message = CommonMessage.ApprovedRequisition();
                        return Json(new { Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.RejectRequisition();
                        return Json(new { Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    return Json(new { Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewBag.Message = CommonMessage.FailureMessage();
                return Json(new { Message = ViewBag.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 22-oct-2019
        /// Created For : to send VSC for approval reject
        /// </summary>
        /// <param name="VSCId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendVCSForDeleteApproval(long VSCId)
        {
            eTracLoginModel ObjLoginModel = null;
            var Obj = new AddChartModel();
            Obj.IsDeleted = true;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    Obj.UserId = ObjLoginModel.UserId;
                }
            }
            try
            {
                if (VSCId > 0)
                {
                    Obj.Id = VSCId;
                    var data = _IePeopleManager.ApprovalRequisition(Obj);
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            var newModel = new AddChartModel();
            ///return PartialView("_AddChart", newModel);
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 21-Oct-2019
        /// Created For : To get Job title Count list
        /// </summary>
        /// <param name="VSCId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetJobTitleCount(long VSCId)
        {
            var lst = new List<AddChartModel>();
            try
            {
                if (VSCId > 0)
                {
                    lst = _IePeopleManager.GetJobTitleCountForRequistion(VSCId);
                    return Json(lst, JsonRequestBehavior.AllowGet);
                    //return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", lst);
                }
                else
                {
                    return Json(lst, JsonRequestBehavior.AllowGet);
                    //return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", new List<AddChartModel>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
            //return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", lst);
        }
        /// <summary>
        /// Created By  : Ashwajit bansod
        /// Created Date : 22-Oct-2019
        /// Created For : To get Job Details by Job Id
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobTitleCountById(long JobId)
        {
            var lst = new JobTitleModel();
            try
            {
                if (JobId > 0)
                {
                    lst = _IePeopleManager.GetJobTitleCount(JobId);
                    return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", lst);
                }
                else
                {
                    return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", new List<AddChartModel>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return PartialView("~/Views/NewAdmin/ePeople/Requisition/_AddRemoveJobTitleCount.cshtml", lst);
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date  :22-oct-2019
        /// Created For : To send job title for approval
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendJobCountForApproval(int JobTitleLastCount, int JobTitleId, int JobTitleCount)
        {
            bool IsSaved = false;
            var model = new JobTitleModel();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    model.UserId = ObjLoginModel.UserId;
                }
            }
            try
            {
                if (JobTitleLastCount > 0 && JobTitleId > 0 && JobTitleCount > 0)
                {
                    model.JobTitleLastCount = JobTitleLastCount;
                    model.JobTitleId = JobTitleId;
                    model.JobTitleCount = JobTitleCount;

                    IsSaved = _IePeopleManager.SendJobTitleForApproval(model);
                    if (IsSaved == true)
                    {
                        ViewBag.Message = CommonMessage.JobCountSendApproval();
                        return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        #endregion Requisition

        #region Status Change
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 18-Oct-2019
        /// Created For : To get employee details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenDemotionForm(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var getDetails = _IePeopleManager.GetEMployeeData(_UserId);
            getDetails.StatusAction = EmployeeStatusChnage.D;
            ViewBag.VSCList = _IePeopleManager.GetVSCList();
            if (getDetails != null)
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_DemotionEmployeeForm.cshtml", getDetails);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_DemotionEmployeeForm.cshtml", new DemotionModel());
            }
        }

        [HttpGet]
        public ActionResult OpenEmploymentStatusChange(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var getDetails = _IePeopleManager.GetEMployeeData(_UserId);
            getDetails.StatusAction = EmployeeStatusChnage.S;
            if (getDetails != null)
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_EmploymentStatusChangeForm.cshtml", getDetails);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_EmploymentStatusChangeForm.cshtml", new DemotionModel());
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For  : To view Location to transfer
        /// Created Date : 11-Nov-2019
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenLocationForTransfer(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var getDetails = _IePeopleManager.GetEMployeeData(_UserId);
            getDetails.StatusAction = EmployeeStatusChnage.L;
            if (getDetails != null)
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_LocationTransfer.cshtml", getDetails);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_LocationTransfer.cshtml", new DemotionModel());
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 11-Nov-2019
        /// Created For : TO get vacant job title
        /// </summary>
        /// <param name="VSC_Id"></param>
        /// <returns></returns>
        public JsonResult GetVacantJobTitle(long VSCId)
        {
            var getList = new List<JobTitleModel>();
            try
            {
                if (VSCId > 0)
                {
                    getList = _IePeopleManager.GetJobTitleVacantList(VSCId);
                }
                return Json(getList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 11-Nov-2019
        /// Created For : To save demotion promotion
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveStatus(DemotionModel Obj) 
        {
            string message = "";
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                Obj.UserId = ObjLoginModel.UserId;
            }
            try
            {
                if(Obj != null)
                {
                    var isSaved = _IePeopleManager.SaveCommonStatusOfEmployee(Obj);
                    if(isSaved == true)
                    {
                         message = CommonMessage.Successful();
                    }
                    else
                    {
                        message = CommonMessage.FailureMessage();                       
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult EmployeestatusList()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getDetails = _IePeopleManager.GetEmployeeStatusList();
            if(getDetails.Count() > 0)
            {
                return Json(getDetails, JsonRequestBehavior.AllowGet);
                //return getDetails;
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
                //return null;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-Nov-2019
        /// Created Fro : To approve/Reject Employee Status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveRejectEmployeeStatus(long Id, string Status, string Comment)
        {
            long UserId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                var isApprove = _IePeopleManager.ApproveRejectEmployeeStatus(Id, Status, UserId, Comment);
                if (isApprove == true)
                {
                    string Message = "";
                    if (Status == "A")
                    {
                        Message = CommonMessage.ApprovedEmployeeStatus();
                    }
                    else
                    {
                        Message = CommonMessage.RejectEmployeeStatus();
                    }
                    return Json(Message, JsonRequestBehavior.AllowGet);
                    //return getDetails;
                }
                else
                {
                    return Json(CommonMessage.FailureMessage(), JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Status Change

        #region edit Forms
        [HttpGet]
        public PartialViewResult _DirectDepositeForm(string Id)
        {
            var model = new DirectDepositeFormModel();
            model = _IGuestUser.GetDirectDepositeDataByEmployeeId(Id);
            return PartialView("~/Views/NewAdmin/ePeople/_DirectDepositeEditForm.cshtml", model);
        }
        [HttpGet]
        public PartialViewResult _EmergencyContactForm(string Id)
        {
            var model = new DirectDepositeFormModel();
            model = _IGuestUser.GetDirectDepositeDataByEmployeeId(Id);
            return PartialView("~/Views/Guest/_emergencyContactForm.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 25-Oct-2019
        /// Created For : To save direct deposite form 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult _DirectDepositeForm(DirectDepositeFormModel model)
        {
            if (model != null)
            {
                _IePeopleManager.SaveDirectDepositeForm(model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.NotSaved = true;
            return Json(false, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-Oct-2019
        /// Created For  :To get uploaded files list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFileView(string EMPId)
        {
            var _workorderems = new workorderEMSEntities();
            var model = new List<UploadedFiles>();
            long _UserId = 0;
            try
            {
                if (EMPId != null)
                {
                    var id = Cryptography.GetDecryptedData(EMPId, true);
                    long.TryParse(id, out _UserId);
                }
                var getUser = _workorderems.UserRegistrations.Where(x => x.UserId == _UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                var _FillableFormRepository = new FillableFormRepository();
                if (getUser != null)
                {
                    model = _IePeopleManager.GetUploadedFilesOfUserTesting(getUser.EmployeeID).ToList();               
                    var details = _IGuestUserRepository.GetEmployee(_UserId);
                    ViewBag.ImageUser = details.Image == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + details.Image;
                    ViewBag.EmployeeID = details.EmpId;
                    ViewBag.EmployeeName = details.FirstName + " " + details.LastName;
                    var getDetails = _FillableFormRepository.GetFileList().ToList();
                    ViewBag.GreenList = getDetails.Where(x => x.FLT_FileType == "Green").ToList();
                    ViewBag.Red = getDetails.Where(x => x.FLT_FileType == "Red").ToList();
                    ViewBag.Yellow = getDetails.Where(x => x.FLT_FileType == "Yellow").ToList();
                }
                return PartialView("~/Views/NewAdmin/ePeople/_FilesEmployeeManagement.cshtml", model);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/NewAdmin/ePeople/_FilesEmployeeManagement.cshtml", model);
            }
        }
        [HttpPost]
        public ActionResult GetFileViewTest(string EMPId)
        {
            var _workorderems = new workorderEMSEntities();

            var model = new List<UploadedFiles>();
            long _UserId = 0;
            try
            {
                if (EMPId != null)
                {
                    var id = Cryptography.GetDecryptedData(EMPId, true);
                    long.TryParse(id, out _UserId);
                }
                var getUser = _workorderems.UserRegistrations.Where(x => x.UserId == _UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                var _FillableFormRepository = new FillableFormRepository();
                if (getUser != null)
                {
                    model = _IePeopleManager.GetUploadedFilesOfUserTesting(getUser.EmployeeID);
                    var details = _IGuestUserRepository.GetEmployee(_UserId);
                    ViewBag.ImageUser = details.Image == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + details.Image;
                    ViewBag.EmployeeID = details.EmpId;
                    ViewBag.EmployeeName = details.FirstName + " " + details.LastName;
                    var getDetails = _FillableFormRepository.GetFileList().ToList();
                    ViewBag.GreenList = getDetails.Where(x => x.FLT_FileType == "Green").ToList();
                    ViewBag.Red = getDetails.Where(x => x.FLT_FileType == "Red").ToList();
                    ViewBag.Yellow = getDetails.Where(x => x.FLT_FileType == "Yellow").ToList();
                }
                return PartialView("~/Views/NewAdmin/ePeople/_FilesEmployeeManagement.cshtml", model);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/NewAdmin/ePeople/_FilesEmployeeManagement.cshtml", model);
            }
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 02-Nov-2019
        /// Created For : To get file by file name to view in employee manegement file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileStreamResult GetPDF(string fileName)
        {
            var str = fileName.Replace("'", "");
            FileStream fs = new FileStream(Server.MapPath("~/Content/FilesRGY/" + str), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 07-Nov-2019
        /// Created For : To upload files by file type and employee id
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="FileId"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFiles(string EMPId, long FileId, string FileName)
        {
            eTracLoginModel ObjLoginModel = null;
            var Obj = new UploadedFiles();
            var _workorderems = new workorderEMSEntities();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;

                        }
                        var getUser = _workorderems.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (getUser != null)
                        {
                            if (fname != null)
                            {
                                string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"]), FName);
                                Obj.FileName = FileName;
                                Obj.FileId = FileId;
                                Obj.FileEmployeeId = EMPId;
                                string LoginEmployeeId = getUser.EmployeeID;
                                Obj.AttachedFileName = FName;
                                var IsSaved = _IFillableFormManager.SaveFile(Obj, LoginEmployeeId);
                            }
                        }

                        // Get the complete folder path and store the file inside it.  
                        //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        //file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Nov-2019
        /// Created For : To download file
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult FileDownload(string id, string fileName)
        {
            try
            {
                if (fileName != null)
                {
                    //id = Cryptography.GetDecryptedData(Id, true);
                    //var _eFleetFuelModel = _IeFleetFuelingManager.GeteFleetFuelingDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + FilePath.Replace("~", "");
                        RootDirectory = RootDirectory + FilePath.Replace("~", "") + fileName;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, fileName).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + FilePath.Replace("~", "") + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return Json(ex.Message);
            }
        }
        #endregion edit Forms

        #region Job Post
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 30-Oct-2019
        /// Created For : To open Job posting form
        /// </summary>
        /// <param name="CSVChartId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OpenJobPostingForm(long CSVChartId)
        {
            var objeTracLoginModel = new eTracLoginModel();
            var model = new JobPostingModel();
            var chartModel = new AddChartModel();
            var _manager = new VehicleSeatingChartManager();
            if (CSVChartId > 0)
            {
                var data = _manager.GetChartData(CSVChartId);
                ViewBag.GetHiringManagerList = _manager.GetChartHiringManager(CSVChartId);
                ViewBag.JobTitle = _manager.GetJobTitleData(CSVChartId);
                if (data != null)
                {
                    chartModel.DepartmentName = data.DepartmentName;
                    chartModel.SeatingName = data.SeatingName;
                    chartModel.JobDescription = data.JobDescription.Replace("|", ",");
                    chartModel.RolesAndResponsibility = data.RolesAndResponsibility;
                    chartModel.Id = data.Id;
                    model.AddChartModel = chartModel;
                }
            }
            //return Json("Acc", JsonRequestBehavior.AllowGet);
            return PartialView("~/Views/NewAdmin/ePeople/JobPosting/_AddJobPostingFromVSC.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 30-Oct-2019
        /// Created For : To save Job posting
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveJobPostingData(JobPostingModel Obj)
        {
            var _manager = new VehicleSeatingChartManager();
            bool isSaved = false;
            try
            {
                if (Obj != null)
                {
                    isSaved = _manager.SaveJobPosting(Obj);
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion Job Post

        #region Leave Setup

        public ActionResult LeaveTypeList()
        {
            return View();
        }

        public ActionResult LeaveType(int TypeId)
        {
            Tbl_LeaveType_Setup ISM = new Tbl_LeaveType_Setup();
            ISM.LeaveYear = System.DateTime.Now.Year.ToString();
            ViewBag.Title = "Add Leave Type";
            if (TypeId > 0)
            {
                ISM = Get_LeaveTypeList(TypeId.ToString(), "").FirstOrDefault();
                ViewBag.Title = "Edit Leave Type";
            }
            else
            {
                ISM.IsActive = true;
            }
            return View("LeaveType", ISM);
        }

        [HttpGet]
        public JsonResult GetAllLeaveTypeList(string _search, string LeaveType, string flagApproved = null, string CustomerType = null, string sidx = null, string UserType = null)
        {
            try
            {
                var AllItemList = new List<Tbl_LeaveType_Setup>();
                AllItemList = Get_LeaveTypeList(LeaveType, string.IsNullOrEmpty(_search) == true ? "" : _search).Select(a => new Tbl_LeaveType_Setup()
                {
                    TypeId = a.TypeId,
                    LeaveDesc = a.LeaveDesc,
                    LeaveCount = a.LeaveCount,
                    LeaveYear = a.LeaveYear,
                    IsActive = a.IsActive,
                    IsCarryForward = a.IsCarryForward,
                    EntryBy = a.EntryBy,
                    //BasicStatus = a.BasicStatus,
                    //VehicleStatus = a.VehicleStatus,

                }).OrderByDescending(x => x.TypeId).ToList();
                return Json(AllItemList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public string LeaveTypeSubmit(Tbl_LeaveType_Setup LTS)
        {
            var status = "";
            DataTable Dt = new DataTable();
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                LTS.EntryBy = objLoginSession.UserId.ToString();
                XmlDocument xml = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(LTS.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, LTS);
                    xmlStream.Position = 0;
                    xml.Load(xmlStream);
                }
                Dt = InsertUpdateLeaveType(xml.InnerXml, LTS.TypeId);
                status = "1";
            }
            catch (Exception ex)
            {
                status = "0";
                throw;
            }

            return status;
        }
        public List<Tbl_LeaveType_Setup> Get_LeaveTypeList(string LeaveType, string search = null)
        {
            string QueryString = "EXEC USP_Get_LeaveTypeList '" + LeaveType + "','" + search + "'";
            DataTable dataTable = DBUtilitie.GetDTResponse(QueryString);
            List<Tbl_LeaveType_Setup> ItemList = DataRowToObject.CreateListFromTable<Tbl_LeaveType_Setup>(dataTable);
            return ItemList;
        }

        public DataTable InsertUpdateLeaveType(string xml, int TypeId)
        {
            string QueryString = "exec Usp_InsertUpdateLeaveType '" + xml + "','" + TypeId + "'";
            return DBUtilities.GetDTResponse(QueryString);
        }

        #endregion

        #region Holiday Master

        [HttpGet]
        public ActionResult HolidayMaster()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                var details = new LocationDetailsModel();
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                ViewBag.IsPageRefresh = false;
                return View("~/Views/EPeople/_HolidayMasterDashboard.cshtml");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetListOfHolidayForJSGrid(long locationId, int Typ)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;

            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = ObjLoginModel.UserId;
                }
            }
            try
            {
                string SQRY = "";
                if (Typ == 0)
                {
                    SQRY = "EXEC USP_Get_Holiday_List";
                }
                if (Typ == 1)
                {
                    SQRY = "select HolidayName,CONVERT(varchar(50),HolidayDate,106) as HolidayDates,LocationName from Tbl_HRMS_Holiday LEFT OUTER JOIN LocationMaster on location = LocationId WHERE HolidayType=1";
                }
                if (Typ == 2)
                {
                    SQRY = "select HolidayName,CONVERT(varchar(50),HolidayDate,106) as HolidayDates,LocationName from Tbl_HRMS_Holiday LEFT OUTER JOIN LocationMaster on location = LocationId WHERE HolidayType=2";
                }
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<HolidayManagment> ITAdministratorList = DataRowToObject.CreateListFromTable<HolidayManagment>(DT);

                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OpenGeneralHoliday()
        {
            return PartialView("~/Views/NewAdmin/ePeople/_AddOpenGeneralHoliday.cshtml");
        }

        [HttpPost]
        public ActionResult AddHoliday(HolidayManagment Obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    Obj.CreatedBy = Convert.ToInt32(ObjLoginModel.UserId);
                    Obj.Location = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            try
            {
                DataTable Dt = new DataTable();
                XmlDocument xmlPTDET = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(Obj.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, Obj);
                    xmlStream.Position = 0;
                    xmlPTDET.Load(xmlStream);
                }

                Dt = InsertGeneralHolidays(xmlPTDET.InnerXml);

            }
            catch (Exception ex)
            {
                //ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            var newModel = new HolidayManagment();
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        public DataTable InsertGeneralHolidays(string xmlPTDET)
        {
            string QueryString = "exec Usp_Insert_Holiday '" + xmlPTDET + "'";
            return DBUtilitie.GetDTResponse(QueryString);
        }

        #endregion Holiday Master

        #region Leave Management

        [HttpGet]
        public ActionResult LeaveManagement()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                ViewBag.UserType = ObjLoginModel.UserType;
            }
            ViewBag.IsPageRefresh = false;
            ViewBag.IsPageEdit = true;
            return View();
        }

        [HttpGet]
        public JsonResult GetLeaveManagementchartData()
        {
            string UserId;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                }
                string SQRY = "EXEC USP_Get_Leave_Management_chart_Data '" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<LeaveManagementchartData> ITAdministratorList = DataRowToObject.CreateListFromTable<LeaveManagementchartData>(DT);

                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
        public ActionResult LeaveManagementAddEdit(int Id)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            try
            {
                DataTable dt1 = new DataTable();
                string SQRY1 = "SELECT LeaveDesc,TypeId FROM Tbl_LeaveType_Setup where IsActive=1 ";
                dt1 = DBUtilities.GetDTResponse(SQRY1);
                Leave.ListLTSM = DataRowToObject.CreateListFromTable<LeaveManagementchartData>(dt1);

                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Leave_Management_Edit '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    if (dt != null)
                    {
                        List<Tbl_Employee_Leave_Management> LeaveList = new List<Tbl_Employee_Leave_Management>();
                        LeaveList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                        Leave = LeaveList.Where(c => c.Id == Id).FirstOrDefault();
                        //ViewBag.IsPageRefresh = true;
                        Leave.ListLTSM = DataRowToObject.CreateListFromTable<LeaveManagementchartData>(dt1);
                    }
                }
                else
                {
                    //ViewBag.IsPageRefresh = false;
                }
            }
            catch (Exception ex) { }
            return View("LeaveManagementAddEdit", Leave);
        }
        [HttpGet]
        public JsonResult GetListLeaveManagementJSGrid(string Search)
        {
            string UserId;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "EXEC USP_Get_Leave_Management '" + Search + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> ITAdministratorList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);
                foreach (var items in ITAdministratorList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult LeaveManagmentSubmit(Tbl_Employee_Leave_Management Leave)
        {
            string UserId;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = Convert.ToString(ObjLoginModel.UserId);
            }
            try
            {
                string SQRY = "EXEC INSERT_LEAVE_MANAGEMENT '" + Leave.Id + "','" + Leave.FromDate + "','" + Leave.ToDate + "','" + Convert.ToString(ObjLoginModel.UserId) + "','" + Leave.LeaveReason + "','" + Leave.LeaveType + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("harshalgibbs0507@gmail.com");
                    mail.To.Add("harshalgibbs0507@gmail.com");
                    mail.Subject = "Leave Request";
                    mail.IsBodyHtml = true;
                    string htmlString = @"<html>
                      <body>
                      <p>Dear sir,</p>
                      <p>Thank you for your letter of yesterday inviting me to come for an interview on Friday afternoon, 5th July, at 2:30.
                              I shall be happy to be there as requested and will bring my diploma and other papers with me.</p>
                      <p>Sincerely,<br>--" + Convert.ToString(ObjLoginModel.UserName) + "</br></p></body></html>";
                    mail.Body = htmlString;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("harshalgibbs0507@gmail.com", "A9558767095");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);

                }
                catch (Exception ex)
                {
                    ViewBag.StrError = ex;
                }
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }

            return View("LeaveManagement");
        }

        public ActionResult LeavemanagementDelete(int Id)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Leave_Management_Delete '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    //if (dt != null)
                    //{
                    //    //List<Tbl_Employee_Leave_Management> LeaveList = new List<Tbl_Employee_Leave_Management>();
                    //    //LeaveList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                    //    //Leave = LeaveList.Where(c => c.Id == Id).FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("LeaveManagement", "EPeople");
        }

        public ActionResult LeavemanagementApproved(int Id)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Leave_Management_Approved '" + Id + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    //if (dt != null)
                    //{
                    //    List<Tbl_Employee_Leave_Management> LeaveList = new List<Tbl_Employee_Leave_Management>();
                    //    LeaveList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                    //    Leave = LeaveList.Where(c => c.Id == Id).FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("GetListLeaveManagementJSGridForAdmin", "EPeople");
        }

        public string LeavemanagementRejected(int Id, string RejectReason)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Leave_Management_Rejected '" + Id + "','" + RejectReason + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return "";
        }

        [HttpGet]
        public ActionResult GetListLeaveManagementJSGridForAdmin()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                var details = new LocationDetailsModel();
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                //ViewBag.IsPageRefresh = false;
                return View("_LeaveManagementForAdmin");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetListLeaveManagementForAdmin(string Search)
        {
            string UserId;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "EXEC USP_Get_Leave_Management_ForAdmin '" + Search + "','" + ObjLoginModel.UserType + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> ITAdministratorList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);

                foreach (var items in ITAdministratorList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public JsonResult GetSameDayEmpLeaveDetails(int Id)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "SELECT Id,(FirstName+' '+LastName) as EmployeeName,LeaveReason,LeaveDay,a.FromDate,a.ToDate	from Tbl_Employee_Leave_Management a LEFT OUTER JOIN UserRegistrations b ON b.UserId = a.EmployeeId WHERE a.Id = '" + Id + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> LeaveDetailstList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);
                foreach (var items in LeaveDetailstList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(LeaveDetailstList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public JsonResult GetListSameDayApplyForLeave(string FromDate, string ToDate)
        {
            string UserId;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "EXEC USP_Get_Same_Day_Apply_For_Leave_Detail '" + ObjLoginModel.UserType + "','" + FromDate + "','" + ToDate + "' ";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> ITAdministratorList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);
                foreach (var items in ITAdministratorList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public JsonResult GetListTodayLeave()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "EXEC USP_Get_Same_Day_Apply_For_Leave_Detail '" + ObjLoginModel.UserType + "','" + DateTime.Now.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + "','" + DateTime.Now.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> ITAdministratorList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);
                foreach (var items in ITAdministratorList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                ITAdministratorList = ITAdministratorList.Where(x => x.FromDateString == DateTime.Now.ToString("dd MMM yyyy", CultureInfo.InvariantCulture) && x.Status == "Approved").ToList();
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetLeaveHistoryDetails(int Id)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    ViewBag.UserType = ObjLoginModel.UserType;
                }
                string SQRY = "USP_Get_Leave_History_Details '" + Id + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<Tbl_Employee_Leave_Management> LeaveDetailstList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(DT);
                foreach (var items in LeaveDetailstList)
                {
                    items.FromDateString = items.FromDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    items.ToDateString = items.ToDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(LeaveDetailstList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        #endregion Leave Management

        #region Employee Attendance Management

        [HttpGet]
        public ActionResult WebEmployeeAttendance()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                string SQRY = "EXEC Get_Employee_Attendance_ClockIn_Out '" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                try
                {
                    if (DT.Rows.Count > 0)
                    {
                        ViewBag.Count = DT.Rows[0]["Count"].ToString();
                        ViewBag.BRCount = DT.Rows[0]["BRCount"].ToString();

                        if (DT.Rows[0]["FirstInTime"] != DBNull.Value)
                        {
                            ViewBag.FirstInTime = Convert.ToDateTime(DT.Rows[0]["FirstInTime"]).ToString("hh:mm tt");
                            TimeSpan diff = System.DateTime.Now - Convert.ToDateTime(DT.Rows[0]["FirstInTime"].ToString());
                            string comm = ":";
                            ViewBag.logouttime = string.Concat(diff.Hours, comm, diff.Minutes);
                        } 
                    }
                    else
                    {
                        ViewBag.Count = 0;
                    }
                }
                catch (Exception ex) { }


                SQRY = "EXEC USP_AttendanceDashBorad N'" + Convert.ToString(ObjLoginModel.UserId) + "'  ";
                DT = DBUtilities.GetDTResponse(SQRY);
                ViewBag.IsPageRefresh = false;
                ViewBag.AttendanceDashBoradModel = DataRowToObject.CreateListFromTable<AttendanceDashBoradModel>(DT);

            }
            return View();
        }

        public ActionResult WebEmployeeAttendanceAddEdit(int Id)
        {
            HolidayManagment Holiday = new HolidayManagment();
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Holiday_Management_Edit '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    if (dt != null)
                    {
                        List<HolidayManagment> HolidayList = new List<HolidayManagment>();
                        HolidayList = DataRowToObject.CreateListFromTable<HolidayManagment>(dt);
                        Holiday = HolidayList.Where(c => c.Id == Id).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex) { }

            return View("HolidayMasterAddEdit", Holiday);
        }

        [HttpPost]
        public ActionResult WebEmployeeAttendanceClockIn(int AttendanceType)
        {
            string UserId;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = Convert.ToString(ObjLoginModel.UserId);
            }
            try
            {
                string SQRY = "EXEC INSERT_Employee_Attendance_ClockIn '" + Convert.ToString(ObjLoginModel.UserId) + "','" + AttendanceType + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return RedirectToAction("WebEmployeeAttendance", "EPeople");
        }
        [HttpPost]
        public ActionResult WebEmployeeAttendanceClockOut(int AttendanceType)
        {
            string UserId;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = Convert.ToString(ObjLoginModel.UserId);
            }
            try
            {
                string SQRY = "EXEC INSERT_Employee_Attendance_ClockOut '" + Convert.ToString(ObjLoginModel.UserId) + "','" + AttendanceType + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return RedirectToAction("WebEmployeeAttendance", "EPeople");
        }

        #endregion Employee Attendance Management

        #region Location Seats

        [HttpGet]
        public ActionResult LocationSeats()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            ViewBag.IsPageRefresh = false;
            return View();
        }

        [HttpGet]
        public JsonResult GetListLocationSeatJSGrid(string Search)
        {
            string UserId;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                }
                string SQRY = "EXEC USP_GetSeatLocations '" + Search + "' ";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<tbl_LocationSeats> LocationSeatList = DataRowToObject.CreateListFromTable<tbl_LocationSeats>(DT);
                return Json(LocationSeatList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult LocationSeatSubmit(tbl_LocationSeats Seat)
        {
            string UserId;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = Convert.ToString(ObjLoginModel.UserId);
            }
            try
            {
                string SQRY = "EXEC INSERT_LocationSeat '" + Seat.SeatId + "','" + Seat.LocationSeatName + "','" + Seat.Colour + "','" + Seat.IsActive + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }

            return View("LocationSeats");
        }

        public JsonResult CheckDuplicateLocationSeat(string LocationSeat)
        {
            string Count = "";
            try
            {
                string SQRY = "EXEC USP_Check_LocationSeat_Duplicate '" + LocationSeat + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<tbl_LocationSeats> ShiftCodeList = DataRowToObject.CreateListFromTable<tbl_LocationSeats>(DT);
                Count = ShiftCodeList.Count.ToString();
                return Json(Count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Count, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LocationSeatsDelete(int SeatId)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (SeatId > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "delete from tbl_LocationSeats where SeatId='" + SeatId + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("LocationSeats", "EPeople");
        }

        [HttpGet]
        public JsonResult LocationSeatsEdit(int SeatId)
        {
            try
            {
                string SQRY = "SELECT SeatId,LocationSeatName,Colour,IsActive FROM tbl_LocationSeats where SeatId='" + SeatId + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<tbl_LocationSeats> LocationSeatList = DataRowToObject.CreateListFromTable<tbl_LocationSeats>(DT);
                return Json(LocationSeatList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        #endregion

        #region Schedule
        public ActionResult CoverageMapFormate()
        {
            return View();
        }
        [HttpGet]
        public JsonResult CoverageMapFormateDetails()
        {
            long locationId = 0;
            WorkOrderEMS.Models.eTracLoginModel ObjLogin = (WorkOrderEMS.Models.eTracLoginModel)Session["eTrac"];
            string loginUserName = "", loginUserEmail = "", loginUserProfile = "";
            if (ObjLogin != null)
            {
                locationId = ObjLogin.LocationID;
            }
            string SQRY = "EXEC USP_GetScheduleLocation '" + locationId + "'";
            DataSet DS = DBUtilities.GetDSResponse(SQRY);
            List<tbl_Staffing_Addition_Details> LocationSeatList = DataRowToObject.CreateListFromTable<tbl_Staffing_Addition_Details>(DS.Tables[0]);
            var LocationList = (from e in LocationSeatList
                                select new
                                {
                                    id = e.Id.ToString() + "," + e.EventId.ToString(),
                                    title = e.EmployeeId,
                                    start = e.FromDateTime.ToString("MM/dd/yyyy") + " " + e.FromDateTime.TimeOfDay.ToString(),
                                    end = e.ToDateTime.ToString("MM/dd/yyyy") + " " + e.ToDateTime.TimeOfDay.ToString(),
                                    backgroundColor = e.EventColor
                                }).Distinct().ToList();
            return Json(LocationList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ScheduleLocation(string id, string start, string end)
        {
            long locationId = 0;
            eTracLoginModel ObjLogin = (WorkOrderEMS.Models.eTracLoginModel)Session["eTrac"];
            if (ObjLogin != null)
            {
                locationId = ObjLogin.LocationID;
            }
            VMtbl_Staffing_Addition VMSA = new VMtbl_Staffing_Addition();
            VMSA.TSAM = new tbl_Staffing_Addition();
            VMSA.listTSADM = new List<tbl_Staffing_Addition_Details>();
            VMSA.ListLS = GetSeatLocations("");
            VMSA.TSAM.UserLocation = locationId;
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    ViewBag.Start = start;
                    ViewBag.End = end;
                }
                else
                {
                    string SQRY = "EXEC USP_GetScheduleLocation '" + locationId + "','" + id.Split(',')[1] + "'";
                    DataSet DS = DBUtilities.GetDSResponse(SQRY);
                    VMSA.TSAM = DataRowToObject.CreateListFromTable<tbl_Staffing_Addition>(DS.Tables[0]).FirstOrDefault();
                    VMSA.listTSADM = DataRowToObject.CreateListFromTable<tbl_Staffing_Addition_Details>(DS.Tables[1]);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(VMSA);
        }
        public ActionResult AddLocationSeatHeadDetails(int id, long Location, string fromtime, string endtime)
        {
            tbl_Staffing_Addition_Details objCVD = new tbl_Staffing_Addition_Details
            {
                Id = id,
                Location = Location,
                FromDateTime = fromtime == "" ? System.DateTime.Now : Convert.ToDateTime(fromtime),
                ToDateTime = endtime == "" ? System.DateTime.Now : Convert.ToDateTime(endtime)
            };
            return PartialView("_LocationSeatHeadDetails", objCVD);
        }
        [HttpPost]
        public ActionResult ScheduleLocationSubmit(VMtbl_Staffing_Addition VMSA, List<tbl_Staffing_Addition_Details> LocationSeatHeadDetailsList)
        {
            var status = "";
            DataTable Dt = new DataTable();
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                VMSA.TSAM.EntryBy = objLoginSession.UserId.ToString();
                XmlDocument xml = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(VMSA.TSAM.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, VMSA.TSAM);
                    xmlStream.Position = 0;
                    xml.Load(xmlStream);
                }
                string xmlDet = "<ArrayOfTbl_Staffing_Addition_Details>";
                foreach (var item in LocationSeatHeadDetailsList)
                {
                    xmlDet = xmlDet + "<tbl_Staffing_Addition_Details>";
                    xmlDet = xmlDet + "<EventId>" + item.EventId + "</EventId>";
                    xmlDet = xmlDet + "<EmployeeId>" + item.EmployeeId + "</EmployeeId>";
                    xmlDet = xmlDet + "<FromDateTime>" + item.FromDateTime + "</FromDateTime>";
                    xmlDet = xmlDet + "<ToDateTime>" + item.ToDateTime + "</ToDateTime>";
                    xmlDet = xmlDet + "<EntryBy>" + objLoginSession.UserId.ToString() + "</EntryBy>";
                    xmlDet = xmlDet + "</tbl_Staffing_Addition_Details>";
                }
                xmlDet = xmlDet + "</ArrayOfTbl_Staffing_Addition_Details>";

                Dt = InsertUpdateScheduleLocation(xml.InnerXml, xmlDet, VMSA.TSAM.EventId);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View("CoverageMapFormate");
        }
        public JsonResult GetScheduleEmployee(string EmployeeId)
        {
            long locationId = 0;
            WorkOrderEMS.Models.eTracLoginModel ObjLogin = (WorkOrderEMS.Models.eTracLoginModel)Session["eTrac"];
            string loginUserName = "", loginUserEmail = "", loginUserProfile = "";
            if (ObjLogin != null)
            {
                locationId = ObjLogin.LocationID;
            }
            string SQRY = "EXEC USP_GetScheduleEmployeeList N'" + locationId + "','" + EmployeeId + "'";
            DataSet DS = DBUtilities.GetDSResponse(SQRY);
            tbl_Staffing_Addition_Details LocationSeat = DataRowToObject.CreateListFromTable<tbl_Staffing_Addition_Details>(DS.Tables[0]).FirstOrDefault();
            var LocationSeatTime = new
            {

                start = LocationSeat.FromDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                end = LocationSeat.ToDateTime.ToString("yyyy-MM-dd hh:mm:ss")

            };
            return Json(LocationSeatTime, JsonRequestBehavior.AllowGet);
        }
        public List<tbl_LocationSeats> GetSeatLocations(string Search)
        {
            string SQRY = "EXEC USP_GetSeatLocations '" + Search + "'";
            DataTable DT = DBUtilities.GetDTResponse(SQRY);
            List<tbl_LocationSeats> LocationSeatList = DataRowToObject.CreateListFromTable<tbl_LocationSeats>(DT);
            return LocationSeatList;
        }
        public DataTable InsertUpdateScheduleLocation(string xml, string xmlDet, long EventId)
        {
            string SQRY = "EXEC USP_InsertUpdateScheduleLocation '" + xml + "','" + xmlDet + "','" + EventId + "'";
            return DBUtilities.GetDTResponse(SQRY);
        }

        public JsonResult UpdateEmployeeSchedule(string id, string title, DateTime start, DateTime end)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            string xmlDet = "<tbl_Staffing_Addition_Details>";
            xmlDet = xmlDet + "<Id>" + id.Split(',')[0] + "</Id>";
            xmlDet = xmlDet + "<EventId>" + id.Split(',')[1] + "</EventId>";
            xmlDet = xmlDet + "<EmployeeId>" + title + "</EmployeeId>";
            xmlDet = xmlDet + "<FromTime>" + start.ToString() + "</FromTime>";
            xmlDet = xmlDet + "<ToTime>" + end.ToString() + "</ToTime>";
            xmlDet = xmlDet + "<EntryBy>" + objLoginSession.UserId.ToString() + "</EntryBy>";
            xmlDet = xmlDet + "<FromDateTime>" + start + "</FromDateTime>";
            xmlDet = xmlDet + "<ToDateTime>" + end + "</ToDateTime>";
            xmlDet = xmlDet + "</tbl_Staffing_Addition_Details>";
            string SQRY = "EXEC USP_UpdateEmployeeSchedule '" + id.Split(',')[0] + "','" + xmlDet + "'";
            DataTable Dt = DBUtilities.GetDTResponse(SQRY);
            var jsonObj = new
            {
                Status = Dt.Rows[0]["Status"].ToString(),
                Message = Dt.Rows[0]["Message"].ToString()
            };
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Location wise Shift list
        public ActionResult ListEmpShift()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetListEmpWiseShift(string txtSearch)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            long locationId = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ObjLoginModel != null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }

            try
            {
                string SQRY = "EXEC SP_GetShiftwiseEmpList '" + locationId + "' ";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<UserModel> LocationSeatList = DataRowToObject.CreateListFromTable<UserModel>(DT);
                return Json(LocationSeatList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EmpWiseShiftDelete(int UserId)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            try
            {
                if (UserId > 0)
                {
                    //DataTable dt = new DataTable();
                    //string SQRY = "EXEC USP_Get_Leave_Management_Delete '" + Id + "'";
                    //dt = DBUtilities.GetDTResponse(SQRY);
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("ListEmpShift", "EPeople");
        }


        #endregion

        #region Schedule Overview

        public ActionResult ScheduleOverview()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            ScheduleOverview VMSA = new ScheduleOverview();
            try
            {
                VMSA.ListLS = GetSeatLocations("");
            }
            catch (Exception)
            {
                throw;
            }
            ViewBag.IsPageRefresh = false;
            return View(VMSA);
        }
        [HttpGet]
        public JsonResult GetListScheduleOverview(string Search)
        {
            string UserId;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = Convert.ToString(ObjLoginModel.UserId);
                }
                string SQRY = "EXEC USP_GetScheduleOverview '" + Search + "' ";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<ScheduleOverview> ScheduleOverviewList = DataRowToObject.CreateListFromTable<ScheduleOverview>(DT);
                return Json(ScheduleOverviewList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
        #endregion

        #region Auto Schedule
        public ActionResult AutoSchedule()
        {
            long locationId = 0;
            eTracLoginModel ObjLogin = (WorkOrderEMS.Models.eTracLoginModel)Session["eTrac"];
            if (ObjLogin != null)
            {
                locationId = ObjLogin.LocationID;
            }
            VMtbl_Staffing_Addition VMSA = new VMtbl_Staffing_Addition();
            try
            {
                VMSA.TSAM = new tbl_Staffing_Addition();
                VMSA.TSAM.UserLocation = locationId;
                VMSA.ListLS = GetSeatLocations("");
            }
            catch (Exception)
            {
                throw;
            }
            return View(VMSA);
        }
        #endregion

        #region GRAPH COUNT

        [HttpGet]
        public ActionResult GetCountEmployeeRequisition()
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                var getCount = _IePeopleManager.GetEMP_ReqCount();
                return Json(getCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion GRAPH COUNT
    }
}
