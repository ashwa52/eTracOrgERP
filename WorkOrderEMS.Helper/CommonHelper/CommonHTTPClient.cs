using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helper
{
    public class CommonHTTPClient
    {
        private readonly string I9StageURL = ConfigurationManager.AppSettings["EVerifyStageURL"];
        private readonly string client_idFlorida = ConfigurationManager.AppSettings["client_idFlorida"];
        private readonly string client_secretFlorida = ConfigurationManager.AppSettings["client_secretFlorida"];
        private readonly string backUserName = ConfigurationManager.AppSettings["BackgroundUserName"].ToString();
        private readonly string backPassword = ConfigurationManager.AppSettings["BackgroundPassword"].ToString();
        public async Task<string> Index(string URL)
        {
            string returnString = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpClient client = new HttpClient();
                client.CancelPendingRequests();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
                client.DefaultRequestHeaders.Authorization = header;

                var result = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead)
                             .ConfigureAwait(false);
                string responseBody = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode == true)
                {
                    returnString = responseBody;
                }
                else
                {
                    returnString = "false";
                }
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            return returnString;
        }
        public string POSTreq(string PostData, string url)
        {

            string returnData = "";
            try
            {
                using (HttpClient objClint = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClint.BaseAddress = new Uri("https://api.availity.com/availity");
                    objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dkpEaHRJSzJiempBUlVNOC9QN3JlUT09");
                    #region Demo Code
                    //string message = JsonConvert.SerializeObject(dynamicJson);
                    //HttpContent c = new StringContent(PostData, Encoding.UTF8, "application/json");
                    var stringContent = new StringContent(PostData, Encoding.UTF8, "application/json");
                    // var response = objClint.PostAsync(url, c).Result;
                    var response = objClint.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {

            }
            return returnData;
        }

        #region I-9 Form
        public async Task<string> I9AuthenticationDemo(string URL)
        {
            string returnString = "";
            URL = "authentication/login";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(I9StageURL);
                client.CancelPendingRequests();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                //var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
                //client.DefaultRequestHeaders.Authorization = header;

                var result = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead)
                             .ConfigureAwait(false);
                string responseBody = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode == true)
                {
                    returnString = responseBody;
                }
                else
                {
                    returnString = "false";
                }
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            return returnString;
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 06-04-2020
        /// Created For : To Authenticate the I9 user
        /// </summary>
        /// <param name="PostData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string I9PostData(string PostData, string url)
        {
            url = "https://stage-everify.uscis.gov/api/v30/authentication/login";
            string returnData = "";
            try
            {
                using (HttpClient objClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //objClient.BaseAddress = new Uri(I9StageURL);
                    //var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("AMIS1739:Elite76!!")));///username:password for auth
                    //objClient.DefaultRequestHeaders.Authorization = header;
                    #region Demo Code
                    var stringContent = new StringContent(PostData, Encoding.UTF8, "application/json");
                    var response = objClient.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {
                returnData = Ex.Message;
            }
            return returnData;
        }

        public string I9RefreshToken(string AccessToken, string url)
        {
            string returnData = "";
            url = "https://stage-everify.uscis.gov/api/v30/authentication/refresh";
            try
            {
                using (HttpClient objClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //objClient.BaseAddress = new Uri(I9StageURL);
                    objClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
                    //var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("AMIS1739:Elite76!!")));///username:password for auth
                    //objClient.DefaultRequestHeaders.Authorization = header;
                    #region Demo Code
                    var stringContent = new StringContent("",Encoding.UTF8, "application/json");
                    var response = objClient.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {
                returnData = Ex.Message;
            }
            return returnData;
        }

        public string I9PostCase(string PostData, string url,string AccessToken)
        {
            string returnData = "";
            url = "https://stage-everify.uscis.gov/api/v30/cases";
            try
            {
                using (HttpClient objClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //objClient.BaseAddress = new Uri(I9StageURL);
                    objClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
                    #region Demo Code
                    var stringContent = new StringContent(PostData, Encoding.UTF8, "application/json");
                    //var stringContent = new StringContent(PostData);
                    var response = objClient.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {
                returnData = Ex.Message;
            }
            return returnData;
        }
        #endregion I-9 Form
        #region Florida blue
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-04-2020
        /// Created For : To authenticate florida blue.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string FloridaBlueAuthentication(string url)
        {
            string returnData = "";
            try
            {
                using (HttpClient objClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                        new KeyValuePair<string, string>("client_id", client_idFlorida),
                        new KeyValuePair<string, string>("client_secret", client_secretFlorida),
                        new KeyValuePair<string, string>("scope", "hipaa")
                    });
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    //var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("AMIS1739:Elite76!!")));///username:password for auth
                    //objClient.DefaultRequestHeaders.Authorization = header;
                    #region Demo Code
                    //var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    var response = objClient.PostAsync("https://api.availity.com/availity/v1/token", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {
                returnData = Ex.Message;
            }
            return returnData;
        }

        public string FloridaBluePost( string url, string AccessToken)
        {
            string returnData = "";
            try
            {
                using (HttpClient objClient = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //objClient.BaseAddress = new Uri(I9StageURL);
                    objClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
                    #region Demo Code
                    //var stringContent = new StringContent(PostData, Encoding.UTF8, "application/json");
                    var response = objClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        returnData = response.Content.ReadAsStringAsync().Result;
                    }
                    #endregion Demo Code
                }
            }
            catch (Exception Ex)
            {
                returnData = Ex.Message;
            }
            return returnData;
        }

        #endregion Florida blue

        #region Background Screening API
        public async Task<string> SendDataForBackGroundScreening(string URL,string postData)
        {
            string returnString = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpClient client = new HttpClient();
                client.CancelPendingRequests();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("eliteparws:Hq4VuK59m")));///username:password for auth
                client.DefaultRequestHeaders.Authorization = header;

                var stringContent = new StringContent(postData, Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://uat-api.applicantinsight.net/v1/api/Screenings", stringContent).Result;
                if (response.IsSuccessStatusCode == true)
                {
                    returnString = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    returnString = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (HttpRequestException e)
            {
                throw;
            }
            return returnString;
        }
        #endregion Background Screening API
    }
}
