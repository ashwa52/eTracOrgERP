using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class JobPostingDropDownModel
    {
        public long JobPostingId { get; set; }
        public string JobTitle { get; set; }
        public string RolesAndResponsibility { get; set; }
        public string JobDescription { get; set; }
        public string LocationName { get; set; }
        public long LocationId { get; set; }
        public string HRManagerId { get; set; }
        public int? PositionCount { get; set; }
        public string JobPostingDate { get; set; }
        public string Status { get; set; }
        public string IsExempt { get; set; }
        public string DOT_Status { get; set; }
    }
}
