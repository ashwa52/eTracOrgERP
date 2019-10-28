using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IeCountingReport
    {
        List<VendorCountModel> GetListVendorCount(long? VendorId, long? Location, DateTime? fromDate, DateTime? toDate, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        List<VendorTypeListServiceModel> GetAllVendorList(long LocationId);
        decimal? GetPaymentTillDateData(int VendorId);
        List<eCountingCountDetailReportModel> GetPendingPODetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId);
        List<eCountingCountDetailReportModel> GetApprovePODetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId);
        List<eCountingCountDetailReportModel> GetPendingBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId);
        List<eCountingCountDetailReportModel> GetAprovedBillDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId);
        List<PendingPaymentListReportModel> GetPendingPaymentDetailsReport(DateTime FromDate, DateTime ToDate, long? VendorId);
        List<PaymentSummaryReportModel> GetPaymentSummaryCount(long? VendorId, DateTime? FromDate, DateTime? ToDate);
        List<LocationServiceModel> GetAllLocationList();
        List<eCountingPaymentDetailsReport> GetPaymentSummaryDetailsReport(long LocationId, string BillType, DateTime FromDate, DateTime ToDate);
    }
}
