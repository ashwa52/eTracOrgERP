using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class POTypeDataModel
    {
        [Required(ErrorMessage = "Location is required")]
        public long? Location { get; set; }

        [Required(ErrorMessage = "PO Type is required")]
        public long? POType { get; set; }
        public long POId { get; set; }
        public string PONumber { get; set; }
        public string POTypeName { get; set; }

        [Required(ErrorMessage = "Vendor is required")]
        public long? Vendor { get; set; }
        public long? SelectedVendor { get; set; }
        public long PointOfContactId { get; set; }
        public string PointOfContactName { get; set; }
        public string PointOfContactNameHidden { get; set; }
        public string PointOfContactAddress { get; set; }
        public string PointOfContactAddressHidden { get; set; }

        [Required(ErrorMessage = "Issue Date is required")]
        public Nullable<System.DateTime> IssueDate { get; set; }
        public string IssueDateDisplay { get; set; }

        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public string DeliveryDateDisplay { get; set; }

        public string CostDuringPeriod { get; set; }
        public string InvoicingFrequency { get; set; }
        public long AllocateToLocation { get; set; }

        [Required(ErrorMessage = "Bill Date is required")]
        public Nullable<System.DateTime> BillDate { get; set; }
        public string AllocateLocationID { get; set; }
        public string COM_Facility_Desc { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Tax { get; set; }
        public long? Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public long COM_FacilityId { get; set; }
       // public List<ResourceData> Resourse { get; set; }
        public string Status { get; set; }
        public string CostCodeName { get; set; }
        public Result Result { get; set; }

        [Required(ErrorMessage = "Vendor name is required")]
        public string VendorName { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal? Amount { get; set; }
        public string IsVendorRegister { get; set; }
        public string IsInjured { get; set; }
        public string Iscontinuation { get; set; }
        public string IsVendorRequest { get; set; }
        public string IsVendorwillingToBill { get; set; }
        public long UserId { get; set; }
        public long CostCode { get; set; }
        public long CFM_CMP_Id { get; set; }
        public decimal? RemainingAmt { get; set; }
        public string LastRemainingAmount { get; set; }
        public string POD_EmergencyPODocument { get; set; }
        public HttpPostedFileBase POD_EmergencyPODocumentFile { get; set; }
        public List<GridDataPO> CompanyFacility { get; set; }
        public List<QuestionAnswerModel> QuestionAnswer { get; set; }
        public string StatusCalculation { get; set; }
        public string POStatus { get; set; }
        public string FacilityType { get; set; }
        public decimal? Total { get; set; }
        public long QBK_Id { get; set; }

        public List<POTypeDataModel> NewPOTypeDetails { get; set; }
    }
    public class POTypeDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<POTypeDataModel> rows { get; set; }
    }

    public class ResourceData
    {
        public long ResourceId { get; set; }
        public string ResourceName { get; set; }
    }
    public class ListGridDataPO
    {
        public List<GridDataPO> GridPOData { get; set; }
    }
    public class GridDataPO
    {
        public string COM_Facility_Desc { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Tax { get; set; }
        public long Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public long COM_FacilityId { get; set; }
        public long CostCode { get; set; }
        public long CFM_CMP_Id { get; set; }
        public string Resourse { get; set; }
        public string Status { get; set; }
        public long POId { get; set; }
    }
}
