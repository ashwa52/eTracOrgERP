using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    public class VendorFacilityModel
    {
        public long VendorType { get; set; }
        public string VendorTypeDisplay { get; set; }
        //Newly added for seperatly add facility for Vendor on dated : 05/March/2019
        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "Product/Service is required")]
        [RegularExpression("^[a-zA-Z .&',-]+$", ErrorMessage = "Special characters or number are not allowed.")]
        public string ProductServiceName { get; set; }

        [Required(ErrorMessage = "Select product/service is required")]
        public string ProductServiceType { get; set; }

        
        //[RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public decimal? UnitCost { get; set; }

        [Required(ErrorMessage = "Unit cost is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string UnitCostForView { get; set; }

        [Required(ErrorMessage = "Tax is required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public decimal? Tax { get; set; }

        [Required(ErrorMessage = "Cost code is required")]
        public long Costcode { get; set; }
        public string CostCodeDisplay { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ProductImage { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public HttpPostedFileBase ProductImageFile { get; set; }
        public long FacilityId { get; set; }
        public long? LocationId { get; set; }
        public long UserId { get; set; }
    }

    public class VendorTypeData
    {
        public long VendorTypeId { get; set; }
        public string VendorTypeName { get; set; }
    }
    public class CostCodeListData
    {
        public long CostCodeId { get; set; }
        public string CostCodeName { get; set; }
        public decimal Amount { get; set; }
    }
}
