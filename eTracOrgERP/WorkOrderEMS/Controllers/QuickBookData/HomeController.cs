using Intuit.Ipp.OAuth2PlatformClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class HomeController : Controller
    {
        DiscoveryClient discoveryClient;
        DiscoveryResponse doc;
        AuthorizeRequest request;
        public static IList<JsonWebKey> keys;
        public static string scope;
        public static string authorizeUrl;
        public static string authorizeUrl2;
        public static long UserRoleId = 0;
        public static long UserId = 0;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly ILogin _ILogin;
        private readonly ICommonMethod _ICommonMethod;
        public static string clientid = ConfigurationManager.AppSettings["clientid"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];
        public static OAuth2Client auth2Client = new OAuth2Client(clientid, clientsecret, redirectUrl, environment);
        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);
        public HomeController(ILogin _ILogin, ICommonMethod ICommonMethod)
        {
            this._ILogin = _ILogin;
            this._ICommonMethod = ICommonMethod;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                if (authorizeUrl == null)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Request.GetOwinContext().Authentication.SignOut("Cookies");
                    //Intialize DiscoverPolicy
                    DiscoveryPolicy dpolicy = new DiscoveryPolicy();
                    dpolicy.RequireHttps = true;
                    dpolicy.ValidateIssuerName = true;

                    //Assign the Sandbox Discovery url for the Apps' Dev clientid and clientsecret that you use
                    //Or
                    //Assign the Production Discovery url for the Apps' Production clientid and clientsecret that you use

                    string discoveryUrl = ConfigurationManager.AppSettings["DiscoveryUrl"];
                    if (discoveryUrl != null && AppController.clientid != null && AppController.clientsecret != null)
                    {
                        discoveryClient = new DiscoveryClient(discoveryUrl);
                    }
                    else
                    {
                        Exception ex = new Exception("Discovery Url missing!");
                        throw ex;
                    }
                    doc = await discoveryClient.GetAsync();
                    if (doc.StatusCode == HttpStatusCode.OK)
                    {
                        //Authorize endpoint
                        AppController.authorizeUrl = doc.AuthorizeEndpoint;
                        //Token endpoint
                        AppController.tokenEndpoint = doc.TokenEndpoint;
                        //Token Revocation enpoint
                        AppController.revocationEndpoint = doc.RevocationEndpoint;
                        //UserInfo endpoint
                        AppController.userinfoEndpoint = doc.UserInfoEndpoint;
                        //Issuer endpoint
                        AppController.issuerEndpoint = doc.Issuer;
                        //JWKS Keys
                        AppController.keys = doc.KeySet.Keys;
                    }

                    //Get mod and exponent value
                    foreach (var key in AppController.keys)
                    {
                        if (key.N != null)
                        {
                            //Mod
                            AppController.mod = key.N;
                        }
                        if (key.E != null)
                        {
                            //Exponent
                            AppController.expo = key.E;
                        }
                    }
                    List<OidcScopes> scopes = new List<OidcScopes>();
                    scopes.Add(OidcScopes.Accounting);
                    string authorizeUrl = auth2Client.GetAuthorizationURL(scopes);

                    Session["authorizeUrl"] = authorizeUrl;
                    authorizeUrl2 = authorizeUrl;
                    ViewBag.IsConnected = true;
                    //scope = OidcScopes.Accounting.GetStringValue() + " " + OidcScopes.Payment.GetStringValue();
                    //authorizeUrl = GetAuthorizeUrl(scope);
                    //Session["authorizeUrl"] = authorizeUrl;
                    //ViewBag.IsConnected =  true ;
                    
                    // perform the redirect here.
                    return Redirect(authorizeUrl);
                }
                else
                {
                    ViewBag.Message = "Already connected to quickbook please proceed.";
                    ViewBag.IsConnected = false; 
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                    Session["authorizeUrl"] = authorizeUrl;
                    authorizeUrl2 = authorizeUrl;
                    switch (UserRoleId)
                    {
                        case ((Int64)(UserType.GlobalAdmin)):
                            Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(UserRoleId, UserId);
                            Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"]; // this line has been added by vijay bcz if usetype is GAdmin or ITAdmin then this type of users will be able too see all services which is assigned to this current location.
                                                                                            // QuickBookIndex();
                            return RedirectToAction("Index", "GlobalAdmin");
                            break;
                        case ((Int64)(UserType.ITAdministrator)):
                            Session["eTrac_UserLocations"] = _ILogin.GetUserAssignedLocations(UserRoleId, UserId);
                            Session["eTrac_UserRoles"] = Session["eTrac_LocationServices"];
                            //QuickBookIndex();
                            return RedirectToAction("Index", "ITAdministrator");
                            break;
                        case ((Int64)(UserType.Administrator)):
                            Session["eTrac_UserLocations"] = _ILogin.GetAdminAssignedLocation(UserId);
                            // QuickBookIndex();
                            return RedirectToAction("Index", "Administrator");
                            break;
                        case ((Int64)(UserType.Manager)):
                            Session["eTrac_UserLocations"] = _ILogin.GetManagerAssignedLocation(UserId);

                            #region this code will execute only when manager declined vendor from vendor email verification page.
                            try
                            {
                                if (Request.Cookies["eTrack_VendorIdForEditAfterDeclinedByManager"] != null)
                                {
                                    string isVendorIDExists = Request.Cookies["eTrack_VendorIdForEditAfterDeclinedByManager"]["VendorID"];
                                    if (isVendorIDExists != null)
                                    {
                                        var abc = Cryptography.GetDecryptedData(isVendorIDExists, true);

                                        if (Convert.ToInt32(abc) > 0)
                                        {
                                            //string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                                            //var adfadsf = HostingPrefix + "/Manager/EditRegisterVendor/?vdr=" + isVendorIDExists;
                                            //Response.Redirect(adfadsf);
                                            // QuickBookIndex();
                                            return RedirectToAction("EditRegisterVendor", "Manager", new { vdr = isVendorIDExists });
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            #endregion // by vijay sahu on 2 july 2015

                            #region This Code Will Execute if Vehicle Declined by Manager and after login redirect to edit vehicle
                            try
                            {
                                if (Request.Cookies["eTrac_VehicleIdForEditAfterDeclinedByManager"] != null)
                                {
                                    string isVehicleIDExists = Request.Cookies["eTrac_VehicleIdForEditAfterDeclinedByManager"]["QRCID"];
                                    if (isVehicleIDExists != null)
                                    {
                                        var abc = Cryptography.GetDecryptedData(isVehicleIDExists, true);

                                        if (Convert.ToInt32(abc) > 0)
                                        {
                                            //string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], System.Globalization.CultureInfo.InvariantCulture);
                                            var redirectURL = HostingPrefix + "QRCSetup/VehicleRegistration/?qr=" + isVehicleIDExists;
                                            Response.Redirect(redirectURL);
                                            //return RedirectToAction("VehicleRegistration", "QRCSetup", new { qr = isVehicleIDExists });
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                            #endregion // by Bhushan Dod on 22 September 2015
                            //QuickBookIndex();
                            return RedirectToAction("Dashboard", "Manager");
                            break;
                        case ((Int64)(UserType.Employee)):
                            Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(UserId);
                            //QuickBookIndex();
                            return RedirectToAction("Index", "Employee");
                            break;
                        case ((Int64)(UserType.Client)):
                            //Session["eTrac_UserLocations"] = _ILogin.GetEmployeeAssignedLocation(result.UserId);
                            //QuickBookIndex();
                            return RedirectToAction("Index", "Client");
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
            
            return View();
        }


        public ActionResult MyAction(string submitButton)
        {
            try
            {
                switch (submitButton)
                {
                    case "C2QB":
                        // delegate sending to C2QB Action
                        return (C2QB());
                    case "GetAppNow":
                        // call another action to GetAppNow
                        return (GetAppNow());
                    case "SIWI":
                        // call another action to SIWI
                        return (SIWI());
                    default:
                        // If they've submitted the form without a submitButton, 
                        // just return the view again.
                        return (View());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        private ActionResult C2QB()
        {
            try
            { 
                scope = OidcScopes.Accounting.GetStringValue() + " " + OidcScopes.Payment.GetStringValue();
                authorizeUrl = GetAuthorizeUrl(scope);
                // perform the redirect here.
                return Redirect(authorizeUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        private ActionResult GetAppNow()
        {
            try
            {
                scope = OidcScopes.Accounting.GetStringValue() + " " + OidcScopes.Payment.GetStringValue() + " " + OidcScopes.OpenId.GetStringValue() + " " + OidcScopes.Address.GetStringValue()
                     + " " + OidcScopes.Email.GetStringValue() + " " + OidcScopes.Phone.GetStringValue()
                     + " " + OidcScopes.Profile.GetStringValue();
                authorizeUrl = GetAuthorizeUrl(scope);
                // perform the redirect here.
                return Redirect(authorizeUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        private ActionResult SIWI()
        {
            try
            {
                scope = OidcScopes.OpenId.GetStringValue() + " " + OidcScopes.Address.GetStringValue()
                     + " " + OidcScopes.Email.GetStringValue() + " " + OidcScopes.Phone.GetStringValue()
                     + " " + OidcScopes.Profile.GetStringValue();
                authorizeUrl = GetAuthorizeUrl(scope);
                // perform the redirect here.
                return Redirect(authorizeUrl);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }


        private void SetTempState(string state)
        {
            try
            {
                var tempId = new ClaimsIdentity("TempState");
                tempId.AddClaim(new Claim("state", state));
                Request.GetOwinContext().Authentication.SignIn(tempId);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }

        private string GetAuthorizeUrl(string scope)
        {
            try
            {
                var state = Guid.NewGuid().ToString("N");
                SetTempState(state);
                //Make Authorization request
                var request = new AuthorizeRequest(AppController.authorizeUrl);

                string url = request.CreateAuthorizeUrl(
                   clientId: AppController.clientid,
                   responseType: OidcConstants.AuthorizeResponse.Code,
                   scope: scope,
                   redirectUri: AppController.redirectUrl,
                   state: state);
                return url;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
        }
    }
}