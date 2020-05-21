using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.TCAPIP
{
    public class CorrectiveActionModel
    {
        public string emp_name { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string Job_Title { get; set; }
        public string emp_id { get; set; }
        public string manager_name { get; set; }
        public string manager_id { get; set; }
        public string Level_ofCorrectiveAction { get; set; }
        public Nullable<DateTime> FDate { get; set; }
        public Nullable<TimeSpan> FTime { get; set; }
        public string FType { get; set; }
        public string Employee_Explanation { get; set; }
        public string Policy_Violation { get; set; }
        public string Expectation_CorrectiveActionPlan { get; set; }
        public string ActionTaken { get; set; }
        public string Next_Action { get; set; }
        public string HR_Approval { get; set; }
        public string Is_Guilty { get; set; }
        public Nullable<DateTime> CTA_Date  { get; set; }
        public string IsExempt { get; set; }
        public string Is_Active { get; set; }
        public string HrDenyReason { get; set; }
        public string HrDenyComment { get; set; }
        public string HRflag { get; set; }
        public string EmpComment { get; set; }
        public string MeetingDateTime { get; set; }
    }
}
