using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class RateOfPayModel
    {
        public int RateOfPayId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public string ManagerName { get; set; }
        public string Operations { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public decimal? RateOfPay { get; set; }
        public string TypeOfPayChange { get; set; }
        public string JobStatus { get; set; }
        public string Emp_Signature { get; set; }
        public Nullable<DateTime> Emp_Date { get; set; }
        public string Man_Signature { get; set; }
        public Nullable<DateTime> Man_Date { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public bool IsSave { get; set; }
    }
}
