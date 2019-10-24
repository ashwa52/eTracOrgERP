using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.BusinessLogic
{
	public interface IGuestUser
	{
		EmployeeVIewModel GetEmployee(long UserId);
		bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel);
		DirectDepositeFormModel GetDirectDepositeDataByUserId(long UserId);
		DirectDepositeFormModel GetDirectDepositeDataByEmployeeId(string EmployeeId);
		bool SetDirectDepositeFormData(DirectDepositeFormModel model, long UserId);
		bool SetEmployeeHandbookData(EmployeeHandbookModel model, long UserId);
		EmployeeHandbookModel GetEmployeeHandBookByUserId(long UserId);
		EmployeeHandbookModel GetEmployeeHandBookByEmployeeId(string EmployeeId);
	}
}
