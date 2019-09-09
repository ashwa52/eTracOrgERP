using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.BusinessLogic
{
    public class UnclosedWOManager : IUnclosedWOManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public UnClosedWOModelDetails GetAllUnOrderList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objUnclosedWO = new UnClosedWOModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                LocationId = 0;
                var currentDate = DateTime.UtcNow;
                DateTime dt1 = currentDate.AddHours(-4);
                var Results = _workorderems.WorkRequestAssignments.Join(_workorderems.UserRegistrations,w => w.AssignToUserId,
                                                                       u => u.UserId,(w,u) => new { w,u})
                                                                       .Where(x => x.w.EndTime == null 
                                                                       && x.w.StartTime != null
                                                                       && x.w.AssignedTime <= dt1
                                                                       && x.w.IsDeleted == false).Select(a => new UnClosedWOModel()
                {
                   WorkOrder = a.w.WorkOrderCode+a.w.WorkOrderCodeID,
                   StartTime = a.w.StartTime.ToString(),
                   AssignedTo = a.u.FirstName+" "+a.u.LastName,
                   LocationName = a.w.LocationMaster.LocationName,
                   WorkRequestId = a.w.WorkOrderCodeID
                }).OrderByDescending(x => x.WorkRequestId).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objUnclosedWO.pageindex = pageindex;
                objUnclosedWO.total = totalPages;
                objUnclosedWO.records = totRecords;
                objUnclosedWO.rows = Results.ToList();
                return objUnclosedWO;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public UnClosedWOModel GetAllUnOrderList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all unorder work order.", null);
                throw;
            }
        }

    }
}
