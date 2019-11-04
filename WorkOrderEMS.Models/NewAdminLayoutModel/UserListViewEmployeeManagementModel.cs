using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class UserListViewEmployeeManagementModel
    {
        public string EmployeeId { get; set; }
        public string ProfilePhoto { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string JobTitle { get; set; }
        public string JobDesc { get; set; }
        public string LocationName { get; set; }
        public long? JobTitleId { get; set; }
        public long? LocationId { get; set; }
        public long? VSTId { get; set; }
        public string UserId { get; set; }
        public List<string> JobDescList { get; set; }
    }
}
