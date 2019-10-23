using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eCountingPaymentDetailsReport
    {
        public long? BillId { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
    }
}
