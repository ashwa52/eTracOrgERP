using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class eCountingReportController : Controller
    {
        // GET: eCountingReport
        private readonly IeCountingReport _IeCountingReport;
        private readonly IVendorManagement _IVendorManagement;
        public eCountingReportController(IeCountingReport _IeCountingReport, IVendorManagement _IVendorManagement)
        {
            this._IeCountingReport = _IeCountingReport;
            this._IVendorManagement = _IVendorManagement;
        }
        public ActionResult eCountingReport()
        {
            return View();
        }

        #region Vendor Details
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To display Vendor detail reprt view.
        /// </summary>
        /// <returns></returns>
        [ActionName("VendorDetails")]
        public ActionResult VendorDetails()
        {
            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];

            }
            var Obj = new VendorSetupManagementModel();
            ViewBag.VendorList = _IeCountingReport.GetAllVendorList(objLoginSession.LocationID);
            ViewBag.VendorType = _IVendorManagement.ListVendorType();
            return PartialView("_VendorDetails", Obj);

        }

        /// <summary>
        /// Created By : Ashwajit Bansod 
        /// Created Date : 31-OCT-2018
        /// Created For : To get Pending Po, Approve PO, Pending bill,approve bill, and pending payment count
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetVendorCountList(string _search, long? VendorId, DateTime? fromDate, DateTime? toDate, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
               
            }
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var CoutList = _IeCountingReport.GetListVendorCount(VendorId, locationId, fromDate, toDate, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var count in CoutList.rows)
                {

                        JQGridRow row = new JQGridRow();
                       // row.id = Cryptography.GetEncryptedData(Convert.ToString(misc.MISId), true);
                        row.cell = new string[5];
                        row.cell[0] = count.PendingPO.ToString();
                        row.cell[1] = count.ApprovePO.ToString();
                        row.cell[2] = count. PendingBill.ToString();
                        row.cell[3] = count. ApproveBill.ToString();
                        row.cell[4] = count. PendingPayment.ToString();
                        rowss.Add(row);
                    
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created by:Ashwajit bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all vendor or company list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllCompanyDataList()
        {
            long Location = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }            
            try
            {
                var result = _IeCountingReport.GetAllVendorList(Location);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all vendor type list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllCompanyTypeDataList()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                var result = _IVendorManagement.ListVendorType();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get payment till date as per vendor id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaymentTillDate(int VendorId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                
                var result = _IeCountingReport.GetPaymentTillDateData(VendorId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>Created by Ashawajit Bansod
        /// Get details of All pending details list 
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllPendingPODetails(DateTime? fromDate, DateTime? toDate, long? VendorId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
             var objDetails = _IeCountingReport.GetPendingPODetailsReport( _fromDate, _toDate, VendorId);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get All approve PO Details as per Vendor Id
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllApprovedPODetails(DateTime? fromDate, DateTime? toDate, long? VendorId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objDetails = _IeCountingReport.GetApprovePODetailsReport(_fromDate, _toDate, VendorId);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By: Ashwajit BAnsod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all pending bill list as per Vendor Id
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllPendingBillDetails(DateTime? fromDate, DateTime? toDate, long? VendorId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objDetails = _IeCountingReport.GetPendingBillDetailsReport(_fromDate, _toDate, VendorId);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By : Aswajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all approve bill details as per vendor id
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllApprovedBillDetails(DateTime? fromDate, DateTime? toDate, long? VendorId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objDetails = _IeCountingReport.GetAprovedBillDetailsReport(_fromDate, _toDate, VendorId);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get All pending payment Details for vendor
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllPendingPaymentDetails(DateTime? fromDate, DateTime? toDate, long? VendorId)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objDetails = _IeCountingReport.GetPendingPaymentDetailsReport(_fromDate, _toDate, VendorId);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all Vendor Details to display
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public JsonResult GetAllVendorDataToViewForReport(long VendorId)
        {
            var getData = new VendorAllViewDataModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;

                long UserId = 0;
                long LocationId = 0;
                long Vendor = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (LocationId == null)
                    {
                        LocationId = ObjLoginModel.LocationID;
                    }
                    UserId = ObjLoginModel.UserId;

                }
                Vendor = Convert.ToInt64(VendorId);
                if (Vendor > 0)
                {
                    getData = _IVendorManagement.GetAllVendorData(VendorId);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getData, JsonRequestBehavior.AllowGet);
        }
        #endregion Vendor Details

        #region Payment Summery
        public ActionResult PaymentSummaryView()
        {
            return PartialView("_ReportPaymentSummary");
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of Work Order Issued for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPaymentSummaryCount(long? LocID, DateTime? fromDate, DateTime? toDate)
        {
                eTracLoginModel objLoginSession = new eTracLoginModel();
                if ((eTracLoginModel)Session["eTrac"] != null)
                {
                    objLoginSession = (eTracLoginModel)Session["eTrac"];
                }
                if (LocID == null || LocID == 0)
                {
                    LocID = 0;
                }
                if (fromDate == null && toDate == null)
                {
                    fromDate = DateTime.UtcNow;
                    toDate = DateTime.UtcNow;
                }
                var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
                //flag status for if user filter record in time span so to date is till midnight. 
                bool isUTCDay = true;
                DateTime _fromDate = fromDate ?? clientdt.Date;
                DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

                //maintaining flag  if interval date come then need to fetch record till midnight of todate day
                if (toDate != null)
                {
                    if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                        isUTCDay = false;
                }
                if (_fromDate != null && _toDate != null)
                {
                    ////if interval date come then need to fetch record till midnight of todate day
                    if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                    {
                        _toDate = _toDate.AddDays(1).Date;
                    }
                    if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).Date;
                    }
                }
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                var lsData = _IeCountingReport.GetPaymentSummaryCount(0, fromDate, toDate).ToList();
                return Json(lsData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllPaymentSummaryDetails(long LocID, string BillType, DateTime? fromDate, DateTime? toDate)
        {

            eTracLoginModel objLoginSession = new eTracLoginModel();
            if ((eTracLoginModel)Session["eTrac"] != null)
            {
                objLoginSession = (eTracLoginModel)Session["eTrac"];
            }
            if (LocID == null || LocID == 0)
            {
                LocID = 0;
            }
            if (fromDate == null && toDate == null)
            {
                fromDate = DateTime.UtcNow;
                toDate = DateTime.UtcNow;
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = fromDate ?? clientdt.Date;
            DateTime _toDate = toDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (toDate != null)
            {
                if (toDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                ////if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            var objDetails = _IeCountingReport.GetPaymentSummaryDetailsReport(LocID, BillType, _fromDate, _toDate);

            return Json(objDetails, JsonRequestBehavior.AllowGet);

        }
        #endregion Payment Summery

        #region Location List
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-Nov-2018
        /// Created For : To get all location
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllLocationList()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                var result = _IeCountingReport.GetAllLocationList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Location List

    }
}