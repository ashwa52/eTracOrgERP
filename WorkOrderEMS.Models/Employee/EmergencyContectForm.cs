using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EmergencyContectForm
	{
		public long? EcfId { get; set; }
		public string NickName { get; set; }
		public long? HomePhone { get; set; }
		public string HomeEmail { get; set; }
		public DateTime? EcfDate { get; set; }
		public string IsActive { get; set; }
		public string EmpId { get; set; }
		public bool	IsSave { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Gender { get; set; }
		public string Citizenship { get; set; }
		public string HomeAddress { get; set; }
		public long Mobile { get; set; }
		public string DOB { get; set; }
		public string SSN { get; set; }
		public string License { get; set; }
		public string EmergencyContactName { get; set; }
		public string RelationShip { get; set; }
		public string Address { get; set; }
		public long ConactInfo { get; set; }
	}

}
