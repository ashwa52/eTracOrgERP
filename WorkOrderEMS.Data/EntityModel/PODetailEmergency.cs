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
    
    public partial class PODetailEmergency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PODetailEmergency()
        {
            this.LogPODetailEmergencies = new HashSet<LogPODetailEmergency>();
        }
    
        public long POE_Id { get; set; }
        public long POE_LocationId { get; set; }
        public long POE_POT_Id { get; set; }
        public string POE_VendorName { get; set; }
        public Nullable<long> POE_QBKId { get; set; }
        public string POE_EmergencyComment { get; set; }
        public decimal POE_POAmount { get; set; }
        public Nullable<System.DateTime> POE_PODate { get; set; }
        public string POE_EmergencyPODocument { get; set; }
        public string POE_IsActive { get; set; }
    
        public virtual LocationMaster LocationMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogPODetailEmergency> LogPODetailEmergencies { get; set; }
        public virtual PONumber PONumber { get; set; }
        public virtual POType POType { get; set; }
    }
}
