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

namespace WorkOrderEMS.Controllers
{
    public class VendorManagementController : Controller
    {
        // GET: VendorManagement
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly IVendorManagement _IVendorManagement;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly IBillDataManager _IBillDataManager;
        private string DocPath = ConfigurationManager.AppSettings["LicenseAndInsuranceDocument"];
        private string DocAccountPath = ConfigurationManager.AppSettings["BankAccountDocsDocument"];
        private string DocFacilityPath = ConfigurationManager.AppSettings["VendorImageFacility"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);

        public VendorManagementController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, IVendorManagement _IVendorManagement, IBillDataManager _IBillDataManager)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._IVendorManagement = _IVendorManagement;
            this._IBillDataManager = _IBillDataManager;
        }
        public ActionResult VendorManagementSetup()
        {
            var objmodel = new VendorSetupManagementModel();
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
            return View(objmodel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-OCT-2018
        /// Created For : To save all vendor data
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VendorManagementSetup(VendorSetupManagementModel Obj)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            var objDAR = new DARModel();
            try
            {
                Obj.UserId = objLoginSession.UserId;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (Obj.ProductList != null)
                {
                    Obj.VendorFacilityListModel = !string.IsNullOrEmpty(Obj.ProductList) ? serializer.Deserialize<List<VendorFacilityModel>>(Obj.ProductList.Replace("/", "-")) : null;
                }
                if (Obj.VendorId == 0 || Obj.VendorId == null)
                {
                    Obj.VendorId = 0;
                    if (Obj.CompanyDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.CompanyDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.CompanyDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["CompanyDocument"]), FileName);
                        Obj.CompanyDocuments = FileName;
                    }
                    if (Obj.VendorContractModel.ContractDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.VendorContractModel.ContractDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.VendorContractModel.ContractDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["VendorContractDocument"]), FileName);
                        Obj.VendorContractModel.ContractDocuments = FileName;
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
                    var result = _IVendorManagement.ProcessVendorSetup(Obj);
                    if (result.Result == Result.Completed)
                    {
                        //if (Session["realmId"] != null)
                        //{
                        string realmId = CallbackController.RealMId.ToString(); // Session["realmId"].ToString();
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

                            PhysicalAddress vendorAddr = new PhysicalAddress();
                            EmailAddress vendoremail = new EmailAddress();
                            TelephoneNumber mobileNumber = new TelephoneNumber();
                            WebSiteAddress websiteaddr = new WebSiteAddress();
                            ModificationMetaData metaData = new ModificationMetaData();
                            Vendor vendor = new Vendor();

                            QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                            //List<Account> accountData customerQueryService.ExecuteIdsQuery(SELECT FROM Customer MaxResults 1000)
                            List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();
                            // vendor.PrimaryPhone.FreeFormNumber =

                            //Mandatory Fields
                            vendor.GivenName = Obj.PointOfContact;
                            vendor.DisplayName = Obj.PointOfContact;
                            if (Obj.VendorAccountDetailsModel.AccountNumber != null)
                            {
                                vendor.AcctNum = Obj.VendorAccountDetailsModel.AccountNumber;
                            }
                            vendor.CompanyName = Obj.CompanyNameLegal;
                            vendor.Active = true;
                            vendor.PrintOnCheckName = Obj.CompanyNameLegal;
                            vendor.TaxIdentifier = Obj.TaxNo;
                            vendor.Suffix = Obj.JobTile;

                            //vendor.ShipAddr.
                            //Address
                            vendorAddr.City = Obj.Address1City;
                            vendorAddr.Country = "USA";
                            vendorAddr.Line1 = Obj.Address1;

                            //End Address

                            //Email
                            if (Obj.VendorEmail != null)
                            {
                                vendoremail.Address = Obj.VendorEmail;
                            }
                            //End Email

                            //Mobile Number
                            mobileNumber.FreeFormNumber = Obj.Phone1;

                            //End Mobile Number

                            //Website
                            if (Obj.Website != null)
                            {
                                websiteaddr.URI = Obj.Website;
                            }
                            //End Website

                            //Created Time
                            metaData.CreateTime = DateTime.UtcNow;
                            //End Created Time
                            vendor.BillAddr = vendorAddr;
                            vendor.PrimaryEmailAddr = vendoremail;
                            vendor.Mobile = mobileNumber;
                            vendor.WebAddr = websiteaddr;
                            vendor.MetaData = metaData;
                            vendor.PrimaryPhone = mobileNumber;
                            Vendor resultVendor = commonServiceQBO.Add(vendor) as Vendor;
                            var resultQuickBook = _IVendorManagement.SaveQuickBookId(resultVendor.Id, Obj.VendorId);
                            if (Obj.VendorFacilityListModel.Count() > 0)
                            {
                                foreach (var item in Obj.VendorFacilityListModel)
                                {
                                    item.UnitCostForView.Replace(",", "");
                                    item.UnitCost = Convert.ToDecimal(item.UnitCostForView);
                                    long CostCodeId = Convert.ToInt64(item.Costcode);
                                    var costCodeName = _IBillDataManager.GetCostCodeData(CostCodeId);
                                    var dataget = accountData.Where(x => x.Name == costCodeName.Description).FirstOrDefault();
                                    var itemdata = new Item();
                                    itemdata.Description = item.ProductServiceName;
                                    itemdata.Name = item.ProductServiceName;
                                    itemdata.FullyQualifiedName = item.ProductServiceName;
                                    itemdata.IncomeAccountRef = new ReferenceType()
                                    {
                                        name = dataget.Name,
                                        Value = dataget.Id
                                    };
                                    itemdata.PrefVendorRef = new ReferenceType()
                                    {
                                        name = resultVendor.DisplayName,
                                        Value = resultVendor.Id
                                    };
                                    itemdata.UnitPrice = Convert.ToDecimal(item.UnitCost);
                                    itemdata.Active = true;
                                    if (item.ProductServiceType == "1")
                                    {
                                        itemdata.Type = ItemTypeEnum.NonInventory;
                                        itemdata.ItemCategoryType = "Product";
                                    }
                                    else
                                    {
                                        itemdata.Type = ItemTypeEnum.Service;
                                        itemdata.ItemCategoryType = "Service";
                                    }
                                    itemdata.TypeSpecified = true;
                                    itemdata.PurchaseCost = Convert.ToDecimal(item.UnitCost);
                                    itemdata.Taxable = true;
                                    itemdata.UnitPriceSpecified = true;
                                    itemdata.AvgCost = Convert.ToDecimal(item.UnitCost);
                                    itemdata.ActiveSpecified = true;
                                    itemdata.PurchaseCostSpecified = true;
                                    //itemdata.RatePercentSpecified = true;

                                    Item resultItem = commonServiceQBO.Add(itemdata) as Item;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        }
                        ViewBag.Message = CommonMessage.VendorSave();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        //return View("CompanyList");
                        return View("UnApprovedVendor");

                        //}
                        //else
                        //{
                        //    ViewBag.Message = CommonMessage.FailureMessage();
                        //    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        //    return View("VendorManagementSetup");
                        //}
                    }
                }
                else
                {
                    if (Obj.CompanyDocumentsFile != null)
                    {
                        string FileName = objLoginSession.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(Obj.CompanyDocumentsFile.FileName);
                        CommonHelper.StaticUploadImage(Obj.CompanyDocumentsFile, Server.MapPath(ConfigurationManager.AppSettings["CompanyDocument"]), FileName);
                        Obj.CompanyDocuments = FileName;
                    }
                    else
                    {
                        Obj.CompanyDocuments = Obj.CompanyDocEdit;
                    }
                    var data = _IVendorManagement.ProcessVendorSetup(Obj);
                    if (data.Result == Result.UpdatedSuccessfully)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return View("UnApprovedVendor");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return View("VendorManagementSetup");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        #region "Ajay Kumar"
        public JsonResult IsTaxNumberIsExists(string TaxNo,long? VendorId)
        {
            bool result = _IVendorManagement.TaxNumberIsExists(TaxNo,Convert.ToInt32(VendorId));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckInsPolicyNumberIsExists(VendorSetupManagementModel Obj)
        { 
            bool result = _IVendorManagement.InsPolicyNumberIsExists(Obj.VendorInsuranceModel.PolicyNumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult FileImport(string id)
        {
            return View();
        }
        
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-Oct-2018
        /// Created For : To store product Image 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FileImport(HttpPostedFileBase File)
        {
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (File != null)
                {
                    string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + File.FileName.ToString();
                    CommonHelper obj_CommonHelper = new CommonHelper();
                    var res = obj_CommonHelper.UploadImage(File, Server.MapPath(ConfigurationManager.AppSettings["VendorImportFilePath"]), ImageName);
                    ViewBag.ImageUrl = res;
                    if (res)
                    {

                        return Json(ImageName);
                    }
                    else { return Json(""); }
                }
                return Json("");
            }
            else
            {
                return Json("");
            }
        }

        #endregion

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-Oct-2018
        /// Created For : To store product Image 
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadedImageFacalityProduct(HttpPostedFileBase File)
        {
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (File != null)
                {
                    string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + File.FileName.ToString();
                    CommonHelper obj_CommonHelper = new CommonHelper();
                    var res = obj_CommonHelper.UploadImage(File, Server.MapPath(ConfigurationManager.AppSettings["VendorImageFacility"]), ImageName);
                    ViewBag.ImageUrl = res;
                    if (res)
                    {

                        return Json(ImageName);
                    }
                    else { return Json(""); }
                }
                return Json("");
            }
            else
            {
                return Json("");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-OCT-2018
        /// Created For: To View List company
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyList()
        {
            return View();
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-OCT-2018
        /// Created For : To get all company or vendor list by location Id
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
        public JsonResult GetAllCompanyListList(string _search, long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if(ObjLoginModel != null)
                {

                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            } 
              
            try
            {
                if (_search != null && _search != "")
                {
                    var AllCompanyListForSearch = _IVendorManagement.GetAllCompanyDataList1(LocationId, rows, TotalRecords, sidx, sord).Where(x => x.CompanyNameLegal.ToLower() == _search.ToLower().Trim()).ToList();
                    return Json(AllCompanyListForSearch.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var AllCompanyList = _IVendorManagement.GetAllCompanyDataList1(LocationId, rows, TotalRecords, sidx, sord);
                    return Json(AllCompanyList.ToList(), JsonRequestBehavior.AllowGet);
                }

                
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
             
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-OCT-2018
        /// Created For : To get all data by vendorId
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllVendorDataToView(string VendorId)
        {
            var getData = new VendorAllViewDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long Vendor = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (LocationId == null)
                    {
                        LocationId = ObjLoginModel.LocationID;
                    }
                    UserId = ObjLoginModel.UserId;
                }
                if (!string.IsNullOrEmpty(VendorId))
                { 
                    long.TryParse(VendorId, out Vendor);
                }
                if (Vendor > 0)
                {
                    getData = _IVendorManagement.GetAllVendorData(Vendor);
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

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 27-OCT-2018
        /// Created For : To Approve Vendor
        /// </summary>
        /// <param name="objPOApproveRejectModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveVendor(ApproveRejectVendorModel objVendorApproveRejectModel)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId = 0; string result = "";
            long VendorId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    LocationId = ObjLoginModel.LocationID;
                    if (LocationId>0) {
                        objVendorApproveRejectModel.LLCM_Id = Convert.ToString(LocationId);
                    }
                }
                if (!string.IsNullOrEmpty(objVendorApproveRejectModel.VendorId))
                {
                   // objVendorApproveRejectModel.VendorId = Cryptography.GetDecryptedData(objVendorApproveRejectModel.VendorId, true);
                    long.TryParse(objVendorApproveRejectModel.VendorId, out VendorId);
                }
                objVendorApproveRejectModel.UserId = ObjLoginModel.UserId;
                objVendorApproveRejectModel.Vendor = VendorId;
                if (objVendorApproveRejectModel.Vendor > 0)
                {
                    result = _IVendorManagement.ApproveVendorByVendorId(objVendorApproveRejectModel);
                }
                else
                {
                    result = "Something went wrong please check Vendor Id.";
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-OCT-2018
        /// Created For : To get all vendor data to edit.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditVendor(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            VendorSetupManagementModel objeFleetDriverModel = new VendorSetupManagementModel();
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
                        long _VendorId = 0;
                        long.TryParse(id, out _VendorId);
                        var _VendorModel = _IVendorManagement.GetVendorDetailsByVendorId(_VendorId);
                        ViewBag.VendorTypeIdData = _VendorModel.VendorType;
                        ViewBag.VendorId = _VendorId;
                        ViewBag.Email = _VendorModel.VendorEmail;
                        ViewBag.CompanyDocEdit = _VendorModel.CompanyDocEdit;
                        ViewBag.Country = _ICommonMethod.GetAllcountries();
                        ViewBag.StateList = _ICommonMethod.GetStateByCountryId(CountryId);
                        ViewBag.JobTitleList = _ICommonMethod.GetGlobalCodeData("UserJobTitle");
                        ViewBag.ClientInvoicingTerm = _IGlobalAdmin.ListClientInvoicingTerm();
                        ViewBag.VendorType = _IVendorManagement.ListVendorType();
                        ViewBag.OperatingHolder = _IGlobalAdmin.ListCompanyHolder(true);
                        ViewBag.ContractType = _IGlobalAdmin.ListContractType();
                        ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                        ViewBag.PaymentTermList = _IVendorManagement.PaymentTermList();
                        ViewBag.CostCodeList = _IVendorManagement.ListAllCostCode();
                        ViewBag.LocationListData = _ICommonMethod.ListAllLocation();
                        ViewBag.AllocatedLocation = _IVendorManagement.ListAllAlocatedLocatioForVender(_VendorId);
                        return View("VendorManagementSetup", _VendorModel);
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
            return View("VendorManagementSetup");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-OCT-2018
        /// Created For : To add account details for vendor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddAccountDetails(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            VendorSetupManagementModel obj = new VendorSetupManagementModel();
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
                        ViewBag.IsAddAccountDetails = true;

                        //id = Cryptography.GetDecryptedData(id, true);
                        long _VendorId = 0;
                        long.TryParse(id, out _VendorId);
                        obj.VendorId = _VendorId;
                        var vendor_id = Cryptography.GetEncryptedData(id, true);
                        ViewBag.VendorIdForCancel = vendor_id;
                        ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
                        return View("AddVendorAccount", obj);
                    }
                }
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-OCT-2018
        /// Created For : To save account details for vendor
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddAccountDetails(VendorSetupManagementModel Obj)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
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
                    var result = _IVendorManagement.SaveVendorAccount(Obj);
                    if (result.Result == Result.Completed)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return View("CompanyList");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return View("VendorManagementSetup");

                    }
                }
                else
                {
                    return View("VendorManagementSetup");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Nov-12-2018
        /// Created For : To View Inurance/License to add.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddInsuraceAndLicenseDetails(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            VendorSetupManagementModel obj = new VendorSetupManagementModel();
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
                        ViewBag.IsAddAccountDetails = true;
                        id = Cryptography.GetDecryptedData(id, true);
                        long _VendorId = 0;
                        long.TryParse(id, out _VendorId);
                        obj.VendorId = _VendorId;
                        return View("AddNewInsuranceAndLicenseVendor", obj);
                    }
                }
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInsuraceAndLicenseDetails(VendorSetupManagementModel Obj)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            try
            {
                Obj.UserId = objLoginSession.UserId;
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
                if (Obj.VendorId > 0)
                {
                    if (Obj.VendorInsuranceModel.LicenseId == 0 && Obj.VendorInsuranceModel.InsuranceID == 0)
                    {
                        ViewBag.VendorId = Obj.VendorId;
                        ViewBag.VendorStatus = true;
                        var result = _IVendorManagement.SaveVendorInsuranceLicense(Obj);
                        if (result.Result == Result.Completed)
                        {
                            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            return View("ListInsuranceLicenseView");
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            return View("ListInsuranceLicenseView");

                        }
                    }
                    else if (Obj.VendorInsuranceModel == null &&
                        Obj.VendorInsuranceModel.InsuranceID > 0 || Obj.VendorInsuranceModel.LicenseId > 0)
                    {
                        ViewBag.VendorId = Obj.VendorId;
                        var result = _IVendorManagement.SaveVendorInsuranceLicense(Obj);
                        if (result.Result == Result.Completed)
                        {
                            ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                            return View("ListInsuranceLicenseView");
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            return View("ListInsuranceLicenseView");

                        }
                    }
                }
                else
                {
                    return View("ListInsuranceLicenseView");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("ListInsuranceLicenseView");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Nov-2018
        /// Created For : To get all location allocated for  vendor
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllAllocatedLocation(long vendorId)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            var result = new List<LocationListServiceModel>();
            try
            {
                if (vendorId > 0)
                {
                    result = _IVendorManagement.ListAllocatedLocatioForVender(vendorId);
                    if (result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 06-Dec-2018
        /// Created For : To View List of License and Insurance.
        /// </summary>
        /// <returns></returns>
        public ActionResult ListInsuranceLicenseView(string Vendorid, bool? VendorStatus)
        {
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                if (!string.IsNullOrEmpty(Vendorid))
                {
                    Vendorid = Cryptography.GetDecryptedData(Vendorid, true);
                    long _VendorId = 0;
                    long.TryParse(Vendorid, out _VendorId);
                    ViewBag.VendorId = _VendorId;
                    ViewBag.VendorStatus = VendorStatus;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 06-Dec-2018
        /// Created For : To get all Insurance List By Vendor Id.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="VendorId"></param>
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
        public JsonResult ListInsurance(string _search, long? VendorId, long? LocationId, bool VendorStatus, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
          
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var AllInsuranceList = _IVendorManagement.GetAllInsuranceDataList(VendorId, LocationId, VendorStatus, rows, TotalRecords, sidx, sord);
                return Json(AllInsuranceList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Dec-2018
        /// Created For : To get all license by vendor Id.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="VendorId"></param>
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
        public JsonResult ListLicense(string _search, long? VendorId, long? LocationId, bool VendorStatus, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            //JQGridResults result = new JQGridResults();
            //List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var AllLicenseList = _IVendorManagement.GetAllLicenseDataList(VendorId, LocationId, VendorStatus, rows, TotalRecords, sidx, sord);
                return Json(AllLicenseList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
           
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To download doc file of insurance and license.
        /// Created Date : 06-Dec-2018
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Insurance"></param>
        /// <returns></returns>
        public ActionResult InsuranceDownload(string Id, bool Insurance)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var Doc = _IVendorManagement.GetDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(Doc.InsuranceDocument))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + DocPath.Replace("~", "");
                        RootDirectory = RootDirectory + DocPath.Replace("~", "") + Doc.InsuranceDocument;
                        if (Directory.GetFiles(IsFileExist, Doc.InsuranceDocument).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Doc.InsuranceDocument);
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
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Dec-2018
        /// Created For : To View Insurance 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddInsurace(string id, long VendorID)
        {
            eTracLoginModel ObjLoginModel = null;
            var obj = new VendorSetupManagementModel();
            var ObjVendorInsuranceModel = new VendorInsuranceModel();
            ViewBag.IsInsurance = true;
            long _id = 0;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    long.TryParse(id, out _id);
                }
                if (VendorID > 0)
                {
                    var vendor_Id = Cryptography.GetEncryptedData(VendorID.ToString(), true);
                    ViewBag.VendorIdForCancel = vendor_Id;
                    ViewBag.VendorStatus = true;
                }
                if (_id > 0)
                {
                    var details = _IVendorManagement.GetInsuranceLicenseCompanyDetails(_id, "Insurance");
                    obj.VendorId = VendorID;
                    obj.VendorInsuranceModel.InsuranceID = _id;

                    return View("AddNewInsuranceAndLicenseVendor", obj);
                }
                else
                {
                    return View("AddNewInsuranceAndLicenseVendor", obj);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Dec-2018
        /// Created For : To Viw License.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddLicense(string id, long VendorID)
        {
            eTracLoginModel ObjLoginModel = null;
            var obj = new VendorSetupManagementModel();
            var objVendorInsuranceModel = new VendorInsuranceModel();
            long _id = 0;
            ViewBag.IsInsurance = false;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    long.TryParse(id, out _id);
                }
                if (VendorID > 0)
                {
                    var vendor_Id = Cryptography.GetEncryptedData(VendorID.ToString(), true);
                    ViewBag.VendorIdForCancel = vendor_Id;
                    ViewBag.VendorStatus = true;
                }
                if (_id > 0)
                {
                    var details = _IVendorManagement.GetInsuranceLicenseCompanyDetails(_id, "License");

                    obj.VendorId = VendorID;
                    objVendorInsuranceModel.LicenseId = _id;
                    obj.VendorInsuranceModel = details;

                    return View("AddNewInsuranceAndLicenseVendor", obj);
                }
                else
                {
                    return View("AddNewInsuranceAndLicenseVendor", obj);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Dec-2018
        /// Created For: To Activate Insurance/License
        /// </summary>
        /// <param name="InsuranceLicenseId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveInsurance(string InsuranceLicenseId, string IsActive, string IsInsuranceLicense)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                bool result = false;
                long _InsuranceLicenseId = 0;
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
                if (!string.IsNullOrEmpty(InsuranceLicenseId))
                {
                    InsuranceLicenseId = Cryptography.GetDecryptedData(InsuranceLicenseId, true);
                    long.TryParse(InsuranceLicenseId, out _InsuranceLicenseId);
                }
                result = _IVendorManagement.ActiveInsuranceLicenseById(_InsuranceLicenseId, UserId, IsActive, IsInsuranceLicense);
                if (result == true)
                {
                    string Message = "";
                    if (IsActive == "Y")
                    {
                        Message = IsInsuranceLicense + " is Activeted.";
                    }
                    else
                    {
                        Message = IsInsuranceLicense + " is Deactiveted.";
                    }
                    ViewBag.Message = Message;
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating payment mode.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To view list of account of vendor.
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public ActionResult ListAccountOfVendor(string VendorId)
        {
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                objLoginSession = (eTracLoginModel)Session["eTrac"];
                if (!string.IsNullOrEmpty(VendorId))
                {
                    VendorId = Cryptography.GetDecryptedData(VendorId, true);
                    long _VendorId = 0;
                    long.TryParse(VendorId, out _VendorId);
                    ViewBag.VendorId = _VendorId;
                    //ViewBag.VendorIdForCancel = VendorId;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To get all Account by vendor Id.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="VendorId"></param>
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
        public JsonResult ListAccounts(string _search, long? VendorId, long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            
            try
            {
                var AllAccountsList = _IVendorManagement.GetAllAccountsDataList(VendorId, LocationId, rows, TotalRecords, sidx, sord);
                return Json(AllAccountsList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
          
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To Active/Deactive account by vendor Id.
        /// </summary>
        /// <param name="AccountsId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveAccounts(string AccountsId, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                bool result = false;
                long _AccountsId = 0;
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
                if (!string.IsNullOrEmpty(AccountsId))
                {
                    AccountsId = Cryptography.GetDecryptedData(AccountsId, true);
                    long.TryParse(AccountsId, out _AccountsId);
                }
                result = _IVendorManagement.ActiveAccountsById(_AccountsId, UserId, IsActive);
                if (result == true)
                {
                    string Message = "";
                    if (IsActive == "Y")
                    {
                        Message = "Account is Activeted.";
                    }
                    else
                    {
                        Message = "Account is Deactiveted.";
                    }
                    ViewBag.Message = Message;
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "Error while activating payment mode.";
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To download document of account.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Insurance"></param>
        /// <returns></returns>
        public ActionResult AccountDocDownload(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var Doc = _IVendorManagement.GetAccountDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(Doc.AccountDocuments))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + DocAccountPath.Replace("~", "");
                        RootDirectory = RootDirectory + DocAccountPath.Replace("~", "") + Doc.AccountDocuments;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, Doc.AccountDocuments).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Doc.AccountDocuments);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + DocAccountPath.Replace("~", "") + "FileNotFound.png";
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
        /// Created Date : 25-Feb-2019
        /// Created For : To view un Approved Vendor
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UnApprovedVendor()
        {
            try
            {
                eTracLoginModel objLoginSession = new eTracLoginModel();
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-Feb-2019
        /// Created For : To get all Un Approved vendor 
        /// </summary>
        /// <param name="_search"></param>
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
        public JsonResult GetAllUnApprovedVendorList(string _search,  long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            LocationId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); 
                UserId = ObjLoginModel.UserId;
                //LocationId = ObjLoginModel.LocationID;
            }  
            try
              {
                
                if (_search != null && _search != "")
                {
                 var  AllCompanyListForSearch = _IVendorManagement.GetAllCompanyDataList1(LocationId, rows, TotalRecords, sidx, sord).Where(x => x.CompanyNameLegal.ToLower() == _search.ToLower().Trim()).ToList();
                    return Json(AllCompanyListForSearch.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var AllCompanyList = _IVendorManagement.GetAllCompanyDataList1(LocationId, rows, TotalRecords, sidx, sord);
                    return Json(AllCompanyList.ToList(), JsonRequestBehavior.AllowGet);
                }
               
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
           
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 05-March-2019
        /// Created For : To View all facility 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddFacilityDetails(string id)
        {
            eTracLoginModel ObjLoginModel = null;
            var obj = new VendorFacilityModel();
            var iid = id;
            long _id = 0;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(id))
                {
                    id = Cryptography.GetDecryptedData(id, true);
                    long.TryParse(id, out _id);
                }
                if (_id > 0)
                {
                    var details = _IVendorManagement.GetVendorDetailsByVendorId(_id);
                    ViewBag.CostCodeList = _IVendorManagement.ListAllCostCode();
                    obj.VendorId = _id;
                    ViewBag.VendorId = _id;
                    ViewBag.VendorName = details.CompanyNameLegal;
                    ViewBag.VendorIdForCancel = iid;
                    return View("AddVendorFacility", obj);
                }
                else
                {
                    return View("CompanyList", obj);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("CompanyList");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 5-March-2019
        /// Created For : To get all Facility list for vendor By VendorId
        /// </summary>
        /// <param name="_search"></param>
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
        public JsonResult ListCompanyFacilityByVendorId(string _search, long? VendorId, long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
       
            try
            {
                
                var AllFacilityList = _IVendorManagement.GetFacilityListCompanyDetails(VendorId, LocationId, rows, TotalRecords, sidx, sord).ToList();
                return Json(AllFacilityList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 05-March-2019
        /// Created For : To Save Vendor Facility
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveVendorFacility(VendorFacilityModel obj)
        {
            var model = new VendorFacilityModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (obj.LocationId == null)
                    {
                        obj.LocationId = ObjLoginModel.LocationID;
                    }
                    obj.UserId = ObjLoginModel.UserId;
                }
                if (obj != null)
                {
                    var details = _IVendorManagement.GetVendorDetailsByVendorId(obj.VendorId);
                    obj.VendorName = details.CompanyNameLegal;
                    string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + obj.ProductImageFile.FileName.ToString();
                    CommonHelper obj_CommonHelper = new CommonHelper();
                    var res = obj_CommonHelper.UploadImage(obj.ProductImageFile, Server.MapPath(ConfigurationManager.AppSettings["VendorImageFacility"]), ImageName);
                    ViewBag.ImageUrl = res;
                    obj.VenderProductImageName = ImageName;
                    var saveFacility = _IVendorManagement.SaveFacilityDetails(obj);
                    if (saveFacility == true)
                    {
                        ViewBag.Message = CommonMessage.Successful();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }
                    ViewBag.CostCodeList = _IVendorManagement.ListAllCostCode();
                    ViewBag.VendorId = obj.VendorId;
                    ViewBag.VendorName = details.CompanyNameLegal;
                    if (obj.VendorId > 0)
                    {
                        var id = Cryptography.GetEncryptedData(Convert.ToString(obj.VendorId), true);
                        ViewBag.VendorIdForCancel = id;
                    }

                    ModelState.Clear();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("AddVendorFacility", model);
        }

        public ActionResult SaveContractAllocation(ContractLocationAllocation obj)
        {
            try
            {
                if (obj != null)
                {
                    var saveFacility = _IVendorManagement.SaveContractAllocation(obj);
                }
                    return Json("");
            }
            catch (Exception ex)
            {
                throw;
            }            
        }
    }
}