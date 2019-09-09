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

namespace WorkOrderEMS.Controllers.eCounting
{
    public class BillController : Controller
    {
        private readonly IBillDataManager _IBillDataManager;
        private readonly IVendorManagement _IVendorManagement;
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string BillPath = Convert.ToString(ConfigurationManager.AppSettings["BillImage"], CultureInfo.InvariantCulture);
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];

        public BillController(IBillDataManager _IBillDataManager, IVendorManagement _IVendorManagement)
        {
            this._IBillDataManager = _IBillDataManager;
            this._IVendorManagement = _IVendorManagement;
        }
        // GET: Bill
        //public ActionResult ListBillView()
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    if (Session["eTrac"] != null)
        //    {
        //        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);                
        //    }
        //    return View();
        //}

            /// <summary>
            /// Created By: Ashwajit Bansod
            /// Created Date : 24-Dec-2018
            /// Created For : To View Pre Bill Data.
            /// </summary>
            /// <returns></returns>
        public ActionResult ListPreBillView()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return View();
        }

        /// <summary>
        /// Created By: Ashwajit bansod
        /// Created Date : 19-10-2018
        /// Created For : To get all bill data
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
        //[HttpGet]
        //public JsonResult GetListBill(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    if (Session["eTrac"] != null)
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
        //        var billList = _IBillDataManager.GetListBill(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
        //        foreach (var bill in billList.rows)
        //        {
        //            if (bill.BillId != null)
        //            {
        //                JQGridRow row = new JQGridRow();
        //                row.id = Cryptography.GetEncryptedData(Convert.ToString(bill.BillId), true);
        //                row.cell = new string[10];
        //                row.cell[0] = bill.BillId.ToString();
        //                row.cell[1] = bill.VendorName;
        //                row.cell[2] = bill. VendorType;
        //                row.cell[3] = bill.BillDate.ToString();
        //                row.cell[4] = bill.BillAmount.ToString();
        //                row.cell[5] = bill.InvoiceDate.ToString();
        //                //row.cell[6] = bill.BillType;
        //                row.cell[6] = bill.Status;
        //                row.cell[7] = bill.Comment; 
        //                row.cell[8] = (bill.BillImage == "" || bill.BillImage == null) ? HostingPrefix + ProfileImagePath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + BillPath.Replace("~", "") + bill.BillImage; 
        //                row.cell[9] = bill.LBLL_Id.ToString();
        //                rowss.Add(row);
        //            }
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
        /// Created By: Ashwajit bansod
        /// Created Date : 19-10-2018
        /// Created For : To get all pre bill data
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
        public JsonResult GetListPreBill(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
                var billList = _IBillDataManager.GetListPreBill(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
                foreach (var bill in billList.rows)
                {
                    if (bill.BillId != null)
                    {
                        JQGridRow row = new JQGridRow();
                        row.id = Cryptography.GetEncryptedData(Convert.ToString(bill.BillId), true);
                        row.cell = new string[11];
                        row.cell[0] = bill.BillId.ToString();
                        row.cell[1] = bill.VendorName;
                        row.cell[2] = bill.EmployeeName;
                        row.cell[3] = bill.VendorType;
                        row.cell[4] = bill.BillDate.ToString();
                        row.cell[5] = bill.BillAmount.ToString();
                        //row.cell[5] = bill.InvoiceDate.ToString();
                        //row.cell[6] = bill.BillType;
                        row.cell[6] = bill.Status == "W" ?"Pending": bill.Status == "Y" ?"Approved":"Reject";
                        row.cell[7] = bill.Comment == null?"N/A": bill.Comment;
                        row.cell[8] = (bill.BillImage == "" || bill.BillImage == null) ? HostingPrefix + ProfileImagePath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + BillPath.Replace("~", "") + bill.BillImage;
                        row.cell[9] = bill.LBLL_Id.ToString();
                        row.cell[10] = bill.VendorId.ToString();
                        rowss.Add(row);
                    }
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
        /// Created By : Ashwajit bansod
        /// Created Date : 20-OCT-2018
        /// Created For : To Approve and reject bill
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveBillData(BillListApproveModel Obj,long LocationId, List<BillFacilityModel> FacilityData)
        {
            eTracLoginModel ObjLoginModel = null;
            string result="";
            string UserName = "";
            string data = "";
            long UserId = 0;
            var resultBill = new Bill();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (Obj != null && Obj.BillId > 0)
                {
                    UserName = ObjLoginModel.UserName;
                    UserId = ObjLoginModel.UserId;
                    //if (Session["realmId"] != null)
                    //{
                    string realmId = CallbackController.RealMId.ToString();// Session["realmId"].ToString();
                    try
                    {
                        string AccessToken = CallbackController.AccessToken.ToString(); //Session["access_token"].ToString();
                        if (AccessToken == null)
                        {
                            AccessToken = Session["refresh_token"].ToString();
                            AccessToken = CallbackController.AccessToken.ToString(); 
                        }
                        var principal = User as ClaimsPrincipal;
                        OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);

                        // Create a ServiceContext with Auth tokens and realmId
                        ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                        serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                        DataService commonServiceQBO = new DataService(serviceContext);
                        // Create a QuickBooks QueryService using ServiceContext
                        QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                        List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor MaxResults 1000").ToList();

                        QueryService<Account> querySvcBill = new QueryService<Account>(serviceContext);
                        List<Account> BillList = querySvcBill.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000").ToList();
                        var VendorDetails = _IVendorManagement.GetCompanyQuickBookId(Convert.ToInt64(Obj.VendorId));
                        //var vendorData = vendorList.Where(x => x.Id == "64").FirstOrDefault();
                        //var dataget = BillList.Where(x => x.Name == "Sample1").FirstOrDefault();
                        var bill = new Bill();
                        //Vendor Reference
                        var reference = new ReferenceType();
                        var accountRef = new AccountBasedExpenseLineDetail();
                        if (VendorDetails > 0)
                        {
                            var vendorData = vendorList.Where(x => x.Id == VendorDetails.ToString()).FirstOrDefault();
                            //Vendor Reference                                   
                            bill.VendorRef = new ReferenceType()
                            {
                                name = vendorData.DisplayName,
                                Value = vendorData.Id
                            };
                        }                        
                        //End Vendor Reference
                        bill.TotalAmt = Convert.ToDecimal(Obj.BillAmount);
                        
                        var metaData = new ModificationMetaData();
                        metaData.CreateTime = Convert.ToDateTime(Obj.InvoiceDate);
                        bill.MetaData = metaData;
                        //End Time
                        var LocationName = _IBillDataManager.GetLocationDataByLocId(LocationId);
                        bill.DepartmentRef = new ReferenceType()
                        {
                            name = LocationName.LocationName,
                            Value = LocationName.QBK_Id.ToString()
                        };
                        Line line = new Line();
                        List<Line> lineList = new List<Line>();
                        bill.APAccountRef = new ReferenceType()
                        {
                            name = "Accounts Payable (A/P)",
                            Value = "33"
                        };
                        int i = 1;
                        var costArray = new List<long>();
                        var costCodeArray = new List<CostCodeListData>();
                        var costDataModel = new CostCodeListData();
                        decimal amt = 0; decimal? Total = 0;
                        if (FacilityData != null && FacilityData.Count() > 0)
                        {
                            foreach (var item in FacilityData)
                            {
                                long CostCodeId = Convert.ToInt64(item.CostCodeId);
                                var costCodeName = _IBillDataManager.GetCostCodeData(CostCodeId);
                            
                                var dataget = BillList.Where(x => x.Name == costCodeName.Description).FirstOrDefault();
                                accountRef.AccountRef = new ReferenceType()
                                {
                                    name = dataget.Name,
                                    Value = dataget.Id
                                };
                                if (costCodeArray.Count() > 0)
                                {
                                    foreach (var tt in costCodeArray)
                                    {
                                        if (tt.CostCodeId == item.CostCodeId)
                                        {
                                            amt = Convert.ToDecimal(item.Amount);
                                            Total = amt + tt.Amount;
                                            line.Amount = Convert.ToDecimal(Total);
                                        }
                                        else
                                        {
                                            line.Amount = Convert.ToDecimal(item.Amount);
                                        }
                                    }
                                    line.AnyIntuitObject = accountRef;
                                    line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                    line.DetailTypeSpecified = true;
                                    //line.Amount = Convert.ToDecimal(item.Amount);
                                    // line.Amount = Convert.ToDecimal(objBillDataServiceModel.InvoiceAmount);
                                    line.AmountSpecified = true;
                                    line.LineNum = Convert.ToString(i);
                                    line.Description = "Manual Bill";
                                    lineList.Add(line);
                                }
                                else
                                {

                                    line.Amount = Convert.ToDecimal(item.Amount);
                                    if (FacilityData.Count() == 1)
                                    {
                                        line.AnyIntuitObject = accountRef;
                                        line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                        line.DetailTypeSpecified = true;
                                        //line.Amount = Convert.ToDecimal(item.Amount);
                                        // line.Amount = Convert.ToDecimal(objBillDataServiceModel.InvoiceAmount);
                                        line.AmountSpecified = true;
                                        line.LineNum = Convert.ToString(i);
                                        line.Description = "Manual Bill";
                                        lineList.Add(line);
                                    }
                                }

                                costDataModel.CostCodeId = Convert.ToInt64(item.CostCodeId);
                                costDataModel.Amount = Convert.ToDecimal(item.Amount);
                                costCodeArray.Add(costDataModel);
                                i++;
                            }
                        }
                        else
                        {
                            line.Amount = Convert.ToDecimal(Obj.BillAmount);
                            var dataget = BillList.Where(x => x.Name == "Other Expenses").FirstOrDefault();
                            accountRef.AccountRef = new ReferenceType()
                            {
                                name = dataget.Name,
                                Value = dataget.Id
                            };
                            line.AnyIntuitObject = accountRef;
                            line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                            line.DetailTypeSpecified = true;
                            //line.Amount = Convert.ToDecimal(item.Amount);
                            // line.Amount = Convert.ToDecimal(objBillDataServiceModel.InvoiceAmount);
                            line.AmountSpecified = true;
                            line.LineNum = Convert.ToString(i);
                            line.Description = "Manual Bill";
                            lineList.Add(line);
                        }
                        bill.Line = lineList.ToArray();

                        // bill.ref
                        resultBill = commonServiceQBO.Add(bill) as Bill;
                        }
                        catch (Exception ex)
                        {
                            return Json(ex.Message, JsonRequestBehavior.AllowGet);
                        }

                    //}
                    Obj.QuickBookBillId = Convert.ToInt64(resultBill.Id);
                    result = _IBillDataManager.ApproveBill(Obj, UserName, UserId, LocationId);                   
                }
                else
                {
                    result = "No data to Approve please check bill data in grid.";
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Dec-2018
        /// Created For  : To get all bill facility list data.
        /// </summary>
        /// <param name="BillId"></param>
        /// <returns></returns>
        public JsonResult BillFacilityListData(string BillId)
        {
            eTracLoginModel ObjLoginModel = null;
            long LocationId = 0;
            long Id = 0;
            var AllBillFacilityList = new List<BillFacilityModel>();
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(BillId))
                {
                    BillId = Cryptography.GetDecryptedData(BillId, true);
                    long.TryParse(BillId, out Id);
                }
                if (Id > 0)
                {
                     AllBillFacilityList = _IBillDataManager.GetAllBillFacilityListById(Id);
                    return Json(AllBillFacilityList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }          
        }
    }
}