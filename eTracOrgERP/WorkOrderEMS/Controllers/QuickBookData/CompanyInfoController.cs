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

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class CompanyInfoController : Controller
    {
        // GET: CompanyInfo
        public ActionResult CompanyDetails()
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
                    QueryService<CompanyInfo> querySvc = new QueryService<CompanyInfo>(serviceContext);
                    CompanyInfo companyInfo = querySvc.ExecuteIdsQuery("SELECT * FROM CompanyInfo").FirstOrDefault();
                    //companyInfo.
                    string output = "Company Name: " + companyInfo.CompanyName + " Company Address: " + companyInfo.CompanyAddr.Line1 + ", " + companyInfo.CompanyAddr.City + ", " + companyInfo.CompanyAddr.Country + " " + companyInfo.CompanyAddr.PostalCode;


                    return View("CompanyDetails", (object)("QBO API call Successful!! Response: " + output));

                }
                catch (Exception ex)
                {
                    return View("CompanyDetails", (object)("QBO API call Failed!" + " Error message: " + ex.Message));
                }
            }
            else
                return View("CompanyDetails", (object)"QBO API call Failed!");
        }
    }
}