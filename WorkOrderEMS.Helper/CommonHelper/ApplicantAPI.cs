using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helper
{
    public class ApplicantAPI
    {
        public async Task<string> Configuration(string URL)
        {
            string returnString = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                //var client = new RestSharp.RestClient("https://api.availity.com/availity/v1/token");
                //var request = new RestSharp.RestRequest(RestSharp.Method.POST);
                //request.AddHeader("Accept", "application/json");
                //request.AddHeader("content-type", "application/x-www-form-urlencoded");
                //request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=cd3a1b06-5f4f-4cb4-a681-5504826fe361&client_secret=kE5nN0uI4jK2nG2cD6pE3gP5hV6pA6gO8cC5xL7jG0qB2xE8hB&scope=hipaa", RestSharp.ParameterType.RequestBody);
                //RestSharp.IRestResponse response = client.Execute(request);
                //var client = new RestSharp.RestClient("https://api.availity.com/availity/v1/token");
                //var request = new RestSharp.RestRequest(RestSharp.Method.GET);
                ////request.AddHeader("Accept", "application/json");
                //request.AddHeader("content-type", "application/x-www-form-urlencoded");
                //request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=cd3a1b06-5f4f-4cb4-a681-5504826fe361&client_secret=kE5nN0uI4jK2nG2cD6pE3gP5hV6pA6gO8cC5xL7jG0qB2xE8hB&scope=hipaa", RestSharp.ParameterType.RequestBody);
                //RestSharp.IRestResponse response = client.Execute(request);

                HttpClient client = new HttpClient();
                client.CancelPendingRequests();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("Ashwa:Ashwa@3839")));///username:password for auth
                //header.
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
        public async Task<string> Configuration1(string URL)
        {
            string returnString = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {

                //var client = new RestClient("https://api.availity.com/availity/v1/configurations?type=REPLACE_THIS_VALUE&subtypeId=REPLACE_THIS_VALUE&payerId=REPLACE_THIS_VALUE");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("Accept", "application/json");
                //request.AddHeader("authorization", "Bearer REPLACE_BEARER_TOKEN");
                //IRestResponse response = client.Execute(request);

                HttpClient client = new HttpClient();
                client.CancelPendingRequests();
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("Ashwa:Ashwa@3839")));///username:password for auth
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
    }
}
