using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class EmergencyContactFormModel
    {
        public long ECF_Id { get; set; }
        public string ECF_EMP_EmployeeId { get; set; }
        public string ECF_NickName { get; set; }
        public long? ECF_HomePhone { get; set; }
        public string ECF_HomeEmail { get; set; }
        public string ECF_FirstName { get; set; }
        public string ECF_MiddleName { get; set; }
        public string ECF_LastName { get; set; }
        public string ECF_Gender { get; set; }
        public string ECF_Citizenship { get; set; }
        public string ECF_HomeAddress { get; set; }
        public long? ECF_Mobile { get; set; }
        public Nullable<DateTime> ECF_BirthDate { get; set; }
        public string ECF_SSN { get; set; }
        public string ECF_DriverLicense { get; set; }
        public string ECF_EmergencyContactName { get; set; }
        public string ECF_Relationship { get; set; }
        public string ECF_Address { get; set; }
        public long? ECF_PhoneNumber { get; set; }
    }
}
