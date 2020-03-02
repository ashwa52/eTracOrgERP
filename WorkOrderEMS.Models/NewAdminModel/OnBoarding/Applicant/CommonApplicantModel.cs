using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Desclaimer Desclaimer { get; set; }

        public CommonApplicantModel() {
            ApplicantId = new long();
            ApplicantPersonalInfo = new List<ApplicantPersonalInfo>();
            ApplicantPersonalInfo a = new ApplicantPersonalInfo();
            ApplicantPersonalInfo.Add(a);
            ApplicantAddress = new List<ApplicantAddress>();
            ApplicantAddress b = new ApplicantAddress();
            ApplicantAddress.Add(b);
            ApplicantContactInfo = new List<ApplicantContactInfo>();
            ApplicantContactInfo c = new ApplicantContactInfo();
            ApplicantContactInfo.Add(c);
            ApplicantAdditionalInfo = new List<ApplicantAdditionalInfo>();
            ApplicantAdditionalInfo d = new ApplicantAdditionalInfo();
            ApplicantAdditionalInfo.Add(d);
            AplicantAcadmicDetails = new List<AplicantAcadmicDetails>();
            AplicantAcadmicDetails e = new AplicantAcadmicDetails();
            AplicantAcadmicDetails.Add(e);
            ApplicantBackgroundHistory = new List<ApplicantBackgroundHistory>();
            ApplicantBackgroundHistory f = new ApplicantBackgroundHistory();
            ApplicantBackgroundHistory.Add(f);
            ApplicantPositionTitle = new List<ApplicantPositionTitle>();
            ApplicantPositionTitle g = new ApplicantPositionTitle();
            ApplicantPositionTitle.Add(g);
            ApplicantAccidentRecord = new List<ApplicantAccidentRecord>();
            ApplicantAccidentRecord h = new ApplicantAccidentRecord();
            ApplicantAccidentRecord.Add(h);
            ApplicantTrafficConvictions = new List<ApplicantTrafficConvictions>();
            ApplicantTrafficConvictions i = new ApplicantTrafficConvictions();
            ApplicantTrafficConvictions i1 = new ApplicantTrafficConvictions();
            ApplicantTrafficConvictions i2 = new ApplicantTrafficConvictions();
            ApplicantTrafficConvictions.Add(i);
            ApplicantTrafficConvictions.Add(i1);
            ApplicantTrafficConvictions.Add(i2);
            ApplicantVehiclesOperated = new List<ApplicantVehiclesOperated>();
            ApplicantVehiclesOperated j = new ApplicantVehiclesOperated();
            ApplicantVehiclesOperated.Add(j);
            ApplicantLicenseHeald = new List<ApplicantLicenseHeald>();
            ApplicantLicenseHeald k = new ApplicantLicenseHeald();
            ApplicantLicenseHeald k1 = new ApplicantLicenseHeald();
            ApplicantLicenseHeald k2 = new ApplicantLicenseHeald();
            ApplicantLicenseHeald.Add(k);
            ApplicantLicenseHeald.Add(k1);
            ApplicantLicenseHeald.Add(k2);
            ApplicantSchecduleAvaliblity = new List<ApplicantSchecduleAvaliblity>();
        }
    }
    public class ApplicantPersonalInfo
    {
        public char API_Action { get; set; }
        public long API_Id { get; set; }        
        public long? API_APT_ApplicantId { get; set; }
        [Required]
        public string API_FirstName { get; set; }
        [Required]
        public string API_MiddleName { get; set; }
        [Required]
        public string API_LastName { get; set; }
        public string API_Resume { get; set; }
        [Required]
        public string API_SSN { get; set; }
        [Required]
        public string API_DLNumber { get; set; }    
        [Required]
        public decimal? API_DesireSalaryWages { get; set; }
        public string API_IsActive { get; set; }
        public string API_Title { get; set; }
    }

    public class Desclaimer
    {
        public long? ApplicantId { get; set; }
        public string Signature { get; set; }
        public string EmployeeId { get; set; }
        public string IsActive { get; set; }
        public Nullable<DateTime> ASG_Date { get; set; }
        public long? Sing_Id { get; set; }
    }
    public class BackgroundCheckForm
    {
        public long UserId { get; set; }
        public bool IsSignature { get; set; }
        public ApplicantPersonalInfo ApplicantPersonalInfo { get; set; }
        public List<ApplicantAddress> ApplicantAddress { get; set; }
    }
}
