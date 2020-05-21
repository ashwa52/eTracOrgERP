﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class ePeopleRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 23-Sept-2019
        /// Created For : To get User List
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public List<UserRegistration> GetUserListByLocation(long? Location)
        {
            try
            {
                var data = objworkorderEMSEntities.UserRegistrations.Where(x => x.IsDeleted == false &&
                                                                          x.IsEmailVerify == true).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Creted By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get User Details by their manager Id 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetOrgnizationListview_Result> GetUserListByUserId(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationListview(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created For : To get self details by Employee Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetOrgnizationCommonview_Result GetUserSelfDetailsByUserId(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationCommonview(EmployeeId).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To get Position in VCS Chart by  Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetOrgnizationPositionView_Result UserPositionVCS(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationPositionView(EmployeeId).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get user data list
        /// Created Date : 05-oct-2019
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetOrgnizationUserView_Result> UserTreeViewDetails(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationUserView(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Creted By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get User Details by their manager Id 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        /// 
        public List<spGetVehicleSeating_Result> GetVSCDetails(long VSCId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeating().Where(x => x.VST_Id == VSCId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 17-Oct-2019
        /// Created For : To get employee management list by login employee id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetEmployeeManagementList_Result> GetEmployeeManagementListData(long LocationId, string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetEmployeeManagementList(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To send VSC for Approval
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SendForApproval(AddChartModel Obj)
        {
            try
            {
                var setData = objworkorderEMSEntities.spSetRequisition(Obj.RequisitionType, Obj.Id, Obj.ActionStatus, Obj.JobTitleCount, Obj.EmployeeId, null, null);
                //var data = objworkorderEMSEntities.spSetRequisitionApproval(Obj.RequisitionId,Obj.EmployeeId,Obj.Status,null,Obj.IsActive);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To get all requisition list
        /// </summary>
        /// <returns></returns>
        public List<spGetRequisitionList_Result> GetRequisitionlist()
        {
            try
            {
                var data = objworkorderEMSEntities.spGetRequisitionList().ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To Approve/Reject requisition
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public int ApproveRejectRequisition(long Id, string Status, string EmployeeId)
        {
            try
            {
                return  objworkorderEMSEntities.spSetRequisitionApproval(Id, EmployeeId, Status,null,"Y");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 21-oct-2019
        /// Created For : To get job title cout list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<spGetJobTitle_Result> GetJobTitleCount(long? Id)
        {
            try
            {
                return objworkorderEMSEntities.spGetJobTitle(Id).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 22-Oct-2019
        /// Created For : TO get job Title details
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public JobTitle GetJobCount(long JobId)
        {
            try
            {
                return objworkorderEMSEntities.JobTitles.Where(x => x.JBT_Id == JobId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Creaed Date : 25-oct-2019
        /// Created For : To get Uploaded files list
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        //public List<spGetFileUpload_Result1> GetUploadFilesList(string EmployeeId)
        //{
        //    try
        //    {
        //        return objworkorderEMSEntities.spGetFileUpload(EmployeeId).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<spGetFileUpload_Result> GetUploadFilesListTesting(string EmployeeId)
        {
            var lst = new List<spGetFileUpload_Result>();
            try
            {
                lst =  objworkorderEMSEntities.spGetFileUpload(EmployeeId).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return lst;
        }

        public List<spGetVacant_JobTitle_Result> GetJobTitleVacant(long VSC_Id)
        {
            return objworkorderEMSEntities.spGetVacant_JobTitle(VSC_Id).ToList();
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 14-Nov-2019
        /// Created For :To save Employee Status
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int SaveEmployeeStatus(DemotionModel Obj)
        {
            try
            {
                return objworkorderEMSEntities.spSetEmployeeStatusChange(Obj.ChangeType,Obj.EmpId,Obj.JobTitleCurrent,
                                                                         Obj.JobTitleId,Obj.LocationIdCurrent,Obj.LocationId,
                                                                         Obj.EmployeeCurrentStatus,Obj.EmploymentStatus, Obj.FromDate,
                                                                         Obj.ToDate,Obj.CreatedBy);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14-Nov-2019
        /// Created For : To get Employee status list
        /// </summary>
        /// <param name="VSC_Id"></param>
        /// <returns></returns>
        public List<spGetEmployeeStatusChangeList_Result> GetEmployeeStatusList()
        {
            return objworkorderEMSEntities.spGetEmployeeStatusChangeList().ToList();
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 15_Nov-2019
        /// Created For : To approve reject Employee Status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public int ApproveRejectEmployeeReject(long Id, string Status, string EmployeeId, string Comment)
        {
            try
            {
                return objworkorderEMSEntities.spSetEmployeeStatusChangeApproval(Id, EmployeeId, Status, Comment);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bnasod
        /// Created Date : 18-Nov-2019
        /// Created For : To send assessment for approve reject via mail.
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="IsActive"></param>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public int SendForAssessment(string Status, string IsActive, long ApplicantId)
        {
            try
            {
                return objworkorderEMSEntities.spSetApplicantStatus(ApplicantId, Status, IsActive);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
