using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class Scheduler
    {

    }
    public class CustomSlotTime
    {
        public long SLT_Id { get; set; }
        public string SLT_fromTime { get; set; }
        public string  SLT_ToTime { get; set; }
        public Nullable<System.DateTime> SLT_Date { get; set; }
        public string SLT_IsActive { get; set; }
    }
}
