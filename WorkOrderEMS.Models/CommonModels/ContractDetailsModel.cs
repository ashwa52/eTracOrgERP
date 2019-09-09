using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ContractDetailsModel
    {
        [Required(ErrorMessage = "Please select Contract Holder")]
        public long? ContractHolder { get; set; }

        [Required(ErrorMessage = "Please select Operating Company")]
        public long? OperatingHolder { get; set; }

        [Required(ErrorMessage = "Contract Start Date is Required")]
        public DateTime? ContractStartDate { get; set; }

        [Required(ErrorMessage = "Contract End Date is Required")]
        public DateTime? ContractEndDate { get; set; }

        [Required(ErrorMessage = "Please select Contract Type")]
        public long? ContractType { get; set; }

        [Required(ErrorMessage = "Please select Client Invoicing Term")]
        public string ClientInvoicingTerm { get; set; }

        [Required(ErrorMessage = "Reporting Type is Required")]
        public DateTime? ReportingDate { get; set; }

        [Required(ErrorMessage = "Inter Company Management Fee is Required")]
        public decimal? IntercompanyMgmFee { get; set; }

        [Required(ErrorMessage = "Inter Company Invoicing is Required")]
        public DateTime? IntercompanyInvoicing { get; set; }

        [Required(ErrorMessage = "Contract Term is Required")]
        public string ContractTerms { get; set; }

        [Required(ErrorMessage = "Please select Additional Years")]
        public bool? AdditonalYears { get; set; }

        public string ReportingType { get; set; }
        public string Years { get; set; }
        public long OperatingHolderSameId { get; set; }
    }
}
