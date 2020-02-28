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
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class AdminCompanyController : Controller
    {
        // GET: AdminSection/AdminCompany
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IVendorManagement _IVendorManagement; 
        private readonly ICompanyAdmin _ICompanyAdmin;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public AdminCompanyController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, IVendorManagement _IVendorManagement, ICompanyAdmin _ICompanyAdmin)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._IVendorManagement = _IVendorManagement;
            this._ICompanyAdmin = _ICompanyAdmin;
        }
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string LicensePath = Convert.ToString(ConfigurationManager.AppSettings["LicenseAndInsuranceDocument"], CultureInfo.InvariantCulture);
        private string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        public ActionResult Index()
        {
            var objmodel = new VendorSetupManagementModel();
            ViewBag.AccountSection = true;
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                long CountryId = 1;
                ViewBag.Country = _ICommonMethod.GetAllcountries();
                ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                ViewBag.LEAVETYPELIST = _ICommonMethod.GetGlobalCodeData("LEAVETYPE");
                ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                ViewBag.VendorType = _IVendorManagement.ListVendorType();
                ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                ViewBag.CostCodeList = _IVendorManagement.ListAllCostCode();
                ViewBag.LocationListData = _ICommonMethod.ListAllLocation();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger; return View("Error");
            }
            return View("~/Areas/AdminSection/Views/AdminCompany/Index.cshtml", objmodel);//D:\Project\eTrac\WorkOrderEMS\Areas\AdminSection\Views\AdminCompany\CompanyView.cshtml
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-OCT-2018
        /// Created For :To save company details and license insurance details for admin panel
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(VendorSetupManagementModel Obj)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            ViewBag.AccountSection = true;
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            var _workorderems = new workorderEMSEntities();
            try
            {
                Obj.UserId = objLoginSession.UserId;
                //Obj.CompanyType = 2;
                if (Obj.VendorId == null)
                {
                    Obj.VendorId = 0;
                    if (Obj.CompanyDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.CompanyDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.CompanyDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["CompanyDocument"]), FileName);
                        Obj.CompanyDocuments = FileName;
                    }
                    if (Obj.VendorInsuranceModel.InsuranceDocumentFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.VendorInsuranceModel.InsuranceDocumentFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.VendorInsuranceModel.InsuranceDocumentFile, Server.MapPath(ConfigurationManager.AppSettings["LicenseAndInsuranceDocument"]), FileName);
                        Obj.VendorInsuranceModel.InsuranceDocument = FileName;
                    }
                    if (Obj.VendorInsuranceModel.LicenseDocumentFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.VendorInsuranceModel.LicenseDocumentFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.VendorInsuranceModel.LicenseDocumentFile, Server.MapPath(ConfigurationManager.AppSettings["LicenseAndInsuranceDocument"]), FileName);
                        Obj.VendorInsuranceModel.LicenseDocument = FileName;
                    }
                    if (Obj.VendorAccountDetailsModel.AccountDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.VendorAccountDetailsModel.AccountDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.VendorAccountDetailsModel.AccountDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["BankAccountDocsDocument"]), FileName);
                        Obj.VendorAccountDetailsModel.AccountDocuments = FileName;
                    }


                    //if (Session["realmId"] != null)
                    //{
                    string realmId = CallbackController.RealMId.ToString();// Session["realmId"].ToString();
                    try
                    {
                        string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                        var principal = User as ClaimsPrincipal;
                        var cmp_Data = new CompanyInfo();
                       //companyi
                        OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                        // Create a ServiceContext with Auth tokens and realmId
                        ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                        serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                        DataService commonServiceQBO = new DataService(serviceContext);
                        // Create a QuickBooks QueryService using ServiceContext
                        QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                        CompanyInfo company = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();

                        QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                        List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();

                        var address = new PhysicalAddress();
                        var emailaddr = new EmailAddress();
                        var websiteAddr = new WebSiteAddress();
                        var telephoneAddr = new TelephoneNumber();

                        var account = new Account();


                        var getCompanyInfo = _IVendorManagement.GetCompanyInfo(Convert.ToInt64(company.Id));
                        if (Obj.CompanyType == 2)
                        {
                            if (getCompanyInfo > 0)
                            {
                                ViewBag.Message = CommonMessage.CompanyAlreadySaved();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            }
                            else
                            {
                                address.City = Obj.Address1City;
                                address.Country = "USA";
                                address.Line1 = Obj.Address1;
                                address.PostalCode = company.CompanyAddr.PostalCode;
                                company.CompanyAddr = address;
                                emailaddr.Address = Obj.VendorEmail;
                                company.CompanyEmailAddr = emailaddr;
                                websiteAddr.URI = Obj.Website;
                                company.CompanyURL = websiteAddr;
                                telephoneAddr.FreeFormNumber = Obj.Phone1;
                                company.PrimaryPhone = telephoneAddr;
                                company.LegalAddr = address;
                                company.LegalName = Obj.CompanyNameLegal;
                                company.CompanyName = Obj.CompanyNameLegal;
                                CompanyInfo update = commonServiceQBO.Update(company) as CompanyInfo;
                                var resultQuickBook = _IVendorManagement.SaveQuickBookId(company.Id, Obj.VendorId);

                                if (Obj.VendorAccountDetailsModel != null)
                                {
                                    account.Active = true;
                                    if (Obj.VendorAccountDetailsModel.PaymentMode == 1)
                                    {
                                        var str = Obj.VendorAccountDetailsModel.CardNumber.Substring(Obj.VendorAccountDetailsModel.CardNumber.Length - 4);
                                        account.AccountAlias = "CreditCard_" + str;
                                        account.Description = "Credit Card";
                                        account.FullyQualifiedName = "CreditCard_" + str;
                                        account.Name = "CreditCard_" + str;
                                        account.AccountType = AccountTypeEnum.CreditCard;
                                        account.BankNum = Obj.VendorAccountDetailsModel.CardNumber;
                                    }
                                    else
                                    {
                                        var str = Obj.VendorAccountDetailsModel.AccountNumber.Substring(Obj.VendorAccountDetailsModel.AccountNumber.Length - 4);
                                        account.AccountAlias = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.Description = "Bank";
                                        account.FullyQualifiedName = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.Name = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.AccountType = AccountTypeEnum.Bank;

                                        account.AcctNum = Obj.VendorAccountDetailsModel.AccountNumber;
                                        account.BankNum = Obj.VendorAccountDetailsModel.AccountNumber;
                                    }

                                    account.Classification = AccountClassificationEnum.Liability;
                                    account.SubAccount = false;
                                    account.AccountTypeSpecified = true;
                                    account.sparse = false;
                                    Account accountSaved = commonServiceQBO.Add(account) as Account;
                                    Obj.VendorAccountDetailsModel.QuickbookAcountId = Convert.ToInt64(accountSaved.Id);
                                }

                                //This one added on 21 Feb
                                var result = _IVendorManagement.ProcessVendorSetup(Obj);
                                if (result.Result == Result.Completed)
                                {
                                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                                    return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
                                }
                                else
                                {
                                    ViewBag.Message = CommonMessage.VendorFailure();
                                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                                    return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
                                }
                                //ViewBag.Message = CommonMessage.VendorSave();
                                //ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    //}

                    if (Obj.CompanyType == 3)
                    {
                        var result = _IVendorManagement.ProcessVendorSetup(Obj);
                        if (result.Result == Result.Completed)
                        {
                            ViewBag.Message = CommonMessage.SaveSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.VendorFailure();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.CompanyAlreadySaved();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-Nov-2018
        /// Created For : To view Admin company list
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyListAdmin()
        {
            ViewBag.AccountSection = true;
            return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
            //return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-Nov-2018
        /// Created For : To get operating and subsidery company list data.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="LocatonId"></param>
        /// <param name="LocationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllCompanyAdminList(string _search, long? LocatonId, long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            ViewBag.AccountSection = true;
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
                var AllCompanyList = _ICompanyAdmin.GetAllCompanyAdmiDataList(LocationId, rows, TotalRecords, sidx, sord);
                foreach (var company in AllCompanyList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(company.VendorId), true);
                    row.cell = new string[7];
                    row.cell[0] = company.VendorId.ToString();
                    row.cell[1] = company.CompanyNameLegal == null ?"N/A": company.CompanyNameLegal;
                    row.cell[2] = company.CompanyType == null ?"N/A": company.CompanyType;
                    row.cell[3] = company.Address == null ?"N/A": company.Address;
                    row.cell[4] = company.TaxIdNo == null?"N/A": company.TaxIdNo;
                    row.cell[5] = company.StateLicExpDate == null ? "N/A": company.StateLicExpDate;
                    row.cell[6] = company.StateLicDoc == null ?"":HostingPrefix + LicensePath.Replace("~", "") + company.StateLicDoc;
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
        /// Created Date : 16-Nov-2018
        /// Created For : To download state license.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DownloadStateLicense(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var _dataModel = _ICompanyAdmin.GetCompanyDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(_dataModel.LicenseDocument))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + LicensePath.Replace("~", "");
                        RootDirectory = RootDirectory + LicensePath.Replace("~", "") + _dataModel.LicenseDocument;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, _dataModel.LicenseDocument).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _dataModel.LicenseDocument);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + LicensePath.Replace("~", "") + "FileNotFound.png";
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
        /// Created By : Ashwajit Bansod
        /// Created For : To view account
        /// Created Date : 07-Jan-2019
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public ActionResult AddAccount(string CompanyId)
        {
            eTracLoginModel ObjLoginModel = null;
            var objModel = new VendorSetupManagementModel();
            try
            {             
                ViewBag.AccountSection = true;
                long id = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(CompanyId))
                {
                    ViewBag.UpdateMode = true;
                    CompanyId = Cryptography.GetDecryptedData(CompanyId, true);
                    long.TryParse(CompanyId, out id);
                    
                }
                var getCompanyDetails = _IVendorManagement.GetCompanyDetails(id);
                objModel.CompanyType = getCompanyDetails.CompanyType;
                objModel.CompanyId = id;
                objModel.VendorId = id;
                ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/AdminCompany/AddAccount.cshtml", objModel);
        }


        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Ja-2018
        /// Created For : To save bank account for company
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAccountDetailsAdmin(VendorSetupManagementModel Obj)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            var accountSaved = new Account();
            try
            {
                Obj.UserId = objLoginSession.UserId;
                if (Obj.VendorId > 0)
                {
                    if (Obj.VendorAccountDetailsModel.AccountDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.VendorAccountDetailsModel.AccountDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.VendorAccountDetailsModel.AccountDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["BankAccountDocsDocument"]), FileName);
                        Obj.VendorAccountDetailsModel.AccountDocuments = FileName;
                    }
                    //Obj.VendorId = Obj.CompanyId;
                    //if (Session["realmId"] != null)
                    //{
                    string realmId = CallbackController.RealMId.ToString(); //Session["realmId"].ToString();
                        try
                        {
                        string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                            var principal = User as ClaimsPrincipal;
                            var cmp_Data = new CompanyInfo();
                            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                            // Create a ServiceContext with Auth tokens and realmId
                            ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                            DataService commonServiceQBO = new DataService(serviceContext);
                            // Create a QuickBooks QueryService using ServiceContext
                            QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                            CompanyInfo company = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();

                            QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                            List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();

                            var address = new PhysicalAddress();
                            var emailaddr = new EmailAddress();
                            var websiteAddr = new WebSiteAddress();
                            var telephoneAddr = new TelephoneNumber();

                            var account = new Account();

                            var getCompanyInfo = _IVendorManagement.GetCompanyInfo(Convert.ToInt64(company.Id));
                            if (getCompanyInfo > 0 && getCompanyInfo == Convert.ToInt64(company.Id))
                            {
                                ViewBag.Message = CommonMessage.CompanyAlreadySaved();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;

                                if (Obj.VendorAccountDetailsModel != null)
                                {
                                    account.Active = true;
                                    if (Obj.VendorAccountDetailsModel.PaymentMode == 1)
                                    {
                                        var str = Obj.VendorAccountDetailsModel.CardNumber.Substring(Obj.VendorAccountDetailsModel.CardNumber.Length - 4);
                                        account.AccountAlias = "CreditCard_" + str;
                                        account.Description = "Credit Card";
                                        account.FullyQualifiedName = "CreditCard_" + str;
                                        account.Name = "CreditCard_"+ str;
                                        account.AccountType = AccountTypeEnum.CreditCard;
                                        account.BankNum = Obj.VendorAccountDetailsModel.CardNumber;
                                        

                                    }
                                    else
                                    {
                                        var str = Obj.VendorAccountDetailsModel.AccountNumber.Substring(Obj.VendorAccountDetailsModel.AccountNumber.Length - 4);
                                        account.AccountAlias = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.Description = "Bank";
                                        account.FullyQualifiedName = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.Name = Obj.VendorAccountDetailsModel.BankName + str;
                                        account.AccountType = AccountTypeEnum.Bank;
                                        //account.SubAccount = "Saving";
                                        account.AcctNum = Obj.VendorAccountDetailsModel.AccountNumber;
                                        account.BankNum = Obj.VendorAccountDetailsModel.AccountNumber;
                                    }

                                    //account.Classification = AccountClassificationEnum.Liability;
                                    account.SubAccount = false;
                                    account.AccountTypeSpecified = true;
                                    account.sparse = false;
                                    account.CurrentBalance = Obj.VendorAccountDetailsModel.BalanceAmount;
                                    account.CurrentBalanceSpecified = true;
                                    account.OpeningBalance = Obj.VendorAccountDetailsModel.BalanceAmount;
                                    account.OpeningBalanceSpecified = true;
                                    account.OpeningBalanceDate = DateTime.UtcNow;
                                    account.OpeningBalanceDateSpecified = true;
                                   
                                    accountSaved = commonServiceQBO.Add(account) as Account;
                                    Obj.VendorAccountDetailsModel.QuickbookAcountId = Convert.ToInt64(accountSaved.Id);
                                }

                                ViewBag.Message = CommonMessage.VendorSave();
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            }

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message;
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        }
                    //}
                    ViewBag.AccountSection = true;
                    var result = _IVendorManagement.SaveVendorAccount(Obj);
                    if (result.Result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return View("~/Areas/AdminSection/Views/AdminCompany/AddAccount.cshtml");

                    }
                }
                else
                {
                    return View("~/Areas/AdminSection/Views/AdminCompany/AddAccount.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/AdminCompany/CompanyListAdmin.cshtml");
        }
    }
}