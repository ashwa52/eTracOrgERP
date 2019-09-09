using Intuit.Ipp.OAuth2PlatformClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Controllers.QuickBookData
{
    public class CallbackController : Controller
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
        public static string RefreshToken = "";

        public static long UserRoleId = 0;
        public static long UserId = 0;
        public static string clientid = ConfigurationManager.AppSettings["clientid"];
        public static string clientsecret = ConfigurationManager.AppSettings["clientsecret"];
        public static string redirectUrl = ConfigurationManager.AppSettings["redirectUrl"];
        IdTokenJWTClaimTypes payloadData;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        private readonly ILogin _ILogin;
        private readonly ICommonMethod _ICommonMethod;

        public CallbackController(ILogin _ILogin, ICommonMethod ICommonMethod)
        {
            this._ILogin = _ILogin;
            this._ICommonMethod = ICommonMethod;
        }
        string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"]);


        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Code = Request.QueryString["code"] ?? "none";
                ViewBag.RealmId = Request.QueryString["realmId"] ?? "none";
                var state = Request.QueryString["state"];
                //var tempState = await GetTempStateAsync();
                if (state != null)//.Equals(tempState.Item1, StringComparison.Ordinal))
                {
                    ViewBag.State = state + " (valid)";
                }
                else
                {
                    ViewBag.State = state + " (invalid)";
                }

                ViewBag.Error = Request.QueryString["error"] ?? "none";
                var tokenClient = new TokenClient(AppController.tokenEndpoint, AppController.clientid, AppController.clientsecret);
                var code = Request.QueryString["code"];
                var realmId = Request.QueryString["realmId"];
                if (realmId != null)
                {
                    Session["realmId"] = realmId;
                    RealMId = realmId;
                }
                //var tempState = await GetTempStateAsync();
                Request.GetOwinContext().Authentication.SignOut("TempState");
                TokenResponse response = await tokenClient.RequestTokenFromCodeAsync(code, AppController.redirectUrl);
                await ValidateResponseAndSignInAsync(response);


                if (!string.IsNullOrEmpty(response.IdentityToken))
                {
                    ViewBag.IdentityToken = response.IdentityToken;
                }
                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    ViewBag.AccessToken = response.AccessToken;
                    AccessToken = response.AccessToken;
                }
                if (!string.IsNullOrEmpty(response.RefreshToken))
                {
                    ViewBag.RefreshToken = response.RefreshToken;
                    RefreshToken = response.RefreshToken;
                }               
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

        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> Token()
        {
            //Request Oauth2 tokens
            var tokenClient = new TokenClient(AppController.tokenEndpoint, AppController.clientid, AppController.clientsecret);
            var code = Request.QueryString["code"];
            var realmId = Request.QueryString["realmId"];
            if (realmId != null)
            {
                Session["realmId"] = realmId;
            }
            //var tempState = await GetTempStateAsync();
            Request.GetOwinContext().Authentication.SignOut("TempState");
            TokenResponse response = await tokenClient.RequestTokenFromCodeAsync(code, AppController.redirectUrl);
            await ValidateResponseAndSignInAsync(response);
            if (!string.IsNullOrEmpty(response.IdentityToken))
            {
                ViewBag.IdentityToken = response.IdentityToken;
            }
            if (!string.IsNullOrEmpty(response.AccessToken))
            {
                ViewBag.AccessToken = response.AccessToken;
            }
            return RedirectToAction("CustomerList", "Customer");
            //return View("Token", response);
        }

        private async Task ValidateResponseAndSignInAsync(TokenResponse response)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrWhiteSpace(response.IdentityToken))
            {
                bool isIdTokenValid = ValidateToken(response.IdentityToken);//throw error is not valid

                if (isIdTokenValid == true)
                {
                    claims.Add(new Claim("id_token", response.IdentityToken));

                }
            }
            if (Session["realmId"] != null)
            {
                claims.Add(new Claim("realmId", Session["realmId"].ToString()));
            }
            if (!string.IsNullOrWhiteSpace(response.AccessToken))
            {
                //var userClaims = await GetUserInfoClaimsAsync(response.AccessToken);
                //claims.AddRange(userClaims);
                Session["access_token"] = response.AccessToken;
                AccessToken = response.AccessToken;
                claims.Add(new Claim("access_token", response.AccessToken));
                claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(response.AccessTokenExpiresIn)).ToString()));
            }
            if (!string.IsNullOrWhiteSpace(response.RefreshToken))
            {
                Session["refresh_token"] = response.RefreshToken;
                RefreshToken = response.RefreshToken;
                claims.Add(new Claim("refresh_token", response.RefreshToken));
                claims.Add(new Claim("refresh_token_expires_at", (DateTime.Now.AddSeconds(response.RefreshTokenExpiresIn)).ToString()));
            }
            var id = new ClaimsIdentity(claims, "Cookies");
            Request.GetOwinContext().Authentication.SignIn(id);
        }

        private bool ValidateToken(string identityToken)
        {
            if (AppController.keys != null)
            {
                //IdentityToken
                if (identityToken != null)
                {
                    //Split the identityToken to get Header and Payload
                    string[] splitValues = identityToken.Split('.');
                    if (splitValues[0] != null)
                    {
                        //Decode header 
                        var headerJson = Encoding.UTF8.GetString(Base64Url.Decode(splitValues[0].ToString()));
                        //Deserilaize headerData
                        IdTokenHeader headerData = JsonConvert.DeserializeObject<IdTokenHeader>(headerJson);
                        //Verify if the key id of the key used to sign the payload is not null
                        if (headerData.Kid == null)
                        {
                            return false;
                        }
                        //Verify if the hashing alg used to sign the payload is not null
                        if (headerData.Alg == null)
                        {
                            return false;
                        }
                    }
                    if (splitValues[1] != null)
                    {
                        //Decode payload
                        var payloadJson = Encoding.UTF8.GetString(Base64Url.Decode(splitValues[1].ToString()));
                        payloadData = JsonConvert.DeserializeObject<IdTokenJWTClaimTypes>(payloadJson);
                        //Verify Aud matches ClientId
                        if (payloadData.Aud != null)
                        {
                            if (payloadData.Aud[0].ToString() != AppController.clientid)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        //Verify Authtime matches the time the ID token was authorized.                
                        if (payloadData.Auth_time == null)
                        {
                            return false;
                        }
                        //Verify exp matches the time the ID token expires, represented in Unix time (integer seconds).                
                        if (payloadData.Exp != null)
                        {
                            long expiration = Convert.ToInt64(payloadData.Exp);
                            long currentEpochTime = EpochTimeExtensions.ToEpochTime(DateTime.UtcNow);
                            //Verify the ID expiration time with what expiry time you have calculated and saved in your application
                            //If they are equal then it means IdToken has expired 
                            if ((expiration - currentEpochTime) <= 0)
                            {
                                return false;
                            }
                        }
                        //Verify Iat matches the time the ID token was issued, represented in Unix time (integer seconds).            
                        if (payloadData.Iat == null)
                        {
                            return false;
                        }
                        //verify Iss matches the  issuer identifier for the issuer of the response.     
                        if (payloadData.Iss != null)
                        {
                            if (payloadData.Iss.ToString() != AppController.issuerEndpoint)
                            {

                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        //Verify sub. Sub is an identifier for the user, unique among all Intuit accounts and never reused. 
                        //An Intuit account can have multiple emails at different points in time, but the sub value is never changed.
                        //Use sub within your application as the unique-identifier key for the user.
                        if (payloadData.Sub == null)
                        {

                            return false;
                        }
                    }
                    //Use external lib to decode mod and expo value and generte hashes
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                    //Read values of n and e from discovery document.
                    rsa.ImportParameters(
                      new RSAParameters()
                      {
                          //Read values from discovery document
                          Modulus = Base64Url.Decode(AppController.mod),
                          Exponent = Base64Url.Decode(AppController.expo)
                      });

                    //Verify Siganture hash matches the signed concatenation of the encoded header and the encoded payload with the specified algorithm
                    SHA256 sha256 = SHA256.Create();

                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(splitValues[0] + '.' + splitValues[1]));

                    RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                    rsaDeformatter.SetHashAlgorithm("SHA256");
                    if (rsaDeformatter.VerifySignature(hash, Base64Url.Decode(splitValues[2])))
                    {
                        //identityToken is valid
                        return true;
                    }
                    else
                    {
                        //identityToken is not valid
                        return false;

                    }
                }
                else
                {
                    //identityToken is not valid
                    return false;
                }
            }
            else
            {
                //Missing mod and expo values
                return false;
            }
        }

        private async Task<IEnumerable<Claim>> GetUserInfoClaimsAsync(string accessToken)
        {
            IEnumerable<Claim> userData = new List<Claim>();
            //Get UserInfo data when correct scope is set for SIWI and Get App now flows
            var userInfoClient = new UserInfoClient(AppController.userinfoEndpoint);

            //UserInfoResponse userInfoResponse = await userInfoClient.GetAsync(accessToken);
            UserInfoResponse userInfoResponse = await userInfoClient.GetAsync(RefreshToken);
            if (userInfoResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                //Read UserInfo Details
                userData = userInfoResponse.Json.ToClaims();

                foreach (Claim item in userData)
                {
                    if (item.Type == "sub" && item.Value != null)
                        sub = item.Value;
                    if (item.Type == "email" && item.Value != null)
                        email = item.Value;
                    if (item.Type == "emailVerified" && item.Value != null)
                        emailVerified = item.Value;
                    if (item.Type == "givenName" && item.Value != null)
                        givenName = item.Value;
                    if (item.Type == "familyName" && item.Value != null)
                        familyName = item.Value;
                    if (item.Type == "phoneNumber" && item.Value != null)
                        phoneNumber = item.Value;
                    if (item.Type == "phoneNumberVerified" && item.Value != null)
                        phoneNumberVerified = item.Value;

                    if (item.Type == "address" && item.Value != null)
                    {

                        Address jsonObject = JsonConvert.DeserializeObject<Address>(item.Value);

                        if (jsonObject.StreetAddress != null)
                            streetAddress = jsonObject.StreetAddress;
                        if (jsonObject.Locality != null)
                            locality = jsonObject.Locality;
                        if (jsonObject.Region != null)
                            region = jsonObject.Region;
                        if (jsonObject.PostalCode != null)
                            postalCode = jsonObject.PostalCode;
                        if (jsonObject.Country != null)
                            country = jsonObject.Country;
                    }

                }

            }

            return userData;
        }



        private async Task<Tuple<string>> GetTempStateAsync()
        {
            var data = await Request.GetOwinContext().Authentication.AuthenticateAsync("TempState");

            var state = data.Identity.FindFirst("state").Value;


            return Tuple.Create(state);
        }
    }
}