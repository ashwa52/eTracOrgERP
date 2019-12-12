using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;

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
            var edu_form = new EducationFormModel();
            var direct_depo = new DirectDepositFormModel();
            var w4_Form = new W4FormModel();
            var emergency_form = new EmergencyContactFormModel();
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
                                edu_form.EVF_Id = getEducationFormDetails.EVF_Id;
                                edu_form.EVF_EmployeeName = getEducationFormDetails.EmployeeName;
                                edu_form.EVF_Employee_Id = getEducationFormDetails.EVF_EMP_EmployeeID;
                                edu_form.EVF_City = getEducationFormDetails.EVF_City;
                                edu_form.EVF_Address = getEducationFormDetails.EVF_Address;
                                edu_form.EVF_AttendedFrom = getEducationFormDetails.EVF_AttendedFrom.Value.ToString("yyyy-MM-dd");
                                edu_form.EVF_AttendedTo = getEducationFormDetails.EVF_AttendedTo.Value.ToString("yyyy-MM-dd");
                                edu_form.EVF_OrganizationName = getEducationFormDetails.EVF_OrgnizationName;
                                edu_form.EVF_SchoolDegreeDiplomaCert = getEducationFormDetails.EVF_SchoolDegreeDiplomaCirtificate;
                                edu_form.EVF_State = getEducationFormDetails.EVF_State;
                                lstDetails.EducationFormModel = edu_form;
                            }
                        }
                        else if(Obj.FormName == "DirectDepositForm")
                        {
                            var getDirectDepositForm = _FillableFormRepository.GetDirectDepositeDetails(getUserDetails.EmployeeID);
                            if (getDirectDepositForm != null)
                            {
                                direct_depo.DDF_AccountNumber_1 = getDirectDepositForm.DDF_AccountNumber_1;
                                direct_depo.DDF_AccountNumber_2 = getDirectDepositForm.DDF_AccountNumber_2;
                                direct_depo.DDF_AccountType_1 = getDirectDepositForm.DDF_AccountType_1;
                                direct_depo.DDF_AccountType_2 = getDirectDepositForm.DDF_AccountType_2;
                                direct_depo.DDF_BankName_1 = getDirectDepositForm.DDF_BankName_1;
                                direct_depo.DDF_BankName_2 = getDirectDepositForm.DDF_BankName_2;
                                direct_depo.DDF_BankRountingNumber_1 = getDirectDepositForm.DDF_BankRoutingNumber_1;
                                direct_depo.DDF_BankRountingNumber_2 = getDirectDepositForm.DDF_BankRoutingNumber_2;
                                direct_depo.DDF_EMP_EmployeeId = getDirectDepositForm.DDF_EMP_EmployeeID;
                                direct_depo.DDF_PercentageOrDollarAmount_1 = getDirectDepositForm.DDF_PrcentageOrDollarAmount_1;
                                direct_depo.DDF_VoidCheck = getDirectDepositForm.DDF_VoidCkeck;
                                direct_depo.EmployeeName = getDirectDepositForm.EmployeeName;
                                lstDetails.DirectDepositFormModel = direct_depo;
                            }
                        }
                        else if(Obj.FormName == "EmergencyContactInfo")
                        {
                            var getEMCForm = _FillableFormRepository.GetEmergencyContactForm(getUserDetails.EmployeeID);
                            if (getEMCForm != null)
                            {
                                emergency_form.ECF_EMP_EmployeeId = getEMCForm.ECF_EMP_EmployeeID;
                                emergency_form.ECF_HomeEmail = getEMCForm.ECF_HomeEmail;
                                emergency_form.ECF_HomePhone = getEMCForm.ECF_HomePhone;
                                emergency_form.ECF_Id = getEMCForm.ECF_Id;
                                emergency_form.ECF_NickName = getEMCForm.ECF_NickName;
                                lstDetails.EmergencyContactFormModel = emergency_form;
                            }
                        }
                        else if(Obj.FormName == "W-4")
                        {
                            var getW4Form = _FillableFormRepository.GetW4FormData(getUserDetails.EmployeeID);
                            if(getW4Form != null)
                            {
                                w4_Form.EmployeerNameAndAddress = getW4Form.EMA_Address;
                                w4_Form.City = getW4Form.EMA_City;
                                w4_Form.State = getW4Form.EMA_State;
                                w4_Form.Zip = getW4Form.EMA_Zip;
                                w4_Form.FirstName = getW4Form.EMP_FirstName;
                                w4_Form.LastName = getW4Form.EMP_LastName;
                                w4_Form.MiddleName = getW4Form.EMP_MiddleName;
                                w4_Form.w4F_10 = getW4Form.w4F_10;
                                w4_Form.EmployeeMaritalStatus= getW4Form.w4F_3MaritalStatus;
                                w4_Form.w4F_4 = getW4Form.w4F_4;
                                w4_Form.w4F_5 = getW4Form.w4F_5;
                                w4_Form.w4F_6 = getW4Form.w4F_6;
                                w4_Form.w4F_7 = getW4Form.w4F_7;
                                w4_Form.w4F_8EmployersName = getW4Form.w4F_8EmployersName;
                                w4_Form.w4F_9 = getW4Form.w4F_9;
                                w4_Form.EmpId = getW4Form.W4F_EMP_EmployeeId;
                                w4_Form.W4FId = getW4Form.W4F_Id;
                                w4_Form.SSN = getW4Form.W4F_SSN;
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
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 10-Oct-2019
        /// Created For : To save file
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public bool SaveFile(UploadedFiles Obj, string EmployeeId)
        {
            var _repo = new FillableFormRepository();
            bool isSaved = false;
            try
            {
                Obj.Action = Obj.Id != null ? "U" : "I";                
                var save = _repo.SetFiles(Obj, EmployeeId);               
                isSaved = true;
            }
            catch (Exception ex)
            {
                isSaved = false;
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveFile(UploadedFiles Obj, string EmployeeId)", "Exception While Saving File.", Obj);
                throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 02-11-2019
        /// Created For : TO save files from mobile
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveFileList(CommonFormModel obj)
        {
            bool isSaved = false;
            var _guestRepository = new GuestUserRepository();

            try
            {
                if(obj != null)
                {
                    if(obj.FormName == "EducationForm" && obj.EducationFormModel != null)
                    {
                        var educatioModel = new EducationVarificationModel();
                        educatioModel.EvfId = obj.EducationFormModel.EVF_Id;
                        educatioModel.Certificate = obj.EducationFormModel.EVF_SchoolDegreeDiplomaCert;
                        educatioModel.HighSchool.SchoolName = obj.EducationFormModel.EVF_OrganizationName;
                        educatioModel.HighSchool.City = obj.EducationFormModel.EVF_City;
                        educatioModel.HighSchool.State = obj.EducationFormModel.EVF_State;
                        educatioModel.HighSchool.AttendFrom = Convert.ToDateTime(obj.EducationFormModel.EVF_AttendedFrom);
                        educatioModel.HighSchool.AttendTo = Convert.ToDateTime(obj.EducationFormModel.EVF_AttendedTo);
                        educatioModel.IsActive = "Y";
                        _guestRepository.SetEducationVerificationForm(obj.UserId, educatioModel);
                        isSaved = true;
                    }
                    else if (obj.FormName == "DirectDepositForm" && obj.DirectDepositFormModel != null)
                    {
                        var directDepo = new DirectDepositeFormModel();
                        directDepo.Account1.EmployeeBankName = obj.DirectDepositFormModel.DDF_BankName_1;
                        directDepo.Account1.AccountType = obj.DirectDepositFormModel.DDF_AccountType_1;
                        directDepo.Account1.Account = obj.DirectDepositFormModel.DDF_AccountNumber_1;
                        directDepo.Account1.BankRouting = obj.DirectDepositFormModel.DDF_BankRountingNumber_1;
                        directDepo.Account1.DepositeAmount = obj.DirectDepositFormModel.DDF_PercentageOrDollarAmount_1;
                        directDepo.Account2.EmployeeBankName = obj.DirectDepositFormModel.DDF_BankName_2;
                        directDepo.Account2.AccountType = obj.DirectDepositFormModel.DDF_AccountType_2;
                        directDepo.Account2.Account = obj.DirectDepositFormModel.DDF_AccountNumber_2;
                        directDepo.Account2.BankRouting = obj.DirectDepositFormModel.DDF_BankRountingNumber_2;
                        directDepo.Account2.DepositeAmount = obj.DirectDepositFormModel.DDF_PercentageOrDollarAmount_2;
                        _guestRepository.SetDirectDepositeFormData(directDepo,obj.UserId);
                        isSaved = true;
                    }
                    else
                    {
                        isSaved = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return isSaved;
        }
    }
}
