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
        FillableFormRepository _FillableFormRepository = new FillableFormRepository();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get details of form.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public CommonFormModel GetFormDetails(CommonFormModel Obj)
        {
            
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
                            if (getEducationFormDetails != null)
                            {
                                lstDetails.EducationFormModel.EVF_Id = getEducationFormDetails.EVF_Id;
                                lstDetails.EducationFormModel.EVF_EmployeeName = getEducationFormDetails.EmployeeName;
                                lstDetails.EducationFormModel.EVF_Employee_Id = getEducationFormDetails.EVF_EMP_EmployeeID;
                                lstDetails.EducationFormModel.EVF_City = getEducationFormDetails.EVF_City;
                                lstDetails.EducationFormModel.EVF_Address = getEducationFormDetails.EVF_Address;
                                lstDetails.EducationFormModel.EVF_AttendedFrom = getEducationFormDetails.EVF_AttendedFrom;
                                lstDetails.EducationFormModel.EVF_AttendedTo = getEducationFormDetails.EVF_AttendedTo;
                                lstDetails.EducationFormModel.EVF_OrganizationName = getEducationFormDetails.EVF_OrgnizationName;
                                lstDetails.EducationFormModel.EVF_SchoolDegreeDiplomaCert = getEducationFormDetails.EVF_SchoolDegreeDiplomaCirtificate;
                                lstDetails.EducationFormModel.EVF_State = getEducationFormDetails.EVF_State;
                            }
                        }
                        else if(Obj.FormName == "DirectDepositForm")
                        {
                            var getDirectDepositForm = _FillableFormRepository.GetDirectDepositeDetails(getUserDetails.EmployeeID);
                            if (getDirectDepositForm != null)
                            {
                                lstDetails.DirectDepositFormModel.DDF_AccountNumber_1 = getDirectDepositForm.DDF_AccountNumber_1;
                                lstDetails.DirectDepositFormModel.DDF_AccountNumber_2 = getDirectDepositForm.DDF_AccountNumber_2;
                                lstDetails.DirectDepositFormModel.DDF_AccountType_1 = getDirectDepositForm.DDF_AccountType_1;
                                lstDetails.DirectDepositFormModel.DDF_AccountType_2 = getDirectDepositForm.DDF_AccountType_2;
                                lstDetails.DirectDepositFormModel.DDF_BankName_1 = getDirectDepositForm.DDF_BankName_1;
                                lstDetails.DirectDepositFormModel.DDF_BankName_2 = getDirectDepositForm.DDF_BankName_2;
                                lstDetails.DirectDepositFormModel.DDF_BankRountingNumber_1 = getDirectDepositForm.DDF_BankRoutingNumber_1;
                                lstDetails.DirectDepositFormModel.DDF_BankRountingNumber_2 = getDirectDepositForm.DDF_BankRoutingNumber_2;
                                lstDetails.DirectDepositFormModel.DDF_EMP_EmployeeId = getDirectDepositForm.DDF_EMP_EmployeeID;
                                lstDetails.DirectDepositFormModel.DDF_PercentageOrDollarAmount_1 = getDirectDepositForm.DDF_PrcentageOrDollarAmount_1;
                                lstDetails.DirectDepositFormModel.DDF_VoidCheck = getDirectDepositForm.DDF_VoidCkeck;
                                lstDetails.DirectDepositFormModel.EmployeeName = getDirectDepositForm.EmployeeName;
                            }
                        }
                        else if(Obj.FormName == "EmergencyContactInfo")
                        {
                            var getEMCForm = _FillableFormRepository.GetEmergencyContactForm(getUserDetails.EmployeeID);
                            if (getEMCForm != null)
                            {
                                lstDetails.EmergencyContactFormModel.ECF_EMP_EmployeeId = getEMCForm.ECF_EMP_EmployeeID;
                                lstDetails.EmergencyContactFormModel.ECF_HomeEmail = getEMCForm.ECF_HomeEmail;
                                lstDetails.EmergencyContactFormModel.ECF_HomePhone = getEMCForm.ECF_HomePhone;
                                lstDetails.EmergencyContactFormModel.ECF_Id = getEMCForm.ECF_Id;
                                lstDetails.EmergencyContactFormModel.ECF_NickName = getEMCForm.ECF_NickName;                            
                            }
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
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get File type list 
        /// </summary>
        /// <returns></returns>
        public List<FormTypeListModel> GetFileList(eTracLoginModel obj)
        {
            var lst = new List<FormTypeListModel>();
            try
            {
                if(obj.UserId > 0)
                {
                    lst = _FillableFormRepository.GetFileList().Select(x => new FormTypeListModel()
                    {
                        FileName = x.FLT_FileSubType,
                        FileType = x.FLT_FileType,
                        FileTypeId = x.FLT_Id,
                        IsActive = x.FLT_IsActive
                    }).ToList();
                }
                return lst;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<FormTypeListModel> GetFormList(eTracLoginModel obj)", "Exception While getting list of files with type.", obj);
                throw;
            }
        }
    }
}
