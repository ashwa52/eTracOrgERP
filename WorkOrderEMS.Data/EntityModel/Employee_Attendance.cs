//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee_Attendance
    {
        public long AttendanceId { get; set; }
        public long UserId { get; set; }
        public Nullable<int> AttendanceYear { get; set; }
        public Nullable<int> AttendanceMonth { get; set; }
        public Nullable<int> AttendanceDay { get; set; }
        public Nullable<System.DateTime> AttendanceDate { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public Nullable<System.DateTime> OutTime { get; set; }
        public Nullable<System.TimeSpan> TotalTime { get; set; }
        public bool LeaveStatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> AttendanceType { get; set; }
        public Nullable<int> TotalSeconds { get; set; }
        public Nullable<int> ApprovalStatus { get; set; }
        public string ARUser { get; set; }
    }
}
