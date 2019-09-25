using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderEMS.Models
{
    public class VendorInsuranceModel
    {
        public long VendorListId { get; set; }
        public string VendorName { get; set; }
        public string Status { get; set; }
        public long InsuranceID { get; set; }
        public long LicenseId { get; set; }
        [Required(ErrorMessage = "Please select License is required")]
        public bool IsCompanyCertificate { get; set; }

        [Required(ErrorMessage = "License is required")]
        [RegularExpression("^[a-zA-Z .&',-]+$", ErrorMessage = "Special characters are not allowed.")]
        public string LicenseName { get; set; }

        [Required(ErrorMessage = "License Number is required")]
        [RegularExpression("([a-zA-Z0-9 ,-]+)")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "License Expiration is required")]
        public Nullable<DateTime> LicenseExpirationDate { get; set; }
        public string LicenseDocument { get; set; }
        public HttpPostedFileBase LicenseDocumentFile { get; set; }

        [Required(ErrorMessage = "Please select insurance is required")]
        public bool IsInsuranceRequired { get; set; }

        [Required(ErrorMessage = "Insurance is required")]
        [RegularExpression("^[a-zA-Z0-9 ,]+$", ErrorMessage = "Special characters are not allowed.")]
        public string InsuranceCarries { get; set; }
       
        [Required(ErrorMessage = "Policy Number is required")]
        [RegularExpression("^[a-zA-Z0-9 -]+$", ErrorMessage = "Special characters are not allowed.")]
        [Remote("CheckInsPolicyNumberIsExists", "VendorManagement", HttpMethod = "POST", ErrorMessage = "Policy Number already in used.")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Insurance Expiration is required")]
        public Nullable<DateTime> InsuranceExpirationDate { get; set; }
        public string InsuranceDocument { get; set; }
        public HttpPostedFileBase InsuranceDocumentFile { get; set; }

    }
    public class InsuranceLicenseListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<VendorInsuranceModel> rows { get; set; }
    }
}
