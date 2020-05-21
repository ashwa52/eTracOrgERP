using System;
using System.Linq;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class DOTmanagementController : Controller
    {
        private readonly IPOTypeDetails _IPOTypeDetails;
        private readonly IBillDataManager _IBillDataManager;
        private readonly IVendorManagement _IVendorManagement;
        public DOTmanagementController(IPOTypeDetails _IPOTypeDetails, IBillDataManager _IBillDataManager, IVendorManagement _IVendorManagement)
        {
            this._IPOTypeDetails = _IPOTypeDetails;
            this._IBillDataManager = _IBillDataManager;
            this._IVendorManagement = _IVendorManagement;
        }
        // GET: POTypeData
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        public ActionResult DOTManagement_Sub()
        {
            var objPOTypeModel = new DOTManagementViewDataModel();
            try
            {
                ViewBag.PONumber = _IPOTypeDetails.PONumberData();
                ViewBag.POType = _IPOTypeDetails.POTypeList();
            }
            catch (Exception ex)
            {
            }
            return View(objPOTypeModel);
        }
        public ActionResult DOTManagement()
        {
            return View();
        }

        //public ActionResult DOTManagementSubmit(DOTManagementViewDataModel DOTM)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.StrError = "Error " + ex.ToString().Replace('\n', '_');
        //        return View("Error");
        //    }
        //    return RedirectToAction(null);
        //}
        public JsonResult GetAllDriverList(string _search, long? UserId, long? LocationId, string status, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserTypeId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ObjLoginModel != null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
                UserTypeId = ObjLoginModel.UserType;
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;

            try
            {
                if (txtSearch != null && txtSearch != "")
                {
                    var AllPOList1 = _IPOTypeDetails.GetAllPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord).Where(n => n.DisplayLogPOId == txtSearch).ToList();
                    return Json(AllPOList1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var AllPOList = _IPOTypeDetails.GetAllPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord);
                    return Json(AllPOList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

        }

    }
}