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
	}

}
