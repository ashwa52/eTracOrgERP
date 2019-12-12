using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class EducationFormModel
    {
        public long EVF_Id { get; set; }
        public string EVF_Employee_Id { get; set; }
        public string EVF_SchoolDegreeDiplomaCert { get; set; }
        public string EVF_OrganizationName { get; set; }
        public string EVF_Address { get; set; }
        public string EVF_City { get; set; }
        public string EVF_State { get; set; }
        public string EVF_AttendedFrom { get; set; }
        public string EVF_AttendedTo{ get; set; }
        public string EVF_EmployeeName{ get; set; }
    }
}
