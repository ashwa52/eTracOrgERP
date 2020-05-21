using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    public class HRMSController : BaseController
    {
        // GET: NewAdmin
        private readonly IDepartment _IDepartment;
        private readonly IGlobalAdmin _GlobalAdminManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IQRCSetup _IQRCSetup;
        private readonly IePeopleManager _IePeopleManager;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];

        public HRMSController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
        }

        #region Shift Master

        [HttpGet]
        public ActionResult ShiftMaster()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return View();
        }

        public ActionResult ShiftMasterAddEdit(int Id)
        {
            ShiftModel Shift = new ShiftModel();
            try
            {
                if (Id > 0)
                {
                    DataTable dt = new DataTable();
                    string SQRY = "EXEC USP_Get_ShiftDetail_Edit '" + Id + "'";
                    dt = DBUtilities.GetDTResponse(SQRY);
                    if (dt != null)
                    {
                        List<ShiftModel> ShiftList = new List<ShiftModel>();
                        ShiftList = DataRowToObject.CreateListFromTable<ShiftModel>(dt);
                        Shift = ShiftList.Where(c => c.Id == Id).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex) { }

            return View("ShiftMasterAddEdit", Shift);
        }

        [HttpGet]
        public JsonResult GetListShiftJSGrid(string Search)
        {
            try
            {
                string SQRY = "EXEC SP_GetShiftList '" + Search + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<ShiftModel> ITAdministratorList = DataRowToObject.CreateListFromTable<ShiftModel>(DT);
                return Json(ITAdministratorList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public ActionResult ShiftManagmentSubmit(ShiftModel objPOTypeDataModel)
        {
            string UserId;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = Convert.ToString(ObjLoginModel.UserId);
            }
            try
            {
                if (objPOTypeDataModel.Id == 0)
                {
                    string SQRY = "EXEC INSERT_Shift_Details '" + objPOTypeDataModel.ShiftCode + "','" + objPOTypeDataModel.ShiftName + "','" + objPOTypeDataModel.Description + "','" + objPOTypeDataModel.IsActive + "','" + Convert.ToString(ObjLoginModel.UserId) + "','" + objPOTypeDataModel.StartTime + "','" + objPOTypeDataModel.EndTime + "'  ";
                    DataTable DT = DBUtilities.GetDTResponse(SQRY);
                }
                else
                {
                    string SQRY1 = "Update Tbl_HRMS_Shift SET ShiftCode='" + objPOTypeDataModel.ShiftCode + "',ShiftName='" + objPOTypeDataModel.ShiftName + "',Description='" + objPOTypeDataModel.Description + "',IsActive='" + objPOTypeDataModel.IsActive + "',UpdatedBy='" + Convert.ToString(ObjLoginModel.UserId) + "',StartTime='" + objPOTypeDataModel.StartTime + "',EndTime='" + objPOTypeDataModel.EndTime + "' where Id='" + objPOTypeDataModel.Id + "' ";
                    DataTable DT = DBUtilities.GetDTResponse(SQRY1);
                }
            }
            catch (Exception ex)
            {
                ViewBag.StrError = ex;

            }
            return RedirectToAction("ShiftMaster");
        }

        public JsonResult CheckDuplicateShiftCode(string ShiftCode)
        {
            string Count = "";
            try
            {
                string SQRY = "EXEC USP_Check_Shift_Duplicate '" + ShiftCode + "'";
                DataTable DT = DBUtilities.GetDTResponse(SQRY);
                List<ShiftModel> ShiftCodeList = DataRowToObject.CreateListFromTable<ShiftModel>(DT);
                Count = ShiftCodeList.Count.ToString();
                return Json(Count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Count, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Shift Master
    }
}