using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class Self
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
    }

    public class ProviderLastName
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class ProviderFirstName
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class ProviderType
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class ProviderNpi
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class ProviderTaxId
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class PayerAssignedProviderId
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class SubmitterId
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class ProviderCity
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class ProviderState
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class ProviderZipCode
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class PlaceOfService
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class AsOfDate
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public DateTime defaultValue { get; set; }
        public DateTime min { get; set; }
        public DateTime max { get; set; }
    }

    public class ToDate
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class Value
    {
        public string code { get; set; }
        public string value { get; set; }
    }

    public class ServiceType
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string mode { get; set; }
        public List<Value> values { get; set; }
    }

    public class ProcedureCode
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class MemberId
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class MedicaidId
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class PatientLastName
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class PatientFirstName
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class PatientMiddleName
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class PatientSuffix
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public string pattern { get; set; }
        public int maxLength { get; set; }
    }

    public class PatientBirthDate
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public DateTime min { get; set; }
        public DateTime max { get; set; }
    }

    public class PatientGender
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class PatientState
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class DefaultValue
    {
        public string code { get; set; }
        public string value { get; set; }
    }

    public class Value2
    {
        public string code { get; set; }
        public string value { get; set; }
    }

    public class SubscriberRelationship
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
        public DefaultValue defaultValue { get; set; }
        public string mode { get; set; }
        public List<Value2> values { get; set; }
    }

    public class GroupNumber
    {
        public string type { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public bool allowed { get; set; }
        public bool required { get; set; }
        public string errorMessage { get; set; }
    }

    public class Elements
    {
        public ProviderLastName providerLastName { get; set; }
        public ProviderFirstName providerFirstName { get; set; }
        public ProviderType providerType { get; set; }
        public ProviderNpi providerNpi { get; set; }
        public ProviderTaxId providerTaxId { get; set; }
        public PayerAssignedProviderId payerAssignedProviderId { get; set; }
        public SubmitterId submitterId { get; set; }
        public ProviderCity providerCity { get; set; }
        public ProviderState providerState { get; set; }
        public ProviderZipCode providerZipCode { get; set; }
        public PlaceOfService placeOfService { get; set; }
        public AsOfDate asOfDate { get; set; }
        public ToDate toDate { get; set; }
        public ServiceType serviceType { get; set; }
        public ProcedureCode procedureCode { get; set; }
        public MemberId memberId { get; set; }
        public MedicaidId medicaidId { get; set; }
        public PatientLastName patientLastName { get; set; }
        public PatientFirstName patientFirstName { get; set; }
        public PatientMiddleName patientMiddleName { get; set; }
        public PatientSuffix patientSuffix { get; set; }
        public PatientBirthDate patientBirthDate { get; set; }
        public PatientGender patientGender { get; set; }
        public PatientState patientState { get; set; }
        public SubscriberRelationship subscriberRelationship { get; set; }
        public GroupNumber groupNumber { get; set; }
    }

    public class RequiredFieldCombinations
    {
        public List<List<string>> patient { get; set; }
    }

    public class Configuration
    {
        public string type { get; set; }
        public string categoryId { get; set; }
        public string categoryValue { get; set; }
        public string payerId { get; set; }
        public string version { get; set; }
        public Elements elements { get; set; }
        public RequiredFieldCombinations requiredFieldCombinations { get; set; }
    }

    public class BenefitList
    {
        public int totalCount { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public Links links { get; set; }
        public List<Configuration> configurations { get; set; }
    }

    //public class FloridaBlueAuthentication
    //{
    //    public string token_type { get; set; }
    //    public string access_token { get; set; }
    //    public int expires_in { get; set; }
    //    public int consented_on { get; set; }
    //    public string scope { get; set; }        
    //}
}
