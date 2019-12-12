using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Models
{
    public class CommonFormModel
    {
        public string FormName { get; set; }
        public long UserId { get; set; }
        public long EmployeeId { get; set; }
        public string ServiceAuthKey { get; set; }
        public EducationFormModel EducationFormModel { get; set; }
        public DirectDepositFormModel DirectDepositFormModel { get; set; }
        public EmergencyContactFormModel EmergencyContactFormModel { get; set; }
        public W4FormModel W4FormModel { get; set; }
    }
}
