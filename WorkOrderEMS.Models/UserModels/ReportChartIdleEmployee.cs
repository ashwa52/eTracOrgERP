using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ReportChartIdleEmployee
    {
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public int? LocationId { get; set; }
        public string IdleTotalTime { get; set; }
        public string Total { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string DifferenceTime { get; set; }
        public long TrackId { get; set; }
        public bool trackStatus { get; set; }
        public TimeSpan? CalculatedDifferenceTime { get; set; }
    }
    public class StartEndAndDifferenceTimeForIdleEmployee
    {
        public DateTime EndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime DifferenceTime { get; set; }
        public string StartTimestring { get; set; }
    }
}
