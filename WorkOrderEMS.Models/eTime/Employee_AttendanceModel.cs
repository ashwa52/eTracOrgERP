using System;

namespace WorkOrderEMS.Models
{
    public class Employee_AttendanceModel
    {
        public long AttendanceId { get; set; }
        public long UserId { get; set; }
        public int AttendanceYear { get; set; }
        public int AttendanceMonth { get; set; }
        public int AttendanceDay { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? TotalHours { get; set; }
        public bool LeaveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string AttendanceDateString { get; set; }
        public string AttendanceDayString { get; set; }
        public string InTimeString { get; set; }
        public string OutTimeString { get; set; }
        public string TotalHoursString { get; set; }

        public DateTime Datum { get; set; }
        public int RYear { get; set; }
        public int RMonth { get; set; }
        public int RDay { get; set; }
        public string dyName { get; set; }
        public string DatumString { get; set; }
        public string Present { get; set; }
    }
}
