using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class MiscellaneousServiceModel
    {
        public int MISCID { get; set; }
        public string ServiceAuthKey { get; set; }
        public long LocationId { get; set; }
    }
    public class MiscellaneousList
    {
        public long LocationId { get; set; }
        public string MISId { get; set; }
        public string UserName { get; set; }
        public long MISNumber { get; set; }
        public long UserId { get; set; }
        public long MId { get; set; }
        public List<MiscellaneousListModel> ChildMiscellaneousList { get; set; }
    }
}
