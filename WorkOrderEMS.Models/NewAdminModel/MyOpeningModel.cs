using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
	public class MyOpeningModel
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public long PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Status { get; set; }
		public string Image { get; set; }
		public string JobTitle { get; set; }
		public long ApplicantId { get; set; }
		public decimal? DesireSalary { get; set; }
        public long? JobTitleId { get; set; }
    }
	public class JobPosting
	{
		public long JobPostingId { get; set; }
		public string JobTitle { get; set; }
		public int? Applicant { get; set; }
		public int? Employee { get; set; }
		public string DatePosted { get; set; }
		public int? Duration { get; set; }
		public string Status { get; set; }
	}
	public class InfoFactSheet
	{
		public string ResumePath { get; set; }
		public MyOpeningModel model  { get; set; }

	}
}
