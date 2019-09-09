using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.CommonModels
{
    public class UnClosedWOModel
    {
        public string WorkOrder { get; set; }
        public string StartTime { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public string LocationName { get; set; }
        public long WorkRequestId { get; set; }
    }
    public class UnClosedWOModelDetails
      {
          public int total { get; set; }
          public int pageindex { get; set; }
          public int records { get; set; }
          public List<UnClosedWOModel> rows { get; set; }
      }

}
