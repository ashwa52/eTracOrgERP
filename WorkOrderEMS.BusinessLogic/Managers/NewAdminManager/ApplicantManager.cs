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
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

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
                    loginModel = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == obj.LoginId && x.ALA_Password == password).Select(a => new eTracLoginModel()
                    {
                        FName = a.ALA_FirstName,
                        LName = a.ALA_LastName,
                        Email = a.ALA_eMailId,
                        MName = a.ALA_MidName,
                        LoginId = a.ALA_LoginId,
                        UserId = a.ALA_UserId
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
                        objEmailHelper.RegistrationLink = ConfigurationManager.AppSettings["hostingprefix"];
                        objEmailHelper.Name = userData.ALA_FirstName + " " + userData.ALA_LastName;
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
            try
            {
                if (obj.FName != null && obj.Email != null)
                {
                    var password = Cryptography.GetEncryptedData(obj.Password, true);
                    //var UserId = new ObjectParameter("UserId", "UserId");
                    var isChanged = _db.spSetApplicantCreateLoginAccess(obj.Email, password, obj.FName, obj.MName, obj.LName, obj.Email, obj.Question, obj.Answer).FirstOrDefault();

                    //var userIdData = isChanged
                    if (isChanged.Value > 0)
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
                    else if (model.IsPrinterAssets == true && model.PrinterAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Printer", model.PrinterAssets.AssetsName,
                            model.PrinterAssets.AssetDescription, model.PrinterAssets.Make, model.PrinterAssets.Model, model.PrinterAssets.SerialNumber
                            , null, null, null, null, null, null, "N");
                    }
                    else if (model.IsCellPhoneAssets == true && model.CellPhoneAssets != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Cell Phone", model.CellPhoneAssets.AssetsName,
                            model.CellPhoneAssets.AssetDescription, model.CellPhoneAssets.Make, model.CellPhoneAssets.Model, model.CellPhoneAssets.SerialNumber,
                            null, null, null, null, null, null, "N");
                    }
                    else if (model.IsOfficePhone == true && model.OfficePhone != null)
                    {
                        var saveData = _db.spSetAssetAllocation(model.Action, model.AssetsId, model.EmployeeId, "Office Phone", model.OfficePhone.AssetsName, model.OfficePhone.AssetDescription,
                            model.OfficePhone.Make, model.OfficePhone.Model, model.OfficePhone.SerialNumber, null, null, null, null,
                            null, null, "N");
                    }
                    else
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
                    var getData = _db.spSetApplicantStatus(model.ApplicantId, "I", "S");
                    #region Email
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    var getEMPData = _db.Employees.Where(x => x.EMP_Id == model.ApplicantId).FirstOrDefault();
                    var userData = _db.spGetEmployeePersonalInfo(getEMPData.EMP_EmployeeID).FirstOrDefault();
                    if (userData != null)
                    {
                        bool IsSent = false;
                        var objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = userData.EMP_Email;
                        objEmailHelper.RegistrationLink = ConfigurationManager.AppSettings["hostingprefix"];
                        objEmailHelper.Name = userData.EMP_FirstName + " " + userData.EMP_LastName;
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
                                //objEmailog.LocationId = location;
                                objEmailog.ModifiedBy = null;
                                objEmailog.ModifiedOn = null;
                                objEmailog.SentBy = model.UserId;
                                objEmailog.SentEmail = userData.EMP_Email;
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
                    System.Data.DataTable ApplicantTrafficConvictionsTable = Obj.ApplicantTrafficConvictions.ToDataTable();
                    System.Data.DataTable ApplicantVehiclesOperatedTable = Obj.ApplicantVehiclesOperated.ToDataTable();
                    System.Data.DataTable ApplicantLicenseHealdTable = Obj.ApplicantLicenseHeald.ToDataTable();
                    //Obj.ApplicantSchecduleAvaliblity = new List<ApplicantSchecduleAvaliblity>();
                    //System.Data.DataTable ApplicantSchecduleAvaliblityTable = Obj.ApplicantSchecduleAvaliblity.ToDataTable();

                    //// convert source data to DataTable 
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
                        //Obj.ApplicantSchecduleAvaliblity[0].ASA_Action = 'I';

                    }

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
                    //var UT_ApplicantSchecduleAvaliblity = new SqlParameter("@UT_ApplicantSchecduleAvaliblity", SqlDbType.Structured);
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

                    //info for ApplicantLicenseHealdTable
                    //UT_ApplicantSchecduleAvaliblity.Value = ApplicantSchecduleAvaliblityTable;
                    //UT_ApplicantSchecduleAvaliblity.TypeName = "[dbo].[UT_ApplicantSchecduleAvaliblity]";

                    context.Database.ExecuteSqlCommand("exec [dbo].[spSetApplicantAllDetails] @Action, @UT_ApplicantPersonalInfo, @UT_ApplicantAddress,@UT_ApplicantContactInfo,@UT_ApplicantAdditionalInfo,@UT_AplicantAcadmicDetails,@UT_ApplicantBackgroundHistory,@UT_ApplicantPositionTitle,@UT_ApplicantAccidentRecord,@UT_ApplicantTrafficConvictions,@UT_ApplicantVehiclesOperated,@UT_ApplicantLicenseHeald",
                                                                                                Action, UT_ApplicantPersonalInfo,UT_ApplicantAddress,UT_ApplicantContactInfo, UT_ApplicantAdditionalInfo, UT_AplicantAcadmicDetails, UT_ApplicantBackgroundHistory, UT_ApplicantPositionTitle, UT_ApplicantAccidentRecord, UT_ApplicantTrafficConvictions, UT_ApplicantVehiclesOperated, UT_ApplicantLicenseHeald
                    );
                    // context.Database.ExecuteSqlCommand("exec [dbo].[spSetApplicantAllDetails] @Action, @UT_ApplicantPersonalInfo,@UT_ApplicantAddress,@UT_ApplicantContactInfo, @UT_ApplicantAdditionalInfo, @UT_AplicantAcadmicDetails, @UT_ApplicantBackgroundHistory, @UT_ApplicantPositionTitle, @UT_ApplicantAccidentRecord, @UT_ApplicantTrafficConvictions, @UT_ApplicantVehiclesOperated, @UT_ApplicantLicenseHeald, @UT_ApplicantSchecduleAvaliblity",
                    //Action, UT_ApplicantPersonalInfo,
                    //UT_ApplicantAddress, UT_ApplicantContactInfo, UT_ApplicantAdditionalInfo,
                    //UT_AplicantAcadmicDetails, UT_ApplicantBackgroundHistory,
                    //UT_ApplicantPositionTitle, UT_ApplicantAccidentRecord,
                    //UT_ApplicantTrafficConvictions, UT_ApplicantVehiclesOperated,
                    //UT_ApplicantLicenseHeald, UT_ApplicantSchecduleAvaliblity
                    //);
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
        #endregion Save Applicant
    }
}
