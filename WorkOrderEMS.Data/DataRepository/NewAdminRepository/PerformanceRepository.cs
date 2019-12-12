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
        /// <summary>
        /// SaveMeetingDetails
        /// </summary>
        /// <param name="objSetupMeeting"></param>
        /// <returns></returns>
        public bool SaveMeetingDetails(SetupMeeting objSetupMeeting)
        {
            try
            {
                //DateTime dt = DateTimeOffset.Parse(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime).UtcDateTime;
                //string _datetime = dt.ToString();
                //objworkorderEMSEntities.spSetReviewMeetingDateTime("I", null, null, objSetupMeeting.ReceipientEmailId, objSetupMeeting.FinYear, objSetupMeeting.FinQrtr, _datetime);
                DateTime dt = Convert.ToDateTime(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime);
                //DateTime dt1 = DateTime.ParseExact(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture);
                objworkorderEMSEntities.spSetReviewMeetingDateTime("I", null, null, objSetupMeeting.ReceipientEmailId, objSetupMeeting.FinYear, objSetupMeeting.FinQrtr, objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime);

            }
            catch (Exception)
            { throw; }
            return true;

        }
        public SetupMeeting GetMeetingDetail(string Id, string FinYear, string FinQuarter)
        {
            SetupMeeting result;
            try
            {
                result = objworkorderEMSEntities.spGetReviewMeetingDateTime(Id, FinYear, FinQuarter).Select(x => new SetupMeeting()
                {
                    RMS_Id = x.RMS_Id,
                    ReceipientEmailId = x.RMS_EMP_EmployeeId,
                    FinYear = x.RMS_FinencialYear,
                    FinQrtr = x.RMS_FinQuarter,
                    RMS_InterviewDateTime = x.RMS_InterviewDateTime,
                    RMS_Date = x.RMS_Date,
                    RMS_IsActive = x.RMS_IsActive
                }).SingleOrDefault();

            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// Get Meeting List For HR
        /// </summary>
        /// <returns></returns>
        public List<ReviewMeeting> GetMeetingList()
        {
            try
            {
                return objworkorderEMSEntities.spGetReviewMeetingList().Select(x => new ReviewMeeting()
                {
                    EmployeeName = x.EmployeeName,
                    EMP_EmployeeID = x.EMP_EmployeeID,
                    EMP_ManagerId = x.EMP_ManagerId,
                    ManagerPhoto = x.ManagerPhoto,
                    ManagerName = x.ManagerName,
                    EmployeePhoto = x.EmployeePhoto,
                    //PRMeetingDateTime = x.PRMeetingDateTime.HasValue ? new DateTimeOffset(x.PRMeetingDateTime.Value, TimeSpan.FromHours(0)).ToLocalTime().DateTime : (DateTime?)null
                    PRMeetingDateTime = x.PRMeetingDateTime
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
