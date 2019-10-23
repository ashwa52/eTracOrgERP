using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class VendorQuickBookController : Controller
    {
        // GET: VendorQuickBook
        public ActionResult VendorList()
        {
            if (Session["realmId"] != null)
            {
                string realmId = Session["realmId"].ToString();
                try
                {
                    string AccessToken = Session["access_token"].ToString();
                    if (AccessToken == null)
                    {
                        AccessToken = Session["refresh_token"].ToString();
                    }
                    var principal = User as ClaimsPrincipal;
                    OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                    // Create a ServiceContext with Auth tokens and realmId
                    ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                    serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                    DataService commonServiceQBO = new DataService(serviceContext);
                    // Create a QuickBooks QueryService using ServiceContext
                    Vendor vendor = new Vendor();
                    
                    QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                    List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor").ToList();
                    QueryService<CompanyInfo> querySvcCompany = new QueryService<CompanyInfo>(serviceContext);
                    List<CompanyInfo> companyInfo = querySvcCompany.ExecuteIdsQuery("SELECT * FROM CompanyInfo").ToList();
                    string output = "";
                    var vendorData = new List<VendorModel>();
                    vendorData = vendorList.Select(x => new VendorModel()
                    {
                        VendorId = Convert.ToInt64(x.Id),
                        Title = x.Title,
                        FirstName = x.GivenName,
                        MiddleName = x.MiddleName,
                        LastName = x.FamilyName,
                    }).OrderByDescending(x => x.VendorId).ToList();


                    return View("VendorList", vendorData);
                }
                catch (Exception ex)
                {
                    return View("ApiCallService", (object)("QBO API call Failed!" + " Error message: " + ex.Message));
                }
            }
            else
                return View("ApiCallService", (object)"QBO API call Failed!");
        }

        public ActionResult AddVendor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddVendor(VendorModel objVendorModel)
        {
            if (Session["realmId"] != null)
            {
                string realmId = Session["realmId"].ToString();
                try
                {
                    string AccessToken = Session["access_token"].ToString();
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
                    Vendor vendor = new Vendor();
                    //Mandatory Fields
                    vendor.GivenName = objVendorModel.FirstName;
                    vendor.Title = objVendorModel.Title;
                    vendor.MiddleName = objVendorModel.MiddleName;
                    vendor.FamilyName = objVendorModel.LastName;
                    vendor.AcctNum = objVendorModel.AccountNo;

                    vendor.Balance = Convert.ToDecimal(objVendorModel.BalanceAmount);
                    vendor.CompanyName = objVendorModel.Company;

                    vendorAddr.City = objVendorModel.City;
                    vendorAddr.Country = objVendorModel.Country;
                    vendoremail.Address = objVendorModel.Email;
                    mobileNumber.FreeFormNumber = objVendorModel.MobileNumber;
                    websiteaddr.URI = objVendorModel.Website;

                    vendor.BillAddr = vendorAddr;
                    vendor.PrimaryEmailAddr = vendoremail;
                    vendor.Mobile = mobileNumber;
                    vendor.WebAddr = websiteaddr;
                    Vendor resultVendor = commonServiceQBO.Add(vendor) as Vendor;
                    QueryService<Vendor> querySvcVendor = new QueryService<Vendor>(serviceContext);
                    List<Vendor> vendorListData = querySvcVendor.ExecuteIdsQuery("SELECT * FROM Vendor").OrderByDescending(x => x.Id).ToList();
                    var tt = vendorListData.FirstOrDefault();
                    Vendor resultCustomer = commonServiceQBO.FindById(tt) as Vendor;
                    return RedirectToAction("VendorList","VendorQuickBook");
                }
                catch (Exception ex)
                {
                    return View("ApiCallService", (object)("QBO API call Failed!" + " Error message: " + ex.Message));
                }
            }
            else
                return View("ApiCallService", (object)"QBO API call Failed!");
        }
    }
}