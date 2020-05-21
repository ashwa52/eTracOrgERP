using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
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
            var getEMPData = _workorderems.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
            if(getEMPData != null)
            {
                details.ApplicantId = ApplicantId;
                details.FirstName = getEMPData.API_FirstName + " " + getEMPData.API_LastName;
                details.Image = (getEMPData.ALA_Photo == "" || getEMPData.ALA_Photo == null) ? ConfigurationManager.AppSettings["hostingPrefix"] + Convert.ToString(ConfigurationManager.AppSettings["ProfilePicPath"]).Replace("~", "") + "no-profile-pic.jpg" :  getEMPData.ALA_Photo;
                details.Phone = getEMPData.ACI_PhoneNo;
                details.Email = getEMPData.ACI_eMail;
                //details.City = getEMPData.ci;
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
            var getEMPData = _workorderems.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
            if (getEMPData != null)
            {
                details.ApplicantId = ApplicantId;
                details.FirstName = getEMPData.API_FirstName + " " + getEMPData.API_LastName;
                details.Image = (getEMPData.ALA_Photo == "null" || getEMPData.ALA_Photo == null) ? ConfigurationManager.AppSettings["hostingPrefix"] + Convert.ToString(ConfigurationManager.AppSettings["ProfilePicPath"]).Replace("~", "") + "no-profile-pic.jpg" : getEMPData.ALA_Photo;
                details.Phone = getEMPData.ACI_PhoneNo;
                details.Email = getEMPData.ACI_eMail;
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
            return View("~/Views/Guest/ThankYou.cshtml");
            //return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetApplicantInfo(long ApplicantId)
        {
            string str = "";
            var details = new EmployeeVIewModel();
            var _workorderems = new workorderEMSEntities();
            var getEMPData = _workorderems.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
            if (getEMPData != null)
            {
                details.ApplicantId = ApplicantId;
                details.FirstName = getEMPData.API_FirstName + " " + getEMPData.API_LastName;
                details.Image = (getEMPData.ALA_Photo == "null" || getEMPData.ALA_Photo == null) ? ConfigurationManager.AppSettings["hostingPrefix"] + Convert.ToString(ConfigurationManager.AppSettings["ProfilePicPath"]).Replace("~", "") + "no-profile-pic.jpg" : getEMPData.ALA_Photo;
                details.Phone = getEMPData.ACI_PhoneNo;
                details.Email = getEMPData.ACI_eMail;
                //details.Status = getEMPData.sta
                details.ActionValue = "Offer";
            }
            return View("GetAssessmentStatus",details);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-March-2020
        /// Created For : TO make applicant ready for  onboarding
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginForOnboarding(long ApplicantId)
        {
            var _workorderems = new workorderEMSEntities();
            var common_B = new Common_B();
            if(ApplicantId > 0)
            {
                common_B.SaveApplicantStatus(ApplicantId, ApplicantStatus.Onboarding, ApplicantIsActiveStatus.Onboarding);
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Login");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-arch-2020
        /// Created For : To accept or reject Interview time
        /// </summary>
        /// <param name="IPT_Id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AcceptRejectInterviewTime(long IPT_Id, string Status)
        {
            var _IePeopleManager = new ePeopleManager();
            var _workorderems = new workorderEMSEntities();
            var common_B = new Common_B();
            try
            {
                if (IPT_Id > 0 && Status != null)
                {
                    _IePeopleManager.ScheduleInterviewOfApplicant(IPT_Id, Status);
                    //common_B.SaveApplicantStatus(ApplicantId, ApplicantStatus.I, ApplicantIsActiveStatus.I);
                    return View("~/Views/Error/_ThankYou.cshtml");
                }
                else
                {
                    ViewBag.Error = CommonMessage.WrongParameterMessage();
                    return View("~/Views/Error/_Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                return View("~/Views/Error/_Error.cshtml");
            }
        }
    }
}