using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Models
{
    public class CommonApplicantModel
    {
        public long ApplicantId { get; set; }
        public List<ApplicantPersonalInfo> ApplicantPersonalInfo { get; set; }
        public List<ApplicantAddress> ApplicantAddress { get; set; }
        public List<ApplicantContactInfo> ApplicantContactInfo { get; set; }
        public List<ApplicantAdditionalInfo> ApplicantAdditionalInfo { get; set; }
        public List<AplicantAcadmicDetails> AplicantAcadmicDetails { get; set; }
        public List<ApplicantBackgroundHistory> ApplicantBackgroundHistory { get; set; }
        public List<ApplicantPositionTitle> ApplicantPositionTitle { get; set; }
        public List<ApplicantAccidentRecord> ApplicantAccidentRecord { get; set; }
        public List<ApplicantTrafficConvictions> ApplicantTrafficConvictions { get; set; }
        public List<ApplicantVehiclesOperated> ApplicantVehiclesOperated { get; set; }
        public List<ApplicantLicenseHeald> ApplicantLicenseHeald { get; set; }
        public List<ApplicantSchecduleAvaliblity> ApplicantSchecduleAvaliblity { get; set; }
    }
    public class ApplicantPersonalInfo
    {
        public char API_Action { get; set; }
        public long API_Id { get; set; }        
        public long ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Resume { get; set; }
        public string SSN { get; set; }
        public string DL_Number { get; set; }      
        public decimal DesireSalary { get; set; }
        public char IsActive { get; set; }
    }
}
