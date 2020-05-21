﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WorkOrderEMS.Models
{
    [DataContract]
    public class eTracLoginModel
    {
        #region Service Login Data


        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username can be not larger than 50 characters")]
        [Display(Name = "User name")]
        [DataMember]
        public string UserName { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "User name")]
        [DataMember]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DataMember]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataMember]
        public string DeviceId { get; set; }

        [DataMember]
        public long DeviceType { get; set; }        //  ServiceDeviceType
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public int Response { get; set; }

        [DataMember]
        public string ServiceAuthKey { get; set; }
        [DataMember]
        public string FormName { get; set; }
        //public string Form { get; set; }
        [DataMember]
        public string EmployeeID { get; set; }
        #endregion Service Login Data

        #region Other Login Data

        //[Required(ErrorMessage = "Recovery email is required")]
        [Display(Name = "Recovery Email")]
        [EmailAddress]
        [DataMember]
        public string RecoveryEmail { get; set; }
        [DataMember]
        public string OldPassword { get; set; }
        [DataMember]
        public string NewPassword { get; set; }

        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public long UserRoleId { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }



        public string UserProfile { get; set; }
        [DataMember]
        public long LocationID { get; set; }

        [DataMember]
        public string FName { get; set; }
        [DataMember]
        public string LName { get; set; }

        public string ContactNo { get; set; }
        [DataMember]
        public string Location { get; set; }
        public string LocationLogo { get; set; }
        public string LocationImage { get; set; }
        [DataMember]
        public string LocationCode { get; set; }
        [DataMember]
        public string LocationServices { get; set; }
        [DataMember]
        public string ProfileImage { get; set; }

        [DataMember]
        public long LogId { get; set; }
        [DataMember]
        public string LoginTime { get; set; }

        [DataMember]
        public string IdleTime { get; set; }

        //Added By Bhushan Dod on 06/16/2015
        //This three fields only for mob bcoz client new requirement mob app running according to selected location
        [DataMember]
        public string LocationIds { get; set; }
        [DataMember]
        public string LocationCodes { get; set; }
        [DataMember]
        public string LocationNames { get; set; }

        public string ReturnUrl { get; set; }

        [DataMember]
        public string TimeZoneName { get; set; }
        [DataMember]
        public long TimeZoneOffset { get; set; }
        [DataMember]
        public bool IsTimeZoneinDaylight { get; set; }

        [DataMember]
        public long UserType { get; set; }

        //This use for new employee app for location of employee
        //Added by : Ashwajit Bansod
        [DataMember]
        public string Lat { get; set; }
        [DataMember]
        public string Long { get; set; }
        [DataMember]
        public string MName { get; set; }
        [DataMember]
        public string LoginId { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Answer { get; set; }
        [DataMember]
        public long? ApplicantId { get; set; }
        [DataMember]
        public long? JobPostingId { get; set; }
        [DataMember]
        public string ApplicantPhoto { get; set; }
        [DataMember]
        public Nullable<DateTime> DateOFJoining { get; set; }

        [DataMember]
        public string RefreshI9Token { get; set; }
        [DataMember]
        public long I9CompanyId { get; set; }
        #endregion Other Login Data
    }
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }

    }
}
