using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class EPeopleController : Controller
    {
        // GET: EPeople
        private readonly IePeopleManager _IePeopleManager;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);     
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        public EPeopleController(IePeopleManager _IePeopleManager)
        {
            this._IePeopleManager = _IePeopleManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 23-Sept-2019
        /// Created for : To get User Employee manager
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserHeirachyList(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel) (Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserHeirarchyList(LocationId, _UserId);
            if (data.Count() > 0)
            {              
                foreach (var item in data)
                {                   
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                    details.Add(item);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        #region Employee Management
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserTreeViewList(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserTreeViewList(_UserId);
            if (data.Count() > 0)
            {
                //foreach (var item in data)
                //{
                //    item.ProfilePhoto = item.ProfilePhoto == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfilePhoto;
                //    details.Add(item);
                //}
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get user details by userid
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserListByUserId(string Id, long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserListViewEmployeeManagementModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetUserListByUserId(LocationId, _UserId);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.ProfilePhoto = item.ProfilePhoto == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfilePhoto;
                    details.Add(item);
                }
                return PartialView("~/Views/NewAdmin/ePeople/_ListUserEMP.cshtml", data);
            }
            else
            {
                return PartialView("~/Views/NewAdmin/ePeople/_ListUserEMP.cshtml", data);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-oct-2019
        /// Created For : To get VCS Position of User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetVehicleSeatingChartPositionedUser(string Id)
        {
            eTracLoginModel ObjLoginModel = null;
            long LocationId = 0;
            var details = new UserListViewEmployeeManagementModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ObjLoginModel != null)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var id = Cryptography.GetDecryptedData(Id, true);
            long _UserId = 0;
            long.TryParse(id, out _UserId);
            var data = _IePeopleManager.GetVCSPositionByUserId(_UserId);
            if (data != null)
            {               
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewVCS()
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); 
            }
            return PartialView("~/Views/NewAdmin/ePeople/_VSCPintingChart.cshtml");
        }
        #endregion Employee Management
    }
}