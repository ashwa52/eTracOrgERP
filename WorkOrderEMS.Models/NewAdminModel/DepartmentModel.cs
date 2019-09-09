using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class DepartmentModel
    {
        [DisplayName("Department Name")]
        public string DepartmentName { get; set; }
        public string IsActive { get; set; }
        public bool IsActive_Grid { get; set; }
        public long? DeptId { get; set; }
        public string Action { get; set; }
    }
    public class DepartmentDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<DepartmentModel> rows { get; set; }
    }
}
