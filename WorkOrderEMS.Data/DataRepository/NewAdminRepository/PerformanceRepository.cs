using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Data.DataRepository.NewAdminRepository
{
    public class PerformanceRepository 
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        public bool SaveMeetingDetails(SetupMeeting objSetupMeeting)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime);
                //DateTime dt1 = DateTime.ParseExact(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture);

                objworkorderEMSEntities.spSetReviewMeetingDateTime("I", null, null, objSetupMeeting.ReceipientEmailId, objSetupMeeting.FinYear, objSetupMeeting.FinQrtr, objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime);

            }
            catch (Exception ex)
            { throw; }
            return true;

        }
    }
}
