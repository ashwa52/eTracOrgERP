using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class VendorAllViewDataModel
    {
        public long VendorId { get; set; }
        public long UserId { get; set; }
        public string VendorTypeData { get; set; }
        public string CompanyNameLegal { get; set; }
        public string CompanyNameDBA { get; set; }
        public string PointOfContact { get; set; }
        public string JobTile { get; set; }
        public string Address1 { get; set; }
        public string Address1City { get; set; }
        public int? Address1State { get; set; }
        public string Address1Country { get; set; }
        public string Address2 { get; set; }
        public string Address2City { get; set; }
        public int? Address2State { get; set; }
        public int? StateAfterIsSame { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public long? VendorType { get; set; }
        public bool IsAddress2Same { get; set; }
        public string SelectedLcation { get; set; }
        public string LocationAllocateId { get; set; }
        public int? CostDuringPeriod { get; set; }
        public string InvoicingFrequency { get; set; }
        public string ProductList { get; set; }

        //Insrance and License Model
        public bool IsCompanyCertificate { get; set; }
        public string LicenseName { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseExpirationDate { get; set; }
        public bool IsInsuranceRequired { get; set; }
        public string InsuranceCarries { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuranceExpirationDate { get; set; }

        //Vendor Contract Model
        public long FirstCompany { get; set; }
        public string SecondaryCompany { get; set; }
        public string ContractType { get; set; }
        public string ContractIssuedBy { get; set; }
        public string ContractExecutedBy { get; set; }
        public string PrimaryPaymentMode { get; set; }
        public string PaymentTerm { get; set; }
        public int? GracePeriod { get; set; }
        public string InvoicingFrequecy { get; set; }
        public int? AllocationNeeded { get; set; }
        public decimal? AnnualValueOfAggriment { get; set; }
        public int? MinimumBillAmount { get; set; }
        public string BillDueDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal? LateFine { get; set; }
        public long SectVendorToLocation { get; set; }

        //Vendor Account Details
        public long PaymentMode { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string SwiftOICCode { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public string BankLocation { get; set; }
        public string PolicyNumberAccount { get; set; }
        //List of Vendor Facility
        public List<VendorFacilityModel> VendorFacilityModel { get; set; }

        public List<LocationDataModel> LocationAssignedModel { get; set; }
    }
    public class LocationDataModel
    {
        public string LocationName { get; set; }
        public long LocationId { get; set; }
        public long LLCM_Id { get; set; }
        public long? QBK_Id { get; set; }
    }
    public class ApproveRejectVendorModel
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        public string VendorId { get; set; }
        public long Vendor { get; set; }
        public long UserId { get; set; }
        public long LocationId { get; set; }
        public string LLCM_Id { get; set; }
    }
}
