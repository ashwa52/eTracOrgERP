using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IePeopleManager
    {
        List<UserModelList> GetUserList(long? LocationId);
        List<UserModelList> GetUserHeirarchyList(long? LocationId, long? UserId);
        List<UserListViewEmployeeManagementModel> GetUserListByUserId(long? LocationId, long? UserId);
        UserListViewEmployeeManagementModel GetVCSPositionByUserId(long? UserId);
        List<UserListViewEmployeeManagementModel> GetUserTreeViewList(long UserId);
        List<UserListViewEmployeeManagementModel> GetUserTreeViewListTesting(long UserId);
        List<AddChartModel> GetVSCList();
        AddChartModel GetVSCDetailsById(long VSCId);
        //List<AddChartModel> GetVSCDetailsById(long VSCId);
        List<UserModelList> GetEmployeeMgmList(long LocationId, long UserId);
        DemotionModel GetEMployeeData(long UserId);
        bool ApprovalRequisition(AddChartModel Obj);
        List<AddChartModel> GetRequisitionlist();
        bool ApproveRejectAction(long Id, string Status, long UserId);
        List<AddChartModel> GetJobTitleCountForRequistion(long VSCId);
        JobTitleModel GetJobTitleCount(long JobId);
        bool SendJobTitleForApproval(JobTitleModel model);
        bool SaveDirectDepositeForm(DirectDepositeFormModel model);
        //IEnumerable<UploadedFiles> GetUploadedFilesOfUser(string EmployeeId);
        List<UploadedFiles> GetUploadedFilesOfUserTesting(string EmployeeId);
        List<GraphCountModel> GetEMP_ReqCount();
        List<JobTitleModel> GetJobTitleVacantList(long VSC_Id);
        bool SaveCommonStatusOfEmployee(DemotionModel Obj);
        List<EmployeeStatusList> GetEmployeeStatusList();
        bool ApproveRejectEmployeeStatus(long Id, string Status, long UserId, string Comment);
        bool SendForAssessment(string Status, string IsActive, long ApplicantId, long UserId);
        bool ClearedOrNot(string IsActive, string ActionVal, long ApplicantId);
        bool SendForBackgroundCheck(string Status, string IsActive, long ApplicantId, long UserId);
    }
}
