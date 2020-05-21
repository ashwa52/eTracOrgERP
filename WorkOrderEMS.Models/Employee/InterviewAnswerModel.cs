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
        //public List<AnswerArr> AnswerArr { get; set; }

    }
    public class AnswerArr
    {
        public int questionId { get; set; }
        public string Answer { get; set; }
        public int masterId { get; set; }
        public string Comment { get; set; }
        public string Comment31 { get; set; }
        public string Comment32 { get; set; }
        //public string MyProperty { get; set; }
    }
}
