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
    
    public partial class Proc_GetAllWorkAssignedToEmployee_Result
    {
        public Nullable<long> RN { get; set; }
        public string WorkStatus { get; set; }
        public string Priority { get; set; }
        public long WorkRequestAssignmentID { get; set; }
        public long WorkRequestProjectType { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public Nullable<long> FacilityRequestId { get; set; }
        public long WorkOrderCodeID { get; set; }
        public Nullable<long> AssetID { get; set; }
        public Nullable<long> eFleetVehicleID { get; set; }
        public long LocationID { get; set; }
        public string WorkOrderCode { get; set; }
        public string ProblemDesc { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public string WorkRequestImage { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public string ProjectDesc { get; set; }
        public Nullable<long> WorkRequestStatus { get; set; }
        public long RequestBy { get; set; }
        public Nullable<long> AssignToUserId { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public string WorkStatusDesc { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public Nullable<int> VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public string CurrentLocation { get; set; }
        public string CustomerContact { get; set; }
        public string DriverLicenseNo { get; set; }
        public string SignatureImage { get; set; }
        public string DisclaimerForm { get; set; }
        public string SurveyForm { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public string ZipCode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string WeekDays { get; set; }
        public string WeekDaysName { get; set; }
        public string FREmployeeImage { get; set; }
        public string LicensePlateNo { get; set; }
        public Nullable<bool> FrDisclaimerStatus { get; set; }
        public Nullable<System.DateTime> TotalPauseTime { get; set; }
        public Nullable<long> PauseStatus { get; set; }
        public Nullable<System.DateTime> PauseTime { get; set; }
        public Nullable<System.DateTime> ConStartTime { get; set; }
        public string SurveyEmailID { get; set; }
    }
}
