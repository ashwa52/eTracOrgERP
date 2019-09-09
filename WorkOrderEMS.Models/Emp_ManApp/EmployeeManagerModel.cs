using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class EmployeeManagerModel
    {
        public string ServiceAuthKey { get; set; }
        public long UserId { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public long LocationId { get; set; }
    }
}
