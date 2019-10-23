using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models       
{
    public class IdleEmployeeModel
    {
        public String EmployeeName { get; set; }
        public long IdleCount { get; set; }
        public long UserID { get; set; }
        public int Hourdata { get; set; }
        public string Hoursdata { get; set; }
        public string MonthData { get; set; }
        public string WeekData { get; set; }
    }
}
