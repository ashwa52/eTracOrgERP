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
    
    public partial class LocationCompanyMapping
    {
        public LocationCompanyMapping()
        {
            this.LogLocationCompanyMappings = new HashSet<LogLocationCompanyMapping>();
        }
    
        public long LCM_Id { get; set; }
        public long LCM_CMP_Id { get; set; }
        public long LCM_LocationId { get; set; }
        public string LCM_IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual ICollection<LogLocationCompanyMapping> LogLocationCompanyMappings { get; set; }
    }
}