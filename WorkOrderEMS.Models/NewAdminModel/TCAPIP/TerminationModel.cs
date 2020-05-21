using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.TCAPIP
{
    public class TerminationModel
    {
        
        public string EmpId { get; set; }
        public string ManagerId { get; set; }
        public string Emp_Name { get; set; }
        public string Manager_Name { get; set; }
        public Nullable<DateTime> Last_Day_Worked { get; set; }
        public Nullable<DateTime> Termination_Date { get; set; }
        public string Reason_For_Leaving { get; set; }
        public string detailed_Expalnation { get; set; }
        public string Final_Incident_Termination { get; set; }
        public string Items_Owned_ByEmployee { get; set; }
        public string ItemsList_Cost { get; set; }
        public string Re_Hire { get; set; }
        public string HR_Decision { get; set; }
        public string IsSeverence { get; set; }
        public int LengthOfSeverence { get; set; }
        public string SeverenceApproval { get; set; }
        public int WitnessedId { get; set; }
        public string HrDenyReason { get; set; }
        public string HrDenyComment { get; set; }
        public string IsExempt { get; set; }
        


    }
    
}
