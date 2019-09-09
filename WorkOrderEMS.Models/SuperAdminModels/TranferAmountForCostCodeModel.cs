using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class TranferAmountForCostCodeModel
    {
        public int NewAllocation { get; set; }
        public int TransferFromOtherCostCode { get; set; }
        public int TransferFromOtherLocation { get; set; }

        public long? BCM_Id { get; set; }
        public long CLM_Id { get; set; }
        public long? CostCode { get; set; }
        public decimal? AssignedPercent { get; set; }
        public int? Year { get; set; }
        public long? BCM_CLM_TransferId { get; set; }
        public long? LocationIdToTransfer { get; set; }
        public string  TransferMode { get; set; }

        [Required]
        [Display(Name = "Budget Amount")]
        public decimal TransferAmt { get; set; }

        [Required]
        [Display(Name = "Select Location")]
        public long Location { get; set; }
        public long CostCodeId { get; set; }
    }
}
