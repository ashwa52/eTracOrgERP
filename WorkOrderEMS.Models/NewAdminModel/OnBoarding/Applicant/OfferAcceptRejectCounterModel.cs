using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class OfferAcceptRejectCounterModel
    {
        public string IsActive { get; set; }
        public string Action { get; set; }
        public long ApplicantId { get; set; }
    }
}
