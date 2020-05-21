using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class DirectDepositeFormModel
	{
		public DirectDepositeFormModel()
		{
			Account1 = new AccountModel();
			Account2 = new AccountModel();
		}
		public AccountModel Account1 { get; set; }
		public AccountModel Account2 { get; set; }
		[Required]
		public string Signature { get; set; }
        public string EmployeeSignatureName { get; set; }
        [Required]
		public string PrintedName { get; set; }
		[Required]
		public string EmployeeId { get; set; }
		[Required]
		public DateTime Date { get; set; }
		public string VoidCheck { get; set; }
		public string IsActive { get; set; }
		public bool IsSave { get; set; }
    }
	public class AccountModel
	{
		[Required]
		public string AccountType { get; set; }
		[Required]
		public string EmployeeBankName { get; set; }
		[Required]
		public string Account { get; set; }
		[Required]
		public string BankRouting { get; set; }
		public decimal? DepositeAmount { get; set; }
		

	}
}
