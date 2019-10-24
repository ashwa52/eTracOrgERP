using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

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
    }
}
