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
    
    public partial class ssp_GetContinuousTaskListByEmpID_Result1
    {
        public long WorkRequestAssignmentID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public string ProjectDesc { get; set; }
        public string WorkOrderCode { get; set; }
        public long WorkOrderCodeID { get; set; }
        public string WeekDaysName { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public long LocationID { get; set; }
        public string LocationName { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string AssignByUserName { get; set; }
        public string UserType { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public Nullable<long> WorkRequestStatus { get; set; }
        public string WorkRequestStatusName { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public string WorkRequestTypeCodeName { get; set; }
        public long WorkRequestProjectType { get; set; }
        public string WorkRequestProjectTypeName { get; set; }
        public Nullable<System.DateTime> ConStartTime { get; set; }
    }
}