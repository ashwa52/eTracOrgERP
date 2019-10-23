using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    public class VendorContractModel
    {
        [Required(ErrorMessage = "First company is required")]
        public long FirstCompany { get; set; }
        public long VendorType { get; set; }

        [Required(ErrorMessage = "Contract type is required")]
        public long ContractType { get; set; }

        [Required(ErrorMessage = "Contract issued by is required")]
        public string ContractIssuedBy { get; set; }

        [Required(ErrorMessage = "Contract executed by is required")]
        public string ContractExecutedBy { get; set; }

        [Required(ErrorMessage = "Payment mode is required")]
        public long PrimaryPaymentMode { get; set; }

        [Required(ErrorMessage = "Payment term is required")]
        public long PaymentTerm { get; set; }

        [Required(ErrorMessage = "Grace period is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public int? GracePeriod { get; set; }

        [Required(ErrorMessage = "Invoicing frequency is required")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "Special characters are not allowed.")]
        public string InvoicingFrequecy { get; set; }

        [Required(ErrorMessage = "Allocation needed is required")]
        public int? AllocationNeeded { get; set; }

        //[Required(ErrorMessage = "Cost during period is required")]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public int? CostDuringPeriod { get; set; }

        [Required(ErrorMessage = "Cost during period is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string CostDuringPeriodForView { get; set; }

        
        //[RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public long AnnualValueOfAggriment { get; set; }

        [Required(ErrorMessage = "Annual value of aggriment is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string AnnualValueOfAggrimentForView { get; set; }

        
        //[RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public decimal MinimumBillAmount { get; set; }

        [Required(ErrorMessage = "Minimum bill amount is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string MinimumBillAmountForView { get; set; }

        [Required(ErrorMessage = "Bill Due Date is required")]
        public Nullable<DateTime> BillDueDate { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public Nullable<DateTime> StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public Nullable<DateTime> EndDate { get; set; }

        
        //[RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public decimal LateFine { get; set; }

        [Required(ErrorMessage = "Late fine is required")]
        [RegularExpression("^[0-9 ,]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string LateFineForView { get; set; }
        public string ContractDocuments { get; set; }
        public HttpPostedFileBase ContractDocumentsFile { get; set; }

        [Required]
        public long SectVendorToLocation { get; set; }
       
        public long AllocateToLocation { get; set; }
        public string SecondaryCompany { get; set; }

        public string ReccurenceForPO { get; set; }

    }
    public class PaymentModeList
    {
        public long PaymentModeId { get; set; }
        public string  PaymentModeName { get; set; }
    }
    public class PaymentTermList
    {
        public long PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
    }
}
