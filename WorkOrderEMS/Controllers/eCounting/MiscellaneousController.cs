using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class MiscellaneousController : Controller
    {
        private readonly IMiscellaneousManager _IMiscellaneousManager;
        private readonly IBillDataManager _IBillDataManager;
        private readonly IVendorManagement _IVendorManagement;
        public MiscellaneousController(IMiscellaneousManager _IMiscellaneousManager, IBillDataManager _IBillDataManager, IVendorManagement _IVendorManagement)
        {
            this._IMiscellaneousManager = _IMiscellaneousManager;
            this._IBillDataManager = _IBillDataManager;
            this._IVendorManagement = _IVendorManagement;
        }
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string MiscellaneousPath = Convert.ToString(ConfigurationManager.AppSettings["MiscellaneousImage"], CultureInfo.InvariantCulture);
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        // GET: Miscellaneous
        public ActionResult ViewMiscellaneous()
        {
            return View();
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To get Master Miscellaneous List as per location Id
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListMasterMiscellaneous(string txtSearch , long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {

                if (txtSearch != null && txtSearch != "")
                {
                    var miscListSearch = _IMiscellaneousManager.GetListMiscellaneous(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                    var FilteredList = miscListSearch.Where(x => x.MISId.ToLower().Contains(txtSearch.ToLower().Trim())).ToList();
                    return Json(miscListSearch.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var miscList = _IMiscellaneousManager.GetListMiscellaneous(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                    return Json(miscList.ToList(), JsonRequestBehavior.AllowGet);
                }
                /*foreach (var misc in miscList.rows)
                {
                    if (misc.MISId != null)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(misc.MISId), true);
                        row.cell = new string[7];
                        row.cell[0] = misc.MISId.ToString();
                        row.cell[1] = misc.LocationName;
                        row.cell[2] = misc.VendorName == null ? misc.UserName : misc.VendorName;
                        row.cell[3] = misc.UserName;
                        row.cell[4] = misc. InvoiceAmount.ToString();
                        row.cell[5] = misc.MISDate;
                        row.cell[6] = misc.Status;
                        rowss.Add(row);
                    }
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
                */

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 22-OCT-2018
        /// Created For  :To get Miscellaneous list as per misc Id
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="MiscId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListMiscellaneousListByMiscId(string _search, long? UserId, string MiscId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long MISNumber = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;

                
                string id = MiscId.Split('S')[1];
                MISNumber = Convert.ToInt64(id);
                //long.TryParse(id, out MISNumber);
            }
            /*
            if (!string.IsNullOrEmpty(MiscId))
            {
                MiscId = Cryptography.GetDecryptedData(MiscId, true);
                string id = MiscId.Split('S')[1];
                MISNumber = Convert.ToInt64(id);
                //long.TryParse(id, out MISNumber);
            }*/
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                //var miscList = _IMiscellaneousManager.GetListMiscellaneousByMiscId(UserId, MISNumber, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                var miscList = _IMiscellaneousManager.GetListMiscellaneousByMiscId(UserId, MISNumber, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                return Json(miscList.ToList(), JsonRequestBehavior.AllowGet);
                /*
                foreach (var misc in miscList.rows)
                {
                    if (misc.MISId != null)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(misc.MISId), true);
                        row.cell = new string[12];
                        row.cell[0] = misc.Status.ToString();
                        row.cell[1] = misc.MISId.ToString();
                        row.cell[2] = misc.LocationName;
                        row.cell[3] = misc.VendorName == null ? misc.UserName : misc.VendorName;
                        row.cell[4] = misc.UserName;
                        row.cell[5] = misc.InvoiceAmount.ToString();
                        row.cell[6] = misc.MISDate;
                        row.cell[7] = (misc.Document == "" || misc.Document == null) ? "" : HostingPrefix + MiscellaneousPath.Replace("~", "") + misc.Document;
                        row.cell[8] = misc.Comment;
                        row.cell[9] = misc.MId.ToString();
                        row.cell[10] = misc.LocationId.ToString();
                        row.cell[11] = misc.Vendor.ToString();
                        rowss.Add(row);
                    }
                }
                
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
                */
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 18-OCT-2018
        /// Created For : To Approve Miscellaneous.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public JsonResult ApproveData(List<MiscellaneousListModel> Obj, long LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result;
            string UserName = "";
            string data = "";
            long UserId = 0;
            var resultBill = new Bill();
            long VendorDetailsId = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (Obj.Count > 0)
                {
                    //long VendorId = 0; 
                    UserName = ObjLoginModel.UserName;
                    UserId = ObjLoginModel.UserId;
                    //foreach (var item in Obj)
                    //{
                    //    VendorId = item.Vendor;
                    //}

                    //if (Session["realmId"] != null)
                    //{
                    string realmId = CallbackController.RealMId.ToString();// Session["realmId"].ToString();
                    try
                    {
                        string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                        var principal = User as ClaimsPrincipal;
                        OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                        // Create a ServiceContext with Auth tokens and realmId
                        ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                        serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                        DataService commonServiceQBO = new DataService(serviceContext);
                        // Create a QuickBooks QueryService using ServiceContext
                        QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                        List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor MaxResults 1000").ToList();

                        QueryService<Account> querySvcAccount = new QueryService<Account>(serviceContext);
                        List<Account> accountData = querySvcAccount.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();
                        /// default elite parking service vendor Id passed hardcoded it will will change in future
                        VendorDetailsId = _IVendorManagement.GetCompanyQuickBookId(Convert.ToInt64(10019));
                        // var vendorData = vendorList.Where(x => x.Id == "64").FirstOrDefault();
                        var bill = new Bill();
                        //Vendor Reference
                        var reference = new ReferenceType();
                        var accountRef = new AccountBasedExpenseLineDetail();
                        if (VendorDetailsId > 0)
                        {
                            var vendorData = vendorList.Where(x => x.Id == VendorDetailsId.ToString()).FirstOrDefault();

                            //Vendor Reference    
                            //Set it Hardcoded because need to add vendor , it is mendatory field so as per manager
                            //operating company will pay miscellaneous expense
                            bill.VendorRef = new ReferenceType()
                            {
                                name = vendorData.CompanyName,
                                Value = vendorData.Id
                            };
                        }

                        bill.APAccountRef = new ReferenceType()
                        {
                            name = "Accounts Payable (A/P)",
                            Value = "33"
                        };

                        QueryService<Department> querySvcDept = new QueryService<Department>(serviceContext);
                        var LocationName = _IBillDataManager.GetLocationDataByLocId(LocationId);
                        bill.DepartmentRef = new ReferenceType()
                        {
                            name = LocationName.LocationName,
                            Value = LocationName.QBK_Id.ToString()
                        };
                        Line line = new Line();
                        List<Line> lineList = new List<Line>();
                        Line[] line1 = { };
                        int i = 1;
                        var costArray = new List<long>();
                        var costCodeArray = new List<CostCodeListData>();
                        var costDataModel = new CostCodeListData();
                        decimal amt = 0; decimal? Total = 0;
                        long CostCodeIdData = 0;
                        var date = new DateTime();
                        long MISNumber = 0;
                        foreach (var item in Obj)
                        {
                            string id = item.MISId.Split('S')[1];
                            MISNumber = Convert.ToInt64(id);
                            var costCodeId = _IMiscellaneousManager.MiscellaneoousDataById(MISNumber);
                            var CostCodeData = _IBillDataManager.GetCostCodeData(Convert.ToInt64(costCodeId.CostCode));
                            var dataget = accountData.Where(x => x.Name == CostCodeData.Description).FirstOrDefault();
                            MISNumber = Convert.ToInt64(item.MISNumber);
                            accountRef.AccountRef = new ReferenceType()
                            {
                                name = dataget.Name,
                                Value = dataget.Id
                            };


                            //if (costCodeArray.Count() > 0)
                            //{
                            //    foreach (var tt in costCodeArray)
                            //    {
                            //        if (tt.CostCodeId == costCodeId.CostCode)
                            //        {
                            //            amt = Convert.ToDecimal(item.InvoiceAmount);
                            //            Total = amt + tt.Amount;
                            //            line.Amount = Convert.ToDecimal(Total);
                            //        }
                            //        else
                            //        {
                            //            line.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            //        }
                            //    }
                            //    date = Convert.ToDateTime(item.MISDate);
                            //    line.LineNum = Convert.ToString(i);
                            //    line.AnyIntuitObject = accountRef;
                            //    line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            //    line.DetailTypeSpecified = true;
                            //    line.AmountSpecified = true;
                            //    bill.Balance = Convert.ToDecimal(item.InvoiceAmount);
                            //    line.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            //    line.Description = "Miscellaneous";
                            //    //line1(line );
                            //    lineList.Add(line);
                            //}
                            //else
                            //{

                            //    line.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            //    if (Obj.Count() == 1)
                            //    {
                            //        date = Convert.ToDateTime(item.MISDate);
                            //        line.LineNum = Convert.ToString(i);

                            //        line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            //        line.DetailTypeSpecified = true;
                            //        line.AnyIntuitObject = accountRef;
                            //        line.AmountSpecified = true;
                            //        bill.Balance = Convert.ToDecimal(item.InvoiceAmount);
                            //        line.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            //        line.Description = "Miscellaneous";
                            //        lineList.Add(line);
                            //    //line.
                            //    }
                            //}

                            //costDataModel.CostCodeId = Convert.ToInt64(costCodeId.CostCode);
                            //costDataModel.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            //costCodeArray.Add(costDataModel);
                            date = Convert.ToDateTime(item.MISDate);
                            line.LineNum = Convert.ToString(i);
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            line.AnyIntuitObject = accountRef;
                            line.AmountSpecified = true;
                            line.Amount = Convert.ToDecimal(item.InvoiceAmount);
                            line.Description = "Miscellaneous";
                            //line
                            lineList.Add(line);

                            i++;
                        }

                        var metaData = new ModificationMetaData();
                        metaData.CreateTime = Convert.ToDateTime(date);
                        bill.MetaData = metaData;
                        bill.Line = lineList.ToArray();
                        resultBill = commonServiceQBO.Add(bill) as Bill;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }

                    //}
                    long MiscQbkId = Convert.ToInt64(resultBill.Id);
                    result = _IMiscellaneousManager.ApproveMiscellaneous(Obj, UserName, UserId, LocationId, MiscQbkId, VendorDetailsId);
                    if (result == true)
                    {
                        data = CommonMessage.ApproveMiscellaneous();
                    }
                    else
                    {
                        data = CommonMessage.ErrorMiscellaneous();
                    }
                }
                else
                {
                    data = "No data to Approve please check miscellaneous data in grid.";
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}