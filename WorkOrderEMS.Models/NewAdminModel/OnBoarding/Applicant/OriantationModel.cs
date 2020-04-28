using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class OriantationModel
    {
        public long? OTN_ID { get; set; }
        public string OTN_EMP_EmployeeID { get; set; }
        public long OTN_LocationId { get; set; }
        public Nullable<DateTime> ONT_OrientationDate { get; set; }
        public Nullable<TimeSpan> ONT_Orientationtime { get; set; }
        public string ONT_IsActive { get; set; }
        public string Action { get; set; }
    }
}
