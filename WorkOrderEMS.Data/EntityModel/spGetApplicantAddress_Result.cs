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
    
    public partial class spGetApplicantAddress_Result
    {
        public long APA_Id { get; set; }
        public Nullable<long> APA_APT_ApplicantId { get; set; }
        public string APA_StreetAddress { get; set; }
        public string APA_Apartment { get; set; }
        public string APA_City { get; set; }
        public string APA_State { get; set; }
        public Nullable<int> APA_Zip { get; set; }
        public Nullable<System.DateTime> APA_YearsAddressFrom { get; set; }
        public Nullable<System.DateTime> APA_YearsAddressTo { get; set; }
        public Nullable<System.DateTime> APA_Date { get; set; }
        public string APA_IsActive { get; set; }
    }
}