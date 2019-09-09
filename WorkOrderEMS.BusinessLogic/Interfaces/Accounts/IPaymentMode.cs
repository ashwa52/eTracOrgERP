using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Interfaces.Accounts
{
   public interface IPaymentMode
    {     /// <summary>
          /// For getting payment list.
          /// </summary>
          /// <param name="UserId"></param>
          /// <param name="pageIndex"></param>
          /// <param name="numberOfRows"></param>
          /// <param name="sortColumnName"></param>
          /// <param name="sortOrderBy"></param>
          /// <returns></returns>
        PaymentModeDetails GetPaymentModeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        PaymentModeModel SavePaymentMode(PaymentModeModel objPaymentModeModel);
        bool ActivePaymentModeById(long PaymentModeId, long UserId, string IsActive);
    }
}
