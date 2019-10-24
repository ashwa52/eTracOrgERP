using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace WorkOrderEMS.Controllers
{
    public class RecruiteeAPIController : ApiController
    {
        public async Task<ActionResult> Index()
        {
            string str = "";
            try
            {
                const string uri = "https://api.recruitee.com/c/40359/candidates/";
                using (var client1 = new HttpClient())
                {
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("terralwright@eliteparkingsoa.com:Elite19!!")));///username:password for auth
                    client1.DefaultRequestHeaders.Authorization = header;
                    str = await client1.GetStringAsync(uri);
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
