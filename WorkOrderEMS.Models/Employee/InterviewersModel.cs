using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class InterviewersViewModel
	{
		public List<Interviewers> Interviewers { get; set; }
		public string currentEmployeeId { get; set; }
		public bool CanInterviewStart { get; set; }
		public DateTime InterviewStartDateTime { get; set; }
	}
	public class Interviewers
	{
		public Nullable<long> INS_API_ApplicantId { get; set; }
		public string INS_EMP_InterviewerEmployeeId { get; set; }
		public string INS_IsHiringManager { get; set; }
		public string ProfileImage { get; set; }
		public string INS_IsAvailable { get; set; }
		public Nullable<System.DateTime> INS_AvailableTime { get; set; }
		public string INS_Comments { get; set; }
		public string InterviwerName { get; set; }

	}

}
