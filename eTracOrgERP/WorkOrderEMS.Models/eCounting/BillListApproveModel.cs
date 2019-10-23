using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BillListApproveModel
    {
        public long? BillId { get; set; }
        public string VendorName { get; set; }
        public string VendorType { get; set; }
        public string BillDate { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public decimal? BillAmount { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string BillImage { get; set; }
        public string BillType { get; set; }
        public string LocationName { get; set; }
        public long? VendorId { get; set; }
        public string Email { get; set; }
        public long LocationId { get; set; }
        public long LBLL_Id { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        //QuickBook Id
        public long QuickBookBillId { get; set; }
    }
    public class BillListApproveDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<BillListApproveModel> rows { get; set; }
    }
}
