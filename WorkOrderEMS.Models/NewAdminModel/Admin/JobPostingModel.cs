using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class JobPostingModel
    {
        public long JobPostingId { get; set; }
        public AddChartModel AddChartModel { get; set; }
        public string Location { get; set; }
        public string Education { get; set; }
        public string EmployeeStatus { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string JobType { get; set; }
        public string EEO { get; set; }
        public string Requirement { get; set; }
        public string JoiningBonus { get; set; }
        public long HiringManagerId { get; set; }

    }
}
