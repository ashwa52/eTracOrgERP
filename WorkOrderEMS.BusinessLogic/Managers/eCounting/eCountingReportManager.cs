using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class eCountingReportManager : IeCountingReport
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        /// <summary>
        /// Created By : Ashwajit Bansod 
        /// Created Date : 31-OCT-2018
        /// Created For : To get Pending Po, Approve PO, Pending bill,approve bill, and pending payment count
        /// </summary>
        /// <returns></returns>
        public VendorCountListDetails GetListVendorCount(long? VendorId, long? Location, DateTime? fromDate, DateTime? toDate, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var objDetails = new VendorCountListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetVendorShowcase(VendorId, fromDate, toDate)
                    .Select(a => new VendorCountModel()
                    {
                       ApproveBill = a.ApprovedBill,
                       ApprovePO = a.ApprovedPO,
                       PendingBill = a.PendingBill,
                       PendingPayment = a.PendingPayment,
                       PendingPO = a.PendingPO,
                       PendingAmount = a.PendingAmount,
                    }).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objDetails.pageindex = pageindex;
                objDetails.total = totalPages;
                objDetails.records = totRecords;
                objDetails.rows = Results.ToList();
                return objDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorCountListDetails GetListVendorCount(long? VendorId, long? Location, DateTime? fromDate, DateTime? toDate, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Vendor Count.", null);
                throw;
            }
        }

        /// <summary>
        /// Created by:Ashwajit bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get all vendor or company list
        /// </summary>
        /// <returns></returns>
        public List<VendorTypeListServiceModel> GetAllVendorList(long LocationId)
        {
            try
            {
                var lstVendor = _workorderems.spGetCompanyList(LocationId).Select(x => new VendorTypeListServiceModel()
                {
                    CompanyNameLegal = x.CMP_NameLegal,
                    CompanyId = x.CMP_Id,
                }).ToList();
                    return lstVendor;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<VendorTypeListServiceModel> GetAllVendorList(long LocationId)", "Exception While Getting List of Vendor.", null);
                throw;
            }

        }

        /// Created By  :Ashwajit Bansod
        /// Created Date : 31-OCT-2018
        /// Created For : To get payment till date as per vendor id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public decimal? GetPaymentTillDateData(int VendorId)
        {
            decimal? Ammount = 0;
            try
            {
                //VendorId = 0;
                Ammount = _workorderems.spGetPayTillDate(VendorId).FirstOrDefault();
                return Ammount;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<VendorTypeListServiceModel> GetAllVendorList(long LocationId)", "Exception While Getting List of Vendor.", null);
                throw;
            }

        }

        /// <summary>Created by Ashawajit Bansod
        /// Get details of All pending details list 
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)
        {
            try
            {
                //string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                //string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
                List<eCountingCountDetailReportModel> lstPendingPOdetails = new List<eCountingCountDetailReportModel>();
                try
                {
                    lstPendingPOdetails = _workorderems.spGetPendingPOList(VendorId, FromDate, ToDate)
                        .Select(a => new eCountingCountDetailReportModel()
                        {
                            DelayDeys = a.Delay_Days.ToString(),
                            Location = a.LocationName,
                            POAmount = a.LPOD_POAmount.ToString(),
                            PODate = a.LPOD_ModifiedOn.ToString("MM/dd/yyyy"),
                            PONumber = "PO"+a.LPOD_POD_Id,
                            POType = a.POT_POType,
                            Vendor = a.CMP_NameLegal
                        }).ToList();

                   //var  lstPendingPO = lstPendingPOdetails
                   //     .Select(a => new eCountingCountDetailReportModel()
                   //     {
                   //         DelayDeys = a.DelayDeys.ToString(),
                   //         Location = a.Location,
                   //         POAmount = a.POAmount.ToString(),
                   //         PODate = a.PODate.ToString(),
                   //         PONumber = "PO" + a.PONumber,
                   //         POType = a.POType,
                   //         Vendor = a.Vendor
                   //     }).ToList<eCountingCountDetailReportModel>();

                    return lstPendingPOdetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstPendingPOdetails);
                    return lstPendingPOdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
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
        public List<eCountingCountDetailReportModel> GetApprovePODetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)
        {
            try
            { 
                List<eCountingCountDetailReportModel> lstPendingPOdetails = new List<eCountingCountDetailReportModel>();
                try
                {
                    lstPendingPOdetails = _workorderems.spGetApprovedPOList(VendorId, FromDate, ToDate)
                        .Select(a => new eCountingCountDetailReportModel()
                        {
                            DelayDeys = a.Delay_Days.ToString(),
                            Location = a.LocationName,
                            POAmount = a.LPOD_POAmount.ToString(),
                            PODate = a.LPOD_ModifiedOn.ToString("MM/dd/yyyy"),
                            PONumber = "PO" + a.LPOD_POD_Id,
                            POType = a.POT_POType,
                            Vendor = a.CMP_NameLegal
                        }).ToList();                   
                    return lstPendingPOdetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstPendingPOdetails);
                    return lstPendingPOdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
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
        public List<eCountingCountDetailReportModel> GetPendingBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)
        {
            try
            {
                List<eCountingCountDetailReportModel> lstPendingBilldetails = new List<eCountingCountDetailReportModel>();
                try
                {
                    lstPendingBilldetails = _workorderems.spGetPendingBillList(VendorId, FromDate, ToDate)
                        .Select(a => new eCountingCountDetailReportModel()
                        {
                            DelayDeys = a.Delay_Days.ToString(),
                            Location = a.LocationName,
                            POAmount = a.BillAmount.ToString(),
                            PODate = a.BillDate.ToString("MM/dd/yyyy"),
                            PONumber =  a.LBLL_Id.ToString(),
                            POType = a.LBLL_BillType,
                            Vendor = a.CMP_NameLegal
                        }).ToList();
                    return lstPendingBilldetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eCountingCountDetailReportModel> GetPendingBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstPendingBilldetails);
                    return lstPendingBilldetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eCountingCountDetailReportModel> GetPendingBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
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
        public List<eCountingCountDetailReportModel> GetAprovedBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)
        {
            try
            {
                List<eCountingCountDetailReportModel> lstApproveBilldetails = new List<eCountingCountDetailReportModel>();
                try
                {
                    lstApproveBilldetails = _workorderems.spGetApprovedBillList(VendorId, FromDate, ToDate)
                        .Select(a => new eCountingCountDetailReportModel()
                        {
                            DelayDeys = a.Delay_Days.ToString(),
                            Location = a.LocationName,
                            POAmount = a.BillAmount.ToString(),
                            PODate = a.BillDate.ToString("MM/dd/yyyy"),
                            PONumber = a.LBLL_Id.ToString(),
                            POType = a.LBLL_BillType,
                            Vendor = a.CMP_NameLegal
                        }).ToList();
                    return lstApproveBilldetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eCountingCountDetailReportModel> GetAprovedBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstApproveBilldetails);
                    return lstApproveBilldetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eCountingCountDetailReportModel> GetAprovedBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
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
        public List<PendingPaymentListReportModel> GetPendingPaymentDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId)
        {
            try
            {
                List<PendingPaymentListReportModel> lstPendingPOdetails = new List<PendingPaymentListReportModel>();
                try
                {
                    lstPendingPOdetails = _workorderems.spGetPendingPaymentList(VendorId, FromDate, ToDate)
                        .Select(a => new PendingPaymentListReportModel()
                        {
                            BillAmount = a.BillAmount.ToString(),
                            BillDate = a.BillDate.ToString("MM/dd/yyyy"),
                            BillNo = a.LBLL_BLL_Id.ToString(),
                            BillType = a.LBLL_BillType,
                            DelayDays = a.Delay_Days.ToString(),
                            LocationName = a.LocationName,
                            //PayMode = a.,
                            VendorName = a.CMP_NameLegal,
                        }).ToList();
                    return lstPendingPOdetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstPendingPOdetails);
                    return lstPendingPOdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
        }

        /// <summary>
        /// Created By : Ashwajit BansodCrea
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public List<PaymentSummaryReportModel> GetPaymentSummaryCount(long? LocationId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                var lstPaymentSummeryCount = new List<PaymentSummaryReportModel>();
                lstPaymentSummeryCount = _workorderems.spGetPaymentSummary(LocationId, FromDate, ToDate).
                    Select(x => new PaymentSummaryReportModel
                    {
                        Bill = x.Bill,
                        MIS = x.MIS,
                        PO = x.PO
                    }).ToList();

                return lstPaymentSummeryCount;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PaymentSummaryReportModel> GetPaymentSummaryCount(long? LocationId, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return null;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-Nov-2018
        /// Created For : To get all details as per Bill Type
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public List<eCountingPaymentDetailsReport> GetPaymentSummaryDetailsReport(long LocationId,string BillType,DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<eCountingPaymentDetailsReport> lstdetails = new List<eCountingPaymentDetailsReport>();
                try
                {
                    lstdetails = _workorderems.spGetPOPaymentSummary(LocationId, FromDate, ToDate, BillType)
                        .Select(a => new eCountingPaymentDetailsReport()
                        {
                            Amount = a.CAT_Amount == 0?"N/A": a.CAT_Amount.ToString(),
                            BillId = a.CAT_BLL_Id,
                            Date = a.CAT_TransactionDate.ToString("MM/dd/yyyy"),
                            LocationName = a.LocationName,
                            UserName = a.VendorName == null ? "N/A" : a.VendorName
                        }).ToList();
                    return lstdetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationId", lstdetails);
                    return lstdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<eCountingCountDetailReportModel> GetPendingPODetailsReport( DateTime FromDate, DateTime ToDate, long? VendorId)", "LocationID", null);
                return null;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-Nov-2018
        /// Create For : To get all location
        /// </summary>
        /// <returns></returns>
        public List<LocationServiceModel> GetAllLocationList()
        {
            try
            {
                var lstLocation = _workorderems.LocationMasters.Join(_workorderems.PermissionDetails, l => l.LocationId, p => p.LocationId,(l,p) => new { l,p})
                      .Where(x => x.l.IsDeleted == false && x.p.PermissionId == 11).Select(x => new LocationServiceModel()
                {
                    LocationId = x.l.LocationId,
                    LocationName = x.l.LocationName,
                }).ToList();
                return lstLocation;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<LocationServiceModel> GetAllLocationList(long LocationId)", "Exception While Getting List of Location.", null);
                throw;
            }

        }
    }
}
