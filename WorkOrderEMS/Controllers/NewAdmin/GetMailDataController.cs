using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Controllers
{
    public class GetMailDataController : Controller
    {
        [System.Web.Mvc.HttpGet]
        public ActionResult GetAssessmentStatus(long ApplicantId, string Status)
        {
            string str = "";
            var details = new EmployeeVIewModel();
            var _workorderems = new workorderEMSEntities();
            var getEMPData = _workorderems.ApplicantInfoes.Where(x => x.API_ApplicantId == ApplicantId).FirstOrDefault();
            if(getEMPData != null)
            {
                details.ApplicantId = ApplicantId;
                details.FirstName = getEMPData.API_FirstName + " " + getEMPData.API_LastName;
                details.Image = (getEMPData.API_Photo == "null" || getEMPData.API_Photo == null) ? ConfigurationManager.AppSettings["hostingPrefix"] + Convert.ToString(ConfigurationManager.AppSettings["ProfilePicPath"]).Replace("~", "") + "no-profile-pic.jpg" :  getEMPData.API_Photo;
                details.Phone = getEMPData.API_PhoneNumber;
                details.Email = getEMPData.API_Email;
                details.City = getEMPData.API_City;
                details.ActionValue = "Assessment";
                details.Status = Status;
            }
            
            if (ApplicantId> 0 && Status != null)
            {
                return View(details);
            }
            else
            {
                return View(details);
            }
        }
        [System.Web.Mvc.HttpGet]
        public ActionResult GetBackGroundStatus(long ApplicantId, string Status)
        {
            string str = "";
            var details = new EmployeeVIewModel();
            var _workorderems = new workorderEMSEntities();
            var getEMPData = _workorderems.ApplicantInfoes.Where(x => x.API_ApplicantId == ApplicantId).FirstOrDefault();
            if (getEMPData != null)
            {
                details.ApplicantId = ApplicantId;
                details.FirstName = getEMPData.API_FirstName;
                details.Image = (getEMPData.API_Photo == "null" || getEMPData.API_Photo == null) ? ConfigurationManager.AppSettings["hostingPrefix"] + Convert.ToString(ConfigurationManager.AppSettings["ProfilePicPath"]).Replace("~","") + "no-profile-pic.jpg" :  getEMPData.API_Photo;
                details.Phone = getEMPData.API_PhoneNumber;
                details.Email = getEMPData.API_Email;
                details.City = getEMPData.API_City;
                details.Status = Status;
                details.ActionValue = "Background";
            }

            if (ApplicantId > 0 && Status != null)
            {
                return View("~/Views/GetMailData/GetAssessmentStatus.cshtml", details);
            }
            else
            {
                return View("~/Views/GetMailData/GetAssessmentStatus.cshtml", details);
            }
        }
        [HttpPost]
        public ActionResult ClearedNotCleared(string IsActive, string ActionVal, long ApplicantId)
        {
            var _IePeopleManager = new ePeopleManager();
            bool isCleared = false;
            try
            {
                if (IsActive != null && ActionVal != null && ApplicantId > 0)
                {
                    isCleared = _IePeopleManager.ClearedOrNot(IsActive, ActionVal, ApplicantId);
                }
                else
                {
                    isCleared = false;
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Login/Index.cshtml");
            }
            return View("~/Views/Login/Index.cshtml");
            //return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}