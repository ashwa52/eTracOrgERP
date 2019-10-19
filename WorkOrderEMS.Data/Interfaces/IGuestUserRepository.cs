using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface IGuestUserRepository
    {
        EmployeeVIewModel GetEmployee(long UserId);
        bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel);
    }
}
