using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class PDFFormController : Controller
    {
        private string DocPath = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PDFForm"], CultureInfo.InvariantCulture); //ConfigurationManager.AppSettings["PDFFormDoc"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        // GET: AdminSection/PDFForm
        private readonly IPDFFormManager _IPDFFormManager;
        public PDFFormController(IPDFFormManager _IPDFFormManager)
        {
            this._IPDFFormManager = _IPDFFormManager;
        }
        public ActionResult Index()
        {
            var obj = new PDFFormModel();
            ViewBag.AccountSection = true;
            ViewBag.ModuleList = _IPDFFormManager.GetModuleList();
            return View(obj);
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 19-Feb-2019
        /// Created For : To get all form details list.
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
        public JsonResult GetPDFFormDetailsList(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
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
                var pdfFormList = _IPDFFormManager.GetPDFFormList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var pdffrom in pdfFormList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(pdffrom.FormId), true);
                    row.cell = new string[3];
                    row.cell[0] = Convert.ToString(pdffrom.FormName);
                    row.cell[1] = Convert.ToString(pdffrom.IsActive);
                    row.cell[2] = pdffrom.FormPath == null ? "N/A":HostingPrefix + DocPath.Replace("~", "") + pdffrom.FormPath;
                    //DocPath + ProfileImagePath.Replace("~", "") + objeFleetDriverModel.DriverImage;
                    //row.cell[2] = Convert.ToString(pdffrom.FormPath);                   
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

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Feb-2019
        /// Created For : To save pdf form 
        /// </summary>
        /// <param name="objPDFFormModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePDFFormDetails(PDFFormModel objPDFFormModel)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                if (objPDFFormModel.FormPathFile != null)
                {
                    string FileName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objPDFFormModel.FormPathFile.FileName);
                    CommonHelper.StaticUploadImage(objPDFFormModel.FormPathFile, Server.MapPath(ConfigurationManager.AppSettings["PDFForm"]), FileName);
                    objPDFFormModel.FormPath = FileName;
                }
                var savedStatus = _IPDFFormManager.SavePDFForm(objPDFFormModel);
                ViewBag.ModuleList = _IPDFFormManager.GetModuleList();
                if (savedStatus == true)
                {
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    ModelState.Clear();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                }
                var obj = new PDFFormModel();
                return View("~/Areas/AdminSection/Views/PDFForm/Index.cshtml", obj);
                    //return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : -8-April-2019
        /// Created For : To get all pdf form data by using Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult FormDownload(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var _FormModel = _IPDFFormManager.GetePDFFormDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(_FormModel.FormPath))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + DocPath.Replace("~", "");
                        RootDirectory = RootDirectory + DocPath.Replace("~", "") + _FormModel.FormPath;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, _FormModel.FormPath).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FormModel.FormPath);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + DocPath.Replace("~", "") + "FileNotFound.png";
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
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansiod
        /// Created Date : 08-April-2019
        /// Created Fot : To Active and deactive form by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveForm(string Id, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                long _Id = 0;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }
                long UserId = ObjLoginModel.UserId;
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    long.TryParse(Id, out _Id);
                }
                result = _IPDFFormManager.ActiveFormById(_Id, UserId, IsActive);
                if (result == true)
                {
                    if (IsActive == "Y")
                    {
                        ViewBag.Message = "Activeted.";
                    }
                    else
                    {
                        ViewBag.Message = "Deactiveted.";
                    }

                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(ViewBag.Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating payment term.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
    }
}