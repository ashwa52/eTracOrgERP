using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetPassengerCountReportModel
    {
        public string WeekDays { get; set; }
        public long? Count { get; set; }
        public string Hour { get; set; }
    }
}
