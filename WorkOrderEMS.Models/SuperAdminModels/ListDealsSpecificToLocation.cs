using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ListDealsSpecificToLocation
    {
        public int Id { get; set; }
        public long? LocationID { get; set; }
        public string DealName { get; set; }
        public string Condition { get; set; }
        public string NewCost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string StartDateDisplay { get; set; }
        public string EndDateDisplay { get; set; }

    }   
}
