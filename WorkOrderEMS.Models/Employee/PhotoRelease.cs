using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class PhotoRelease
	{
		public string	EmpId { get; set; }
		public string IsActive { get; set; }
		public string Name { get; set; }
		public long? PRFId { get; set; }
		public bool IsSave { get; set; }
	}
}
