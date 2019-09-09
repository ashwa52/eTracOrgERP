using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IRuleManager
    {
        RuleModelDetails GetRuleList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        bool SaveRule(RuleModel objRuleModel);
        List<EmployeeListModel> GetUserDataList(long LocationId, long UserId);
        RuleModel GetRuleDetailsById(long ruleId);
    }
}
