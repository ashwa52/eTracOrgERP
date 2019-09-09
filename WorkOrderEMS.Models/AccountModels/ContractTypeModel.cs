using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.AccountModels
{
    public class ContractTypeModel
    {
        public long CTT_Id { get; set; }
        [DisplayName("Contract Type")]
        public string CTT_ContractType { get; set; }

        [DisplayName("Description")]
        public string CTT_Discription { get; set; }
        public string CTT_IsActive { get; set; }
        public Result Result { get; set; }
    }
    public class ContractTypeDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<ContractTypeModel> rows { get; set; }
    }
}
