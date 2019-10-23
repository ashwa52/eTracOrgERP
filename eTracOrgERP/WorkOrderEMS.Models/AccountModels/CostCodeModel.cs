using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class CostCodeModel
    {
        public long CostCodeId { get; set; }
        public Nullable<long> CostCode { get; set; }
        public string Description { get; set; }
        public long LocationId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Result Result { get; set; }
        public long ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserName { get; set; }
        public long ApprovedBy { get; set; }
        public string IsActive { get; set; }
        public long QuickBookCostCodeMasterId { get; set; }
        public long QuickBookCostCodeId { get; set; }
        public string CategoryList { get; set; }
        public string SubCategory { get; set; }
        public string CatagoryValue { get; set; }
    }
    public class CostCodeDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<CostCodeModel> rows { get; set; }
    }
    public class SubCostCodeModel
    {
        public long SubCostCodeId { get; set; }
        public long SubCostCode { get; set; }
        public Nullable<long> CCM_CostCode { get; set; }
        public string Description { get; set; }
        public long LocationId { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Result Result { get; set; }
        public long ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserName { get; set; }
        public long CostCode { get; set; }
        public string IsActive { get; set; }
    }
    public class SubCostCodeDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<SubCostCodeModel> rows { get; set; }
    }
}
