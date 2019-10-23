using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IPaymentTerms
    {
        /// <summary>
        /// For getting payment list.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        PaymentTermsDetails GetPaymentTermsList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        PaymentTermsModel SavePaymentTerms(PaymentTermsModel objPaymentTermsModel);

        bool ActivePaymentTermById(long PaymentTermId, long UserId, string IsActive);
    }
}
