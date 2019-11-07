using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class CustomerSetupManagementModel
    {
        public string LocationIds { get; set; }
        public string Status { get; set; }
        public long? CustomerId { get; set; }
        public string id { get; set; }
        public long UserId { get; set; }
       
        [Required]
        [DisplayName("Address1")]
        [RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address1 { get; set; }

        [Required]
        [DisplayName("Address1 City")]
        [RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address1City { get; set; }

        [Required]
        [DisplayName("Address1 State")]
        public int? Address1State { get; set; }

        public string Address1Country { get; set; }

        //[Required]
        [DisplayName("Address2")]
        //[RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address2 { get; set; }

        //[Required]
        [DisplayName("Address2 City")]
        //[RegularExpression("^[a-zA-Z0-9, -@]+$", ErrorMessage = "Special characters are not allowed.")]
        public string Address2City { get; set; }

        //[Required]
        [DisplayName("Address2 State")]
        public int? Address2State { get; set; }
        public int? StateAfterIsSame { get; set; }
        public string Address2Country { get; set; }

        [Required]
        [DisplayName("Phone1")]
        [StringLength(12, ErrorMessage = "Invalid Phone1", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string Phone1 { get; set; }
        [StringLength(12, ErrorMessage = "Invalid Phone2", MinimumLength = 8)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string Phone2 { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailId { get; set; }

        //[Url(ErrorMessage = "Invalid Website URL")]
        //public string Website { get; set; }

        [Required]
        [DisplayName("Customer Type")]
        public string CustomerType { get; set; }

        public bool IsAddress2Same { get; set; }
        public string SelectedLcation { get; set; }
        public string LocationAllocateId { get; set; }

        public long? CompanyId { get; set; }
        public Result Result { get; set; }
        public long? CompanyType { get; set; }
        public long? COD_ID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string DLNo { get; set; }
        [Required]
        public int? MethodOfCommunication { get; set; }
        [Required]
        public int? ParkingFacilityLocation { get; set; }
        public decimal MonthlyPrice { get; set; }
        public bool IsAllowToSendText { get; set; }
        [Required]
        [DisplayName("ZipCode1")]
        [StringLength(8, ErrorMessage = "Invalid Zip", MinimumLength = 5)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string ZipCode1 { get; set; }
        
        [DisplayName("ZipCode2")]
        [StringLength(8, ErrorMessage = "Invalid Zip", MinimumLength = 5)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Special characters or letters are not allowed.")]
        public string ZipCode2 { get; set; }

        public bool IsMonthlyAppointmentSchedule { get; set; }
        public DateTime ScheduleAppointDate { get; set; }
        public string ScheduleAppointTime { get; set; }

        public CustomerVehicleInformationModel CustomerVehicleModel { get; set; }
        public List<CustomerVehicleDetails> CustomerVehicleDetails { get; set; }
        public CustomerPaymentInformationModel CustomerPaymentModel { get; set; }

        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string AccountStatus { get; set; }
        public string CustomerTypeText { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Minimum 6 and Maximum 10 characters required", MinimumLength = 6)]
        public string CustomerUserName { get; set; }
        [Required]
        [StringLength(8, ErrorMessage = "only 8 characters required", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string CustomerPassword { get; set; }
        [Required]
        public int SecurityQuestion { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string SecurityAnswer { get; set; }
        public long TCBD_ID { get; set; }

    }
   
    public class CustomerVehicleInformationModel
    {
        public long CustomerId { get; set; }
        public List<CustomerVehicleDetails> CustomerVehicleDetails { get; set; }
        //public bool IsMonthlyAppointmentSchedule { get; set; }
        //public Nullable<DateTime> ScheduleAppointDate { get; set; }
        //public string ScheduleAppointTime { get; set; }
    }

    public class CustomerVehicleDetails
    {
        public long ID { get; set; }
        public int SrNo { get; set; }
        public long CustomerId { get; set; }
        public string LicensePlateNo { get; set; }
        public int? State { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string StateText { get; set; }
    }

    public class CustomerPaymentInformationModel
    {
        public long ID { get; set; }
        public long CustomerId { get; set; }
        [Required]
        public string IsMonthlyParkingPaidFor { get; set; }
        public string CompanyName { get; set; }
        public bool IsSendDirectInvoiceToCompany { get; set; }
        public string CompanyEmail { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string BankAccountNo { get; set; }
        public string BankName { get; set; }
        public string BankIFSC { get; set; }
        public string BankRoutingNo { get; set; }
        public bool IsSignupForAutomaticPayment { get; set; }
        public string CardFirstName { get; set; }
        public string CardLastName { get; set; }
        public string Address { get; set; }
        public string CardType   { get; set; }
        public string CardNo { get; set; }
        public DateTime CardExpirationDate { get; set; }

    }

    public static class ExtensionMethods
    {
        public static string ReplaceSpecialCharacters(this string value)
        {
            return value.Replace("&", "&amp;").Replace("'", "&#39;").Replace("’", "&#39;").Replace("”", "").Replace("–", "-").Replace("�", " ").Replace(System.Environment.NewLine, " ").Trim();
        }

        public static DateTime ToExDateTime(this DateTime date)
        {
            System.Globalization.CultureInfo cif = new System.Globalization.CultureInfo("en-GB");
            DateTime dt;
            try
            {
                dt = (date <= DateTime.MinValue || date >= DateTime.MaxValue) ? Convert.ToDateTime("01/01/1900", cif) : Convert.ToDateTime(date, cif);
            }
            catch (Exception)
            {
                dt = Convert.ToDateTime("01/01/1900", cif);
            }
            return dt;
        }
    }
}
