using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.AccountModels
{
    public class POTypeModel
    {
        public long POT_Id { get; set; }

        [DisplayName("PO Name")]
        public string POT_POName { get; set; }
        public string POT_IsActive { get; set; }
        public Result Result { get; set; }
    }
    public class POTypeModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<POTypeModel> rows { get; set; }
    }
}
