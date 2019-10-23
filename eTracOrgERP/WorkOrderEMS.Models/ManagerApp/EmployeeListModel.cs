using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class EmployeeListModel
    {
        public long? LocationId { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public string EmployeeId { get; set; }
    }
}
