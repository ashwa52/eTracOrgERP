using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class W4FormModel
	{
		public long? W4FId { get; set; }
		[Required(ErrorMessage ="*")]
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		[Required(ErrorMessage = "*")]
		public string LastName { get; set; }
		public string EmpId { get; set; }
		[Required(ErrorMessage = "*")]
		public string SSN { get; set; }
		public MeritalStatus MeritalStatus { get; set; }

		public bool NameDiffer { get; set; }
		public int? TotalAllowence { get; set; }
		public decimal? AdditionalAmount { get; set; }
		public decimal? ClaimExemption { get; set; }
		public string EmployeerNameAndAddress { get; set; }
		public DateTime? FirstEmployeementDate { get; set; }
		public string EIN { get; set; }
		public string IsActive { get; set; }
		public bool IsSave { get; set; }
	}
	public class MeritalStatus
	{
		public bool Single { get; set; }
		public bool Married { get; set; }
		public bool PartiallyMarried { get; set; }
	}

}
