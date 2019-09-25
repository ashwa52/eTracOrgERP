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
            if (data != null)
            {
                data.ProfileImage = data.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + data.ProfileImage;
                foreach (var item in data.ChildrenList)
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
    }
}