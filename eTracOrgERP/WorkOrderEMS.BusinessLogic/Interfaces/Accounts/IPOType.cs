using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Interfaces.Accounts
{
    public interface IPOType
    {
        POTypeModelDetails GetPOTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        POTypeModel SavePOType(POTypeModel objPOTypeModel);
        bool ActivePOTypeById(long POTypeId, long UserId, string IsActive);

    }
}
