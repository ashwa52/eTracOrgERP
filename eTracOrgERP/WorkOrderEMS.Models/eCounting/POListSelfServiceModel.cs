using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class POListSelfServiceModel
    {
        public string WaitingForName { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string LocationName { get; set; }
        public string PONumber { get; set; }
        public decimal? Amount { get; set; }
    }
}
