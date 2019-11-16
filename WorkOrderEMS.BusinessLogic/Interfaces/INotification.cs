using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface INotification
    {
        List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails);
    }
}
