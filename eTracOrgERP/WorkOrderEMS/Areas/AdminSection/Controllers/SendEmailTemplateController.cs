using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class SendEmailTemplateController : Controller
    {
        // GET: AdminSection/SendEmailTemplate
        private readonly ISendEmailTemplateManager _ISendEmailTemplateManager;
        public SendEmailTemplateController(ISendEmailTemplateManager _ISendEmailTemplateManager)
        {
            this._ISendEmailTemplateManager = _ISendEmailTemplateManager;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        public ActionResult SendMail()
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.AccountSection = true;
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                long Location = ObjLoginModel.LocationID;
                var savedStatus = _ISendEmailTemplateManager.SendMailTemplate(Location);
                    ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
            }
            catch (Exception ex)
            {
                return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("CostCodeAndRule", "CostCode");
        }
    }
}