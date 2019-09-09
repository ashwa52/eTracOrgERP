using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class SavingServicePOModel
    {
        public List<GridDataPO> CompanyFacility { get; set; }
        public List<QuestionAnswerModel> QusetionAnswer { get; set; }
        public long LocationId { get; set; }
        public long POType { get; set; }
        public long VendorId { get; set; }
        public string PointOfContact { get; set; }
        public string Address { get; set; }
        public string InvoicingFrequency { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public string IsVendorRegister { get; set; }
        public string PONumber { get; set; }
        public long AllocateToLocation { get; set; }
        public string VendorName { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public long UserId { get; set; }
        public string POStatus { get; set; }
        public decimal? Total { get; set; }
    }
}
