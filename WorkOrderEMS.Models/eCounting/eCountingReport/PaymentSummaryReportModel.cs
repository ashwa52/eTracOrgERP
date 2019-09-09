using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PaymentSummaryReportModel
    {
        public decimal? Bill { get; set; }
        public decimal? MIS { get; set; }
        public decimal? PO { get; set; }
    }
}
