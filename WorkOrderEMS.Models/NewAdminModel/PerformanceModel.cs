using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class PerformanceModel
    {
        public string EMP_EmployeeID { get; set; }
        public string EMP_Photo { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string JBT_JobTitle { get; set; }
        public string LocationName { get; set; }
        public DateTime? EMP_DateOfJoining { get; set; }
        public int? Assesment { get; set; }
    }
    public class GWCQUestionModel
    {
        public long QuestionId { get; set; }
        public string AssessmentType{ get; set; }
        public string QuestionType{ get; set; }
        public string Question{ get; set; }

        public string Answer { get; set; }

        public string EmployeeId { get; set;}
        public long SelfAssessmentId { get; set;}

        public string SAM_IsActive { get; set; }

        public string EmployeeName { get; set; }
        public string  Image { get; set; }
        public string JobTitle { get; set; }

        public string Comment { get; set; }

        public Nullable<long> SAR_Id { get; set; }
        public string SAR_EMP_EmployeeId { get; set; }
        public string SAR_EMP_EmployeeIdManager { get; set; }
        public string SAR_QuestionType { get; set; }
        public long ASQ_Id { get; set; }
        public string ASQ_Question { get; set; }
        public string SAR_AnswerSelf { get; set; }
        public string SAR_AnswerManager { get; set; }
        public string SAR_Comments { get; set; }
        public string SAR_IsActive { get; set; }



    }

}
