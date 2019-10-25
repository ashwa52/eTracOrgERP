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
    
    public partial class SP_GetAllWorkRequestAssignmentByUsertype_Result2
    {
        public long WorkRequestAssignmentID { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public string WorkRequestTypeName { get; set; }
        public Nullable<long> AssetID { get; set; }
        public long LocationID { get; set; }
        public string LocationName { get; set; }
        public string ProblemDesc { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public string PriorityLevelName { get; set; }
        public string WorkRequestImage { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public string ProjectDesc { get; set; }
        public Nullable<long> WorkRequestStatus { get; set; }
        public string WorkRequestStatusName { get; set; }
        public long RequestBy { get; set; }
        public Nullable<long> AssignToUserId { get; set; }
        public string AssignToUserName { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long WorkRequestProjectType { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public string WorkRequestProjectTypeName { get; set; }
        public string QRCName { get; set; }
        public string CodeName { get; set; }
        public string QRCodeID { get; set; }
        public string CodeID { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public string CreatedByProfile { get; set; }
        public string CreatedByUserName { get; set; }
        public string FacilityRequestType { get; set; }
        public string DisclaimerForm { get; set; }
        public string SurveyForm { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string WeekDaysName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string DriverLicenseNo { get; set; }
        public Nullable<int> VehicleYear { get; set; }
        public Nullable<long> PauseStatus { get; set; }
        public string TotalTimeTaken { get; set; }
        public Nullable<System.DateTime> ConStartTime { get; set; }
        public Nullable<long> eFleetVehicleID { get; set; }
    }
}
