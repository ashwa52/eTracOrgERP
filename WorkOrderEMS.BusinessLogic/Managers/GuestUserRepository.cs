﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;
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

        public EmployeeVIewModel GetEmployeeDetails(long userId)
        {
            var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;
            var tt = new EmployeeVIewModel();
            try
            {
                tt = objworkorderEMSEntities.Employees.Where(x => x.EMP_EmployeeID == employeeId).
                   Select(x => new EmployeeVIewModel
                   {
                       //Address = x.EMA_Address,
                       //City = x.EMA_City,
                       //State = x.EMA_State,
                       //Cityzenship = x.CTZ_Citizenship,
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
                       //Zip = x.EMA_Zip,
                       //EMP_Gender = x.EMP_Gender.ToString(),
                       ApplicantId = x.EMP_API_ApplicantId.Value
                   }).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return tt;
        }
        public EmployeeVIewModel GetEmployee(long userId)
        {
            var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;
            var tt = new EmployeeVIewModel();
            try
            {
                tt = objworkorderEMSEntities.spGetEmployeePersonalInfo(employeeId).
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
                       EMP_Gender = x.EMP_Gender.ToString(),
                       ApplicantId = Convert.ToInt64(x.EMP_API_ApplicantId)
                   }).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }
            return tt;
        }

        public CommonApplicantModel GetApplicantAllDetails(long userId)
        {
            CommonApplicantModel commonModel = new CommonApplicantModel();
            var employee = GetEmployee(userId);
            var _commonModel = objworkorderEMSEntities.spGetApplicantAllDetails(employee.ApplicantId).FirstOrDefault();

            return commonModel;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// </summary>
        /// <param name="Applicant"></param>
        /// <returns></returns>
        public CommonApplicantModel GetApplicantAllDetailsToView(long Applicant)
        {
            CommonApplicantModel commonModel = new CommonApplicantModel();
            commonModel.ApplicantAddress = objworkorderEMSEntities.spGetApplicantAddress(Applicant).Select
                (x => new WorkOrderEMS.Models.ApplicantAddress() {
                    APA_Apartment = x.APA_Apartment,
                    APA_City  =x.APA_City,
                    APA_State = objworkorderEMSEntities.MasterStates.Where(ms => ms.StateCode == x.APA_State).FirstOrDefault().StateName
                    ,
                    APA_StreetAddress = x.APA_StreetAddress,
                    APA_YearsAddressFrom = x.APA_YearsAddressFrom,
                    APA_YearsAddressTo = x.APA_YearsAddressTo,
                    APA_Zip = x.APA_Zip
                }).ToList();
            commonModel.ApplicantPersonalInfo = objworkorderEMSEntities.spGetApplicantPersonalInfo(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantPersonalInfo() {
                      API_DesireSalaryWages = x.API_DesireSalaryWages,
                      API_DLNumber = x.API_DLNumber,
                      API_FirstName = x.API_FirstName,
                      API_LastName = x.API_LastName,
                      API_MiddleName = x.API_MidName,
                      API_SSN = x.API_SSN,
                      API_Title = x.API_Title,                     
                  }).ToList();
            commonModel.AplicantAcadmicDetails = objworkorderEMSEntities.spGetAplicantAcadmicDetails(Applicant).Select
                  (x => new WorkOrderEMS.Models.AplicantAcadmicDetails(){
                      AAD_AttendedFrom = x.AAD_AttendedFrom,
                      AAD_AttendedTo = x.AAD_AttendedTo,
                      AAD_City = x.AAD_City,
                      AAD_EducationType = x.AAD_EducationType,
                      AAD_InstituteName = x.AAD_InstituteName,
                      AAD_State = objworkorderEMSEntities.MasterStates.Where(ms => ms.StateCode == x.AAD_State).FirstOrDefault().StateName,
                      AAD_Zip = Convert.ToInt32(x.AAD_Zip),                     
                  }).ToList();
            commonModel.ApplicantAccidentRecord = objworkorderEMSEntities.spGetApplicantAccidentRecord(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantAccidentRecord()
                  {
                    AAR_AccidantDate = x.AAR_AccidantDate,
                    AAR_Discription = x.AAR_Discription,
                    AAR_NumberOfFatalities = Convert.ToInt32(x.AAR_NumberOfFatalities),
                    AAR_NumberOfInjuries = Convert.ToInt32(x.AAR_NumberOfInjuries)
                  }).ToList();
            commonModel.ApplicantAdditionalInfo = objworkorderEMSEntities.spGetApplicantAdditionalInfo(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantAdditionalInfo()
                  {
                    AAI_Age21Completed = Convert.ToChar(x.AAI_Age21Completed),
                    AAI_AnyRefOrEmployeeInELITE = Convert.ToChar(x.AAI_AnyRefOrEmployeeInELITE),
                    AAI_AvailableDate = x.AAI_AvailableDate,
                    AAI_DepartureDate = x.AAI_DepartureDate,
                    AAI_EverWorkForELITE = Convert.ToChar(x.AAI_EverWorkForELITE),
                    AAI_NameOfRefOrEmployeeInELITE = x.AAI_NameOfRefOrEmployeeInELITE,
                    AAI_PriorMilitaryService = Convert.ToChar(x.AAI_PriorMilitaryService),
                    AAI_ReasonForLeaving = x.AAI_ReasonForLeaving,
                    AAI_WorkEligibilityUS = Convert.ToChar(x.AAI_WorkEligibilityUS)
                  }).ToList();
            commonModel.ApplicantBackgroundHistory = objworkorderEMSEntities.spGetApplicantBackgroundHistory(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantBackgroundHistory()
                  {
                      ABH_Address = x.ABH_Address,
                      ABH_City = x.ABH_City,
                      ABH_CompanyName = x.ABH_CompanyName,
                      ABH_Phone = Convert.ToInt64(x.ABH_Phone),
                      ABH_ReasonForGAP = x.ABH_ReasonForGAP,
                      ABH_ReasonforLeaving = x.ABH_ReasonforLeaving,
                      ABH_SafetySensitiveFunction =Convert.ToChar(x.ABH_SafetySensitiveFunction),
                      ABH_State = objworkorderEMSEntities.MasterStates.Where(ms => ms.StateCode == x.ABH_State).FirstOrDefault().StateName,
                      
                      ABH_StillEmployed = Convert.ToChar(x.ABH_StillEmployed),
                      ABH_SubToFedralMotor = Convert.ToChar(x.ABH_SubToFedralMotor),
                      ABH_ZIPCode = Convert.ToInt32(x.ABH_ZIPCode)
                  }).ToList();
            commonModel.ApplicantContactInfo = objworkorderEMSEntities.spGetApplicantContactInfo(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantContactInfo()
                  {
                      ACI_eMail = x.ACI_eMail,
                      ACI_PhoneNo = Convert.ToInt64(x.ACI_PhoneNo),
                      ACI_PrefredContactMethod = x.ACI_PrefredContactMethod
                  }).ToList();
            commonModel.ApplicantLicenseHeald  = objworkorderEMSEntities.spGetApplicantLicenseHeald(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantLicenseHeald()
                  {
                     ALH_ExpirationDate = x.ALH_ExpirationDate,
                     ALH_IssueDate = x.ALH_IssueDate,
                     ALH_LicenceNumber = x.ALH_LicenceNumber,
                     ALH_LicenseType = x.ALH_LicenseType,
                     ALH_State = objworkorderEMSEntities.MasterStates.Where(ms => ms.StateCode == x.ALH_State).FirstOrDefault().StateName,
                      
                  }).ToList();
            commonModel.ApplicantPositionTitle = objworkorderEMSEntities.spGetApplicantPositionTitle(Applicant).Select
                  (x => new WorkOrderEMS.Models.ApplicantPositionTitle()
                  {
                      APT_FromMoYr  =x.APT_FromMoYr,
                      APT_PositionTitle = x.APT_PositionTitle,
                      APT_Salary = Convert.ToDecimal(x.APT_Salary),
                      APT_ToMoYr = x.APT_ToMoYr
                  }).ToList();
            commonModel.ApplicantSchecduleAvaliblity = objworkorderEMSEntities.spGetApplicantSchecduleAvaliblity(Applicant).Select
                 (x => new WorkOrderEMS.Models.ApplicantSchecduleAvaliblity()
                 {
                     ASA_AvaliableEndTime = Convert.ToDateTime(x.ASA_AvaliableEndTime),
                     ASA_AvaliableStartTime = Convert.ToDateTime(x.ASA_AvaliableStartTime),
                     ASA_WeekDay = x.ASA_WeekDay
                 }).ToList();
            commonModel.ApplicantTrafficConvictions = objworkorderEMSEntities.spGetApplicantTrafficConvictions(Applicant).Select
                 (x => new ApplicantTrafficConvictions()
                 {
                     ATC_AtFaultAccident = x.ATC_AtFaultAccident == "Y" ? true:false,
                     ATC_AtMovingViolation = x.ATC_AtMovingViolation == "Y" ? true : false,
                     ATC_ConvictedDate = x.ATC_ConvictedDate,
                     ATC_StateOfViolation = x.ATC_StateOfViolation,
                     ATC_Violation = x.ATC_Violation
                 }).ToList();
            commonModel.ApplicantVehiclesOperated = objworkorderEMSEntities.spGetApplicantVehiclesOperated(Applicant).Select
                 (x => new WorkOrderEMS.Models.ApplicantVehiclesOperated()
                 {
                     AVO_DenideLicensePermit = Convert.ToChar(x.AVO_DenideLicensePermit),
                     AVO_DeniedLicensePermitExplanation = x.AVO_DeniedLicensePermitExplanation,
                     AVO_SuspendRevokeLicensePermit = Convert.ToChar(x.AVO_SuspendRevokeLicensePermit),
                     AVO_SuspendRevokeLicensePermitExplanation = x.AVO_SuspendRevokeLicensePermitExplanation
                 }).ToList();
            return commonModel;
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
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount.HasValue ? model.Account1.DepositeAmount.Value : 0, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;
                    var add =  Context.spSetDirectDepositForm("I", EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount.HasValue ? model.Account1.DepositeAmount.Value : 0, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y");// > 0 ? true : false;
                    return true;
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
                        EcfId = x.ECF_Id,
                        FirstName = x.ECF_FirstName,
                        MiddleName = x.ECF_MiddleName,
                        LastName = x.ECF_LastName,
                        Address = x.ECF_Address,
                        Citizenship = x.ECF_Citizenship,
                        ConactInfo = x.ECF_EmergencyContactName,
                        DOB = x.ECF_BirthDate,
                        EcfDate = x.ECF_Date,
                        EmergencyContactName = x.ECF_EmergencyContactName,
                        Gender = x.ECF_Gender,
                        HomeAddress = x.ECF_HomeAddress,
                        License = x.ECF_DriverLicense,
                        Mobile = x.ECF_Mobile,
                        RelationShip = x.ECF_Relationship,
                        SSN = x.ECF_SSN,
                        IsActive = x.ECF_IsActive
                    }).FirstOrDefault();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Feb-2020
        /// Created For : To save and Update Emeregency contact form
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
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
                        Context.spSetEmergencyContactForm("I", model.EcfId, empid, model.NickName, model.HomePhone,model.Address, model.HomeEmail,model.FirstName,
                            model.MiddleName, model.LastName, model.Address, model.Gender, model.Citizenship, model.DOB, model.License, model.EmergencyContactName, model.Mobile
                            ,model.HomePhone, model.SSN, model.RelationShip, model.IsActive);
                    }
                    else
                    {
                        Context.spSetEmergencyContactForm("U", model.EcfId, empid, model.NickName, model.HomePhone, model.Address, model.HomeEmail, model.FirstName,
                            model.MiddleName, model.LastName, model.Address, model.Gender, model.Citizenship, model.DOB, model.License, model.EmergencyContactName, model.Mobile
                            , model.HomePhone, model.SSN, model.RelationShip, model.IsActive);
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
                        EmployeeName = x.EmployeeName,
                        //CafId = x.
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

        public bool SetEmergencyContactFormData(EmergencyContactFormModel model, long UserId)
        {
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;
                    var data = GetDirectDepositeDataByEmployeeId(EmployeeId);
                    if (data != null)
                        //Context.spSetEmergencyContactForm()
                    return Context.spSetEmergencyContactForm("U", model.ECF_Id, EmployeeId, model.ECF_NickName, model.ECF_HomePhone,
                            model.ECF_HomeAddress, model.ECF_HomeEmail, model.ECF_FirstName, model.ECF_MiddleName, model.ECF_LastName,
                            model.ECF_Address, model.ECF_Gender, model.ECF_Citizenship, model.ECF_BirthDate, model.ECF_DriverLicense,
                            model.ECF_EmergencyContactName, model.ECF_Mobile, model.ECF_PhoneNumber, model.ECF_SSN, model.ECF_Relationship,"Y") > 0 ? true : false;

                    return Context.spSetEmergencyContactForm("I", model.ECF_Id, EmployeeId, model.ECF_NickName, model.ECF_HomePhone,
                             model.ECF_HomeAddress, model.ECF_HomeEmail, model.ECF_FirstName, model.ECF_MiddleName, model.ECF_LastName,
                             model.ECF_Address, model.ECF_Gender, model.ECF_Citizenship, model.ECF_BirthDate, model.ECF_DriverLicense,
                             model.ECF_EmergencyContactName, model.ECF_Mobile, model.ECF_PhoneNumber, model.ECF_SSN, model.ECF_Relationship, "Y") > 0 ? true : false;
                    //return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


}