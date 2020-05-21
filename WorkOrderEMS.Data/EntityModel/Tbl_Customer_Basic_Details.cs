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
    
    public partial class Tbl_Customer_Basic_Details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Customer_Basic_Details()
        {
            this.Tbl_Log_Customer_Basic_Details = new HashSet<Tbl_Log_Customer_Basic_Details>();
        }
    
        public long Id { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string Address1 { get; set; }
        public string Addr1City { get; set; }
        public Nullable<int> Addr1StateId { get; set; }
        public Nullable<int> Addr1CountryId { get; set; }
        public string ZipCode1 { get; set; }
        public Nullable<bool> IsAddress2Same { get; set; }
        public string Address2 { get; set; }
        public string Addr2City { get; set; }
        public Nullable<int> Addr2StateId { get; set; }
        public Nullable<int> Addr2CountryId { get; set; }
        public string ZipCode2 { get; set; }
        public string DLNo { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public Nullable<int> MethodOfCommunication { get; set; }
        public Nullable<int> ParkingFacilityLocation { get; set; }
        public Nullable<decimal> MonthlyPrice { get; set; }
        public Nullable<bool> IsAllowToSendText { get; set; }
        public string CustomerUserName { get; set; }
        public string CustomerPassword { get; set; }
        public Nullable<int> SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public Nullable<bool> IsMonthlyAppointmentSchedule { get; set; }
        public Nullable<System.DateTime> ScheduleAppointDate { get; set; }
        public string ScheduleAppointTime { get; set; }
        public string IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Basic_Details> Tbl_Log_Customer_Basic_Details { get; set; }
    }
}
