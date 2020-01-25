using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
//using Windows.Web.Http.Filters;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS;

namespace WorkOrderEMS.Helper
{
    public class RecruiteeAPI 
    {
        //static readonly 
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
                if(result.IsSuccessStatusCode == true)
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

       

        public async Task<string> GetRecruitee(string URL)
        {
            string str = "";
            try
            {
                const string uri = "https://api.recruitee.com/c/40359/candidates/";
                using (var client1 = new HttpClient())
                {
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
                    client1.DefaultRequestHeaders.Authorization = header;
                   var tt = await client1.GetStringAsync(uri);
                }
            }
            catch (Exception ex)
            {

            }
            return str;
        }
        public string POSTreq(string PostData, string url)
        {

            string returnData = "";
            try
            {
                using (HttpClient objClint = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClint.BaseAddress = new Uri("https://api.recruitee.com");
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

        public string POSTreqDemo(string PostData, string url)
        {
            string returnData = "";
            try
            {
                using (HttpClient objClint = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                    objClint.BaseAddress = new Uri("https://api.recruitee.com");
                    objClint.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "dkpEaHRJSzJiempBUlVNOC9QN3JlUT09");
                    #region Demo Code

                    var stringContent = new StringContent(PostData, Encoding.UTF8, "application/json");
                    var response =  objClint.PostAsync(url, new StringContent(PostData)).Result;
                    //var response = objClint.PostAsync(url, stringContent).Result;

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
    }
}
