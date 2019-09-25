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
        UserModelList GetUserHeirarchyList(long? LocationId, long? UserId);
    }
}
