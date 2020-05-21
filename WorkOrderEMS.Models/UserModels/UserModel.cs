using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;


namespace WorkOrderEMS.Models
{

    public class UserModel : IdentityUser
    {

         public long UserId { get; set; }
        public long? ManagerId { get; set; }
        public string DeviceId { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool UpdateMode { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Email ID")]
        [EmailAddress]
        // [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter correct email address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Username requiured.")]
        [DisplayName("User Name")]
        public string AlternateEmail { get; set; }

        public string SubscriptionEmail { get; set; }
        [Display(Name = "Permission Level")]
        [DataMember]
        public long UserType { get; set; }
        [Required]
        [DisplayName("Location Name")]
        public Nullable<long> ProjectID { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        public string Logo { get; set; }
        public string LogoFile { get; set; }

        public Nullable<long> Gender { get; set; }

        [DisplayName("Date of Hire")]
        public string DOB { get; set; }

        [DisplayName("Profile Image")]
        public string myProfileImage { get; set; }

        [Required, FileExtensions(Extensions = "jpg,jpeg,png",
             ErrorMessage = "Specify a Imgae file.")]
        public HttpPostedFileBase ProfileImage { get; set; }


        public string ProfileImageFile { get; set; }

        [DisplayName("Login Active")]
        public bool IsLoginActive { get; set; }

        [DisplayName("Email Verify")]
        public bool IsEmailVerify { get; set; }

        [DisplayName("Select Time Zone")]
        public Nullable<long> TimeZoneId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        [DisplayName("Select Vendor")]
        public Nullable<long> VendorID { get; set; }
        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Hiring Date")]
        public Nullable<System.DateTime> HiringDate { get; set; }
        [Required]
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [DisplayName("Other Job Title")]
        public string JobTitleOther { get; set; }


        [DisplayName("Employee Category")]
        public Nullable<long> EmployeeCategory { get; set; }
        [DisplayName("QRC ID")]
        public Nullable<long> QRCID { get; set; }
        public AddressModel Address { get; set; }

        [DisplayName("Employee ID")]
        public string EmployeeID { get; set; }


        [Required]
        [DisplayName("Select Location")]
        public long Location { get; set; }


        [Required]
        [DisplayName("Select Administrator")]
        public long Administrator { get; set; }

        [DisplayName("Select Manager")]
        public bool IsExistingManager { get; set; }
        [DisplayName("Select Client")]
        public long ExistClientID { get; set; }
        [DisplayName("Select Manager")]
        public long ExistManagerID { get; set; }
        public Nullable<System.DateTime> IdleTimeLimit { get; set; }
        //public long Manager { get; set; }

        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public long Response { get; set; }
        public string SelectedServicesID { get; set; }
        public string SelectedLocationName { get; set; }
        public Nullable<long> SelectedLocationId { get; set; }
        public long SelectedUserType { get; set; }
        [Required]
        public long SelectManager { get; set; }

        public string SignatureImageBase { get; set; }

        //For Client saving
        public string LocationIds { get; set; }
        public string SelectedLocation { get; set; }
        public string Company { get; set; }
        public long? PaymentMode { get; set; }
        public long? PaymentTerm { get; set; }
        public string UserTypeView { get; set; }
        public string QBKId { get; set; }

    }
    public class UserModelList
    {
        public Nullable<long> UserId { get; set; }
        public string UserEmail { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<System.DateTime> HiringDate { get; set; }
        public string CodeName { get; set; }
        public string EmployeeProfile { get; set; }
        public string UserType { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<long> QRCID { get; set; }
        public long RoleId { get; set; }
        public long Location { get; set; }
        public string id { get; set; }
       // public List<UserModelList> ChildrenList { get; set; }

    }


    public class AdminUserForDrop
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
    }
    public class UserModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<UserModel> rows { get; set; }
    }
    public class ShiftModel
    {
        public int Id { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
    public class HolidayManagment    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string HolidayDates { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<HolidayManagment> HMM { get; set; }
        public string HolidayDateString { get; set; }
        public long Location { get; set; }
        public string HolidayType { get; set; }
        public string LocationName { get; set; }
    }

    public class tbl_Staffing_Addition
    {
        public long EventId { get; set; }
        public bool IsHCTS { get; set; }
        public bool IsSTS { get; set; }
        public bool IsAutoSchedule { get; set; }
        public DateTime ScheduleDate { get; set; }
        public long Location { get; set; }
        public long UserLocation { get; set; }
        public int HeadCnt { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
    public class tbl_Staffing_Addition_Details
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string EmployeeId { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string EventColor { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime ToDateTime { get; set; }
        public long Location { get; set; }
    }
    public class VMtbl_Staffing_Addition
    {
        public tbl_Staffing_Addition TSAM { get; set; }
        public tbl_Staffing_Addition_Details TSADM { get; set; }
        public List<tbl_Staffing_Addition_Details> listTSADM { get; set; }
        public List<tbl_LocationSeats> ListLS { get; set; }
    }
        public class Tbl_LeaveType_Setup
    {
        public int TypeId { get; set; }
        public string LeaveDesc { get; set; }
        public decimal LeaveCount { get; set; }
        public string LeaveYear { get; set; }
        public bool IsActive { get; set; }
        public bool IsCarryForward { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
    public class Tbl_Employee_Leave_Management
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveReason { get; set; }
        public string LeaveType { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public int ApprovedRejectBy { get; set; }
        public string RejectReason { get; set; }
        public int IdHid { get; set; }
        public DateTime RejectDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public string Status { get; set; }
        public int LeaveDay { get; set; }
        public List<LeaveManagementchartData> ListLTSM { get; set; }
        public List<Tbl_Employee_Leave_Management> TELM { get; set; }
    }
    public class WebEmployeeAttendance
    {
        public int Id { get; set; }
        public int SrNo { get; set; }
        public string CreatedBy { get; set; }
        public List<WebEmployeeAttendance> HMM { get; set; }
    }

    public class LeaveManagementchartData
    {

        public int TypeId { get; set; }
        public string LeaveDesc { get; set; }
        public decimal TotalLeave { get; set; }
        public decimal TakenLeave { get; set; }
        public decimal RemainingLeave { get; set; }

    }

    public class tbl_LocationSeats
    {
        public int SeatId { get; set; }
        public string LocationSeatName { get; set; }
        public string Colour { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ScheduleOverview
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public long Location { get; set; }
        public string StartTime { get; set; }
        public List<tbl_LocationSeats> ListLS { get; set; }
        public string EmployeeId { get; set; }
        public string ScheduleDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public long UserLocation { get; set; }
        public long EventId { get; set; }
        public string FTTime { get; set; }
    }
    //public class ApplicantSchecduleAvaliblity
    //{
    //    public long ASA_Id { get; set; }
    //    public long ASA_ApplicantId { get; set; }
    //    public string ASA_EMP_EmployeeId { get; set; }
    //    public TimeSpan ASA_AvaliableStartTime { get; set; }
    //    public TimeSpan ASA_AvaliableEndTime { get; set; }
    //    public DateTime ASA_Date { get; set; }
    //    public string ASA_IsActive { get; set; }
    //    public long ASA_AvaliableUserLocation { get; set; }
    //    public char ASA_Action { get; set; }
    //    public string ASA_WeekDay { get; set; }
    //    //public Nullable<DateTime> ASA_AvaliableStartTime { get; set; }
    //    //public Nullable<DateTime> ASA_AvaliableEndTime { get; set; }
        
    //}

}
