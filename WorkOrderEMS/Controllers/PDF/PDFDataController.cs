using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class PDFDataController : Controller
    {
        // GET: PDFData
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string PDFForm = Convert.ToString(ConfigurationManager.AppSettings["PDFFormView"], CultureInfo.InvariantCulture);
        private string PDFPath = Convert.ToString(ConfigurationManager.AppSettings["PDFForm"], CultureInfo.InvariantCulture);
      
        private readonly IMainPDFFormManager _IMainPDFFormManager;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public PDFDataController(IMainPDFFormManager _IMainPDFFormManager)
        {
            this._IMainPDFFormManager = _IMainPDFFormManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-April-2019
        /// Created For : To save PDF form data to PDF form and save file name to database
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(CharitableContributionForm obj)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            long LocationId = 0;
            var objModel = new PDFSaveModel();
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId > 0)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
            }
            try
            {
                string pdfTemplate = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PDFPath"]), "Content/PDFForms/CharitableContributionRequestForm.pdf");
                string FileName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString("CharitableContributionRequestForm");
                obj.FileName = FileName;
                ViewBag.FileName = FileName;
                var newFile = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PDFForm"]), "PDFFormAfterFilling/" + FileName + ".pdf");
                //@"D:\Project\eTrac\WorkOrderEMS\Content\PDFForms\PDFFormAfterFilling\" + FileName + ".pdf";
                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(
                    newFile, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;
                objModel.UserId = ObjLoginModel.UserId;
                objModel.LocationId = ObjLoginModel.LocationID;
                objModel.FileName = FileName;
                var savePDF = _IMainPDFFormManager.SavePDFData(objModel);
                // set form pdfFormFields
                // The first worksheet and W-4 form
                //pdfFormFields.SetFieldProperty("names", "textsize", 4f, null);
                pdfFormFields.SetField("Services of America Inc accounting department accountseliteparkingofamericacom Please write", obj.NonProfitOrganization);
                pdfFormFields.SetField("Name of nonprofit organization", obj.MailingAddress);
                pdfFormFields.SetField("undefined", obj.ContactPerson);
                pdfFormFields.SetField("Contact person", obj.PhoneNumber);
                pdfFormFields.SetField("Phone number", obj.NameOfEvent);
                pdfFormFields.SetField("Name of event", obj.DateOfEvent);
                pdfFormFields.SetField("Date of event", obj.DateOfEvent);
                pdfFormFields.SetField("Federal Tax ID", obj.FederalTaxID);
                pdfFormFields.SetField("Contribution requested eg funds food services", obj.ContributionRequested);
                pdfFormFields.SetField("Estimated number of attendees at event or people receiving donation", obj.EstimatedNumberOfAttendees);
                pdfFormFields.SetField("malefemale age special interests  sports education child welfare etc 1", obj.DescribeTheDemographic);
                pdfFormFields.SetField("malefemale age special interests  sports education child welfare etc 2", obj.DescribeTheDemographic1);
                pdfFormFields.SetField("press releases interviews 1", obj.publicityIsPlanned);
                pdfFormFields.SetField("press releases interviews 2", obj.publicityIsPlanned1);
                pdfFormFields.SetField("What opportunities will be available to display the Elite Parking Services logo 1", obj.opportunitiesDescription);
                pdfFormFields.SetField("What opportunities will be available to display the Elite Parking Services logo 2", obj.opportunitiesDescription1);
                // report by reading values from completed PDF
                string sTmp = "Charitable Contribution Request Form Completed for " +
                    pdfFormFields.GetField("Services of America Inc accounting department accountseliteparkingofamericacom Please write") + " " +
                    pdfFormFields.GetField("Mailing address");
                //MessageBox.Show(sTmp, "Finished");
                // flatten the form to remove editting options, set it to false
                // to leave the form open to subsequent manual edits
                pdfStamper.FormFlattening = false;
                // close the pdf

                //foreach (System.Collections.DictionaryEntry de in pdfReader.AcroFields.Fields)
                //{
                //    pdfFormFields.SetFieldProperty(de.Key.ToString(),
                //             "setfflags",
                //              PdfFormField.FF_READ_ONLY,
                //              null);
                //}
                //pdfStamper.AcroFields.SetFieldProperty();
                //pdfFormFields.GetField(pdfDoc, true).FlattenFields();
                pdfStamper.Close();
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            var objData = new CharitableContributionForm();
            
            return View("PDFView");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-April-2019
        /// Created For : To view pdf form data 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public ActionResult GetPDF(string FileName)
        {
            var file = FileName.Replace(@";", "");
            /*D:\Project\eTrac\WorkOrderEMS\Content\PDFForms\PDFFormAfterFilling\*/
            //FileStream fs = new FileStream(Server.MapPath(HostingPrefix + (ConfigurationManager.AppSettings["PDFFormView"]).Replace("~", "") + FileName), FileMode.Open, FileAccess.Read);
            FileStream fs = new FileStream(Server.MapPath(@"~\Content\PDFForms\PDFFormAfterFilling\"+ file + ".pdf"), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult PDFList()
        {
            return View();
        }
        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date : 09-April-2019
        /// Created For : To get pdf form data.
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
        public JsonResult GetPDFFormList(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var pdfFormList = _IMainPDFFormManager.GetMainPDFFormList(UserId, rows, TotalRecords, sidx, sord);
                foreach (var pdffrom in pdfFormList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(pdffrom.FormId), true);
                    row.cell = new string[3];
                    row.cell[0] = Convert.ToString(pdffrom.FormName);
                    row.cell[1] = Convert.ToString(pdffrom.IsActive);
                    row.cell[2] =  HostingPrefix + PDFForm.Replace("~", "") + pdffrom.FormPath;
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
        /// Created Date : 09-April-2019
        /// Created For : To get all form name list 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPDFFormName()
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            long LocationId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId > 0)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
            }
            try
            {
                var result = _IMainPDFFormManager.GetPDFFormNameList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            
        }
    }
}