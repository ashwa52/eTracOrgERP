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
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult CustomerList()
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
                    Customer customer = new Customer();

                    QueryService<Customer> querySvc = new QueryService<Customer>(serviceContext);
                    List<Customer> customers = querySvc.ExecuteIdsQuery("SELECT * FROM Customer").ToList();
                    QueryService<CompanyInfo> querySvcCompany = new QueryService<CompanyInfo>(serviceContext);
                    List<CompanyInfo> companyInfo = querySvcCompany.ExecuteIdsQuery("SELECT * FROM CompanyInfo").ToList();
                    string output = "";
                    var customerData = new List<CustomerModel>();
                    customerData = customers.Select(x => new CustomerModel()
                    {
                        CustomerId = Convert.ToInt64(x.Id),
                        Title = x.Title,
                        FirstName = x.GivenName,
                        MiddleName = x.MiddleName,
                        LastName = x.FamilyName
                    }).ToList();
                    return View("CustomerList", customerData);
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