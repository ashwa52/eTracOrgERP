using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.GlobalSettings
{
    public class ARRules
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string Condition { get; set; }
        public string Settings { get; set; }
        public bool Status { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
