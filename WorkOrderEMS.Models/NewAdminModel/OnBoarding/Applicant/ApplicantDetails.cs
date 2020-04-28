using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ApplicantDetails
    {
        public string ApplicantName { get; set; }
        public string ApplicantStatus { get; set; }
        public string ApplicantComment { get; set; }
        public string LocationName { get; set; }
        public string JobTitle { get; set; }
        public long JobId { get; set; }
        public long? ApplicantId { get; set; }
        public string HiringManagerId { get; set; }
        public long? IPT_Id { get; set; }
        //public string ApplicantName { get; set; }
    }
}
