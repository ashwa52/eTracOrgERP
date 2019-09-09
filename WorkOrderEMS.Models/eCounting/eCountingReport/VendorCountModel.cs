using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class VendorCountModel
    {
        public int? PendingPO { get; set; }
        public int? ApprovePO { get; set; }
        public int? PendingBill { get; set; }
        public int? ApproveBill { get; set; }
        public int? PendingPayment { get; set; }
        public decimal? PendingAmount { get; set; }
    }
    public class VendorCountListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<VendorCountModel> rows { get; set; }
    }
}
