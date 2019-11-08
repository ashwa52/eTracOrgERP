using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class GraphCountModel
    {
        public string JobTitle { get; set; }
        public int Employee { get; set; }
        public string RequisitionName { get; set; }
        public int Requisition { get; set; }

    }
    public class EmployeeCount
    {
        public string JobTitle { get; set; }
        public int Employee { get; set; }
        
    }
    public class RequisitionCount
    {
        public string RequisitionName { get; set; }
        public int Requisition { get; set; }
    }
}
