using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class CharitableContributionForm
    {
        public string NonProfitOrganization { get; set; }
        public string MailingAddress { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string NameOfEvent { get; set; }
        public string DateOfEvent { get; set; }
        public string FederalTaxID { get; set; }
        public string ContributionRequested { get; set; }
        public string EstimatedNumberOfAttendees { get; set; }
        public string DescribeTheDemographic{ get; set; }
        public string DescribeTheDemographic1 { get; set; }
        public string publicityIsPlanned { get; set; }
        public string publicityIsPlanned1 { get; set; }
        public string opportunitiesDescription { get; set; }
        public string opportunitiesDescription1{ get; set; }
        public string FileName { get; set; }
    }
}
