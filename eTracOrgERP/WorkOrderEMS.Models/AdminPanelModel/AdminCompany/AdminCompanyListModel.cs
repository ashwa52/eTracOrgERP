using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AdminCompanyModel
    {
        public long VendorId { get; set; }
        public string CompanyNameLegal { get; set; }
        public string Address { get; set; }
        public string CompanyType { get; set; }
        public string TaxIdNo { get; set; }
        public string StateLicExpDate { get; set; }
        public string StateLicDoc { get; set; }
    }
    public class CompanyAdminListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<AdminCompanyModel> rows { get; set; }
    }
}
