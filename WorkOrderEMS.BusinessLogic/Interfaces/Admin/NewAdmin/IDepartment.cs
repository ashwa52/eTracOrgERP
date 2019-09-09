using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IDepartment
    {
        bool SaveDepartment(DepartmentModel Obj);
        //DepartmentDetails ListAllDepartment(string txt, long? LocationId, long? UserId, long? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        DepartmentModel GetDepartmentData(long Id);
        List<DepartmentModel> ListAllDepartment(string txt, long? LocationId, long? UserId);
    }
}
