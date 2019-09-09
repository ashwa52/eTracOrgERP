using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interface
{
    public interface IPaymentManager
    {
        PaymentModelDetails GetListPaymentByLocationId(long? UserId, long? LocationID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        PaymentModelDetails GetListPaidtByLocationId(long? UserId, long? LocationID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        //List<PaymentModel> GetAccountDetails(long VendorId);
        List<PaymentModel> GetAccountDetails(long VendorId, long OperatingCompanyId);
        string MakePayment(PaymentModel objPaymentModel, PaymentModel ObjData);
        POApproveRejectModel GetPODetails(PaymentModel objPaymentModel, PaymentModel ObjData);
        long? GetMiscellaneousNumber(long MiscId);
    }
}
