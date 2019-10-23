using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BillDataServiceModel
    {
        public long PODId { get; set; }
        public long MIS_Id { get; set; }
        public long VendorId { get; set; }
        public long LocationId { get; set; }
        public string BillType { get; set; }
        public decimal PoMisBdaAmount { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoiceDocument { get; set; }
        public long ModifiedBy { get; set; }
        public long ApprovedBy { get; set; }
        public string IsActive { get; set; }
        public long UserId { get; set; }
        public Nullable<DateTime> InvoiceDate { get; set; }
        public string Comment { get; set; }
        public string POF_ID { get; set; }
        public string Quantity { get; set; }
        public string POImage { get; set; }
        public string AmountFlag { get; set; }
        //QuickBook Data
        public long QuickBookBillId { get; set; }
        public string CheckedPOId { get; set; }
        public string UnitPrice { get; set; }
        public string CostCode { get; set; }

        //For manual Bill data saving 
        public List<GridDataPO> FacilityListForManualBill { get; set; }
        public int? BillNumber { get; set; }
    }
    
}
