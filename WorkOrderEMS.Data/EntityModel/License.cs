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
    
    public partial class License
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public License()
        {
            this.LogLicenses = new HashSet<LogLicense>();
        }
    
        public long LNC_Id { get; set; }
        public long LNC_CMP_Id { get; set; }
        public string LNC_LicenseName { get; set; }
        public string LNC_LicenseNumber { get; set; }
        public System.DateTime LNC_ExpirationDate { get; set; }
        public string LNC_LicenseDocument { get; set; }
        public string LNC_IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogLicense> LogLicenses { get; set; }
    }
}
