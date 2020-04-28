using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PostI9FormDataInput
    {
        public string client_software_version { get; set; }
        public string duplicate_continue_reason { get; set; }
        public string first_name { get; set; }
        public object middle_initial { get; set; }
        public string last_name { get; set; }
        public object other_last_names_used { get; set; }
        public string date_of_birth { get; set; }
        public object employee_email_address { get; set; }
        public object phone_number { get; set; }
        public string ssn { get; set; }
        public object alien_number { get; set; }
        public object client_company_id { get; set; }
        public string citizenship_status_code { get; set; }
        public string document_a_type_code { get; set; }
        public object document_b_type_code { get; set; }
        public object document_c_type_code { get; set; }
        public object document_sub_type_code { get; set; }
        public object i94_number { get; set; }
        public object i551_number { get; set; }
        public object i766_number { get; set; }
        public string us_passport_number { get; set; }
        public object foreign_passport_number { get; set; }
        public object document_bc_number { get; set; }
        public string expiration_date { get; set; }
        public object country_code { get; set; }
        public object us_state_code { get; set; }
        public bool no_expiration_date { get; set; }
        public object visa_number { get; set; }
        public object employer_case_id { get; set; }
        public string date_of_hire { get; set; }
        public object reason_for_delay_code { get; set; }
        public object reason_for_delay_description { get; set; }
        public string sevis_number { get; set; }
        public string case_creator_name { get; set; }
        public string case_creator_email_address { get; set; }
        public string case_creator_phone_number { get; set; }
        public object case_creator_phone_number_extension { get; set; }        
    }
    public class GetI9FormDataOutput
    {

         public string case_number { get; set; }
         public string client_software_version { get; set; }
         public object duplicate_continue_reason { get; set; }
         public string first_name { get; set; }
         public object middle_initial { get; set; }
         public string last_name { get; set; }
         public List<object> other_last_names_used { get; set; }
         public string date_of_birth { get; set; }
         public object employee_email_address { get; set; }
         public object phone_number { get; set; }
         public string ssn { get; set; }
         public object alien_number { get; set; }
         public int client_company_id { get; set; }
         public string citizenship_status_code { get; set; }
         public string document_a_type_code { get; set; }
         public object document_b_type_code { get; set; }
         public object document_c_type_code { get; set; }
         public object document_sub_type_code { get; set; }
         public object i94_number { get; set; }
         public object i551_number { get; set; }
         public object i766_number { get; set; }
         public string us_passport_number { get; set; }
         public object foreign_passport_number { get; set; }
         public object document_bc_number { get; set; }
         public string expiration_date { get; set; }
         public object country_code { get; set; }
         public object us_state_code { get; set; }
         public bool no_expiration_date { get; set; }
         public object visa_number { get; set; }
         public object employer_case_id { get; set; }
         public string date_of_hire { get; set; }
         public object reason_for_delay_code { get; set; }
         public object reason_for_delay_description { get; set; }
         public object sevis_number { get; set; }
         public string case_creator_name { get; set; }
         public string case_creator_email_address { get; set; }
         public string case_creator_phone_number { get; set; }
         public object case_creator_phone_number_extension { get; set; }
         public object case_eligibility_statement { get; set; }
         public string case_status { get; set; }
         public object case_status_display { get; set; }
         public object dhs_referral_status { get; set; }
         public object ssa_referral_status { get; set; }
        
    }

    public class SubmitCase
    {

        public string case_number { get; set; }
        public string case_status { get; set; }
        public string case_status_display { get; set; }
        public string dhs_referral_status { get; set; }
        public string ssa_referral_status { get; set; }
        public string case_eligibility_statement { get; set; }
        
    }
}
