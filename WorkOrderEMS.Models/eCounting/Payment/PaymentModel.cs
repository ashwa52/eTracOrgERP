using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PaymentModel
    {
        public long LLBL_ID { get; set; }
        public long? BillNo { get; set; }
        public string VendorName { get; set; }
        public string BillType { get; set; }
        public string LocationName { get; set; }
        public decimal? BillAmount { get; set; }
        public Nullable<DateTime> BillDate { get; set; }
        public int? GracePeriod { get; set; }
        [Required(ErrorMessage = "Please select Payment Mode")]
        public string PaymentMode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Please check Card Number")]
        public string CARDNo { get; set; }

        [Required(ErrorMessage = "Please check Account Number")]
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "Please add cheque Number")]
        public long? ChequeNo { get; set; }
        public string Payment { get; set; }

        [Required(ErrorMessage = "Please add Account Number")]
        public string AccNo { get; set; }

        public string PaymentByCash { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        public List<Card> CardNumber { get; set; }
        public List<AccountNo> AccountNumber { get; set; }
        public long? VendorId { get; set; }
        public long CompanyAccountId { get; set; }
        public string OperatingCompany { get; set; }
        public long? OperatingCompanyId { get; set; }
        public long? LocationId { get; set; }
        public long? UserId { get; set; }
        public bool IsCancel { get; set; }
        public long OpeartorCAD_Id { get; set; }
        public string DisplayDate { get; set; }
        public string PaymentNote { get; set; }
        public string ActionDoneOn { get; set; }
        public string ActionDoneBy { get; set; }
    }
    public class Card
    {
        public string CardNo { get; set; }
    }
    public class AccountNo
    {
        public string AccountNumber { get; set; }
    }
    public class PaymentModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<PaymentModel> rows { get; set; }
    }
}
