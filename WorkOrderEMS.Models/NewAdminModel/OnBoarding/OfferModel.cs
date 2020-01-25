using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class OfferModel
    {
        public string Position { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public long ApplicantId { get; set; }
        public string EmployeeId { get; set; }
        public string HRName { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
    }
    
}
