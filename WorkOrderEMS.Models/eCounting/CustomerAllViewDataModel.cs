using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class CustomerAllViewDataModel
    {
        public long CustomerId { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string Address1 { get; set; }
        public string Address1City { get; set; }
        public int? Address1State { get; set; }
        public string Address1Country { get; set; }
        public string Address2 { get; set; }
        public string Address2City { get; set; }
        public int? Address2State { get; set; }
        public int? StateAfterIsSame { get; set; }
        public string IsAddress2Same { get; set; }
        public string Phone1 { get; set; }
        public string DLNo { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int? MethodOfCommunication { get; set; }
        public int? ParkingFacilityLocation { get; set; }
        public decimal MonthlyPrice { get; set; }
        public string IsAllowToSendText { get; set; }
        public string IsMonthlyAppointmentSchedule { get; set; }
        public string ScheduleAppointDate { get; set; }
        public string ScheduleAppointTime { get; set; }

        //Payment Account Model
        public string IsMonthlyParkingPaidFor { get; set; }
        public string CompanyName { get; set; }
        public string IsSendDirectInvoiceToCompany { get; set; }
        public string CompanyEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCcode { get; set; }
        public string BankRoutingNo { get; set; }
        public string CardHolderName { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CardExpirationDate { get; set; }
        public string SwiftBICcode { get; set; }
        public string IsSignupForAutomaticPayment { get; set; }

        public string CustomerTypeText { get; set; }
        public string Address1StateText { get; set; }
        public string Address2StateText { get; set; }
        public string Status { get; set; }
        //List of Vehicle Details
        public List<CustomerVehicleDetails> CustomerVehicleDetails { get; set; }
        //public List<LocationDataModel> LocationAssignedModel { get; set; }
    }
    public class ApproveRejectCustomerModel
    {
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; }
        public string CustomerId { get; set; }
        public long Customer { get; set; }
        public long UserId { get; set; }
        public long LocationId { get; set; }
        public string LLCM_Id { get; set; }
    }


}


