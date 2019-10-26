using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    public class VendorAccountDetailsModel
    {
        public long PaymentMode { get; set; }
        public long AccountID { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        [RegularExpression("^[a-zA-Z .&',-]+$", ErrorMessage = "Special characters or number are not allowed.")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        [RegularExpression("^[a-zA-Z .&',-]+$", ErrorMessage = "Special characters or number are not allowed.")]
        public string BankNameForCard { get; set; }

        [Required(ErrorMessage = "Account Number is required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "IFSC Code is required")]
        [RegularExpression("^[a-zA-Z0-9 -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string IFSCCode { get; set; }

        [Required(ErrorMessage = "Swift OIC Code is required")]
        [RegularExpression("^[a-zA-Z0-9 -]+$", ErrorMessage = "Special characters are not allowed.")]
        public string SwiftOICCode { get; set; }
        public string AccountDocuments { get; set; }
        public HttpPostedFileBase AccountDocumentsFile { get; set; }

        [Required(ErrorMessage = "Card Number is required")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Card Holder Name is required")]
        [RegularExpression("^[a-zA-Z -]+$", ErrorMessage = "Special characters or number are not allowed.")]
        public string CardHolderName { get; set; }

        [Required(ErrorMessage = "Expiration Date is required")]
        public Nullable<DateTime> ExpirationDate { get; set; }

        [Required(ErrorMessage = "Bank Location is required")]
        [RegularExpression("^[a-zA-Z -]+$", ErrorMessage = "Special characters or number are not allowed.")]
        public string BankLocation { get; set; }
        public decimal BalanceAmount { get; set; }
        public long? QuickbookAcountId { get; set; }
        public string Id { get; set; }
        public string IsPrimary { get; set; }
    }

    public class VendorAccountDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<VendorAccountDetailsModel> rows { get; set; }
    }
}
