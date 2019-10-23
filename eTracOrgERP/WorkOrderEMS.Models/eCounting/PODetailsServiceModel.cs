using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PODetailsServiceModel
    {
        public string PONumber { get; set; }
        //public long? Location { get; set; }
        public string Location { get; set; }
        //public long? POType { get; set; }
        public string POType { get; set; }
        //public long? Vendor { get; set; }
        public string Vendor { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public string POD_EmergencyPODocument { get; set; }
        public Nullable<DateTime> IssueDate  { get; set; }
        public Nullable<DateTime> BillDate { get; set; }
        public string InvoicingFrequency { get; set; }
        public string CostDuringPeriod { get; set; }
        public string Comment { get; set; }
        public decimal? Amount { get; set; }
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string PointOfContactName { get; set; }
        public decimal? Total { get; set; }
        public long IsVendorRegister { get; set; }
        public long? VendorId { get; set; }
        public long? LocationId { get; set; }
        public long? POTypeId { get; set; }
        public List<FacilityListData> FacilityListData { get; set; }
        public List<QuestionAnswerData> QuestionAnswerData { get; set; }
    }

    public class FacilityListData
    {
        public long COM_FacilityId { get; set; }
        public string COM_Facility_Desc { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Tax { get; set; }
        public string FacilityType { get; set; }
        public long CostCode { get; set; }
        public long? Quantity { get; set; }
        public long? POF_ID { get; set; }
    }

    public class QuestionAnswerData
    {
        public long POId { get; set; }
        public long POQId { get; set; }
        public long QuestionId { get; set; }
        public string Answer { get; set; }
    }
    
}
