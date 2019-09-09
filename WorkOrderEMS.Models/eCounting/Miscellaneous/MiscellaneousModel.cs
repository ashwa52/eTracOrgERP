using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class MiscellaneousModel
    {
        public long CostCodeId { get; set; }
        public string CostCodeName { get; set; }
    }
    public class LocationServiceModel
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; }
    }
    public class miscellaneousNumberModel
    {
        public string MISNumber { get; set; }
    }
}
