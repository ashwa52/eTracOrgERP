using System;

namespace WorkOrderEMS.Models
{
    public class Employee_PunchTimeRequestModel
    {
        public long PunchTimeId { get; set; }
        public long UserId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public DateTime? TotalHours { get; set; }
        public bool isActive { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isApproved { get; set; }
        public bool isRejected { get; set; }
        public long ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string AttendanceDateString { get; set; }
        public string AttendanceDayString { get; set; }
        public string InTimeString { get; set; }
        public string OutTimeString { get; set; }
        public string TotalHoursString { get; set; }
    }
}
