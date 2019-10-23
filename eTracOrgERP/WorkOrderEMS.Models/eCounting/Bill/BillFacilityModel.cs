using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BillFacilityModel
    {
        public long? BillFacilityId { get; set; }
        public long? CompanyFacilityId { get; set; }
        public long? CostCodeId { get; set; }
        public string FacilityType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public string Description { get; set; }
        public long? BillUnit { get; set; }
        public string IsActive { get; set; }
        public long? CostCodeQuickBookId { get; set; }
        public string CostCoseDescription { get; set; }
    }
}
