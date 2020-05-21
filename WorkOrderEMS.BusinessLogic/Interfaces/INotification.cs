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
        bool SaveNotification(NotificationDetailModel obj);
        List<NotificationDetailModel> GetNotification(long userId, string Username);
        bool ReadNotificationById(long NotificationId);
        ApplicantDetails GetApplicantDetails(long ApplicantId);
        NotificationDetailModel NotificationDetailsforMeetingDateTime(NotificationDetailModel obj);

    }
}
