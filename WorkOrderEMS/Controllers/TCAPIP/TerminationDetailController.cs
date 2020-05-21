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
using WorkOrderEMS.Controllers;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;


namespace WorkOrderEMS.Controllers.TCAPIP
{

    public class TerminationDetaillsController : Controller
    {
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly ITerminationManager _ITermination;
        
        public TerminationDetaillsController(ITerminationManager _ITermination, IFillableFormManager _IFillableFormManager)
        {
            this._ITermination = _ITermination;
            this._IFillableFormManager = _IFillableFormManager;
        }

        /// <summary>
        /// Get Termination Employee Details 
        /// Created by: Rajat Toppo
        /// Date: 15-03-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public ActionResult TerminationForm(string EmpId)
        
        {
            //string EmpId = "HDHR1234";
            try
            {
                
                if (EmpId != null)
                {
                    var objTerminationModel = _ITermination.TerminationFormEmployeeDetails(EmpId);

                    //ViewBag.AssetsList = _IDepartment.GetAllAsstesList(EmpId);
                    // ViewBag.EMP_IsExempt = objTerminationModel.EMP_IsExempt;
                    //ViewBag.EMP_IsExempt = "Y";
                    if (objTerminationModel != null)
                    {
                        return View("~/Views/TerminationDetail/TerminationForm.cshtml", objTerminationModel);

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();

        }

        /// <summary>
        /// Create Pdf and send email for Termination Form
        /// created by: Rajat Toppo
        /// Date: 30-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveTerminationForm(TerminationModel Obj)
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
                if (Obj != null)
                {
                  var TerminationSaved = _ITermination.SaveTerminationFormDetails(Obj);

                    if (TerminationSaved) 
                    {
                        var _FillableFormRepository = new FillableFormRepository();
                        #region PDF
                        string viewName = "~/Views/TerminationDetail/TerminationForm.cshtml";
                        string path = Obj.EmpId + "TerminationForm";
                        var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.ComplaintFile)).FirstOrDefault();
                        var getpdf = HtmlConvertToPdf(viewName, Obj, path, getDetails.FLT_Id, Obj.EmpId);
                        #endregion PDF


                        //Email
                        _ITermination.SendTerminationFormEmail(Obj, ObjLoginModel.UserId);

                        //Email

                        if (Obj.IsSeverence == "Yes")
                        {


                            #region PDF
                            string viewName1 = "~/Views/TerminationDetail/Serverance.cshtml";
                            string path1 = Obj.EmpId + "Serverance";
                            var getDetails1 = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.ComplaintFile)).FirstOrDefault();
                            var getpdf1 = HtmlConvertToPdf(viewName1, Obj, path1, getDetails1.FLT_Id, Obj.EmpId);
                            #endregion PDF

                            _ITermination.SendServeranceEmail(Obj, ObjLoginModel.UserId);
                        }

                        return View("~/Views/TerminationDetail/TerminationForm.cshtml");
                    }
                    else
                    {
                        return View("~/Views/TerminationDetail/TerminationForm.cshtml");
                    }

                 return View("~/Views/TerminationDetail/TerminationForm.cshtml");

                }

            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }


        /// <summary>
        /// Create Pdf and send email for severance agreement
        /// created by: Rajat Toppo
        /// Date: 30-03-2020
        /// </summary>
        /// <param name="Obj1"></param>
        /// <returns></returns>
        public ActionResult SeveranceAgreement(TerminationModel Obj1)
        {

                eTracLoginModel ObjLoginModel = null;
           
               
                //string EmpId = "HDHR1234";
                try
                {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                if (Obj1 != null)
                {
                    var _FillableFormRepository = new FillableFormRepository();
                    #region PDF
                    string viewName = "~/Views/TerminationDetail/Serverance.cshtml";
                    string path = Obj1.EmpId + "Serverance";
                    var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.ComplaintFile)).FirstOrDefault();
                    var getpdf = HtmlConvertToPdf(viewName, Obj1, path, getDetails.FLT_Id, Obj1.EmpId);
                    #endregion PDF

                    
                        _ITermination.SendServeranceEmail(Obj1, ObjLoginModel.UserId);
                    

                    return View("~/Views/TerminationDetail/Serverance.cshtml");
                }
                }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        /// <summary>
        /// Get Employee Grid View
        /// created by: Rajat Toppo
        /// Date: 19-03-2020
        /// </summary>
        /// <returns></returns>
        public ActionResult TerminationList()
        
        { 
            return View();
        }

        /// <summary>
        /// Get Team employee List Grid
        /// created by: Rajat Toppo
        /// Date: 19-03-2020
        /// </summary>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmployeeTerminationList(DateTime? ToDate, DateTime? FromDate)
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
                
                    var getTList = _ITermination.TerminationlistTeamDetails(ObjLoginModel.UserName, ToDate, FromDate);
                    return Json(getTList, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return Json("");
        }
        /// <summary>
        /// Get Finalize employee List Grid
        /// created by: Rajat Toppo
        /// Date: 21-03-2020
        /// </summary>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmployeeTerminationFinalizeList(DateTime? ToDate, DateTime? FromDate)
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
               
                
                    var getTList1 = _ITermination.TerminationlistFinalizedetails(ObjLoginModel.UserId, ToDate, FromDate);
                    return Json(getTList1, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            return Json("");
        }

        /// <summary>
        /// Review Termination Details in Manager Grid
        /// Created by: Rajat Toppo
        /// Date: 19-04-2020
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTerminationDetailsForReview(string EmpId)
        {
            
            try
            {
                if (EmpId != null)
                {
                    
                    var ReviewList = _ITermination.TerminationEmployeeDetailsforReview(EmpId);
                    
                    

                   return Json(ReviewList);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return Json("");
        }

    

        /// <summary>
        /// Termination Deny Acknowledgement
        /// Created by: Rajat Toppo
        /// Date: 20-04-2020
        /// </summary>
        /// <returns></returns>
        public ActionResult TerminationDenyAcknowledge(string EmpId)
        {
            //var Emp_Name = "HDHR1234";
            try
            {
                var TEmpName = _ITermination.TerminationFormEmployeeDetails(EmpId);
                return View("~/Views/TerminationDetaills/TerminationAcknowledge.cshtml",TEmpName.Emp_Name);
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View();
        }

        /// <summary>
        /// Get Pending employee List Grid
        /// created by: Rajat Toppo
        /// Date: 24-03-2020
        /// </summary>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult EmployeeTerminationPendingList(DateTime? ToDate, DateTime? FromDate)
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

                    var getTTerminationList = _ITermination.TerminationlistPendingdetails(ObjLoginModel.UserId, ToDate, FromDate);
                    
                    return Json(getTTerminationList, JsonRequestBehavior.AllowGet);
               

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
            }
            return Json("");
        }
        /// <summary>
        /// Saving Termination Employee Witness Details.
        /// Created by: Rajat Toppo
        /// Date: 27-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpPost]
        public EmptyResult TerminationEmployeeWitnessDetails(WitnessModel Obj,string EmpId)
        {
            try
            {
                if(Obj != null)
                {
                    _ITermination.SaveTerminationWitnessDetails(Obj,EmpId);
                    return new EmptyResult();
                    
                }

            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Created by: Rajat Toppo
        /// Date: 15-04-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public ActionResult EmployeeAssetList(String EmpId)
        {
            try
            {
                if (EmpId != null)
                {
                    var ObjAssetEmployeeModel = _ITermination.TerminationEmployeeDetails(EmpId);
                    var getAssetStatusList = _ITermination.TerminationEmployeeAssetDetails(EmpId);
                    ObjAssetEmployeeModel.EmployeeAssetDetails = getAssetStatusList;

                    return View("~/Views/TerminationDetaills/AssetstatusList.cshtml", ObjAssetEmployeeModel);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        /// <summary>
        /// Get Termination Employee Asset data
        /// Created by: Rajat Toppo
        /// Date: 30-03-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public ActionResult EmployeeAssetAllocation(string EmpId)
        {
            try
            {
                if(EmpId != null)
                {
                    var ObjAssetModel = _ITermination.TerminationEmployeeDetails(EmpId);
                    var getAssetList = _ITermination.TerminationEmployeeAssetDetails(EmpId);
                    ObjAssetModel.EmployeeAssetDetails = getAssetList;

                    return View("~/Views/TerminationDetaills/AssetList.cshtml", ObjAssetModel);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View();
        }

        /// <summary>
        /// Saving Notification For HR Termination Approval
        /// created by: Rajat Toppo
        /// Date: 22-04-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public EmptyResult HrTerminationApprove(string EmpId)
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
                _ITermination.SaveNotificationForHrApproval(EmpId, ObjLoginModel.UserName);
            }
            catch (Exception Ex)
            {

            }

            return new EmptyResult();
        }


        /// <summary>
        /// Saving Notification For HR Termination Denial
        /// Created by: Rajat Toppo
        /// Date: 16-04-2020
        /// </summary>
        /// <returns></returns>
        public EmptyResult HrDenialReason(WitnessModel obj,string EmpId)
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
                _ITermination.SaveHrDenyReasonandComment(obj, EmpId);
                _ITermination.SaveNotificationForHrDenial(obj, EmpId, ObjLoginModel.UserName);
            }
            catch (Exception Ex)
            {

            }
            
            return new EmptyResult();
        }

        /// <summary>
        /// Set Asset Status and Return Date
        /// Created by Rajat Toppo
        /// Date: 02-04-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AssetReturnDetails(List<EmployeeAssetDetails> obj,string EmpId)
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
                bool isTrue = _ITermination.SaveAssetStatusReturnDate(obj, EmpId, ObjLoginModel.UserName);


                if (isTrue == true)
                {
                    var GetEmployeeDetailsEmail = _ITermination.TerminationEmployeeDetails(EmpId);
                    var GetAsseTListEmail = _ITermination.TerminationEmployeeAssetDetails(EmpId);
                    //_ITermination.SendAssetDetailsEmail(GetEmployeeDetailsEmail, GetAsseTListEmail);


                }
                return View();
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
            }
            return View();
        }

        public JsonResult TerminationFormAllDetails(string EMPId)
        {
            var objTerminationForm = new TerminationModel();
            try
            {
                if(EMPId != null)
                {
                    objTerminationForm = _ITermination.EmployeeTerminationFormAlldetails(EMPId);
                    return Json(objTerminationForm);
                }
            }
            catch(Exception ex)
            {
                
            }
            return Json("");
        }
        /// <summary>
        /// Html to PDF Covert
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="FileId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [NonAction]
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