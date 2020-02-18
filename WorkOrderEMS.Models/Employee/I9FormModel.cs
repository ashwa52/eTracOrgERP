using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class I9FormModel
    {
        public string EMP_FirstName { get; set; }
        public string EMP_MiddleName { get; set; }
        public string EMP_LastName { get; set; }
        public string EMA_Address { get; set; }
        public string EMA_City { get; set; }
        public string EMA_State { get; set; }
        public Nullable<int> EMA_Zip { get; set; }
        public long? I9F_Id { get; set; }
        public string I9F_EMP_EmployeeId { get; set; }
        public string I9F_Sec1_SSN { get; set; }
        public string I9F_Sec1_CitizenOfUS { get; set; }
        public string I9F_Sec1_NonCitizenOfUS { get; set; }
        public string I9F_Sec1_AlienRegistrationNum_USCIS { get; set; }
        public Nullable<System.DateTime> I9F_Sec1_AlienAuthorizedToWorkDate { get; set; }
        public string I9F_Sec1_I94AdmissionNumber { get; set; }
        public string I9F_Sec1_ForeignPassportNumber { get; set; }
        public string I9F_Sec1_ForeignPassportIssuanceCountry { get; set; }
        public string I9F_Sec1_SignatureOfEmployee { get; set; }
        public Nullable<System.DateTime> I9F_Sec1_DateOfEmployeeSign { get; set; }
        public string I9F_Sec1_QRCodeSec1 { get; set; }
        public string I9F_Sec1_PreparerAndTranslator { get; set; }
        public string I9F_Sec1_SignatureOfPreparerOrTranslator { get; set; }
        public Nullable<System.DateTime> I9F_Sec1_DateOfPreparerOrTranslatorSign { get; set; }
        public string I9F_Sec1_FirstName { get; set; }
        public string I9F_Sec1_LastName { get; set; }
        public string I9F_Sec1_Address { get; set; }
        public string I9F_Sec1_City { get; set; }
        public string I9F_Sec1_State { get; set; }
        public Nullable<int> I9F_Sec1_ZipCode { get; set; }
        public string I9F_Sec2_ListA_DocumentTitle1 { get; set; }
        public string I9F_Sec2_ListA_IssuingAuthority1 { get; set; }
        public string I9F_Sec2_ListA_DocumentNumber1 { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_ListA_ExpirationDate1 { get; set; }
        public string I9F_Sec2_ListA_DocumentTitle2 { get; set; }
        public string I9F_Sec2_ListA_IssuingAuthority2 { get; set; }
        public string I9F_Sec2_ListA_DocumentNumber2 { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_ListA_ExpirationDate2 { get; set; }
        public string I9F_Sec2_ListA_DocumentTitle3 { get; set; }
        public string I9F_Sec2_ListA_IssuingAuthority3 { get; set; }
        public string I9F_Sec2_ListA_DocumentNumber3 { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_ListA_ExpirationDate3 { get; set; }
        public string I9F_Sec2_ListB_DocumentTitle { get; set; }
        public string I9F_Sec2_ListB_IssuingAuthority { get; set; }
        public string I9F_Sec2_ListB_DocumentNumber { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_ListB_ExpirationDate { get; set; }
        public string I9F_Sec2_ListC_DocumentTitle { get; set; }
        public string I9F_Sec2_ListC_IssuingAuthority { get; set; }
        public string I9F_Sec2_ListC_DocumentNumber { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_ListC_ExpirationDate { get; set; }
        public string I9F_Sec2_AdditionalInformation { get; set; }
        public string I9F_Sec2_QRCodeSec2AndSec3 { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_EmployeesFirstDayOfEmployment { get; set; }
        public string I9F_Sec2_SignatureOfEmployerOrAuthorized { get; set; }
        public Nullable<System.DateTime> I9F_Sec2_DateOfEmployerOrAuthorizedSign { get; set; }
        public string I9F_Sec2_LastNameOfEmployerOrAuthorized { get; set; }
        public string I9F_Sec2_FirstNameOfEmployerOrAuthorized { get; set; }
        public string I9F_Sec2_MiddleInitialOfEmployerOrAuthorized { get; set; }
        public string I9F_Sec2_EmployersBusinessOrgnization_Name { get; set; }
        public string I9F_Sec2_EmployersBusinessOrgnization_Address { get; set; }
        public string I9F_Sec2_EmployersBusinessOrgnization_City { get; set; }
        public string I9F_Sec2_EmployersBusinessOrgnization_State { get; set; }
        public Nullable<int> I9F_Sec2_EmployersBusinessOrgnization_ZipCode { get; set; }
        //public string I9F_Sec2_A_LastName { get; set; }
        //public string I9F_Sec2_A_FirstName { get; set; }
        //public string I9F_Sec2_A_MiddleInitial { get; set; }
        //public Nullable<System.DateTime> I9F_Sec2_B_DateOfReHire { get; set; }
        //public string I9F_Sec2_C_DocumentTitle { get; set; }
        //public string I9F_Sec2_C_DocumentNumber { get; set; }
        //public Nullable<System.DateTime> I9F_Sec2_C_ExpirationDate { get; set; }
        //public string I9F_Sec2_C_SignatureOfEmployerOrAuthorized { get; set; }
        //public Nullable<System.DateTime> I9F_Sec2_C_DateOfEmployerOrAuthorizedSign { get; set; }
        //public string I9F_Sec2_C_NameOfEmployerOrAuthorized { get; set; }
        public Nullable<System.DateTime> I9F_Date { get; set; }
        public string I9F_IsActive { get; set; }


        //---------------New Add------------------------------------
        public string I9F_Sec1_MiddleInitiaL { get; set; }
        public string I9F_Sec1_OtherLastName { get; set; }
        public string I9F_Sec1_AptNumber { get; set; }

        public DateTime? I9F_Sec1_dateOfBirth { get; set; }

        public string I9F_Sec1_Email { get; set; }

        public long? I9F_Sec1_EmployeeTelephoneNumber { get; set; }

        public string I9F_Sec2_TitleOfEmployerOrOthrizedRepresentative { get; set; }

        public string I9F_Sec2_CitizenshipImmigrationStatus { get; set; }

        public string I9F_Sec3_A_LastName { get; set; }

        public string I9F_Sec3_A_FirstName { get; set; }

        public string I9F_Sec3_A_MiddleInitial { get; set; }

        public DateTime? I9F_Sec3_B_DateOfReHire { get; set; }

        public string I9F_Sec3_C_DocumentTitle { get; set; }

        public string I9F_Sec3_C_DocumentNumber { get; set; }

        public DateTime? I9F_Sec3_C_ExpirationDate { get; set; }

        public string I9F_Sec3_C_SignatureOfEmployerOrAuthorized { get; set; }

        public DateTime? I9F_Sec3_C_DateOfEmployerOrAuthorizedSign { get; set; }

        public string I9F_Sec3_C_NameOfEmployerOrAuthorized { get; set; }

        public bool IsSave { get; set; }
    }
}
