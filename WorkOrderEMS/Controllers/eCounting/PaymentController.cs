using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interface;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class PaymentController : Controller
    {
        // GET: Payment
        private readonly IPaymentManager _IPaymentManager;
        private readonly IBillDataManager _IBillDataManager;
        private readonly IPOTypeDetails _IPOTypeDetails;
        private readonly IVendorManagement _IVendorManagement;
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public PaymentController(IPaymentManager _IPaymentManager, IBillDataManager _IBillDataManager, IVendorManagement _IVendorManagement, IPOTypeDetails _IPOTypeDetails)
        {
            this._IPaymentManager = _IPaymentManager;
            this._IBillDataManager = _IBillDataManager;
            this._IVendorManagement = _IVendorManagement;
            this._IPOTypeDetails = _IPOTypeDetails;
        }
        public ActionResult PaymentView()
        {
            long LocationId = 0;
            eTracLoginModel ObjLoginModel = null;
            long MISNumber = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
            }
            ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-OCT-2018
        /// Created For : To Get Payment bill list as per location Id to pay bill
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
        public JsonResult GetPaymentListByLocation(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null, string BillTypeId = null)
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
            }
            ViewBag.PaymentModeList = _IVendorManagement.PaymentModeList();
            try
            {                
                locationId = 0;//Need to fetch all data // it was previously done need to be discussed
                if (BillTypeId == "0") {// Logic :to show all data 
                    BillTypeId = null;
                }
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    var paymentList = _IPaymentManager.GetListPaymentByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType, BillTypeId).Where(x => !String.IsNullOrEmpty(x.VendorName));
                    var FilterList = paymentList.Where(X => X.VendorName.ToLower().Contains(txtSearch.ToLower())).ToList();
                    return Json(FilterList.ToList(), JsonRequestBehavior.AllowGet);
                }
                else {
                    var paymentList = _IPaymentManager.GetListPaymentByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType, BillTypeId);
                    return Json(paymentList.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            //JQGridResults result = new JQGridResults();
            //List<JQGridRow> rowss = new List<JQGridRow>();
            //sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            //sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            //txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            //try
            //{
            //    locationId = 0;//Need to fetch all data
            //    var paymentList = _IPaymentManager.GetListPaymentByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
            //    foreach (var payment in paymentList.rows)
            //    {
            //        if (payment.BillNo != null)
            //        {
            //            JQGridRow row = new JQGridRow();
            //            row.id = Cryptography.GetEncryptedData(Convert.ToString(payment.BillNo), true);
            //            row.cell = new string[14];
            //            row.cell[0] = payment.BillNo.ToString();
            //            row.cell[1] = payment.LocationName == null ? "N/A" : payment.LocationName;
            //            row.cell[2] = payment.VendorName == null ? "N/A" : payment.VendorName;
            //            row.cell[3] = payment.OperatingCompany == null ? "N/A" : payment.OperatingCompany;
            //            row.cell[4] = payment.BillType == null ? "N/A" : payment.BillType;
            //            row.cell[5] = payment.BillAmount.ToString();
            //            row.cell[6] = payment.BillDate.ToString();
            //            row.cell[7] = payment.GracePeriod == null ? "N/A" : payment.GracePeriod.ToString();
            //            row.cell[8] = (payment.PaymentMode == null && payment.BillType == "MIS") ? "MISC" : (payment.PaymentMode == null && payment.BillType == "ManualBill") ? "ManualBill" : payment.PaymentMode.ToString();
            //            row.cell[9] = payment.Status.ToString();
            //            row.cell[10] = payment.VendorId > 0 ? payment.VendorId.ToString() : "0";
            //            row.cell[11] = payment.OperatingCompanyId > 0 ? payment.OperatingCompanyId.ToString() : "0";
            //            row.cell[12] = payment.LocationId > 0 ? payment.LocationId.ToString() : "0";
            //            row.cell[13] = payment.LLBL_ID.ToString();
            //            rowss.Add(row);
            //        }
            //    }
            //    result.rows = rowss.ToArray();
            //    result.page = Convert.ToInt32(page);
            //    result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
            //    result.records = Convert.ToInt32(TotalRecords.Value);
            //}
            //catch (Exception ex)
            //{ return Json(ex.Message, JsonRequestBehavior.AllowGet); }

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-OCT-2018
        /// Created For : To get Paid bill payment data list by location
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
        public JsonResult GetPaidListByBillID(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null, string BillTypeId = null)
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
            }

            try
            {
                locationId = 0;//Need to fetch all data // was done previously need to be discussed
                if (!string.IsNullOrEmpty(txtSearch))
                {
                    var paymentList = _IPaymentManager.GetListPaidtByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType, BillTypeId).Where(x => !String.IsNullOrEmpty(x.VendorName));
                    var returnFilteredList = paymentList.Where(item => item.VendorName.ToLower().Contains(txtSearch.ToLower()));
                    return Json(returnFilteredList.ToList(), JsonRequestBehavior.AllowGet);
                }
                else {
                    var paymentList = _IPaymentManager.GetListPaidtByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType, BillTypeId);
                    return Json(paymentList.ToList(), JsonRequestBehavior.AllowGet);
                }                
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            //JQGridResults result = new JQGridResults();
            //List<JQGridRow> rowss = new List<JQGridRow>();
            //sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            //sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            //txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            //try
            //{
            //    locationId = 0;//Need to fetch all data
            //    var paymentList = _IPaymentManager.GetListPaidtByLocationId(UserId, locationId, rows, TotalRecords, sidx, sord, locationId, txtSearch, UserType);
            //    foreach (var payment in paymentList.rows)
            //    {
            //        if (payment.BillNo != null)
            //        {
            //            JQGridRow row = new JQGridRow();
            //            row.id = Cryptography.GetEncryptedData(Convert.ToString(payment.BillNo), true);
            //            row.cell = new string[13];
            //            row.cell[0] = payment.BillNo.ToString();
            //            row.cell[1] = payment.LocationName == null ? "N/A" : payment.LocationName; ;
            //            row.cell[2] = payment.VendorName == null ? "N/A" : payment.VendorName;
            //            row.cell[3] = payment.OperatingCompany == null ? "N/A" : payment.OperatingCompany;
            //            row.cell[4] = payment.BillType == null ? "N/A" : payment.BillType;
            //            row.cell[5] = payment.BillAmount.ToString();
            //            row.cell[6] = payment.BillDate.ToString();
            //            row.cell[7] = payment.GracePeriod == null ? "N/A" : payment.GracePeriod.ToString();
            //            row.cell[8] = (payment.PaymentMode == null && payment.BillType == "MIS") ? "MISC" : (payment.PaymentMode == null && payment.BillType == "ManualBill") ? "ManualBill" : payment.PaymentMode.ToString();
            //            row.cell[9] = payment.Description == null ? "N/A" : payment.Description;
            //            row.cell[10] = payment.Status == "P" ? "Paid" : "Cancel";
            //            row.cell[11] = payment.OperatingCompanyId > 0 ? payment.OperatingCompanyId.ToString() : "0";
            //            row.cell[12] = payment.LocationId > 0 ? payment.LocationId.ToString() : "0";
            //            rowss.Add(row);
            //        }
            //    }
            //    result.rows = rowss.ToArray();
            //    result.page = Convert.ToInt32(page);
            //    result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
            //    result.records = Convert.ToInt32(TotalRecords.Value);
            //}
            //catch (Exception ex)
            //{ return Json(ex.Message, JsonRequestBehavior.AllowGet); }            
        }

        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Account details of vendor
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCompanyAccountDetails(long VendorId, long OperatingCompanyId)
        {
            eTracLoginModel ObjLoginModel = null;
            long Vendor = 0;
            var result = new List<PaymentModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (VendorId > 0)
                {
                    result = _IPaymentManager.GetAccountDetails(VendorId, OperatingCompanyId);
                    if(result != null)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            //return null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To Make Payment using differnt Payment mode and save payment data to database
        /// </summary>
        /// <param name="objPaymentModel"></param>
        /// <param name="ObjData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MakePaymentData(PaymentModel objPaymentModel, PaymentModel ObjData)
        {
            eTracLoginModel ObjLoginModel = null;
            long Vendor = 0;
            var result = "";
            var resultPayment = new BillPayment();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (objPaymentModel != null && ObjData != null)
                {
                    objPaymentModel.UserId = ObjLoginModel.UserId;
                    
                    string realmId = CallbackController.RealMId.ToString(); // Session["realmId"].ToString();
                    try
                    {
                        if (realmId != null)
                        {
                            string AccessToken = CallbackController.AccessToken.ToString(); //Session["access_token"].ToString();
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

                            var VendorDetails = _IVendorManagement.GetCompanyQuickBookId(Convert.ToInt64(ObjData.VendorId));//to whom we are sending (ObjData.VendorId)
                            //var getAccountDetails = _IVendorManagement.GetAccountDetailsByVendorId(Convert.ToInt64(objPaymentModel.OpeartorCAD_Id)); // (Convert.ToInt64(ObjData.VendorId));
                            var getAccountDetails = _IVendorManagement.GetAccountDetailsByVendorId(Convert.ToInt64(objPaymentModel.CompanyAccountId)); // (Convert.ToInt64(ObjData.VendorId));
                            var getBill = _IBillDataManager.GetBillQBKId(Convert.ToInt64(ObjData.BillNo));
                            QueryService<Bill> querySvcBill = new QueryService<Bill>(serviceContext);
                            List<Bill> billData = querySvcBill.ExecuteIdsQuery("SELECT * FROM Bill MaxResults 1000").ToList();

                            var bill = billData.Where(x => x.Id == getBill.ToString()).FirstOrDefault();
                            // var vendorData = vendorList.Where(x => x.Id == "64").FirstOrDefault();
                            var payment = new BillPayment();
                            //Vendor Reference
                            var reference = new ReferenceType();
                            var accountRef = new AccountBasedExpenseLineDetail();
                            var billPaymentCheck = new BillPaymentCheck();
                            var billPaymentCredit = new BillPaymentCreditCard();
                            var line = new Line();
                            var lineList = new List<Line>();

                            if (VendorDetails > 0)
                            {
                                var vendorData = vendorList.Where(x => x.Id == VendorDetails.ToString()).FirstOrDefault();
                                //Vendor Reference                                   
                                payment.VendorRef = new ReferenceType()
                                {
                                    name = vendorData.DisplayName,
                                    Value = vendorData.Id
                                };
                            }
                            line.LineNum = "1";
                            var any = new IntuitAnyType();
                            if (getAccountDetails != null)
                            {
                                var ayintuit = new IntuitAnyType();
                                var accountsDetails = accountData.Where(x => x.Id == getAccountDetails.QuickbookAcountId.ToString()).FirstOrDefault();//getAccountDetails.QuickbookAcountId.ToString()
                                if (ObjData.PaymentMode == "Wired")
                                {
                                    payment.PayType = BillPaymentTypeEnum.CreditCard;
                                    var CCD = new CreditCardPayment();
                                    billPaymentCredit.CCAccountRef = new ReferenceType()
                                    {
                                        name = accountsDetails.Name,
                                        Value = accountsDetails.Id,
                                    };
                                    payment.AnyIntuitObject = billPaymentCredit;
                                }
                                else if (ObjData.PaymentMode == "Card")
                                {
                                    payment.PayType = BillPaymentTypeEnum.CreditCard;
                                    var CCD = new CreditCardPayment();
                                    billPaymentCredit.CCAccountRef = new ReferenceType()
                                    {
                                        name = accountsDetails.Name,
                                        Value = accountsDetails.Id,
                                    };
                                    payment.AnyIntuitObject = billPaymentCredit;
                                }
                                else if (ObjData.PaymentMode == "Check")
                                {
                                    var checking = new CheckPayment();
                                    payment.PayType = BillPaymentTypeEnum.Check;
                                    billPaymentCheck.BankAccountRef = new ReferenceType()
                                    {
                                        name = accountsDetails.Name,
                                        Value = accountsDetails.Id
                                    };
                                    billPaymentCheck.PrintStatus = PrintStatusEnum.NeedToPrint;
                                    payment.AnyIntuitObject = billPaymentCheck;
                                }
                            }
                            payment.APAccountRef = new ReferenceType()
                            {
                                name = "Accounts Payable (A/P)",
                                Value = "33"
                            };
                            QueryService<Department> querySvcDept = new QueryService<Department>(serviceContext);
                            var LocationName = _IBillDataManager.GetLocationDataByLocId(Convert.ToInt64(ObjData.LocationId));
                            payment.DepartmentRef = new ReferenceType()
                            {
                                name = LocationName.LocationName,
                                Value = LocationName.QBK_Id.ToString()
                            };

                            line.Amount = Convert.ToDecimal(ObjData.BillAmount);
                            line.AmountSpecified = true;
                            var linkedlist = new List<LinkedTxn>();
                            var linked = new LinkedTxn();
                            linked.TxnId = bill.Id;
                            linked.TxnType = "Bill";

                            linkedlist.Add(linked);
                            line.LinkedTxn = linkedlist.ToArray();
                            line.DetailType = LineDetailTypeEnum.PaymentLineDetail;
                            lineList.Add(line);
                            payment.Line = lineList.ToArray();
                            //payment.PayType = BillPaymentTypeEnum.CreditCard;
                            payment.PayTypeSpecified = true;
                            payment.TotalAmt = Convert.ToDecimal(ObjData.BillAmount);
                            payment.TotalAmtSpecified = true;

                            var metaData = new ModificationMetaData();
                            metaData.CreateTime = Convert.ToDateTime(ObjData.BillDate);
                            payment.MetaData = metaData;
                            payment.PayTypeSpecified = true;
                            resultPayment = commonServiceQBO.Add(payment) as BillPayment;

                            //To close PO after Payment. Update Payment in Quickbook.
                            QueryService<PurchaseOrder> querySvcPO = new QueryService<PurchaseOrder>(serviceContext);
                            List<PurchaseOrder> POList = querySvcPO.ExecuteIdsQuery("SELECT * FROM PurchaseOrder MaxResults 1000").ToList();
                            if (ObjData.BillType == "PO")
                            {
                                var getPOQData = _IPaymentManager.GetPODetails(objPaymentModel, ObjData);
                                if (getPOQData.QuickBookPOId > 0)
                                {
                                    var data = POList.Where(x => x.Id == getPOQData.QuickBookPOId.ToString()).FirstOrDefault();
                                    data.POStatus = PurchaseOrderStatusEnum.Closed;
                                    var update = commonServiceQBO.Update(data) as PurchaseOrder;
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Message = CommonMessage.FailureMessage();
                            result = CommonMessage.FailureMessage();
                            ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = ex.Message;
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                    }

                    result = _IPaymentManager.MakePayment(objPaymentModel, ObjData);
                    if (result != null)
                    {                           
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Message = CommonMessage.FailureMessage();
                    ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                }
     
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            //return null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-April-2019
        /// Created For : To get PO Details ad facility list by PO id
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        public JsonResult GetAllPODetailByPOId(long POId, string _search, long? UserId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            var getResult = new POTypeDataModel();
            var resultPayment = new BillPayment();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                var obj = new PaymentModel();
                obj.BillNo = POId; 
                var getPOByBillNo = _IPaymentManager.GetPODetails(null, obj);
                getResult = _IPOTypeDetails.GetPODetailsById(getPOByBillNo.POId);
                if (getResult != null)
                {
                    getResult.IssueDateDisplay = getResult.IssueDate.ToString();
                    getResult.DeliveryDateDisplay = getResult.DeliveryDate.ToString();
                    //JQGridResults result = new JQGridResults();
                    //List<JQGridRow> rowss = new List<JQGridRow>();
                    //decimal? grandTotal = 0;
                    //sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
                    //sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
                    txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
                    getResult.NewPOTypeDetails = _IPOTypeDetails.GetAllPOFacilityByPOIdList(ObjLoginModel.UserId, getPOByBillNo.POId, rows, TotalRecords, sidx, sord);
                   
                    //foreach (var poFacilityList in getResult.NewPOTypeDetails.rows)
                    //{
                    //    grandTotal += poFacilityList.UnitPrice * poFacilityList.Quantity;
                    //    poFacilityList.TotalPrice = grandTotal;
                    //    //poFacilityList.TotalPrice = poFacilityList.UnitPrice * poFacilityList.Quantity;
                    //    poFacilityList.Total = poFacilityList.UnitPrice * poFacilityList.Quantity;
                    //    JQGridRow row = new JQGridRow();
                    //    row.id = Cryptography.GetEncryptedData(Convert.ToString(poFacilityList.COM_FacilityId), true);
                    //    row.cell = new string[10];
                    //    row.cell[0] = poFacilityList.COM_FacilityId.ToString();
                    //    row.cell[1] = poFacilityList.CostCode.ToString();
                    //    row.cell[2] = poFacilityList.FacilityType.ToString();
                    //    row.cell[3] = poFacilityList.COM_Facility_Desc.ToString();
                    //    row.cell[4] = poFacilityList.UnitPrice.ToString();
                    //    row.cell[5] = poFacilityList.Tax.ToString();
                    //    row.cell[6] = poFacilityList.Quantity.ToString();
                    //    row.cell[7] = poFacilityList.Total.ToString();
                    //    row.cell[8] = poFacilityList.TotalPrice.ToString();
                    //    row.cell[9] = poFacilityList.CostCodeName.ToString();
                    //    // .Add(row);
                    //}
                   //result.rows = rowss.ToArray();
                   // result.page = Convert.ToInt32(page);
                    //result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                    //result.records = Convert.ToInt32(TotalRecords.Value);
                    }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(getResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24/July/2019
        /// Created For : To get miscellaneous number for miscellaneous information.
        /// </summary>
        /// <param name="MiscId"></param>
        /// <returns></returns>
        public JsonResult GetMiscellaneousNumber(long MiscId)
        {
            string MiscellaneousNumber = string.Empty;
            try
            {
                if(MiscId > 0)
                {
                    var result = _IPaymentManager.GetMiscellaneousNumber(MiscId);
                    if (result > 0)
                    {                      
                         MiscellaneousNumber = Cryptography.GetEncryptedData(result.ToString(), true);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(MiscellaneousNumber, JsonRequestBehavior.AllowGet);
        }
    }
}