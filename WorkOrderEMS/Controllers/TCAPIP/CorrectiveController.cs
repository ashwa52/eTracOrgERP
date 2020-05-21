using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;

namespace WorkOrderEMS.Controllers.CorrectiveAct
{
    public class CorrectiveController : Controller
    {
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly ICorrectiveManager _ICorrectiveManager;
        
        public CorrectiveController(ICorrectiveManager _ICorrectiveManager, IFillableFormManager _IFillableFormManager)
        {
            this._ICorrectiveManager = _ICorrectiveManager;
            this._IFillableFormManager = _IFillableFormManager;
        }


        /// <summary>
        /// Created by: Rajat Toppo
        /// Created For: Action method to get data into CorrectiveAction Form
        /// Date: 13-03-2020
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CorrectiveActionForm(string EMPId)
        {
                //string empid = "HR1234";
                var objCorrectiveActionModel = new CorrectiveActionModel();
                try
                {
                    if (EMPId != null)
                    {
                        objCorrectiveActionModel = _ICorrectiveManager.EmployeeDetailsForCorrectiveActionForm(EMPId);

                        return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveActionModel);
                    }
                    else
                    { 
                        return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", new CorrectiveActionModel());
                    }
                }
                catch (Exception ex)
                {

                }

                return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml");            
        }

        public JsonResult EmployeeIsExempt(string EMPId)
        {
            var objCorrectiveAction = new CorrectiveActionModel();
            try
            {
                objCorrectiveAction = _ICorrectiveManager.EmployeeDetailsForCorrectiveActionForm(EMPId);
                return Json(objCorrectiveAction);
            }
            catch (Exception Ex)
            {

            }

            return Json("");
        }

        /// <summary>
        /// Sending Meeting Time and Date to employee through Notification
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult MeetingWithEmolyee(string EMPId, WitnessModel obj)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _ICorrectiveManager.SendMeetingTimeToEmployeeByNotification(EMPId, ObjLoginModel.UserName, obj);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View("");
        }
        /// <summary>
        /// Saving Corrective action meeting date and time
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public EmptyResult SaveMeetingDateTime(CorrectiveActionModel obj)
        {
            try
            {
                if(obj != null)
                {
                   var DateTimeSaved = _ICorrectiveManager.SaveMeetingDateTimewithEmployeeId(obj);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return new EmptyResult();
        }

        public ActionResult MeetingWithEmolyeeNonExempt(string EMPId, WitnessModel obj)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _ICorrectiveManager.SendMeetingTimeToEmployeeByNotificationNonExempt(EMPId, ObjLoginModel.UserName, obj);

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View("");
        }


        /// <summary>
        /// For manager and employee review during meeting
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="HRA"></param>
        /// <returns></returns>
        public ActionResult CorrectiveActionFormReview(string EMPId)
        {
            var objCorrectiveActionModel = new CorrectiveActionModel();
            try
            {
                if (EMPId != null)
                {
                    objCorrectiveActionModel = _ICorrectiveManager.CorrectiveActionEmployeeFormDetailsForReview(EMPId);
                    //objCorrectiveActionModel = _ICorrectiveManager.GetEmployeeDetails(EMPId);
                    return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveActionModel);
                }
                else
                {
                    return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", new CorrectiveActionModel());
                }
            }
            catch (Exception ex)
            {

            }

            return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml");

        }

        /// <summary>
        /// Created By: Rajat Toppo
        /// Created For: Controller to set data into database for CorrectiveAction Form
        /// Date: 13-03-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveCorrectiveAction(CorrectiveActionModel obj)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }

                bool isset = _ICorrectiveManager.SaveEmployeeCorrectiveActionDetails(obj);
                if(isset == true)
                {
                    _ICorrectiveManager.SaveCorrectiveActionNotificationbyManager(obj,ObjLoginModel.UserName);
                }
                return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml");
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml");
        }

        public EmptyResult EmployeeCorrectiveActionHRApproval(string EmpId)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _ICorrectiveManager.SaveCorrectiveActionNotificationForHrApproval(EmpId, ObjLoginModel.UserName);
            }
            catch (Exception Ex)
            {

            }

            return new EmptyResult();
        }

        public EmptyResult EmployeeCorrectiveActionHrDenial(WitnessModel obj, string EmpId)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _ICorrectiveManager.CorrectiveActionHrDenyReasonandComment(obj, EmpId);
                _ICorrectiveManager.SaveCorrectiveActionNotificationForHrDenial(obj, EmpId, ObjLoginModel.UserName);
            }
            catch (Exception Ex)
            {

            }

            return new EmptyResult();
        }

        public JsonResult CorrectiveActiondetailsForReview(string EmpId)
        {

            try
            {
                if (EmpId != null)
                {

                    var ReviewList = _ICorrectiveManager.CorrectiveActionReviewEmployeeDetails(EmpId);



                    return Json(ReviewList);
                }
            }
            catch (Exception ex)
            {

            }
            return Json("");
        }

        public JsonResult GetCorrectiveActionIsActiveA(long UserId)
        {
            var objCorrectiveActionModel = new CorrectiveActionModel();
            try
            {
                if (UserId != null)
                {

                    objCorrectiveActionModel = _ICorrectiveManager.CorrectiveActionFormDetailsEmployee(UserId);

                    return Json(objCorrectiveActionModel);
                }
            }
            catch (Exception ex)
            {

            }
            return Json("");
        }

        //Corrective Action Meeting Date&Time with Dane T  Grey 05/20/2020 8:30 AM, 05/14/2020 7:30 AM, 05/21/2020 9:30 AM
        public ActionResult TerminationDenyAcknowledge(string EmpId)
        {
            
            try
            {
                var TEmpName = _ICorrectiveManager.EmployeeDetailsForCorrectiveActionForm(EmpId);
                return View("~/Views/CorrectiveAction/CorrectiveActionAcknowledgement.cshtml", TEmpName.emp_name);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult EmployeeAcceptCommentAfterMeeting(long UserId)
        {
            var objCorrectiveModel = new CorrectiveActionModel();
            try
            {
               bool IsActive = _ICorrectiveManager.SetIsActiveBCorrective(UserId);
               if (IsActive)
               {
                  objCorrectiveModel = _ICorrectiveManager.CorrectiveActionFormDetailsEmployee(UserId);
                  return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
               }
                else
                {
                   return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
                }
                
            }
            catch(Exception Ex)
            {

            }
            return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
        }

        /// <summary>
        /// Saving Notification after clicking end meeting
        /// Created by Rajat Toppo
        /// Date: 03-05-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult CorrectiveActionMeeting(CorrectiveActionModel obj)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
               bool NotificationSaved = _ICorrectiveManager.SaveNotificationforMeetingByManagerToEmployee(obj,ObjLoginModel.UserName);
            }
            catch (Exception Ex)
            {

            }
            return View("");
        }

        public ActionResult EmployeeDisputeCommentAfterMeeting(long UserId)
        {
            var objCorrectiveModel = new CorrectiveActionModel();
            try
            {
                bool IsActive = _ICorrectiveManager.SetIsActiveCCorrective(UserId);
                if (IsActive)
                {
                    objCorrectiveModel = _ICorrectiveManager.CorrectiveActionFormDetailsEmployee(UserId);
                    return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
                }
                else
                {
                    return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
                }

            }
            catch (Exception Ex)
            {

            }
            return View("~/Views/CorrectiveAction/CorrectiveActionForm.cshtml", objCorrectiveModel);
        }

        public EmptyResult SaveCorrectiveActionEmployeeCommentPDF(CorrectiveActionModel obj)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                
                if(obj != null)
                {
                    var _FillableFormRepository = new FillableFormRepository();
                    #region PDF
                    string viewName1 = "~/Views/CorrectiveAction/CorrectiveActionForm.cshtml";
                    string path1 = obj.emp_id + "CorrectiveActionForm";
                    var getDetails1 = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.ComplaintFile)).FirstOrDefault();
                    var getpdf1 = HtmlConvertToPdf(viewName1, obj, path1, getDetails1.FLT_Id, obj.emp_id);
                    #endregion PDF
                }
            }
            catch (Exception Ex)
            {

            }

            return new EmptyResult();
        }

        /// <summary>
        /// Sending notification to hr after dispute
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ActionResult CorrectiveActionDispute(CorrectiveActionModel obj)
        {
            var ObjLoginModel = new eTracLoginModel();
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _ICorrectiveManager.NotificationToHRForEmployeeCorrectiveActionDispute(obj, ObjLoginModel.UserName);

                if (obj != null)
                {
                    var _FillableFormRepository = new FillableFormRepository();
                    #region PDF
                    string viewName1 = "~/Views/CorrectiveAction/CorrectiveActionForm.cshtml";
                    string path1 = obj.emp_id + "CorrectiveActionForm";
                    var getDetails1 = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.ComplaintFile)).FirstOrDefault();
                    var getpdf1 = HtmlConvertToPdf(viewName1, obj, path1, getDetails1.FLT_Id, obj.emp_id);
                    #endregion PDF
                }
            }
            catch (Exception Ex)
            {

            }
            return View("");
        }

        


        public async Task<bool> HtmlConvertToPdf(string viewName, object model, string path, long FileId, string EmployeeId)
        {
            bool status = false;
            try
            {
                var pdf = new Rotativa.ViewAsPdf(viewName, model)
                {
                    FileName = path,
                    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = pdf.BuildFile(ControllerContext);
                var root = Server.MapPath("~/FilesRGY/");
                var fullPath = Path.Combine(root, pdf.FileName);
                fullPath = Path.GetFullPath(fullPath);
                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(pdfData, 0, pdfData.Length);
                }
                if (path != null)
                {
                    var Obj = new UploadedFiles();
                    Obj.FileName = path;
                    Obj.FileId = FileId;
                    Obj.FileEmployeeId = EmployeeId;
                    string LoginEmployeeId = EmployeeId;
                    Obj.AttachedFileName = path;
                    var IsSaved = _IFillableFormManager.SaveFile(Obj, LoginEmployeeId);
                }
                return status = true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




    }
}