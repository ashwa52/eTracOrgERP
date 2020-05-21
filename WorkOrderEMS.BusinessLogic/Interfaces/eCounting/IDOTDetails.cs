using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IDOTDetails
    {
        List<DOTManagementViewDataModel> GetAllDOTList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

    }
}
