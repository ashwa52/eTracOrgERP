﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Data.DataRepository
{
	public class NewAdminRepository : BaseRepository<ApplicantInfo>, INewAdminRepository
	{
		workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

		public List<spGetApplicantInfo_Result1> GetApplicantInfo(string employeeId)
		{
			return _workorderEMSEntities.spGetApplicantInfo(employeeId).ToList();
		}

		public bool SaveApplicantInfo(OnboardingDetailRequestModel onboardingDetailRequestModel)
		{
			return true;
		}

		public bool SaveGuestEmployeeBasicInfo(GuestEmployeeBasicInfoRequestModel guestEmployeeBasicInfoRequestModel)
		{
			return true;
		}

	}
}
