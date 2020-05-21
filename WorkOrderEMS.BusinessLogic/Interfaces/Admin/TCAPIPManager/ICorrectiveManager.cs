using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;


namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface ICorrectiveManager
    {
        CorrectiveActionModel EmployeeDetailsForCorrectiveActionForm(string empid);

        CorrectiveActionModel CorrectiveActionEmployeeFormDetailsForReview(string EMPId);

        bool SaveEmployeeCorrectiveActionDetails(CorrectiveActionModel obj);

        bool SaveCorrectiveActionNotificationbyManager(CorrectiveActionModel obj, string LoginEmployeeId);

        bool SaveCorrectiveActionNotificationForHrApproval(string EmpId, string LoginEmployeeId);

        bool SaveCorrectiveActionNotificationForHrDenial(WitnessModel obj, string EmpId, string LoginEmployeeId);

        bool CorrectiveActionHrDenyReasonandComment(WitnessModel obj, string EmpId);

        CorrectiveActionModel CorrectiveActionReviewEmployeeDetails(string EmpId);

        CorrectiveActionModel CorrectiveActionFormDetailsEmployee(long UserId);

        bool SaveNotificationforMeetingByManagerToEmployee(CorrectiveActionModel obj, string LoginEmployeeId);

        bool SetIsActiveBCorrective(long UserId);

        bool SetIsActiveCCorrective(long UserId);

        bool NotificationToHRForEmployeeCorrectiveActionDispute(CorrectiveActionModel obj, string LoginEmployeeId);

        bool SendMeetingTimeToEmployeeByNotification(string EMPId, string LoginEmployeeId, WitnessModel obj);

        bool SendMeetingTimeToEmployeeByNotificationNonExempt(string EMPId, string LoginEmployeeId, WitnessModel obj);
        bool SaveMeetingDateTimewithEmployeeId(CorrectiveActionModel obj);
    }
}
