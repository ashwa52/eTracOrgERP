using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class MiscellaneousListModel
    {
        public long LocationId { get; set; }
        public string Address { get; set; }
        public string LocationName { get; set; }
        public string MISId { get; set; }
        public string VendorName { get; set; }
        public string UserName { get; set; }
        public long MISNumber { get; set; }
        public string VendorId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string MISDate { get; set; }
        public string Status { get; set; }
        public string Document { get; set; }
        public string Comment { get; set; }
        public long MId { get; set; }
        public long Vendor { get; set; }
        public long UserId { get; set; }
    }
    public class MiscellaneousListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<MiscellaneousListModel> rows { get; set; }
    }
}
