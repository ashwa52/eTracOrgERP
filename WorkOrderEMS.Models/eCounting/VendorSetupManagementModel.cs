using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class VendorSetupManagementModel
    {
        public string LocationIds { get; set; }
        public string Status { get; set; }
        public long? VendorId { get; set; }
        public string id { get; set; }
        public long UserId { get; set; }
        public string VendorTypeData { get; set; }
        public long QuickBookVendorId { get; set; }

        [Required]
        [DisplayName("Company Name Legal")]
        [RegularExpression("^[a-zA-Z0-9, -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string CompanyNameLegal { get; set; }

        //[Required]
        [DisplayName("Company Name DBA")]
        //[RegularExpression("^[a-zA-Z0-9, -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string CompanyNameDBA { get; set; }

        [Required]
        [DisplayName("Point Of Contact")]
        [RegularExpression("^[a-zA-Z0-9, -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string PointOfContact { get; set; }

        [Required]
        [DisplayName("Job Title")]
        [RegularExpression("^[a-zA-Z0-9, -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string JobTile { get; set; }


        //public string SSN { get; set; }

        [Required]
        [DisplayName("Address1")]
        [RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address1 { get; set; }

        [Required]
        [DisplayName("Address1 City")]
        [RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address1City { get; set; }

        [Required]
        [DisplayName("Address1 State")]
        public int? Address1State { get; set; }

        public string Address1Country { get; set; }

        //[Required]
        [DisplayName("Address2")]
        //[RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address2 { get; set; }

        //[Required]
        [DisplayName("Address2 City")]
        //[RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address2City { get; set; }

        //[Required]
        [DisplayName("Address2 State")]
        public int? Address2State { get; set; }
        public int? StateAfterIsSame { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        [Remote("IsTaxNumberIsExists", "VendorManagement", AdditionalFields = "VendorId", ErrorMessage = "Tax No already in used.")]
        public string TaxNo { get; set; }
        //public string Address2Country { get; set; }

        [Required]
        [DisplayName("Phone1")]
        [StringLength(12, ErrorMessage = "Invalid Phone1", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string Phone1 { get; set; }
        [StringLength(12, ErrorMessage = "Invalid Phone2", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string Phone2 { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string VendorEmail { get; set; }
        [Url(ErrorMessage = "Invalid Website URL")]
        public string Website { get; set; }

        [Required]
        [DisplayName("Vendor Type")]
        public long? VendorType { get; set; }

        // public int PaymentTerm { get; set; }
        //public string GracePeriod { get; set; }
        public bool IsAddress2Same { get; set; }
        public string SelectedLcation { get; set; }
        public string LocationAllocateId { get; set; }

        //[Required(ErrorMessage = "Company Documents is required")]
        [DisplayName("Company Documents")]
        public string CompanyDocuments { get; set; }
        public string CompanyDocEdit { get; set; }
        public HttpPostedFileBase CompanyDocumentsFile { get; set; }

        public long? CompanyId { get; set; }
        public long PointOfContactId { get; set; }
        public int? CostDuringPeriod { get; set; }
        public string InvoicingFrequency { get; set; }
        [Required(ErrorMessage = "Please add atleat one product/service")]
        public string ProductList { get; set; }
        public Result Result { get; set; }
        public long? CompanyType { get; set; }
        public string IsReccoring { get; set; }

        public string AccountStatus { get; set; }
        public string InsuranceStatus { get; set; }
        public string LicenseStatus { get; set; }
        public long? COD_ID { get; set; }

        public VendorInsuranceModel VendorInsuranceModel { get; set; }
        public VendorFacilityModel VendorFacilityModel { get; set; }
        public VendorContractModel VendorContractModel { get; set; }
        public VendorAccountDetailsModel VendorAccountDetailsModel { get; set; }
        public List<VendorFacilityModel> VendorFacilityListModel { get; set; }
    }
    public class VendorCompanyContractDocument
    {
        public long VendorId { get; set; }
        public string CNT_ContractDocument { get; set; }
        public string CMP_CompanyDocument { get; set; }
    }
    public class LocationAllocation
    {
        public int LocationId { get; set; }
        public string Percentage { get; set; }
    }
    public class CompanyListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<VendorSetupManagementModel> rows { get; set; }
    }
}
