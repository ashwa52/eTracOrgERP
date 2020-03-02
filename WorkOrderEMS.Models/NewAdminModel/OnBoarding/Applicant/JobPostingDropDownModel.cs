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
    }
}
