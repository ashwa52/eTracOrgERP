using System;

namespace WorkOrderEMS.Models
{
    public class APILogin
    {
        public string Userid { get; set; }
        public string Password { get; set; }
    }
    public class DeviceManager
    {
        public long DeviceId { get; set; }
        public string DeviceCategory { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public string MACAddres { get; set; }
        public string UniqueDeviceId { get; set; }
        public string NotificationTokenId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public bool isApproved { get; set; }
        public long ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string DeviceDisplayName { get; set; }
        public long EmpId { get; set; }
    }

    public class Employee_APIAttendanceModel
    {
        public long UserId { get; set; }
        public long LocationId { get; set; }
        public int Month { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class Employee_APIPunchInOutModel
    {
        public long UserId { get; set; }
        public int AttendanceType { get; set; }
        public DateTime RequestDate { get; set; }
    }
    public class Employee_ProfileModel
    {
        public long UserId { get; set; }
        public string UserEmail { get; set; }
        public string AlternateEmail { get; set; }
        public long UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string ProfileImage { get; set; }
        public string EmployeeID { get; set; }
         
    }

    public class LocationModel
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; }
    }
    public class AttendanceApproval
    {
        public string TimeCardIds { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
    }
}
