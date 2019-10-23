using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface ITreeViewManager
    {
        List<TreeViewModel> ListTreeViewCostCode(long LocationId);
        bool SaveCostCodeIds(List<long> CostCodeIds, long LocationId, long UserId);
    }
}
