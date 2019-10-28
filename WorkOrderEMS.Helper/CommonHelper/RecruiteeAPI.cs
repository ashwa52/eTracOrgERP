using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS;

namespace WorkOrderEMS.Helper
{
    public class RecruiteeAPI 
    {
        static readonly HttpClient client = new HttpClient();
        public async Task<string> Index(string URL)
        {
            string returnString = "";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
                client.DefaultRequestHeaders.Authorization = header;
                var result = await client.GetAsync(URL, HttpCompletionOption.ResponseHeadersRead)
                             .ConfigureAwait(false);
                //HttpResponseMessage response = await client.GetAsync("https://api.recruitee.com/c/40359/offers/");
                //response.EnsureSuccessStatusCode();
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
        //public async Task<string> GetRecruitee(string URL)
        //{
        //    string str = "";
        //    try
        //    {
        //        const string uri = "https://api.recruitee.com/c/40359/candidates/";
        //        using (var client1 = new HttpClient())
        //        {
        //            var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
        //            client1.DefaultRequestHeaders.Authorization = header;
        //            //var tt = await client1.GetStringAsync(uri);
        //            var tt = await client1.GetStringAsync(uri).Wait();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return str;
        //}

        //public string Main()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://api.recruitee.com/c/40359/candidates/");
        //        Uri uri = new Uri("https://api.recruitee.com/c/40359/candidates/");
        //        var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
        //        client.DefaultRequestHeaders.Authorization = header;
        //        //HTTP GET
        //        var responseTask = client.GetAsync(uri);
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {

        //            var readTask = result.Content.ReadAsStringAsync();
        //            readTask.Wait();

        //        }
        //    }
        //    return "sd";
        //}


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
    }
}
