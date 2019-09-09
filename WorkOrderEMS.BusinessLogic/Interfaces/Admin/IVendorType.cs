using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Interfaces.Accounts
{
    public interface IVendorType
    {
        VendorTypeModelDetails GetVendorTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        VendorTypeModel SaveVendorType(VendorTypeModel objVendorTypeModel);

        Result DeleteVendorType(long vendorId, long loggedInUserId, string location);

        bool ActiveVendorTypeById(long VendorTypeId, long UserId, string IsActive);
    }
}
