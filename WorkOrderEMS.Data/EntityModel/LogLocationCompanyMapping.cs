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
    
    public partial class LogLocationCompanyMapping
    {
        public long LLCM_Id { get; set; }
        public long LLCM_LCM_Id { get; set; }
        public long LLCM_CMP_Id { get; set; }
        public long LLCM_LocationId { get; set; }
        public long LLCM_ModifiedBy { get; set; }
        public System.DateTime LLCM_ModifiedOn { get; set; }
        public Nullable<long> LLCM_ApprovedBy { get; set; }
        public Nullable<System.DateTime> LLCM_ApprovedOn { get; set; }
        public string LLCM_Comment { get; set; }
        public string LLCM_IsApprove { get; set; }
        public string LLCM_IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual LocationCompanyMapping LocationCompanyMapping { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
    }
}
