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
        //Done till here//
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
        public long API_APT_ApplicantId { get; set; }
        public string API_FirstName { get; set; }
        public string API_MiddleName { get; set; }
        public string API_LastName { get; set; }
        public string API_Resume { get; set; }
        public string API_SSN { get; set; }
        public string API_DLNumber { get; set; }      
        public decimal API_DesireSalaryWages { get; set; }
        public char API_IsActive { get; set; }
    }
}
