using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Interfaces.Accounts
{
    public interface IContractType
    {
        ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        ContractTypeModel SaveContractType(ContractTypeModel objContractTypeModel);
        bool ActiveContractTypeById(long ContractTypeId, long UserId, string IsActive);
    }
}
