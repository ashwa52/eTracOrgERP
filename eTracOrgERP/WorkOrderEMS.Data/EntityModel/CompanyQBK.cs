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
    
    public partial class CompanyQBK
    {
        public long QBK_Id { get; set; }
        public long QBK_RefId { get; set; }
        public long QBK_CMP_Id { get; set; }
        public System.DateTime QBK_Date { get; set; }
        public string QBK_IsActive { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
