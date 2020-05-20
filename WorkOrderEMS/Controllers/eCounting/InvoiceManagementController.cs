using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data.Classes;
using System.Data;
using WorkOrderEMS.BusinessLogic.Exception_B;
using System.Xml;
using WorkOrderEMS.Service;
using System.Xml.Serialization;
using Newtonsoft.Json;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI;
using static WorkOrderEMS.Controllers.eMaintenanceDisclaimer.eMaintenanceDisclaimerController;

namespace WorkOrderEMS.Controllers
{
    public class InvoiceManagementController : Controller
    {
        private readonly ICustomerManagement _ICustomerManagement;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IVendorManagement _IVendorManagement;
        private readonly IBillDataManager _IBillDataManager;
        private string DocPath = ConfigurationManager.AppSettings["LicenseAndInsuranceDocument"];
        private string DocAccountPath = ConfigurationManager.AppSettings["BankAccountDocsDocument"];
        private string DocFacilityPath = ConfigurationManager.AppSettings["VendorImageFacility"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private bool IsPageRefresh = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPageRefresh"] ?? "false");

        workorderEMSEntities _workorderems = new workorderEMSEntities();
        ARService ARS = new ARService();
        DBUtilities DBUtilities = new DBUtilities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InvoiceDashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ClientInvoiceList()
        {
            try
            {
                //var id = Cryptography.GetDecryptedData("JPXO3WbEfW2wZFBq+BB7dA==", true);
                eTracLoginModel ObjLoginModel = null;
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
                ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                //ViewBag.AdministratorList = null;
                //ViewBag.IsPageRefresh = IsPageRefresh;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }


        public InvoiceManagementController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICustomerManagement _ICustomerManagement, IBillDataManager _IBillDataManager)
        {

            this._ICustomerManagement = _ICustomerManagement;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            //this._IVendorManagement = _IVendorManagement;
            this._IBillDataManager = _IBillDataManager;

        }



        public ActionResult CreateClientInvoice(string id = "", string IsDraft = "")
        {
            var objmodel = new ClientInvoiceDataModel();
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                long CountryId = 1;
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                ViewBag.OffScheduleInvoiceReason = _ICommonMethod.GetGlobalCodeData("OffScheduleInvoiceReason");
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.UpdateMode = false;
                    var objItem = new InvoiceItemDetails()
                    {
                        SrNo = 1
                    };
                    objmodel.ListInvoiceItemDetails = new List<InvoiceItemDetails>();
                    objmodel.ListInvoiceItemDetails.Add(objItem);
                }
                else
                {
                    ViewBag.UpdateMode = true;
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    objmodel = ARS.GetInvoiceData(id, IsDraft);
                    objmodel.ListInvoiceItemDetails = ARS.GetInvoiceItemsList(id, IsDraft);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            return View(objmodel);
        }

        public ActionResult AddInvoiceItemList(int id)
        {
            InvoiceItemDetails objCVD = new InvoiceItemDetails
            {
                SrNo = id
            };
            return PartialView("_InvoiceItemDetails", objCVD);
        }

        [HttpPost]
        public JsonResult CreateClientInvoiceSubmit(ClientInvoiceDataModel ObjInv, List<InvoiceItemDetails> InvoiceItemDetailsList)
        {
            bool booStatus = false;
            dynamic Result = "";
            HttpFileCollectionBase files = Request.Files;
            var Message = "";
            string Action = "";
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            ObjInv.UserId = objLoginSession.UserId;
            try
            {
                if (files[0].ContentLength > 0)
                {
                    string AttachmentName = ObjInv.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(files[0].FileName);
                    CommonHelper.StaticUploadImage(files[0], Server.MapPath(ConfigurationManager.AppSettings["InvoiceDocuments"]), AttachmentName);
                    ObjInv.InvoiceDocument = AttachmentName;
                }
                if (ObjInv.Id == 0)
                {
                    Action = "I";
                    string xmlInvoiceDetails = GetXmlString(ObjInv);
                    string xmlItemDetails = GetXmlString(InvoiceItemDetailsList);

                    DataTable DtResult = ARS.InsertUpdateInvoiceBill(Action, xmlInvoiceDetails, xmlItemDetails, ObjInv.UserId, 0, ObjInv.InvoiceStatus == "0" ? "Y" : "N");
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                        if (booStatus)
                        {
                            UploadInvoiceToQuickBook(ObjInv, InvoiceItemDetailsList);
                        }
                    }
                }
                else
                {
                    var context = new workorderEMSEntities();
                    Action = "U";
                    string xmlInvoiceDetails = GetXmlString(ObjInv);
                    string xmlItemDetails = GetXmlString(InvoiceItemDetailsList);

                    DataTable DtResult = ARS.InsertUpdateInvoiceBill((!String.IsNullOrEmpty(ObjInv.DraftNumber) && ObjInv.InvoiceStatus == "1") ? "I" : "U", xmlInvoiceDetails, xmlItemDetails, ObjInv.UserId, 0, ObjInv.InvoiceStatus == "0" ? "Y" : "N");
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                    }
                }
                return new JsonResult()
                {
                    Data = new
                    {
                        Status = booStatus,
                        Messsage = Message
                    }
                };
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public ActionResult CreateClientInvoiceSubmit(ClientInvoiceDataModel ObjInv, List<InvoiceItemDetails> InvoiceItemDetailsList)", "Exception While Saving Client Invoice.", ObjInv);
                return new JsonResult()
                {
                    Data = new
                    {
                        Status = 0,
                        Messsage = ex.Message
                    }
                };
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            //return RedirectToAction("ClientInvoiceList");
            //return View("ClientInvoiceList");
        }

        [HttpPost]
        public JsonResult GetInvoiceDataToView(string Id, bool IsDraft)
        {
            var getData = new ClientInvoiceDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long InvoiceId = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    //if (LocationId == null)
                    //{
                    //    LocationId = ObjLoginModel.LocationID;
                    //}
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    long.TryParse(Id, out InvoiceId);
                }
                string Url = ConfigurationManager.AppSettings["InvoiceDocuments"];
                DataSet DS = (IsDraft == true) ? ARS.GetDraftDataforView(InvoiceId) : ARS.GetInvoiceDataforView(InvoiceId);
                ClientInvoiceDataModel CID = DataRowToObject.CreateListFromTable<ClientInvoiceDataModel>(DS.Tables[0]).Select(x => new ClientInvoiceDataModel()
                {
                    ClientCompanyName = x.ClientCompanyName,
                    ClientLocationName = x.ClientLocationName,
                    InvoicePaymentTermsDesc = x.InvoicePaymentTermsDesc,
                    ClientPointOfContactName = x.ClientPointOfContactName,
                    PositionTitle = x.PositionTitle,
                    InvoiceDueDateDisplay = x.InvoiceDueDate.ToString("dd MMM yyyy"),
                    InvoiceNumber = x.InvoiceNumber,
                    LocationCode = x.LocationCode,
                    LocationAddress = x.LocationAddress,
                    ReasonForOffScheduleInvoice = x.ReasonForOffScheduleInvoice,
                    ReasonForOffScheduleInvoiceDesc = x.ReasonForOffScheduleInvoiceDesc,
                    InvoiceDateDisplay = x.InvoiceDate.ToString("dd MMM yyyy"),
                    ContractTypeDesc = x.ContractTypeDesc,
                    EntryOn = x.ModifiedOn,
                    ModifiedOn = x.ModifiedOn,
                    EntryByDisplay = x.EntryByDisplay,
                    ModifiedByDisplay = x.ModifiedByDisplay,
                    EntryOnDisplay = x.EntryOnDisplay,
                    ModifiedOnDisplay = x.ModifiedOnDisplay,
                    InvoiceTypeDesc = x.InvoiceTypeDesc,
                    TaxAmount = x.TaxAmount,
                    GrandTotal = x.GrandTotal,
                    InvoiceDocument = x.InvoiceDocument,
                    InvoiceDocumentUrl = Server.MapPath(Url) + x.InvoiceDocumentUrl,
                    SubmissionOn = x.SubmissionOn,
                    InvoiceLastSenttoclientDate = x.InvoiceLastSenttoclientDate,
                    PendingAmount = x.PendingAmount,
                    Id = x.Id,
                    InvoiceStatus = x.InvoiceStatus,
                    DraftNumber = x.DraftNumber,
                    PaymentTotal=x.PaymentTotal,
                    Comment = x.Comment,
                    PaymentReceiveDateDisplay= x.PaymentReceiveDateDisplay
                }).FirstOrDefault();
                CID.ListInvoiceItemDetails = DataRowToObject.CreateListFromTable<InvoiceItemDetails>(DS.Tables[1]);
                getData = CID;

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllInvoiceList(string _search, long? LocationId, string flagApproved = null, string UserType = null, string InvoiceStatus = null, string InvoiceType = null, long? ClientLocationCode = null, string StartDate = null, string EndDate = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            //if (flagApproved != "Y")
                //LocationId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                //if (flagApproved == "Y")
                LocationId = ObjLoginModel.LocationID;
            }
            if (ClientLocationCode == null)
            {
                ClientLocationCode = LocationId;
            }
            try
            {
                var AllInvoiceList = new List<ClientInvoiceDataModel>();
                AllInvoiceList = ARS.Get_InvoiceList(LocationId, string.IsNullOrEmpty(_search) == true ? "" : _search).Select(a => new ClientInvoiceDataModel()
                {
                    //Id = Cryptography.GetEncryptedData(Convert.ToString(a.Id), true),
                    Id = a.Id,
                    InvoiceDate = a.InvoiceDate,
                    InvoiceDateDisplay = a.InvoiceDateDisplay,
                    GrandTotal = a.GrandTotal,
                    PaymentTotal = a.PaymentTotal,
                    InvoiceStatus = a.InvoiceStatus,
                    InvoiceStatusDesc = a.InvoiceStatusDesc,
                    InvoiceType = a.InvoiceType,
                    InvoiceTypeDesc = a.InvoiceTypeDesc,
                    ClientLocationCode = a.ClientLocationCode,
                    PaymentReminder = (a.InvoiceDueDate < DateTime.Now && (a.InvoiceStatus != "4" && a.InvoiceStatus != "5" && a.InvoiceStatus != "0")) ? true : false,
                    IsDraft = a.IsDraft,
                    InvoiceNumber = a.InvoiceNumber,
                    EntryOn = a.EntryOn,
                    IsCreditIssued = a.IsCreditIssued,
                }).OrderByDescending(x => x.EntryOn).ToList();
                //if (LocationId > 0)
                //{
                //    AllInvoiceList = AllInvoiceList.Where(x => x.InvoiceStatus == "Y").ToList();
                //}
                //else
                //{
                //    AllInvoiceList = AllInvoiceList.Where(x => x.InvoiceStatus != "Y").ToList();     
                //}

                if (InvoiceStatus != null && InvoiceStatus != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.InvoiceStatus == InvoiceStatus).ToList();
                }
                if (InvoiceType != null && InvoiceType != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.InvoiceType == Convert.ToInt32(InvoiceType)).ToList();
                }
                if (ClientLocationCode > 0 && ClientLocationCode != null)
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.ClientLocationCode == ClientLocationCode).ToList();
                }
                if (StartDate != null && StartDate != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.InvoiceDate >= Convert.ToDateTime(StartDate) && c.InvoiceDate <= Convert.ToDateTime(EndDate)).ToList();
                }

                return Json(AllInvoiceList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDashboardForInvoiceStatusCount(long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                LocationId = ObjLoginModel.LocationID;
            }
            try
            {
                
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    var DS = ARS.GetInvoiceCountByStatus(LocationId).FirstOrDefault();
                    var model = new InvoiceCountForGraph();
                    //return JsonConvert.SerializeObject(DS.Tables[0]);
                    if (DS != null)
                    {
                        model.Pending_Approval = DS.Pending_Approval;
                        model.Pending_Submission = DS.Pending_Submission;
                        model.Pending_Payment = DS.Pending_Payment;
                        model.Paid = DS.Paid;
                        model.Cancelled = DS.Cancelled;
                        model.Total = DS.Total;
                        model.Draft = DS.Draft;
                    }
                    return Json(new { model }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return Json(new { ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDashboardForCreditInvoiceStatusCount(long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                LocationId = ObjLoginModel.LocationID;
            }
            try
            {
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    var DS = ARS.GetCreditInvoiceCountByStatus(LocationId).FirstOrDefault();
                    var model = new CreditInvoiceCountForGraph();
                    //return JsonConvert.SerializeObject(DS.Tables[0]);
                    if (DS != null)
                    {
                        model.Pending_Approval = DS.Pending_Approval;
                        model.Pending_Invoice_Credit = DS.Pending_Invoice_Credit;
                        model.Credit_Applied = DS.Credit_Applied;
                        model.Pending_Refund_Payment = DS.Pending_Refund_Payment;
                        model.Refund_Paid = DS.Refund_Paid;
                        model.Cancelled = DS.Cancelled;
                        model.Total = DS.Total;
                        model.Draft = DS.Draft;
                    }
                    return Json(new { model }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(null, JsonRequestBehavior.AllowGet); }
            }
            catch (Exception ex)
            {
                return Json(new { ex.InnerException }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SubmissionInvoice(ApproveDenyClientInvoiceModel objADCIM)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long _InvoiceId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        //objADCIM.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objADCIM.InvoiceId))
                {
                    long.TryParse(objADCIM.InvoiceId, out _InvoiceId);
                }
                objADCIM.UserId = ObjLoginModel.UserId;
                objADCIM.Id = _InvoiceId;
                if (objADCIM.Id > 0)
                {
                    bool isSaved = false;
                    result = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();

                    if (objADCIM.Id > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objADCIM.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (userData != null)
                        {
                            DataTable DtResult = ARS.SubmissionClientInvoice(objADCIM.Id, objADCIM.Comment, objADCIM.UserId);

                            if (DtResult != null && DtResult.Rows.Count > 0)
                            {
                                var booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                                var Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                                if (booStatus)
                                {
                                    isSaved = true;
                                    result = "Invoice Submitted.";
                                }
                                else
                                {
                                    isSaved = false;
                                    result = "Something went wrong.";
                                }
                            }
                        }
                        else { isSaved = false; }
                        if (isSaved == true)
                        {
                            #region Email
                            var objEmailLogRepository = new WorkOrderEMS.Data.DataRepository.EmailLogRepository();
                            var objEmailReturn = new List<EmailToManagerModel>();
                            var objListEmailog = new List<EmailLog>();
                            var objTemplateModel = new TemplateModel();
                            if (isSaved == true)
                            {
                                ARService ARS = new ARService();
                                DataSet DtDetails = ARS.GetSubmissionMailDetails(objADCIM.Id);
                                //var vendorDetail = _workorderems.spGetVendorAllDetail(objCustomerApproveRejectModel.Customer).FirstOrDefault();
                                var locationDetails = _workorderems.LocationMasters.Where(x => x.LocationId == objADCIM.ClientLocationCode && x.IsDeleted == false).FirstOrDefault();
                                var ClientName = _workorderems.Companies.Where(x => x.CMP_Id == locationDetails.ContractHolder).FirstOrDefault();
                                var userEmail = _workorderems.CompanyDetails.Where(x => x.COD_CMP_Id == locationDetails.ContractHolder).FirstOrDefault();
                                //if (vendorDetail != null)
                                //{
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.DtInvoiceDetails = DtDetails;
                                objEmailHelper.ClientName = "Jitesh Italiya";//ClientName.CMP_NameDBA;
                                objEmailHelper.emailid = "jiteshitaliya@gmail.com";//userEmail.COD_Email; //vendorDetail.COD_Email;
                                objEmailHelper.UserName = userData.FirstName + ' ' + userData.FirstName; //vendorDetail.CMP_NameLegal;
                                objEmailHelper.LocationName = locationDetails.LocationName;
                                objEmailHelper.ClientUserId = 0; //objCustomerApproveRejectModel.Customer.ToString();
                                objEmailHelper.ApproveRemoveStatus = ""; //ApproveRemoveSatus;
                                objEmailHelper.MailType = "InvoiceSubmissionTemplate";
                                objEmailHelper.SentBy = objADCIM.UserId;
                                objEmailHelper.LocationID = objADCIM.LocationId;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                //Push Notification     
                                if (IsSent == true)
                                {
                                    var objEmailog = new EmailLog();
                                    try
                                    {
                                        objEmailog.CreatedBy = objADCIM.UserId;
                                        objEmailog.CreatedDate = DateTime.UtcNow;
                                        objEmailog.DeletedBy = null;
                                        objEmailog.DeletedOn = null;
                                        objEmailog.LocationId = objADCIM.LocationId;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = objADCIM.LocationId;
                                        objEmailog.SentEmail = "jiteshitaliya@gmail.com";//userEmail.COD_Email; //vendorDetail.COD_Email;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = 0; //objCustomerApproveRejectModel.Customer;
                                        objListEmailog.Add(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                                using (var context = new workorderEMSEntities())
                                {
                                    context.EmailLogs.AddRange(objListEmailog);
                                    context.SaveChanges();
                                }

                                //}
                                #region Save DAR
                                //objDAR.ActivityDetails = DarMessage.VendorApprovedCancel(vendorDetail.CMP_NameLegal, locationDetails.LocationName, ApproveRemoveSatus);
                                //objDAR.TaskType = (long)TaskTypeCategory.PaymentApporveCancel;
                                //objDAR.UserId = objCustomerApproveRejectModel.UserId;
                                //objDAR.CreatedBy = objCustomerApproveRejectModel.UserId;
                                //objDAR.LocationId = objCustomerApproveRejectModel.LocationId;
                                //objDAR.CreatedOn = DateTime.UtcNow;
                                //CommonManager.SaveDAR(objDAR);
                                #endregion DAR
                            }
                            else
                            {
                                result = CommonMessage.FailureMessage();
                            }
                            #endregion Email
                        }
                    }

                    //-----------------------------------------------------------------------
                }
                else
                {
                    result = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public JsonResult SubmissionInvoice(ApproveDenyClientInvoiceModel objADCIM)", "Exception While Submission Client Invoice.", null);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region "Payment Reminder"
        [HttpPost]
        public JsonResult PaymentRemainder(ApproveDenyClientInvoiceModel objADCIM)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long _InvoiceId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        //objADCIM.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objADCIM.InvoiceId))
                {
                    long.TryParse(objADCIM.InvoiceId, out _InvoiceId);
                }
                objADCIM.UserId = ObjLoginModel.UserId;
                objADCIM.Id = _InvoiceId;
                if (objADCIM.Id > 0)
                {
                    bool isSaved = false;
                    result = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();

                    if (objADCIM.Id > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objADCIM.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();

                        #region Email
                        var objEmailLogRepository = new WorkOrderEMS.Data.DataRepository.EmailLogRepository();
                            var objEmailReturn = new List<EmailToManagerModel>();
                            var objListEmailog = new List<EmailLog>();
                            var objTemplateModel = new TemplateModel();

                                ARService ARS = new ARService();
                                DataSet DtDetails = ARS.GetSubmissionMailDetails(objADCIM.Id);
                                var locationDetails = _workorderems.LocationMasters.Where(x => x.LocationId == objADCIM.ClientLocationCode && x.IsDeleted == false).FirstOrDefault();
                                var ClientName = _workorderems.Companies.Where(x => x.CMP_Id == locationDetails.ContractHolder).FirstOrDefault();
                                var userEmail = _workorderems.CompanyDetails.Where(x => x.COD_CMP_Id == locationDetails.ContractHolder).FirstOrDefault();


                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.DtInvoiceDetails = DtDetails;
                                objEmailHelper.ClientName = "Jitesh Italiya";//ClientName.CMP_NameDBA;
                                objEmailHelper.emailid = "jiteshitaliya@gmail.com";//userEmail.COD_Email; //vendorDetail.COD_Email;
                                objEmailHelper.UserName = userData.FirstName + ' ' + userData.LastName; //vendorDetail.CMP_NameLegal;
                                objEmailHelper.LocationName = locationDetails.LocationName;
                                objEmailHelper.ClientUserId = 0; //objCustomerApproveRejectModel.Customer.ToString();
                                objEmailHelper.ApproveRemoveStatus = ""; //ApproveRemoveSatus;
                                objEmailHelper.MailType = "InvoicePaymentReminderTemplate";
                                objEmailHelper.SentBy = objADCIM.UserId;
                                objEmailHelper.LocationID = objADCIM.LocationId;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                //Push Notification     
                                if (IsSent == true)
                                {
                                    var objEmailog = new EmailLog();
                                    try
                                    {
                                        objEmailog.CreatedBy = objADCIM.UserId;
                                        objEmailog.CreatedDate = DateTime.UtcNow;
                                        objEmailog.DeletedBy = null;
                                        objEmailog.DeletedOn = null;
                                        objEmailog.LocationId = objADCIM.LocationId;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = objADCIM.LocationId;
                                        objEmailog.SentEmail = "jiteshitaliya@gmail.com";//userEmail.COD_Email;  //vendorDetail.COD_Email;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = 0; //objCustomerApproveRejectModel.Customer;
                                        objListEmailog.Add(objEmailog);
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                }
                                using (var context = new workorderEMSEntities())
                                {
                                    context.EmailLogs.AddRange(objListEmailog);
                                    context.SaveChanges();
                                }
                            #endregion Email
                    }
                }
                else
                {
                    result = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        public JsonResult ApproveInvoice(ApproveDenyClientInvoiceModel objADCIM)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long _InvoiceId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        //objADCIM.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objADCIM.InvoiceId))
                {
                    long.TryParse(objADCIM.InvoiceId, out _InvoiceId);
                }
                objADCIM.UserId = ObjLoginModel.UserId;
                objADCIM.Id = _InvoiceId;
                if (objADCIM.Id > 0)
                {
                    bool isSaved = false;
                    result = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();
                    string Status = "";

                    if (objADCIM.Id > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objADCIM.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();

                        if (userData != null)
                        {
                            if (objADCIM.Action == "A")
                            {
                                result = "Invoice Approved.";
                                Status = "A";
                            }
                            else
                            {
                                result = "Invoice Rejected.";
                                Status = "R";
                            }

                            DataTable DtResult = ARS.ApproveDenyClientInvoice(objADCIM.Id, objADCIM.Comment, Status, objADCIM.UserId);

                            if (DtResult != null && DtResult.Rows.Count > 0)
                            {
                                var booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                                var Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                                if (booStatus)
                                {
                                    isSaved = true;
                                }
                                else
                                {
                                    isSaved = false;
                                }
                            }
                        }
                        else { isSaved = false; }
                        if (isSaved == true)
                        {
                            #region Email
                            //var objEmailLogRepository = new EmailLogRepository();
                            //var objEmailReturn = new List<EmailToManagerModel>();
                            //var objListEmailog = new List<EmailLog>();
                            //var objTemplateModel = new TemplateModel();
                            //if (isSaved == true)
                            //{
                            //    var vendorDetail = _workorderems.spGetVendorAllDetail(objCustomerApproveRejectModel.Customer).FirstOrDefault();
                            //    var locationDetails = _workorderems.LocationMasters.Where(x => x.LocationId == objCustomerApproveRejectModel.LocationId
                            //                                                         && x.IsDeleted == false).FirstOrDefault();
                            //    if (vendorDetail != null)
                            //    {
                            //        bool IsSent = false;
                            //        var objEmailHelper = new EmailHelper();
                            //        objEmailHelper.emailid = vendorDetail.COD_Email;
                            //        objEmailHelper.VendorName = vendorDetail.CMP_NameLegal;
                            //        objEmailHelper.LocationName = locationDetails.LocationName;
                            //        objEmailHelper.VendorId = objCustomerApproveRejectModel.Customer.ToString();
                            //        objEmailHelper.ApproveRemoveStatus = ApproveRemoveSatus;
                            //        objEmailHelper.MailType = "VENDORAPPROVEDREJECT";
                            //        objEmailHelper.SentBy = objCustomerApproveRejectModel.UserId;
                            //        objEmailHelper.LocationID = objCustomerApproveRejectModel.LocationId;
                            //        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            //        IsSent = objEmailHelper.SendEmailWithTemplate();

                            //        //Push Notification     
                            //        if (IsSent == true)
                            //        {
                            //            var objEmailog = new EmailLog();
                            //            try
                            //            {
                            //                objEmailog.CreatedBy = objCustomerApproveRejectModel.UserId;
                            //                objEmailog.CreatedDate = DateTime.UtcNow;
                            //                objEmailog.DeletedBy = null;
                            //                objEmailog.DeletedOn = null;
                            //                objEmailog.LocationId = objCustomerApproveRejectModel.LocationId;
                            //                objEmailog.ModifiedBy = null;
                            //                objEmailog.ModifiedOn = null;
                            //                objEmailog.SentBy = objCustomerApproveRejectModel.LocationId;
                            //                objEmailog.SentEmail = vendorDetail.COD_Email;
                            //                objEmailog.Subject = objEmailHelper.Subject;
                            //                objEmailog.SentTo = objCustomerApproveRejectModel.Customer;
                            //                objListEmailog.Add(objEmailog);
                            //            }
                            //            catch (Exception)
                            //            {
                            //                throw;
                            //            }
                            //        }
                            //        using (var context = new workorderEMSEntities())
                            //        {
                            //            context.EmailLogs.AddRange(objListEmailog);
                            //            context.SaveChanges();
                            //        }

                            //    }
                            //    #region Save DAR
                            //    objDAR.ActivityDetails = DarMessage.VendorApprovedCancel(vendorDetail.CMP_NameLegal, locationDetails.LocationName, ApproveRemoveSatus);
                            //    objDAR.TaskType = (long)TaskTypeCategory.PaymentApporveCancel;
                            //    objDAR.UserId = objCustomerApproveRejectModel.UserId;
                            //    objDAR.CreatedBy = objCustomerApproveRejectModel.UserId;
                            //    objDAR.LocationId = objCustomerApproveRejectModel.LocationId;
                            //    objDAR.CreatedOn = DateTime.UtcNow;
                            //    CommonManager.SaveDAR(objDAR);
                            //    #endregion DAR
                            //}
                            //else
                            //{
                            //    result = CommonMessage.FailureMessage();
                            //}
                            #endregion Email
                        }
                    }

                    //-----------------------------------------------------------------------
                }
                else
                {
                    result = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public JsonResult ApproveInvoice(ApproveDenyClientInvoiceModel objADCIM)", "Exception While Approving Client Invoice.", null);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelInvoice(ApproveDenyClientInvoiceModel objADCIM)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long _InvoiceId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        //objADCIM.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objADCIM.InvoiceId))
                {
                    long.TryParse(objADCIM.InvoiceId, out _InvoiceId);
                }
                objADCIM.UserId = ObjLoginModel.UserId;
                objADCIM.Id = _InvoiceId;
                if (objADCIM.Id > 0)
                {
                    bool isSaved = false;
                    result = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();

                    if (objADCIM.Id > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objADCIM.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (userData != null)
                        {
                            DataTable DtResult = ARS.CancelClientInvoice(objADCIM.Id, objADCIM.Comment, objADCIM.UserId);

                            if (DtResult != null && DtResult.Rows.Count > 0)
                            {
                                var booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                                var Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                                if (booStatus)
                                { isSaved = true; }
                                else
                                { isSaved = false; }
                            }
                        }
                        else { isSaved = false; }
                    }

                    //-----------------------------------------------------------------------
                }
                else
                {
                    result = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public JsonResult CancelInvoice(ApproveDenyClientInvoiceModel objADCIM)", "Exception While Submission Client Invoice.", null);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceivePaymentClientInvoice(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            //ViewBag.IsPageRefresh = IsPageRefresh;
            long CountryId = 1;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (id != null)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        //ViewBag.UpdateMode = true;
                        //id = Cryptography.GetDecryptedData(id, true);
                        long _InvoiceId = 0;
                        long.TryParse(id, out _InvoiceId);

                        var objmodel = ARS.Get_InvoiceDetail_ForRecievePayment(_InvoiceId);
                        ViewBag.CustomerId = _InvoiceId;
                        ViewBag.Country = _ICommonMethod.GetAllcountries();
                        ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                        ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                        ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                        ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                        ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                        ViewBag.OffScheduleInvoiceReason = _ICommonMethod.GetGlobalCodeData("OffScheduleInvoiceReason");
                        ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                        ViewBag.PaymentMethod = ARS.GetGlobalCodes(0, "PaymentMethod");
                        ViewBag.DepositAccount = ARS.GetGlobalCodes(0, "PaymentMethod");
                        ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                        return View(objmodel);
                    }
                    else
                    {
                        ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                        ViewBag.Message = Result.DoesNotExist;
                        return View("Error");
                    }
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult ReceivePaymentClientInvoiceSubmit(ClientInvoiceDataModel ObjInv)
        {
            bool booStatus = false;
            var Message = "";

            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            ObjInv.UserId = objLoginSession.UserId;

            try
            {
                if (ObjInv.Id > 0)
                {
                    string xmlInvoiceDetails = GetXmlString(ObjInv);
                    DataTable DtResult = ARS.RecievePaymentInvoiceBill(xmlInvoiceDetails, ObjInv.UserId);
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    //if (DtResult != null && DtResult.Rows.Count > 0)
                    //{
                    //    booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                    //    Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                    //    if (booStatus)
                    //    {
                    //        ARS.ClientInvoice_AccountTransaction("I", ObjInv.CompanyId,null,null,null, ObjInv.Id, ObjInv.GrandTotal,
                    //            null, ObjInv.DepositAccount, ObjInv.UserId,ObjInv.ClientLocationCode,null,null,"Y" );
                    //    }
                    //    else
                    //    {
                    //        throw new Exception(Message);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public ActionResult RecievePaymentClientInvoiceSubmit(ClientInvoiceDataModel ObjInv)", "Exception While Recieve Payment Client Invoice.", ObjInv);
                throw;
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            //return RedirectToAction("ClientInvoiceList");
            return View("ClientInvoiceList");
        }

        [HttpPost]
        public JsonResult GetDataFromLocationMasterSetting(string Id)
        {
            string getData;
            try
            {
                eTracLoginModel ObjLoginModel = null;
                long _LocationId = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    _LocationId = ObjLoginModel.LocationID;
                }


                //
                //if (!string.IsNullOrEmpty(Id))
                //{
                //    long.TryParse(Id, out _LocationId);
                //}

                var Dt = ARS.GetDataFromLocationMasterSetting(_LocationId);
                getData = Convert.ToString(Dt.Rows[0][0]);

                return new JsonResult()
                {
                    Data = new
                    {
                        LocationCode = Convert.ToString(Dt.Rows[0]["LocationCode"]),
                        LocationAddress = Convert.ToString(Dt.Rows[0]["LocationAddress"]),
                        PaymentTerm = Convert.ToString(Dt.Rows[0]["PaymentTerm"]),
                        PaymentTermDesc = Convert.ToString(Dt.Rows[0]["PaymentTermDesc"]),
                        ContractType = Convert.ToString(Dt.Rows[0]["ContractType"]),
                        ContractTypeDesc = Convert.ToString(Dt.Rows[0]["ContractTypeDesc"]),

                        LocationId = Convert.ToString(Dt.Rows[0]["LocationId"]),
                        CompanyId = Convert.ToString(Dt.Rows[0]["CompanyId"]),
                        ClientCompanyName = Convert.ToString(Dt.Rows[0]["ClientCompanyName"]),
                        ClientPointOfContactName = Convert.ToString(Dt.Rows[0]["ClientPointOfContactName"]),
                        PositionTitle = Convert.ToString(Dt.Rows[0]["PositionTitle"]),
                    }
                };
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult GetItemMasterDataByItemId(string Id)
        {
            try
            {
                long _Id = 0;
                if (!string.IsNullOrEmpty(Id))
                {
                    long.TryParse(Id, out _Id);
                }
                var Dt = ARS.GetItemMasterDetails(_Id);

                return new JsonResult()
                {
                    Data = new
                    {
                        ItemDescription = Convert.ToString(Dt.Rows[0]["ItemDescription"]),
                        CategoryType = Convert.ToString(Dt.Rows[0]["CategoryType"]),
                        CategoryTypeDesc = Convert.ToString(Dt.Rows[0]["CategoryTypeDesc"]),
                        RevenueAccount = Convert.ToString(Dt.Rows[0]["RevenueAccount"]),
                        RevenueAccountDesc = Convert.ToString(Dt.Rows[0]["RevenueAccountDesc"]),
                        ItemRate = Convert.ToString(Dt.Rows[0]["ItemRate"]),
                        TaxPercentage = Convert.ToString(Dt.Rows[0]["TaxPercentage"]),
                    }
                };
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }

        #region Item Master

        public ActionResult ItemList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllItemList(string _search, long? ItemCode, string flagApproved = null, string CustomerType = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (flagApproved != "Y")
                ItemCode = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (flagApproved == "Y")
                    ItemCode = ObjLoginModel.LocationID;
            }
            try
            {
                var AllItemList = new List<ItemServiceModel>();
                AllItemList = ARS.Get_ItemList(ItemCode, string.IsNullOrEmpty(_search) == true ? "" : _search).Select(a => new ItemServiceModel()
                {
                    Id = a.Id,
                    ItemCode = a.ItemCode,
                    ItemDescription = a.ItemDescription,
                    CategoryType = a.CategoryType,
                    Category = a.Category,
                    RevenueCode = a.RevenueCode,
                    Revenue = a.Revenue,
                    ExpenseType = a.ExpenseType,
                    Expense = a.Expense,
                    ItemRate = a.ItemRate,
                    ItemUnit = a.ItemUnit,
                    Unit = a.Unit,
                    SpecialNote = a.SpecialNote,
                    TaxPercentage = a.TaxPercentage,
                    EntryBy = a.EntryBy,
                }).OrderByDescending(x => x.Id).ToList();
                return Json(AllItemList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Item(Int64 ItemId)
        {
            ViewBag.ITEMCATEGORY = ARS.GetGlobalCodes(0, "ITEMCATEGORY");
            ViewBag.REVENUETYPE = ARS.GetGlobalCodes(0, "REVENUETYPE");
            ViewBag.EXPENCETYPE = ARS.GetGlobalCodes(0, "EXPENCETYPE");
            ViewBag.ITEMUNITTYPE = ARS.GetGlobalCodes(0, "ITEMUNITTYPE");
            ItemServiceModel ISM = new ItemServiceModel();
            ViewBag.Title = "Add Item";
            if (ItemId > 0)
            {
                ISM = ARS.Get_ItemList(ItemId, "").FirstOrDefault();
                ViewBag.Title = "Edit Item";
            }
            return View("Item", ISM);
        }
        public JsonResult ViewItem(Int64 ItemId)
        {
            ItemServiceModel ISM = new ItemServiceModel();
            if (ItemId > 0)
            {
                ISM = ARS.Get_ItemList(ItemId, "").FirstOrDefault();
            }
            return Json(ISM, JsonRequestBehavior.AllowGet);
        }
        public string CheckItemCode(string ItemCode, double Id)
        {
            var status = "1";
            List<ItemServiceModel> ListISM = new List<ItemServiceModel>();
            if (ItemCode != "")
            {
                if (Id > 0)
                {
                    ListISM = ARS.Get_ItemList(0, ItemCode).Where(c => c.ItemCode == ItemCode && c.Id != Id).ToList();
                }
                else
                {
                    ListISM = ARS.Get_ItemList(0, ItemCode).Where(c => c.ItemCode == ItemCode).ToList();
                }
                if (ListISM.Count() > 0)
                {
                    status = "0";
                }
            }
            return status;
        }
        [HttpPost]
        public string ItemSubmit(ItemServiceModel ISM)
        {
            var status = "";
            DataTable Dt = new DataTable();
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                ISM.EntryBy = objLoginSession.UserId.ToString();
                XmlDocument xml = new XmlDocument();
                XmlSerializer xmlSerializer = new XmlSerializer(ISM.GetType());
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xmlSerializer.Serialize(xmlStream, ISM);
                    xmlStream.Position = 0;
                    xml.Load(xmlStream);
                }
                Dt = InsertUpdateItem(xml.InnerXml, ISM.Id);
                status = "1";
            }
            catch (Exception)
            {
                status = "0";
                throw;
            }

            return status;
        }
        public DataTable InsertUpdateItem(string xml, long? id)
        {
            string QueryString = "exec Usp_InsertUpdate_Item_Master '" + xml + "','" + id + "'";
            return DBUtilities.GetDTResponse(QueryString);
        }
        #endregion

        #region Credit Invoice

        [HttpGet]
        public ActionResult ClientCreditInvoiceList()
        {
            try
            {
                //var id = Cryptography.GetDecryptedData("JPXO3WbEfW2wZFBq+BB7dA==", true);
                eTracLoginModel ObjLoginModel = null;
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
                ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                //ViewBag.AdministratorList = null;
                //ViewBag.IsPageRefresh = IsPageRefresh;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        public ActionResult CreateCreditInvoice(string id = "", string IsDraft = "")
        {
            var objmodel = new CreditMemoDataModel();
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                long CountryId = 1;
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                ViewBag.OffScheduleInvoiceReason = _ICommonMethod.GetGlobalCodeData("OffScheduleInvoiceReason");
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.UpdateMode = false;
                    var objItem = new CreditMemoItemDetails()
                    {
                        SrNo = 1
                    };
                    objmodel.ListInvoiceItemDetails = new List<CreditMemoItemDetails>();
                    objmodel.ListInvoiceItemDetails.Add(objItem);
                    objmodel.EmployeeIssuingCredit = objLoginSession.FName + " " + objLoginSession.LName;
                    objmodel.Id = 0;
                }
                else
                {
                    ViewBag.UpdateMode = false;
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    objmodel = ARS.GetInvoiceDataForCreditMemo(id);
                    objmodel.EmployeeIssuingCredit = objLoginSession.FName + " " + objLoginSession.LName;
                    objmodel.InvoiceDocument = "";
                    objmodel.ListInvoiceItemDetails = ARS.GetInvoiceItemsListForCreditMemo(id);
                    objmodel.Id = 0;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            return View(objmodel);
        }

        public ActionResult EditCreateCreditInvoice(string id = "", string IsDraft = "")
        {
            eTracLoginModel ObjLoginModel = null;
            var objmodel = new CreditMemoDataModel();
            //ViewBag.IsPageRefresh = IsPageRefresh;
            long CountryId = 1;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (id != null)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        ViewBag.UpdateMode = true;
                        CountryId = 1;
                        ViewBag.Country = _ICommonMethod.GetAllcountries();
                        ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                        ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                        ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                        ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                        ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                        ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                        ViewBag.OffScheduleInvoiceReason = _ICommonMethod.GetGlobalCodeData("OffScheduleInvoiceReason");
                        ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                        objmodel = ARS.GetCreditMemoDetailsForEdit(id, IsDraft);
                        objmodel.ListInvoiceItemDetails = ARS.GetCreditMemoItemDetailsForEdit(id, IsDraft);
                        return View("CreateCreditInvoice", objmodel);
                    }
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CreateCreditInvoice", objmodel);
        }

        public ActionResult GetCreateCreditInvoice(CreditMemoDataModel ObjInv, List<CreditMemoItemDetails> InvoiceItemDetailsList)
        {
            var objmodel = new CreditMemoDataModel();
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                long CountryId = 1;
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                ViewBag.OffScheduleInvoiceReason = _ICommonMethod.GetGlobalCodeData("OffScheduleInvoiceReason");
                objmodel = ObjInv;
                objmodel.ListInvoiceItemDetails = InvoiceItemDetailsList;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            return View(objmodel);
        }

        [HttpPost]
        public JsonResult CreateCreditMemoInvoiceSubmit(CreditMemoDataModel ObjInv, List<CreditMemoItemDetails> InvoiceItemDetailsList)
        {
            bool booStatus = false;
            dynamic Result = "";
            HttpFileCollectionBase files = Request.Files;
            var Message = "";
            string Action = "";
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            ObjInv.UserId = objLoginSession.UserId;
            ObjInv.ClientLocationCode = objLoginSession.LocationID;
            try
            {
                if (files[0].ContentLength > 0)
                {
                    string AttachmentName = ObjInv.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(files[0].FileName);
                    CommonHelper.StaticUploadImage(files[0], Server.MapPath(ConfigurationManager.AppSettings["InvoiceDocuments"]), AttachmentName);
                    ObjInv.InvoiceDocument = AttachmentName;
                }
                if (ObjInv.CreditMemoId == 0)
                {
                    Action = "I";
                    string xmlInvoiceDetails = GetXmlString(ObjInv);
                    string xmlItemDetails = GetXmlString(InvoiceItemDetailsList);

                    DataTable DtResult = ARS.InsertUpdateCreditMemo(Action, xmlInvoiceDetails, xmlItemDetails, ObjInv.UserId, 0, ObjInv.CreditMemoStatus == "0" ? "Y" : "N");
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                        //if (booStatus)
                        //{
                        //    Obj.Result = Result.Completed;
                        //}
                        //else
                        //{
                        //    throw new Exception(Message);
                        //}
                    }
                }
                else
                {
                    var context = new workorderEMSEntities();
                    Action = "U";
                    string xmlInvoiceDetails = GetXmlString(ObjInv);
                    string xmlItemDetails = GetXmlString(InvoiceItemDetailsList);

                    DataTable DtResult = ARS.InsertUpdateCreditMemo((!String.IsNullOrEmpty(ObjInv.DraftCreditMemoNumber) && ObjInv.CreditMemoStatus == "1") ? "I" : "U", xmlInvoiceDetails, xmlItemDetails, ObjInv.UserId, 0, ObjInv.CreditMemoStatus == "0" ? "Y" : "N");
                    ViewBag.InvoiceStatus = ARS.GetGlobalCodes(0, "InvoiceStatus");
                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);

                    }
                }
                return new JsonResult()
                {
                    Data = new
                    {
                        Status = booStatus,
                        Message
                    }
                };
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public ActionResult CreateClientInvoiceSubmit(ClientInvoiceDataModel ObjInv, List<InvoiceItemDetails> InvoiceItemDetailsList)", "Exception While Saving Client Invoice.", ObjInv);
                return new JsonResult()
                {
                    Data = new
                    {
                        Status = 0,
                        Messsage = ex.Message
                    }
                };
            }
            //ViewBag.IsPageRefresh = IsPageRefresh;
            //return RedirectToAction("ClientInvoiceList");
            //return View("ClientInvoiceList");
        }

        [HttpGet]
        public JsonResult GetAllCreditInvoiceList(string _search, long? LocationId, string flagApproved = null, string UserType = null, string InvoiceStatus = null, string InvoiceType = null, long? ClientLocationCode = null, string StartDate = null, string EndDate = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            
            //if (flagApproved != "Y")
            //    LocationId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                //if (flagApproved == "Y")
                    LocationId = ObjLoginModel.LocationID;
            }
            if (ClientLocationCode == null)
            {
                ClientLocationCode = LocationId;
            }
            try
            {
                var AllInvoiceList = new List<CreditMemoDataModel>();
                AllInvoiceList = ARS.Get_CreditInvoiceList(LocationId, string.IsNullOrEmpty(_search) == true ? "" : _search).Select(a => new CreditMemoDataModel()
                {
                    //Id = Cryptography.GetEncryptedData(Convert.ToString(a.Id), true),
                    CreditMemoId = a.CreditMemoId,
                    CreditMemoNumber = a.CreditMemoNumber,
                    CreditIssuedDate = a.CreditIssuedDate,
                    CreditIssuedDateDisplay = a.CreditIssuedDateDisplay,
                    GrandTotal = a.GrandTotal,
                    PaymentTotal = a.PaymentTotal,
                    TotalCreditAmount = a.TotalCreditAmount,
                    CreditMemoStatus = a.CreditMemoStatus,
                    CreditMemoStatusDesc = a.CreditMemoStatusDesc,
                    CreditMemoType = a.CreditMemoType,
                    CreditMemoTypeDesc = a.CreditMemoTypeDesc,
                    ClientLocationCode = a.ClientLocationCode,
                    PaymentReminder = (a.InvoiceDueDate < DateTime.Now && (a.InvoiceStatus != "4" && a.InvoiceStatus != "5" && a.InvoiceStatus != "0")) ? true : false,
                    IsDraft = a.IsDraft,
                    InvoiceNumber = a.InvoiceNumber,
                    EntryOn = a.EntryOn
                }).OrderByDescending(x => x.EntryOn).ToList();
                //if (LocationId > 0)
                //{
                //    AllInvoiceList = AllInvoiceList.Where(x => x.InvoiceStatus == "Y").ToList();
                //}
                //else
                //{
                //    AllInvoiceList = AllInvoiceList.Where(x => x.InvoiceStatus != "Y").ToList();     
                //}

                if (InvoiceStatus != null && InvoiceStatus != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.CreditMemoStatus == InvoiceStatus).ToList();
                }
                if (InvoiceType != null && InvoiceType != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.CreditMemoType == Convert.ToInt32(InvoiceType)).ToList();
                }
                if (ClientLocationCode > 0 && ClientLocationCode != null)
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.ClientLocationCode == ClientLocationCode).ToList();
                }
                if (StartDate != null && StartDate != "")
                {
                    AllInvoiceList = AllInvoiceList.Where(c => c.CreditIssuedDate >= Convert.ToDateTime(StartDate) && c.CreditIssuedDate <= Convert.ToDateTime(EndDate)).ToList();
                }

                return Json(AllInvoiceList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCreditInvoiceDataToView(string Id, bool IsDraft)
        {
            var getData = new CreditMemoDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long InvoiceId = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    //if (LocationId == null)
                    //{
                    //    LocationId = ObjLoginModel.LocationID;
                    //}
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    long.TryParse(Id, out InvoiceId);
                }
                string Url = ConfigurationManager.AppSettings["InvoiceDocuments"];
                DataSet DS = (IsDraft == true) ? ARS.GetDraftCreditInvoiceDataforView(InvoiceId) : ARS.GetCreditInvoiceDataforView(InvoiceId);
                CreditMemoDataModel CID = DataRowToObject.CreateListFromTable<CreditMemoDataModel>(DS.Tables[0]).Select(x => new CreditMemoDataModel()
                {
                    CreditMemoNumber = x.CreditMemoNumber,
                    DraftCreditMemoNumber = x.DraftCreditMemoNumber,
                    CreditMemoStatus = x.CreditMemoStatus,
                    ClientCompanyName = x.ClientCompanyName,
                    ClientLocationName = x.ClientLocationName,
                    CreditMemoStatusDesc = x.CreditMemoStatusDesc,
                    ClientPointOfContactName = x.ClientPointOfContactName,
                    PositionTitle = x.PositionTitle,
                    CreditIssuedDateDisplay = x.CreditIssuedDate.ToString("dd MMM yyyy"),
                    InvoiceNumber = x.InvoiceNumber,
                    LocationCode = x.LocationCode,
                    LocationAddress = x.LocationAddress,
                    TotalCreditAmount = x.TotalCreditAmount,
                    InvoiceDateDisplay = x.InvoiceDate.ToString("dd MMM yyyy"),
                    ContractTypeDesc = x.ContractTypeDesc,
                    EntryOn = x.ModifiedOn,
                    ModifiedOn = x.ModifiedOn,
                    EntryByDisplay = x.EntryByDisplay,
                    ModifiedByDisplay = x.ModifiedByDisplay,
                    EntryOnDisplay = x.EntryOnDisplay,
                    ModifiedOnDisplay = x.ModifiedOnDisplay,
                    CreditMemoTypeDesc = x.CreditMemoTypeDesc,
                    TaxAmount = x.TaxAmount,
                    GrandTotal = x.GrandTotal,
                    InvoiceDocument = x.InvoiceDocument,
                    InvoiceDocumentUrl = Server.MapPath(Url) + x.InvoiceDocumentUrl,
                    SubmissionOn = x.SubmissionOn,
                    InvoiceLastSenttoclientDate = x.InvoiceLastSenttoclientDate,
                    PendingAmount = x.PendingAmount,
                    Id = x.Id,
                    InvoiceStatus = x.InvoiceStatus,
                    ApprovedByDisplay=x.ApprovedByDisplay,
                    ApprovedOnDisplay = x.ApprovedOnDisplay,
                    Comment = x.Comment
                }).FirstOrDefault();
                CID.ListInvoiceItemDetails = DataRowToObject.CreateListFromTable<CreditMemoItemDetails>(DS.Tables[1]);
                getData = CID;

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveRejectCreditMemo(ApproveRejectCreditMemol objCreditMemo)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long _InvoiceId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        //objADCIM.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objCreditMemo.InvoiceId))
                {
                    long.TryParse(objCreditMemo.InvoiceId, out _InvoiceId);
                }
                objCreditMemo.UserId = ObjLoginModel.UserId;
                objCreditMemo.Id = _InvoiceId;
                if (objCreditMemo.Id > 0)
                {
                    bool isSaved = false;
                    result = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();
                    string Status = "";

                    if (objCreditMemo.Id > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objCreditMemo.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (userData != null)
                        {
                            if (objCreditMemo.Action == "A")
                            {
                                result = "Credit Memo Approved.";
                                Status = "A";
                            }
                            else
                            {
                                result = "Credit Memo Rejected.";
                                Status = "R";
                            }

                            DataTable DtResult = ARS.ApproveRejectCreditMemo(objCreditMemo.Id, objCreditMemo.Comment, Status, objCreditMemo.UserId);

                            if (DtResult != null && DtResult.Rows.Count > 0)
                            {
                                var booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                                var Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                                if (booStatus)
                                {
                                    isSaved = true;
                                }
                                else
                                {
                                    isSaved = false;
                                }
                            }
                        }
                        else { isSaved = false; }
                        if (isSaved == true)
                        {
                            #region Email
                            //var objEmailLogRepository = new EmailLogRepository();
                            //var objEmailReturn = new List<EmailToManagerModel>();
                            //var objListEmailog = new List<EmailLog>();
                            //var objTemplateModel = new TemplateModel();
                            //if (isSaved == true)
                            //{
                            //    var vendorDetail = _workorderems.spGetVendorAllDetail(objCustomerApproveRejectModel.Customer).FirstOrDefault();
                            //    var locationDetails = _workorderems.LocationMasters.Where(x => x.LocationId == objCustomerApproveRejectModel.LocationId
                            //                                                         && x.IsDeleted == false).FirstOrDefault();
                            //    if (vendorDetail != null)
                            //    {
                            //        bool IsSent = false;
                            //        var objEmailHelper = new EmailHelper();
                            //        objEmailHelper.emailid = vendorDetail.COD_Email;
                            //        objEmailHelper.VendorName = vendorDetail.CMP_NameLegal;
                            //        objEmailHelper.LocationName = locationDetails.LocationName;
                            //        objEmailHelper.VendorId = objCustomerApproveRejectModel.Customer.ToString();
                            //        objEmailHelper.ApproveRemoveStatus = ApproveRemoveSatus;
                            //        objEmailHelper.MailType = "VENDORAPPROVEDREJECT";
                            //        objEmailHelper.SentBy = objCustomerApproveRejectModel.UserId;
                            //        objEmailHelper.LocationID = objCustomerApproveRejectModel.LocationId;
                            //        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            //        IsSent = objEmailHelper.SendEmailWithTemplate();

                            //        //Push Notification     
                            //        if (IsSent == true)
                            //        {
                            //            var objEmailog = new EmailLog();
                            //            try
                            //            {
                            //                objEmailog.CreatedBy = objCustomerApproveRejectModel.UserId;
                            //                objEmailog.CreatedDate = DateTime.UtcNow;
                            //                objEmailog.DeletedBy = null;
                            //                objEmailog.DeletedOn = null;
                            //                objEmailog.LocationId = objCustomerApproveRejectModel.LocationId;
                            //                objEmailog.ModifiedBy = null;
                            //                objEmailog.ModifiedOn = null;
                            //                objEmailog.SentBy = objCustomerApproveRejectModel.LocationId;
                            //                objEmailog.SentEmail = vendorDetail.COD_Email;
                            //                objEmailog.Subject = objEmailHelper.Subject;
                            //                objEmailog.SentTo = objCustomerApproveRejectModel.Customer;
                            //                objListEmailog.Add(objEmailog);
                            //            }
                            //            catch (Exception)
                            //            {
                            //                throw;
                            //            }
                            //        }
                            //        using (var context = new workorderEMSEntities())
                            //        {
                            //            context.EmailLogs.AddRange(objListEmailog);
                            //            context.SaveChanges();
                            //        }

                            //    }
                            //    #region Save DAR
                            //    objDAR.ActivityDetails = DarMessage.VendorApprovedCancel(vendorDetail.CMP_NameLegal, locationDetails.LocationName, ApproveRemoveSatus);
                            //    objDAR.TaskType = (long)TaskTypeCategory.PaymentApporveCancel;
                            //    objDAR.UserId = objCustomerApproveRejectModel.UserId;
                            //    objDAR.CreatedBy = objCustomerApproveRejectModel.UserId;
                            //    objDAR.LocationId = objCustomerApproveRejectModel.LocationId;
                            //    objDAR.CreatedOn = DateTime.UtcNow;
                            //    CommonManager.SaveDAR(objDAR);
                            //    #endregion DAR
                            //}
                            //else
                            //{
                            //    result = CommonMessage.FailureMessage();
                            //}
                            #endregion Email
                        }
                    }

                    //-----------------------------------------------------------------------
                }
                else
                {
                    result = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public JsonResult ApproveInvoice(ApproveDenyClientInvoiceModel objADCIM)", "Exception While Approving Client Invoice.", null);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult PrintInvoicePDF(string Body)
        //{

        //    string Data = HttpUtility.UrlDecode(Body);
        //    var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
        //    var path1 = "/Documents/random.pdf";
        //    path1 = Server.MapPath("~/" + path1);
        //    htmlToPdf.GeneratePdf(Data.ToString(), null, path1);
        //    return Json(path1, JsonRequestBehavior.AllowGet);
        //}
        public string HTMLToPdf(string HTML, string FileName)
        {
            FileName = FileName.Replace(' ', '+').Replace('/', '@');//Here @ char replace due to '/' encrypt id it break the URL to open file in HTMLToPDF

            string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
            var fileName = System.Web.HttpContext.Current.Server.MapPath("~/Content/eMaintenance/SurveyDownload/") + FileName + ".pdf";

            //Render PlaceHolder to temporary stream
            System.IO.StringWriter stringWrite = new StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            StringReader reader = new StringReader(HTML);

            //Create PDF document
            Document doc = new Document(PageSize.A4);
            HTMLWorker parser = new HTMLWorker(doc);
            PdfWriter.GetInstance(doc, new FileStream(fileName,

            FileMode.Create));
            doc.Open();

            /********************************************************************************/
            //var interfaceProps = new Dictionary<string, Object>();
            //var ih = new ImageHander() { BaseUri = Request.Url.ToString() };
            //interfaceProps.Add(HTMLWorker.IMG_PROVIDER, ih);

            foreach (IElement element in HTMLWorker.ParseToList(
            new StringReader(HTML), null))
            {
                doc.Add(element);
            }
            doc.Close();
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName);     // to open file prompt Box open or Save file        
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();

            return FileName;
        }
        #endregion

        #region DBCode

        public static string GetXmlString(dynamic Object)
        {
            string XMLString = "";
            XmlDocument xd = new XmlDocument();
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(Object.GetType());

            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, Object);
                xmlStream.Position = 0;
                xd.Load(xmlStream);
            }
            XMLString = xd.InnerXml.ReplaceSpecialCharacters();
            return XMLString;
        }

        #endregion

        #region Upload Invoice QB

        private bool UploadInvoiceToQuickBook(ClientInvoiceDataModel ObjInv, List<InvoiceItemDetails> InvoiceItemDetailsList)
        {
            var booResult = false;
            try
            {
                string realmId = CallbackController.RealMId.ToString(); // Session["realmId"].ToString();
                string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                var principal = User as ClaimsPrincipal;
                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                // Create a ServiceContext with Auth tokens and realmId
                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                DataService commonServiceQBO = new DataService(serviceContext);
                // Create a QuickBooks QueryService using ServiceContext

                ////Create Account 
                //QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                //Account qbAccount = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").FirstOrDefault();

                //QueryService<Invoice> objInvoice = new QueryService<Invoice>(serviceContext);
                //Invoice inv = objInvoice.ExecuteIdsQuery("Select * From Invoice MaxResults 1000").FirstOrDefault();

                ////Add item------------------------------------------------------------------------
                //QueryService<Item> itemQueryService = new QueryService<Item>(serviceContext);
                //Item item = itemQueryService.ExecuteIdsQuery("Select * From Item MaxResults 1000").FirstOrDefault();

                Intuit.Ipp.Data.Invoice invoice = new Intuit.Ipp.Data.Invoice();
                //invoice.Deposit = new Decimal(0.00);
                //invoice.DepositSpecified = true;
                
                //Retrive Customer Data
                QueryService<Customer> customerQueryService = new QueryService<Customer>(serviceContext);
                Customer customer = customerQueryService.ExecuteIdsQuery("Select * From Customer where id='88'").FirstOrDefault();
                invoice.CustomerRef = new ReferenceType()
                {
                    Value = customer.Id,
                    name = customer.DisplayName
                };
                invoice.BillAddr = customer.BillAddr;

                // Step 5: Set other properties such as Total Amount, Due Date, Email status and Transaction Date
                invoice.DueDate = (ObjInv.InvoiceDueDate <= DateTime.MinValue || ObjInv.InvoiceDueDate >= DateTime.MaxValue) ? DateTime.UtcNow.Date : ObjInv.InvoiceDueDate;
                invoice.DueDateSpecified = true;

                invoice.TotalAmt = ObjInv.GrandTotal; //new Decimal(10.00);
                invoice.TotalAmtSpecified = true;
                invoice.EmailStatus = EmailStatusEnum.NotSet;
                invoice.EmailStatusSpecified = true;
                invoice.BillEmail = new EmailAddress()
                {
                    Address = "jiteshitaliya@gmail.com"
                };
                invoice.Balance = new Decimal(10.00);
                invoice.BalanceSpecified = true;

                invoice.TxnDate = DateTime.UtcNow.Date;
                invoice.TxnDateSpecified = true;
                invoice.TxnTaxDetail = new TxnTaxDetail()
                {
                    TotalTax = ObjInv.TaxAmount, //Convert.ToDecimal(10),
                    TotalTaxSpecified = true,
                };

                invoice.DepartmentRef = new ReferenceType()
                {
                    Value = null,
                    name = null,
                    type = null
                };

                
                /* QueryService<Item> querySvcItem = new QueryService<Item>(serviceContext);
                Item item = querySvcItem.ExecuteIdsQuery("SELECT * FROM Item WHERE Name = 'Lighting'").FirstOrDefault();*/
                List<Line> lineList = new List<Line>();

                foreach (InvoiceItemDetails objItem in InvoiceItemDetailsList) {
                    Line line = new Line();
                    line.Description = objItem.ItemDescription;
                    line.Amount = objItem.TotalCost; //new Decimal(100.00);
                    line.AmountSpecified = true;

                    SalesItemLineDetail salesItemLineDetail = new SalesItemLineDetail();
                    salesItemLineDetail.Qty = new Decimal(objItem.ItemQty); //new Decimal(1.0);
                    salesItemLineDetail.QtySpecified = true;
                    salesItemLineDetail.ItemRef = new ReferenceType()
                    {
                        Value = "49", //item.Id,
                        name = "Visual Studio" //item.Name
                    };

                    salesItemLineDetail.ItemAccountRef = new ReferenceType()
                    {
                        Value = "113",
                        name = "Software:Visual Studio"
                    };
                    //salesItemLineDetail.TaxCodeRef = new ReferenceType()
                    //{
                    //    Value = "TAX"
                    //};
                    //salesItemLineDetail.TaxClassificationRef = new ReferenceType()
                    //{
                    //    Value = "EUC-09020802-V1-00120000"
                    //};

                    line.AnyIntuitObject = salesItemLineDetail;
                    line.DetailType = LineDetailTypeEnum.SalesItemLineDetail;
                    line.DetailTypeSpecified = true;

                    lineList.Add(line);
                }

                invoice.Line = lineList.ToArray();

                Intuit.Ipp.Data.Invoice addedInvoice = commonServiceQBO.Add<Intuit.Ipp.Data.Invoice>(invoice);
                booResult = true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                booResult = false;
            }
            return booResult;
        }

        #endregion

        #region Credit Invoice PDF
        public ActionResult CreditInvoicePDF()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public JsonResult ExportResultPDF(string Id, bool IsDraft)
        {
            var getData = new CreditMemoDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long InvoiceId = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(Id))
                {
                    long.TryParse(Id, out InvoiceId);
                }
                string Url = ConfigurationManager.AppSettings["InvoiceDocuments"];
                DataSet DS = (IsDraft == true) ? ARS.GetDraftCreditInvoiceDataforView(InvoiceId) : ARS.GetCreditInvoiceDataforView(InvoiceId);
                CreditMemoDataModel CID = DataRowToObject.CreateListFromTable<CreditMemoDataModel>(DS.Tables[0]).Select(x => new CreditMemoDataModel()

                {
                    CreditMemoNumber = x.CreditMemoNumber,
                    DraftCreditMemoNumber = x.DraftCreditMemoNumber,
                    CreditMemoStatus = x.CreditMemoStatus,
                    ClientCompanyName = x.ClientCompanyName,
                    ClientLocationName = x.ClientLocationName,
                    CreditMemoStatusDesc = x.CreditMemoStatusDesc,
                    ClientPointOfContactName = x.ClientPointOfContactName,
                    PositionTitle = x.PositionTitle,
                    CreditIssuedDateDisplay = x.CreditIssuedDate.ToString("dd MMM yyyy"),
                    InvoiceNumber = x.InvoiceNumber,
                    LocationCode = x.LocationCode,
                    LocationAddress = x.LocationAddress,
                    TotalCreditAmount = x.TotalCreditAmount,
                    InvoiceDateDisplay = x.InvoiceDate.ToString("dd MMM yyyy"),
                    ContractTypeDesc = x.ContractTypeDesc,
                    EntryOn = x.ModifiedOn,
                    ModifiedOn = x.ModifiedOn,
                    EntryByDisplay = x.EntryByDisplay,
                    ModifiedByDisplay = x.ModifiedByDisplay,
                    EntryOnDisplay = x.EntryOnDisplay,
                    ModifiedOnDisplay = x.ModifiedOnDisplay,
                    CreditMemoTypeDesc = x.CreditMemoTypeDesc,
                    TaxAmount = x.TaxAmount,
                    GrandTotal = x.GrandTotal,
                    InvoiceDocument = x.InvoiceDocument,
                    InvoiceDocumentUrl = Server.MapPath(Url) + x.InvoiceDocumentUrl,
                    SubmissionOn = x.SubmissionOn,
                    InvoiceLastSenttoclientDate = x.InvoiceLastSenttoclientDate,
                    PendingAmount = x.PendingAmount,
                    Id = x.Id,
                    InvoiceStatus = x.InvoiceStatus,
                    ApprovedByDisplay = x.ApprovedByDisplay,
                    ApprovedOnDisplay = x.ApprovedOnDisplay,
                    Comment =x.Comment
                }).FirstOrDefault();
                CID.ListInvoiceItemDetails = DataRowToObject.CreateListFromTable<CreditMemoItemDetails>(DS.Tables[1]);
                getData = CID;

                var fileName = "CreditInvoice_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                string filePath = Path.Combine(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CreditInvoiceTempPDFPath"]), fileName);
                if (CommonHelper.StaticCreateFolderIfNeeded(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CreditInvoiceTempPDFPath"])))
                {
                    /*-----------Delete old Temp files-------*/
                    System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["CreditInvoiceTempPDFPath"]));
                    foreach (FileInfo fi in di.GetFiles())
                    {
                        fi.Delete();
                    }
                    /*-----------Delete old Temp files-------*/

                    var file = new Rotativa.ViewAsPdf("CreditInvoicePDF", getData);
                    var byteArray = file.BuildPdf(ControllerContext);
                    var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();
                }
                return Json(new
                {
                    aaData = filePath
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    aaData = "0"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public FileResult DownloadExportFile(string stFile)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(stFile);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(stFile));
        }
        #endregion
    }
}