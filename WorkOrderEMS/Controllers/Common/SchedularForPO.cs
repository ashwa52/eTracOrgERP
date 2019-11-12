using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.Controllers.Common
{
    public class SchedularForPO : IJob
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
              _workorderems.sp_SetPOReccuring();
            }
            catch (Exception ex)
            {
                 
            }
            return null;
        }
        }
}