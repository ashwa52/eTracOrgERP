using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class DirectDepositFormModel
    {
        public string DDF_EMP_EmployeeId { get; set; }
        public string DDF_BankName_1 { get; set; }
        public string DDF_Account_1 { get; set; }
        public string DDF_AccountType_1 { get; set; }
        public string DDF_BankRountingNumber_1 { get; set; }
        public decimal? DDF_PercentageOrDollarAmount_1 { get; set; }
        public string DDF_BankName_2 { get; set; }
        public string DDF_AccountType_2 { get; set; }
        public string DDF_AccountNumber_1 { get; set; }
        public string DDF_AccountNumber_2 { get; set; }
        public string DDF_BankRountingNumber_2 { get; set; }
        public string DDF_VoidCheck { get; set; }
        public string EmployeeName { get; set; }
    }
}
