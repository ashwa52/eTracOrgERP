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
    
    public partial class Tbl_Client_Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Client_Invoice()
        {
            this.Tbl_Client_Invoice_Item_Details = new HashSet<Tbl_Client_Invoice_Item_Details>();
            this.Tbl_Client_Invoice_Receive_Payment = new HashSet<Tbl_Client_Invoice_Receive_Payment>();
        }
    
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> InvoiceDueDate { get; set; }
        public Nullable<int> InvoiceType { get; set; }
        public string InvoiceStatus { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public Nullable<long> LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationAddress { get; set; }
        public Nullable<long> ContractType { get; set; }
        public string ClientPointOfContactName { get; set; }
        public string PositionTitle { get; set; }
        public Nullable<long> InvoicePaymentTerms { get; set; }
        public Nullable<long> ReasonForOffScheduleInvoice { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> TaxPercentage { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public string Comment { get; set; }
        public string IsActive { get; set; }
        public Nullable<long> EntryBy { get; set; }
        public Nullable<System.DateTime> EntryOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string InvoiceDocument { get; set; }
        public Nullable<decimal> PendingAmount { get; set; }
        public Nullable<long> SubmissionBy { get; set; }
        public Nullable<System.DateTime> SubmissionOn { get; set; }
        public Nullable<long> CancelledBy { get; set; }
        public Nullable<System.DateTime> CancelledOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Client_Invoice_Item_Details> Tbl_Client_Invoice_Item_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Client_Invoice_Receive_Payment> Tbl_Client_Invoice_Receive_Payment { get; set; }
    }
}
