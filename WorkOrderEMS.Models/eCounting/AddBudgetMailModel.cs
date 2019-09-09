using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AddBudgetMailModel
    {
        public long LocationId { get; set; }
        public long VendorId { get; set; }
        public long UserId { get; set; }
        public decimal Amount { get; set; }
        public long CostCode { get; set; }
    }
}
