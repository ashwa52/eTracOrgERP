using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AccessPermissionModel
    {
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public long ModuleId { get; set; }
        public long SubModuleId { get; set; }
        public long UserTYpeId { get; set; }
        public string UserTypeName { get; set; }
        public bool Checked { get; set; }
        public List<ChildrenModel> Children { get; set; }
    }
    public class ChildrenModel
    {
        public long Id { get; set; }
        public string SubModuleName { get; set; }
        public bool Checked { get; set; }
        public List<ChildrenModel> Children { get; set; }
    }
}
