using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ApplicantAddress
    {
        public char APA_Action { get; set; }
        public long APA_Id { get; set; }
        public long? APA_APT_ApplicantId { get; set; }
        [DisplayName("Street Address")]
        public string APA_StreetAddress { get; set; }
        [DisplayName("Apartment")]
        public string APA_Apartment { get; set; }
        [DisplayName("City")]
        public string APA_City { get; set; }
        [DisplayName("State")]
        public string APA_State { get; set; }
        [DisplayName("Zip")]
        public int? APA_Zip { get; set; }
        [DisplayName("Years Address From")]
        public Nullable<DateTime> APA_YearsAddressFrom { get; set; }
        [DisplayName("Years Address To")]
        public Nullable<DateTime> APA_YearsAddressTo { get; set; }
        public string APA_IsActive { get; set; }

    }
    public class ApplicantContactInfo
    {
        public char ACI_Action { get; set; }
        public long ACI_Id { get; set; }
        public long ACI_APT_ApplicantId { get; set; }
        [DisplayName("Phone No")]
        public long ACI_PhoneNo { get; set; }
        [DisplayName("eMail")]
        public string ACI_eMail { get; set; }
        [DisplayName("Prefred Contact Method")]
        public string ACI_PrefredContactMethod { get; set; }
        //public Nullable<DateTime> ACI_Date { get; set; }
        public string ACI_IsActive { get; set; }

    }
    public class ApplicantAdditionalInfo
    {
        public char AAI_Action { get; set; }
        public long AAI_Id { get; set; }
        public long AAI_ApplicantId { get; set; }
        [DisplayName("Work Eligibility US")]
        public char AAI_WorkEligibilityUS { get; set; }
        [DisplayName("Available Date")]
        public Nullable<DateTime> AAI_AvailableDate { get; set; }
        [DisplayName("Age 21 Completed")]
        public char AAI_Age21Completed { get; set; }
        [DisplayName("Any Ref Or Employee In ELITE")]
        public char AAI_AnyRefOrEmployeeInELITE { get; set; }
        [DisplayName("Name Of Ref Or Employee In ELITE")]
        public string AAI_NameOfRefOrEmployeeInELITE { get; set; }
        [DisplayName("Prior Military Service")]
        public char AAI_PriorMilitaryService { get; set; }
        [DisplayName("Ever Work For ELITE")]
        public char AAI_EverWorkForELITE { get; set; }
        [DisplayName("Departure Date")]
        public Nullable<DateTime> AAI_DepartureDate { get; set; }
        [DisplayName("Reason For Leaving")]
        public string AAI_ReasonForLeaving { get; set; }
        //public Nullable<DateTime> AAI_Date { get; set; }
        public string AAI_IsActive { get; set; }

    }
    public class AplicantAcadmicDetails
    {
        public char AAD_Action { get; set; }
        public long AAD_Id { get; set; }
        public long AAD_APT_ApplicantId { get; set; }
        public string AAD_EducationType { get; set; }
        public string AAD_InstituteName { get; set; }
        public Nullable<DateTime> AAD_AttendedFrom { get; set; }
        public Nullable<DateTime> AAD_AttendedTo { get; set; }
        public string AAD_Address { get; set; }
        public string AAD_City { get; set; }
        public string AAD_State { get; set; }
        public int AAD_Zip { get; set; }
        public char AAD_IsActive { get; set; }

    }
    public class ApplicantBackgroundHistory
    {
        public char ABH_Action { get; set; }
        public long ABH_Id { get; set; }
        public long ABH_ApplicantId { get; set; }
        public string ABH_CompanyName { get; set; }
        public string ABH_Address { get; set; }
        public string ABH_City { get; set; }
        public string ABH_State { get; set; }
        public int ABH_ZIPCode { get; set; }
        public long ABH_Phone { get; set; }
        public char ABH_StillEmployed { get; set; }
        public string ABH_ReasonforLeaving { get; set; }
        public string ABH_ReasonForGAP { get; set; }
        public char ABH_SubToFedralMotor { get; set; }
        public char ABH_SafetySensitiveFunction { get; set; }
        //public Nullable<DateTime> ABH_Date { get; set; }
        public char ABH_IsActive { get; set; }

    }

    public class ApplicantPositionTitle
    {
        public char APT_Action { get; set; }
        public long APT_Id { get; set; }
        public long APT_ApplicantId { get; set; }
        public string APT_CompanyName { get; set; }
        public string APT_PositionTitle { get; set; }
        public decimal APT_Salary { get; set; }
        public Nullable<DateTime> APT_FromMoYr { get; set; }
        public Nullable<DateTime> APT_ToMoYr { get; set; }
        public char APT_IsActive { get; set; }

    }

    public class ApplicantAccidentRecord
    {
        public char AAR_Action { get; set; }
        public long AAR_Id { get; set; }
        public long AAR_ApplicantId { get; set; }
        public Nullable<DateTime> AAR_AccidantDate { get; set; }
        //public Nullable<DateTime> AAR_Date { get; set; }
        public string AAR_Discription { get; set; }
        public int AAR_NumberOfFatalities { get; set; }
        public int AAR_NumberOfInjuries { get; set; }
        //public Nullable<DateTime> AAR_Date { get; set; }
        public char AAR_IsActive { get; set; }

    }
    public class ApplicantTrafficConvictions
    {
        public char ATC_Action { get; set; }
        public long ATC_Id { get; set; }
        public long ATC_APT_ApplicantId { get; set; }
        public Nullable<DateTime> ATC_ConvictedDate { get; set; }
        public string ATC_Violation { get; set; }
        public string ATC_StateOfViolation { get; set; }
        public bool ATC_AtFaultAccident { get; set; }
        public bool ATC_AtMovingViolation { get; set; }
        public char ATC_IsActive { get; set; }

    }
    public class ATC
    {
        public char ATC_Action { get; set; }
        public long ATC_Id { get; set; }
        public long ATC_APT_ApplicantId { get; set; }
        public Nullable<DateTime> ATC_ConvictedDate { get; set; }
        public string ATC_Violation { get; set; }
        public string ATC_StateOfViolation { get; set; }
        public char ATC_AtFaultAccident { get; set; }
        public char ATC_AtMovingViolation { get; set; }
        public char ATC_IsActive { get; set; }
    }
    public class ApplicantVehiclesOperated
    {
        public char AVO_Action { get; set; }
        public long AVO_Id { get; set; }
        public long AVO_ApplicantId { get; set; }
        public char AVO_DenideLicensePermit { get; set; }
        public string AVO_DeniedLicensePermitExplanation { get; set; }
        public char AVO_SuspendRevokeLicensePermit { get; set; }
        public string AVO_SuspendRevokeLicensePermitExplanation { get; set; }
        public char AVO_IsActive { get; set; }

    }
    public class ApplicantLicenseHeald
    {
        public char ALH_Action { get; set; }
        public long ALH_Id { get; set; }
        public long ALH_ApplicantId { get; set; }
        public string ALH_State { get; set; }
        public string ALH_LicenceNumber { get; set; }
        public string ALH_LicenseType { get; set; }
        public Nullable<DateTime> ALH_IssueDate { get; set; }
        public Nullable<DateTime> ALH_ExpirationDate { get; set; }
        public char ALH_IsActive { get; set; }

    }
    
    public class ApplicantSchecduleAvaliblity
    {
        public char ASA_Action { get; set; }
        public long ASA_Id { get; set; }
        public long ASA_ApplicantId { get; set; }
        public string ASA_WeekDay { get; set; }
        public Nullable<DateTime> ASA_AvaliableStartTime { get; set; }
        public Nullable<DateTime> ASA_AvaliableEndTime { get; set; }
        public char ASA_IsActive { get; set; }

    }
    public class AcadmicCertification
    {
        public long ACD_APT_ApplicantId { get; set; }
        public string ACD_CertificationName { get; set; }
        public Nullable<DateTime> ACD_CertificationEarnedYear { get; set; }
        public string ACD_CertifyingAgency { get; set; }
        public string ACD_CertificateAttached { get; set; }
        public string ACD_Address { get; set; }
        public string ACD_City { get; set; }
        public string ACD_State { get; set; }
        public int? ACD_Zip { get; set; }
        public string ACD_CertificateFileName { get; set; }
    }
}