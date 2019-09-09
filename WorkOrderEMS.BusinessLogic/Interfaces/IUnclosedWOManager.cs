using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IUnclosedWOManager
    {
        UnClosedWOModelDetails GetAllUnOrderList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
    }
}
