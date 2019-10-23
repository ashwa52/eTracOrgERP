using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ManagerAppModel
    {
        public long LocationId { get; set; }
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
        public string WorkOrderId { get; set; }
        public long WorkRequestCodeId { get; set; }
        public string WorkRequestCode { get; set; }
        public Nullable<DateTime> AssignedTime { get; set; }
        public string AssignedTimeInterval { get; set; }
        public string TimeZoneName { get; set; }
        public long TimeZoneOffset { get; set; }
        public bool IsTimeZoneinDaylight { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
    }
    public class DashboardCountModel
    {
        public Nullable<long> WorkRequestCount { get; set; }
        public Nullable<long> DarCount { get; set; }
        public Nullable<long> FacilityRequestCount { get; set; }
        public Nullable<long> ContinuousRequestCount { get; set; }
        public Nullable<long> MissedContinuesWOCount { get; set; }
        public Nullable<long> CompletedContinuesCount { get; set; }
        public Nullable<long> InProgressContinuesCount { get; set; }
        public Nullable<long> LocationId { get; set; }
    }
}
