using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.TCAPIP
{
    public class WitnessModel
    {
        
        public TerminationDenyModel TerminationDenyModel { get; set; }
        public string Tmn_Empid { get; set; }
        public string IsEliteEmployee { get; set; }
        public string WitnessName { get; set; }
        public long LocationId { get; set; }
        public string CompanyTheyWorkFor { get; set; }
        public string Cposition { get; set; }
        public string MyProperty { get; set; }
        public int LengthOfSeverence { get; set; }
        public string MeetingDateOne { get; set; }
        public string MeetingDateTwo { get; set; }
        public string MeetingDateThree { get; set; }
        public string MeetingTimeOne { get; set; }
        public string MeetingTimeTwo { get; set; }
        public string MeetingTimeThree { get; set; }
        public string MeetingDateNonExempt { get; set; }
        public string MeetingTimeNonExempt { get; set; }
    }

    public class TerminationDenyModel
    {
        
        public string ReasonForDenial { get; set; }
        public string AdditionalReasonComments { get; set; }
    }
    
}
