using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Controllers.Administrator
{
    public class BaseController : Controller
    {
        public DBUtilities DBUtilities = new DBUtilities();
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string ReturnUrl = "";
            try
            {
                base.OnActionExecuting(filterContext);
                /////////////////Define Default 
                if (User.Identity.IsAuthenticated == true)
                {

                }
                else
                {
                    RedirectToAction("Index", "Login", new { ReturnUrl = ReturnUrl });
                }
            }
            catch (Exception ex)
            {
                TempData["logout"] = CommonMessage.SessionExpired();
                RedirectToAction("index", "Login", ReturnUrl);
                //RedirectToAction("Index", "Login", new { ReturnUrl = ReturnUrl });
            }
        }
    }
}