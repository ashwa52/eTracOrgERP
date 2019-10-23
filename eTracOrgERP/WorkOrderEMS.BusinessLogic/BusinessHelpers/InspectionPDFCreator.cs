using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WorkOrderEMS.BusinessLogic.Managers.eFleet;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.BusinessHelpers
{
    public static class InspectionPDFCreator<T>
    {
        public static string PDFCreationDAREMailforInspection(T obj1, eFleetVehicle result, string EncryptVehicleId)
        {
            try
            {
                string returnPath = string.Empty;
                dynamic obj = obj1;
                var objEmailLogRepository = new EmailLogRepository();
                var objEmailReturn = new List<EmailToManagerModel>();
                var objListEmailog = new List<EmailLog>();
                //if (result.Data.Response == 1)
                //{
                objEmailReturn = objEmailLogRepository.SendEmailToManagerForeFleetInspection(obj.LocationId, obj.UserId).Result;
                // }
                if (result.DamageTireStatus == false && result.EmergencyAccessoriesStatus == false
                    && result.EngineExteriorStatus == false && result.InteriorMileageDriverStatus == false
                    && obj.Status == eFleetCheckInOut.PreTrip || obj.Status == eFleetCheckInOut.PostTrip)
                {
                    var objTemplateModel = new TemplateModel();
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.DamageTireDetails != null)
                    {
                        var ObjReturnDamageTire = new eFleetDamageTireModel();
                        ObjReturnDamageTire = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(result.DamageTireDetails);
                        objTemplateModel.DamageTire = ObjReturnDamageTire;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.InteriorMileageDriverDetails != null)
                    {
                        var objInteriorMileageDriverDetails = new eFleetInteriorMileageDriverModel();
                        objInteriorMileageDriverDetails = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.DeserializeXml(result.InteriorMileageDriverDetails);
                        objTemplateModel.InteriorMileageDriver = objInteriorMileageDriverDetails;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.EngineExteriorDetails != null)
                    {
                        var objEngineExteriorDetails = new eFleetEngineExteriorModel();
                        objEngineExteriorDetails = GenericDataContractSerializer<eFleetEngineExteriorModel>.DeserializeXml(result.EngineExteriorDetails);
                        objTemplateModel.EngineExterior = objEngineExteriorDetails;
                    }
                    //Added By Bhushan Dod on 06/09/2017 for Damage in mobile app
                    if (result.EmergencyAccessoriesDetails != null)
                    {
                        var objEmergencyAccessoriesDetails = new eFleetEmergencyAccessoriesModel();
                        objEmergencyAccessoriesDetails = GenericDataContractSerializer<eFleetEmergencyAccessoriesModel>.DeserializeXml(result.EmergencyAccessoriesDetails);
                        objTemplateModel.EmergencyAccessories = objEmergencyAccessoriesDetails;
                    }
                    objTemplateModel.Status = obj.Status;
                    objTemplateModel.UserName = obj.UserName;
                    objTemplateModel.VehicleNumber = result.VehicleNumber;
                    objTemplateModel.QRCodeID = result.QRCodeID;
                    objTemplateModel.TimeZoneName = obj.TimeZoneName;
                    objTemplateModel.TimeZoneOffset = obj.TimeZoneOffset;
                    objTemplateModel.IsTimeZoneinDaylight = obj.IsTimeZoneinDaylight;

                    PDFModel objPDFModel = new PDFModel();
                    objPDFModel.htmlData = TemplateDesigner.eFleetTemplate(objTemplateModel);

                    objPDFModel.EncryptVehicleId = EncryptVehicleId;
                    objPDFModel.InspectionSignatureImage = obj.InspectionSignatureImage;
                    objPDFModel.Username = obj.UserName;
                    returnPath = VehicleManager.SaveInspectionPDF(objPDFModel);

                    if (obj.Status == eFleetCheckInOut.PreTrip)
                    {
                        using (var context = new workorderEMSEntities())
                        {
                            var objeFleetVehicleInspectionLog = new eFleetVehicleInspectionLog();
                            try
                            {
                                objeFleetVehicleInspectionLog.CreatedBy = obj.UserId;
                                objeFleetVehicleInspectionLog.CreatedDate = DateTime.UtcNow;
                                objeFleetVehicleInspectionLog.LocationId = obj.LocationId;
                                objeFleetVehicleInspectionLog.ModifiedBy = null;
                                objeFleetVehicleInspectionLog.ModifiedDate = null;
                                objeFleetVehicleInspectionLog.PreInspection = returnPath; 
                                objeFleetVehicleInspectionLog.VehicleID = obj.VehicleID;
                                context.eFleetVehicleInspectionLogs.Add(objeFleetVehicleInspectionLog);
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public static string PDFCreationDAREMailforInspection(T obj1, eFleetVehicle result)", "while inserting inspection log", result.VehicleID);
                            }
                        }
                    }
                    else
                    {
                        using (var context = new workorderEMSEntities())
                        {
                            try
                            {
                                long vid = obj.VehicleID;
                                long loc = obj.LocationId;
                                var logData = context.eFleetVehicleInspectionLogs.Where(x => x.VehicleID == vid && x.LocationId == loc).FirstOrDefault();
                                if (logData != null && logData.InspectionLogID > 0)
                                {
                                    logData.ModifiedBy = obj.UserId;
                                    logData.ModifiedDate = DateTime.UtcNow;
                                    logData.PostInspection = returnPath;
                                    context.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public static string PDFCreationDAREMailforInspection(T obj1, eFleetVehicle result)", "while updating inspection log", result.VehicleID);
                            }
                        }
                    }
                    if (objEmailReturn.Count > 0)
                    {
                        foreach (var item in objEmailReturn)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.VehicleMake = result.Make;
                            objEmailHelper.VehicleModel = result.Model;
                            objEmailHelper.VehicleIdentificationNumber = result.VehicleNumber;
                            objEmailHelper.LocationName = obj.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.QrCodeId = obj.QrcodeId;
                            objEmailHelper.InfractionStatus = obj.Status;
                            objEmailHelper.MailType = "EFLEETINSPECTIONREPORT";
                            objEmailHelper.SentBy = item.RequestBy;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                            string[] attachFiles = new string[1];
                            for (var i = 0; i < attachFiles.Count(); i++)
                            {
                                attachFiles[i] = HttpContext.Current.Server.MapPath("~/Content/eFleetDocs/Inspection/" + returnPath);
                            }
                            IsSent = objEmailHelper.SendEmailWithTemplate(attachFiles);

                            //Push Notification
                            string message = PushNotificationMessages.eFleetInspectionReported(obj.LocationName, result.QRCodeID, result.VehicleNumber);
                            PushNotification.GCMAndroid(message, item.DeviceId, objEmailHelper);
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = item.RequestBy;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = item.LocationID;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = item.RequestBy;
                                    objEmailog.SentEmail = item.ManagerEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.ManagerUserId;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception ex)
                                {
                                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public static string PDFCreationDAREMailforInspection(T obj1, eFleetVehicle result)", "while saving email log", result.VehicleID);
                                }
                            }
                        }
                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges();
                        }
                        //var x = EmailLogRepository.InsertEntitiesNew("EmailLog", objListEmailog);
                        //Task<bool> x = null;
                        //foreach (var i in objListEmailog)
                        //{
                        //    x = objEmailLogRepository.SaveEmailLogAsync(i);
                        //}
                    }
                }
                return returnPath;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public static string PDFCreationDAREMailforInspection(T obj1, eFleetVehicle result)", "while creating PDF", result.VehicleID);
                return null;
            }
        }
    }
}
