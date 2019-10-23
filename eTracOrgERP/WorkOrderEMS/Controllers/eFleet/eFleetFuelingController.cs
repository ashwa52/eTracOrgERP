using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Exception_B;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.Controllers.eFleet
{
    [RoutePrefix("fueling")]
    [Route("{action}")]
    public class eFleetFuelingController : Controller
    {
        private readonly IeFleetFuelingManager _IeFleetFuelingManager;
        private string FuelDocPath = ConfigurationManager.AppSettings["eFleetFueling"];
        public eFleetFuelingController(IeFleetFuelingManager _IeFleetFuelingManager)
        {
            this._IeFleetFuelingManager = _IeFleetFuelingManager;
        }
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        [HttpGet]
        [Route("listfueling")]
        public ActionResult ListFueling()
        {
            try
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
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("ListFueling");
        }

        [HttpGet]
        public JsonResult GetFuelingList(int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string frmDate =null, string todate = null, string sidx = null, string statusType = null)
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
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            //Fetching record like 2017-06-11T00:00:00-04:00 to 2017-06-12T00:0000-04:00
            string fromDate = (frmDate == null || frmDate == " " || frmDate == "") ? clientdt.Date.ToString() : frmDate;
            string toDate = (todate == null || todate == " " || todate == "") ? clientdt.AddDays(1).Date.ToString() : todate;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (todate != null && todate != "" && frmDate != null && frmDate != "")
            {
                DateTime tt = Convert.ToDateTime(toDate);
                if (tt.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }

            if (fromDate != null && toDate != null)
            {
                DateTime frmd = Convert.ToDateTime(fromDate);
                DateTime tod = Convert.ToDateTime(toDate);
                ////if interval date come then need to fetch record till midnight of todate day
                if ((frmd.Date != tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
                if ((frmd.Date == tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM"))
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
            }

            //Converting datetime from userTZ to UTC
            DateTime fDate = Convert.ToDateTime(fromDate).ConvertClientTZtoUTC();
            DateTime tDate = Convert.ToDateTime(toDate).ConvertClientTZtoUTC();

            var objeFleetFuelingModelForService = new eFleetFuelingModelForService();
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "FuelingDate" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);		
            try
            {
                var eFleetFuelingList = _IeFleetFuelingManager.GetListeFleetFuelingForJQGridDetails(ObjLoginModel.LocationID, rows, TotalRecords, sidx, sord, txtSearch, Convert.ToInt64(statusType), fDate,tDate);
                foreach (var eFleetFuel in eFleetFuelingList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(eFleetFuel.FuelID), true);
                    row.cell = new string[11];
                    row.cell[0] = eFleetFuel.VehicleNumber;
                    row.cell[1] = eFleetFuel.QRCodeID;
                    row.cell[2] = eFleetFuel.Mileage;
                    //row.cell[3] = eFleetFuel.CurrentFuel;
                    row.cell[3] = eFleetFuel.FuelTypeName;
                    //row.cell[5] = eFleetFuel.ReceiptNo;
                    row.cell[4] = eFleetFuel.FuelingDate.ToString("MM'/'dd'/'yyyy hh:mm tt");
                    row.cell[5] = Convert.ToString(eFleetFuel.Gallons);
                    row.cell[6] = ("$" + Convert.ToString(eFleetFuel.PricePerGallon));
                    row.cell[7] = ("$" + Convert.ToString(eFleetFuel.Total));
                    row.cell[8] = eFleetFuel.GasStatioName;
                    //row.cell[11] = eFleetFuel.CardNo;
                    row.cell[9] = eFleetFuel.DriverName;
                    row.cell[10] = eFleetFuel.FuelReceiptImage;
                    rowss.Add(row);
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            {
                Exception_B.exceptionHandel_Runtime(ex, "JsonResult GetFuelingList(string _search, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string statusType = null)", "eFleetFuelingController", null);

                List<JQGridRow> rowsss = new List<JQGridRow>();
                result.rows = rowsss.ToArray();
                result.page = 0;
                result.total = 0;
                result.records = 0;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReceiptDownload(string Id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Id))
                {
                    Id = Cryptography.GetDecryptedData(Id, true);
                    var _eFleetFuelModel = _IeFleetFuelingManager.GeteFleetFuelingDetailsById(Convert.ToInt64(Id));
                    if (!string.IsNullOrEmpty(_eFleetFuelModel.FuelReceiptImage))
                    {
                        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                        string IsFileExist = RootDirectory + FuelDocPath.Replace("~", "");
                        RootDirectory = RootDirectory + FuelDocPath.Replace("~", "") + _eFleetFuelModel.FuelReceiptImage;
                        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;
                        if (Directory.GetFiles(IsFileExist, _eFleetFuelModel.FuelReceiptImage).Length > 0)
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _eFleetFuelModel.FuelReceiptImage);
                        }
                        else
                        {
                            RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + FuelDocPath.Replace("~", "") + "FileNotFound.png";
                            byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
                            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else { return Json("Id is Empty!"); }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                return Json(ex.Message);
            }
        }

    }
}