using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
		public long? EvfId { get; set; }
		public string IsActive { get; set; }

	}
	
	public class HighSchool
	{
		public string SchoolName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? AttendFrom { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? AttendTo { get; set; }
	}
	public class HigherSchool
	{
		public string SchoolName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? AttendFrom { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
		[DataType(DataType.Date)]
		public DateTime? AttendTo { get; set; }
		public string Cretificate { get; set; }
	}
}
