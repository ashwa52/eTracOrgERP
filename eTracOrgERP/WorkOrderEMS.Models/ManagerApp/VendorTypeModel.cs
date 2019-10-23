using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.AccountModels
{
    public class VendorTypeModel
    {
        public long Vendor_Id { get; set; }

        [MaxLength(20, ErrorMessage = "Vendor Type cannot be greater than 20 characters ")]
        [Required(ErrorMessage ="Required")]
        [DisplayName("Vendor Type")]
        public string VendorType { get; set; }
        public string Vendor_IsActive { get; set; }
        public Result Result { get; set; }
    }
    public class VendorTypeModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<VendorTypeModel> rows { get; set; }
    }
}
