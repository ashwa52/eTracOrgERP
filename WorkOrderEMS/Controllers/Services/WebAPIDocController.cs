using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WorkOrderEMS.Controllers.Services
{
    public class WebAPIDocController : Controller
    {
        public class ValidateMimeMultipartContentFilter : ActionFilterAttribute
        {
            public void OnActionExecuted(System.Web.Http.Controllers.HttpActionContext actionContext)
            {
                if (!actionContext.Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
            }

            public void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
            {

            }

        }
    }
}