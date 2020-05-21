using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class BackgroundScreeningAPIModel
    {
        public bool collectAdditionalInfo { get; set; }
        public string comment { get; set; }
        public List<CountyCivilLowerSearch> countyCivilLowerSearches { get; set; }
        public List<CountyCivilUpperSearch> countyCivilUpperSearches { get; set; }
        public List<CountyCriminalSearch> countyCriminalSearches { get; set; }
        public long customScreeningId { get; set; }
        public object customSubjectId { get; set; }
        public bool disableDuplicateChecking { get; set; }
        public List<DrugScreening> drugScreenings { get; set; }
        public List<EducationVerification> educationVerifications { get; set; }
        public List<EmploymentVerification> employmentVerifications { get; set; }
        public List<FederalBankruptcySearch> federalBankruptcySearches { get; set; }
        public List<FederalCivilSearch> federalCivilSearches { get; set; }
        public List<FederalCriminalSearch> federalCriminalSearches { get; set; }
        public List<MotorVehicleRecordSearch> motorVehicleRecordSearches { get; set; }
        public string packageId { get; set; }
        public PositionLocation positionLocation { get; set; }
        public PositionStartingPay positionStartingPay { get; set; }
        public bool processAddressInsight { get; set; }
        public bool processAdverseAction { get; set; }
        public bool processBusinessCreditCheck { get; set; }
        public bool processCreditCheck { get; set; }
        public bool processFacisScreening { get; set; }
        public bool processGlobalInsight { get; set; }
        public bool processGsaScreening { get; set; }
        public bool processNationalCriminalInsight { get; set; }
        public bool processOfacScreening { get; set; }
        public bool processSocialSecurityTrace { get; set; }
        public string referenceId { get; set; }
        public string requesterEmail { get; set; }
        public string requesterName { get; set; }
        public List<SexOffenderSearch> sexOffenderSearches { get; set; }
        public List<StateCriminalSearch> stateCriminalSearches { get; set; }
        public List<SubjectAdmittedCriminalHistory> subjectAdmittedCriminalHistory { get; set; }
        public string subjectCurrentAddressAddressLine { get; set; }
        public string subjectCurrentAddressCountryCode { get; set; }
        public string subjectCurrentAddressMunicipality { get; set; }
        public string subjectCurrentAddressPostalCode { get; set; }
        public string subjectCurrentAddressRegion { get; set; }
        public string subjectCurrentAddressStartDate { get; set; }
        public string subjectDateOfBirth { get; set; }
        public string subjectEmail { get; set; }
        public string subjectFamilyName { get; set; }
        public object subjectFederalEmployerIdentificationNumber { get; set; }
        public string subjectGender { get; set; }
        public string subjectGivenName { get; set; }
        public List<SubjectIndividualAlias> subjectIndividualAliases { get; set; }
        public string subjectMiddleName { get; set; }
        public object subjectOrganizationName { get; set; }
        public List<SubjectPreviousLocation> subjectPreviousLocations { get; set; }
        public string subjectSocialSecurityNumber { get; set; }
        public string subjectTelephoneNumber { get; set; }
        public string userDefinedField2 { get; set; }
        public string userDefinedField3 { get; set; }
        public List<WorkersCompensationSearch> workersCompensationSearches { get; set; }
    }
    public class CountyCivilLowerSearch
    {
        public string county { get; set; }
        public string region { get; set; }
    }

    public class CountyCivilUpperSearch
    {
        public string county { get; set; }
        public string region { get; set; }
    }

    public class CountyCriminalSearch
    {
        public string county { get; set; }
        public string region { get; set; }
    }

    public class DrugScreening
    {
        public string testType { get; set; }
    }

    public class EducationVerification
    {
        public string address { get; set; }
        public string courseOfStudy { get; set; }
        public string degree { get; set; }
        public string fromDate { get; set; }
        public bool graduated { get; set; }
        public string municipality { get; set; }
        public string organization { get; set; }
        public string phone { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
        public string studentName { get; set; }
        public string toDate { get; set; }
    }

    public class EmploymentVerification
    {
        public string address { get; set; }
        public string contactName { get; set; }
        public string contactTelephone { get; set; }
        public string employer { get; set; }
        public string fromDate { get; set; }
        public bool isCurrentEmployer { get; set; }
        public string municipality { get; set; }
        public string reasonForLeaving { get; set; }
        public string region { get; set; }
        public string remunerationInterval { get; set; }
        public int remunerationValue { get; set; }
        public bool permissionToContact { get; set; }
        public string positionTitle { get; set; }
        public string postalCode { get; set; }
        public string toDate { get; set; }
    }

    public class FederalBankruptcySearch
    {
        public string county { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
    }

    public class FederalCivilSearch
    {
        public string county { get; set; }
        public string region { get; set; }
    }

    public class FederalCriminalSearch
    {
        public string county { get; set; }
        public object courtName { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
    }

    public class MotorVehicleRecordSearch
    {
        public string countryCode { get; set; }
        public string licenseIdentifier { get; set; }
        public string region { get; set; }
    }

    public class PositionLocation
    {
        public string address { get; set; }
        public string countryCode { get; set; }
        public string municipality { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
    }

    public class PositionStartingPay
    {
        public string currencyId { get; set; }
        public string interval { get; set; }
        public int value { get; set; }
    }

    public class SexOffenderSearch
    {
        public string region { get; set; }
    }

    public class StateCriminalSearch
    {
        public string region { get; set; }
    }

    public class SubjectAdmittedCriminalHistory
    {
        public string caseNumber { get; set; }
        public string charge { get; set; }
        public string date { get; set; }
        public string disposition { get; set; }
        public string jurisdiction { get; set; }
        public string finalLevel { get; set; }
        public string notes { get; set; }
        public string sentence { get; set; }
    }

    public class SubjectIndividualAlias
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    public class SubjectPreviousLocation
    {
        public string address { get; set; }
        public string countryCode { get; set; }
        public string endDate { get; set; }
        public string municipality { get; set; }
        public string postalCode { get; set; }
        public string region { get; set; }
        public string startDate { get; set; }
    }

    public class WorkersCompensationSearch
    {
        public string region { get; set; }
    }
}
