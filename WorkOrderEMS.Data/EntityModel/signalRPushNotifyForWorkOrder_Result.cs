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
    
    public partial class signalRPushNotifyForWorkOrder_Result
    {
        public long WorkRequestAssignmentID { get; set; }
        public string WorkOrderCode { get; set; }
        public long WorkOrderCodeID { get; set; }
        public long WorkRequestProjectType { get; set; }
        public long LocationID { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
