using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class MiscellaneousDetails
    {
        public List<MiscellaneousDetailsmodel> MiscellaneousDetailsmodel { get; set; }
    }
    public class MiscellaneousDetailsmodel
    {
        public long LocationId { get; set; }
        public long? CostCode { get; set; }
        public long MISId { get; set; }
        public long VendorId { get; set; }
        public string Discription { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public string InvoiceDocument { get; set; }
        public long UserId { get; set; }
        public string IsActive { get; set; }
        public string Document { get; set; }
    }
}
