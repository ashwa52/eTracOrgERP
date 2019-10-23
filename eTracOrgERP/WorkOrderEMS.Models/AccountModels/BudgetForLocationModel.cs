using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BudgetForLocationModel
    {
        [Required]
        [Display(Name = "Budget Amount")]
        public decimal? BudgetAmount { get; set; }
        public long LocationId { get; set; }
        public long? CostCode { get; set; }
        public long? MasterCostCode { get; set; }
        public decimal? AssignedPercent { get; set; }
        public decimal? AssignedAmount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public string IsActive { get; set; }
        public string Description { get; set; }
        public string SelectLocationId { get; set; }
        public int? Year { get; set; }
        public long? BCM_Id { get; set; }
        public long CLM_Id { get; set; }
        public string BudgetStatus { get; set; }
        public long? BCM_CLM_TransferId { get; set; }
        public string BudgetSource { get; set; }
        public string Location { get; set; }
    }
    public class BudgetDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<BudgetForLocationModel> rows { get; set; }
    }
}
