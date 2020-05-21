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
        public long ApplicantId { get; set; }
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
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public string w4F_4 { get; set; }
        public Nullable<int> w4F_5 { get; set; }
        public Nullable<decimal> w4F_6 { get; set; }
        public Nullable<decimal> w4F_7 { get; set; }
        public string w4F_8EmployersName { get; set; }
        public Nullable<System.DateTime> w4F_9 { get; set; }
        public string w4F_10 { get; set; }
        public string EmployeeMaritalStatus { get; set; }
        public bool IsSignature { get; set; }
        public string EmployeeSignatureName { get; set; }
        public string EmployeeSignaature { get; set; }
    }
	public class MeritalStatus
	{
		public bool Single { get; set; }
		public bool Married { get; set; }
		public bool PartiallyMarried { get; set; }
	}

}
