using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eCountingCountDetailReportModel
    {
        public string PONumber { get; set; }
        public string POType { get; set; }
        public string Vendor { get; set; }
        public string PODate { get; set; }
        public string POAmount { get; set; }
        public string Location { get; set; }
        public string DelayDeys { get; set; }       
    }
    public class PendingPaymentListReportModel
    {
        public string BillNo { get; set; }
        public string LocationName { get; set; }
        public string VendorName { get; set; }
        public string BillType { get; set; }
        public string BillAmount { get; set; }
        public string BillDate { get; set; }
        public string PayMode { get; set; }
        public string DelayDays { get; set; }
    }
}
