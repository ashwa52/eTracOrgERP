using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class InterviewAnswerModel
	{
		public long? QusId { get; set; }
		public long? ApplicantId { get; set; }
		public string Answer { get; set; }
		public string Comment { get; set; }

	}
}
