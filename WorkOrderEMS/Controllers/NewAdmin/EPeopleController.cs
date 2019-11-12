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
        public ActionResult ChartDetailsView(string Id)
        {//D:\Project\eTrac\eTracOrgERP\WorkOrderEMS\Views\NewAdmin\ePeople\_VSCPointingChartDemo.cshtml
            //return PartialView("~/Views/NewAdmin/ePeople/_VSCPointingChart.cshtml");
            Session["EmployeeId"] = Id;
            return PartialView("~/Views/NewAdmin/ePeople/_VSCPointingChartDemo.cshtml");
        }
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
            ViewBag.UserIdFirstTime = Id;
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserTreeViewList(_UserId);
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
                    model = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
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
        public JsonResult SaveStatus(DemotionModel Obj) 
        {
            string message = "";
            try
            {
                if(Obj != null)
                {
                    var isSaved = _IePeopleManager.SavePromoDemo(Obj);
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
                    model = _IePeopleManager.GetUploadedFilesOfUser(getUser.EmployeeID);
                }
                if (getUser != null)
                {
                    var details = _IGuestUserRepository.GetEmployee(_UserId);
                    ViewBag.ImageUser = details.Image == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + details.Image;
                    ViewBag.EmployeeID = details.EmpId;
                    ViewBag.EmployeeName = details.FirstName + " " + details.LastName;
                    var getDetails = _FillableFormRepository.GetFileList();
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
        /// Created By  :Ashwajit bansod
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
                                string LoginEMployeeId = getUser.EmployeeID;
                                Obj.AttachedFileName = FName;
                                var IsSaved = _IFillableFormManager.SaveFile(Obj, LoginEMployeeId);
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

        #region Holiday Master
        /// <summary>
        /// Created By : Tushar Goyani
        /// Created Date : 20-Oct-2019
        /// Created For : To Manage Holiday Master
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HolidayMaster()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return View();
        }

        public ActionResult HolidayMasterAddEdit(int Id)
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

        [HttpGet]
        public JsonResult GetListHolidayJSGrid(string Search)
        {
            try
            {
                string SQRY = "EXEC USP_Get_Holiday_Management '" + Search + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<HolidayManagment> ITAdministratorList = DataRowToObject.CreateListFromTable<HolidayManagment>(DT);
                foreach (var items in ITAdministratorList)
                {
                    items.HolidayDateString = items.HolidayDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                }
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult HolidayManagmentSubmit(HolidayManagment Holiday)
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
                string SQRY = "EXEC INSERT_HOLIDAY_MANAGEMENT '" + Holiday.Id + "','" + Holiday.HolidayDate + "','" + Holiday.HolidayName + "','" + Holiday.Description + "','" + Holiday.IsActive + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return View("HolidayMaster");
        }

        public ActionResult HolidayMasterAddDelete(int Id)
        {
            HolidayManagment Holiday = new HolidayManagment();
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Holiday_Management_Delete '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    if (dt != null)
                    {
                        List<HolidayManagment> HolidayList = new List<HolidayManagment>();
                        HolidayList = DataRowToObject.CreateListFromTable<HolidayManagment>(dt);
                        Holiday = HolidayList.Where(c => c.Id == Id).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("HolidayMaster", "EPeople");
        }

        #endregion Holiday Master

        #region Leave Management
        /// <summary>
        /// Created By : Tushar Goyani
        /// Created Date : 20-Oct-2019
        /// Created For : To Manage Leave
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveManagement()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return View();
        }

        public ActionResult LeaveManagementAddEdit(int Id)
        {
            Tbl_Employee_Leave_Management Leave = new Tbl_Employee_Leave_Management();
            try
            {
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
                    }
                }
            }
            catch (Exception ex) { }

            return View("LeaveManagementAddEdit", Leave);
        }

        [HttpGet]
        public JsonResult GetListLeaveManagementJSGrid(string Search)
        {
            try
            {
                string SQRY = "EXEC USP_Get_Leave_Management '" + Search + "'";
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
                string SQRY = "EXEC INSERT_LEAVE_MANAGEMENT '" + Leave.Id + "','" + Leave.FromDate + "','" + Leave.ToDate + "','" + Leave.EmployeeId + "','" + Leave.LeaveReason + "','" + Leave.LeaveType + "','" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return View("LeaveManagement");
        }

        public ActionResult LeavemanagementDelete(int Id)
        {
            Tbl_Employee_Leave_Management Holiday = new Tbl_Employee_Leave_Management();
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_Leave_Management_Delete '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    //if (dt != null)
                    //{
                    //    //List<Tbl_Employee_Leave_Management> HolidayList = new List<Tbl_Employee_Leave_Management>();
                    //    //HolidayList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                    //    //Holiday = HolidayList.Where(c => c.Id == Id).FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("LeaveManagement", "EPeople");
        }

        public ActionResult LeavemanagementApproved(int Id)
        {
            Tbl_Employee_Leave_Management Holiday = new Tbl_Employee_Leave_Management();
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
                    //    List<Tbl_Employee_Leave_Management> HolidayList = new List<Tbl_Employee_Leave_Management>();
                    //    HolidayList = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                    //    Holiday = HolidayList.Where(c => c.Id == Id).FirstOrDefault();
                    //}
                }
            }
            catch (Exception ex) { ViewBag.StrError = ex; }
            return RedirectToAction("LeaveManagement", "EPeople");
        }

        public ActionResult LeavemanagementRejected(int Id, string RejectReason)
        {
            Tbl_Employee_Leave_Management Holiday = new Tbl_Employee_Leave_Management();
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
            return RedirectToAction("LeaveManagement", "EPeople");
        }

        #endregion Leave Management

        #region Employee Attendance Management
        /// <summary>
        /// Created By : Tushar Goyani
        /// Created Date : 20-Oct-2019
        /// Created For : To Manage Employee Attendance
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WebEmployeeAttendance()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                string SQRY = "EXEC Get_Employee_Attendance_ClockIn_Out '" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                if (DT.Rows.Count > 0)
                {
                    ViewBag.Count = DT.Rows[0]["Count"].ToString();
                    ViewBag.LoginTime = DT.Rows[0]["LoginTime"].ToString();
                }
                else
                {
                    ViewBag.Count = 0;
                }
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

        [HttpGet]
        public ActionResult WebEmployeeAttendanceClockIn()
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
                string SQRY = "EXEC INSERT_Employee_Attendance_ClockIn '" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return RedirectToAction("WebEmployeeAttendance", "EPeople");
        }

        public ActionResult WebEmployeeAttendanceClockOut()
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
                string SQRY = "EXEC INSERT_Employee_Attendance_ClockOut '" + Convert.ToString(ObjLoginModel.UserId) + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return RedirectToAction("WebEmployeeAttendance", "EPeople");
        }

        #endregion Employee Attendance Management

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
