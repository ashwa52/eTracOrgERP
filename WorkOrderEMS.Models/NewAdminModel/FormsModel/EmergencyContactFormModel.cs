using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class EmergencyContactFormModel
    {
        public long ECF_Id { get; set; }
        public string ECF_EMP_EmployeeId { get; set; }
        public string ECF_NickName { get; set; }
        public long? ECF_HomePhone { get; set; }
        public string ECF_HomeEmail { get; set; }
    }
}
