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
    
    public partial class EmployeeAddress
    {
        public long EMA_Id { get; set; }
        public string EMA_EMP_EmployeeID { get; set; }
        public string EMA_Address { get; set; }
        public string EMA_City { get; set; }
        public string EMA_State { get; set; }
        public Nullable<int> EMA_Zip { get; set; }
        public Nullable<long> EMA_AddressPhoneNo { get; set; }
        public Nullable<System.DateTime> EMA_Date { get; set; }
        public string EMA_IsActive { get; set; }
    }
}
