using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface ICompanyAdmin
    {
        CompanyAdminListDetails GetAllCompanyAdmiDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        VendorInsuranceModel GetCompanyDetailsById(long CMPID);
    }
}
