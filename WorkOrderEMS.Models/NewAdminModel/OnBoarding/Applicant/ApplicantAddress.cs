using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ApplicantAddress
    {
        public long APA_Id { get; set; }
        public string APA_StreetAddress { get; set; }
        public string APA_Apartment { get; set; }
        public string APA_City { get; set; }
        public string APA_State { get; set; }
        public string APA_Zip { get; set; }
        public Nullable<DateTime> APA_YearsAddressFrom { get; set; }
        public Nullable<DateTime> APA_YearsAddressTo { get; set; }

    }
    public class ApplicantContactInfo
    {
        public long ACI_PhoneNo { get; set; }
        public string ACI_eMail { get; set; }
        public string ACI_PrefredContactMethod { get; set; }
        public Nullable<DateTime> ACI_Date { get; set; }
        public string ACI_IsActive { get; set; }
    }
    public class ApplicantAdditionalInfo
    {
        public long AAI_Id { get; set; }
        public char AAI_WorkEligibilityUS { get; set; }
        public Nullable<DateTime> AAI_AvailableDate { get; set; }
        public Nullable<DateTime> AAI_Age21Completed { get; set; }
        public char AAI_AnyRefOrEmployeeInELITE { get; set; }
        public string AAI_NameOfRefOrEmployeeInELITE { get; set; }
        public char AAI_PriorMilitaryService { get; set; }
        public char AAI_EverWorkForELITE { get; set; }
        public Nullable<DateTime> AAI_DepartureDate { get; set; }
        public string AAI_ReasonForLeaving { get; set; }
        public Nullable<DateTime> AAI_Date { get; set; }
        public string AAI_IsActive { get; set; }
    }
    public class AplicantAcadmicDetails
    {
        public long AAD_Id { get; set; }
        public string AAD_EducationType { get; set; }
        public string AAD_InstituteName { get; set; }
        public Nullable<DateTime> AAD_AttendedFrom { get; set; }
        public Nullable<DateTime> AAD_AttendedTo { get; set; }
        public string AAD_City { get; set; }
        public string AAD_State { get; set; }
        public int AAD_Zip { get; set; }
        public char AAD_IsActive { get; set; }
    }
    public class ApplicantBackgroundHistory
    {
        public long ABH_Id { get; set; }
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
        public Nullable<DateTime> ABH_Date { get; set; }
        public char ABH_IsActive { get; set; }
    }

    public class ApplicantPositionTitle
    {
        public long APT_Id { get; set; }
        public string APT_PositionTitle { get; set; }
        public decimal APT_Salary { get; set; }
        public Nullable<DateTime> MyProperty { get; set; }
        public Nullable<DateTime> APT_ToMoYr { get; set; }
        public char APT_IsActive { get; set; }
    }

    public class ApplicantAccidentRecord
    {
        public long AAR_Id { get; set; }
        public Nullable<DateTime> AAR_AccidantDate { get; set; }
        public string AAR_Discription { get; set; }
        public int AAR_NumberOfFatalities { get; set; }
        public int AAR_NumberOfInjuries { get; set; }
        public char AAR_IsActive { get; set; }
    }
    public class ApplicantTrafficConvictions
    {
        public long ATC_Id { get; set; }
        public Nullable<DateTime> ATC_ConvictedDate { get; set; }
        public string ATC_Violation { get; set; }
        public string ATC_StateOfViolation { get; set; }
        public char ATC_AtFaultAccident { get; set; }
        public char ATC_AtMovingViolation { get; set; }
        public char ATC_IsActive { get; set; }
    }
    public class ApplicantVehiclesOperated
    {
        public long AVO_Id { get; set; }
        public char AVO_DenideLicensePermit { get; set; }
        public string AVO_DeniedLicensePermitExplanation { get; set; }
        public char AVO_SuspendRevokeLicensePermit { get; set; }
        public string AVO_SuspendRevokeLicensePermitExplanation { get; set; }
        public char AVO_IsActive { get; set; }
    }
    public class ApplicantLicenseHeald
    {
        public long ALH_Id { get; set; }
        public string ALH_State { get; set; }
        public string ALH_LicenceNumber { get; set; }
        public string ALH_LicenseType { get; set; }
        public Nullable<DateTime> ALH_IssueDate { get; set; }
        public Nullable<DateTime> ALH_ExpirationDate { get; set; }
        public char ALH_IsActive { get; set; }
    }
    public class ApplicantSchecduleAvaliblity
    {
        public long ASA_Id { get; set; }
        public DateTime ASA_AvaliableStartTime { get; set; }
        public DateTime ASA_AvaliableEndTime { get; set; }
        public char ASA_IsActive { get; set; }
    }
}