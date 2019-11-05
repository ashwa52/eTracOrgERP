using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.GlobalSettings
{
    public class Invoices
    {
        public int Id { get; set; }
        
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool IsInvoiceDate { get; set; }


        public string OverrideCode { get; set; }
         public DateTime DueDate { get; set; }
        public int InvoiceType { get; set; }
        public string InvoiceTypeName { get; set; }
        public int LocationId { get; set; }

        public int InvoiceCriteria { get; set; }
        public string InvoiceCriteriaName { get; set; }
        public int Status { get; set; }
        public int WhomYou { get; set; }
        public string WhomYouName { get; set; }

        public int Business { get; set; }
        public string BusinessName { get; set; }
        public int PuposeOfBilling { get; set; }
        public string PuposeOfBillingName { get; set; }
        public int FeeType { get; set; }
        public string FeeTypeName { get; set; }

        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal PendingAmount { get; set; }

        public bool IsCancelled { get; set; }
        public string CancelledBy { get; set; }
        public DateTime CancelledDate { get; set; }

        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }

        public bool IsRejected { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }

        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
