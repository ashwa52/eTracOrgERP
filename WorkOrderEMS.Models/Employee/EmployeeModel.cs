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
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int? Zip { get; set; }
		public long? Phone { get; set; }
		public string Email { get; set; }
		[Display(Name = "Date of Birth")]
		[DataType(DataType.Date)]
		public DateTime? Dob { get; set; }
		public string SocialSecurityNumber { get; set; }
		public string Cityzenship { get; set; }
		public string DlNumber { get; set; }
		public bool IsEditEnable { get; set; }
	}

}
