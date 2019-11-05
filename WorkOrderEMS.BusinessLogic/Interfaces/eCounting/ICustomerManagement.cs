using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface ICustomerManagement
    {

        VendorAllViewDataModel GetAllCustomerData(long CustmerID, string Status = null);
        //IList<CustomerSetupManagementModel> GetAllCustomerDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
    }
}
