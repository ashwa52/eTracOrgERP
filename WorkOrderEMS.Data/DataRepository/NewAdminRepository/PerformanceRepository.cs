using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

                DateTime dt = Convert.ToDateTime(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime);

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

        public List<EventModel> GetMyEvents(long UserId, string start, string end)
        {
            List<EventModel> result = new List<EventModel>();
            try
            {
                var fromDate = Convert.ToDateTime(start);
                var toDate = Convert.ToDateTime(end);
                using (objworkorderEMSEntities = new workorderEMSEntities())
                {
                    //var rslt = objworkorderEMSEntities.Appointments.Where(s => s.DateTimeScheduled >= fromDate && EntityFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate && s.CreatedBy == UserName);
                    // var rslt = objworkorderEMSEntities.BookSlotTimes.Join(objworkorderEMSEntities.SlotTimes).Where(s => s.BST_SlotDate >= fromDate && EntityFunctions.AddMinutes(s.BST_SlotDate, 60) <= toDate && s.BST_EMP_EmployeeID == UserId);
                    var rslt = objworkorderEMSEntities.SlotTimes    // your starting point - table in the "from" statement
                            .Join(objworkorderEMSEntities.BookSlotTimes, // the source table of the inner join
                               post => post.SLT_Id,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                               meta => meta.BST_SLT_Id,   // Select the foreign key (the second part of the "on" clause)
                               (post, meta) => new { Post = post, Meta = meta }) // selection
                              .Where(postAndMeta => postAndMeta.Meta.BST_Date >= fromDate && EntityFunctions.AddMinutes(postAndMeta.Meta.BST_Date, 60) <= toDate && postAndMeta.Meta.BST_EMP_EmployeeID == UserId);

                    foreach (var item in rslt)
                    {
                        EventModel rec = new EventModel();
                        //DateTime dt = Convert.ToDateTime(item.Meta.BST_SlotDate + " " + item.Post.SLT_fromTime);
                        //DateTime dt1 = DateTime.ParseExact(item.Meta.BST_SlotDate + " " + item.Post.SLT_fromTime, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture);
                        rec.id = item.Meta.BST_Id;
                        // rec.SomeImportantKey = item.SomeImportantKey;
                        rec.start = Convert.ToDateTime(item.Meta.BST_SlotDate.ToShortDateString() + " " + item.Post.SLT_fromTime).ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        //rec.start = item.Meta.BST_SlotDate.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        rec.end = item.Meta.BST_SlotDate.AddMinutes(60).ToString("s"); // field AppointmentLength is in minutes
                        rec.title = string.IsNullOrEmpty(item.Meta.BST_IsActive) ? "Title:" + rec.start.ToString() : "Title: " + item.Meta.BST_IsActive;
                        //rec.title = string.IsNullOrEmpty(item.Meta.BST_Title) ? "Title:" + rec.start.ToString() : "Title: " + item.Meta.BST_Title;
                        //rec.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
                        //rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
                        rec.StatusString = "#FF8000:BOOKED";
                        rec.StatusColor = "#FF8000:BOOKED";
                        string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
                        rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1, rec.StatusColor.Length - ColorCode.Length - 1);
                        rec.StatusColor = ColorCode;
                        result.Add(rec);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public bool CreateNewEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration, string JobId, string ApplicantName, string ApplicantEmail, long ManagerId, string selectedManagers)
        {
            bool result = false;
            // Appointment obj = new Appointment();

            try
            {
                using (objworkorderEMSEntities = new workorderEMSEntities())
                {
                    objworkorderEMSEntities.spSetBookSlotTime("I", null, Title, ManagerId, Convert.ToDateTime(NewEventDate), Convert.ToInt32(NewEventTime), 1, "Y");
                    if (selectedManagers != "")
                    {
                        var mgrlist = selectedManagers.Split(',').Distinct().ToList();
                        long? mgr1, mgr2, mgr3;
                        try
                        {
                            mgr1 = ManagerId;
                        }
                        catch (Exception e)
                        {
                            mgr1 = null;
                        }
                        try
                        {
                            mgr2 = Convert.ToInt64(mgrlist[0]);
                        }
                        catch (Exception e)
                        {
                            mgr2 = null;
                        }
                        try
                        {
                            mgr3 = Convert.ToInt64(mgrlist[1]);
                        }
                        catch (Exception e)
                        {
                            mgr3 = null;
                        }

                        objworkorderEMSEntities.spSetInterviewPanel("I", null, Convert.ToInt64(JobId), mgr1, mgr2, mgr3, 1, "Y");
                        result = true;
                    }
                    else
                    {
                        objworkorderEMSEntities.spSetInterviewPanel("I", null, Convert.ToInt64(JobId), ManagerId, null, null, 1, "Y");
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public bool UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            bool result = false;
            using (objworkorderEMSEntities = new workorderEMSEntities())
            {
                //{
                //    var rec = objworkorderEMSEntities.Appointments.FirstOrDefault(s => s.ID == id);
                //    if (rec != null)
                //    {
                //        DateTime DateTimeStart = DateTime.Parse(NewEventStart, null, DateTimeStyles.RoundtripKind).ToLocalTime(); // and convert offset to localtime
                //        rec.DateTimeScheduled = DateTimeStart;
                //        if (!String.IsNullOrEmpty(NewEventEnd))
                //        {
                //            TimeSpan span = DateTime.Parse(NewEventEnd, null, DateTimeStyles.RoundtripKind).ToLocalTime() - DateTimeStart;
                //            rec.AppointmentLength = Convert.ToInt32(span.TotalMinutes);
                //        }
                //        objworkorderEMSEntities.SaveChanges();
                //        result = true;
                //    }
                //}
            }
            return result;
        }
        public List<EventModel> GetBookedSlots(long UserId)
        {
            List<EventModel> result = new List<EventModel>();
            try
            {
                using (objworkorderEMSEntities = new workorderEMSEntities())
                {
                    var rslt = objworkorderEMSEntities.SlotTimes    // your starting point - table in the "from" statement
                           .Join(objworkorderEMSEntities.BookSlotTimes, // the source table of the inner join
                              post => post.SLT_Id,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                              meta => meta.BST_SLT_Id,   // Select the foreign key (the second part of the "on" clause)
                              (post, meta) => new { Post = post, Meta = meta }) // selection
                             .Where(postAndMeta => postAndMeta.Meta.BST_EMP_EmployeeID == UserId);



                    //var myList = objworkorderEMSEntities.SlotTimes
                    //                        .Join(
                    //                        objworkorderEMSEntities.BookSlotTimes,
                    //                        st => st.SLT_Id,
                    //                        bst => bst.BST_SLT_Id,
                    //                        (st, bst) => new { SlotTime = st, BookedSlotTime = bst })
                    //                        .Join(
                    //                        objworkorderEMSEntities.InterviewProposalTimes.Where(dsi => dsi.IPT_JPS_JobPostingId>0),
                    //                        cs => cs.BookedSlotTime.job,
                    //                        dsi => dsi.Sector_code,
                    //                        (cs, dsi) => new { cs.Company, cs.Sector, IndustryCode = dsi.Industry_code })
                    //                        .Select(c => new
                    //                        {
                    //                            c.Company.Equity_cusip,
                    //                            c.Company.Company_name,
                    //                            c.Company.Primary_exchange,
                    //                            c.Company.Sector_code,
                    //                            c.Sector.Description,
                    //                            c.IndustryCode
                    //                        });

                    foreach (var item in rslt)
                    {
                        EventModel rec = new EventModel();
                        //DateTime dt = Convert.ToDateTime(item.Meta.BST_SlotDate + " " + item.Post.SLT_fromTime);
                        //DateTime dt1 = DateTime.ParseExact(item.Meta.BST_SlotDate + " " + item.Post.SLT_fromTime, "dd/MM/yy h:mm:ss tt", CultureInfo.InvariantCulture);
                        rec.id = item.Meta.BST_Id;
                        // rec.SomeImportantKey = item.SomeImportantKey;
                        rec.start = Convert.ToDateTime(item.Meta.BST_SlotDate.ToShortDateString() + " " + item.Post.SLT_fromTime).ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        //rec.start = item.Meta.BST_SlotDate.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        rec.end = Convert.ToDateTime(item.Meta.BST_SlotDate.ToShortDateString() + " " + item.Post.SLT_ToTime).ToString("s"); // field AppointmentLength is in minutes
                        rec.title = string.IsNullOrEmpty(item.Meta.BST_Title) ? "Title:" + rec.start.ToString() : "Title: " + item.Meta.BST_Title;
                        //rec.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
                        //rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
                        rec.StatusString = "#FF8000:BOOKED";
                        rec.StatusColor = "#FF8000:BOOKED";
                        string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
                        rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1, rec.StatusColor.Length - ColorCode.Length - 1);
                        rec.StatusColor = ColorCode;
                        result.Add(rec);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;

        }


        public int GetAppointmentCount(string ApplicantId)
        {
            try
            {
                using (objworkorderEMSEntities = new workorderEMSEntities())
                {
                    return 0;
                    //return objworkorderEMSEntities.Appointments.Where(x => x.CreatedFor == ApplicantId && x.IsDeleted == false).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
