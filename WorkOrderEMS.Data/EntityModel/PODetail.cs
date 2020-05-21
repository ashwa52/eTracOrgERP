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
    
    public partial class PODetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PODetail()
        {
            this.LogPODetails = new HashSet<LogPODetail>();
            this.POFacilityItems = new HashSet<POFacilityItem>();
        }
    
        public long POD_Id { get; set; }
        public long POD_LocationId { get; set; }
        public long POD_POT_Id { get; set; }
        public Nullable<long> POD_CMP_Id { get; set; }
        public Nullable<long> POD_QBKId { get; set; }
        public Nullable<System.DateTime> POD_DeliveryDate { get; set; }
        public Nullable<System.DateTime> POD_PODate { get; set; }
        public Nullable<decimal> POD_POAmount { get; set; }
        public Nullable<System.DateTime> POD_ReoccourringBillDate { get; set; }
        public string POD_EmergencyPODocument { get; set; }
        public string POD_IsActive { get; set; }
        public Nullable<long> POD_ReferenceId { get; set; }
        public Nullable<System.DateTime> POD_ReccuringDate { get; set; }
        public string POD_ReccuringStatus { get; set; }
    
        public virtual LocationMaster LocationMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogPODetail> LogPODetails { get; set; }
        public virtual PONumber PONumber { get; set; }
        public virtual POType POType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<POFacilityItem> POFacilityItems { get; set; }
    }
}
