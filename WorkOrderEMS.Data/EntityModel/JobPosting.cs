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
    
    public partial class JobPosting
    {
        public long JPS_JobPostingId { get; set; }
        public Nullable<long> JPS_JobPostingIdRecruitee { get; set; }
        public long JPS_JobTitleID { get; set; }
        public string JPS_HiringManagerID { get; set; }
        public string JPS_JobTitle { get; set; }
        public string JPS_HiringManager { get; set; }
        public Nullable<int> JPS_NumberOfPost { get; set; }
        public Nullable<System.DateTime> JPS_DateClose { get; set; }
        public Nullable<System.DateTime> JPS_Date { get; set; }
        public string JPS_IsActive { get; set; }
        public Nullable<long> JPS_LocationId { get; set; }
        public string JPS_DrivingType { get; set; }
    }
}
