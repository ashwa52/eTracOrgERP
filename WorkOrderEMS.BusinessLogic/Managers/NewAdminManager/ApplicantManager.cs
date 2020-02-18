using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
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
                    //// convert source data to DataTable 
                    if (Obj.ApplicantPersonalInfo != null)
                    {
                        //var Date = new SqlParameter("@Date", System.Data.SqlDbType.DateTime2);
                        Obj.ApplicantPersonalInfo[0].API_Action = 'I';
                    }
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
                    System.Data.DataTable ApplicantSchecduleAvaliblityTable = Obj.ApplicantSchecduleAvaliblity.ToDataTable();
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

                    //info for ApplicantLicenseHealdTable
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

        #endregion Save Applicant
        #region Background Check
        public EmployeeVIewModel GetApplicantByApplicantId(long ApplicantId)
        {
            var lst = new EmployeeVIewModel();
            try
            {
                var getDetails = _db.spGetApplicantAllDetails(ApplicantId).Select(x => new EmployeeVIewModel()
                {
                    FirstName = x.API_FirstName,
                    MiddleName = x.API_MidName,
                    LastName = x.API_LastName,
                    City = x.AAD_City,
                    State = x.AAD_State,
                    Zip = x.AAD_Zip,
                    StreetAddress = x.APA_StreetAddress,
                    SocialSecurityNumber = x.API_SSN,
                    Phone = x.ACI_PhoneNo,
                    LicenseNumber = x.ALH_LicenceNumber,
                    //Dob = x
                }).FirstOrDefault();
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContactListModel GetContactListByApplicantId(long ApplicantId)", "Exception While getting the list of contact details.", ApplicantId);
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
        public bool SendApplicantInfoForBackgrounddCheck(EmployeeVIewModel model)
        {
            bool isSentForBackground = false;
            try
            {
                if (model != null && model.ApplicantId > 0)
                {
                    var sentForBackgroudCheck = _db.spSetApplicantStatus(model.ApplicantId, ApplicantStatus.F, ApplicantStatus.S);
                    #region Create PO
                    var poNum = _db.spGetPONumber().FirstOrDefault();
                    var poNumber = "PO" + poNum.ToString();
                    var POType = Convert.ToInt64(Helper.POType.NormalPO);
                    var company = _db.Companies.Where(x => x.CMP_NameLegal == VendorName.BackgroundCheck).FirstOrDefault();
                    var getHRDetail = _db.JobPostings.Join(_db.Applicants, jp => jp.JPS_JobPostingId, ap => ap.APT_JobPostingId, (jp, ap) => new { jp, ap }).
                                      Where(x => x.ap.APT_ApplicantId == model.ApplicantId).FirstOrDefault();
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
                            objEmailHelper.UserName = model.FirstName + " " + model.LastName;
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
            try
            {
                var action = model.I9F_Id == null ? "I" : "U";
                if (UserId > 0 && ApplicantId > 0 && model != null && model.I9F_Id == null)
                {
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
                               model.I9F_Sec3_C_DateOfEmployerOrAuthorizedSign,model.I9F_Sec3_C_NameOfEmployerOrAuthorized,model.I9F_Date,"Y");
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
                               model.I9F_Sec3_C_DateOfEmployerOrAuthorizedSign, model.I9F_Sec3_C_NameOfEmployerOrAuthorized, model.I9F_Date, "Y");
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
    }
}