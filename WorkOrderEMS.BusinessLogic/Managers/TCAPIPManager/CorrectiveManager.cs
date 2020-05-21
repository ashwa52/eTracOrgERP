using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;


namespace WorkOrderEMS.BusinessLogic
{
    
    public class CorrectiveManager : ICorrectiveManager
    {

        workorderEMSEntities _db = new workorderEMSEntities();
        NotificationManager NotificationObj = new NotificationManager();

        /// <summary>
        /// Created by: Rajat Toppo
        /// Created For: Logic to get employee details from database
        /// Date: 13-03-2020
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public CorrectiveActionModel EmployeeDetailsForCorrectiveActionForm(string empid)
        {
            try
            {
                return _db.spGetOrgnizationCommonview(empid).Select(x => new CorrectiveActionModel()
                {
                    emp_id = x.EMP_EmployeeID,
                    emp_name = x.EmployeeName,
                    Job_Title = x.JBT_JobTitle,
                    manager_name = x.ManagerName,
                    manager_id = x.EMP_ManagerId,
                    IsExempt = x.EMP_IsExempt    

                }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CorrectiveActionModel GetTerminationDetails(string EmpId)", "Exception While getting termination details by employee Id", empid);
                throw;
            }
        }

        /// <summary>
        /// Saving Notification For meeting end by manager to employee
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveNotificationforMeetingByManagerToEmployee(CorrectiveActionModel obj, string LoginEmployeeId)
        {
            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if (LoginEmployeeId != null)
                {
                    

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(obj.emp_id).FirstOrDefault();

                    objNotification.Message = DarMessage.CorrectiveActionMeeting(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.CorrectiveAction;
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveNotificationforMeetingByManagerToEmployee(CorrectiveActionModel obj, string LoginEmployeeId)", "Exception While Saving notification by manager for meeting by manager id", LoginEmployeeId);
                throw;
            }
            return issaved;
        }


        /// <summary>
        /// For manager and employee review during meeting
        /// </summary>
        /// <param name="EMPId"></param>
        /// <returns></returns>
        public CorrectiveActionModel CorrectiveActionEmployeeFormDetailsForReview(string EMPId)
        {
            try
            {
                var cad = _db.spGetCorrectiveActionForm().Where(x => x.CTA_EmployeeId == EMPId).FirstOrDefault();

                //var correctiveactiondetails = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == EMPId).FirstOrDefault();
                return _db.spGetOrgnizationCommonview(EMPId).Select(x => new CorrectiveActionModel()
                {
                    emp_id = x.EMP_EmployeeID,
                    emp_name = x.EmployeeName,
                    manager_name = x.ManagerName,
                    manager_id = x.EMP_ManagerId,
                    Job_Title = x.JBT_JobTitle,
                    Level_ofCorrectiveAction = cad.CTA_LevelOfCorrectiveAction,
                    FDate = cad.CTA_IncidentDate,
                    FTime = cad.CTA_IncidentTime,
                    FType = cad.CTA_TypeOfIncident,
                    Employee_Explanation = cad.CTA_EmployeesExplanation,
                    Policy_Violation = cad.CTA_PolicyViolation,
                    Expectation_CorrectiveActionPlan = cad.CTA_ExpectationCorrectiveActionPlan,
                    ActionTaken = cad.CTA_ActionTaken,
                    Next_Action = cad.CTA_NextActionStep,
                    CTA_Date = cad.CTA_Date,
                    HR_Approval = cad.CTA_HRApproal,
                    Is_Active = cad.CTA_IsActive

                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CorrectiveActionModel GetCorrectiveActionFormDetailsForReview(string EMPId)", "Exception While getting Corrective Action details by employee Id", EMPId);
                throw;
            }
        }

        /// <summary>
        /// Created By: Rajat Toppo
        /// Created For: Logic to save employee details into database
        /// Date: 13-03-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveEmployeeCorrectiveActionDetails(CorrectiveActionModel obj)
        {
            bool isSaved = false;
            try
            {
                if(obj != null)
                {   
                    //data which are not avialable for now are set null
                    _db.spSetCorrectiveActionForm("I", null, obj.emp_id, obj.manager_id,obj.Level_ofCorrectiveAction,obj.FDate,obj.FTime,obj.FType,obj.Employee_Explanation,obj.Policy_Violation,obj.Expectation_CorrectiveActionPlan,obj.ActionTaken,obj.Next_Action,obj.HR_Approval,obj.Is_Guilty,null,null);
                    isSaved = true;
                    
                }
                else
                {
                    isSaved = false;
                }
                
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveEmployeeCorrectiveActionDetails(CorrectiveActionModel obj)", "Exception While Saving Corrective Action For ", obj.emp_id);
                throw;
            }
            return isSaved;
        }
           
        /// <summary>
        ///Saving Corrective Action Notification by Manager of Employee To HR
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveCorrectiveActionNotificationbyManager(CorrectiveActionModel obj, string LoginEmployeeId)
        {
            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if (LoginEmployeeId != null)
                {
                    //var TerminationId = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                    //_db.spSetTermination_HRApproval(TerminationId.TMN_Id, "Y", "", "");

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(obj.emp_id).FirstOrDefault();

                    objNotification.Message = DarMessage.CorrectiveActionRequest(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.CorrectiveAction;
                    objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                    objNotification.CreatedByUser = LoginEmployeeId;
                    objNotification.CreatedByIsWorkable = false;
                    objNotification.AssignToUser = EmployeeDetails.EMP_ManagerId;//change it to hr
                    objNotification.AssignToIsWorkable = true;
                    objNotification.Priority = Priority.High;

                    NotificationObj.SaveNotification(objNotification);
                    issaved = true;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCorrectiveActionNotificationbyManager(CorrectiveActionModel obj, string LoginEmployeeId)", "Exception While Saving notification by manager to hr by manager id", LoginEmployeeId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving Coorective Action hr approval and hr notification
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveCorrectiveActionNotificationForHrApproval(string EmpId, string LoginEmployeeId)
        {

            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if (EmpId != null)
                {
                    var CAId = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == EmpId).FirstOrDefault();
                    _db.spSetCorrectiveAction_HRApproval(CAId.CTA_Id, "Y", "", "");
                    _db.spSetCorrectiveActionForm("U", CAId.CTA_Id, CAId.CTA_EmployeeId, CAId.CTA_ManagerId, CAId.CTA_LevelOfCorrectiveAction, CAId.CTA_IncidentDate, CAId.CTA_IncidentTime, CAId.CTA_TypeOfIncident, CAId.CTA_EmployeesExplanation, CAId.CTA_PolicyViolation, CAId.CTA_ExpectationCorrectiveActionPlan, CAId.CTA_ActionTaken, CAId.CTA_NextActionStep, CAId.CTA_HRApproal, CAId.CTA_IsGuilty, CAId.CTA_Date,"A");
                    

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(EmpId).FirstOrDefault();

                    objNotification.Message = DarMessage.CorrectiveActionApprove(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.CorrectiveAction;
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCorrectiveActionNotificationForHrApproval(string EmpId, string LoginEmployeeId)", "Exception While Saving Hr Approval by employee id", EmpId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving Corrective action Hr denial and notification
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool SaveCorrectiveActionNotificationForHrDenial(WitnessModel obj, string EmpId, string LoginEmployeeId)
        {
            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                if (EmpId != null)
                {
                    //var TerminationId = _db.TerminationForms.Where(x => x.TMN_EmployeeId == EmpId).FirstOrDefault();
                    //_db.spSetTermination_HRApproval(TerminationId.TMN_Id, "Y", "", "");

                    var EmployeeDetails = _db.spGetOrgnizationCommonview(EmpId).FirstOrDefault();

                    objNotification.Message = DarMessage.CorrectiveActionDeny(EmployeeDetails.EmployeeName);
                    objNotification.Module = ModuleSubModule.ePeople;
                    objNotification.SubModule = ModuleSubModule.CorrectiveAction;
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCorrectiveActionNotificationForHrApproval(string EmpId, string LoginEmployeeId)", "Exception While Saving Hr Approval by employee id", EmpId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving Corrective Action HR reason for deny and comment
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public bool CorrectiveActionHrDenyReasonandComment(WitnessModel obj, string EmpId)
        {
            bool issaved = false;
            try
            {
                if (EmpId != null)
                {
                    var CorrectiveActionId = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == EmpId).FirstOrDefault();
                    _db.spSetCorrectiveAction_HRApproval(CorrectiveActionId.CTA_Id, "N", obj.TerminationDenyModel.ReasonForDenial, obj.TerminationDenyModel.AdditionalReasonComments);
                    issaved = true;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool CorrectiveActionHrDenyReasonandComment(WitnessModel obj, string EmpId)", "Exception While Saving Hr deny reason and comment by employee id", EmpId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Saving meeting Date time in Corrective Action Form
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveMeetingDateTimewithEmployeeId(CorrectiveActionModel obj)
        {
            bool issaved = false;
            try
            {
                var CAFormId = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == obj.emp_id).FirstOrDefault();
                _db.spSetMeetingDateTime(CAFormId.CTA_Id, obj.MeetingDateTime);
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveMeetingDateTimewithEmployeeId(CorrectiveActionModel obj)", "Exception While Saving meeting Date time using employee id",obj.emp_id);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Getting Corrective Action Employee Details for review
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public CorrectiveActionModel CorrectiveActionReviewEmployeeDetails(string EmpId)
        {
            try
            {
                var ReviewTlist = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == EmpId).FirstOrDefault();
                var ReviewName = _db.spGetOrgnizationCommonview(EmpId).Select(x => new CorrectiveActionModel()
                {

                    emp_name = x.EmployeeName

                }).FirstOrDefault();
                return new CorrectiveActionModel()
                {
                    emp_id = ReviewTlist.CTA_EmployeeId,
                    emp_name = ReviewName.emp_name,
                    HrDenyReason = ReviewTlist.CTA_HRDenyReason,
                    HR_Approval = ReviewTlist.CTA_HRApproal,
                    HrDenyComment = ReviewTlist.CTA_HRDenyComment

                };

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public CorrectiveActionModel GetCorrectiveActionReviewEmployeeDetails(string EmpId)", "Exception While getting Review details by Employee Id", EmpId);
                throw;
            }
        }

        /// <summary>
        /// Set IsActive B in Corrrective Action form
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SetIsActiveBCorrective(long UserId)
        {
            bool issaved = false;
            try
            {
                var GetEmpid = _db.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault();
                var ECAA = _db.spGetCorrectiveActionForm().Where(x => x.CTA_EmployeeId == GetEmpid.EmployeeID).FirstOrDefault();
                _db.spSetCorrectiveActionForm("U", ECAA.CTA_Id, ECAA.CTA_EmployeeId, ECAA.CTA_ManagerId, ECAA.CTA_LevelOfCorrectiveAction, ECAA.CTA_IncidentDate, ECAA.CTA_IncidentTime, ECAA.CTA_TypeOfIncident, ECAA.CTA_EmployeesExplanation, ECAA.CTA_PolicyViolation, ECAA.CTA_ExpectationCorrectiveActionPlan, ECAA.CTA_ActionTaken, ECAA.CTA_NextActionStep, ECAA.CTA_HRApproal, ECAA.CTA_IsGuilty, ECAA.CTA_Date, "B");
                issaved = true;
            }
            catch(Exception)
            {
                throw;   
            }
            return issaved;
        }

        /// <summary>
        /// Set IsActive C in Corrrective Action form
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool SetIsActiveCCorrective(long UserId)
        {
            bool issaved = false;
            try
            {
                var GetEmpid = _db.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault();
                var ECAD = _db.spGetCorrectiveActionForm().Where(x => x.CTA_EmployeeId == GetEmpid.EmployeeID).FirstOrDefault();
                _db.spSetCorrectiveActionForm("U", ECAD.CTA_Id, ECAD.CTA_EmployeeId, ECAD.CTA_ManagerId, ECAD.CTA_LevelOfCorrectiveAction, ECAD.CTA_IncidentDate, ECAD.CTA_IncidentTime, ECAD.CTA_TypeOfIncident, ECAD.CTA_EmployeesExplanation, ECAD.CTA_PolicyViolation, ECAD.CTA_ExpectationCorrectiveActionPlan, ECAD.CTA_ActionTaken, ECAD.CTA_NextActionStep, ECAD.CTA_HRApproal, ECAD.CTA_IsGuilty, ECAD.CTA_Date, "C");
                issaved = true;
            }
            catch (Exception)
            {
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// On click dispute button in Corrective action Form and saving notification for hr by employee
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <returns></returns>
        public bool NotificationToHRForEmployeeCorrectiveActionDispute(CorrectiveActionModel obj, string LoginEmployeeId)
        {
            var objNotification = new NotificationDetailModel();
            bool issaved = false;
            try
            {
                var EmployeeDetails = _db.spGetOrgnizationCommonview(obj.emp_id).FirstOrDefault();

                objNotification.Message = DarMessage.EmployeeCorrectiveActionDispute(EmployeeDetails.EmployeeName);
                objNotification.Module = ModuleSubModule.ePeople;
                objNotification.SubModule = ModuleSubModule.CorrectiveAction;
                objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                objNotification.CreatedByUser = LoginEmployeeId;
                objNotification.CreatedByIsWorkable = false;
                objNotification.AssignToUser = EmployeeDetails.EMP_ManagerId;//change it to hr
                objNotification.AssignToIsWorkable = true;
                objNotification.Priority = Priority.High;

                NotificationObj.SaveNotification(objNotification);
                issaved = true;
            }
            catch(Exception ex)
            {

            }
            return issaved;
        }

        /// <summary>
        /// sending meetime date and time to employee(Exempt) by manager 
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SendMeetingTimeToEmployeeByNotification(string EMPId, string LoginEmployeeId, WitnessModel obj)
        {
            var objNotification = new NotificationDetailModel();
            var DateTimeOne = obj.MeetingDateOne + " " + obj.MeetingTimeOne;
            var DateTimeTwo = obj.MeetingDateTwo + " " + obj.MeetingTimeTwo;
            var DateTimeThree = obj.MeetingDateThree + " " + obj.MeetingTimeThree;

            bool issaved = false;
            try
            {
                var EmployeeDetails = _db.spGetOrgnizationCommonview(EMPId).FirstOrDefault();

                objNotification.Message = DarMessage.CorrectiveActionMeetingDateTime(EmployeeDetails.ManagerName, DateTimeOne, DateTimeTwo, DateTimeThree);
                objNotification.Module = ModuleSubModule.CorrectiveAction;
                objNotification.SubModule = ModuleSubModule.ScheduleMeeting;
                objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                objNotification.CreatedByUser = LoginEmployeeId;
                objNotification.CreatedByIsWorkable = false;
                objNotification.AssignToUser = EmployeeDetails.EMP_EmployeeID;
                objNotification.AssignToIsWorkable = true;
                objNotification.Priority = Priority.High;

                NotificationObj.SaveNotification(objNotification);
                issaved = true;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SendMeetingTimeToEmployeeByNotification(string EMPId, string LoginEmployeeId, WitnessModel obj)", "Exception While while saving notification for meeting by employeeId", EMPId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// sending meetime date and time to employee(Non-Exempt) by manager 
        /// </summary>
        /// <param name="EMPId"></param>
        /// <param name="LoginEmployeeId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SendMeetingTimeToEmployeeByNotificationNonExempt(string EMPId, string LoginEmployeeId, WitnessModel obj)
        {
            var objNotification = new NotificationDetailModel();
            var DateTimeOne = obj.MeetingDateNonExempt + " " + obj.MeetingTimeNonExempt;
            
            bool issaved = false;
            try
            {
                var EmployeeDetails = _db.spGetOrgnizationCommonview(EMPId).FirstOrDefault();

                objNotification.Message = DarMessage.CorrectiveActionMeetingDateTime(EmployeeDetails.ManagerName, DateTimeOne);
                objNotification.Module = ModuleSubModule.CorrectiveAction;
                objNotification.SubModule = ModuleSubModule.ScheduleMeeting;
                objNotification.SubModuleId = EmployeeDetails.EMP_EmployeeID;
                objNotification.CreatedByUser = LoginEmployeeId;
                objNotification.CreatedByIsWorkable = false;
                objNotification.AssignToUser = EmployeeDetails.EMP_EmployeeID;
                objNotification.AssignToIsWorkable = true;
                objNotification.Priority = Priority.High;

                NotificationObj.SaveNotification(objNotification);
                issaved = true;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool SendMeetingTimeToEmployeeByNotificationNonExempt(string EMPId, string LoginEmployeeId, WitnessModel obj)", "Exception While while saving notification for meeting by employeeId", EMPId);
                throw;
            }
            return issaved;
        }

        /// <summary>
        /// Getting Corrective Action Form Details;
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public CorrectiveActionModel CorrectiveActionFormDetailsEmployee(long UserId)
        {
            try
            {
                var GetEmpid = _db.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault();
                var caad = _db.spGetCorrectiveActionForm().Where(x => x.CTA_EmployeeId == GetEmpid.EmployeeID).FirstOrDefault();
                if(caad != null)
                {
                    return _db.spGetOrgnizationCommonview(GetEmpid.EmployeeID).Select(x => new CorrectiveActionModel()
                    {
                        emp_id = x.EMP_EmployeeID,
                        emp_name = x.EmployeeName,
                        manager_name = x.ManagerName,
                        manager_id = x.EMP_ManagerId,
                        Job_Title = x.JBT_JobTitle,
                        Level_ofCorrectiveAction = caad.CTA_LevelOfCorrectiveAction,
                        FDate = caad.CTA_IncidentDate,
                        FTime = caad.CTA_IncidentTime,
                        FType = caad.CTA_TypeOfIncident,
                        Employee_Explanation = caad.CTA_EmployeesExplanation,
                        Policy_Violation = caad.CTA_PolicyViolation,
                        Expectation_CorrectiveActionPlan = caad.CTA_ExpectationCorrectiveActionPlan,
                        ActionTaken = caad.CTA_ActionTaken,
                        Next_Action = caad.CTA_NextActionStep,
                        CTA_Date = caad.CTA_Date,
                        HR_Approval = caad.CTA_HRApproal,
                        Is_Active = caad.CTA_IsActive

                    }).FirstOrDefault();
                }
                else
                {
                    return null;
                }
                //var correctiveactiondetails = _db.CorrectiveActionForms.Where(x => x.CTA_EmployeeId == EMPId).FirstOrDefault();
                
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "CorrectiveActionModel GetCorrectiveActionFormDetailsEmployee(long UserId)", "Exception While getting Corrective Action details by userID", UserId);
                throw;
            }
        }




    }
}
