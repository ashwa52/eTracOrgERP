using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models.eCounting.DebitMemo
{
    public class DebitMemoModel
    {
        public long DebitId { get; set; }
        public long? Location { get; set; }
        public string LocationName { get; set; }
        public long? VendorId { get; set; }
        public string VendorName { get; set; }
        public long? PurchaseOrderId { get; set; }
        public string ProductOrderName { get; set; }
        public long DebitAmount { get; set; }
        public DateTime? Date { get; set; }
        public string DisplayDate { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }
        public DebitMemoStatus DebitMemoStatus { get; set; }
        public DebitMemoStatus DebitMemoStatusEdit { get; set; }
        public HttpPostedFileBase DebitMemoFile { get; set; }
        public string UploadedDocumentName { get; set; }
        public string UploadedEditDocumentName { get; set; }
        public string DisplayDebitMemoStatus { get; set; }
        public string ActionModeI { get; set; }
        public string ActionModeU { get; set; }
        public int? EditStatus { get; set; }
        public HttpPostedFileBase editNewDocument { get; set; }
    }

    public enum DebitMemoStatus 
    {
        [Display(Name = "Pending")]
        Pending = 1,
        [Display(Name = "Cleared")]
        Cleared = 2,
        [Display(Name = "Cancelled")]
        Cancelled = 3        
    }
}
