using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AccessPermisionTreeViewModel
    {       
        public long? id { get; set; }
        public long? ModuleId { get; set; }
        public long? SubModuleId { get; set; }
        public string name { get; set; }  //text
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public long VST_Id { get; set; }
        public long VSM_VST_Id { get; set; }
        public long UserTypeId { get; set; }
        public string CodeName { get; set; }
        public string VSTTitle { get; set; }
        public bool @checked { get; set; }
        public List<AccessPermisionTreeViewModel> item { get; set; }  //children       
    }
}
