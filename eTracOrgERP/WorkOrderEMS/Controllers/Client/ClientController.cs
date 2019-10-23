using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Claims;
using System.Web.Mvc;
using WorkOrderEMS.App_Start;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using System.Linq;

namespace WorkOrderEMS.Controllers.Client
{
    [CustomActionFilter]
    [Authorize]
    public class ClientController : Controller
    {
        //
        // GET: /Client/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IManageManager _IManageManager;
        private readonly IClientManager _IClientManager;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IUser _IUser;
        private readonly IVendorManagement _IVendorManagement;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private string path = ConfigurationManager.AppSettings["ProjectLogoPath"];
        private string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);

        public ClientController(ICommonMethod _ICommonMethod, IManageManager _IManageManager, IClientManager _IClientManager, IGlobalAdmin _IGlobalAdmin, IVendorManagement _IVendorManagement, IUser _IUser)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IManageManager = _IManageManager;
            this._IClientManager = _IClientManager;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IVendorManagement = _IVendorManagement;
            this._IUser = _IUser;
        }

        #region client This section is use when user click link in email


        //This Method is use to open the Client  Profile 
        //information when click on verify link in email
        [NonAction]
        public ActionResult Client(string usr)
        {
            try
            {
                //long userid = 53;
                long userid = 0;

                if (!string.IsNullOrEmpty(usr))
                {
                    usr = Cryptography.GetDecryptedData(usr, true);
                    long.TryParse(usr, out userid);
                }
                QRCModel _UserModel = new QRCModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (userid > 0)
                {
                    _UserModel.UserModel = _IClientManager.GetClientById(userid, "GetUserByID", null, null, null, null, null);
                    //_UserModel.Password = Cryptography.GetDecryptedData(_UserModel.Password, true);
                    _UserModel.UserModel.Password = "";
                }
                return View("Client", _UserModel);
            }


            catch (Exception ex) { ViewBag.Error = ex.Message; return View("Error"); }
            //ViewBag.UpdateMode = false;
            //return View();
        }

        //This Method is use to open the Client  Profile 

        [HttpGet]
        public ActionResult ClientProfile()
        {
            try
            {
                // long userid = 53; ;
                long userid = 0;
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                //ViewBag.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
                //userid = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
                userid = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;



                QRCModel _UserModel = new QRCModel();
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                if (userid > 0)
                {
                    _UserModel.UserModel = _IClientManager.GetClientById(userid, "GetUserByID", null, null, null, null, null);
                    _UserModel.UpdateMode = true;
                }
                return View("Client", _UserModel);
            }
            catch (Exception ex)
            {
                { ViewBag.Error = ex.Message; return View("Error"); }
            }
        }

        [HttpPost]
        public ActionResult Client(QRCModel ObjUserModel, string ActionName)
        {
            //Commented By Bhushan DOD for Code review which is unused variable
            //ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            DARModel objDAR = null;

            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                if (ObjUserModel.UserModel.UserId > 0)
                {
                    ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
                    //if (ModelState.IsValid)
                    //{
                    if (ObjUserModel.UserModel.UserId == 0)
                    {
                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjLoginModel.Location);
                    }
                    else
                    {
                        ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;


                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjLoginModel.Location);
                    }
                    //if (Session["ImageName"] != null)
                    //{
                    //    ObjUserModel.UserModel.ProfileImage = Convert.ToString(Session["ImageName"]);
                    //}

                    //Result result = _IGlobalAdmin.SaveLocation(ObjUserModel, out locationId);
                    //if (result == Result.Completed)

                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjLoginModel.LocationID, ObjLoginModel.UserId, "");
                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        return RedirectToAction("Login", "GlobalAdmin");
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                        ModelState.Clear();
                        return RedirectToAction("Login", "GlobalAdmin");

                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                    //   ViewBag.Location = Cryptography.GetEncryptedData(QRCID.ToString(), true);
                    //}
                    //else
                    //{
                    //    ViewBag.Message = CommonMessage.FillAllRequired(); // Please fill all required fields.
                    //}
                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            ViewBag.Country = _ICommonMethod.GetAllcountries();
            ViewBag.UpdateMode = false;
            ObjUserModel.UserModel = _IClientManager.GetClientById(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);
            ViewBag.ActionName = ActionName;
            return View("Client", ObjUserModel);

        }

        #endregion Manager

        #region Work Request OLD

        //[HttpGet]
        //public ActionResult WorkRequest()
        //{
        //    try
        //    {
        //        WorkRequestModel WorkRequestModel = new WorkRequestModel();
        //        ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.TASKTYPE));
        //        ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.WORKPRIORITY));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Error = ex.Message; return View("Error"); }
        //    }
        //}

        //[HttpPost]
        //public ActionResult WorkRequest(WorkRequestModel _WorkRequestModel)
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        if (_WorkRequestModel.WorkRequestID == 0)
        //        {
        //            _WorkRequestModel.ProjectId = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
        //            _WorkRequestModel.RequestBy = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
        //            _WorkRequestModel.CreatedBy = _WorkRequestModel.RequestBy;

        //            //_WorkRequestModel.ProjectId = Session["ProjectID"] != null ? Convert.ToInt32(Session["ProjectID"]) : 0;
        //            //_WorkRequestModel.RequestBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0; ;
        //            //_WorkRequestModel.CreatedBy = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0; ;
        //            _WorkRequestModel.status = Convert.ToString(WorkRequestStatus.Pending);
        //            _WorkRequestModel.CreatedDate = DateTime.Now;
        //            _WorkRequestModel.IsDeleted = false;
        //        }
        //        Result result = _IManageManager.SaveWorkRequest(_WorkRequestModel);
        //        if (result == Result.Completed)
        //        {
        //            ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.success;

        //        }
        //        else if (result == Result.DuplicateRecord)
        //        {
        //            ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
        //        }
        //        else if (result == Result.UpdatedSuccessfully)
        //        {
        //            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.success;// store the message for successful in tempdata to display in view.
        //        }
        //        else
        //        {
        //            ViewBag.Message = CommonMessage.FailureMessage();
        //            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //        }
        //        ViewBag.TaskType = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.TASKTYPE));
        //        ViewBag.TaskPriority = _ICommonMethod.GetGlobalCodeDataList(Convert.ToString(GolbalCodeName.WORKPRIORITY));
        //        ViewBag.WorkArea = _ICommonMethod.GetWorkArea();
        //        return View("WorkRequest", _WorkRequestModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        //    }

        //}

        //public ActionResult WorkRequestList()
        //{
        //    try
        //    {
        //        eTracLoginModel ObjLoginModel = null;
        //        if (Session["eTrac"] != null)
        //        { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

        //        ViewBag.ProjectID = (ObjLoginModel != null && ObjLoginModel.LocationID > 0) ? (ObjLoginModel.LocationID) : 0;
        //        ViewBag.UserId = (ObjLoginModel != null && ObjLoginModel.UserId > 0) ? (ObjLoginModel.UserId) : 0;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        //    }
        //}
        #endregion


        /// <summary>Create
        /// Modified by vijay sahu on 20 feb 2015
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Index(string usr)
        {
            try
            {
                //ViewBag.Country = _ICommonMethod.GetAllcountries();
                //var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                //ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                //ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                //return View("myClient", _UserModel);
                return View();
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>Create
        /// Modified by vijay sahu on 20 feb 2015  
        /// <CreatedFor>Load UI for Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        public ActionResult Create(string usr)
        {
            try
            {
                //string UserId = Cryptography.GetDecryptedData(usr, true);
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                var _UserModel = new QRCModel();//_ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                return View("myClient", _UserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }

        /// <summary>ClientRegistration
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>Load Partial UI to Create New User</CreatedFor>
        /// </summary>
        /// <param name="usr"></param>
        /// <returns></returns>
        //[HttpGet]
        public ActionResult ClientRegistration(UserModel ObjUserModel)
        {
            try
            {
                //UserModel ObjUserModel = new UserModel();
                ViewBag.myModelprefixName = "ClientModel.";
                ViewBag.ActionSection = "client";
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //var _UserModel = _ICommonMethod.LoadInvitedUser(usr);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                return PartialView("_myRegistration", ObjUserModel);
            }
            catch (Exception ex)
            { ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error"); }
        }


        //[HttpGet]
        //public ActionResult ClientRegistration()
        //{
        //    UserModel ObjUserModel = new UserModel();
        //    return RedirectToAction("ClientRegistration", ObjUserModel);
        //}




        /// <summary>Create
        /// <CreatedBy>Nagendra Upwanshi</CreatedBy>
        /// <CreatedOn>Nov-18-2014</CreatedOn>
        /// <CreatedFor>POST method for Create New User</CreatedFor>
        /// </summary>
        /// <param name="ObjUserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(QRCModel ObjUserModel)
        {
            DARModel objDAR = null;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }

                //if (ModelState.IsValid)
                //{

                if (ObjUserModel != null && ObjUserModel.UserModel != null) //&& ObjUserModel.UserModel.UserId == 0
                {
                    if (ObjUserModel.UserModel.UserId == 0)
                    {
                        #region password

                        ObjUserModel.UserModel.Password = _ICommonMethod.CreateRandomPassword();

                        /*
                        ObjUserModel.UserModel.Password = Guid.NewGuid().ToString();
                        ObjUserModel.UserModel.Password = ObjUserModel.UserModel.Password.Length > pwdmaxlendth ? ObjUserModel.UserModel.Password.Substring(0, pwdmaxlendth) : ObjUserModel.UserModel.Password;
                        ObjUserModel.UserModel.Password = Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true);
                        //ObjUserModel.UserModel.Password = (!string.IsNullOrEmpty(ObjUserModel.UserModel.Password)) ? Cryptography.GetEncryptedData(ObjUserModel.UserModel.Password, true) : ObjUserModel.UserModel.Password;
                        */
                        #endregion password

                        ObjUserModel.UserModel.CreatedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.CreatedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewManagerCreatedDar(ObjLoginModel.Location);

                    }
                    else
                    {
                        ObjUserModel.UserModel.ModifiedBy = ObjLoginModel.UserId;
                        ObjUserModel.UserModel.ModifiedDate = DateTime.UtcNow;
                        ObjUserModel.UserModel.IsDeleted = false;

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ManagerUpdatedDar(ObjLoginModel.Location);
                    }

                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(ObjUserModel.UserModel, out QRCID, true, objDAR, ObjLoginModel.LocationID, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        //return View("ITAdministrator");//return RedirectToAction("Create ", "GlobalAdmin");
                        ObjUserModel = _ICommonMethod.LoadInvitedUser(string.Empty);
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                        ModelState.Clear();
                        //return RedirectToAction("index", "GlobalAdmin");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }
                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();

                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();

                //System.Data.Entity.Core.Objects.ObjectParameter paramTotalRecords = new System.Data.Entity.Core.Objects.ObjectParameter("TotalRecords", typeof(int));
                //ObjUserModel.JobTitleList = _ICommonMethod.GetGlobalCodeData("JOBTITLE");
                //ObjUserModel.LocationList = _IGlobalAdmin.GetAllLocationList(0, "GetAllLocation", 1, 10000, "LocationName", "desc", "", paramTotalRecords);
                //paramTotalRecords = null;
            }

            ViewBag.UpdateMode = false;
            //ObjUserModel.UserModel = _IClientManager.GetClientByID(ObjUserModel.UserModel.UserId, "GetUserByID", null, null, null, null, null);
            return View("myClient", ObjUserModel);
        }


        #region WorkRequest New
        [HttpGet]
        public ActionResult WorkRequestAssignment()
        {
            try
            {
                eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //Added By Bhushan Dod on 03/27/2015 for Harcoded change value
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);//Here previously hard coded value 
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.Location = _ICommonMethod.GetLocationByClientId(objeTracLoginModel.UserId);
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult WorkRequestAssignment(WorkRequestAssignmentModel objWorkRequestAssignmentModel)
        {
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            WorkRequestAssignmentModel _objWorkRequestAssignmentModel = new WorkRequestAssignmentModel();
            string msg = string.Empty;
            try
            {

                CommonHelper ObjCommonHelper = new CommonHelper();
                if (objWorkRequestAssignmentModel.WorkRequestAssignmentID == 0)
                {
                    objWorkRequestAssignmentModel.CreatedBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.CreatedDate = DateTime.UtcNow;
                    objWorkRequestAssignmentModel.RequestBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.WorkRequestStatus = 14;
                }
                else
                {
                    objWorkRequestAssignmentModel.ModifiedBy = objeTracLoginModel.UserId;
                    objWorkRequestAssignmentModel.ModifiedDate = DateTime.UtcNow;
                }

                _objWorkRequestAssignmentModel = _IGlobalAdmin.SaveWorkRequestAssignment(objWorkRequestAssignmentModel);
                if (objWorkRequestAssignmentModel.WorkRequestImg != null)
                {
                    WorkRequestImagepath = Server.MapPath(WorkRequestImagepath);
                    ObjCommonHelper.UploadImage(objWorkRequestAssignmentModel.WorkRequestImg, WorkRequestImagepath, objWorkRequestAssignmentModel.WorkRequestImg.FileName);
                }
                if (_objWorkRequestAssignmentModel.Result == Result.Completed)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
                else if (_objWorkRequestAssignmentModel.Result == Result.DuplicateRecord)
                {
                    ViewBag.Message = CommonMessage.DuplicateRecordMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                }
                else if (_objWorkRequestAssignmentModel.Result == Result.UpdatedSuccessfully)
                {
                    ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.

                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                return View("WorkRequestAssignment", _objWorkRequestAssignmentModel);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ViewBag.AssignToUser = _IGlobalAdmin.GetLocationEmployee(objeTracLoginModel.LocationID);
                ViewBag.Asset = _ICommonMethod.GetAssetList(objeTracLoginModel.LocationID);
                ViewBag.Location = _ICommonMethod.GetAllLocation();
                ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
                ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
                ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
            }
        }
        public ActionResult WorkAssignmentList()
        {
            eTracLoginModel objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
            try
            {
                ViewBag.UserID = objeTracLoginModel.UserId;
                return View("WorkAssignmentList");

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Created By : Bhushan Dod on 26/02/2015
        /// Description : To Create work request created client.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateClient(UserModel UserModel)
        {
            //Commented by Ashwajit Bansod for Code review bcoz unused variable.
            //ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            DARModel objDAR = null;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (UserModel != null)
                {
                    UserModel.Password = (!string.IsNullOrEmpty(UserModel.Password)) ? Cryptography.GetEncryptedData(UserModel.Password, true) : UserModel.Password;
                    if (UserModel.UserId == 0)
                    {
                        if (UserModel.ProfileImage != null)
                        {
                            string ClImageName = UserModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + UserModel.ProfileImage.FileName.ToString();
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            UserModel.ProfileImageFile = ClImageName;
                        }
                        UserModel.CreatedBy = ObjLoginModel.UserId;
                        UserModel.CreatedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;
                        objDAR = new DARModel();
                        objDAR.LocationId = UserModel.Location;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewClientCreatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserCreation);
                    }
                    else
                    {
                        if (UserModel.ProfileImage != null)
                        {
                            string ClImageName = UserModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + UserModel.ProfileImage.FileName.ToString();
                            CommonHelper obj_CommonHelper = new CommonHelper();
                            obj_CommonHelper.UploadImage(UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                            UserModel.ProfileImageFile = ClImageName;
                        }
                        UserModel.ModifiedBy = ObjLoginModel.UserId;
                        UserModel.ModifiedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;
                        if (!String.IsNullOrEmpty(UserModel.Password))
                        {
                            UserModel.Password = Cryptography.GetEncryptedData(UserModel.Password, true);
                        }

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ClientUpdatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserUpdate);
                    }
                    long QRCID = 0;
                    //Result result = _IClientManager.SaveClient(ObjLocationMasterModel.ClientModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveClient(UserModel, out QRCID, true, objDAR, 0, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        ModelState.Clear();
                        return View("myClient");
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                        ModelState.Clear();
                        return View("myClient");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.UpdateMode = false;
                UserModel = new UserModel();//_IClientManager.GetClientById(UserModel.UserId, "GetUserByID", null, null, null, null, null);
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
            }//ViewBag.ActionName = ActionName;
            return View("myClient", UserModel);


        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// created Date : 07-May-2019
        /// Created For : TO save New Client
        /// </summary>
        /// <param name="UserModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateNewClient(UserModel UserModel)
        {
            //Commented by Ashwajit Bansod for Code review bcoz unused variable.
            //ObjectParameter paramTotalrecord = new ObjectParameter("TotalRecords", typeof(int));
            var action = this.ControllerContext.RouteData.Values["action"].ToString();
            string ImageUniqueName = string.Empty;
            string ImageURL = string.Empty;
            DARModel objDAR = null;
            try
            {

                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
                if (UserModel != null)
                {
                    UserModel.Password = (!string.IsNullOrEmpty(UserModel.Password)) ? Cryptography.GetEncryptedData(UserModel.Password, true) : UserModel.Password;
                    if (UserModel.ProfileImage != null)
                    {
                        string ClImageName = UserModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + UserModel.ProfileImage.FileName.ToString();
                        CommonHelper obj_CommonHelper = new CommonHelper();
                        obj_CommonHelper.UploadImage(UserModel.ProfileImage, Server.MapPath(ConfigurationManager.AppSettings["ProfilePicPath"]), ClImageName);
                        UserModel.ProfileImageFile = ClImageName;
                    }
                    if (UserModel.SignatureImageBase != null)
                    {
                        string ImagePath = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["UserSignature"].ToString());
                        ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmmsstt") + "UserSignature";
                        string url = HostingPrefix + ImagePath.Replace("~", "") + ImageUniqueName + ".jpg";
                        ImageURL = ImageUniqueName + ".jpg";
                        if (!Directory.Exists(ImagePath))
                        {
                            Directory.CreateDirectory(ImagePath);
                        }

                        var ImageLocation = ImagePath + ImageURL;
                        //var decodedString = ObjUserModel.SignatureImageBase.Replace("=", "");
                        string convert = UserModel.SignatureImageBase.Replace("data:image/png;base64,", String.Empty);
                        string RemoveSpace = convert.Replace(" ", "+");
                        byte[] image64 = Convert.FromBase64String(RemoveSpace);
                        using (MemoryStream ms = new MemoryStream(image64))
                        {
                            using (Bitmap bm2 = new Bitmap(ms))
                            {
                                bm2.Save(ImageLocation);
                            }
                        }
                    }
                    if (UserModel.UserId == 0)
                    {

                        UserModel.SignatureImageBase = ImageURL;
                        UserModel.CreatedBy = ObjLoginModel.UserId;
                        UserModel.CreatedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;
                        objDAR = new DARModel();
                        objDAR.LocationId = UserModel.Location;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.CreatedBy = ObjLoginModel.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.NewClientCreatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserCreation);
                    }
                    else
                    {
                        UserModel.SignatureImageBase = ImageURL;
                        UserModel.ModifiedBy = ObjLoginModel.UserId;
                        UserModel.ModifiedDate = DateTime.UtcNow;
                        UserModel.IsDeleted = false;
                        if (!String.IsNullOrEmpty(UserModel.Password))
                        {
                            UserModel.Password = Cryptography.GetEncryptedData(UserModel.Password, true);
                        }

                        objDAR = new DARModel();
                        objDAR.LocationId = ObjLoginModel.LocationID;
                        objDAR.UserId = ObjLoginModel.UserId;
                        objDAR.ModifiedBy = ObjLoginModel.UserId;
                        objDAR.ModifiedOn = DateTime.UtcNow;
                        objDAR.ActivityDetails = DarMessage.ClientUpdatedDar(ObjLoginModel.Location);
                        objDAR.TaskType = Convert.ToInt64(TaskTypeCategory.UserUpdate);
                    }
                    long QRCID = 0;
                    #region QuickBook Department
                    if (UserModel.UserId == 0)
                    {
                        string realmId = CallbackController.RealMId.ToString();
                        if (realmId != null)
                        {
                            try
                            {
                                string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                                var principal = User as ClaimsPrincipal;
                                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                                // Create a ServiceContext with Auth tokens and realmId
                                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                                DataService commonServiceQBO = new DataService(serviceContext);
                                // Create a QuickBooks QueryService using ServiceContext
                                QueryService<Customer> querySvcDept = new QueryService<Customer>(serviceContext);
                                //List<Department> department = querySvcDept.ExecuteIdsQuery("SELECT * FROM Department");
                                var customer = new Customer();
                                var physicalAddr = new PhysicalAddress();
                                var telephone = new TelephoneNumber();
                                var emailAddr = new EmailAddress();
                                customer.GivenName = UserModel.FirstName;
                                customer.FamilyName = UserModel.LastName;
                                if (UserModel.Address != null)
                                {
                                    physicalAddr.City = UserModel.Address.City;
                                    physicalAddr.Country = "USA";
                                    physicalAddr.Line1 = UserModel.Address.Address1;
                                    physicalAddr.Line2 = UserModel.Address.Address2;
                                    physicalAddr.PostalCode = UserModel.Address.ZipCode;
                                    customer.BillAddr = physicalAddr;
                                }

                                customer.Active = true;
                                customer.ActiveSpecified = true;
                                customer.CompanyName = UserModel.CompanyName;
                                customer.ContactName = UserModel.FirstName + " " + UserModel.LastName;
                                customer.DisplayName = UserModel.FirstName + " " + UserModel.LastName;
                                customer.FullyQualifiedName = UserModel.FirstName + " " + UserModel.LastName;
                                telephone.FreeFormNumber = UserModel.PhoneNumber;
                                customer.Mobile = telephone;
                                if (UserModel.UserEmail != null)
                                {
                                    emailAddr.Address = UserModel.UserEmail;
                                    customer.PrimaryEmailAddr = emailAddr;
                                }
                                //CustomField[] custom = { };
                                //var dataCust = new CustomField();
                                //var departmentLst = new List<CustomField>();
                                //var depart = new Department();
                                customer.PrimaryPhone = telephone;
                                //if(UserModel.SelectedLocation != null)
                                //{
                                //    dataCust.AnyIntuitObject = depart;
                                //    departmentLst.Add(dataCust);                                    
                                //}
                                //customer.CustomField = departmentLst.ToArray();
                                var save = commonServiceQBO.Add(customer) as Customer;
                                if (save != null)
                                {
                                    UserModel.QBKId = save.Id;
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = CommonMessage.FailureMessage();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            }
                        }
                    }
                    #endregion Quickbook
                    //Result result = _IClientManager.SaveClient(ObjLocationMasterModel.ClientModel, out QRCID, true, objDAR);
                    Result result = _IClientManager.SaveNewClient(UserModel, out QRCID, true, objDAR, 0, ObjLoginModel.UserId, "");

                    if (result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        #region Email to Client User

                        //if (UserModel.ExistClientID == 0)
                        //{
                        //    UserModel.Password = Cryptography.GetEncryptedData(UserModel.Password, true);
                        //    UserModel.UserType = Convert.ToInt32(UserType.Client);
                        //    myCommonController.SendEmailToUser(UserModel, UserModel.LocationName, Address, UserModel.Address2, objLoginSession.UserId);
                        //}

                        #endregion Email to Client User
                        ModelState.Clear();
                        return View("ClientList");
                    }
                    else if (result == Result.DuplicateRecord)
                    {
                        ViewBag.Message = CommonMessage.DuplicateRecordEmailIdMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Info; // store the message for successful in tempdata to display in view.
                    }
                    else if (result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;// store the message for successful in tempdata to display in view.
                        ModelState.Clear();
                        return View("ClientList");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                    }

                }
                else { ViewBag.Message = CommonMessage.InvalidEntry(); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            finally
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.UpdateMode = false;
                UserModel = new UserModel();//_IClientManager.GetClientById(UserModel.UserId, "GetUserByID", null, null, null, null, null);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
            }//ViewBag.ActionName = ActionName;
            return View("ClientList", UserModel);


        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-May-2019
        /// Created For : TO view cliet list
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientList()
        {
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-May-2019
        /// Created For : To get all client list by location id
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
        public JsonResult GetListClient(long? UserId, long? LocationId, string _search, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
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
                var lstClient = _IClientManager.GetClientList(UserId, LocationId, rows, TotalRecords, sidx, sord);
                foreach (var client in lstClient.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(client.UserId), true);
                    row.cell = new string[5];
                    row.cell[0] = client.UserName.ToString();
                    row.cell[1] = client.UserEmail.ToString();
                    row.cell[2] = client.UserTypeView.ToString();
                    row.cell[3] = client.LocationIds == null ?"n/A": client.LocationIds;
                    row.cell[4] = client.ProfileImageFile == null ? HostingPrefix + ProfileImagePath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfileImagePath.Replace("~", "") + client.ProfileImageFile;
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

        public ActionResult EditClient(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            //var UserModel = new UserModel();
            string usr = id;
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
                    long _UserId = 0;
                    long.TryParse(id, out _UserId);
                    //UserModel.Location = ObjLoginModel.LocationID;
                    var _UserModel =  _ICommonMethod.LoadInvitedUser(usr);
                    var getCompanyDetails = _IClientManager.GetCompanyDetails(_UserModel.UserModel.UserId);
                    _UserModel.UserModel.CompanyName = getCompanyDetails.CompanyNameLegal;
                    if (_UserModel != null && getCompanyDetails != null)
                    {
                        string csvString = string.Empty;
                        var details = _IClientManager.GetLocationIds(getCompanyDetails.CompanyId);
                        if (details.Count > 0)
                        {
                            _UserModel.UserModel.LocationIds = string.Join(",", details.Select(x => x.LocationId));                   
                        }
                        ViewBag.MultiLocation = _UserModel.UserModel.LocationIds;
                    }                    
                    ViewBag.SignatureImage = _UserModel.UserModel.SignatureImageBase;
                    _UserModel.UserModel.SignatureImageBase = null;
                    //var _UserModel = _IClientManager.GetClientById(_UserId, "GetUserByID", null, null, null, null, null);
                    return View("myClient", _UserModel);
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
            finally
            {
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                //var UserModelData = new UserModel();//_IClientManager.GetClientById(UserModel.UserId, "GetUserByID", null, null, null, null, null);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LocationList = _IGlobalAdmin.GetAllLocationNew();
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
            }
            return View("myClient");
        }


        /// <summary>
        /// Created By : Bhushan Dod on 26/02/2015
        /// Description : To Cancel work request created by client.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelWorkRequestCreatedByClient(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                long WorOrderID = Convert.ToInt64(Cryptography.GetDecryptedData(Id, true));
                var Res = _IClientManager.CancelWorkOrderByEmployee(WorOrderID, ObjLoginModel.UserId);
                if (Res == Result.Delete)
                {
                    return Json("Work order Canceled .");
                }
                else if (Res == Result.DoesNotExist)
                {
                    return Json("Work order not found");
                }
                else
                {
                    return Json(Res);
                }

            }
            else
            {
                return Json("Session Expired.");
            }


        }
    }
}