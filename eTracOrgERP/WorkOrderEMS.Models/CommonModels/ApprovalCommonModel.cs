using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ApprovalCommonModel<T>
    {
        public T ApprovalData { get; set; }
    }
    public class Approval
    {
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public string ModuleName { get; set; }
        public string CurrentLevel { get; set; }
        public string Email { get; set; }
        public string ManagerName { get; set; }
        public string RuleLevel { get; set; }
        public long RuleId { get; set; }
        public string RuleName { get; set; }
        public string DeviceId { get; set; }
    }
    public class ApprovalInput
    {
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public string ModuleName { get; set; }
        public string CurrentLevel { get; set; }
    }

}
