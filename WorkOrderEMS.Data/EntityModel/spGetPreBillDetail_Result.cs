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
    
    public partial class spGetPreBillDetail_Result
    {
        public long LPBL_Id { get; set; }
        public long LPBL_PBL_Id { get; set; }
        public string LocationName { get; set; }
        public string CMP_NameLegal { get; set; }
        public string Employee_Name { get; set; }
        public Nullable<decimal> PBLAmount { get; set; }
        public string PBLDate { get; set; }
        public string LPBL_InvoiceDocument { get; set; }
        public string LPBL_Comment { get; set; }
        public string Status { get; set; }
        public long LPBL_LocationId { get; set; }
        public long LPBL_ModifiedBy { get; set; }
    }
}