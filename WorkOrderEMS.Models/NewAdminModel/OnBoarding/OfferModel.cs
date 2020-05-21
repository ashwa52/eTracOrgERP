using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class OfferModel
    {
        public string HMPosition { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public long ApplicantId { get; set; }
        public string EmployeeId { get; set; }
        public string HMName { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
        public string EmployeeName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> CurrentDate { get; set; }
        public string PersonalEmail { get; set; }
        public long? PhoneNumber { get; set; }
        public string LocationName { get; set; }
        public string Title { get; set; }
        public decimal? DesireSalaryWages { get; set; }
        public decimal? OfferAmount { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public string IsExempt { get; set; }
        public decimal? VST_RateOfPay { get; set; }
    }
    
}
