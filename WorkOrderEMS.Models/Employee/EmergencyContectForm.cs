using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EmergencyContectForm
	{
		public long? EcfId { get; set; }
		[Required(ErrorMessage ="*")]
		public string NickName { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public long? HomePhone { get; set; }
		[Required(ErrorMessage = "*")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string HomeEmail { get; set; }
		public DateTime? EcfDate { get; set; }
		public string IsActive { get; set; }
		public string EmpId { get; set; }
		public bool	IsSave { get; set; }
		[Required(ErrorMessage = "*")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "*")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "*")]
		public string MiddleName { get; set; }
		[Required(ErrorMessage = "*")]
		public string Gender { get; set; }
		[Required(ErrorMessage = "*")]
		public string Citizenship { get; set; }
		[Required(ErrorMessage = "*")]
		public string HomeAddress { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.PhoneNumber)]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
		public long Mobile { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public string DOB { get; set; }
		[Required(ErrorMessage = "*")]
		public string SSN { get; set; }
		[Required(ErrorMessage = "*")]
		public string License { get; set; }
		[Required(ErrorMessage = "*")]
		public string EmergencyContactName { get; set; }
		[Required(ErrorMessage = "*")]
		public string RelationShip { get; set; }
		[Required(ErrorMessage = "*")]
		public string Address { get; set; }
		[Required(ErrorMessage = "*")]
		public long ConactInfo { get; set; }
	}

}
