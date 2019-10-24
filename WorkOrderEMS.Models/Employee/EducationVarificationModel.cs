using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EducationVarificationModel
	{
		public string Name { get; set; }
		public string DOB { get; set; }
		public string SSN { get; set; }
		public HigherSchool HighSchool { get; set; }
		public HigherSchool HigherSchool { get; set; }
		public string Certificate { get; set; }
		public string Signature { get; set; }
		public string Date { get; set; }
		public bool	IsSave { get; set; }
		public string EmpId { get; set; }

	}
	
	public class HighSchool
	{
		public string SchoolName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public DateTime? AttendFrom { get; set; }
		public DateTime? AttendTo { get; set; }
	}
	public class HigherSchool
	{
		public string SchoolName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public DateTime? AttendFrom { get; set; }
		public DateTime? AttendTo { get; set; }
	}
}
