using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EmployeeHandbookModel
	{
		[Required]
		public string Signature { get; set; }
		[Required]
		public string EmployeeName { get; set; }
		[Required]
		public string Date { get; set; }
		public string IsActive { get; set; }
		public long? EhbId { get; set; }


	}
}
