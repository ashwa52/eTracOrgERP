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
    
    public partial class CostCode
    {
        public long CCD_CostCode { get; set; }
        public long CCD_QBKId { get; set; }
        public long CCD_CCM_CostCode { get; set; }
        public string CCD_Description { get; set; }
        public string CCD_FacilityType { get; set; }
        public string CCD_IsActive { get; set; }
    
        public virtual CostCodeMaster CostCodeMaster { get; set; }
    }
}