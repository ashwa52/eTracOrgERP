using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
	public class OnboardingDetailRequestModel
	{
		public int App_Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string StreetAddress1 { get; set; }
		public string StreetAddress2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string MobileNumber { get; set; }
		public string EmailId { get; set; }
		public string EmpId { get; set; }
		public long CreatedBy { get; set; }
		public long? API_JobTitleID { get; set; }
		public Nullable<System.DateTime> API_DateOfJoining { get; set; }

	}

	public class GuestEmployeeBasicInfoRequestModel
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string EmployeeId { get; set; }
		public string NickName { get; set; }
		public string Gender { get; set; }
		public string Email { get; set; }
		public string EmergencyContact { get; set; }
		public string Relationship { get; set; }
		public string Address { get; set; }
		public string Phonenumber { get; set; }
		public string Birthday { get; set; }
		public string SSN { get; set; }
		public string LicenceStateId { get; set; }

	}
}
