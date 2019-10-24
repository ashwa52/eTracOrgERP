using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class FillableFormManager : IFillableFormManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get details of form.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public CommonFormModel GetFormDetails(CommonFormModel Obj)
        {
            var _FillableFormRepository = new FillableFormRepository();
            var lstDetails = new CommonFormModel();
            try
            {
                if (Obj.UserId > 0)
                {
                    var getUserDetails = _workorderems.UserRegistrations.Where(x => x.UserId == Obj.UserId && x.IsEmailVerify == true && x.IsDeleted == false).FirstOrDefault();
                    if (getUserDetails != null)
                    {
                        if(Obj.FormName == "EducationForm")
                        {
                            var getEducationFormDetails = _FillableFormRepository.GetEducationFormDetails(getUserDetails.EmployeeID);
                            //if(getEducationFormDetails != null)
                            //{
                            //    lstDetails.EducationFormModel.EVF_Id = getEducationFormDetails.EVF_Id;
                            //    lstDetails.EducationFormModel.EVF_EmployeeName = getEducationFormDetails.EmployeeName;
                            //    lstDetails.EducationFormModel.EVF_Employee_Id = getEducationFormDetails.EVF_EMP_EmployeeID;
                            //    lstDetails.EducationFormModel.EVF_City = getEducationFormDetails.EVF_City;
                            //    lstDetails.EducationFormModel.EVF_Address = getEducationFormDetails.EVF_Address;
                            //    lstDetails.EducationFormModel.EVF_AttendedFrom = getEducationFormDetails.EVF_AttendedFrom;
                            //    lstDetails.EducationFormModel.EVF_AttendedTo = getEducationFormDetails.EVF_AttendedTo;
                            //    lstDetails.EducationFormModel.EVF_OrganizationName = getEducationFormDetails.EVF_OrgnizationName;
                            //    lstDetails.EducationFormModel.EVF_SchoolDegreeDiplomaCert = getEducationFormDetails.EVF_SchoolDegreeDiplomaCirtificate;
                            //    lstDetails.EducationFormModel.EVF_State = getEducationFormDetails.EVF_State;
                            //}
                        }
                        else if(Obj.FormName == "DirectDepositForm")
                        {
                            var getDirectDepositForm = _FillableFormRepository.GetDirectDepositeDetails(getUserDetails.EmployeeID);
                            //if (getDirectDepositForm != null)
                            //{
                            //    lstDetails.EducationFormModel.EVF_Id = ;
                            //    lstDetails.EducationFormModel.EVF_EmployeeName = ;
                            //    lstDetails.EducationFormModel.EVF_Employee_Id = getEducationFormDetails.EVF_EMP_EmployeeID;
                            //    lstDetails.EducationFormModel.EVF_City = getEducationFormDetails.EVF_City;
                            //    lstDetails.EducationFormModel.EVF_Address = getEducationFormDetails.EVF_Address;
                            //    lstDetails.EducationFormModel.EVF_AttendedFrom = getEducationFormDetails.EVF_AttendedFrom;
                            //    lstDetails.EducationFormModel.EVF_AttendedTo = getEducationFormDetails.EVF_AttendedTo;
                            //    lstDetails.EducationFormModel.EVF_OrganizationName = getEducationFormDetails.EVF_OrgnizationName;
                            //    lstDetails.EducationFormModel.EVF_SchoolDegreeDiplomaCert = getEducationFormDetails.EVF_SchoolDegreeDiplomaCirtificate;
                            //    lstDetails.EducationFormModel.EVF_State = getEducationFormDetails.EVF_State;
                            //}
                        }
                        else if(Obj.FormName == "EmergencyContactInfo")
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CommonFormModel GetFormDetails(CommonFormModel Obj)", "Exception While getting details of fillable form.", Obj);
                throw;
            }
            return lstDetails;           
        }
    }
}
