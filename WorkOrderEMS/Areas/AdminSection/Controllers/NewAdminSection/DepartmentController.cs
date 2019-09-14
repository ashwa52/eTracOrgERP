using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Areas.AdminSection.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: AdminSection/Department
        private readonly IDepartment _IDepartment;
        public DepartmentController(IDepartment _IDepartment)
        {
            this._IDepartment = _IDepartment;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public ActionResult Index()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                    {
                        (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetDepartmentList()
        {
            return null;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-August-2019
        /// Created For : To save Department data
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDepartment(DepartmentModel Obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                if(Obj != null && Obj.DepartmentName != null)
                {
                    var isSaved = _IDepartment.SaveDepartment(Obj);
                    if (isSaved == true)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(new { Message = ViewBag.Message, AlertMessageClass = ViewBag.AlertMessageClass }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Crated Date : 26-Aug-2019
        /// Created For : To get all department List
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListDepartment()
        {
            long? LocationId = 0, UserId = 0;
            string txt = string.Empty;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
                UserId = ObjLoginModel.UserId;
            }            
            try
            {
                var data = _IDepartment.ListAllDepartment(txt, LocationId,UserId);
                if (data.Count() > 0)
                {
                    var arrDept = data.ToArray();
                    return Json(arrDept, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DepartmentModel[] arr = { };
                    return Json(arr, JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        //[HttpGet]
        //public JsonResult GetListDepartment(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    ViewBag.AccountSection = true;
        //    if (Session != null && Session["eTrac"] != null)
        //    {
        //        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //        if (locationId == null)
        //        {
        //            locationId = ObjLoginModel.LocationID;
        //        }
        //        UserId = ObjLoginModel.UserId;
        //    }

        //    JQGridResults result = new JQGridResults();
        //    List<JQGridRow> rowss = new List<JQGridRow>();
        //    sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
        //    sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
        //    txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
        //    try
        //    {
        //        var lstDept = _IDepartment.ListAllDepartment(txtSearch, locationId, UserId, page, rows, sidx, sord);
        //        //var contractTypeList = _IContractType.GetContractTypeList(UserId, rows, TotalRecords, sidx, sord);
        //        foreach (var contracttype in lstDept.rows)
        //        {
        //            JQGridRow row = new JQGridRow();
        //            row.id = Cryptography.GetEncryptedData(Convert.ToString(contracttype.CTT_Id), true);
        //            row.cell = new string[3];
        //            row.cell[0] = Convert.ToString(contracttype.CTT_ContractType);
        //            row.cell[1] = Convert.ToString(contracttype.CTT_Discription);
        //            row.cell[2] = Convert.ToString(contracttype.CTT_IsActive);
        //            rowss.Add(row);
        //        }
        //        result.rows = rowss.ToArray();
        //        result.page = Convert.ToInt32(page);
        //        result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
        //        result.records = Convert.ToInt32(TotalRecords.Value);
        //    }
        //    catch (Exception ex)
        //    { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-Aug-2019
        /// Created For : To get Department Details by Department Id.
        /// </summary>
        /// <param name="Id"></param>
        [HttpPost]
        public JsonResult EditDepartment(long Id)
        {
            eTracLoginModel ObjLoginModel = null;
            ViewBag.IsUpdate = true;
            var model = new DepartmentModel();
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)
                    {
                        (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                    }
                }
            }
            try
            {
                if(Id > 0)
                {
                    model = _IDepartment.GetDepartmentData(Id);
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-Sept-2019
        /// Created For : To delete Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDepartment(long id)
        {
            eTracLoginModel ObjLoginModel = null;
            var lst = new AddChartModel();
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            try
            {
                var data = _IDepartment.GetDepartmentData(id);
                if (data != null)
                {
                    if(id > 0)
                    {
                        data.DeptId = id;
                        var dataDetails = _IDepartment.DeleteDepartmentById(data);
                    }                    
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message; ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}