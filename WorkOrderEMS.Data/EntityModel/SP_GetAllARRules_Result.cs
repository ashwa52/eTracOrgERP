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
    
    public partial class SP_GetAllARRules_Result
    {
        public Nullable<long> ROWNUM { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string Condition { get; set; }
        public string Settings { get; set; }
        public Nullable<bool> Status { get; set; }
        public string EntryBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}