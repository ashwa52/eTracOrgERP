﻿using System;
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
    }
}