using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class SelfIdentificationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MidleName { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public string Race_Ethnicity { get; set; }
        public string VeteranStatus { get; set; }
        public Nullable<DateTime> DateOfDischarge { get; set; }
        public string Disability { get; set; }
        public long? Self_Id { get; set; }
        public string EmployeeId { get; set; }
        public string Description { get; set; }
    }
    public class ApplicantFunFactModel
    {
        public string Answer_Que1 { get; set; }
        public string Answer_Que2 { get; set; }
        public string Answer_Que3 { get; set; }
        public string Answer_Que4 { get; set; }
        public long? Fun_Fact_Id { get; set; }
        public string Employee_Id { get; set; }
        public long Applicant_Id { get; set; }
    }
}
