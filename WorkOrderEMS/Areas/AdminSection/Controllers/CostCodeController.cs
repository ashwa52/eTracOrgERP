using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class CostCodeController : Controller
    {
        // GET: AdminSection/CostCode
        private readonly ICostCode _ICostCode;
        public CostCodeController(ICostCode _ICostCode)
        {
            this._ICostCode = _ICostCode;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public ActionResult CostCodeAndRule()
        {
            ViewBag.AccountSection = true;
            return View("~/Areas/AdminSection/Views/CostCode/CostCodeandRules.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18 July 2018
        /// Created For : To Save Cost Code 
        /// </summary>
        /// <param name="objCostCodeModel"></param>
        /// <returns></returns>
        public ActionResult SaveCostCode(CostCodeModel objCostCodeModel)
        {
            eTracLoginModel ObjLoginModel = null;
            workorderEMSEntities _workorderems = new workorderEMSEntities();
            ViewBag.AccountSection = true;
            bool isUpdate = false;
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
            try
            {
                if (objCostCodeModel.CostCodeId == 0)  //Save cost code
                {
                    objCostCodeModel.CreatedBy = ObjLoginModel.UserId;
                    objCostCodeModel.CreatedDate = DateTime.UtcNow;
                    objCostCodeModel.IsDeleted = false;
                    objCostCodeModel.LocationId = ObjLoginModel.LocationID;
                    var SubaccountCostCode = new Account();
                    var accountCostCode = new Account();
                    SubaccountCostCode = null; accountCostCode = null;
                    var data = new Account();
                    string realmId = CallbackController.RealMId.ToString();//Session["realmId"].ToString();
                    if (realmId != null)
                    {
                        try
                        {
                            string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                            string refreshToken = CallbackController.RefreshToken;
                            var principal = User as ClaimsPrincipal;
                            OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);
                            //OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(principal.FindFirst("refresh_token").Value);
                            //OAuthRequestValidator oauthValidator = new OAuthRequestValidator(refreshToken);

                            // Create a ServiceContext with Auth tokens and realmId
                            ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                            serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                            DataService commonServiceQBO = new DataService(serviceContext);
                            // Create a QuickBooks QueryService using ServiceContext
                            Account account = new Account();

                            QueryService<Account> querySvcCompany = new QueryService<Account>(serviceContext);
                            List<Account> listAccount = querySvcCompany.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000")
                                .ToList();
                            ReferenceType parentReference = new ReferenceType();
                            if (objCostCodeModel.CostCode != null)
                            {
                                var getMasterData = _workorderems.CostCodeMasters.Where(x => x.CCM_CostCode == objCostCodeModel.CostCode).FirstOrDefault();
                                QueryService<Account> querySvcAcc = new QueryService<Account>(serviceContext);
                                Account AccountInfo = querySvcAcc.ExecuteIdsQuery("SELECT * FROM Account").FirstOrDefault();
                                data = listAccount.Where(x => x.Name == getMasterData.CCM_Description).FirstOrDefault();

                                account.SubAccount = true;
                                parentReference.name = getMasterData.CCM_Description;
                                parentReference.Value = data.Id;
                                //account.AccountSubType = "AccountsPayable";
                                account.AccountSubType = objCostCodeModel.CatagoryValue;
                                //account.AccountType = AccountTypeEnum.Expense;
                                account.Name = objCostCodeModel.Description;
                                account.Active = true;
                                account.AccountAlias = objCostCodeModel.Description;
                                account.Description = objCostCodeModel.Description;
                                //parentReference.Value = 
                                account.ParentRef = parentReference;
                                SubaccountCostCode = commonServiceQBO.Add(account) as Account;
                            }
                            else
                            {
                                account.Active = true;
                                account.AccountAlias = objCostCodeModel.Description;
                                account.Description = objCostCodeModel.Description;
                                account.FullyQualifiedName = objCostCodeModel.Description;
                                account.Name = objCostCodeModel.Description;

                                switch (objCostCodeModel.CategoryList)
                                {
                                    case "Accounts Payable":
                                        account.AccountType = AccountTypeEnum.AccountsPayable;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Accounts Receivable":
                                        account.AccountType = AccountTypeEnum.AccountsReceivable;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Bank":
                                        account.AccountType = AccountTypeEnum.Bank;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Cost of Goods Sold":
                                        account.AccountType = AccountTypeEnum.CostofGoodsSold;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Credit Card":
                                        account.AccountType = AccountTypeEnum.CreditCard;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Equity":
                                        account.AccountType = AccountTypeEnum.Equity;
                                        account.Classification = AccountClassificationEnum.Equity;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Expense":
                                        account.AccountType = AccountTypeEnum.Expense;
                                        account.Classification = AccountClassificationEnum.Expense;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Fixed Asset":
                                        account.AccountType = AccountTypeEnum.FixedAsset;
                                        account.Classification = AccountClassificationEnum.Asset;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Income":
                                        account.AccountType = AccountTypeEnum.Income;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Long Term Liability":
                                        account.AccountType = AccountTypeEnum.LongTermLiability;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Other Current Asset":
                                        account.AccountType = AccountTypeEnum.OtherCurrentAsset;
                                        account.Classification = AccountClassificationEnum.Asset;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Other Current Liability":
                                        account.AccountType = AccountTypeEnum.OtherCurrentLiability;
                                        account.Classification = AccountClassificationEnum.Liability;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Other Expense":
                                        account.AccountType = AccountTypeEnum.OtherExpense;
                                        account.Classification = AccountClassificationEnum.Expense;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;
                                    case "Other Income":
                                        account.AccountType = AccountTypeEnum.OtherIncome;
                                        account.Classification = AccountClassificationEnum.Revenue;
                                        account.AccountSubType = objCostCodeModel.CatagoryValue;
                                        break;

                                    default:
                                        break;
                                }
                                account.SubAccount = false;
                                account.AccountTypeSpecified = true;
                                account.sparse = false;
                                accountCostCode = commonServiceQBO.Add(account) as Account;
                            }

                        }
                        catch (Exception ex)
                        {
                            ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        }
                        //}
                        if (accountCostCode != null)
                        {
                            objCostCodeModel.QuickBookCostCodeMasterId = Convert.ToInt64(accountCostCode.Id);
                        }
                        if (SubaccountCostCode != null)
                        {
                            objCostCodeModel.QuickBookCostCodeId = Convert.ToInt64(SubaccountCostCode.Id);
                            objCostCodeModel.QuickBookCostCodeMasterId = Convert.ToInt64(data.Id);
                        }
                        var SaveCostCodeData = _ICostCode.SaveCostCode(objCostCodeModel);
                        if (objCostCodeModel.Result == Result.Completed)
                        {
                            ModelState.Clear();
                            ViewBag.Message = CommonMessage.CostCodeSaved();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        }
                    }
                    else  //edit cost code data update
                    {
                        isUpdate = true;
                        objCostCodeModel.ModifiedBy = ObjLoginModel.UserId;
                        objCostCodeModel.ModifiedDate = DateTime.UtcNow;
                        objCostCodeModel.LocationId = ObjLoginModel.LocationID;
                        var Data = _ICostCode.SaveCostCode(objCostCodeModel);
                        if (objCostCodeModel.Result == Result.UpdatedSuccessfully)
                        {
                            ModelState.Clear();
                            ViewBag.Message = CommonMessage.CostCodeSaved();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }

            finally
            {
                ViewBag.CategoryList = _ICostCode.GetCategoryList();
            }
            return View("~/Areas/AdminSection/Views/CostCode/CostCodeList.cshtml");
            //return View("CostCodeList");
        }
        
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July-17-2018
        /// Created For : To show List of Cost code list
        /// </summary>
        /// <returns></returns>
        public ActionResult ListCostCode()
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                //var returnModel = new eFleetDriverModel();
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
                ViewBag.CategoryList = _ICostCode.GetCategoryList();
                return View("~/Areas/AdminSection/Views/CostCode/CostCodeList.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("~/Areas/AdminSection/Views/CostCode/CostCodeList.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July-17-2018
        /// Created For : To fetch Cost data from database and display in JQ Grid.
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
        public JsonResult GetListCostCode(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            CostCodeModel objCostCodeModel = new CostCodeModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var costCodeList = _ICostCode.GetListCostCode(UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var costCode in costCodeList.rows)
                {
                    if (costCode.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(costCode.CostCode), true);
                        row.cell = new string[2];
                        row.cell[0] = costCode.CostCode.ToString();
                        row.cell[1] = costCode.Description;
                       // row.cell[2] = costCode.IsActive;
                        rowss.Add(row);
                    }
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
        /// Created Date : 20 July 2018
        /// Created For : To fetch sub cost code data as per Master cost code Id
        /// </summary>
        /// <param name="CostCodeId"></param>
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
        public JsonResult GetListOfSubCostCode(string id, string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            long _CostCodeId = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            if (!string.IsNullOrEmpty(id))
            {
                id = Cryptography.GetDecryptedData(id, true);
                long.TryParse(id, out _CostCodeId);
            }
            CostCodeModel objCostCodeModel = new CostCodeModel();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var costCodeList = _ICostCode.GetListOfSubCostCode(_CostCodeId, UserId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var costCode in costCodeList.rows)
                {
                    if (costCode.IsDeleted == false)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(costCode.CCM_CostCode), true);
                        row.cell = new string[3];
                        row.cell[0] = costCode.CCM_CostCode.ToString();
                        row.cell[1] = costCode.Description;
                        row.cell[2] = costCode.IsActive == "Y"?"Active":"Not Activated";
                        rowss.Add(row);
                    }
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
        /// Created Date : 19-Nov-2018
        /// Created For : To active CostCode
        /// </summary>
        /// <param name="CostCodeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActiveCostCode(long CostCodeId, string IsActive)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                ViewBag.AccountSection = true;
                bool result = false;
                string returnString;
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
                //if (!string.IsNullOrEmpty(CostCodeId))
                //{
                //    CostCodeId = Cryptography.GetDecryptedData(CostCodeId, true);
                //    long.TryParse(CostCodeId, out _CostCodeId);
                //}
                result = _ICostCode.ActiveCostCodeById(CostCodeId, UserId, IsActive);
                if(result == true)
                {
                    if (IsActive == "Y")
                    {
                        returnString = "Cost Code is Activated.";
                        ViewBag.Message = returnString;
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return Json(returnString, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        returnString = "Cost Code is Deactivated.";
                        ViewBag.Message = returnString;
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return Json(returnString, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    returnString = "Error while activating cost code.";
                    ViewBag.Message = returnString;
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    return Json(returnString, JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }            
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 08-Jan-2018
        /// Created For : To get all sub category as per cateoty name
        /// </summary>
        /// <param name="CategoryListName"></param>
        /// <returns></returns>
        public JsonResult GetSubCategoryList(string CategoryListName)
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            objLoginSession = (eTracLoginModel)Session["eTrac"];
            var data = new List<string>();
            try
            {
                if (CategoryListName != null)
                {
                   data = _ICostCode.GetSubCategoryList(CategoryListName);
                }
                else
                {
                    ViewBag.Message = CommonMessage.CategoryNotSelected();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}