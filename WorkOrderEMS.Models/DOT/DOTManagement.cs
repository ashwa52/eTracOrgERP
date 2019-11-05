using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class DOTManagementViewData
    {
       public string NameofCompanyApplying { get; set; }
       public string CompanyID { get; set; }
        public DateTime TodayDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int ZipPostCode { get; set; }
        public string Email { get; set; }
        public string HomeTelephone { get; set; }
        public string IDNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        
    }
}
