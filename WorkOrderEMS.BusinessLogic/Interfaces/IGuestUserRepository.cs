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
		string GetPhotoRelease(long userId);
		void SetPhotoRelease(long userId, PhotoRelease model);
		void SetEmergencyForm(long userId, EmergencyContectForm model);
		EmergencyContectForm GetEmergencyForm(long userId);
		ConfidenialityAgreementModel GetConfidenialityAgreementForm(long userId);
		void SetConfidenialityAgreementForm(long userId, ConfidenialityAgreementModel model);
		void SetEducationVerificationForm(long userId, EducationVarificationModel model);
		EducationVarificationModel GetEducationVerificationForm(long userId);
		void SetW4Form(long userId, W4FormModel model);
		W4FormModel GetW4Form(long userId);
		PersonalFileModel GetFormsStatus(long userId);
	}
}
