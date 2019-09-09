using Intuit.Ipp.OAuth2PlatformClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class Callback1Controller : Controller
    {
        string sub = "";
        string email = "";
        string emailVerified = "";
        string givenName = "";
        string familyName = "";
        string phoneNumber = "";
        string phoneNumberVerified = "";
        string streetAddress = "";
        string locality = "";
        string region = "";
        string postalCode = "";
        string country = "";
        public static string RealMId = "";
        public static string AccessToken = "";

        public static long UserRoleId = 0;
        public static long UserId = 0;
        public static string clientid = ConfigurationManager.AppSettings["clientid"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        public static string environment = ConfigurationManager.AppSettings["appEnvironment"];
        IdTokenJWTClaimTypes payloadData;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly ILogin _ILogin;
        private readonly ICommonMethod _ICommonMethod;
        public Callback1Controller(ILogin _ILogin, ICommonMethod ICommonMethod)
        {
            this._ILogin = _ILogin;
            this._ICommonMethod = ICommonMethod;
        }
        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);


        // GET: Callback1
        /// <summary>
        /// Code and realmid/company id recieved on Index page after redirect is complete from Authorization url
        /// </summary>
        public async Task<ActionResult> Index()
        {
            try
            {
                //Sync the state info and update if it is not the same
                ViewBag.Code = Request.QueryString["code"] ?? "none";
                ViewBag.RealmId = Request.QueryString["realmId"] ?? "none";
                var state = Request.QueryString["state"];
                if (state.Equals(App1Controller.auth2Client.CSRFToken, StringComparison.Ordinal))
                {
                    ViewBag.State = state + " (valid)";
                }
                else
                {
                    ViewBag.State = state + " (invalid)";
                }

                string code = Request.QueryString["code"] ?? "none";
                string realmId = Request.QueryString["realmId"] ?? "none";
                if (realmId != null)
                {
                    Session["realmId"] = realmId;
                    RealMId = realmId;
                }
                await GetAuthTokensAsync(code, realmId);

                ViewBag.Error = Request.QueryString["error"] ?? "none";

                ViewBag.QuickBookMessage = "QuickBook is now Connected";
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
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
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
            return View();
        }

        /// <summary>
        /// Exchange Auth code with Auth Access and Refresh tokens and add them to Claim list
        /// </summary>
        private async Task GetAuthTokensAsync(string code, string realmId)
        {
            if (realmId != null)
            {
                Session["realmId"] = realmId;
            }
            if (code != null)
            {
                Session["code"] = code;
            }
            //Request.GetOwinContext().Authentication.SignOut("TempState");
            var tokenResponse = await App1Controller.auth2Client.GetBearerTokenAsync(code);

            var claims = new List<Claim>();

            if (Session["realmId"] != null)
            {
                claims.Add(new Claim("realmId", Session["realmId"].ToString()));
            }

            if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                Session["access_token"] = tokenResponse.AccessToken;
                AccessToken = tokenResponse.AccessToken;
                claims.Add(new Claim("access_token", tokenResponse.AccessToken));
                claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn)).ToString()));
            }

            if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
            {
                Session["refresh_token"] = tokenResponse.RefreshToken;
                claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
                claims.Add(new Claim("refresh_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn)).ToString()));
            }
            var id = new ClaimsIdentity(claims, "Cookies");
            //Request.GetOwinContext().Authentication.SignIn(id);
        }
    }
}