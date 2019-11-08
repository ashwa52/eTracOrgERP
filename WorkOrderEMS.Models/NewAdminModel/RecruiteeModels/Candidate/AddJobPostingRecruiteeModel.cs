using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.RecruiteeModels.Candidate.Add
{
    public class Offer
    {
        public string department { get; set; }
        public string description { get; set; }
        public string kind { get; set; }
        public string title { get; set; }
        public int position { get; set; }
        public string status { get; set; }
        public string postal_code { get; set; }
        public string requirements { get; set; }
        public bool remote { get; set; }
        public string city { get; set; }
        public string country_code { get; set; }
    }
    public class Root
    {
        public Offer offer { get; set; }
    }
}
