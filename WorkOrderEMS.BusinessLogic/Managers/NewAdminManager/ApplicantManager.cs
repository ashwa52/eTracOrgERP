using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.BusinessLogic
{
    public class ApplicantManager : IApplicantManager
    {
        workorderEMSEntities _db = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Dec-2019
        /// Created For : To validate applicant using Username and password.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<eTracLoginModel> ValidateApplicant(eTracLoginModel obj)
        {
            var loginModel = new eTracLoginModel();
            var loginData = new ServiceResponseModel<eTracLoginModel>();
            try
            {
                if (obj.LoginId != null && obj.Password != null)
                {
                    var password = Cryptography.GetEncryptedData(obj.Password, true);
                    loginModel = _db.ApplicantLoginAccesses.Join(_db.Applicants,ala => ala.ALA_UserId,ap => ap.APT_ALA_UserId,(ala,ap) => new { ala, ap }).Where(x => x.ala.ALA_LoginId == obj.LoginId && x.ala.ALA_Password == password).Select(a => new eTracLoginModel()
                    {
                        FName = a.ala.ALA_FirstName,
                        LName = a.ala.ALA_LastName,
                        Email = a.ala.ALA_eMailId,
                        MName = a.ala.ALA_MidName,
                        LoginId = a.ala.ALA_LoginId,
                        UserId = a.ala.ALA_UserId,
                        ApplicantId = a.ap.APT_ApplicantId
                    }).FirstOrDefault();
                    if (loginModel != null)
                    {
                        loginData.Data = loginModel;
                        loginData.Message = CommonMessage.Successful();
                        loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        loginData.Data = loginModel;
                        loginData.Message = CommonMessage.NoRecordMessage();
                        loginData.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<eTracLoginModel> ValidateApplicant(eTracLoginModel obj)", "Exception While validate applicant.", obj);
                throw;
            }
            return loginData;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-Dec-2019
        /// Created For : To forgot password
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<eTracLoginModel> ForgotPassword(eTracLoginModel obj)
        {
            var loginData = new ServiceResponseModel<eTracLoginModel>();
            try
            {
                if (obj.Email != null)
                {
                    //var password = Cryptography.GetEncryptedData(obj.NewPassword, true);
                    //var isSaved = _db.spSetApplicantForgotPassword(obj.LoginId, password);
                    //if(isSaved > 0)
                    //{
                    #region Email
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    if (obj.NewPassword != null)
                    {
                        var password = Cryptography.GetEncryptedData(obj.NewPassword, true);
                        //var isSaved = _db.spSetApplicantForgotPassword(obj.LoginId, password);
                    }
                    var userData = _db.ApplicantLoginAccesses.Where(x => x.ALA_eMailId == obj.Email).FirstOrDefault();
                    if (userData != null)
                    {
                        bool IsSent = false;
                        var objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = userData.ALA_eMailId;
                        objEmailHelper.RegistrationLink = ConfigurationManager.AppSettings["hostingprefix"];
                        objEmailHelper.Name = userData.ALA_FirstName + " " + userData.ALA_LastName;
                        objEmailHelper.NewPassword = obj.NewPassword;
                        objEmailHelper.MailType = "FORGOTPASSWORDAPPLICANT";
                        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                        IsSent = objEmailHelper.SendEmailWithTemplate();
                        if (IsSent == true)
                        {
                            var objEmailog = new EmailLog();
                            try
                            {
                                objEmailog.CreatedBy = userData.ALA_UserId;
                                objEmailog.CreatedDate = DateTime.UtcNow;
                                objEmailog.DeletedBy = null;
                                objEmailog.DeletedOn = null;
                                //objEmailog.LocationId = location;
                                objEmailog.ModifiedBy = null;
                                objEmailog.ModifiedOn = null;
                                objEmailog.SentBy = userData.ALA_UserId;
                                objEmailog.SentEmail = userData.ALA_eMailId;
                                objEmailog.Subject = objEmailHelper.Subject;
                                //objEmailog.SentTo = userData.E;
                                objListEmailog.Add(objEmailog);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges();
                        }
                    }
                    #endregion Email
                    loginData.Message = CommonMessage.ForgotMailSend();
                    loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    //}
                    //else
                    //{
                    //    loginData.Data = null;
                    //    loginData.Message = CommonMessage.NoRecordMessage();
                    //    loginData.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    //}

                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<eTracLoginModel> ForgotPassword(eTracLoginModel obj)", "Exception While forgot password.", obj);
                throw;
            }
            return loginData;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To set forgot password
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<eTracLoginModel> SetForgotPassword(eTracLoginModel obj)
        {
            var loginData = new ServiceResponseModel<eTracLoginModel>();
            try
            {
                if (obj.NewPassword != null && obj.LoginId != null)
                {
                    var password = Cryptography.GetEncryptedData(obj.NewPassword, true);
                    var isSaved = _db.spSetApplicantForgotPassword(obj.LoginId, password);
                    if (isSaved > 0)
                    {
                        loginData.Message = CommonMessage.PasswordSaved();
                        loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        loginData.Data = null;
                        loginData.Message = CommonMessage.NoRecordMessage();
                        loginData.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<eTracLoginModel> SetForgotPassword(eTracLoginModel obj)", "Exception While set a password.", obj);
                throw;
            }
            return loginData;
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 28-Dec-2019
        /// Created For : To change password of Applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> ChangePassword(eTracLoginModel obj)
        {
            var loginModel = new eTracLoginModel();
            var loginData = new ServiceResponseModel<string>();
            try
            {
                if (obj.LoginId != null && obj.NewPassword != null && obj.Password != null)
                {
                    var password = Cryptography.GetEncryptedData(obj.Password, true);
                    var newPassword = Cryptography.GetEncryptedData(obj.NewPassword, true);
                    var isChanged = _db.spSetApplicantChangePassword(obj.LoginId, password, newPassword);
                    if (isChanged > 0)
                    {
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        var userData = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == obj.LoginId).FirstOrDefault();
                        if (userData != null)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = userData.ALA_eMailId;
                            objEmailHelper.LoginId = userData.ALA_LoginId;
                            objEmailHelper.NewPassword = obj.NewPassword;
                            objEmailHelper.AcceptAssessmentLink = ConfigurationManager.AppSettings["hostingprefix"];
                            objEmailHelper.Name = userData.ALA_FirstName + " " + userData.ALA_LastName;
                            objEmailHelper.MailType = "CHANGEPASSWORDAPPLICANT";
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = userData.ALA_UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = userData.ALA_UserId;
                                    objEmailog.SentEmail = userData.ALA_eMailId;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.EmailLogs.AddRange(objListEmailog);
                                context.SaveChanges();
                            }
                        }
                        #endregion Email
                        loginData.Message = CommonMessage.PasswordChanged();
                        loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        loginData.Data = null;
                        loginData.Message = CommonMessage.PasswordChangedError();
                        loginData.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> ChangePassword(eTracLoginModel obj)", "Exception While change password.", obj);
                throw;
            }
            return loginData;
        }

        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 28-Dec-2019
        /// Created For : To sign Up Applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> SignUpApplicant(eTracLoginModel obj)
        {
            var loginModel = new eTracLoginModel();
            var loginData = new ServiceResponseModel<string>();
            var _applicant = new Applicant();
            var _common = new Common_B();
            try
            {
                if (obj.FName != null && obj.Email != null)
                {
                    var password = Cryptography.GetEncryptedData(obj.Password, true);
                    var ApplicantId = new ObjectParameter("APT_ApplicantId", typeof(int));
                    var isChanged = _db.spSetApplicantCreateLoginAccess(obj.Email, password, obj.FName, obj.MName, obj.LName, obj.Email, obj.Question, obj.Answer,obj.ApplicantPhoto).FirstOrDefault();
                    var saveApply = _db.spSetApplicantJobApply("I", isChanged, obj.JobPostingId,obj.DateOFJoining, "Y", ApplicantId);
                    var getApplicant = _db.Applicants.Where(x => x.APT_ALA_UserId == isChanged).FirstOrDefault();
                    //_common.SaveApplicantStatus(getApplicant.APT_ApplicantId, ApplicantStatus.A, "Y");
                    if (isChanged > 0)
                    {
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        var userData = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == obj.LoginId).FirstOrDefault();
                        if (userData != null)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = userData.ALA_eMailId;
                            objEmailHelper.LoginId = userData.ALA_LoginId;
                            objEmailHelper.RegistrationLink = ConfigurationManager.AppSettings["hostingprefix"];
                            objEmailHelper.Name = userData.ALA_FirstName + " " + userData.ALA_LastName;
                            objEmailHelper.MailType = "SIGNUPAPPLICANT";
                            objEmailHelper.Password = obj.Password;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = userData.ALA_UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = userData.ALA_UserId;
                                    objEmailog.SentEmail = userData.ALA_eMailId;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.EmailLogs.AddRange(objListEmailog);
                                context.SaveChanges();
                            }
                        }
                        #endregion Email
                        loginData.Message = CommonMessage.SignUp();
                        loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        loginData.Data = null;
                        loginData.Message = CommonMessage.SignUpError();
                        loginData.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> SignUpApplicant(eTracLoginModel obj)", "Exception While signup applicant.", obj);
                throw;
            }
            return loginData;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To check for Login Id is already exist or not
        /// Created Date : 28-Dec-2019
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> CheckLoginId(eTracLoginModel obj)
        {
            var loginModel = new eTracLoginModel();
            var loginData = new ServiceResponseModel<string>();
            try
            {
                if (obj.LoginId != null)
                {
                    var Availability = new ObjectParameter("Availability", "");
                    var isChanged = _db.spGetApplicantNewLoginCheckAvailability(obj.LoginId, Availability);
                    if (Availability.Value.ToString() == "False")
                    {
                        loginData.Message = CommonMessage.AlreadyExist();
                        loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        loginData.Data = null;
                        loginData.Message = CommonMessage.NotExist();
                        loginData.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    loginData.Data = null;
                    loginData.Message = CommonMessage.WrongParameterMessage();
                    loginData.Response = Convert.ToInt32(ServiceResponse.FailedResponse, CultureInfo.CurrentCulture);
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<string> CheckLoginId(eTracLoginModel obj)", "Exception While checking login id.", obj);
                throw;
            }
            return loginData;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To save assets
        /// Created Date : 20-Dec-2019
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveAssets(AssetsAllocationModel model)
        {
            bool isSaved = false;

            try
            {

                if (model != null && model.AssetsId == 0)
                {
                    //var computer = model.IsComputerAssets == true ? "Computer" : null;
                    //var CellPhoneAssets = model.IsCellPhoneAssets == true ? "Cell Phone" : null;
                    //var MiscAssets = model.IsMiscAssets == true ? "Misc" : null;
                    //var OfficePhone = model.IsOfficePhone == true ? "Office Phone" : null;
                    //var Printer = model.IsPrinterAssets == true ? "Printer" : null;
                    if (model.IsComputerAssets == true && model.ComputerAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Computer", model.ComputerAssets.AssetsName, model.ComputerAssets.AssetDescription, model.ComputerAssets.Make, model.ComputerAssets.Model,
                            model.ComputerAssets.SerialNumber, model.ComputerAssets.Login, model.ComputerAssets.Password, null, null, null, null, "N");
                    }
                    if (model.IsPrinterAssets == true && model.PrinterAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Printer", model.PrinterAssets.AssetsName,
                            model.PrinterAssets.AssetDescription, model.PrinterAssets.Make, model.PrinterAssets.Model, model.PrinterAssets.SerialNumber
                            , null, null, null, null, null, null, "N");
                    }
                    if (model.IsCellPhoneAssets == true && model.CellPhoneAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Cell Phone", model.CellPhoneAssets.AssetsName,
                            model.CellPhoneAssets.AssetDescription, model.CellPhoneAssets.Make, model.CellPhoneAssets.Model, model.CellPhoneAssets.SerialNumber,
                            null, null, null, null, null, null, "N");
                    }
                     if (model.IsOfficePhone == true && model.OfficePhone != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Office Phone", model.OfficePhone.AssetsName, model.OfficePhone.AssetDescription,
                            model.OfficePhone.Make, model.OfficePhone.Model, model.OfficePhone.SerialNumber, null, null, null, null,
                            null, null, "N");
                    }
                    if(model.IsMiscAssets == true && model.MiscAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Misc Assets", model.MiscAssets.AssetsName, model.MiscAssets.AssetDescription,
                            model.MiscAssets.Make, model.MiscAssets.Model, model.MiscAssets.SerialNumber, null, null, null, null, null, null, "N");
                    }

                    isSaved = true;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveAssets(AssetsAllocationModel model)", "Exception While Saving File.", model);
                throw;
            }
            return isSaved;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To send offer letter to applicant
        /// created Date : 24-Dec-2019
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendOffer(OfferModel model)
        {
            bool isAccept = false;
            try
            {
                if (model != null)
                {
                    var getData = _db.spSetApplicantStatus(model.ApplicantId, ApplicantStatus.Offer, ApplicantIsActiveStatus.Sent);
                    var getApplicant = _db.Applicants.Where(x => x.APT_ApplicantId == model.ApplicantId).FirstOrDefault();
                    //_common.SaveApplicantStatus(getApplicant.APT_ApplicantId, ApplicantStatus.A, "Y");
                    #region Email
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    var userData = _db.ApplicantLoginAccesses.Join(_db.Applicants, ala => ala.ALA_UserId, ap => ap.APT_ALA_UserId, (ala, ap) => new { ala, ap }).Where(x => x.ap.APT_ApplicantId == model.ApplicantId).Select(a => new eTracLoginModel()
                    {
                        FName = a.ala.ALA_FirstName,
                        LName = a.ala.ALA_LastName,
                        Email = a.ala.ALA_eMailId,
                        MName = a.ala.ALA_MidName,
                        LoginId = a.ala.ALA_LoginId,
                        UserId = a.ala.ALA_UserId,
                        ApplicantId = a.ap.APT_ApplicantId
                    }).FirstOrDefault();
                    var getEMPData = _db.Employees.Where(x => x.EMP_Id == model.ApplicantId).FirstOrDefault();
                    //var userData = _db.spGetEmployeePersonalInfo(getEMPData.EMP_EmployeeID).FirstOrDefault();
                    if (userData != null)
                    {
                        bool IsSent = false;
                        var objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = userData.Email;
                        objEmailHelper.RegistrationLink = ConfigurationManager.AppSettings["hostingprefix"] + "/GetMailData/GetApplicantInfo?ApplicantId=" + model.ApplicantId.ToString();
                        objEmailHelper.Name = userData.FName + " " + userData.LName;
                        objEmailHelper.MailType = "SENDOFFER";
                        objEmailHelper.SentBy = model.UserId;
                        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                        IsSent = objEmailHelper.SendEmailWithTemplate();
                        if (IsSent == true)
                        {
                            var objEmailog = new EmailLog();
                            try
                            {
                                objEmailog.CreatedBy = model.UserId;
                                objEmailog.CreatedDate = DateTime.UtcNow;
                                objEmailog.DeletedBy = null;
                                objEmailog.DeletedOn = null;
                                objEmailog.ModifiedBy = null;
                                objEmailog.ModifiedOn = null;
                                objEmailog.SentBy = model.UserId;
                                objEmailog.SentEmail = userData.Email;
                                objEmailog.Subject = objEmailHelper.Subject;
                                objListEmailog.Add(objEmailog);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges();
                        }
                    }
                    #endregion Email
                    isAccept = true;
                }
                else
                {
                    isAccept = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendOffer(OfferModel model)", "Exception While sending a offer to applicant.", model);
                throw;
            }
            return isAccept;
        }

        #region Save Applicant
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Jan-2020
        /// Created For -  To save applicant using data table
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveApplicantData(CommonApplicantModel Obj)
        {
            bool Flag = false;
            try
            {
                if (Obj.ApplicantPersonalInfo != null)
                {
                    Obj.ApplicantPersonalInfo[0].API_Action = 'I';
                    Obj.ApplicantAddress[0].APA_Action = 'I';
                    Obj.ApplicantContactInfo[0].ACI_Action = 'I';
                    Obj.ApplicantAdditionalInfo[0].AAI_Action = 'I';
                    Obj.AplicantAcadmicDetails[0].AAD_Action = 'I';
                    Obj.ApplicantBackgroundHistory[0].ABH_Action = 'I';
                    Obj.ApplicantPositionTitle[0].APT_Action = 'I';
                    Obj.ApplicantAccidentRecord[0].AAR_Action = 'I';
                    Obj.ApplicantTrafficConvictions[0].ATC_Action = 'I';
                    Obj.ApplicantVehiclesOperated[0].AVO_Action = 'I';
                    Obj.ApplicantLicenseHeald[0].ALH_Action = 'I';
                    Obj.ApplicantSchecduleAvaliblity[0].ASA_Action = 'I';

                }
                List<ATC> ListATC = new List<ATC>();

                for(int i=0; i<Obj.ApplicantTrafficConvictions.Count;i++)
                { 
                ATC atc = new ATC();
                    atc.ATC_Action = Obj.ApplicantTrafficConvictions[i].ATC_Action;
                    atc.ATC_APT_ApplicantId = Obj.ApplicantTrafficConvictions[i].ATC_APT_ApplicantId;
                    atc.ATC_AtFaultAccident = Obj.ApplicantTrafficConvictions[i].ATC_AtFaultAccident==true?'Y':'N';
                    atc.ATC_AtMovingViolation = Obj.ApplicantTrafficConvictions[i].ATC_AtMovingViolation == true ? 'Y' : 'N';
                    atc.ATC_ConvictedDate = Obj.ApplicantTrafficConvictions[i].ATC_ConvictedDate;
                    atc.ATC_Id = Obj.ApplicantTrafficConvictions[i].ATC_Id;
                    atc.ATC_IsActive = Obj.ApplicantTrafficConvictions[i].ATC_IsActive;
                    atc.ATC_StateOfViolation = Obj.ApplicantTrafficConvictions[i].ATC_StateOfViolation;
                    atc.ATC_Violation = Obj.ApplicantTrafficConvictions[i].ATC_Violation;
                    ListATC.Add(atc);

                }

                //// save using procedure with table value parameter 
                using (var context = new workorderEMSEntities())
                {
                    System.Data.DataTable ApplicantPersonalInfoTable = Obj.ApplicantPersonalInfo.ToDataTable();
                    System.Data.DataTable AplicantAcadmicDetailsTable = Obj.AplicantAcadmicDetails.ToDataTable();
                    System.Data.DataTable ApplicantAddressTable = Obj.ApplicantAddress.ToDataTable();
                    System.Data.DataTable ApplicantContactInfoTable = Obj.ApplicantContactInfo.ToDataTable();
                    System.Data.DataTable ApplicantAdditionalInfoTable = Obj.ApplicantAdditionalInfo.ToDataTable();
                    System.Data.DataTable ApplicantBackgroundHistoryTable = Obj.ApplicantBackgroundHistory.ToDataTable();
                    System.Data.DataTable ApplicantPositionTitleTable = Obj.ApplicantPositionTitle.ToDataTable();
                    System.Data.DataTable ApplicantAccidentRecordTable = Obj.ApplicantAccidentRecord.ToDataTable();
                    System.Data.DataTable ApplicantTrafficConvictionsTable = ListATC.ToDataTable();
                    System.Data.DataTable ApplicantVehiclesOperatedTable = Obj.ApplicantVehiclesOperated.ToDataTable();
                    System.Data.DataTable ApplicantLicenseHealdTable = Obj.ApplicantLicenseHeald.ToDataTable();
                    //Obj.ApplicantSchecduleAvaliblity = new List<ApplicantSchecduleAvaliblity>();
                    System.Data.DataTable ApplicantSchecduleAvaliblityTable = Obj.ApplicantSchecduleAvaliblity.ToDataTable();

                    //// convert source data to DataTable 


                    var Action = new SqlParameter("@Action", SqlDbType.Char);
                    Action.Value = "I";
                    //var ACB_BillAmount = new SqlParameter("@ACB_BillAmount", SqlDbType.Decimal);
                    //ACB_BillAmount.Value = Obj.ApplicantId;
                    var UT_ApplicantPersonalInfo = new SqlParameter("@UT_ApplicantPersonalInfo", SqlDbType.Structured);
                    var UT_ApplicantAddress = new SqlParameter("@UT_ApplicantAddress", SqlDbType.Structured);
                    var UT_ApplicantContactInfo = new SqlParameter("@UT_ApplicantContactInfo", SqlDbType.Structured);
                    var UT_ApplicantAdditionalInfo = new SqlParameter("@UT_ApplicantAdditionalInfo", SqlDbType.Structured);
                    var UT_AplicantAcadmicDetails = new SqlParameter("@UT_AplicantAcadmicDetails", SqlDbType.Structured);
                    var UT_ApplicantBackgroundHistory = new SqlParameter("@UT_ApplicantBackgroundHistory", SqlDbType.Structured);
                    var UT_ApplicantPositionTitle = new SqlParameter("@UT_ApplicantPositionTitle", SqlDbType.Structured);
                    var UT_ApplicantAccidentRecord = new SqlParameter("@UT_ApplicantAccidentRecord", SqlDbType.Structured);
                    var UT_ApplicantTrafficConvictions = new SqlParameter("@UT_ApplicantTrafficConvictions", SqlDbType.Structured);
                    var UT_ApplicantVehiclesOperated = new SqlParameter("@UT_ApplicantVehiclesOperated", SqlDbType.Structured);
                    var UT_ApplicantLicenseHeald = new SqlParameter("@UT_ApplicantLicenseHeald", SqlDbType.Structured);
                    var UT_ApplicantSchecduleAvaliblity = new SqlParameter("@UT_ApplicantSchecduleAvaliblity", SqlDbType.Structured);
                    // info for ApplicantPersonalInfoTable
                    UT_ApplicantPersonalInfo.Value = ApplicantPersonalInfoTable;
                    UT_ApplicantPersonalInfo.TypeName = "[dbo].[UT_ApplicantPersonalInfo]";

                    //info for AplicantAcadmicDetailsTable
                    UT_AplicantAcadmicDetails.Value = AplicantAcadmicDetailsTable;
                    UT_AplicantAcadmicDetails.TypeName = "[dbo].[UT_AplicantAcadmicDetails]";

                    //info for ApplicantAddressTable
                    UT_ApplicantAddress.Value = ApplicantAddressTable;
                    UT_ApplicantAddress.TypeName = "[dbo].[UT_ApplicantAddress]";

                    //info for ApplicantContactInfoTable
                    UT_ApplicantContactInfo.Value = ApplicantContactInfoTable;
                    UT_ApplicantContactInfo.TypeName = "[dbo].[UT_ApplicantContactInfo]";

                    //info for ApplicantAdditionalInfoTable
                    UT_ApplicantAdditionalInfo.Value = ApplicantAdditionalInfoTable;
                    UT_ApplicantAdditionalInfo.TypeName = "[dbo].[UT_ApplicantAdditionalInfo]";

                    //info for ApplicantBackgroundHistoryTable
                    UT_ApplicantBackgroundHistory.Value = ApplicantBackgroundHistoryTable;
                    UT_ApplicantBackgroundHistory.TypeName = "[dbo].[UT_ApplicantBackgroundHistory]";

                    //info for ApplicantPositionTitleTable
                    UT_ApplicantPositionTitle.Value = ApplicantPositionTitleTable;
                    UT_ApplicantPositionTitle.TypeName = "[dbo].[UT_ApplicantPositionTitle]";

                    //info for ApplicantAccidentRecordTable
                    UT_ApplicantAccidentRecord.Value = ApplicantAccidentRecordTable;
                    UT_ApplicantAccidentRecord.TypeName = "[dbo].[UT_ApplicantAccidentRecord]";

                    //info for ApplicantTrafficConvictionsTable
                    UT_ApplicantTrafficConvictions.Value = ApplicantTrafficConvictionsTable;
                    UT_ApplicantTrafficConvictions.TypeName = "[dbo].[UT_ApplicantTrafficConvictions]";

                    //info for ApplicantVehiclesOperatedTable
                    UT_ApplicantVehiclesOperated.Value = ApplicantVehiclesOperatedTable;
                    UT_ApplicantVehiclesOperated.TypeName = "[dbo].[UT_ApplicantVehiclesOperated]";

                    //info for ApplicantLicenseHealdTable
                    UT_ApplicantLicenseHeald.Value = ApplicantLicenseHealdTable;
                    UT_ApplicantLicenseHeald.TypeName = "[dbo].[UT_ApplicantLicenseHeald]";

                   // info for ApplicantLicenseHealdTable
                    UT_ApplicantSchecduleAvaliblity.Value = ApplicantSchecduleAvaliblityTable;
                    UT_ApplicantSchecduleAvaliblity.TypeName = "[dbo].[UT_ApplicantSchecduleAvaliblity]";

                    context.Database.ExecuteSqlCommand("exec [dbo].[spSetApplicantAllDetails] @Action, @UT_ApplicantPersonalInfo,@UT_ApplicantAddress,@UT_ApplicantContactInfo, @UT_ApplicantAdditionalInfo, @UT_AplicantAcadmicDetails, @UT_ApplicantBackgroundHistory, @UT_ApplicantPositionTitle, @UT_ApplicantAccidentRecord, @UT_ApplicantTrafficConvictions, @UT_ApplicantVehiclesOperated, @UT_ApplicantLicenseHeald, @UT_ApplicantSchecduleAvaliblity",
                        Action, UT_ApplicantPersonalInfo,
                        UT_ApplicantAddress, UT_ApplicantContactInfo, UT_ApplicantAdditionalInfo,
                        UT_AplicantAcadmicDetails, UT_ApplicantBackgroundHistory,
                        UT_ApplicantPositionTitle, UT_ApplicantAccidentRecord,
                        UT_ApplicantTrafficConvictions, UT_ApplicantVehiclesOperated,
                        UT_ApplicantLicenseHeald, UT_ApplicantSchecduleAvaliblity
                        );
                    Flag = true;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveApplicantData(CommonApplicantModel Obj)", "Exception while saving applicant details", Obj);
                throw;
            }
            return Flag;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Fb-2020
        /// Created For : TO update Contact Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateContactDetailsApplicant(ContactListModel model, List<ContactModel> lstModel)
        {
            bool isSaved = false;
            try
            {
                foreach (var item in lstModel)
                {
                    if (item.ContactId > 0)
                    {
                        var getDetails = _db.ApplicantContactInfoes.Where(x => x.ACI_Id == item.ContactId).FirstOrDefault();
                        var updateData = _db.spSetApplicantContactInfo("U", getDetails.ACI_Id, getDetails.ACI_APT_ApplicantId, getDetails.ACI_PhoneNo, getDetails.ACI_eMail, getDetails.ACI_PrefredContactMethod, "C");
                    }
                }
                //var getDetails = _db.ApplicantContactInfoes.Where(x => x.ACI_Id == item.ContactId).FirstOrDefault();
                if (model != null && model.ContactModelData != null && model.ContactModelData.ContactNo != null)
                {
                    var update = _db.spSetApplicantContactInfo("I", null, model.ContactModelData.ACI_APT_ApplicantId, model.ContactModelData.ContactNo, model.ContactModelData.EmailId, "Mobile", "C");
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool UpdateContactDetailsApplicant(ContactListModel model)", "Exception While updating contact details.", model);
                throw;
            }
            return isSaved;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb 2020
        /// Creatd For : To get list of applicnt contact by applicant id
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public ContactListModel GetContactListByApplicantId(long ApplicantId)
        {
            var lst = new ContactListModel();
            var data = new ContactModel();
            try
            {
                var getDetails = _db.spGetApplicantContactInfo(ApplicantId).Select(x => new ContactModel() {
                    ContactId = x.ACI_Id,
                    ContactNo = x.ACI_PhoneNo,
                    EmailId = x.ACI_eMail,
                    IsChecked = x.ACI_IsActive,
                    ACI_APT_ApplicantId = x.ACI_APT_ApplicantId
                }).ToList();
                lst.ContactModel = getDetails;
                data.ACI_APT_ApplicantId = ApplicantId;
                lst.ContactModelData = data;
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContactListModel GetContactListByApplicantId(long ApplicantId)", "Exception While getting the list of contact details.", ApplicantId);
                throw;
            }
            //return isSaved;
        }

        /// <summary>
        /// Created by: Ashwajit Bansod
        /// Created Date : 01-04-2020
        /// Created For : To save certificate of applicant with details
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveAcadmicertificate(AcadmicCertification obj)
        {
            bool isSaved = false;
            try
            {
                if (obj != null && obj.ACD_APT_ApplicantId > 0)
                {
                    using (workorderEMSEntities context = new workorderEMSEntities()) {
                        var save = context.spSetAplicantCertificationDetails("I", null, obj.ACD_APT_ApplicantId, obj.ACD_CertificationName,
                                        obj.ACD_CertificationEarnedYear, obj.ACD_CertifyingAgency, obj.ACD_CertificateAttached,
                                        obj.ACD_Address, obj.ACD_City, obj.ACD_State, obj.ACD_Zip, "Y");
                    }
                        isSaved = true;               
                }
                else
                    isSaved = false;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveAcadmicertificate(AcadmicCertification obj)", "Exception While saving applicant ertification.", obj);
                throw;
            }
            return isSaved;
        }

        #endregion Save Applicant
        #region Background Check
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created For : To get applicant details for background check
        /// Created Date : 20-Feb-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public BackgroundCheckForm GetApplicantByApplicantId(long ApplicantId)
        {
            var lst = new BackgroundCheckForm();
            try
            {
                var getApplicantDetails = _db.spGetApplicantPersonalInfo(ApplicantId).Select(x =>
                new Models.ApplicantPersonalInfo() {
                    API_APT_ApplicantId = x.API_APT_ApplicantId,
                    API_DesireSalaryWages = x.API_DesireSalaryWages,
                    API_DLNumber = x.API_DLNumber,
                    API_FirstName = x.API_FirstName,
                    API_Id  = x.API_Id,
                    API_IsActive = x.API_IsActive,
                    API_LastName = x.API_LastName,
                   API_MiddleName = x.API_MidName,
                   API_SSN = x.API_SSN
                }).FirstOrDefault();
                if (getApplicantDetails != null)
                {
                    lst.ApplicantPersonalInfo = getApplicantDetails;
                }
                var getAddressInfo = _db.spGetApplicantAddress(ApplicantId).Select(x =>
                new Models.ApplicantAddress()
                {
                    APA_Apartment = x.APA_Apartment,
                    APA_APT_ApplicantId = x.APA_APT_ApplicantId,
                    APA_City = x.APA_City,
                    APA_Id = x.APA_Id,
                    APA_IsActive = x.APA_IsActive,
                    APA_State  = x.APA_State,
                    APA_StreetAddress = x.APA_StreetAddress,
                    APA_YearsAddressFrom = x.APA_YearsAddressFrom,
                    APA_YearsAddressTo = x.APA_YearsAddressTo,
                    APA_Zip = x.APA_Zip
                }).ToList();
                if(getAddressInfo != null)
                {
                    lst.ApplicantAddress = getAddressInfo;
                }
                //var getDetails = _db.spGetApplicantAllDetails(ApplicantId).Select(x => new EmployeeVIewModel()
                //{
                //    FirstName = x.API_FirstName,
                //    MiddleName = x.API_MidName,
                //    LastName = x.API_LastName,
                //    City = x.AAD_City,
                //    State = x.AAD_State,
                //    Zip = x.AAD_Zip,
                //    StreetAddress = x.APA_StreetAddress,
                //    SocialSecurityNumber = x.API_SSN,
                //    Phone = x.ACI_PhoneNo,
                //    LicenseNumber = x.ALH_LicenceNumber,
                //    //Dob = x
                //}).FirstOrDefault();
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BackgroundCheckForm GetApplicantByApplicantId(long ApplicantId)", "Exception While getting the details of applicant for background check.", ApplicantId);
                throw;
            }
            //return getDetails;
            //return isSaved;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Feb-2020
        /// Created For : To send applicant details for background check and create a normal PO 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendApplicantInfoForBackgrounddCheck(BackgroundCheckForm model)
        {
            bool isSentForBackground = false;
            try
            {
                if (model != null && model.ApplicantPersonalInfo.API_APT_ApplicantId > 0)
                {
                    var sentForBackgroudCheck = _db.spSetApplicantStatus(model.ApplicantPersonalInfo.API_APT_ApplicantId, ApplicantStatus.BackgroundCheck, ApplicantIsActiveStatus.Sent);
                    #region Create PO
                    var poNum = _db.spGetPONumber().FirstOrDefault();
                    var poNumber = "PO" + poNum.ToString();
                    var POType = Convert.ToInt64(Helper.POType.NormalPO);
                    var company = _db.Companies.Where(x => x.CMP_NameLegal == VendorName.BackgroundCheck).FirstOrDefault();
                    var getHRDetail = _db.JobPostings.Join(_db.Applicants, jp => jp.JPS_JobPostingId, ap => ap.APT_JobPostingId, (jp, ap) => new { jp, ap }).
                                      Where(x => x.ap.APT_ApplicantId == model.ApplicantPersonalInfo.API_APT_ApplicantId).FirstOrDefault();
                    var saveNormalPO = _db.spSetPODetail("I", poNum, getHRDetail.jp.JPS_LocationId,
                                                                         POType, company.CMP_Id, null,
                                                                          null, null, null, model.UserId, model.UserId, "Y", null
                                                                          , null, null, company.CompanyQBKs.FirstOrDefault().QBK_RefId, null);

                    if (getHRDetail != null)
                    {
                        var userData = _db.UserRegistrations.Where(x => x.EmployeeID == getHRDetail.jp.JPS_HiringManagerID
                                                                        && x.IsDeleted == false).FirstOrDefault();

                        isSentForBackground = true;
                        #endregion End PO
                        #region Background Screening
                        var client = new CommonHTTPClient();
                        var getSerialize = ConvertBackgroundDataJsonSerializer(model, userData);                                              
                        var saveBackgroundScreening = client.SendDataForBackGroundScreening(APIName.BackGroudScreeningPostLink, getSerialize);
                        #endregion Background Screening
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        if (userData != null)
                        {
                            var locationData = _db.LocationMasters.Where(x => x.LocationId == getHRDetail.jp.JPS_LocationId && x.IsDeleted == false).FirstOrDefault();
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = userData.UserEmail;
                            objEmailHelper.ManagerName = userData.FirstName + " "+ userData.LastName;
                            objEmailHelper.LocationName = locationData.LocationName;
                            objEmailHelper.UserName = model.ApplicantPersonalInfo.API_FirstName + " " + model.ApplicantPersonalInfo.API_LastName;
                            objEmailHelper.PONumber = poNumber;
                            //objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "POAPPROVEDREJECT";
                            objEmailHelper.SentBy = userData.UserId;
                            objEmailHelper.LocationID = getHRDetail.jp.JPS_LocationId;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
                            //Will use this when we need this
                            //var objNotify = new NotificationDetailModel();
                            //var _ICommonMethod = new CommonMethodManager();
                            //objNotify.CreatedBy = objPOTypeDataModel.UserId;
                            //objNotify.CreatedDate = DateTime.UtcNow;
                            //objNotify.AssignTo = getRuleData.UserId;
                            //objNotify.POID = PONumber;
                            //var saveDataForNotification = _ICommonMethod.SaveNotificationDetail(objNotify);

                            //if (getRuleData.DeviceId != null)
                            //{
                            //    objEmailHelper.IsWorkable = true;
                            //    // objEmailHelper.LogPOId = 
                            //    string message = PushNotificationMessages.POCreate(objPOTypeDataModel.PONumber, objEmailHelper.UserName, objEmailHelper.LocationName);
                            //    PushNotificationFCM.FCMAndroid(message, getRuleData.DeviceId, objEmailHelper);
                            //}
                            //Push Notification
                            /// string message = PushNotificationMessages.eFleetIncidentForServiceReported(objeFleetVehicleIncidentModel.LocationName, objeFleetVehicleIncidentModel.QRCodeID, objeFleetVehicleIncidentModel.VehicleNumber);
                            //PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = userData.UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = getHRDetail.jp.JPS_LocationId;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = userData.UserId;
                                    objEmailog.SentEmail = userData.UserEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = userData.UserId;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                            using (var context = new workorderEMSEntities())
                            {
                                context.EmailLogs.AddRange(objListEmailog);
                                context.SaveChanges();
                            }
                        }
                        #endregion Email
                    }
                    #endregion Create PO
                    //    return isSentForBackground;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendApplicantInfoForBackgrounddCheck(EmployeeVIewModel model)", "Exception While sending Applicant info for background check.", model);
                throw;
            }
            //return getDetails;
            //return isSaved;
            return isSentForBackground;
        }
        /// <summary>
        /// Created By  :Ashwajity Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To get I9 Info by Applicant Id
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public I9FormModel GetI9FormData(long ApplicantId, long UserId)
        {
            var _modelI9 = new I9FormModel();
            try
            {
                if(ApplicantId > 0 && UserId  > 0)
                {
                    var getApplicantDetails = _db.UserRegistrations.Where(x => x.UserId == UserId && x.IsDeleted == false).FirstOrDefault().EmployeeID;
                    _modelI9 = _db.spGetI9Form(getApplicantDetails).Select(x => new I9FormModel() {
                        EMA_Address = x.EMA_Address,
                        EMA_City = x.EMA_City,
                        EMA_State = x.EMA_State,
                        EMA_Zip  = x.EMA_Zip,
                        EMP_FirstName = x.EMP_FirstName,
                        EMP_LastName = x.EMP_LastName,
                        EMP_MiddleName = x.EMP_MiddleName,
                        I9F_Date = x.I9F_Date,
                        I9F_EMP_EmployeeId = x.I9F_EMP_EmployeeId,
                        I9F_Id = x.I9F_Id,
                        I9F_IsActive = x.I9F_IsActive,
                        I9F_Sec1_Address = x.I9F_Sec1_Address,
                        I9F_Sec1_AlienAuthorizedToWorkDate = x.I9F_Sec1_AlienAuthorizedToWorkDate,
                        I9F_Sec1_AlienRegistrationNum_USCIS = x.I9F_Sec1_AlienRegistrationNum_USCIS,
                        I9F_Sec1_AptNumber = x.I9F_Sec1_AptNumber,
                        I9F_Sec1_CitizenOfUS = x.I9F_Sec1_CitizenOfUS,
                        I9F_Sec1_City = x.I9F_Sec1_City,
                        I9F_Sec1_dateOfBirth = x.I9F_Sec1_dateOfBirth,
                        I9F_Sec1_DateOfEmployeeSign = x.I9F_Sec1_DateOfEmployeeSign,
                        I9F_Sec1_DateOfPreparerOrTranslatorSign = x.I9F_Sec1_DateOfPreparerOrTranslatorSign,
                        I9F_Sec1_Email = x.I9F_Sec1_Email,
                        I9F_Sec1_EmployeeTelephoneNumber = x.I9F_Sec1_EmployeeTelephoneNumber,
                        I9F_Sec1_FirstName =x.I9F_Sec1_FirstName,
                        I9F_Sec1_ForeignPassportIssuanceCountry =x.I9F_Sec1_ForeignPassportIssuanceCountry,
                        I9F_Sec1_ForeignPassportNumber = x.I9F_Sec1_ForeignPassportNumber,
                        I9F_Sec1_I94AdmissionNumber = x.I9F_Sec1_I94AdmissionNumber,
                        I9F_Sec1_LastName = x.I9F_Sec1_LastName,
                        I9F_Sec1_MiddleInitiaL =x.I9F_Sec1_MiddleInitial,
                        I9F_Sec1_NonCitizenOfUS = x.I9F_Sec1_NonCitizenOfUS,
                        I9F_Sec1_OtherLastName = x.I9F_Sec1_OtherLastName,
                        I9F_Sec1_PreparerAndTranslator = x.I9F_Sec1_PreparerAndTranslator,
                        I9F_Sec1_QRCodeSec1 =x.I9F_Sec1_QRCodeSec1,
                        I9F_Sec1_SignatureOfEmployee =x.I9F_Sec1_SignatureOfEmployee,
                        I9F_Sec1_SignatureOfPreparerOrTranslator =x.I9F_Sec1_SignatureOfPreparerOrTranslator,
                        I9F_Sec1_SSN =x.I9F_Sec1_SSN,
                        I9F_Sec1_State =x.I9F_Sec1_State,
                        I9F_Sec1_ZipCode =x.I9F_Sec1_ZipCode,
                        I9F_Sec2_AdditionalInformation =x.I9F_Sec2_AdditionalInformation,
                        //I9F_Sec2_A_FirstName =x.I9F_Sec2_FirstNameOfEmployerOrAuthorized,
                        //I9F_Sec2_A_LastName = x.laa
                        //I9F_Sec2_A_MiddleInitial = x.
                        //I9F_Sec2_B_DateOfReHire =x.
                        I9F_Sec2_CitizenshipImmigrationStatus = x.I9F_Sec2_CitizenshipImmigrationStatus,
                        //I9F_Sec2_C_DateOfEmployerOrAuthorizedSign = x.I9F_Sec2_DateOfEmployerOrAuthorizedSign,
                        //I9F_Sec2_C_DocumentNumber = x.
                        //I9F_Sec2_C_DocumentTitle =x.
                        //I9F_Sec2_C_ExpirationDate =  
                        //I9F_Sec2_C_NameOfEmployerOrAuthorized =
                        I9F_Sec2_EmployeesFirstDayOfEmployment = x.I9F_Sec2_EmployeesFirstDayOfEmployment,
                        I9F_Sec2_DateOfEmployerOrAuthorizedSign = x.I9F_Sec2_DateOfEmployerOrAuthorizedSign,
                        //I9F_Sec2_C_SignatureOfEmployerOrAuthorized = 
                        I9F_Sec2_EmployersBusinessOrgnization_Address = x.I9F_Sec2_EmployersBusinessOrgnization_Address,
                        I9F_Sec2_EmployersBusinessOrgnization_City = x.I9F_Sec2_EmployersBusinessOrgnization_City,
                        I9F_Sec2_EmployersBusinessOrgnization_Name = x.I9F_Sec2_EmployersBusinessOrgnization_Name,
                        I9F_Sec2_EmployersBusinessOrgnization_State = x.I9F_Sec2_EmployersBusinessOrgnization_State,
                        I9F_Sec2_EmployersBusinessOrgnization_ZipCode = x.I9F_Sec2_EmployersBusinessOrgnization_ZipCode,
                        I9F_Sec2_FirstNameOfEmployerOrAuthorized = x.I9F_Sec2_FirstNameOfEmployerOrAuthorized,
                        I9F_Sec2_LastNameOfEmployerOrAuthorized = x.I9F_Sec2_LastNameOfEmployerOrAuthorized,
                        I9F_Sec2_ListA_DocumentNumber1 =x.I9F_Sec2_ListA_DocumentNumber1,
                        I9F_Sec2_ListA_DocumentNumber2 = x.I9F_Sec2_ListA_DocumentNumber2,
                        I9F_Sec2_ListA_DocumentNumber3 =x.I9F_Sec2_ListA_DocumentNumber3,
                        I9F_Sec2_ListA_DocumentTitle1 = x. I9F_Sec2_ListA_DocumentTitle1,
                        I9F_Sec2_ListA_DocumentTitle2 = x.I9F_Sec2_ListA_DocumentTitle2,
                        I9F_Sec2_ListA_DocumentTitle3 = x.I9F_Sec2_ListA_DocumentTitle3,
                        I9F_Sec2_ListA_ExpirationDate1 = x.I9F_Sec2_ListA_ExpirationDate1,
                        I9F_Sec2_ListA_ExpirationDate2 = x.I9F_Sec2_ListA_ExpirationDate2,
                        I9F_Sec2_ListA_ExpirationDate3 = x.I9F_Sec2_ListA_ExpirationDate3,
                        I9F_Sec2_ListA_IssuingAuthority1 = x.I9F_Sec2_ListA_IssuingAuthority1,
                        I9F_Sec2_ListA_IssuingAuthority2 = x.I9F_Sec2_ListA_IssuingAuthority2,
                        I9F_Sec2_ListA_IssuingAuthority3  =x.I9F_Sec2_ListA_IssuingAuthority3,
                        I9F_Sec2_ListB_DocumentNumber =x.I9F_Sec2_ListB_DocumentNumber,
                        I9F_Sec2_ListB_DocumentTitle = x.I9F_Sec2_ListB_DocumentTitle,
                        I9F_Sec2_ListB_ExpirationDate = x.I9F_Sec2_ListB_ExpirationDate,
                        I9F_Sec2_ListB_IssuingAuthority = x.I9F_Sec2_ListB_IssuingAuthority,
                        I9F_Sec2_ListC_DocumentNumber = x.I9F_Sec2_ListC_DocumentNumber,
                        I9F_Sec2_ListC_DocumentTitle = x.I9F_Sec2_ListC_DocumentTitle,
                        I9F_Sec2_ListC_ExpirationDate = x.I9F_Sec2_ListC_ExpirationDate,
                        I9F_Sec2_ListC_IssuingAuthority  =x.I9F_Sec2_ListC_IssuingAuthority,
                        I9F_Sec2_QRCodeSec2AndSec3 = x.I9F_Sec2_QRCodeSec2AndSec3,
                        I9F_Sec2_SignatureOfEmployerOrAuthorized = x.I9F_Sec2_SignatureOfEmployerOrAuthorized,
                        I9F_Sec2_TitleOfEmployerOrOthrizedRepresentative = x.I9F_Sec2_TitleOfEmployerOrOthrizedRepresentative,
                        I9F_Sec3_A_FirstName = x.I9F_Sec3_A_FirstName,
                        I9F_Sec3_A_LastName = x.I9F_Sec3_A_LastName,
                        I9F_Sec3_A_MiddleInitial = x.I9F_Sec3_A_MiddleInitial,
                        I9F_Sec3_B_DateOfReHire = x.I9F_Sec3_B_DateOfReHire,
                        I9F_Sec3_C_DateOfEmployerOrAuthorizedSign = x.I9F_Sec3_C_DateOfEmployerOrAuthorizedSign,
                        I9F_Sec3_C_DocumentNumber = x.I9F_Sec3_C_DocumentNumber,
                        I9F_Sec3_C_DocumentTitle =x.I9F_Sec3_C_DocumentTitle,
                        I9F_Sec3_C_ExpirationDate = x.I9F_Sec3_C_ExpirationDate,
                        I9F_Sec3_C_NameOfEmployerOrAuthorized = x.I9F_Sec3_C_NameOfEmployerOrAuthorized,
                        I9F_Sec3_C_SignatureOfEmployerOrAuthorized = x.I9F_Sec3_C_SignatureOfEmployerOrAuthorized,
                        I9F_Sec2_MiddleInitialOfEmployerOrAuthorized = x.I9F_Sec2_MiddleInitialOfEmployerOrAuthorized,
                        //I9F_Sec2_C_SignatureOfEmployerOrAuthorized =x.
                        Case_Number = x.I9F_CaseNumber
                        
                    }).FirstOrDefault();
                    if (_modelI9 == null)
                        return new I9FormModel();
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public I9FormModel GetI9FormData(long ApplicantId)", "Exception While getting the I9 details.", ApplicantId);
                throw;
            }
            return _modelI9;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To save I9 form details
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ApplicantId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SetI9Form(long UserId, long ApplicantId, I9FormModel model)
        {
            var isSaved = false;
            var postData = new CommonHTTPClient();
            var _modelAPI = new PostI9FormDataInput();
            var objCommon = new CommonMethodManager();
            var submitCase = new SubmitCase();
            try
            {
                var action = model.I9F_Id == null ? "I" : "U";
                var getDetails = _db.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
                var getManagerDetails = _db.Employees.Where(x => x.EMP_EmployeeID == getDetails.HiringManagerEmployeeId).FirstOrDefault();
                if (UserId > 0 && ApplicantId > 0 && model != null && model.I9F_Id == null)
                {
                    #region I9 API
                    _modelAPI.alien_number = model.I9F_Sec1_AlienRegistrationNum_USCIS;
                    _modelAPI.case_creator_email_address = getManagerDetails.EMP_Email;
                    _modelAPI.case_creator_name = getDetails.HiringManagerName;
                    _modelAPI.case_creator_phone_number = getManagerDetails.EMP_Phone.ToString();
                    _modelAPI.citizenship_status_code = "US_CITIZEN";//model.I9F_Sec2_CitizenshipImmigrationStatus;
                    _modelAPI.client_company_id = null;
                    _modelAPI.client_software_version = "30";
                    _modelAPI.date_of_birth = model.I9F_Sec1_dateOfBirth == null || model.I9F_Sec1_dateOfBirth.ToString() == "" ? DateTime.Now.ToString("yyyy-MM-dd") :  model.I9F_Sec1_dateOfBirth.Value.ToString("yyyy-MM-dd");
                    _modelAPI.date_of_hire = model.I9F_Sec3_B_DateOfReHire == null || model.I9F_Sec3_B_DateOfReHire.ToString() == "" ? DateTime.Now.ToString("yyyy-MM-dd"): model.I9F_Sec3_B_DateOfReHire.Value.ToString("yyyy-MM-dd");
                    _modelAPI.document_a_type_code = "US_PASSPORT";
                    _modelAPI.document_bc_number =  model.I9F_Sec2_ListA_DocumentNumber1;
                    _modelAPI.document_b_type_code = model.I9F_Sec2_ListB_DocumentTitle;
                    _modelAPI.document_c_type_code = model.I9F_Sec2_ListC_DocumentTitle;
                    _modelAPI.document_sub_type_code = null;
                    _modelAPI.duplicate_continue_reason = null;
                    _modelAPI.employee_email_address = model.I9F_Sec1_Email;
                    _modelAPI.employer_case_id = null;
                    _modelAPI.expiration_date = model.I9F_Sec3_C_ExpirationDate == null || model.I9F_Sec3_C_ExpirationDate.ToString() == "" ? DateTime.Now.ToString("yyyy-MM-dd") : model.I9F_Sec3_C_ExpirationDate.ToString("yyyy-MM-dd");
                    _modelAPI.us_passport_number = model.I9F_Sec1_ForeignPassportNumber;// model.I9F_Sec1_ForeignPassportNumber;
                    _modelAPI.us_state_code = model.I9F_Sec1_State;
                    _modelAPI.visa_number = null;
                    _modelAPI.no_expiration_date = false;
                    _modelAPI.ssn = model.I9F_Sec1_SSN;//model.I9F_Sec1_SSN;
                    _modelAPI.reason_for_delay_code = "TECHNICAL_PROBLEMS";
                    var getSerializeString = objCommon.GetJsoSerializeDataForAPI(APIName.I9PostDataCase, _modelAPI);
                    var saveAPI = postData.I9PostCase(getSerializeString, "", model.RefreshTokenI9);
                    var getOutputData = Newtonsoft.Json.JsonConvert.DeserializeObject<GetI9FormDataOutput>(saveAPI); 
                    #endregion I9 API
                    var save = _db.spSetI9Form(action, model.I9F_Id, model.I9F_EMP_EmployeeId, model.I9F_Sec1_SSN, model.I9F_Sec1_CitizenOfUS, model.I9F_Sec1_NonCitizenOfUS,
                               model.I9F_Sec1_AlienRegistrationNum_USCIS, model.I9F_Sec1_AlienAuthorizedToWorkDate, model.I9F_Sec1_I94AdmissionNumber, model.I9F_Sec1_ForeignPassportNumber,
                               model.I9F_Sec1_ForeignPassportIssuanceCountry, model.I9F_Sec1_SignatureOfEmployee, model.I9F_Sec1_DateOfEmployeeSign, model.I9F_Sec1_QRCodeSec1,
                               model.I9F_Sec1_PreparerAndTranslator, model.I9F_Sec1_SignatureOfPreparerOrTranslator, model.I9F_Sec1_DateOfPreparerOrTranslatorSign, model.I9F_Sec1_FirstName,
                               model.I9F_Sec1_MiddleInitiaL, model.I9F_Sec1_LastName, model.I9F_Sec1_OtherLastName, model.I9F_Sec1_dateOfBirth, model.I9F_Sec1_Address,
                               model.I9F_Sec1_AptNumber, model.I9F_Sec1_City, model.I9F_Sec1_State, model.I9F_Sec1_ZipCode, model.I9F_Sec1_Email, model.I9F_Sec1_EmployeeTelephoneNumber, model.I9F_Sec2_ListA_DocumentTitle1,
                               model.I9F_Sec2_ListA_IssuingAuthority1,model.I9F_Sec2_ListA_DocumentNumber1,model.I9F_Sec2_ListA_ExpirationDate1, model.I9F_Sec2_ListA_DocumentTitle2,
                               model.I9F_Sec2_ListA_IssuingAuthority2, model.I9F_Sec2_ListA_DocumentNumber2, model.I9F_Sec2_ListA_ExpirationDate2, model.I9F_Sec2_ListA_DocumentTitle3,
                               model.I9F_Sec2_ListA_IssuingAuthority3, model.I9F_Sec2_ListA_DocumentNumber3,model.I9F_Sec2_ListA_ExpirationDate3,model.I9F_Sec2_ListB_DocumentTitle,
                               model.I9F_Sec2_ListB_IssuingAuthority,model.I9F_Sec2_ListB_DocumentNumber,model.I9F_Sec2_ListB_ExpirationDate, model.I9F_Sec2_ListC_DocumentTitle,
                               model.I9F_Sec2_ListC_IssuingAuthority, model.I9F_Sec2_ListC_DocumentNumber, model.I9F_Sec2_ListC_ExpirationDate, model.I9F_Sec2_AdditionalInformation,
                               model.I9F_Sec2_QRCodeSec2AndSec3, model.I9F_Sec2_EmployeesFirstDayOfEmployment, model.I9F_Sec2_SignatureOfEmployerOrAuthorized, model.I9F_Sec2_DateOfEmployerOrAuthorizedSign,
                               model.I9F_Sec2_LastNameOfEmployerOrAuthorized, model.I9F_Sec2_FirstNameOfEmployerOrAuthorized, model.I9F_Sec2_MiddleInitialOfEmployerOrAuthorized,
                               model.I9F_Sec2_EmployersBusinessOrgnization_Name, model.I9F_Sec2_EmployersBusinessOrgnization_Address, model.I9F_Sec2_EmployersBusinessOrgnization_City,
                               model.I9F_Sec2_EmployersBusinessOrgnization_State, model.I9F_Sec2_EmployersBusinessOrgnization_ZipCode, model.I9F_Sec2_TitleOfEmployerOrOthrizedRepresentative,
                               model.I9F_Sec2_CitizenshipImmigrationStatus, model.I9F_Sec3_A_LastName, model.I9F_Sec3_A_FirstName, model.I9F_Sec3_A_MiddleInitial, model.I9F_Sec3_B_DateOfReHire,
                               model.I9F_Sec3_C_DocumentTitle, model.I9F_Sec3_C_DocumentNumber, model.I9F_Sec3_C_ExpirationDate,model.I9F_Sec3_C_SignatureOfEmployerOrAuthorized,
                               model.I9F_Sec3_C_DateOfEmployerOrAuthorizedSign,model.I9F_Sec3_C_NameOfEmployerOrAuthorized,model.I9F_Date,"Y", getOutputData.case_number);
                    
                }
                else
                {
                    _db.spSetI9Form(action, model.I9F_Id, model.I9F_EMP_EmployeeId, model.I9F_Sec1_SSN, model.I9F_Sec1_CitizenOfUS, model.I9F_Sec1_NonCitizenOfUS,
                               model.I9F_Sec1_AlienRegistrationNum_USCIS, model.I9F_Sec1_AlienAuthorizedToWorkDate, model.I9F_Sec1_I94AdmissionNumber, model.I9F_Sec1_ForeignPassportNumber,
                               model.I9F_Sec1_ForeignPassportIssuanceCountry, model.I9F_Sec1_SignatureOfEmployee, model.I9F_Sec1_DateOfEmployeeSign, model.I9F_Sec1_QRCodeSec1,
                               model.I9F_Sec1_PreparerAndTranslator, model.I9F_Sec1_SignatureOfPreparerOrTranslator, model.I9F_Sec1_DateOfPreparerOrTranslatorSign, model.I9F_Sec1_FirstName,
                               model.I9F_Sec1_MiddleInitiaL, model.I9F_Sec1_LastName, model.I9F_Sec1_OtherLastName, model.I9F_Sec1_dateOfBirth, model.I9F_Sec1_Address,
                               model.I9F_Sec1_AptNumber, model.I9F_Sec1_City, model.I9F_Sec1_State, model.I9F_Sec1_ZipCode, model.I9F_Sec1_Email, model.I9F_Sec1_EmployeeTelephoneNumber, model.I9F_Sec2_ListA_DocumentTitle1,
                               model.I9F_Sec2_ListA_IssuingAuthority1, model.I9F_Sec2_ListA_DocumentNumber1, model.I9F_Sec2_ListA_ExpirationDate1, model.I9F_Sec2_ListA_DocumentTitle2,
                               model.I9F_Sec2_ListA_IssuingAuthority2, model.I9F_Sec2_ListA_DocumentNumber2, model.I9F_Sec2_ListA_ExpirationDate2, model.I9F_Sec2_ListA_DocumentTitle3,
                               model.I9F_Sec2_ListA_IssuingAuthority3, model.I9F_Sec2_ListA_DocumentNumber3, model.I9F_Sec2_ListA_ExpirationDate3, model.I9F_Sec2_ListB_DocumentTitle,
                               model.I9F_Sec2_ListB_IssuingAuthority, model.I9F_Sec2_ListB_DocumentNumber, model.I9F_Sec2_ListB_ExpirationDate, model.I9F_Sec2_ListC_DocumentTitle,
                               model.I9F_Sec2_ListC_IssuingAuthority, model.I9F_Sec2_ListC_DocumentNumber, model.I9F_Sec2_ListC_ExpirationDate, model.I9F_Sec2_AdditionalInformation,
                               model.I9F_Sec2_QRCodeSec2AndSec3, model.I9F_Sec2_EmployeesFirstDayOfEmployment, model.I9F_Sec2_SignatureOfEmployerOrAuthorized, model.I9F_Sec2_DateOfEmployerOrAuthorizedSign,
                               model.I9F_Sec2_LastNameOfEmployerOrAuthorized, model.I9F_Sec2_FirstNameOfEmployerOrAuthorized, model.I9F_Sec2_MiddleInitialOfEmployerOrAuthorized,
                               model.I9F_Sec2_EmployersBusinessOrgnization_Name, model.I9F_Sec2_EmployersBusinessOrgnization_Address, model.I9F_Sec2_EmployersBusinessOrgnization_City,
                               model.I9F_Sec2_EmployersBusinessOrgnization_State, model.I9F_Sec2_EmployersBusinessOrgnization_ZipCode, model.I9F_Sec2_TitleOfEmployerOrOthrizedRepresentative,
                               model.I9F_Sec2_CitizenshipImmigrationStatus, model.I9F_Sec3_A_LastName, model.I9F_Sec3_A_FirstName, model.I9F_Sec3_A_MiddleInitial, model.I9F_Sec3_B_DateOfReHire,
                               model.I9F_Sec3_C_DocumentTitle, model.I9F_Sec3_C_DocumentNumber, model.I9F_Sec3_C_ExpirationDate, model.I9F_Sec3_C_SignatureOfEmployerOrAuthorized,
                               model.I9F_Sec3_C_DateOfEmployerOrAuthorizedSign, model.I9F_Sec3_C_NameOfEmployerOrAuthorized, model.I9F_Date, "Y", model.Case_Number);
                    #region API
                    var saveAPI = postData.I9PostCase("", APIName.I9CaseSubmit + model.Case_Number+ "/submit", model.RefreshTokenI9);
                    var getOutputData = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitCase>(saveAPI);
                    #endregion API
                }
                isSaved = true;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SetI9Form(long UserId, long ApplicanId, I9FormModel model)", "Exception While saving  I9 details.", model);
                throw;
            }
            return isSaved;

        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-Feb-2020
        /// Created For : To get signature by applicant id.
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public Desclaimer GetSignature(long ApplicantId)
        {
            var signature = new Desclaimer();
            try
            {
               if(ApplicantId > 0)
                {
                    signature = _db.spGetApplicantSignature(ApplicantId).Select(x =>
                    new Desclaimer() {
                        ApplicantId  = x.ASG_APT_ApplicantId,
                        ASG_Date = x.ASG_Date,
                        EmployeeId  =x.ASG_EMP_EmployeeId,
                        Signature = x.ASG_Signature,
                        Sing_Id = x.ASG_Id,
                        IsActive = x.ASG_IsActive
                    }).FirstOrDefault();
                    if(signature != null)
                    {
                       
                        return signature;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string GetSignature(long ApplicantId)", "Exception While getting signature.", ApplicantId);
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 19-Feb-2020
        /// Created For : To save desclaimer data of applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveDesclaimerData(Desclaimer obj)
        {
            bool isSaved = false;
            try
            {
                if (obj != null)
                {
                    string Action = "I";
                    var save = _db.spSetApplicantSignature(Action, obj.Sing_Id, obj.ApplicantId, null, obj.Signature, "Y");
                    isSaved = save > 0 ? true : false;
                    return isSaved;
                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveDesclaimerData(Desclaimer obj)", "Exception While saving desclaimer data.", obj);
                throw;
            }
        }
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : TO save self identification form of applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveSelfIdentification(SelfIdentificationModel obj)
        {
            bool isSaved = false;
            try
            {
                if (obj != null)
                {
                    string Action = obj.Self_Id == null?"I":"U";
                    var save = _db.spSetEEO(Action,obj.Self_Id, obj.EmployeeId, obj.Gender, obj.Race_Ethnicity,
                                            obj.VeteranStatus, obj.DateOfDischarge, obj.Disability,"Y", obj.Description);
                    isSaved = save > 0 ? true : false;
                    return isSaved;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveDesclaimerData(Desclaimer obj)", "Exception While saving desclaimer data.", obj);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : TO get self identifiation form details
        /// Created For : 21-Feb-2020
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public SelfIdentificationModel GetSelfIdentification(string EmployeeId)
        {
            var lst = new SelfIdentificationModel();
            try
            {
                if(EmployeeId != null)
                {
                    lst = _db.spGetEEO(EmployeeId).Select(x => new SelfIdentificationModel() {
                        DateOfDischarge = x.EEO_VeteranDateOfDischarge,
                        Disability = x.EEO_DisabilityDisclose,
                        EmployeeId = x.EEO_EMP_EmployeeID,
                        //FirstName = x.f
                    }).FirstOrDefault();
                    if (lst != null)
                        return lst;
                    else
                        return null;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public SelfIdentificationModel GetSelfIdentification(string EmployeeId)", "Exception While getting the details of self identification.", EmployeeId);
                throw;
            }
            return lst;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : To save applicant fun facts of applicant.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveApplicantFunFacts(ApplicantFunFactModel obj)
        {
            bool isSaved = false;
            int save = 0;
            int i = 0;
            try
            {
                if (obj != null)
                {
                    var getMasterId = Convert.ToInt64(InterviewQuestionsId.MasterId);
                    var getQuestionsIdForFunFacts = _db.InterviewQuestionChilds.Where(x => x.IQC_IQM_Id == getMasterId).ToList();
                    for(i = 0;i< getQuestionsIdForFunFacts.Count();i++)
                    {
                        string Answer = string.Empty;
                        if (getQuestionsIdForFunFacts[i].IQC_Id == Convert.ToInt64(InterviewQuestionsId.QuestionId_45))
                            Answer = obj.Answer_Que1;
                        else if (getQuestionsIdForFunFacts[i].IQC_Id == Convert.ToInt64(InterviewQuestionsId.QuestionId_46))
                            Answer = obj.Answer_Que2;
                        else if (getQuestionsIdForFunFacts[i].IQC_Id == Convert.ToInt64(InterviewQuestionsId.QuestionId_47))
                            Answer = obj.Answer_Que3;
                        else 
                            Answer = obj.Answer_Que4;
                        save = _db.spSetInterviewAnswer(getQuestionsIdForFunFacts[i].IQC_IQM_Id, obj.Applicant_Id, obj.Employee_Id, null, Answer);
                    }                   
                    isSaved = save > 0 ? true : false;
                    return isSaved;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveApplicantFunFacts(ApplicantFunFactModel obj)", "Exception While saving .", obj);
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 28-Feb-2020
        /// Created For : To get the details of rate of pay
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public RateOfPayModel GetRateOfPayInfo(long ApplicantId, string employeeId)
        {
            var _model = new RateOfPayModel();
            try
            {
                if(ApplicantId != null)
                {
                    _model = _db.spGetFormRateOfPay(ApplicantId).Select(x => new RateOfPayModel()
                    {
                        EmployeeName = x.EmployeeName,
                        EmployeeNumber = employeeId,
                        ManagerName = x.HiringManager,
                        JobTitle = x.JBT_JobTitle,
                        RateOfPay = x.VST_RateOfPay,
                        Operations = x.Operation,
                        JobStatus = x.VST_EmploymentStatus,
                        Location = x.LocationName,
                        TypeOfPayChange = x.TypeOfPayChange,
                        IsExempt = x.VST_IsExempt
                    }).FirstOrDefault() ;
                }
                return _model;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public RateOfPayModel GetRateOfPayInfo(string ApplicantId)", "Exception While getting the details of rate of pay .", ApplicantId);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-March-2020
        /// Created For : 
        /// </summary>
        /// <returns></returns>
        public List<JobPostingDropDownModel> GetListJobPosting(eTracLoginModel obj)
        {
            var lst = new List<JobPostingDropDownModel>();
            try
            {
                lst = _db.spGetJobPosting(null).Select(x => new JobPostingDropDownModel()
                {
                    JobPostingId = x.JPS_JobPostingId,
                    JobTitle = x.JobTitle,
                    JobDescription = x.VST_JobDescription,
                    LocationName = x.LocationName,
                    PositionCount = x.PositionCount,
                    Status = x.Status,
                    JobPostingDate = x.JobPostingDate,
                    IsExempt = x.VST_IsExempt,
                    DOT_Status = x.JPS_DrivingType
                    //JobDescription = x.jo
                }).ToList();
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<JobPostingModel> GetListJobPosting(eTracLoginModel obj)", "Exception While getting the list of jobs.", obj);
                throw;
            }
            return lst;
        }

        #region Oriantation
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 11-March-2020
        /// Created For : To save employee oriantation withh date and time
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveOriantation (OriantationModel model)
        {
            var isSaved = false;
            try
            {
                var Action = model.OTN_ID == null ? "I" : "U";
                _db.spSetOrientation(Action, model.OTN_ID, model.OTN_EMP_EmployeeID, model.OTN_LocationId,
                                                  model.ONT_OrientationDate, model.ONT_Orientationtime, model.ONT_IsActive);
                 isSaved = true;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveOriantation (OriantationModel model)", "Exception While saving oriantation of employee.", model);               
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        #endregion Oriantation

        #region Offer Letter
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 14-March-2020
        /// Created for : to get offer details of applicant to send offer
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public OfferModel GetOfferDetailsOfApplicant(long ApplicantId)
        {
            var lst = new OfferModel();
            try
            {
                lst = _db.spGetOfferLetter(ApplicantId).Select(x => new OfferModel()
                {
                    DesireSalaryWages = x.API_DesireSalaryWages,
                    DueDate = x.OFL_OfferLetterDueDate,
                     Email= x.ACI_eMail,
                    FirstName = x.API_FirstName,
                    LastName = x.API_LastName,
                    MiddleName = x.API_MidName,
                    LocationName = x.LocationName,
                    HMName = x.HiringManagerName,
                    IsExempt = x.VST_IsExempt,
                    PhoneNumber = x.ACI_PhoneNo,
                    ApplicantId = ApplicantId,
                    OfferAmount = x.OFL_OfferAmount,
                    StartDate = x.APT_DateOfJoining,
                    Title = x.JBT_JobTitle,
                    HMPosition = x.HiringManagerPosition,
                    VST_RateOfPay = x.VST_RateOfPay
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<OfferModel> GetOfferDetailsOfApplicant(long AplicantId)", "Exception While getting the details of applicant.", ApplicantId);
                throw;
            }
            return lst;
        }
        #endregion Offer Letter
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-March-2020
        /// Created For : To screened or reject applicant as per detail 
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <param name="IsScreened"></param>
        /// <returns></returns>
        public bool SaveScreenRejectStatusApplicant(long ApplicantId, bool IsScreened)
        {
            var status = IsScreened == true ? ApplicantStatus.Screened : ApplicantStatus.Reject;
            return _db.spSetApplicantStatus(ApplicantId, status, ApplicantIsActiveStatus.Pass) > 0 ? true : false;
        }
       
        public EmployeeVIewModel GetApplicantAllDetails(long ApplicantId)
        {
            return  _db.spGetApplicantAllDetails(ApplicantId).Select(x => new EmployeeVIewModel()
            {
                FirstName = x.API_FirstName,
                MiddleName = x.API_MidName,
                LastName = x.API_LastName,
                HiringManager = x.HiringManagerEmployeeId
                //Dob = x
            }).FirstOrDefault();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14-04-2020
        /// Created For : To add all details to background screening model and convert into json serialize
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string ConvertBackgroundDataJsonSerializer(BackgroundCheckForm model,UserRegistration user)
        {
            string result = string.Empty;
            string zipcode = string.Empty;
            //models obj
            var objCommon = new CommonMethodManager();
            var objBackground = new BackgroundScreeningAPIModel();
            var objCountyCivilLowerSearch = new CountyCivilLowerSearch();
            var objcountyCivilUpperSearches = new CountyCivilUpperSearch();
            var objcountyCriminalSearches = new CountyCriminalSearch();
            var drugScreenings = new DrugScreening();
            var objeducationVerifications = new EducationVerification();
            var objemploymentVerifications = new EmploymentVerification();
            var objfederalBankruptcySearches = new FederalBankruptcySearch();
            var objfederalCriminalSearches = new FederalCriminalSearch();
            var objfederalCivilSearches = new FederalCivilSearch();
            var objmotorVehicleRecordSearches = new MotorVehicleRecordSearch();
            var objpositionLocation = new PositionLocation();
            var objpositionStartingPay = new PositionStartingPay();
            var objsexOffenderSearches = new SexOffenderSearch();
            var objstateCriminalSearches = new StateCriminalSearch();
            var objsubjectAdmittedCriminalHistory = new SubjectAdmittedCriminalHistory();
            var objsubjectIndividualAliases = new SubjectIndividualAlias();
            var objsubjectPreviousLocations = new SubjectPreviousLocation();
            var objworkersCompensationSearches = new WorkersCompensationSearch();
            var data = _db.ApplicantLoginAccesses.Join(_db.Applicants, q => q.ALA_UserId, u => u.APT_ALA_UserId, (q, u) => new { q, u }).
                                       Where(x => x.u.APT_ApplicantId == model.ApplicantPersonalInfo.API_APT_ApplicantId).FirstOrDefault();
            var contact = _db.spGetApplicantContactInfo(model.ApplicantPersonalInfo.API_APT_ApplicantId).Take(1).FirstOrDefault();
            objBackground.collectAdditionalInfo = true;
            objBackground.comment = null;// model.ApplicantPersonalInfo.
            objCountyCivilLowerSearch.county =  "Pasco";
            objCountyCivilLowerSearch.region = "FL";
            objBackground.countyCivilLowerSearches.Add(objCountyCivilLowerSearch);
            objcountyCivilUpperSearches.county = "Pasco";
            objcountyCivilUpperSearches.region = "FL";
            objBackground.countyCivilUpperSearches.Add(objcountyCivilUpperSearches);
            objcountyCriminalSearches.county = "Pasco";
            objcountyCriminalSearches.region = "FL";
            objBackground.countyCriminalSearches.Add(objcountyCriminalSearches);
            objBackground.customScreeningId = Convert.ToInt64(model.ApplicantPersonalInfo.API_APT_ApplicantId);
            objBackground.customSubjectId = Convert.ToInt64(model.ApplicantPersonalInfo.API_APT_ApplicantId);
            objBackground.disableDuplicateChecking = true;
            drugScreenings.testType = "Screening";
            objBackground.drugScreenings.Add(drugScreenings);
            var getEducationDetails = _db.spGetAplicantAcadmicDetails(model.ApplicantPersonalInfo.API_APT_ApplicantId).ToList();
            foreach (var item in getEducationDetails)
            {
                objeducationVerifications.address = item.AAD_City+","+item.AAD_State;
                objeducationVerifications.courseOfStudy = item.AAD_EducationType;
                objeducationVerifications.degree = item.AAD_EducationType;
                objeducationVerifications.fromDate = item.AAD_AttendedFrom.ToString("yyyy-MM-dd");
                objeducationVerifications.graduated = true;
                objeducationVerifications.municipality = item.AAD_City;
                objeducationVerifications.organization = item.AAD_InstituteName;
                objeducationVerifications.phone  = contact.ACI_PhoneNo.ToString();
                objeducationVerifications.postalCode = item.AAD_Zip.ToString();
                objeducationVerifications.region = "FL";
                objeducationVerifications.toDate = item.AAD_AttendedTo.ToString("yyyy-MM-dd");
                objeducationVerifications.studentName = model.ApplicantPersonalInfo.API_FirstName + " " + model.ApplicantPersonalInfo.API_LastName;
                objBackground.educationVerifications.Add(objeducationVerifications);
            }
            var getBackgroundDetails = _db.spGetApplicantBackgroundHistory(model.ApplicantPersonalInfo.API_APT_ApplicantId).ToList();
            foreach (var item in getBackgroundDetails)
            {
                objemploymentVerifications.address = item.ABH_Address;
                objemploymentVerifications.contactName = item.ABH_CompanyName;
                objemploymentVerifications.contactTelephone = item.ABH_Phone.ToString();
                objemploymentVerifications.employer = item.ABH_CompanyName;
                objemploymentVerifications.fromDate = item.ABH_Date.ToString("yyyy-MM-dd");
                objemploymentVerifications.toDate = item.ABH_Date.ToString("yyyy-MM-dd");
                objemploymentVerifications.isCurrentEmployer = item.ABH_StillEmployed == "Y"?true:false;
                objemploymentVerifications.municipality = item.ABH_City;
                objemploymentVerifications.permissionToContact = true;
                objemploymentVerifications.positionTitle = "";
                objemploymentVerifications.postalCode = item.ABH_ZIPCode.ToString();
                objemploymentVerifications.reasonForLeaving = item.ABH_ReasonforLeaving;
                objemploymentVerifications.region = "FL";
                objemploymentVerifications.remunerationInterval = "Hourly";
                objemploymentVerifications.remunerationValue = 0;
                objBackground.employmentVerifications.Add(objemploymentVerifications);
            }
            objfederalBankruptcySearches.county = "Pasco";
            objfederalBankruptcySearches.postalCode = model.ApplicantAddress[0].APA_Zip.ToString();
            objfederalBankruptcySearches.region = "FL";
            objBackground.federalBankruptcySearches.Add(objfederalBankruptcySearches);

            objfederalCivilSearches.region = "FL";
            objfederalCivilSearches.county = "Pasco";
            objBackground.federalCivilSearches.Add(objfederalCivilSearches);

            objfederalCriminalSearches.county = "Pasco";
            objfederalCriminalSearches.region = "FL";
            objfederalCriminalSearches.postalCode = model.ApplicantAddress[0].APA_Zip.ToString();
            objBackground.federalCriminalSearches.Add(objfederalCriminalSearches);

            objmotorVehicleRecordSearches.region = "FL";
            objmotorVehicleRecordSearches.countryCode = "US";
            objmotorVehicleRecordSearches.licenseIdentifier = model.ApplicantPersonalInfo.API_DLNumber;
            objBackground.motorVehicleRecordSearches.Add(objmotorVehicleRecordSearches);

            objBackground.packageId = "AAVPP";
            //var getPositionDetails = _db.spGetApplicantPositionTitle(model.ApplicantPersonalInfo.API_APT_ApplicantId).ToList();
            //foreach (var item in getPositionDetails)
            //{

            //}
            objpositionLocation.address = model.ApplicantAddress[0].APA_StreetAddress;
            objpositionLocation.countryCode = "US";
            objpositionLocation.municipality = model.ApplicantAddress[0].APA_City;
            objpositionLocation.postalCode = model.ApplicantAddress[0].APA_Zip.ToString();
            objBackground.positionLocation = objpositionLocation;

            objpositionStartingPay.currencyId = "USD";
            objpositionStartingPay.value = 0;
            objpositionStartingPay.interval = "Hourly";
            objBackground.positionStartingPay = objpositionStartingPay;

            objBackground.processAddressInsight = true;
            objBackground.processAdverseAction = true;
            objBackground.processBusinessCreditCheck = true;
            objBackground.processCreditCheck = true;
            objBackground.processFacisScreening = true;
            objBackground.processGlobalInsight = true;
            objBackground.processGsaScreening = true;
            objBackground.processNationalCriminalInsight = true;
            objBackground.processOfacScreening = true;
            objBackground.processSocialSecurityTrace = true;
            objBackground.referenceId = null;
            objBackground.requesterEmail = user.UserEmail;
            objBackground.requesterName = user.FirstName + " " + user.LastName;

            objsexOffenderSearches.region = "FL";
            objBackground.sexOffenderSearches.Add(objsexOffenderSearches);

            objstateCriminalSearches.region = "FL";
            objBackground.stateCriminalSearches.Add(objstateCriminalSearches);

            objsubjectAdmittedCriminalHistory.caseNumber = null;
            objsubjectAdmittedCriminalHistory.charge = null;
            objsubjectAdmittedCriminalHistory.date = DateTime.Now.ToString("yyyy-MM-dd");
            objsubjectAdmittedCriminalHistory.disposition = null;
            objsubjectAdmittedCriminalHistory.finalLevel = null;
            objsubjectAdmittedCriminalHistory.jurisdiction = null;
            objsubjectAdmittedCriminalHistory.notes = null;
            objsubjectAdmittedCriminalHistory.sentence = null;
            objBackground.subjectAdmittedCriminalHistory.Add(objsubjectAdmittedCriminalHistory);

            var getAdditionalInfo = _db.spGetApplicantAdditionalInfo(model.ApplicantPersonalInfo.API_APT_ApplicantId).FirstOrDefault();
            objBackground.subjectCurrentAddressAddressLine = model.ApplicantAddress[0].APA_StreetAddress;
            objBackground.subjectCurrentAddressCountryCode = "US";
            objBackground.subjectCurrentAddressMunicipality = model.ApplicantAddress[0].APA_City;
            objBackground.subjectCurrentAddressPostalCode = model.ApplicantAddress[0].APA_Zip.ToString();
            objBackground.subjectCurrentAddressRegion = "FL";
            objBackground.subjectCurrentAddressStartDate = data.u.APT_DateOfJoining.ToString("yyyy-MM-dd");
            //dob add later
            objBackground.subjectDateOfBirth = "1994-04-22";//getAdditionalInfo.AAI_AvailableDate.ToString("yyyy-MM-dd");
            objBackground.subjectEmail = data.q.ALA_eMailId;
            objBackground.subjectFamilyName = model.ApplicantPersonalInfo.API_LastName;
            objBackground.subjectFederalEmployerIdentificationNumber = model.ApplicantPersonalInfo.API_SSN;
            //add later
            objBackground.subjectGender = "Male";
            objBackground.subjectGivenName = model.ApplicantPersonalInfo.API_FirstName + " " + model.ApplicantPersonalInfo.API_LastName;

            objsubjectIndividualAliases.familyName = model.ApplicantPersonalInfo.API_LastName;
            objsubjectIndividualAliases.givenName = model.ApplicantPersonalInfo.API_FirstName + " " + model.ApplicantPersonalInfo.API_LastName;
            objBackground.subjectIndividualAliases.Add(objsubjectIndividualAliases);

            objBackground.subjectMiddleName = model.ApplicantPersonalInfo.API_MiddleName;
            objBackground.subjectOrganizationName = getBackgroundDetails[0].ABH_CompanyName;

            foreach (var item in model.ApplicantAddress)
            {
                objsubjectPreviousLocations.address = item.APA_StreetAddress;
                objsubjectPreviousLocations.countryCode = "US";
                objsubjectPreviousLocations.endDate = item.APA_YearsAddressTo.ToString("yyyy-MM-dd");
                objsubjectPreviousLocations.municipality = item.APA_City;
                objsubjectPreviousLocations.postalCode = item.APA_Zip.ToString();
                objsubjectPreviousLocations.region = "FL";
                objsubjectPreviousLocations.startDate = item.APA_YearsAddressFrom.ToString("yyyy-MM-dd");
                objBackground.subjectPreviousLocations.Add(objsubjectPreviousLocations);
            }

            objBackground.subjectSocialSecurityNumber = model.ApplicantPersonalInfo.API_SSN;
            objBackground.subjectTelephoneNumber = contact.ACI_PhoneNo.ToString();
            objBackground.userDefinedField2 = null;
            objBackground.userDefinedField3 = null;

            objworkersCompensationSearches.region = "FL";
            objBackground.workersCompensationSearches.Add(objworkersCompensationSearches);
            return result = objCommon.GetJsoSerializeDataForAPI(APIName.I9AuthenticationAPI, objBackground);
        }
    }
}