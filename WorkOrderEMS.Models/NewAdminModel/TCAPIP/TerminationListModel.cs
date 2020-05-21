using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.TCAPIP
{
    public class TerminationListModel
    {
        public long manager_id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string seatTitle  { get; set; }
        public string emp_id { get; set; }
        public string Etype { get; set; }
        public string emp_photo { get; set; }
        public Nullable<DateTime> DateIssued { get; set; }
        public string  SeveranceApproval { get; set; }

    }
}
