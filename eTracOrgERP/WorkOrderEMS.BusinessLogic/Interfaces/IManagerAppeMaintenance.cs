using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IManagerAppeMaintenance
    {
        List<UnAssignedWorkOrderModel> UnassignedWorkOrderList(ManagerAppModel objManagerAppModel);
        List<EmployeeListModel> EmployeeList(ManagerAppModel objManagerAppModel);
        ServiceResponseModel<string> AssignedEmployee(ManagerAppModel objManagerAppModel);
        ServiceResponseModel<DashboardCountModel> GetCountOfDashboardForManager(ManagerAppModel obj);
    }
}
