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
    
    public partial class JobTitle
    {
        public long JBT_Id { get; set; }
        public string JBT_JobTitle { get; set; }
        public long JBT_VST_Id { get; set; }
        public Nullable<int> JBT_JobCount { get; set; }
        public Nullable<int> JBT_FillJobCount { get; set; }
        public Nullable<System.DateTime> JBT_Date { get; set; }
        public string JBT_IsActive { get; set; }
    }
}
