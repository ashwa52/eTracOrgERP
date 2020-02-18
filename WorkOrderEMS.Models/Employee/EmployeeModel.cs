using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EmployeeVIewModel
	{
        public long UserId { get; set; }
        public long ApplicantId { get; set; }
        public string Action { get; set; }
        public string EmpId { get; set; }
		public string Image { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string MiddleName { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string State { get; set; }
		[Required]
		public int? Zip { get; set; }
		[Required]
		public long? Phone { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Display(Name = "Date of Birth")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		[Required]
		public DateTime? Dob { get; set; }
		[Required]
		public string SocialSecurityNumber { get; set; }
		[Required]
		public string Cityzenship { get; set; }
		[Required]
		public string DlNumber { get; set; }
		[Required]
		public bool IsEditEnable { get; set; }
        public string LicenseNumber { get; set; }
        public string ActionValue { get; set; }
        public string Status { get; set; }

        //Added for Applicant
        
        public string StreetAddress { get; set; }
        public string APIUnit { get; set; }
        public string DesiredSalary { get; set; }
        public string PreferendName { get; set; }
        public Nullable<DateTime> YearsAtAddrss { get; set; }
        public string ContactType { get; set; }
        public string PreferendMethodOfContact { get; set; }
        public bool EligibleToWorkInUS { get; set; }
        public Nullable<DateTime> AvailableDate { get; set; }
        public bool Years24Age { get; set; }
        public bool  RelativeInElite { get; set; }
        public bool FromMilitary { get; set; }
        public string WhoInMilatary { get; set; }
        public string WhoInElite { get; set; }
        public bool HaveYouWorkELite { get; set; }
        public Nullable<DateTime> DateOFDeparture { get; set; }
        public string ReasonForlLeave { get; set; }

    }

}
