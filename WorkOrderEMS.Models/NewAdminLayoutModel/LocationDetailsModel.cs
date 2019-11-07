using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class LocationDetailsModel
    {
        public ListLocationModel ListLocationModel { get; set; }
        public CostCodeModel CostCodeModel { get; set; }
        public BudgetForLocationModel BudgetForLocationModel { get; set; }
        public ContractDetailsModel ContractDetailsModel { get; set; }

        public LocationRuleMappingModel LocationRuleMappingModel { get; set; }
    }
}
