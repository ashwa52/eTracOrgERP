using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class CreditMemoDataModel
    {
        //public string LocationIds { get; set; }
        //public string Status { get; set; }
        //public long? CustomerId { get; set; }
        public long Id { get; set; }
        public long CreditMemoId { get; set; }
        public long UserId { get; set; }
        //public Result Result { get; set; }

        public string CreditMemoNumber { get; set; }
        public DateTime CreditIssuedDate { get; set; }
        public string CreditIssuedDateDisplay { get; set; }
        public long? CompanyId { get; set; }
        //[Required]
        public string ClientCompanyName { get; set; }
        public string ClientLocationName { get; set; }
        public long? ClientLocationCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationAddress { get; set; }
        [Required]
        public long ContractType { get; set; }
        public string ContractTypeDesc { get; set; }
        public string ClientPointOfContactName { get; set; }
        public string PositionTitle { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceType { get; set; }
        public string InvoiceTypeDesc { get; set; }
        public string InvoiceStatus { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDueDate { get; set; }
        public long InvoicePaymentTerms { get; set; }
        public string InvoicePaymentTermsDesc { get; set; }
        public long? ReasonForOffScheduleInvoice { get; set; }
        public string ReasonForOffScheduleInvoiceDesc { get; set; }
        public List<CreditMemoItemDetails> ListInvoiceItemDetails { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaymentTotal { get; set; }

        public string InvoiceDateDisplay { get; set; }
        public string InvoiceDueDateDisplay { get; set; }
        public string InvoiceStatusDescForCredit { get; set; }
        public string InvoiceDocument { get; set; }
        public string InvoiceDocumentUrl { get; set; }
        public decimal PendingAmount { get; set; }
        public DateTime SubmissionOn { get; set; }
        public string InvoiceLastSenttoclientDate { get; set; }


        //Receive Payment
        public DateTime PaymentReceiveDate { get; set; }
        public string PaymentReceiveDateDisplay { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string ReferenceCheckNo { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        [Required]
        public string DepositStatus { get; set; }
        public string DepositAccount { get; set; }
        public DateTime DepositDate { get; set; }
        public string DepositDateDisplay { get; set; }
        public DateTime EstimatedDepositDate { get; set; }
        public string EstimatedDepositDateDisplay { get; set; }
        public string Comment { get; set; }

        public string EntryByDisplay { get; set; }
        public string EntryOnDisplay { get; set; }
        public string ModifiedByDisplay { get; set; }
        public string ModifiedOnDisplay { get; set; }
        public string DraftCreditMemoNumber { get; set; }
        public DateTime EntryOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool PaymentReminder { get; set; }
        public string IsDraft { get; set; }
        public string EmployeeIssuingCredit { get; set; }
        public string CreditMemoStatus { get; set; }
        public string CreditMemoStatusDesc { get; set; }
        public int CreditMemoType { get; set; }
        public string CreditMemoTypeDesc { get; set; }
        public decimal InvoiceSubTotal { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal NewGrandTotal { get; set; }
        public long InvoiceId { get; set; }
        public string ApprovedByDisplay { get; set; }
        public string ApprovedOnDisplay { get; set; }
    }

    public class CreditMemoItemDetails
    {
        public long Id { get; set; }
        public long SrNo { get; set; }
        //public long CustomerId { get; set; }
        public long ItemNo { get; set; }
        public string ItemNoDesc { get; set; }
        public string ItemDescription { get; set; }
        public long ItemType { get; set; }
        public string ItemTypeDesc { get; set; }
        public long RevenueAccount { get; set; }
        public string RevenueAccountDesc { get; set; }
        public int ItemQty { get; set; }
        public decimal ItemUnitCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CreditAmt { get; set; }
        public string RevenueAcct { get; set; }
        public long CreditReason { get; set; }
        public string CreditReasonDesc { get; set; }
    }

    public class ApproveRejectCreditMemol
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        public string InvoiceId { get; set; }
        //public long Customer { get; set; }
        public long UserId { get; set; }
        public long LocationId { get; set; }
        public string Action { get; set; }
        public long ClientLocationCode { get; set; }
    }

    public class CreditInvoiceCountForGraph
    {
        public Nullable<long> Pending_Approval { get; set; }
        public Nullable<long> Pending_Invoice_Credit { get; set; }
        public Nullable<long> Credit_Applied { get; set; }
        public Nullable<long> Pending_Refund_Payment { get; set; }
        public Nullable<long> Refund_Paid { get; set; }
        public Nullable<long> Cancelled { get; set; }
        public Nullable<long> Total { get; set; }
        public Nullable<long> Draft { get; set; }
    }
}
