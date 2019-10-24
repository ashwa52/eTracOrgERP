using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class EPeopleController : Controller
    {
        // GET: EPeople
        private readonly IePeopleManager _IePeopleManager;
        private readonly IGuestUserRepository _IGuestUserRepository;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IAdminDashboard _IAdminDashboard;
        private readonly IDepartment _IDepartment;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        public EPeopleController(IePeopleManager _IePeopleManager, IAdminDashboard _IAdminDashboard, IGuestUserRepository _IGuestUserRepository, ICommonMethod _ICommonMethod, IDepartment _IDepartment)
        {
            this._IePeopleManager = _IePeopleManager;
            this._IGuestUserRepository = _IGuestUserRepository;
            this._ICommonMethod = _ICommonMethod;
            this._IDepartment = _IDepartment;
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
            catch(Exception ex)
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
                 isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfo(model);
                if (isSaveSuccess)
                    return Json(isSaveSuccess, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(isSaveSuccess, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
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
                        var ex = System.Text.RegularExpressions.Regex.Replace(Obj.RolesAndResponsibility, @"<[^>]+>|&nbsp;", "").Trim();                       
                        var removeNR = Obj.RolesAndResponsibility.Replace("\r\n", "");
                        var removepTag = removeNR.Replace("<p>", "");
                        var removeendTag = removepTag.Replace("</p>", ",");
                        var removeSpace = removeendTag.Replace("&nbsp;", " ");
                        System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        Obj.RolesAndResponsibility = removeSpace;//rx.Replace(Obj.RolesAndResponsibility, "");
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
            if(Id > 0 && Status != null)
            {
                var data = _IePeopleManager.ApproveRejectAction(Id, Status, UserId);
                if (data == true)
                {
                    if (Status == "A")
                    {
                        ViewBag.Message = CommonMessage.ApprovedRequisition();
                        return Json(new { Message = ViewBag.Message}, JsonRequestBehavior.AllowGet);
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
        public JsonResult SendJobCountForApproval(int JobTitleLastCount,int JobTitleId, int JobTitleCount)
        {
            bool IsSaved = false;
            var model = new JobTitleModel();
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
            if(getDetails != null)
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
            if (getDetails != null)
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_EmploymentStatusChangeForm.cshtml", getDetails);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/StatusChaneForm/_EmploymentStatusChangeForm.cshtml", new DemotionModel());
            }
        }
        #endregion Status Change
    }
}