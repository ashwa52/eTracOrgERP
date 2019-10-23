using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
     public class POCommonServiceModel
    {
        public long VendorId { get; set; }
        public long LocationId { get; set; }
        public long UserId { get; set; }
        public long POId { get; set; }
        public string Status { get; set; }
    }

    public class CompanyFacilityListServiceModel
    {
        public List<CompanyFacility> CompanyFacility { get; set; }
        public List<ResourceData> Resourse { get; set; }
        //public List<VendorDetails> CompanyDetails { get; set; }
        public string PointOfContact { get; set; }
        public string InvoicingFrequency { get; set; }
        public int? CostDuringPeriod { get; set; }
        public string Address { get; set; }
        public decimal? AnnualValueOfAggreement { get; set; }
        public long? IdSecondParty { get; set; }
        public long? ContractCompanyTypeId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public long? ContractId { get; set; }
    }

    public class CompanyFacility
    {
        public string COM_Facility_Desc { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Tax { get; set; }
        public long Quantity { get; set; }
        public decimal? TotalPrice { get; set; }
        public long COM_FacilityId { get; set; }
        public string Status { get; set; }
        public long CostCode { get; set; }
        public long CompanyId { get; set; }
        public decimal? RemainingAmount { get; set; }
    }
    public class VendorDetails
    {
        public string PointOfContact { get; set; }
        public string InvoicingFrequency { get; set; }
        public int? CostDuringPeriod { get; set; }
        public string Address { get; set; }
    }
}
