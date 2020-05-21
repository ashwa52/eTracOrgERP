using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        private readonly INotification _INotification;
        public NotificationController(INotification _INotification)
        {
            this._INotification = _INotification;            
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 02-04-2020
        /// Created For :  To get Unseen list of user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUnseenList()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getList = _INotification.GetNotification(ObjLoginModel.UserId, ObjLoginModel.UserName);
            return View("_NotificationList", getList);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-04-2020
        /// Created For : To make notification readable
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReadNotification(long NotificationId)
        {
            try
            {
                if (NotificationId > 0)
                {
                    var isRead = _INotification.ReadNotificationById(NotificationId);
                    return RedirectToAction("GetUnseenList");
                }
                else
                    return RedirectToAction("GetUnseenList");
            }
            catch(Exception ex)
            {
                return RedirectToAction("GetUnseenList");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get applicant details by Applicant id
        /// Created Date : 03-04-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetViewForApplicantDetails(long ApplicantId,string ApplicantStatus)
        {
            var getData = new ApplicantDetails();
            try
            {
                if(ApplicantId > 0 && ApplicantStatus != null)
                {
                     getData = _INotification.GetApplicantDetails(ApplicantId);
                }
                else
                {
                    return View("_ApplicantDetailsWhenAcceptRejectCounterOffer", getData);
                }
            }
            catch(Exception ex)
            {

            }
            return View("_ApplicantDetailsWhenAcceptRejectCounterOffer", getData);
        }
        [HttpPost]
        public ActionResult ViewForMeeting(NotificationDetailModel obj)
        {


            try
            {
                if (obj != null)
                {

                    var getnotificationdetails = _INotification.NotificationDetailsforMeetingDateTime(obj);

                    return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml", getnotificationdetails);
                }
                else
                {
                    return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml");
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml");
        }

    }
}