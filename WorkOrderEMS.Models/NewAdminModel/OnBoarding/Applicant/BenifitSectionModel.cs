using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BenifitSectionModel
    {
        public string ApplicantId { get; set; }
        public List<BenifitList> BenifitList { get; set; }
    }
    public class BenifitList
    {
        public string Code { get; set; }
        public string CodeName { get; set; }
    }
}
