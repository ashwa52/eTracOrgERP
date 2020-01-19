using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.BusinessLogic
{
    public class GuestUserRepository : IGuestUser
    {
        private readonly workorderEMSEntities objworkorderEMSEntities;

        public GuestUserRepository()
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
                    Zip = x.EMA_Zip
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
        public DirectDepositeFormModel GetDirectDepositeDataByUserId(long UserId)
        {
            try
            {
                DirectDepositeFormModel result = new DirectDepositeFormModel();
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;
                    result = Context.spGetDirectDepositForm(EmployeeId).Select(x => new DirectDepositeFormModel
                    {
                        Account1 = new AccountModel
                        {
                            Account = x.DDF_AccountNumber_1,
                            AccountType = x.DDF_AccountType_1,
                            BankRouting = x.DDF_BankRoutingNumber_1,
                            DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
                            EmployeeBankName = x.DDF_BankRoutingNumber_1

                        },
                        Account2 = new AccountModel
                        {
                            Account = x.DDF_AccountNumber_2,
                            AccountType = x.DDF_AccountType_2,
                            BankRouting = x.DDF_BankRoutingNumber_2,
                            DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
                            EmployeeBankName = x.DDF_BankRoutingNumber_2
                        },
                        EmployeeId = EmployeeId,
                        PrintedName = x.EmployeeName,


                    }).FirstOrDefault();
                    result = result == null ? new DirectDepositeFormModel() : result;
                    result.EmployeeId = EmployeeId;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DirectDepositeFormModel GetDirectDepositeDataByEmployeeId(string EmployeeId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var result = Context.spGetDirectDepositForm(EmployeeId).Select(x => new DirectDepositeFormModel
                    {
                        Account1 = new AccountModel
                        {
                            Account = x.DDF_AccountNumber_1,
                            AccountType = x.DDF_AccountType_1,
                            BankRouting = x.DDF_BankRoutingNumber_1,
                            DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
                            EmployeeBankName = x.DDF_BankRoutingNumber_1

                        },
                        Account2 = new AccountModel
                        {
                            Account = x.DDF_AccountNumber_2,
                            AccountType = x.DDF_AccountType_2,
                            BankRouting = x.DDF_BankRoutingNumber_2,
                            DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
                            EmployeeBankName = x.DDF_BankRoutingNumber_2
                        },
                        EmployeeId = x.DDF_EMP_EmployeeID,
                        PrintedName = x.EmployeeName,


                    }).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SetDirectDepositeFormData(DirectDepositeFormModel model, long UserId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;
                    var data = GetDirectDepositeDataByEmployeeId(EmployeeId);
                    if (data != null)
                        return Context.spSetDirectDepositForm("U", EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;

                    return Context.spSetDirectDepositForm("I", EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount.HasValue ? model.Account1.DepositeAmount.Value : 0, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SetEmployeeHandbookData(EmployeeHandbookModel model, long UserId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var data = GetEmployeeHandBookByUserId(UserId);
                    var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;

                    if (data != null)
                        return Context.spSetEmployeeHandbook("U", model.EhbId, EmployeeId, model.IsActive) > 0 ? true : false;

                    return Context.spSetEmployeeHandbook("I", model.EhbId, EmployeeId, model.IsActive) > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeHandbookModel GetEmployeeHandBookByUserId(long UserId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;


                    var result = Context.spGetEmployeeHandbook(EmployeeId).Select(x => new EmployeeHandbookModel
                    {
                        EmployeeName = x

                    }).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeHandbookModel GetEmployeeHandBookByEmployeeId(string EmployeeId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var result = Context.spGetEmployeeHandbook(EmployeeId).Select(x => new EmployeeHandbookModel
                    {
                        EmployeeName = x

                    }).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetEmployeeId(long userId)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var result = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetPhotoRelease(long userId)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    string name = Context.spGetPhotoReleaseForm(empid).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetPhotoRelease(long userId, PhotoRelease model)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var isexist = GetPhotoRelease(userId);
                    if (string.IsNullOrEmpty(isexist))
                    {
                        Context.spSetPhotoReleaseForm("I", model.PRFId, empid, model.IsActive);
                    }
                    else
                    {
                        Context.spSetPhotoReleaseForm("U", model.PRFId, empid, model.IsActive);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmergencyContectForm GetEmergencyForm(long userId)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var name = Context.spGetEmergencyContactForm(empid).Select(x => new EmergencyContectForm
                    {
                        NickName = x.ECF_NickName,
                        HomeEmail = x.ECF_HomeEmail,
                        HomePhone = x.ECF_HomePhone,
                        EmpId = x.ECF_EMP_EmployeeID,
                        EcfId = x.ECF_Id
                    }).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetEmergencyForm(long userId, EmergencyContectForm model)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var isexist = GetEmergencyForm(userId);
                    if (ReferenceEquals(isexist, null))
                    {
                       // Context.spSetEmergencyContactForm("I", model.EcfId, empid, model.NickName, model.HomePhone, model.HomeEmail, model.EcfDate, model.IsActive);
                    }
                    else
                    {
                       // Context.spSetEmergencyContactForm("U", model.EcfId, empid, model.NickName, model.HomePhone, model.HomeEmail, model.EcfDate, model.IsActive);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ConfidenialityAgreementModel GetConfidenialityAgreementForm(long userId)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var name = Context.spGetConfidentialityAgreement(empid).Select(x => new ConfidenialityAgreementModel
                    {
                        Email = x.EMP_Email,
                        EmpAddress = x.EmpAddress,
                        EmpId = x.CDA_EMP_EmployeeID,
                        EmployeeName = x.EmployeeName
                    }).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetConfidenialityAgreementForm(long userId, ConfidenialityAgreementModel model)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var isexist = GetConfidenialityAgreementForm(userId);
                    if (ReferenceEquals(isexist, null))
                    {
                        Context.spSetConfidentialityAgreement("I", model.CafId, empid, model.IsActive);
                    }
                    else
                    {
                        Context.spSetConfidentialityAgreement("U", model.CafId, empid, model.IsActive);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EducationVarificationModel GetEducationVerificationForm(long userId)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var name = Context.spGetEducationVerificationForm(empid).Select(x => new EducationVarificationModel
                    {

                        EmpId = empid,
                        HighSchool = new Education
                        {
                            AttendFrom = x.EVF_AttendedFrom,
                            AttendTo = x.EVF_AttendedTo,
                            City = x.EVF_City,
                            SchoolName = x.EVF_OrgnizationName,
                            State = x.EVF_State,
                            Cretificate = x.EVF_SchoolDegreeDiplomaCirtificate,
                        },
                        Name = x.EmployeeName,
                        EvfId = x.EVF_Id
                    }).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetEducationVerificationForm(long userId, EducationVarificationModel model)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var isexist = GetEducationVerificationForm(userId);
                    if (ReferenceEquals(isexist, null))
                    {
                        Context.spSetEducationVerificationForm("I", model.EvfId, empid, model.Certificate, model.HighSchool.SchoolName, "", model.HighSchool.City, model.HighSchool.State, model.HighSchool.AttendFrom, model.HighSchool.AttendTo, model.IsActive);
                    }
                    else
                    {
                        Context.spSetEducationVerificationForm("U", isexist.EvfId, empid, model.Certificate, model.HighSchool.SchoolName, "", model.HighSchool.City, model.HighSchool.State, model.HighSchool.AttendFrom, model.HighSchool.AttendTo, model.IsActive);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public W4FormModel GetW4Form(long userId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var name = Context.spGetW4Form(empid).Select(x => new W4FormModel
                    {
                        FirstName = x.EMP_FirstName,
                        MiddleName = x.EMP_MiddleName,
                        LastName = x.EMP_LastName,
                        EmpId = empid,
                        EIN = x.w4F_10,
                        SSN = x.W4F_SSN,
                        EmployeerNameAndAddress = x.w4F_8EmployersName,
                        MeritalStatus = GetMaritalStatus(x.w4F_3MaritalStatus),
                        FirstEmployeementDate = x.w4F_9,
                        AdditionalAmount = x.w4F_6,
                        NameDiffer = x.w4F_4 == "Y" ? true : false,
                        ClaimExemption = x.w4F_7,
                        TotalAllowence = x.w4F_5,
                        W4FId = x.W4F_Id
                    }).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MeritalStatus GetMaritalStatus(string w4F_3MaritalStatus)
        {
            if (string.IsNullOrEmpty(w4F_3MaritalStatus))
            {
                return new MeritalStatus();
            }
            if (w4F_3MaritalStatus == "Single")
                return new MeritalStatus { Single = true };
            if (w4F_3MaritalStatus == "Single")
                return new MeritalStatus { Single = true };
            if (w4F_3MaritalStatus == "Married")
                return new MeritalStatus { Married = true };
            return new MeritalStatus { PartiallyMarried = true };
        }


        public void SetW4Form(long userId, W4FormModel model)
        {

            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var isexist = GetW4Form(userId);
                    if (ReferenceEquals(isexist, null))
                    {
                        Context.spSetW4Form("I", model.W4FId, empid, model.SSN, GetMaritalStatusAsString(model.MeritalStatus), model.NameDiffer == true ? "Y" : "N", model.TotalAllowence, model.AdditionalAmount, model.ClaimExemption, model.EmployeerNameAndAddress, model.FirstEmployeementDate, model.EIN, model.IsActive);
                    }
                    else
                    {
                        Context.spSetW4Form("U", isexist.W4FId, empid, model.SSN, GetMaritalStatusAsString(model.MeritalStatus), model.NameDiffer == true ? "Y" : "N", model.TotalAllowence, model.AdditionalAmount, model.ClaimExemption, model.EmployeerNameAndAddress, model.FirstEmployeementDate, model.EIN, model.IsActive);

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMaritalStatusAsString(MeritalStatus meritalStatus)
        {
            if (meritalStatus.Married)
                return "Married";
            if (meritalStatus.Single)
                return "Single";
            return "PartiallyMarried";
        }
        public PersonalFileModel GetFormsStatus(long userId)
        {
            try
            {

                using (workorderEMSEntities Context = new workorderEMSEntities())
                {

                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var formsStatus = Context.spGetForrmStatus(empid).Select(x => new PersonalFileModel
                    {
                        W4Status = x.W4F_IsActive,
                        ConfidentialityAgreement = x.CDA_IsActive,
                        DirectDepositForm = x.DDF_IsActive,
                        EducationVerificationForm = x.EVF_IsActive,
                        EmergencyContactFormStatus = x.ECF_IsActive,
                        EmployeeHandbook = x.EHB_IsActive,
                        PhotoReleaseForm = x.PRF_IsActive
                    }).FirstOrDefault();
                    return formsStatus;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}