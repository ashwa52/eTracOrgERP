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

namespace WorkOrderEMS.Controllers
{
    public class CustomerManagementController : Controller
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

        [HttpGet]
        public ActionResult ListWaitingCustomers()
        {
            try
            {
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
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = IsPageRefresh;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        [HttpGet]
        public ActionResult ListCustomers()
        {
            try
            {
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
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = IsPageRefresh;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }


        [HttpGet]
        public ActionResult ListCustomerEnquiry()
        {
            try
            {
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
                ViewBag.AdministratorList = null;
                ViewBag.IsPageRefresh = IsPageRefresh;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }


        public ActionResult CustomerEnquiryView(string CustomerId, string Status)
        {
            CustomerAllViewDataModel getData = new CustomerAllViewDataModel();
            ViewBag.AdministratorList = null;
            ViewBag.IsPageRefresh = true;
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long Customer = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    //if (LocationId == null)
                    //{
                    //    LocationId = ObjLoginModel.LocationID;
                    //}
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    long.TryParse(CustomerId, out Customer);
                }
                if (Customer > 0)
                {
                    //getData = _ICustomerManagement.GetAllCustomerData(Customer, Status);

                    if (Status == "U")
                    {
                        //getData = _workorderems.spGetVendorAllDetailForEditApproval(CustomerId).Select
                        getData = ARS.GetCustomerAllDetailForEditApproval(Customer)
                        .Select
                           (x => new CustomerAllViewDataModel()
                           {
                               CustomerId = x.CustomerId,
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               CustomerType = x.CustomerType,
                               Address1 = x.Address1 + "-" + x.Address1City,
                               Address1State = x.Address1State,
                               Address2 = x.Address2 + "-" + x.Address2City,
                               Address2State = x.Address2State,
                               Phone1 = x.Phone1,
                               Phone2 = x.Phone2 == null ? "N/A" : x.Phone2,
                               Email = x.Email == null ? "N/A" : x.Email,
                               Website = x.Website == null ? "N/A" : x.Website,
                               DLNo = x.DLNo,
                               MethodOfCommunication = x.MethodOfCommunication,
                               ParkingFacilityLocation = x.ParkingFacilityLocation,
                               IsAllowToSendText = x.IsAllowToSendText,
                               MonthlyPrice = x.MonthlyPrice,
                               IsMonthlyAppointmentSchedule = x.IsMonthlyAppointmentSchedule,
                               //ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate.ToString("yyyy/MM/dd"),
                               ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate,
                               ScheduleAppointTime = x.ScheduleAppointTime,

                               //this is for Payment for 
                               IsMonthlyParkingPaidFor = x.IsMonthlyParkingPaidFor,
                               CompanyName = x.CompanyName,
                               IsSendDirectInvoiceToCompany = x.IsSendDirectInvoiceToCompany,
                               CompanyEmail = x.CompanyEmail,
                               PaymentMethod = x.PaymentMethod,
                               AccountNumber = x.AccountNumber == null ? "N/A" : x.AccountNumber,
                               BankName = x.BankName == null ? "N/A" : x.BankName,
                               IFSCcode = x.IFSCcode == null ? "N/A" : x.IFSCcode,
                               BankRoutingNo = x.BankRoutingNo == null ? "N/A" : x.BankRoutingNo,
                               CardHolderName = x.CardHolderName == null ? "N/A" : x.CardHolderName,
                               Address = x.Address == null ? "N/A" : x.Address,
                               CardNumber = x.CardNumber == null ? "N/A" : x.CardNumber,
                               CardType = x.CardType == null ? "N/A" : x.CardType,
                               CardExpirationDate = x.CardExpirationDate == null ? "N/A" : x.CardExpirationDate,
                               SwiftBICcode = x.SwiftBICcode == null ? "N/A" : x.SwiftBICcode,
                               IsSignupForAutomaticPayment = x.IsSignupForAutomaticPayment,

                           }).FirstOrDefault();
                    }
                    else
                    {
                        getData = ARS.GetCustomerAllDetailForApproval(Customer).Select
                        (x => new CustomerAllViewDataModel()
                        {
                            CustomerId = x.CustomerId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            CustomerType = x.CustomerType,
                            CustomerTypeText = x.CustomerTypeText,
                            Address1 = x.Address1,
                            Address1State = x.Address1State,
                            Address1StateText = x.Address1StateText,
                            Address2 = x.Address2,
                            Address2State = x.Address2State,
                            Address2StateText = x.Address2StateText,
                            Phone1 = x.Phone1,
                            Phone2 = x.Phone2 == null ? "N/A" : x.Phone2,
                            Email = x.Email == null ? "N/A" : x.Email,
                            Website = x.Website == null ? "N/A" : x.Website,
                            DLNo = x.DLNo,
                            MethodOfCommunication = x.MethodOfCommunication,
                            ParkingFacilityLocation = x.ParkingFacilityLocation,
                            MonthlyPrice = x.MonthlyPrice,
                            IsAllowToSendText = x.IsAllowToSendText,
                            IsMonthlyAppointmentSchedule = x.IsMonthlyAppointmentSchedule,
                            ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate,
                            ScheduleAppointTime = x.ScheduleAppointTime,

                            //this is for Payment for 
                            IsMonthlyParkingPaidFor = x.IsMonthlyParkingPaidFor,
                            CompanyName = x.CompanyName,
                            IsSendDirectInvoiceToCompany = x.IsSendDirectInvoiceToCompany,
                            CompanyEmail = x.CompanyEmail,
                            PaymentMethod = x.PaymentMethod,
                            AccountNumber = x.AccountNumber == null ? "N/A" : x.AccountNumber,
                            BankName = x.BankName == null ? "N/A" : x.BankName,
                            IFSCcode = x.IFSCcode == null ? "N/A" : x.IFSCcode,
                            BankRoutingNo = x.BankRoutingNo == null ? "N/A" : x.BankRoutingNo,
                            CardHolderName = x.CardHolderName == null ? "N/A" : x.CardHolderName,
                            Address = x.Address == null ? "N/A" : x.Address,
                            CardNumber = x.CardNumber == null ? "N/A" : x.CardNumber,
                            CardType = x.CardType == null ? "N/A" : x.CardType,
                            CardExpirationDate = x.CardExpirationDate == null ? "N/A" : x.CardExpirationDate,
                            SwiftBICcode = x.SwiftBICcode == null ? "N/A" : x.SwiftBICcode,
                            IsSignupForAutomaticPayment = x.IsSignupForAutomaticPayment,
                        }).FirstOrDefault();

                    }

                 
                  
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
            }
            catch (Exception ex)
            {
               // return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return View(getData) ;
        }

        public CustomerManagementController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICustomerManagement _ICustomerManagement, IBillDataManager _IBillDataManager)
        {

            this._ICustomerManagement = _ICustomerManagement;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            //this._IVendorManagement = _IVendorManagement;
            this._IBillDataManager = _IBillDataManager;

        }
        public ActionResult CustomerManagementSetup()
        {
            var objmodel = new CustomerSetupManagementModel();
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                ViewBag.UpdateMode = false;
                long CountryId = 1;
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                ViewBag.PrefCommMethod = _ICommonMethod.GetGlobalCodeData("PrefCommMethod");
                ViewBag.CardType = _ICommonMethod.GetGlobalCodeData("CardType");
                ViewBag.SecurityQuestion = _ICommonMethod.GetGlobalCodeData("SecurityQuestion");
                var objVehModel = new CustomerVehicleInformationModel();
                var objVehDetails = new CustomerVehicleDetails();
                objVehModel.CustomerVehicleDetails = new List<CustomerVehicleDetails>();
                objVehModel.CustomerVehicleDetails.Add(objVehDetails);
                objmodel.CustomerVehicleModel = new CustomerVehicleInformationModel();
                objmodel.CustomerVehicleModel = objVehModel;
                objmodel.CustomerPaymentModel = new CustomerPaymentInformationModel();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            ViewBag.IsPageRefresh = IsPageRefresh;
            return View(objmodel);
        }

        public ActionResult AddVehicleList(int id)
        {
            long CountryId = 1;
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
            CustomerVehicleDetails objCVD = new CustomerVehicleDetails
            {
                SrNo = id
            };
            return PartialView("_CustomerVehicleDetails", objCVD);
        }

        [HttpPost]
        public ActionResult CustomerManagementSetup(CustomerSetupManagementModel Obj, List<CustomerVehicleDetails> CustomerVehicleDetailsList, string LocationIds)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            ViewBag.IsPageRefresh = IsPageRefresh;
            var objDAR = new DARModel();
            try
            {
                Obj.UserId = objLoginSession.UserId;

                if (Obj.CustomerId == 0 || Obj.CustomerId == null)
                {
                    Obj.CustomerId = 0;
                    Obj.CustomerType = Obj.CustomerPaymentModel.IsMonthlyParkingPaidFor; // I for Individula; C for Company;
                    Obj.ScheduleAppointDate = ExtensionMethods.ToExDateTime(Obj.ScheduleAppointDate);
                    Obj.CustomerPaymentModel.CardExpirationDate = ExtensionMethods.ToExDateTime(Obj.CustomerPaymentModel.CardExpirationDate);

                    var result = ProcessCustomerSetup(Obj, CustomerVehicleDetailsList);

                    if (result.Result == Result.Completed)
                    {
                        return RedirectToAction("ListWaitingCustomers");
                    }
                }
                else
                {
                    Obj.CustomerType = Obj.CustomerPaymentModel.IsMonthlyParkingPaidFor; // I for Individula; C for Company;
                    Obj.ScheduleAppointDate = ExtensionMethods.ToExDateTime(Obj.ScheduleAppointDate);
                    Obj.CustomerPaymentModel.CardExpirationDate = ExtensionMethods.ToExDateTime(Obj.CustomerPaymentModel.CardExpirationDate);

                    var data = ProcessCustomerSetup(Obj, CustomerVehicleDetailsList);
                    if (data.Result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return RedirectToAction("ListCustomers");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return RedirectToAction("CustomerManagementSetup");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            //return View("ListWaitingCustomers");
            return RedirectToAction("ListWaitingCustomers");
        }

        public CustomerSetupManagementModel ProcessCustomerSetup(CustomerSetupManagementModel Obj, List<CustomerVehicleDetails> CustomerVehicleDetailsList)
        {
            bool booStatus = false;
            var Message = "";
            var objCustomerManagement = new CustomerSetupManagementModel();
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            string annualValue = string.Empty; string costPeriod = string.Empty;
            string latefine = string.Empty; string minimumBill = string.Empty;
            long COT_ID = 0;
            if (Obj.CompanyType == 2 || Obj.CompanyType == 3)
            {
                //COT_ID = Convert.ToInt64(Obj.CompanyType);
                //Obj.VendorType = Convert.ToInt64(VendorTypeValue.VendorType);
                //Primarymode = Obj.VendorAccountDetailsModel.PaymentMode;
            }
            else
            {
                //Primarymode = Obj.VendorContractModel.PrimaryPaymentMode;
                //COT_ID = Convert.ToInt64(eCounting.COT_ID);
            }
            COT_ID = 4; // 4 For Client [Customer] 
            string Action = "";
            try
            {
                if (Obj.CustomerId == 0)
                {
                    Action = "I";

                    string XMLBasicDetails = GetXmlString(Obj);
                    string XMLVehicleDetails = GetXmlString(CustomerVehicleDetailsList);
                    string XMLPaymentAccountDetails = GetXmlString(Obj.CustomerPaymentModel);

                    DataTable DtResult = ARS.InsertUpdateCustomerDetailsSubmit(Action, XMLBasicDetails, XMLVehicleDetails, XMLPaymentAccountDetails
                        , Obj.UserId, 0, "N");

                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                        if (booStatus)
                        {
                            Obj.Result = Result.Completed;
                        }
                        else
                        {
                            throw new Exception(Message);
                        }
                    }

                    #region Email
                    //var objEmailLogRepository = new EmailLogRepository();
                    //var objEmailReturn = new List<EmailToManagerModel>();
                    //var objListEmailog = new List<EmailLog>();
                    //var objTemplateModel = new TemplateModel();
                    //if (Obj.Result == Result.Completed)
                    //{

                    //    bool IsSent = false;
                    //    var objEmailHelper = new EmailHelper();
                    //    objEmailHelper.emailid = Obj.VendorEmail;
                    //    objEmailHelper.LocationName = LocName;
                    //    objEmailHelper.VendorId = Obj.VendorId.ToString();
                    //    objEmailHelper.VendorName = Obj.CompanyNameLegal;
                    //    objEmailHelper.MailType = "VENDORCREATE";
                    //    objEmailHelper.SentBy = Obj.UserId;
                    //    objEmailHelper.LocationIdsVendor = LocIds;
                    //    objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                    //    IsSent = objEmailHelper.SendEmailWithTemplate();
                    //    if (Obj.Result == Result.Completed)
                    //    {
                    //        var objEmailog = new EmailLog();
                    //        try
                    //        {
                    //            objEmailog.CreatedBy = Obj.UserId; ;
                    //            objEmailog.CreatedDate = DateTime.UtcNow;
                    //            objEmailog.DeletedBy = null;
                    //            objEmailog.DeletedOn = null;
                    //            objEmailog.LocationId = null;
                    //            objEmailog.ModifiedBy = null;
                    //            objEmailog.ModifiedOn = null;
                    //            objEmailog.SentBy = Obj.UserId;
                    //            objEmailog.SentEmail = Obj.VendorEmail;
                    //            objEmailog.Subject = objEmailHelper.Subject;
                    //            objEmailog.SentTo = Obj.UserId;
                    //            objListEmailog.Add(objEmailog);
                    //        }
                    //        catch (Exception)
                    //        {
                    //            throw;
                    //        }
                    //    }
                    //}
                    //using (var context = new workorderEMSEntities())
                    //{
                    //    context.EmailLogs.AddRange(objListEmailog);
                    //    context.SaveChanges(); ;
                    //}
                    #endregion Email
                    //}
                    #region Save DAR
                    //objDAR.ActivityDetails = DarMessage.CreateVendor(LocName);
                    //objDAR.TaskType = (long)TaskTypeCategory.CreateVendor;
                    #endregion Save DAR
                }
                else
                {
                    var context = new workorderEMSEntities();
                    Action = "U";
                    string XMLBasicDetails = GetXmlString(Obj);
                    string XMLVehicleDetails = GetXmlString(CustomerVehicleDetailsList);
                    string XMLPaymentAccountDetails = GetXmlString(Obj.CustomerPaymentModel);

                    DataTable DtResult = ARS.InsertUpdateCustomerDetailsSubmit(Action, XMLBasicDetails, XMLVehicleDetails, XMLPaymentAccountDetails
                        , Obj.UserId, 0, "Y");

                    if (DtResult != null && DtResult.Rows.Count > 0)
                    {
                        booStatus = Convert.ToBoolean(DtResult.Rows[0]["Status"]);
                        Message = Convert.ToString(DtResult.Rows[0]["Message"]);
                        if (booStatus)
                        {
                            Obj.Result = Result.UpdatedSuccessfully;
                        }
                        else {
                            throw new Exception(Message);
                        }
                    }

                    #region Save DAR
                    //objDAR.ActivityDetails = DarMessage.UpdateVendor();
                    //objDAR.TaskType = (long)TaskTypeCategory.UpdateVendor;
                    #endregion Save DAR
                }
                #region DAR
                //objDAR.UserId = Obj.UserId;
                //objDAR.CreatedBy = Obj.UserId;
                //objDAR.CreatedOn = DateTime.UtcNow;
                //CommonManager.SaveDAR(objDAR);
                #endregion DAR
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public CustomerSetupManagementModel ProcessCustomerSetup(CustomerSetupManagementModel Obj, List<CustomerVehicleDetails> CustomerVehicleDetailsList)", "Exception While Saving All Customer Data.", Obj);
                throw;
            }
            return Obj;
        }

        [HttpPost]
        public JsonResult GetAllCustomerDataToView(string CustomerId, string Status)
        {
            var getData = new CustomerAllViewDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long Customer = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    //if (LocationId == null)
                    //{
                    //    LocationId = ObjLoginModel.LocationID;
                    //}
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    long.TryParse(CustomerId, out Customer);
                }
                if (Customer > 0)
                {
                    //getData = _ICustomerManagement.GetAllCustomerData(Customer, Status);

                    if (Status == "U")
                    {
                        //getData = _workorderems.spGetVendorAllDetailForEditApproval(CustomerId).Select
                        getData = ARS.GetCustomerAllDetailForEditApproval(Customer)
                        .Select
                           (x => new CustomerAllViewDataModel()
                           {
                               CustomerId = x.CustomerId,
                               FirstName = x.FirstName,
                               LastName = x.LastName,
                               CustomerType = x.CustomerType,
                               Address1 = x.Address1 + "-" + x.Address1City,
                               Address1State = x.Address1State,
                               Address2 = x.Address2 + "-" + x.Address2City,
                               Address2State = x.Address2State,
                               Phone1 = x.Phone1,
                               Phone2 = x.Phone2 == null ? "N/A" : x.Phone2,
                               Email = x.Email == null ? "N/A" : x.Email,
                               Website = x.Website == null ? "N/A" : x.Website,
                               DLNo = x.DLNo,
                               MethodOfCommunication = x.MethodOfCommunication,
                               ParkingFacilityLocation = x.ParkingFacilityLocation,
                               IsAllowToSendText = x.IsAllowToSendText,
                               MonthlyPrice = x.MonthlyPrice,
                               IsMonthlyAppointmentSchedule = x.IsMonthlyAppointmentSchedule,
                               //ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate.ToString("yyyy/MM/dd"),
                               ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate,
                               ScheduleAppointTime = x.ScheduleAppointTime,

                               //this is for Payment for 
                               IsMonthlyParkingPaidFor = x.IsMonthlyParkingPaidFor,
                               CompanyName = x.CompanyName,
                               IsSendDirectInvoiceToCompany = x.IsSendDirectInvoiceToCompany,
                               CompanyEmail = x.CompanyEmail,
                               PaymentMethod = x.PaymentMethod,
                               AccountNumber = x.AccountNumber == null ? "N/A" : x.AccountNumber,
                               BankName = x.BankName == null ? "N/A" : x.BankName,
                               IFSCcode = x.IFSCcode == null ? "N/A" : x.IFSCcode,
                               BankRoutingNo = x.BankRoutingNo == null ? "N/A" : x.BankRoutingNo,
                               CardHolderName = x.CardHolderName == null ? "N/A" : x.CardHolderName,
                               Address = x.Address == null ? "N/A" : x.Address,
                               CardNumber = x.CardNumber == null ? "N/A" : x.CardNumber,
                               CardType = x.CardType == null ? "N/A" : x.CardType,
                               CardExpirationDate = x.CardExpirationDate == null ? "N/A" : x.CardExpirationDate,
                               SwiftBICcode = x.SwiftBICcode == null ? "N/A" : x.SwiftBICcode,
                               IsSignupForAutomaticPayment = x.IsSignupForAutomaticPayment,

                           }).FirstOrDefault();
                    }
                    else
                    {
                        getData = ARS.GetCustomerAllDetailForApproval(Customer).Select
                        (x => new CustomerAllViewDataModel()
                        {
                            CustomerId = x.CustomerId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            CustomerType = x.CustomerType,
                            CustomerTypeText = x.CustomerTypeText,
                            Address1 = x.Address1,
                            Address1State = x.Address1State,
                            Address1StateText = x.Address1StateText,
                            Address2 = x.Address2,
                            Address2State = x.Address2State,
                            Address2StateText = x.Address2StateText,
                            Phone1 = x.Phone1,
                            Phone2 = x.Phone2 == null ? "N/A" : x.Phone2,
                            Email = x.Email == null ? "N/A" : x.Email,
                            Website = x.Website == null ? "N/A" : x.Website,
                            DLNo = x.DLNo,
                            MethodOfCommunication = x.MethodOfCommunication,
                            ParkingFacilityLocation = x.ParkingFacilityLocation,
                            MonthlyPrice = x.MonthlyPrice,
                            IsAllowToSendText = x.IsAllowToSendText,
                            IsMonthlyAppointmentSchedule = x.IsMonthlyAppointmentSchedule,
                            ScheduleAppointDate = x.ScheduleAppointDate == null ? "N/A" : x.ScheduleAppointDate,
                            ScheduleAppointTime = x.ScheduleAppointTime,

                            //this is for Payment for 
                            IsMonthlyParkingPaidFor = x.IsMonthlyParkingPaidFor,
                            CompanyName = x.CompanyName,
                            IsSendDirectInvoiceToCompany = x.IsSendDirectInvoiceToCompany,
                            CompanyEmail = x.CompanyEmail,
                            PaymentMethod = x.PaymentMethod,
                            AccountNumber = x.AccountNumber == null ? "N/A" : x.AccountNumber,
                            BankName = x.BankName == null ? "N/A" : x.BankName,
                            IFSCcode = x.IFSCcode == null ? "N/A" : x.IFSCcode,
                            BankRoutingNo = x.BankRoutingNo == null ? "N/A" : x.BankRoutingNo,
                            CardHolderName = x.CardHolderName == null ? "N/A" : x.CardHolderName,
                            Address = x.Address == null ? "N/A" : x.Address,
                            CardNumber = x.CardNumber == null ? "N/A" : x.CardNumber,
                            CardType = x.CardType == null ? "N/A" : x.CardType,
                            CardExpirationDate = x.CardExpirationDate == null ? "N/A" : x.CardExpirationDate,
                            SwiftBICcode = x.SwiftBICcode == null ? "N/A" : x.SwiftBICcode,
                            IsSignupForAutomaticPayment = x.IsSignupForAutomaticPayment,
                        }).FirstOrDefault();

                    }

                    getData.CustomerVehicleDetails = ARS.GetCustomerVehicleDetails(Customer).Select
                    (x => new CustomerVehicleDetails()
                    {
                        LicensePlateNo = x.LicensePlateNo,
                        State = x.State,
                        StateText = x.StateText,
                        Year = x.Year,
                        Make = x.Make,
                        Model = x.Model,
                        Color = x.Model,
                    }).ToList();
                    //getData.LocationAssignedModel = _workorderems.spGetLocationCompanyMappingForApproval(CustomerId).Select
                    //(x => new LocationDataModel()
                    //{
                    //    LocationName = x.LocationName,
                    //    LocationId = x.LLCM_LocationId,
                    //    LLCM_Id = x.LLCM_Id
                    //}).ToList();
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllCustomerList(string _search, long? LocationId, string flagApproved = null, string CustomerType = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (flagApproved != "Y")
                LocationId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (flagApproved == "Y")
                    LocationId = ObjLoginModel.LocationID;
            }
            try
            {
                var AllCompanyList = new List<CustomerSetupManagementModel>();
                AllCompanyList = ARS.Get_CustomerList(LocationId, string.IsNullOrEmpty(_search) == true ? "" : _search).Select(a => new CustomerSetupManagementModel()
                {
                    id = Cryptography.GetEncryptedData(Convert.ToString(a.CustomerId), true),
                    CustomerId = a.CustomerId,
                    CustomerName = a.CustomerName,
                    LocationName = a.LocationName,
                    Address1 = a.Address1,
                    Phone1 = a.Phone1,
                    EmailId = a.EmailId,
                    CustomerType = a.CustomerType,
                    CustomerTypeText = a.CustomerTypeText,
                    Status = a.Status,
                    AccountStatus = a.AccountStatus,
                    //BasicStatus = a.BasicStatus,
                    //VehicleStatus = a.VehicleStatus,

                }).OrderByDescending(x => x.CustomerId).ToList();
                if (LocationId > 0)
                {
                    AllCompanyList = AllCompanyList.Where(x => x.Status == "Y").ToList();
                }
                else
                {
                    AllCompanyList = AllCompanyList.Where(x => x.Status != "Y").ToList();     
                }

                if (CustomerType != null && CustomerType != "" && CustomerType != "All")
                {
                    AllCompanyList = AllCompanyList.Where(c => c.CustomerType == CustomerType).ToList();
                }
                return Json(AllCompanyList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ApproveCustomer(ApproveRejectCustomerModel objCustomerApproveRejectModel)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long CustomerId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId > 0)
                    {
                        objCustomerApproveRejectModel.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objCustomerApproveRejectModel.CustomerId))
                {
                    long.TryParse(objCustomerApproveRejectModel.CustomerId, out CustomerId);
                }
                objCustomerApproveRejectModel.UserId = ObjLoginModel.UserId;
                objCustomerApproveRejectModel.Customer = CustomerId;
                if (objCustomerApproveRejectModel.Customer > 0)
                {
                    //result = ApproveCustomerByCustomerId(objCustomerApproveRejectModel);
                    bool isSaved = false;
                    result = "";
                    string ApproveRemoveSatus = "";
                    var objDAR = new DARModel();
                    var CommonManager = new CommonMethodManager();
                    string Status = "";

                    if (objCustomerApproveRejectModel.Customer > 0)
                    {
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objCustomerApproveRejectModel.UserId
                                                        && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (userData != null)
                        {
                            if (objCustomerApproveRejectModel.Comment == null)
                            {
                                result = CommonMessage.CustomerApprove();
                                ApproveRemoveSatus = "Appoved";
                                Status = "Y";
                            }
                            else
                            {
                                result = CommonMessage.CustomerReject();
                                ApproveRemoveSatus = "Removed";
                                Status = "N";
                            }

                            //var IsApprove = _workorderems.spSetApprovalForVendorAllDetail(objCustomerApproveRejectModel.Customer,
                            //                                                     objCustomerApproveRejectModel.Comment, Status, objCustomerApproveRejectModel.UserId);

                            DataTable DtResult = ARS.InsertUpdateCustomerDetailsSubmit(objCustomerApproveRejectModel.Customer,
                                                                                  objCustomerApproveRejectModel.Comment, Status, objCustomerApproveRejectModel.UserId);

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

                                #region "TEQ"
                                //var _locationList = _workorderems.LocationCompanyMappings.Where(n => n.LCM_CMP_Id == objCustomerApproveRejectModel.Customer).ToList();
                                //if (_locationList != null)
                                //{
                                //    foreach (var item in _locationList)
                                //    {
                                //        long LocId = Convert.ToInt64(item.LCM_Id);
                                //        var resultloc = _workorderems.LocationCompanyMappings.Where(n => n.LCM_Id == LocId).FirstOrDefault();
                                //        if (resultloc != null)
                                //        {
                                //            resultloc.LCM_IsActive = Status;
                                //            _workorderems.SaveChanges();
                                //        }

                                //    }
                                //}
                                #endregion

                                //var facilityApprove = _workorderems.spSetApprovalForCompanyFacilityMapping(objCustomerApproveRejectModel.Customer,
                                //                                                                           objCustomerApproveRejectModel.Comment,
                                //                                                                           Status, objCustomerApproveRejectModel.UserId);
                                //isSaved = true;
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
                    result = "Something went wrong please check Customer Id.";
                }
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public string ApprovePOByPOId(long Id)", "Exception While Approving PO.", null);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCustomer(string id)
       {
            eTracLoginModel ObjLoginModel = null;
            CustomerSetupManagementModel objeFleetDriverModel = new CustomerSetupManagementModel();
            ViewBag.IsPageRefresh = IsPageRefresh;
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
                        id = Cryptography.GetDecryptedData(id, true);
                        long _CustomerId = 0;
                        long.TryParse(id, out _CustomerId);

                        var _CustomerModel = GetCustomerDetailsByCustomerId(_CustomerId);
                        ViewBag.CustomerId = _CustomerId;
                        ViewBag.Country = _ICommonMethod.GetAllcountries();
                        ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                        ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                        ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                        ViewBag.SecurityQuestion = _ICommonMethod.GetGlobalCodeData("SecurityQuestion");
                        ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                        ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                        ViewBag.PrefCommMethod = _ICommonMethod.GetGlobalCodeData("PrefCommMethod");
                        ViewBag.CardType = _ICommonMethod.GetGlobalCodeData("CardType");
                        ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                        ViewBag.IsMonthlyAppointmentSchedule = _CustomerModel.IsMonthlyAppointmentSchedule;
                        return View("CustomerManagementSetup", _CustomerModel);
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
            return View("CustomerManagementSetup");
        }

        public CustomerSetupManagementModel GetCustomerDetailsByCustomerId(long CustomerId)
        {
            var data = new CustomerSetupManagementModel();
            try
            {
                if (CustomerId > 0)
                {
                    data = ARS.Get_CustomerDetail_ForEdit(CustomerId);
                }
                else
                {
                    return data;
                }
                return data;
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "public CustomerSetupManagementModel GetCustomerDetailsByCustomerId(long CustomerId)", "Exception While Editing Customer.", null);
                throw;
            }
        }

        [HttpPost]
        public string GetMonthlyPriceFromLocationSetting(string Id)
        {
            string getData;
            try
            {
                //eTracLoginModel ObjLoginModel = null;

                //if (Session != null && Session["eTrac"] != null)
                //{
                //    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //    UserId = ObjLoginModel.UserId;
                //}

                long _LocationId = 0;
                if (!string.IsNullOrEmpty(Id))
                {
                    long.TryParse(Id, out _LocationId);
                }

                var Dt = ARS.GetMonthlyPriceFromLocationSetting(_LocationId);
                getData = Convert.ToString(Dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                return "0.00";
            }
            return getData;
        }

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
    }
}