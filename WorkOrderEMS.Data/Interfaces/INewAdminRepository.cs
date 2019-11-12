using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Data.Interfaces
{
	public interface INewAdminRepository
	{
		List<spGetApplicantInfo_Result2> GetApplicantInfo(string employeeId);
		bool SaveApplicantInfo(OnboardingDetailRequestModel onboardingDetailRequestModel);
		bool SaveGuestEmployeeBasicInfo(GuestEmployeeBasicInfoRequestModel guestEmployeeBasicInfoRequestModel);
	}
}
