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
    
    public partial class LogTaxDetail
    {
        public long LTXD_Id { get; set; }
        public long LTXD_TXD_Id { get; set; }
        public long LTXD_CMP_Id { get; set; }
        public string LTXD_TaxIdNumber { get; set; }
        public string LTXD_TaxDocument { get; set; }
        public long LTXD_ModifiedBy { get; set; }
        public System.DateTime LTXD_ModifiedOn { get; set; }
        public Nullable<long> LTXD_ApprovedBy { get; set; }
        public Nullable<System.DateTime> LTXD_ApprovedOn { get; set; }
        public string LTXD_Comment { get; set; }
        public string LTXD_IsApprove { get; set; }
        public string LTXD_IsActive { get; set; }
    
        public virtual TaxDetail TaxDetail { get; set; }
    }
}