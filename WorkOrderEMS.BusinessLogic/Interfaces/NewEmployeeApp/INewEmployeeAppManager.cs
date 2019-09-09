using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface INewEmployeeAppManager
    {
        List<UnAssignedWorkOrderModel> TaskListForEmployee(EmployeeManagerModel objEmpManagerAppModel);

        List<UnAssignedWorkOrderModel> FacilityTaskListForEmployee(EmployeeManagerModel objEmpManagerAppModel);
    }
}
