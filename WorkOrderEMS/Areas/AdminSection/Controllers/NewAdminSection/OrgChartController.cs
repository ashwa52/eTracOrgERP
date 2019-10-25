using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class OrgChartController : Controller
    {
        // GET: AdminSection/OrgChart
        private readonly IAdminDashboard _IAdminDashboard;
        public OrgChartController(IAdminDashboard _IAdminDashboard)
        {
            this._IAdminDashboard = _IAdminDashboard;
        }
        public ActionResult Index()
        {
            return View("~/Areas/AdminSection/Views/OrgChart/_ViewChart.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Sept-2019
        /// Created For : To get access permission list
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAccessDataList(long VST_Id)
        {
            var objeTracLoginModel = new eTracLoginModel();
            var lstOfTree = new List<AccessPermisionTreeViewModel>();
            //var objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);           
            bool isUpdate = false;
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = objeTracLoginModel.LocationID;
                        }
                    }
                }
                lstOfTree = _IAdminDashboard.ListTreeViewAccessPermission(VST_Id);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return this.Json(lstOfTree, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAccessPermission(AccessPermisionTreeViewModel obj)
        {
            var objeTracLoginModel = new eTracLoginModel();
            bool isSaved = false;
            try
            {
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        objeTracLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = objeTracLoginModel.LocationID;
                        }
                    }
                }
                if (obj != null)
                {
                    isSaved = _IAdminDashboard.SaceAccessPermission(obj);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return this.Json(isSaved, JsonRequestBehavior.AllowGet);
        }
    }
}