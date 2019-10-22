using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.eCounting.DebitMemo
{
    public class DebitMemoModel
    {
        public int DebitId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public long DebitAmount { get; set; }
        public DateTime? Date { get; set; }
        public string DisplayDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}
