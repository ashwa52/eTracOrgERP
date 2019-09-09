using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class SendEmailTemplateManager : ISendEmailTemplateManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public bool SendMailTemplate(long LocationId)
        {
            try
            {
                bool IsMail = true;
                #region Email
                var objEmailLogRepository = new EmailLogRepository();
                var objEmailReturn = new List<EmailToManagerModel>();
                var objListEmailog = new List<EmailLog>();
                var objTemplateModel = new TemplateModel();
                var userData = _workorderems.UserRegistrations.Where(x => x.UserId == 3
                                                                    && x.IsDeleted == false).FirstOrDefault();

                if (IsMail == true)
                {
                    var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == LocationId && x.IsDeleted == false).FirstOrDefault();
                    bool IsSent = false;
                    var objEmailHelper = new EmailHelper();
                    objEmailHelper.emailid = userData.SubscriptionEmail;
                    objEmailHelper.ManagerName = userData.FirstName + " " + userData.LastName;
                    objEmailHelper.LocationName = locationData.LocationName;
                    objEmailHelper.UserName = userData.FirstName + " " + userData.LastName;
                    objEmailHelper.MailType = "EMAILDEMOSEND";
                    objEmailHelper.SentBy = userData.UserId;
                    objEmailHelper.LocationID = locationData.LocationId;
                    objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                    IsSent = objEmailHelper.SendEmailWithTemplate();

                    //Push Notification
                    /// string message = PushNotificationMessages.eFleetIncidentForServiceReported(objeFleetVehicleIncidentModel.LocationName, objeFleetVehicleIncidentModel.QRCodeID, objeFleetVehicleIncidentModel.VehicleNumber);
                    //PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                    if (IsSent == true)
                    {
                        var objEmailog = new EmailLog();
                        try
                        {
                            objEmailog.CreatedBy = userData.UserId;
                            objEmailog.CreatedDate = DateTime.UtcNow;
                            objEmailog.DeletedBy = null;
                            objEmailog.DeletedOn = null;
                            objEmailog.LocationId = locationData.LocationId;
                            objEmailog.ModifiedBy = null;
                            objEmailog.ModifiedOn = null;
                            objEmailog.SentBy = userData.UserId;
                            objEmailog.SentEmail = userData.SubscriptionEmail;
                            objEmailog.Subject = objEmailHelper.Subject;
                            objEmailog.SentTo = userData.UserId;
                            objListEmailog.Add(objEmailog);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    using (var context = new workorderEMSEntities())
                    {
                        context.EmailLogs.AddRange(objListEmailog);
                        context.SaveChanges(); ;
                    }
                }
                #endregion Email
            }
            
            catch (Exception ex)
            {

            }
            return true;
        }
    }
}