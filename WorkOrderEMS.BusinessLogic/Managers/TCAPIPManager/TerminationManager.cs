using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;
using System.Data.Entity.Core.Objects;

namespace WorkOrderEMS.BusinessLogic
{
    public class TerminationManager : ITerminationManager
    {
        NotificationManager NotificationObj = new NotificationManager();

          workorderEMSEntities _db = new workorderEMSEntities();
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
       
        /// <summary>
        /// Logic to get Termination Employee Details
        /// Created by: Rajat Toppo
        /// Date:15-03-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public TerminationModel TerminationFormEmployeeDetails(string EmpId)
        {
            try
            {
                return _db.spGetOrgnizationCommonview(EmpId).Select(x => new TerminationModel() { 
                     EmpId = x.EMP_EmployeeID,
                     Emp_Name = x.EmployeeName,
                     ManagerId = x.EMP_ManagerId,
                     IsExempt = x.EMP_IsExempt,
                     Manager_Name = x.ManagerName



                }).FirstOrDefault();
            }
            catch(Exception ex)
            {

                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TerminationModel GetTerminationDetails(string EmpId)", "Exception While getting termination details by employee Id", EmpId);
                throw;

            }

        }

        public TerminationModel EmployeeTerminationFormAlldetails(string EmpId)
        {
            try
            {
                var TDetails = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                return _db.spGetOrgnizationCommonview(EmpId).Select(x => new TerminationModel()
               {
                    EmpId = x.EMP_EmployeeID,
                    Emp_Name = x.EmployeeName,
                    ManagerId = x.EMP_ManagerId,
                    Last_Day_Worked = TDetails.TMN_LastWorkingDay,
                    Termination_Date = TDetails.TMN_Date,
                    Reason_For_Leaving = TDetails.TMN_TerminationReason,
                    detailed_Expalnation = TDetails.TMN_TerminationDiscription,
                    Re_Hire = TDetails.TMN_RehireRecommended,
                    Manager_Name = x.ManagerName



                }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TerminationModel EmployeeTerminationFormAlldetails(string EmpId)", "Exception While getting termination details by employee Id", EmpId);
                throw;
            }
        }

        /// <summary>
        /// Logic to Set Termination Form Details
        /// Created by: Rajat Toppo
        /// Date: 17-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SaveTerminationFormDetails(TerminationModel Obj)
        {
            bool isSaved = false;
            try
            {
                if(Obj != null)
                {
                    _db.spSetTerminationForm("I",null,Obj.EmpId,Obj.Manager_Name, Obj.Termination_Date, Obj.Last_Day_Worked, Obj.Reason_For_Leaving, Obj.detailed_Expalnation,Obj.Re_Hire,Obj.HR_Decision,Obj.IsSeverence,Obj.LengthOfSeverence,Obj.SeverenceApproval,Obj.WitnessedId,"");
                    isSaved = true;
                }
               else
                {
                    isSaved = false;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveTerminationFormDetails(TerminationModel Obj)", "Exception While saving termination form by employeeid", Obj.EmpId);
                throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Sending Serverance agreement email to employee
        /// created by: Rajat Toppo
        /// Date: 30-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SendServeranceEmail(TerminationModel Obj, long LoginEmployeeId)
        {
            #region Email
            var objEmailLogRepository = new EmailLogRepository();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            var objTemplateModel = new TemplateModel();
            //var userData = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == Obj.ManagerId).FirstOrDefault();
            if (Obj != null)
            {
                bool IsSent = false;
                var objEmailHelper = new EmailHelper();
                objEmailHelper.Name = Obj.Emp_Name;
                objEmailHelper.ManagerName = Obj.Manager_Name;
                objEmailHelper.EmployeeId = Obj.EmpId;
                objEmailHelper.LastDayWorked = Obj.Last_Day_Worked;
                objEmailHelper.TerminationDate = Obj.Termination_Date;
                objEmailHelper.ReasonForLeaving = Obj.Reason_For_Leaving;
                objEmailHelper.DetailedExplanation = Obj.detailed_Expalnation;
                objEmailHelper.FinalIncidentTermination = Obj.Final_Incident_Termination;
                objEmailHelper.ItemsOwnedByEmployee = Obj.Items_Owned_ByEmployee;
                objEmailHelper.ItemListCost = Obj.ItemsList_Cost;
                objEmailHelper.ReHire = Obj.Re_Hire;
                objEmailHelper.HrDecision = Obj.HR_Decision;
                objEmailHelper.IsServerance = Obj.IsSeverence;
                objEmailHelper.emailid = "rajat99938@gmail.com";


                objEmailHelper.MailType = "EMPLOYEESERVERANCE";
                
                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                IsSent = objEmailHelper.SendEmailWithTemplate();
                if (IsSent == true)
                {
                    var objEmailog = new EmailLog();
                    try
                    {
                        objEmailog.CreatedBy = LoginEmployeeId;
                        objEmailog.CreatedDate = DateTime.UtcNow;
                        objEmailog.DeletedBy = null;
                        objEmailog.DeletedOn = null;
                        objEmailog.ModifiedBy = null;
                        objEmailog.ModifiedOn = null;
                        objEmailog.SentBy = LoginEmployeeId;
                        objEmailog.SentEmail = "rajat99938@gmail.com";
                        objEmailog.Subject = objEmailHelper.Subject;
                        objListEmailog.Add(objEmailog);
                      //userData.ALA_eMailId

                    }
                    catch (Exception ex)
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public bool SendServeranceEmail(TerminationModel Obj, long LoginEmployeeId)", "Exception sending serverance email by LoginId", LoginEmployeeId);
                        throw;
                    }


                }
                using (var context = new workorderEMSEntities())
                {
                    context.EmailLogs.AddRange(objListEmailog);
                    context.SaveChanges();
                }

            }
            return true;
            #endregion Email


        }

        /// <summary>
        /// logic to get Team Grid Data
        /// </summary>
        /// <param name="man_id"></param>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <param name="Tstatus"></param>
        /// <returns></returns>
        public List<TerminationListModel> TerminationlistTeamDetails(string LoginMangerId, DateTime? ToDate, DateTime? FromDate)
        {
            TerminationListModel obj = new TerminationListModel();
            try
            {
                return _db.spGetOrgnizationListview(LoginMangerId).Select(x => new TerminationListModel()
                {
                    emp_id = x.EMP_EmployeeID,
                    name = x.EMP_EmployeeID,
                    emp_photo = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                    seatTitle = x.JBT_JobTitle

                }).ToList();

                    //return _db.spGetPIP_Termination_CorrectiveAction(man_id, FromDate, ToDate).Select(x => new TerminationListModel()
                    //{
                    //    emp_id = x.EmployeeID,
                    //    name = x.EmployeeName,
                    //    emp_photo = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                    //    seatTitle = x.JBT_JobTitle,

                //}).ToList();

            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TerminationModel GetTerminationDetails(string EmpId)", "Exception While getting terminationlist details by manager Id", LoginMangerId);
                throw;

            }



        }

        /// <summary>
        /// Logic to get Finalize Grid Data
        /// </summary>
        /// <param name="man_id"></param>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <param name="Tstatus"></param>
        /// <returns></returns>
        public List<TerminationListModel> TerminationlistFinalizedetails(long man_id, DateTime? ToDate, DateTime? FromDate)
        {
            TerminationListModel obj = new TerminationListModel();
            try
            {
               
                return _db.spGetPIP_Termination_CorrectiveAction(man_id, FromDate, ToDate).Select(x => new TerminationListModel()
                {
                            emp_id = x.EmployeeID,
                            name = x.EmployeeName,
                            emp_photo = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                            status = x.TStatus,
                            seatTitle = x.JBT_JobTitle,
                            //Etype = x.type
                            
                }).ToList();

                   
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TerminationModel GetTerminationDetails(string EmpId)", "Exception While getting terminationlist details by manager Id", man_id);
                throw;

            }
        }

        /// <summary>
        /// Logic to get Pending Grid Data
        /// </summary>
        /// <param name="man_id"></param>
        /// <param name="ToDate"></param>
        /// <param name="FromDate"></param>
        /// <param name="Tstatus"></param>
        /// <returns></returns>
        public List<TerminationListModel> TerminationlistPendingdetails(long man_id, DateTime? ToDate, DateTime? FromDate)
        {
            TerminationListModel obj = new TerminationListModel();
            try
            {
                
                
                  return _db.spGetPIP_Termination_CorrectiveAction(man_id, FromDate, ToDate).Select(x => new TerminationListModel()
                  {
                        emp_id = x.EmployeeID,
                        name = x.EmployeeName,
                        emp_photo = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,                       
                        seatTitle = x.JBT_JobTitle,
                        //DateIssued
                        Etype = x.type,
                        status = x.TStatus
                        

                  }).ToList();

                     
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public TerminationModel GetTerminationDetails(string EmpId)", "Exception While getting terminationlist details by manager Id", man_id);
                throw;

            }
        }

        /// <summary>
        /// Saving Termination Employee Witness Details
        /// Created By: Rajat Toppo
        /// Date:27-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="TEmpId"></param>
        /// <returns></returns>
        public bool SaveTerminationWitnessDetails(WitnessModel Obj,string TEmpId)
        {
            bool isSaved = false;
            try
            {
                var TerminationDetails = _db.TerminationForms.Where(x => x.TMN_EmployeeId == TEmpId).FirstOrDefault();
                if (Obj != null)
                {
                     var wTS_Id = new ObjectParameter("wTS_Id", typeof(int));
                    var SaveWitness = _db.spSetWitness("I", TerminationDetails.TMN_Id, Obj.IsEliteEmployee, Obj.WitnessName, null, Obj.Cposition, Obj.CompanyTheyWorkFor, "Y", wTS_Id);
                    isSaved = true;
                }
               
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveTerminationWitnessDetails(WitnessModel Obj,string TEmpId)", "Exception While saving witness details by EmployeeId", TEmpId);
                throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Get Termination Employee Details for Grid
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public EmployeeDetailsModel TerminationEmployeeDetails(string EmpId)
        {
            return _db.spGetOrgnizationCommonview(EmpId).Select(x => new EmployeeDetailsModel()
            {


                empid = x.EMP_EmployeeID,
                employee_name = x.EmployeeName,
                employee_photo = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                OperationHead = x.DepartmentName,
                man_id = x.EMP_ManagerId,
                
                
                

            }).FirstOrDefault();
        }

        /// <summary>
        /// Logic to get Termination Employee Asset Details
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public List<EmployeeAssetDetails> TerminationEmployeeAssetDetails(string EmpId)
        {
            try
            {

                return _db.spGetAssetAllocation(EmpId).Select(x => new EmployeeAssetDetails()
                {
                    AssetRowId= x.ATA_Id,
                    AssetDetails = x.ATA_Type,
                    AssetName = x.ATA_AssetName,
                    ReturnDate = x.ATA_ReturnDate,
                    AssetStatus = x.ATA_ReturnStatus

                }).ToList();
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public List<AssetModel> GetTerminationEmployeeAssetDetails(string EmpId)", "Exception While getting Assetlist details by Employee Id", EmpId);
                throw;
            }
        }

        public TerminationModel TerminationEmployeeDetailsforReview(string EmpId)
        {
            try
            {
                var ReviewTlist = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                var ReviewName = _db.spGetOrgnizationCommonview(EmpId).Select(x => new TerminationModel()
                {

                    Emp_Name = x.EmployeeName

                }).FirstOrDefault();
                return new TerminationModel()
                {
                    EmpId = ReviewTlist.TMN_EmployeeId,
                    Emp_Name = ReviewName.Emp_Name,
                    HrDenyReason = ReviewTlist.TMN_HRDenyReason,
                    HR_Decision = ReviewTlist.TMN_HRApproal,
                    HrDenyComment = ReviewTlist.TMN_HRDenyComment

                };

            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public TerminationModel GetTerminationReviewEmployeeDetails(string EmpId)", "Exception While getting Review details by Employee Id", EmpId);
                throw;
            }
        }

        /// <summary>
        /// Set Hr Deny Reason and comment for Termination Deny
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public bool SaveHrDenyReasonandComment(WitnessModel obj, string EmpId)
        {
           bool issaved = false;
            try
            { 
                if(EmpId != null)
                {
                    var TerminationId = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                     _db.spSetTermination_HRApproval(TerminationId.TMN_Id, "N", obj.TerminationDenyModel.ReasonForDenial, obj.TerminationDenyModel.AdditionalReasonComments);
                    _db.spSetTermination_SeveranceApproval(TerminationId.TMN_Id, "W");//Code needed to be changed
                    issaved = true;
                }
              
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveHrDenyReasonandComment(WitnessModel obj, string EmpId)", "Exception While saving Hr deny reason by EmployeeId", EmpId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Set HR Approval and saving Notification for same
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveNotificationForHrApproval(string EmpId, string LoginEmployeeId)
        {

            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if (EmpId != null)
                {
                    var TerminationId = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                    _db.spSetTermination_HRApproval(TerminationId.TMN_Id, "Y","","");
                    _db.spSetTermination_SeveranceApproval(TerminationId.TMN_Id,"W");//code needed to be changed

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(EmpId).FirstOrDefault();

                    objNotification.Message = DarMessage.TerminationApprove(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.TerminateApproveDeny;
                    objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                    objNotification.CreatedByUser = LoginEmployeeId;
                    objNotification.CreatedByIsWorkable = false;
                    objNotification.AssignToUser = EmployeeDetails.EMP_ManagerId;
                    objNotification.AssignToIsWorkable = true;
                    objNotification.Priority = Priority.High;

                    NotificationObj.SaveNotification(objNotification);
                    issaved = true;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveNotificationForHrApproval(string EmpId, string LoginEmployeeId)", "Exception While saving Notification by hr for approval by LoginId", LoginEmployeeId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving Notification for deny by HR
        /// Created by: Rajat Toppo
        /// Date:17-04-2020s
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveNotificationForHrDenial(WitnessModel obj, string EmpId, string LoginEmployeeId)
        {
            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if(EmpId != null)
                {

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(EmpId).FirstOrDefault();

                    objNotification.Message = DarMessage.TerminationDeny(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.TerminateApproveDeny;
                    objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                    objNotification.CreatedByUser = LoginEmployeeId;
                    objNotification.CreatedByIsWorkable = false;
                    objNotification.AssignToUser = EmployeeDetails.EMP_ManagerId;
                    objNotification.AssignToIsWorkable = true;
                    objNotification.Priority = Priority.High;

                    NotificationObj.SaveNotification(objNotification);
                    issaved = true;
                }
              
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveNotificationForHrDenial(WitnessModel obj, string EmpId, string LoginEmployeeId)", "Exception While saving Notification for hr denial by LoginId", LoginEmployeeId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving Notification for AssetList by Manager
        /// Created by: Rajat Toppo
        /// Date: 14-04-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveAssetStatusReturnDate(List<EmployeeAssetDetails> obj,string EmpId, string LoginEmployeeId)
        {
            bool issaved = false;
            try
            {

                var objNotification = new NotificationDetailModel();
                List<AssetAllocation> lostlist = new List<AssetAllocation>();
                var EmployeeDetails = _db.spGetOrgnizationCommonview(EmpId).FirstOrDefault();
                //Financial Department
                var getDept = _db.spGetDepartmentEmail(10004).ToList();
                if (obj != null) {
                    
                    foreach (var item in obj)
                    {
                        var details = _db.AssetAllocations.Where(x => x.ATA_Id == item.AssetRowId).FirstOrDefault();
                        var save = _db.spSetAssetAllocation("U",item.AssetRowId,details.ATA_EMP_EmployeeId, details.ATA_Type, details.ATA_AssetName, details.ATA_AssetDescription, details.ATA_Make, details.ATA_Model, details.ATA_SerialNumber, details.ATA_Login, details.ATA_Password, details.ATA_AssignDate, details.ATA_ReturnAcceptBy, item.ReturnDate,item.AssetStatus,"");
                    }

                    
                    if(getDept.Count() > 0)
                    {
                        foreach (var item in getDept)
                        {
                            objNotification.Message = DarMessage.AssetLost(EmployeeDetails.EmployeeName);
                            objNotification.Module = ModuleSubModule.ePeople;
                            objNotification.SubModule = ModuleSubModule.TerminateEmployee;
                            objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                            objNotification.CreatedByUser =  LoginEmployeeId;
                            objNotification.CreatedByIsWorkable = false;
                            objNotification.AssignToUser = item.EmployeeId;
                            objNotification.AssignToIsWorkable = true;
                            objNotification.Priority = Priority.High;

                            NotificationObj.SaveNotification(objNotification);
                        }
                    }
                    
                    issaved = true;
                    
                }
               

            }
            
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveAssetStatusReturnDate(List<EmployeeAssetDetails> obj,string EmpId, string LoginEmployeeId)", "Exception While saving Asset Return status date by employeeid", EmpId);
                throw;
            }
            return issaved;
        }
       

        /// <summary>
        /// Sending Termination Form Email
        ///created by: Rajat Toppo
        /// Date: 30-03-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SendTerminationFormEmail(TerminationModel Obj, long LoginEmployeeId)
        {
            #region Email
            var objEmailLogRepository = new EmailLogRepository();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            var objTemplateModel = new TemplateModel();
            //var userData = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == Obj.ManagerId).FirstOrDefault();
            var EmployeeEmail = _db.spGetEmployeePersonalInfo(Obj.EmpId).FirstOrDefault();

            var DepartmentMail = _db.spGetDepartmentEmail(10002).ToList();

             if (Obj != null)
            {
                bool IsSent = false;
                var objEmailHelper = new EmailHelper();
                objEmailHelper.Name = Obj.Emp_Name;
                objEmailHelper.ManagerName = Obj.Manager_Name;
                objEmailHelper.EmployeeId = Obj.EmpId;
                objEmailHelper.LastDayWorked = Obj.Last_Day_Worked;
                objEmailHelper.TerminationDate = Obj.Termination_Date;
                objEmailHelper.ReasonForLeaving = Obj.Reason_For_Leaving;
                objEmailHelper.DetailedExplanation = Obj.detailed_Expalnation;
                objEmailHelper.FinalIncidentTermination = Obj.Final_Incident_Termination;
                objEmailHelper.ItemsOwnedByEmployee = Obj.Items_Owned_ByEmployee;
                objEmailHelper.ItemListCost = Obj.ItemsList_Cost;
                objEmailHelper.ReHire = Obj.Re_Hire;
                objEmailHelper.HrDecision = Obj.HR_Decision;
                objEmailHelper.IsServerance = Obj.IsSeverence;
                //Change 
                objEmailHelper.emailid = "rajat99938@gmail.com";


                objEmailHelper.MailType = "EMPLOYEETERMINATION";
                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                IsSent = objEmailHelper.SendEmailWithTemplate();

                if (IsSent == true)
                {
                    var objEmailog = new EmailLog();
                    try
                    {
                        objEmailog.CreatedBy = LoginEmployeeId;
                        objEmailog.CreatedDate = DateTime.UtcNow;
                        objEmailog.DeletedBy = null;
                        objEmailog.DeletedOn = null;
                        objEmailog.ModifiedBy = null;
                        objEmailog.ModifiedOn = null;
                        objEmailog.SentBy = LoginEmployeeId;
                        objEmailog.SentEmail = EmployeeEmail.EMP_Email;
                        objEmailog.Subject = objEmailHelper.Subject;
                        objListEmailog.Add(objEmailog);

                        

                    }
                    catch (Exception ex)
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendTerminationFormEmail(TerminationModel Obj, long LoginEmployeeId)", "Exception While sending email by by LoginId", LoginEmployeeId);
                        throw;
                    }


                }
                using (var context = new workorderEMSEntities())
                {
                    context.EmailLogs.AddRange(objListEmailog);
                    context.SaveChanges();
                }

                foreach (var email in DepartmentMail)
                {
                    
                    objEmailHelper.Name = Obj.Emp_Name;
                    objEmailHelper.ManagerName = Obj.Manager_Name;
                    objEmailHelper.EmployeeId = Obj.EmpId;
                    objEmailHelper.LastDayWorked = Obj.Last_Day_Worked;
                    objEmailHelper.TerminationDate = Obj.Termination_Date;
                    objEmailHelper.ReasonForLeaving = Obj.Reason_For_Leaving;
                    objEmailHelper.DetailedExplanation = Obj.detailed_Expalnation;
                    objEmailHelper.FinalIncidentTermination = Obj.Final_Incident_Termination;
                    objEmailHelper.ItemsOwnedByEmployee = Obj.Items_Owned_ByEmployee;
                    objEmailHelper.ItemListCost = Obj.ItemsList_Cost;
                    objEmailHelper.ReHire = Obj.Re_Hire;
                    objEmailHelper.HrDecision = Obj.HR_Decision;
                    objEmailHelper.IsServerance = Obj.IsSeverence;



                    objEmailHelper.MailType = "EMPLOYEETERMINATION";
                    objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                    IsSent = objEmailHelper.SendEmailWithTemplate();
                    if (IsSent == true)
                    {
                        var objEmailog = new EmailLog();
                        try
                        {
                            objEmailog.CreatedBy = LoginEmployeeId;
                            objEmailog.CreatedDate = DateTime.UtcNow;
                            objEmailog.DeletedBy = null;
                            objEmailog.DeletedOn = null;
                            objEmailog.ModifiedBy = null;
                            objEmailog.ModifiedOn = null;
                            objEmailog.SentBy = LoginEmployeeId;
                            objEmailog.SentEmail = email.eMail;
                            objEmailog.Subject = objEmailHelper.Subject;
                            objListEmailog.Add(objEmailog);



                        }
                        catch (Exception ex)
                        {
                            Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendTerminationFormEmail(TerminationModel Obj, long LoginEmployeeId)", "Exception While sending email by by LoginId", LoginEmployeeId);
                            throw;
                        }


                    }
                    using (var context = new workorderEMSEntities())
                    {
                        context.EmailLogs.AddRange(objListEmailog);
                        context.SaveChanges();
                    }
                }
            }
            return true;
            #endregion Email

        }

        /// <summary>
        /// Sending Email of Asset list
        /// Created by: Rajat Toppo
        /// Date: 16-04-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj1"></param>
        /// <returns></returns>
        public bool SendAssetDetailsEmail(EmployeeDetailsModel obj, List<EmployeeAssetDetails> obj1)
        {
            #region Email
            var objEmailLogRepository = new EmailLogRepository();
            var objEmailReturn = new List<EmailToManagerModel>();
            var objListEmailog = new List<EmailLog>();
            var objTemplateModel = new TemplateModel();
            var LoginuserData = _db.ApplicantLoginAccesses.Where(x => x.ALA_LoginId == obj.man_id).FirstOrDefault();
            var userData = _db.UserRegistrations.Where(x => x.EmployeeID == obj.empid).FirstOrDefault();
            
               if (userData != null)
            {
                bool IsSent = false;
                var objEmailHelper = new EmailHelper();
                objEmailHelper.Name = obj.employee_name;
                objEmailHelper.EmployeeId = obj.empid;
                string tableBody = "<table id='detailsTable' style='width: 600px; border: 1px solid #b9acac;margin-left:27px;'>< tr bgcolor = '#4da6ff' style = 'border: 1px solid #dddddd;' >< td >< b > Asset Name </ b ></ td > < td > < b > Asset Details </ b > </ td > < td > < b > Return Status </ b > </ td > < td > < b > Return Date </ b > </ td ></ tr > ";
                foreach (var item in obj1)
                {

                    tableBody += "< tr style = 'border: 1px solid #dddddd;' ><td> " + item.AssetName + " </ td >< td > " + item.AssetDetails + " </ td > < td > " + item.AssetStatus + " </ td > < td > " + item.ReturnDate + " </ td > </ tr > ";
                     
                     
                 }
                tableBody += "</ table > ";
               objEmailHelper.TableBody = tableBody;


                objEmailHelper.MailType = "ASSETSINFORMATION";
                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                IsSent = objEmailHelper.SendEmailWithTemplate();
                if (IsSent == true)
                {
                    var objEmailog = new EmailLog();
                    try
                    {
                        
                        objEmailog.CreatedBy = LoginuserData.ALA_UserId;
                        objEmailog.CreatedDate = DateTime.UtcNow;
                        objEmailog.DeletedBy = null;
                        objEmailog.DeletedOn = null;
                        objEmailog.ModifiedBy = null;
                        objEmailog.ModifiedOn = null;
                        objEmailog.SentBy = LoginuserData.ALA_UserId;
                        objEmailog.SentEmail = userData.UserEmail;
                        objEmailog.Subject = objEmailHelper.Subject;
                        objListEmailog.Add(objEmailog);
                        
                        //userData.ALA_eMailId

                    }
                    catch (Exception ex)
                    {
                        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendAssetDetailsEmail(EmployeeDetailsModel obj, List<EmployeeAssetDetails> obj1)", "Exception While sending email by by LoginId", LoginuserData.ALA_UserId);
                        throw;
                    }


                }
                using (var context = new workorderEMSEntities())
                {
                    context.EmailLogs.AddRange(objListEmailog);
                    context.SaveChanges();
                }

            }
            return true;
            #endregion Email


        }

    }


   




}
