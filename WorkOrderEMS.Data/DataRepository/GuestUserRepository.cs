using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Data.DataRepository
{
    public class GuestUserRepositoryData : IGuestUserRepository
    {
  
        private readonly workorderEMSEntities objworkorderEMSEntities;
        public GuestUserRepositoryData()
        {
            objworkorderEMSEntities = new workorderEMSEntities();
        }
        public EmployeeVIewModel GetEmployee(long userId)
        {
            var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;
            return objworkorderEMSEntities.spGetEmployeePersonalInfo(employeeId).
                Select(x => new EmployeeVIewModel
                {
                    Address = x.EMA_Address,
                    City = x.EMA_City,
                    State = x.EMA_State,
                    Cityzenship = x.CTZ_Citizenship,
                    DlNumber = x.EMP_DrivingLicenseNumber,
                    Dob = x.EMP_DateOfBirth,
                    Email = x.EMP_Email,
                    EmpId = x.EMP_EmployeeID,
                    FirstName = x.EMP_FirstName,
                    LastName = x.EMP_LastName,
                    MiddleName = x.EMP_MiddleName,
                    Image = x.EMP_Photo,
                    Phone = x.EMP_Phone,
                    SocialSecurityNumber = x.EMP_SSN,
                    Zip = x.EMA_Zip,
                    LicenseNumber = x.EMP_DrivingLicenseNumber
                }).FirstOrDefault(); ;
        }
        public bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel)
        {
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var isEmployeeExists = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).Any();
                    var EMPAction = isEmployeeExists ? "U" : "I";
                    var result = Context.spSetEmployee(EMPAction, null, onboardingDetailRequestModel.EmpId, null,
                                                onboardingDetailRequestModel.FirstName, onboardingDetailRequestModel.MiddleName, onboardingDetailRequestModel.LastName,
                                                onboardingDetailRequestModel.Email, onboardingDetailRequestModel.Phone
                                                , onboardingDetailRequestModel.DlNumber, onboardingDetailRequestModel.Dob, onboardingDetailRequestModel.SocialSecurityNumber,
                                                null, null, null, null, null,
                                                null, null, null, DateTime.Now, "1", null, onboardingDetailRequestModel.Address,
                                                onboardingDetailRequestModel.City, onboardingDetailRequestModel.State, null, onboardingDetailRequestModel.Cityzenship).ToList();

                    if (result.Any())
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
