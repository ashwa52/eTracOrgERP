using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderEMS.Controllers.EmployeeIdle
{
    public class IdleEmployeeController : Controller
    {
        // GET: IdleEmployee
        public ActionResult IdleEmployeeReport()
        {
            return View("EmployeeIdleReport");
        }
    }
}