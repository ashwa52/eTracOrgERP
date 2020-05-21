//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WorkOrderEMS.Data.EntityModel;
//using WorkOrderEMS.Helper;
//using WorkOrderEMS.Models;

//namespace WorkOrderEMS.BusinessLogic
//{
//    public class NotificationManager : INotification
//    {
//        workorderEMSEntities Context = new workorderEMSEntities();
//        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
//        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
//        public List<EmailHelper> GetUnseenList(NotificationDetailModel objDetails)
//        {
//            var _db = new workorderEMSEntities();
//            var listTask = new List<EmailHelper>();
//            var objEmailHelper = new EmailHelper();
//            var notification = new List<NotificationDetailModel>();
//            var obj = new EmailHelper();
//            try
//            {
//                notification = _db.NotificationDetails.Where(x => x.IsRead == false && x.IsDeleted == false && x.AssignUserId == objDetails.UserId || x.AssignUserId == 0).Select(a => new NotificationDetailModel()
//                {
//                    WorkOrderID = a.WorkOrderID,
//                    AssignTo = a.AssignUserId,
//                    POID = a.POID,
//                    BillID = a.BillID,
//                    MiscellaneousID = a.MiscellaneousID,
//                    eScanQRCID = a.eScanId,
//                    IsRead = a.IsRead
//                }).ToList();
//                var userDetails = _db.UserRegistrations.Where(x => x.UserId == objDetails.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
//                if (notification.Count() > 0)
//                {
//                    foreach (var item in notification)
//                    {
//                        #region Wo
//                        if (item.WorkOrderID != null && item.WorkOrderID > 0)
//                        {
//                            obj = _db.WorkRequestAssignments.Join(_db.NotificationDetails, q => q.WorkRequestAssignmentID, u => u.WorkOrderID, (q, u) => new { q, u }).
//                                      Where(x => (x.q.WorkRequestAssignmentID == item.WorkOrderID) && (x.u.IsDeleted == false)
//                                      && (x.u.IsRead == false && x.q.IsDeleted == false && (x.q.AssignToUserId == objDetails.UserId || x.q.AssignToUserId == 0))).Select
//                                      (a => new EmailHelper()
//                                      {
//                                          MailType = a.q.WorkRequestProjectType == 128 ? "WORKORDERREQUESTTOEMPLOYEE" : a.q.WorkRequestProjectType == 129 ? "SPECIALWORKORDERTOEMPLOYEE" : a.q.WorkRequestProjectType == 279 ? "CONTINIOUSREQUEST" : "EMAINTENANCE",
//                                          WorkRequestAssignmentID = a.q.WorkRequestAssignmentID,
//                                          WorkOrderCodeId = a.q.WorkOrderCode + a.q.WorkOrderCodeID.ToString(),
//                                          AssignedTime = a.q.AssignedTime.ToString(),
//                                          ProjectDesc = a.q.ProblemDesc,
//                                          UserName = a.u.AssignUserId.ToString(),
//                                          LocationName = a.q.LocationMaster.LocationName,
//                                          WorkOrderImage = a.q.WorkRequestImage,// == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + a.q.WorkRequestImage,
//                                          WorkRequestType = a.q.WorkRequestType,
//                                          WorkRequestProjectType = a.q.WorkRequestProjectType,
//                                          PriorityLevel = a.q.PriorityLevel,
//                                          WeekDays = a.q.WeekDays,
//                                          StartTime = a.q.StartTime.ToString(),
//                                          EndTime = a.q.EndTime.ToString(),
//                                          ModifiedDate = a.q.ModifiedDate,
//                                          StartDate = a.q.StartDate.ToString(),
//                                          EndDate = a.q.EndDate.ToString(),
//                                          IsWorkable = true,

//                                          CustomerName = a.q.CustomerName,
//                                          VehicleModel1 = a.q.VehicleModel,
//                                          VehicleMake1 = a.q.VehicleMake.ToString(),
//                                          VehicleYear = a.q.VehicleYear.ToString(),
//                                          FrCurrentLocation = a.q.CurrentLocation,
//                                          VehicleColor = a.q.VehicleColor,
//                                          DriverLicenseNo = a.q.DriverLicenseNo,
//                                          CustomerContact = a.q.CustomerContact,
//                                          FacilityRequest = a.q.FacilityRequestId,
//                                          AddressFacilityReq = a.q.Address,
//                                          LicensePlateNo = a.q.LicensePlateNo
//                                      }).FirstOrDefault();
//                            if (obj != null)
//                            {
//                                obj.WorkOrderImage = obj.WorkOrderImage == null ? HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + obj.WorkOrderImage;
//                                //obj.AssignedTime = obj.AssignedTime.ToString("HH:mm");
//                                ///This is to check if it is manager then the notification make it non workable
//                                ///make sure that WO should be Urgent or facility
//                                if (userDetails != null)
//                                {
//                                    if (userDetails.UserType == Convert.ToInt64(UserType.Manager) &&
//                                      (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.FacilityRequest)
//                                      || obj.PriorityLevel == Convert.ToInt64(WorkRequestPriority.Level1Urgent)))
//                                    {
//                                        if ((obj.StartTime == null || obj.StartTime == "") && (obj.EndTime == null || obj.EndTime == ""))
//                                        {
//                                            obj.ProjectDesc = CommonMessage.WOStatusMessageForManager(obj.WorkOrderCodeId);
//                                            obj.IsWorkable = false;
//                                            obj.Message = "No one accepted faciliy request " + obj.WorkOrderCodeId + " of type" + obj.FacilityRequest + " at location" + obj.LocationName;
//                                        }
//                                        else
//                                        {
//                                            obj.ProjectDesc = null;
//                                        }
//                                    }
//                                }
//                                ///This is for Continues WO to send notification as per days 
//                                if (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.ContinuousRequest))
//                                {
//                                    var dateList = obj.WeekDays.Split(',').ToList();
//                                    var todaysDate = DateTime.Now.ToShortDateString();
//                                    obj.Message = "Continous request " + obj.WorkOrderCodeId + " has not been started after arrived time" + obj.StartTime;
//                                    foreach (var date in dateList)
//                                    {
//                                        if (date == todaysDate)
//                                        {
//                                            //obj.IsWorkable = false;
//                                            listTask.Add(obj);
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    listTask.Add(obj);
//                                }
//                            }
//                        }
//                        #endregion WO

//                        #region PO
//                        ///This is for PO
//                        else if (item.POID != null && item.POID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
//                        {
//                            var POData = new EmailHelper();
//                            if (item.IsRead == false)
//                            {
//                                POData = _db.LogPODetails.Where(x => x.LPOD_POD_Id == item.POID && x.LPOD_IsActive == "Y")
//                                   .Select(a => new EmailHelper()
//                                   {
//                                       MailType = item.POID != null ? "POAPPROVEDREJECT" : null,
//                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                                       LocationName = a.LocationMaster.LocationName,
//                                       PONumber = a.LPOD_POD_Id.ToString(),
//                                       SentBy = userDetails.UserId,
//                                       LocationID = a.LPOD_LocationId,
//                                       LogPOId = a.LPOD_Id.ToString(),
//                                       Total = a.LPOD_POAmount.ToString(),
//                                       CMPId = a.LPOD_CMP_Id,
//                                       ApproveRemoveStatus = a.LPOD_IsApprove == "W" ? POStatus.W : a.LPOD_IsApprove == "Y" ? POStatus.Y : POStatus.N,
//                                       IsWorkable = true,
//                                       Comment = a.LPOD_Comment
//                                   }).OrderByDescending(x => x.LogPOId).FirstOrDefault();
//                            }
//                            //var data = POData.
//                            //_db.LogPODetails.Join(_db.NotificationDetails, q => q.LPOD_POD_Id, u => u.POID, (q, u) => new { q, u }).
//                            //    Where(x => (x.q.LPOD_POD_Id == item.POID) //&& (x.u.IsDeleted == false)
//                            //  /*&& (x.u.IsRead == false)*/).Select
//                            //(a => new EmailHelper()
//                            //{
//                            //  MailType = item.POID != null ? "POAPPROVEDREJECT" : null,
//                            //ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                            //LocationName = a.q.LocationMaster.LocationName,
//                            //PONumber = a.q.LPOD_POD_Id.ToString(),
//                            //SentBy = userDetails.UserId == 0 ? 0: userDetails.UserId,
//                            //LocationID = a.q.LPOD_LocationId == 0 ? 0:a.q.LPOD_LocationId,
//                            //LogPOId = a.q.LPOD_Id == 0 ? null : a.q.LPOD_Id.ToString(),
//                            //Total = a.q.LPOD_POAmount == null ? null :a.q.LPOD_POAmount.ToString(),
//                            //CMPId = a.q.LPOD_CMP_Id == null ? null : a.q.LPOD_CMP_Id,
//                            //        IsWorkable = true
//                            //      }).FirstOrDefault();
//                            if (POData != null)
//                            {
//                                if (POData.ApproveRemoveStatus == POStatus.W)
//                                {
//                                    POData.Message = "PO " + POData.PONumber + " has been approved by your manager, Current status is " + POData.ApproveRemoveStatus;
//                                }
//                                else if (POData.ApproveRemoveStatus == POStatus.Y)
//                                {
//                                    POData.Message = "PO " + POData.PONumber + " received final approval";
//                                }
//                                else if (POData.Comment != null)
//                                {
//                                    POData.Message = "PO " + POData.PONumber + " has been rejected due to" + POData.Comment + " , Please contact your manager";
//                                }
//                                ///To approve PO need company name so added this code just for approval process.
//                                if (POData != null && POData.CMPId > 0)
//                                {
//                                    var companyData = _db.Companies.Where(x => x.CMP_Id == POData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
//                                    if (companyData != null)
//                                    {
//                                        POData.CompanyName = companyData.CMP_NameLegal;
//                                        POData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
//                                    }
//                                }
//                                listTask.Add(POData);
//                            }
//                        }
//                        #endregion PO

//                        #region Bill
//                        else if (item.BillID != null && item.BillID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
//                        {
//                            var BillData = new EmailHelper();
//                            if (item.IsRead == false)
//                            {
//                                BillData = _db.LogPreBills.Where(x => x.LPBL_PBL_Id == item.BillID && x.LPBL_IsActive == "Y")
//                                   .Select(a => new EmailHelper()
//                                   {
//                                       MailType = item.BillID != null ? "BILLAPPROVE" : null,
//                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                                       LocationName = a.LocationMaster.LocationName,
//                                       BillId = a.LPBL_PBL_Id.ToString(),
//                                       SentBy = userDetails.UserId,
//                                       LocationID = a.LPBL_LocationId,
//                                       LogBillId = a.LPBL_Id,
//                                       Total = a.LPBL_InvoiceAmount.ToString(),
//                                       CMPId = a.LPBL_CMP_Id,
//                                       IsWorkable = true
//                                   }).FirstOrDefault();
//                            }
//                            if (BillData != null)
//                            {
//                                BillData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
//                                ///To approve PO need company name so added this code just for approval process.
//                                if (BillData != null && BillData.CMPId > 0)
//                                {
//                                    var companyData = _db.Companies.Where(x => x.CMP_Id == BillData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
//                                    if (companyData != null)
//                                    {
//                                        BillData.CompanyName = companyData.CMP_NameLegal;

//                                    }
//                                }
//                                listTask.Add(BillData);
//                            }
//                            else
//                            {
//                                BillData = _db.LogBills.Where(x => x.LBLL_POD_Id == item.BillID && x.LBLL_IsApprove == "Y" && x.LBLL_IsActive == "Y")
//                                   .Select(a => new EmailHelper()
//                                   {
//                                       MailType = item.BillID != null ? "BILLAPPROVE" : null,
//                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                                       LocationName = a.LocationMaster.LocationName,
//                                       BillId = a.LBLL_BLL_Id.ToString(),
//                                       SentBy = userDetails.UserId,
//                                       LocationID = a.LBLL_LocationId,
//                                       LogBillId = a.LBLL_Id,
//                                       Total = a.LBLL_InvoiceAmount.ToString(),
//                                       CMPId = a.LBLL_CMP_Id,
//                                       IsWorkable = true
//                                   }).FirstOrDefault();
//                            }
//                        }
//                        #endregion Bill

//                        #region Miscellaneous
//                        else if (item.MiscellaneousID != null && item.MiscellaneousID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
//                        {
//                            var MiscData = new EmailHelper();
//                            if (item.IsRead == false)
//                            {
//                                MiscData = _db.LogMiscellaneous.Where(x => x.LMIS_MIS_Id == item.MiscellaneousID && x.LMIS_IsActive == "Y")
//                                   .Select(a => new EmailHelper()
//                                   {
//                                       MailType = item.MiscellaneousID != null ? "APPROVEMISCELLANEOUS" : null,
//                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                                       LocationName = a.LocationMaster.LocationName,
//                                       MISId = a.LMIS_MIS_Id.ToString(),
//                                       SentBy = userDetails.UserId,
//                                       LocationID = a.LMIS_LocationId,
//                                       LogMiscId = a.LMIS_Id.ToString(),
//                                       Total = a.LMIS_InvoiceAmount.ToString(),
//                                       CMPId = a.LMIS_CMP_Id,
//                                       IsWorkable = true
//                                   }).FirstOrDefault();
//                            }
//                            if (MiscData != null)
//                            {
//                                MiscData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
//                                ///To approve PO need company name so added this code just for approval process.
//                                if (MiscData != null && MiscData.CMPId > 0)
//                                {
//                                    var companyData = _db.Companies.Where(x => x.CMP_Id == MiscData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
//                                    if (companyData != null)
//                                    {
//                                        MiscData.CompanyName = companyData.CMP_NameLegal;
//                                    }
//                                }
//                                listTask.Add(MiscData);
//                            }
//                        }
//                        #endregion Miscellaneous

//                        #region eScan
//                        else if (item.eScanQRCID != null && item.eScanQRCID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
//                        {
//                            var eScanData = new EmailHelper();
//                            if (item.IsRead == false)
//                            {
//                                eScanData = _db.QRCMasters.Where(x => x.QRCID == item.eScanQRCID && x.IsDeleted == false)
//                                   .Select(a => new EmailHelper()
//                                   {
//                                       MailType = a.IsDamage == true ? "QRCVEHICLEDAMAGE" : a.CheckOutStatus == true ? "CHECKINCHECKOUT" : null,
//                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
//                                       LocationName = a.LocationMaster.LocationName,
//                                       QrCodeId = a.QRCodeID.ToString(),
//                                       SentBy = userDetails.UserId,
//                                       LocationID = a.LocationId,
//                                       IsWorkable = false,
//                                       CheckoutUserName = a.UserName
//                                   }).FirstOrDefault();
//                            }
//                            if (eScanData != null)
//                            {
//                                eScanData.Message = "Someone want to checkout QRC " + eScanData.QrCodeId + " which is already checked out by" + eScanData.CheckoutUserName;
//                                eScanData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
//                                ///To approve PO need company name so added this code just for approval process.                        
//                                listTask.Add(eScanData);
//                            }
//                        }
//                        #endregion eScan
//                    }
//                }

//                //if(notification.Count() > 0)
//                //{
//                //    foreach (var item in notification)
//                //    {
//                //        if(item.WorkOrderID != null && item.WorkOrderID > 0)
//                //        {
//                //            var dd = _db.WorkRequestAssignments.Where(x => x.WorkRequestAssignmentID == item.WorkOrderID

//                //             && x.IsDeleted == false
//                //            ).Select(a => new EmailHelper() { 

//                //            }).ToList();
//                //        }
//                //    }
//                //}
//                return listTask.OrderByDescending(x => x.WorkRequestAssignmentID).OrderByDescending(x => x.PONumber).ToList();
//            }
//            catch (Exception ex)
//            {
//                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<EmailHelper> GetUnseenList(NotificationDetailModel objDetails)", "Exception while getting the list for notification details", obj);
//                throw;
//            }
//            //return listTask;

//        }
//        /// <summary>
//        /// Created By : Ashwajit Bansod
//        /// Created Date : 13-Nov-2019
//        /// Created For : to get work order unseen list
//        /// </summary>
//        /// <param name="objDetails"></param>
//        /// <returns></returns>
//        public List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails)
//        {
//            var _db = new workorderEMSEntities();
//            var listTask = new List<EmailHelper>();
//            var objEmailHelper = new EmailHelper();
//            var notification = new List<NotificationDetailModel>();
//            var obj = new EmailHelper();
//            try
//            {
//                notification = _db.NotificationDetails.Where(x => x.IsRead == false && x.IsDeleted == false &&  x.AssignUserId == objDetails.UserId || x.AssignUserId == 0 && (x.WorkOrderID > 0)).Select(a => new NotificationDetailModel()
//                {
//                    WorkOrderID = a.WorkOrderID,
//                    AssignTo = a.AssignUserId,
//                    //POID = a.POID,
//                    //BillID = a.BillID,
//                    //MiscellaneousID = a.MiscellaneousID,
//                    //eScanQRCID = a.eScanId,
//                    IsRead = a.IsRead
//                }).ToList();
//                var userDetails = _db.UserRegistrations.Where(x => x.UserId == objDetails.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
//                if (notification.Count() > 0)
//                {
//                    foreach (var item in notification)
//                    {
//                        #region Wo
//                        if (item.WorkOrderID != null && item.WorkOrderID > 0)
//                        {
//                            obj = _db.WorkRequestAssignments.Join(_db.NotificationDetails, q => q.WorkOrderCodeID, u => u.WorkOrderID, (q, u) => new { q, u }).
//                                      Where(x => (x.q.WorkOrderCodeID == item.WorkOrderID) 
//                                      //&& (x.u.IsDeleted == false)
//                                      //&& (x.u.IsRead == false && x.q.IsDeleted == false && (x.q.AssignToUserId == objDetails.UserId || x.q.AssignToUserId == 0)))
//                                      ).Select
//                                      (a => new EmailHelper()
//                                      {
//                                          MailType = a.q.WorkRequestProjectType == 128 ? "WORKORDERREQUESTTOEMPLOYEE" : a.q.WorkRequestProjectType == 129 ? "SPECIALWORKORDERTOEMPLOYEE" : a.q.WorkRequestProjectType == 279 ? "CONTINIOUSREQUEST" : "EMAINTENANCE",
//                                          WorkRequestAssignmentID = a.q.WorkRequestAssignmentID,
//                                          WorkOrderCodeId = a.q.WorkOrderCode + a.q.WorkOrderCodeID.ToString(),
//                                          AssignedTime = a.q.AssignedTime.ToString(),
//                                          ProjectDesc = a.q.ProblemDesc,
//                                          UserName = a.u.AssignUserId.ToString(),
//                                          LocationName = a.q.LocationMaster.LocationName,
//                                          WorkOrderImage = a.q.WorkRequestImage,// == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + a.q.WorkRequestImage,
//                                          WorkRequestType = a.q.WorkRequestType,
//                                          WorkRequestProjectType = a.q.WorkRequestProjectType,
//                                          PriorityLevel = a.q.PriorityLevel,
//                                          WeekDays = a.q.WeekDays,
//                                          StartTime = a.q.StartTime.ToString(),
//                                          EndTime = a.q.EndTime.ToString(),
//                                          ModifiedDate = a.q.ModifiedDate,
//                                          StartDate = a.q.StartDate.ToString(),
//                                          EndDate = a.q.EndDate.ToString(),
//                                          IsWorkable = true,
//                                          CustomerName = a.q.CustomerName,
//                                          VehicleModel1 = a.q.VehicleModel,
//                                          VehicleMake1 = a.q.VehicleMake.ToString(),
//                                          VehicleYear = a.q.VehicleYear.ToString(),
//                                          FrCurrentLocation = a.q.CurrentLocation,
//                                          VehicleColor = a.q.VehicleColor,
//                                          DriverLicenseNo = a.q.DriverLicenseNo,
//                                          CustomerContact = a.q.CustomerContact,
//                                          FacilityRequest = a.q.FacilityRequestId,
//                                          FacilityRequestType = a.q.FacilityRequestId != null || a.q.FacilityRequestId > 0 ?_db.GlobalCodes.Where(x => x.GlobalCodeId == a.q.FacilityRequestId).FirstOrDefault().CodeName :null,
//                                          AddressFacilityReq = a.q.Address,
//                                          LicensePlateNo = a.q.LicensePlateNo
//                                      }).FirstOrDefault();
//                            if (obj != null)
//                            {
//                                obj.WorkOrderImage = obj.WorkOrderImage == null ? HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + obj.WorkOrderImage;
//                                //obj.AssignedTime = obj.AssignedTime.ToString("HH:mm");
//                                ///This is to check if it is manager then the notification make it non workable
//                                ///make sure that WO should be Urgent or facility
//                                if (userDetails != null)
//                                {
//                                    if (userDetails.UserType == Convert.ToInt64(UserType.Manager) &&
//                                      (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.FacilityRequest)
//                                      || obj.PriorityLevel == Convert.ToInt64(WorkRequestPriority.Level1Urgent)))
//                                    {
//                                        if ((obj.StartTime == null || obj.StartTime == "") && (obj.EndTime == null || obj.EndTime == ""))
//                                        {
//                                            obj.ProjectDesc = CommonMessage.WOStatusMessageForManager(obj.WorkOrderCodeId);
//                                            obj.IsWorkable = false;
//                                            obj.Message = "No one accepted faciliy request " + obj.WorkOrderCodeId + " of type" + obj.FacilityRequest + " at location" + obj.LocationName;
//                                        }
//                                        else
//                                        {
//                                            obj.ProjectDesc = null;
//                                        }
//                                    }
//                                }
//                                ///This is for Continues WO to send notification as per days 
//                                if (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.ContinuousRequest))
//                                {
//                                    var dateList = obj.WeekDays.Split(',').ToList();
//                                    var todaysDate = DateTime.Now.ToShortDateString();
//                                    obj.Message = "Continous request " + obj.WorkOrderCodeId + " has not been started after arrived time" + obj.StartTime;
//                                    foreach (var date in dateList)
//                                    {
//                                        if (date == todaysDate)
//                                        {
//                                            listTask.Add(obj);
//                                        }
//                                    }
//                                }
//                                else
//                                {
//                                    listTask.Add(obj);
//                                }
//                            }
//                        }
//                        #endregion WO
//                    }
//                }
//                return listTask.OrderByDescending(x => x.WorkRequestAssignmentID).ToList();
//            }
//            catch (Exception ex)
//            {
//                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails)", "Exception while getting the list of WO for notification details", obj);
//                throw;
//            }
//        }


//        //Corrective Action Meeting Date&Time with Dane T  Grey, 05/20/2020 9:20 AM, 05/20/2020 8:30 AM, 05/14/2020 7:30 AM
//        /// <summary>
//        /// Notification Save
//        /// Corrective Action Meeting Date&Time with Dane  T  Grey 05/20/2020 8:30 AM, 05/14/2020 7:30 AM, 05/21/2020 9:30 AM
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public bool SaveNotification(NotificationDetailModel obj)
//        {
//            bool isSaved = false;
//            try
//            {
//                if (obj != null)
//                {
//                    var saveNotification = Context.spSetNotification("I", null, obj.Message,obj.Module, obj.SubModule, obj.SubModuleId, obj.CreatedByUser, obj.AssignToUser, obj.AssignToIsWorkable, obj.CreatedByIsWorkable, obj.Priority, null, false, "Y");
//                }
//            }
//            catch (Exception ex)
//            {
//                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveNotification(NotificationDetailModel obj)", "Exception while saving the notification details", obj);
//                throw;
//            }
//            return isSaved;
//        }


//    }
//}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class NotificationManager : INotification
    {
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        workorderEMSEntities Context = new workorderEMSEntities();
        private string ApplicantProfilePic = ConfigurationManager.AppSettings["ApplicantSignature"];
        public List<EmailHelper> GetUnseenList(NotificationDetailModel objDetails)
        {
            var _db = new workorderEMSEntities();
            var listTask = new List<EmailHelper>();
            var objEmailHelper = new EmailHelper();
            var notification = new List<NotificationDetailModel>();
            var obj = new EmailHelper();
            try
            {
                notification = _db.NotificationDetails.Where(x => x.IsRead == false && x.IsDeleted == false && x.AssignUserId == objDetails.UserId || x.AssignUserId == 0).Select(a => new NotificationDetailModel()
                {
                    WorkOrderID = a.WorkOrderID,
                    AssignTo = a.AssignUserId,
                    POID = a.POID,
                    BillID = a.BillID,
                    MiscellaneousID = a.MiscellaneousID,
                    eScanQRCID = a.eScanId,
                    IsRead = a.IsRead
                }).ToList();
                var userDetails = _db.UserRegistrations.Where(x => x.UserId == objDetails.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                if (notification.Count() > 0)
                {
                    foreach (var item in notification)
                    {
                        #region Wo
                        if (item.WorkOrderID != null && item.WorkOrderID > 0)
                        {
                            obj = _db.WorkRequestAssignments.Join(_db.NotificationDetails, q => q.WorkRequestAssignmentID, u => u.WorkOrderID, (q, u) => new { q, u }).
                                      Where(x => (x.q.WorkRequestAssignmentID == item.WorkOrderID) && (x.u.IsDeleted == false)
                                      && (x.u.IsRead == false && x.q.IsDeleted == false && (x.q.AssignToUserId == objDetails.UserId || x.q.AssignToUserId == 0))).Select
                                      (a => new EmailHelper()
                                      {
                                          MailType = a.q.WorkRequestProjectType == 128 ? "WORKORDERREQUESTTOEMPLOYEE" : a.q.WorkRequestProjectType == 129 ? "SPECIALWORKORDERTOEMPLOYEE" : a.q.WorkRequestProjectType == 279 ? "CONTINIOUSREQUEST" : "EMAINTENANCE",
                                          WorkRequestAssignmentID = a.q.WorkRequestAssignmentID,
                                          WorkOrderCodeId = a.q.WorkOrderCode + a.q.WorkOrderCodeID.ToString(),
                                          AssignedTime = a.q.AssignedTime.ToString(),
                                          ProjectDesc = a.q.ProblemDesc,
                                          UserName = a.u.AssignUserId.ToString(),
                                          LocationName = a.q.LocationMaster.LocationName,
                                          WorkOrderImage = a.q.WorkRequestImage,// == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + a.q.WorkRequestImage,
                                          WorkRequestType = a.q.WorkRequestType,
                                          WorkRequestProjectType = a.q.WorkRequestProjectType,
                                          PriorityLevel = a.q.PriorityLevel,
                                          WeekDays = a.q.WeekDays,
                                          StartTime = a.q.StartTime.ToString(),
                                          EndTime = a.q.EndTime.ToString(),
                                          ModifiedDate = a.q.ModifiedDate,
                                          StartDate = a.q.StartDate.ToString(),
                                          EndDate = a.q.EndDate.ToString(),
                                          IsWorkable = true,

                                          CustomerName = a.q.CustomerName,
                                          VehicleModel1 = a.q.VehicleModel,
                                          VehicleMake1 = a.q.VehicleMake.ToString(),
                                          VehicleYear = a.q.VehicleYear.ToString(),
                                          FrCurrentLocation = a.q.CurrentLocation,
                                          VehicleColor = a.q.VehicleColor,
                                          DriverLicenseNo = a.q.DriverLicenseNo,
                                          CustomerContact = a.q.CustomerContact,
                                          FacilityRequest = a.q.FacilityRequestId,
                                          AddressFacilityReq = a.q.Address,
                                          LicensePlateNo = a.q.LicensePlateNo
                                      }).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.WorkOrderImage = obj.WorkOrderImage == null ? HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + obj.WorkOrderImage;
                                //obj.AssignedTime = obj.AssignedTime.ToString("HH:mm");
                                ///This is to check if it is manager then the notification make it non workable
                                ///make sure that WO should be Urgent or facility
                                if (userDetails != null)
                                {
                                    if (userDetails.UserType == Convert.ToInt64(UserType.Manager) &&
                                      (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.FacilityRequest)
                                      || obj.PriorityLevel == Convert.ToInt64(WorkRequestPriority.Level1Urgent)))
                                    {
                                        if ((obj.StartTime == null || obj.StartTime == "") && (obj.EndTime == null || obj.EndTime == ""))
                                        {
                                            obj.ProjectDesc = CommonMessage.WOStatusMessageForManager(obj.WorkOrderCodeId);
                                            obj.IsWorkable = false;
                                            obj.Message = "No one accepted faciliy request " + obj.WorkOrderCodeId + " of type" + obj.FacilityRequest + " at location" + obj.LocationName;
                                        }
                                        else
                                        {
                                            obj.ProjectDesc = null;
                                        }
                                    }
                                }
                                ///This is for Continues WO to send notification as per days 
                                if (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.ContinuousRequest))
                                {
                                    var dateList = obj.WeekDays.Split(',').ToList();
                                    var todaysDate = DateTime.Now.ToShortDateString();
                                    obj.Message = "Continous request " + obj.WorkOrderCodeId + " has not been started after arrived time" + obj.StartTime;
                                    foreach (var date in dateList)
                                    {
                                        if (date == todaysDate)
                                        {
                                            //obj.IsWorkable = false;
                                            listTask.Add(obj);
                                        }
                                    }
                                }
                                else
                                {
                                    listTask.Add(obj);
                                }
                            }
                        }
                        #endregion WO

                        #region PO
                        ///This is for PO
                        else if (item.POID != null && item.POID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
                        {
                            var POData = new EmailHelper();
                            if (item.IsRead == false)
                            {
                                POData = _db.LogPODetails.Where(x => x.LPOD_POD_Id == item.POID && x.LPOD_IsActive == "Y")
                                   .Select(a => new EmailHelper()
                                   {
                                       MailType = item.POID != null ? "POAPPROVEDREJECT" : null,
                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                                       LocationName = a.LocationMaster.LocationName,
                                       PONumber = a.LPOD_POD_Id.ToString(),
                                       SentBy = userDetails.UserId,
                                       LocationID = a.LPOD_LocationId,
                                       LogPOId = a.LPOD_Id.ToString(),
                                       Total = a.LPOD_POAmount.ToString(),
                                       CMPId = a.LPOD_CMP_Id,
                                       ApproveRemoveStatus = a.LPOD_IsApprove == "W" ? POStatus.W : a.LPOD_IsApprove == "Y" ? POStatus.Y : POStatus.N,
                                       IsWorkable = true,
                                       Comment = a.LPOD_Comment
                                   }).OrderByDescending(x => x.LogPOId).FirstOrDefault();
                            }
                            //var data = POData.
                            //_db.LogPODetails.Join(_db.NotificationDetails, q => q.LPOD_POD_Id, u => u.POID, (q, u) => new { q, u }).
                            //    Where(x => (x.q.LPOD_POD_Id == item.POID) //&& (x.u.IsDeleted == false)
                            //  /*&& (x.u.IsRead == false)*/).Select
                            //(a => new EmailHelper()
                            //{
                            //  MailType = item.POID != null ? "POAPPROVEDREJECT" : null,
                            //ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                            //LocationName = a.q.LocationMaster.LocationName,
                            //PONumber = a.q.LPOD_POD_Id.ToString(),
                            //SentBy = userDetails.UserId == 0 ? 0: userDetails.UserId,
                            //LocationID = a.q.LPOD_LocationId == 0 ? 0:a.q.LPOD_LocationId,
                            //LogPOId = a.q.LPOD_Id == 0 ? null : a.q.LPOD_Id.ToString(),
                            //Total = a.q.LPOD_POAmount == null ? null :a.q.LPOD_POAmount.ToString(),
                            //CMPId = a.q.LPOD_CMP_Id == null ? null : a.q.LPOD_CMP_Id,
                            //        IsWorkable = true
                            //      }).FirstOrDefault();
                            if (POData != null)
                            {
                                if (POData.ApproveRemoveStatus == POStatus.W)
                                {
                                    POData.Message = "PO " + POData.PONumber + " has been approved by your manager, Current status is " + POData.ApproveRemoveStatus;
                                }
                                else if (POData.ApproveRemoveStatus == POStatus.Y)
                                {
                                    POData.Message = "PO " + POData.PONumber + " received final approval";
                                }
                                else if (POData.Comment != null)
                                {
                                    POData.Message = "PO " + POData.PONumber + " has been rejected due to" + POData.Comment + " , Please contact your manager";
                                }
                                ///To approve PO need company name so added this code just for approval process.
                                if (POData != null && POData.CMPId > 0)
                                {
                                    var companyData = _db.Companies.Where(x => x.CMP_Id == POData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
                                    if (companyData != null)
                                    {
                                        POData.CompanyName = companyData.CMP_NameLegal;
                                        POData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
                                    }
                                }
                                listTask.Add(POData);
                            }
                        }
                        #endregion PO

                        #region Bill
                        else if (item.BillID != null && item.BillID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
                        {
                            var BillData = new EmailHelper();
                            if (item.IsRead == false)
                            {
                                BillData = _db.LogPreBills.Where(x => x.LPBL_PBL_Id == item.BillID && x.LPBL_IsActive == "Y")
                                   .Select(a => new EmailHelper()
                                   {
                                       MailType = item.BillID != null ? "BILLAPPROVE" : null,
                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                                       LocationName = a.LocationMaster.LocationName,
                                       BillId = a.LPBL_PBL_Id.ToString(),
                                       SentBy = userDetails.UserId,
                                       LocationID = a.LPBL_LocationId,
                                       LogBillId = a.LPBL_Id,
                                       Total = a.LPBL_InvoiceAmount.ToString(),
                                       CMPId = a.LPBL_CMP_Id,
                                       IsWorkable = true
                                   }).FirstOrDefault();
                            }
                            if (BillData != null)
                            {
                                BillData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
                                ///To approve PO need company name so added this code just for approval process.
                                if (BillData != null && BillData.CMPId > 0)
                                {
                                    var companyData = _db.Companies.Where(x => x.CMP_Id == BillData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
                                    if (companyData != null)
                                    {
                                        BillData.CompanyName = companyData.CMP_NameLegal;

                                    }
                                }
                                listTask.Add(BillData);
                            }
                            else
                            {
                                BillData = _db.LogBills.Where(x => x.LBLL_POD_Id == item.BillID && x.LBLL_IsApprove == "Y" && x.LBLL_IsActive == "Y")
                                   .Select(a => new EmailHelper()
                                   {
                                       MailType = item.BillID != null ? "BILLAPPROVE" : null,
                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                                       LocationName = a.LocationMaster.LocationName,
                                       BillId = a.LBLL_BLL_Id.ToString(),
                                       SentBy = userDetails.UserId,
                                       LocationID = a.LBLL_LocationId,
                                       LogBillId = a.LBLL_Id,
                                       Total = a.LBLL_InvoiceAmount.ToString(),
                                       CMPId = a.LBLL_CMP_Id,
                                       IsWorkable = true
                                   }).FirstOrDefault();
                            }
                        }
                        #endregion Bill

                        #region Miscellaneous
                        else if (item.MiscellaneousID != null && item.MiscellaneousID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
                        {
                            var MiscData = new EmailHelper();
                            if (item.IsRead == false)
                            {
                                MiscData = _db.LogMiscellaneous.Where(x => x.LMIS_MIS_Id == item.MiscellaneousID && x.LMIS_IsActive == "Y")
                                   .Select(a => new EmailHelper()
                                   {
                                       MailType = item.MiscellaneousID != null ? "APPROVEMISCELLANEOUS" : null,
                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                                       LocationName = a.LocationMaster.LocationName,
                                       MISId = a.LMIS_MIS_Id.ToString(),
                                       SentBy = userDetails.UserId,
                                       LocationID = a.LMIS_LocationId,
                                       LogMiscId = a.LMIS_Id.ToString(),
                                       Total = a.LMIS_InvoiceAmount.ToString(),
                                       CMPId = a.LMIS_CMP_Id,
                                       IsWorkable = true
                                   }).FirstOrDefault();
                            }
                            if (MiscData != null)
                            {
                                MiscData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
                                ///To approve PO need company name so added this code just for approval process.
                                if (MiscData != null && MiscData.CMPId > 0)
                                {
                                    var companyData = _db.Companies.Where(x => x.CMP_Id == MiscData.CMPId && x.CMP_IsActive == "Y").FirstOrDefault();
                                    if (companyData != null)
                                    {
                                        MiscData.CompanyName = companyData.CMP_NameLegal;
                                    }
                                }
                                listTask.Add(MiscData);
                            }
                        }
                        #endregion Miscellaneous

                        #region eScan
                        else if (item.eScanQRCID != null && item.eScanQRCID > 0 && userDetails.UserType != Convert.ToInt64(UserType.Employee))
                        {
                            var eScanData = new EmailHelper();
                            if (item.IsRead == false)
                            {
                                eScanData = _db.QRCMasters.Where(x => x.QRCID == item.eScanQRCID && x.IsDeleted == false)
                                   .Select(a => new EmailHelper()
                                   {
                                       MailType = a.IsDamage == true ? "QRCVEHICLEDAMAGE" : a.CheckOutStatus == true ? "CHECKINCHECKOUT" : null,
                                       ManagerName = userDetails.FirstName + " " + userDetails.LastName,
                                       LocationName = a.LocationMaster.LocationName,
                                       QrCodeId = a.QRCodeID.ToString(),
                                       SentBy = userDetails.UserId,
                                       LocationID = a.LocationId,
                                       IsWorkable = false,
                                       CheckoutUserName = a.UserName
                                   }).FirstOrDefault();
                            }
                            if (eScanData != null)
                            {
                                eScanData.Message = "Someone want to checkout QRC " + eScanData.QrCodeId + " which is already checked out by" + eScanData.CheckoutUserName;
                                eScanData.WorkOrderImage = HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png";
                                ///To approve PO need company name so added this code just for approval process.                        
                                listTask.Add(eScanData);
                            }
                        }
                        #endregion eScan
                    }
                }

                //if(notification.Count() > 0)
                //{
                //    foreach (var item in notification)
                //    {
                //        if(item.WorkOrderID != null && item.WorkOrderID > 0)
                //        {
                //            var dd = _db.WorkRequestAssignments.Where(x => x.WorkRequestAssignmentID == item.WorkOrderID

                //             && x.IsDeleted == false
                //            ).Select(a => new EmailHelper() { 

                //            }).ToList();
                //        }
                //    }
                //}
                return listTask.OrderByDescending(x => x.WorkRequestAssignmentID).OrderByDescending(x => x.PONumber).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<EmailHelper> GetUnseenList(NotificationDetailModel objDetails)", "Exception while getting the list for notification details", obj);
                throw;
            }
            //return listTask;

        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Nov-2019
        /// Created For : to get work order unseen list
        /// </summary>
        /// <param name="objDetails"></param>
        /// <returns></returns>
        public List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails)
        {
            var _db = new workorderEMSEntities();
            var listTask = new List<EmailHelper>();
            var objEmailHelper = new EmailHelper();
            var notification = new List<NotificationDetailModel>();
            var obj = new EmailHelper();
            try
            {
                notification = _db.NotificationDetails.Where(x => x.IsRead == false && x.IsDeleted == false && x.AssignUserId == objDetails.UserId || x.AssignUserId == 0 && (x.WorkOrderID > 0)).Select(a => new NotificationDetailModel()
                {
                    WorkOrderID = a.WorkOrderID,
                    AssignTo = a.AssignUserId,
                    //POID = a.POID,
                    //BillID = a.BillID,
                    //MiscellaneousID = a.MiscellaneousID,
                    //eScanQRCID = a.eScanId,
                    IsRead = a.IsRead
                }).ToList();
                var userDetails = _db.UserRegistrations.Where(x => x.UserId == objDetails.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                if (notification.Count() > 0)
                {
                    foreach (var item in notification)
                    {
                        #region Wo
                        if (item.WorkOrderID != null && item.WorkOrderID > 0)
                        {
                            obj = _db.WorkRequestAssignments.Join(_db.NotificationDetails, q => q.WorkOrderCodeID, u => u.WorkOrderID, (q, u) => new { q, u }).
                                      Where(x => (x.q.WorkOrderCodeID == item.WorkOrderID)
                                      //&& (x.u.IsDeleted == false)
                                      //&& (x.u.IsRead == false && x.q.IsDeleted == false && (x.q.AssignToUserId == objDetails.UserId || x.q.AssignToUserId == 0)))
                                      ).Select
                                      (a => new EmailHelper()
                                      {
                                          MailType = a.q.WorkRequestProjectType == 128 ? "WORKORDERREQUESTTOEMPLOYEE" : a.q.WorkRequestProjectType == 129 ? "SPECIALWORKORDERTOEMPLOYEE" : a.q.WorkRequestProjectType == 279 ? "CONTINIOUSREQUEST" : "EMAINTENANCE",
                                          WorkRequestAssignmentID = a.q.WorkRequestAssignmentID,
                                          WorkOrderCodeId = a.q.WorkOrderCode + a.q.WorkOrderCodeID.ToString(),
                                          AssignedTime = a.q.AssignedTime.ToString(),
                                          ProjectDesc = a.q.ProblemDesc,
                                          UserName = a.u.AssignUserId.ToString(),
                                          LocationName = a.q.LocationMaster.LocationName,
                                          WorkOrderImage = a.q.WorkRequestImage,// == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + a.q.WorkRequestImage,
                                          WorkRequestType = a.q.WorkRequestType,
                                          WorkRequestProjectType = a.q.WorkRequestProjectType,
                                          PriorityLevel = a.q.PriorityLevel,
                                          WeekDays = a.q.WeekDays,
                                          StartTime = a.q.StartTime.ToString(),
                                          EndTime = a.q.EndTime.ToString(),
                                          ModifiedDate = a.q.ModifiedDate,
                                          StartDate = a.q.StartDate.ToString(),
                                          EndDate = a.q.EndDate.ToString(),
                                          IsWorkable = true,
                                          CustomerName = a.q.CustomerName,
                                          VehicleModel1 = a.q.VehicleModel,
                                          VehicleMake1 = a.q.VehicleMake.ToString(),
                                          VehicleYear = a.q.VehicleYear.ToString(),
                                          FrCurrentLocation = a.q.CurrentLocation,
                                          VehicleColor = a.q.VehicleColor,
                                          DriverLicenseNo = a.q.DriverLicenseNo,
                                          CustomerContact = a.q.CustomerContact,
                                          FacilityRequest = a.q.FacilityRequestId,
                                          FacilityRequestType = a.q.FacilityRequestId != null || a.q.FacilityRequestId > 0 ? _db.GlobalCodes.Where(x => x.GlobalCodeId == a.q.FacilityRequestId).FirstOrDefault().CodeName : null,
                                          AddressFacilityReq = a.q.Address,
                                          LicensePlateNo = a.q.LicensePlateNo
                                      }).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.WorkOrderImage = obj.WorkOrderImage == null ? HostingPrefix + "/Content/Images/WorkRequest/no-asset-pic.png" : HostingPrefix + (ConfigurationManager.AppSettings["WorkRequestImage"]).Replace("~", "") + obj.WorkOrderImage;
                                //obj.AssignedTime = obj.AssignedTime.ToString("HH:mm");
                                ///This is to check if it is manager then the notification make it non workable
                                ///make sure that WO should be Urgent or facility
                                if (userDetails != null)
                                {
                                    if (userDetails.UserType == Convert.ToInt64(UserType.Manager) &&
                                      (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.FacilityRequest)
                                      || obj.PriorityLevel == Convert.ToInt64(WorkRequestPriority.Level1Urgent)))
                                    {
                                        if ((obj.StartTime == null || obj.StartTime == "") && (obj.EndTime == null || obj.EndTime == ""))
                                        {
                                            obj.ProjectDesc = CommonMessage.WOStatusMessageForManager(obj.WorkOrderCodeId);
                                            obj.IsWorkable = false;
                                            obj.Message = "No one accepted faciliy request " + obj.WorkOrderCodeId + " of type" + obj.FacilityRequest + " at location" + obj.LocationName;
                                        }
                                        else
                                        {
                                            obj.ProjectDesc = null;
                                        }
                                    }
                                }
                                ///This is for Continues WO to send notification as per days 
                                if (obj.WorkRequestProjectType == Convert.ToInt64(WorkRequestProjectType.ContinuousRequest))
                                {
                                    var dateList = obj.WeekDays.Split(',').ToList();
                                    var todaysDate = DateTime.Now.ToShortDateString();
                                    obj.Message = "Continous request " + obj.WorkOrderCodeId + " has not been started after arrived time" + obj.StartTime;
                                    foreach (var date in dateList)
                                    {
                                        if (date == todaysDate)
                                        {
                                            listTask.Add(obj);
                                        }
                                    }
                                }
                                else
                                {
                                    listTask.Add(obj);
                                }
                            }
                        }
                        #endregion WO
                    }
                }
                return listTask.OrderByDescending(x => x.WorkRequestAssignmentID).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails)", "Exception while getting the list of WO for notification details", obj);
                throw;
            }
        }

        #region New Notification
        public List<NotificationDetailModel> GetNotification(long userId, string Username)
        {

            var _model = new List<NotificationDetailModel>();
            try
            {
                if (userId > 0)
                {
                    var data = Context.Notifications.Where(x => x.NTF_AssignTo == Username || x.NTF_CreatedBy == Username).Select(x => new NotificationDetailModel()
                    {
                        NotificationId = x.NTF_Id,
                        AssignToUser = x.NTF_AssignTo,
                        CreatedByUser = x.NTF_CreatedBy,
                        IsRead = x.NTF_IsRead,
                        SubModule = x.NTF_Submodule,
                        Priority = x.NFT_Severity,
                        AssignToIsWorkable = x.NTF_AssignToIsWorkable,
                        CreatedByIsWorkable = x.NTF_CreatedByIsWorkable,
                        Details = x.NTF_Details,
                        SubModuleId1 = x.NTF_SubmoduleId
                    }).ToList();
                    foreach (var item in data)
                    {
                        var getEMPDetails = Context.Employees.Where(x => x.EMP_EmployeeID == item.AssignToUser && x.EMP_IsActive == "Y").FirstOrDefault();
                        if (item.SubModule == ModuleSubModule.OnBoarding && item.SubModuleId1 != null)
                        {
                            var offerAcceptRejectData = Context.ApplicantLoginAccesses.Join(Context.Applicants, q => q.ALA_UserId, u => u.APT_ALA_UserId, (q, u) => new { q, u }).
                                       Where(x => x.u.APT_ApplicantId == Convert.ToInt64(item.SubModuleId)).FirstOrDefault();
                            if (offerAcceptRejectData != null)
                            {
                                item.OfferAcceptRejectCounterStatus = offerAcceptRejectData.u.APT_IsActive;//== ApplicantIsActiveStatus.OfferAccepted
                                item.ApplicantStatus = offerAcceptRejectData.u.APT_Status;
                                item.Photo = offerAcceptRejectData.q.ALA_Photo == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + ApplicantProfilePic.Replace("~", "") + offerAcceptRejectData.q.ALA_Photo;
                            }
                            _model.Add(item);
                        }
                        else if (item.SubModule == ModuleSubModule.EvaluationComplete || item.SubModule == ModuleSubModule.EvaluationStart || item.SubModule == ModuleSubModule.AssessmentStart && item.SubModuleId1 != null)
                        {
                            
                            if (getEMPDetails != null)
                            {
                                item.Photo = getEMPDetails.EMP_Photo == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + ApplicantProfilePic.Replace("~", "") + getEMPDetails.EMP_Photo;
                                _model.Add(item);
                            }
                        }
                        else if (item.SubModule == ModuleSubModule.InterviewerAcceptDeny && item.SubModuleId1 != null)
                        {
                            _model.Add(item);
                        }
                        else if (item.SubModule == ModuleSubModule.Interviewer && item.SubModuleId1 != null)
                        {
                            _model.Add(item);
                        }
                        else if (item.SubModule == ModuleSubModule.ScheduleMeeting && item.SubModuleId1 != null)
                        {
                            if (getEMPDetails != null)
                            {
                                item.Photo = getEMPDetails.EMP_Photo == null ? HostingPrefix + "/Content/Images/ProjectLogo/no-profile-pic.jpg" : HostingPrefix + ApplicantProfilePic.Replace("~", "") + getEMPDetails.EMP_Photo;
                                _model.Add(item);
                            }
                        }
                    }
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<NotificationDetailModel> GetNotification(long userId)", "Exception while getting the list of notification", userId);
            }
            return _model;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-04-2020
        /// Created For : To make notification readable
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        public bool ReadNotificationById(long NotificationId)
        {
            bool isRead = false;
            var repository = new NotificationRepository();
            try
            {
                if (NotificationId > 0)
                {
                    var getData = Context.Notifications.Where(x => x.NTF_Id == NotificationId && x.NTF_IsRead == false).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.NTF_IsRead = true;
                        repository.Update(getData);
                        repository.SaveChanges();
                    }

                    isRead = true;
                }
                else
                    isRead = false;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ReadNotificationById(long NotificationId)", "Exception while making notification readable", NotificationId);
                throw;
            }
            return isRead;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get applicant details by Applicant id
        /// Created Date : 03-04-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public ApplicantDetails GetApplicantDetails(long ApplicantId)
        {
            var ApplicantDetails = new ApplicantDetails();
            try
            {
                if (ApplicantId > 0)
                {
                    ApplicantDetails = Context.spGetApplicantAllDetails(ApplicantId).
                        Select(x => new ApplicantDetails()
                        {
                            ApplicantName = x.API_FirstName + " " + x.API_LastName,
                            JobTitle = x.JBT_JobTitle,
                            LocationName = x.LocationName,

                        }).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ApplicantDetails GetApplicantDetails(long ApplicantId)", "Exception while getting the details of applicant", ApplicantId);
                throw;
            }
            return ApplicantDetails;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-04-2020
        /// Created For  : To save notification
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveNotification(NotificationDetailModel obj)
        {
            bool isSaved = false;
            try
            {
                if (obj != null)
                {
                    var saveNotification = Context.spSetNotification("I", null, obj.Message,obj.Module, obj.SubModule, obj.SubModuleId, obj.CreatedByUser, obj.AssignToUser, obj.AssignToIsWorkable, obj.CreatedByIsWorkable, obj.Priority, null, false, "Y");
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveNotification(NotificationDetailModel obj)", "Exception while saving the notification details", obj);
                throw;
            }
            return isSaved;
        }
        #endregion New Notification

        /// <summary>
        /// Get meeting date and time from notifications table
        /// Created by: Rajat Toppo
        /// Date: 15-05-2020
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        public NotificationDetailModel NotificationDetailsforMeetingDateTime(NotificationDetailModel obj)
        {
            try
            {
   
                var NotificationTable = Context.Notifications.Where(x => x.NTF_Id == obj.NotificationId).FirstOrDefault();
                var ApplicantIsExempt = Context.Employees.Where(c => c.EMP_EmployeeID == obj.emp_id).FirstOrDefault();
      
                return (new NotificationDetailModel()
                {
                     Details = NotificationTable.NTF_Details,
                     IsExempt = ApplicantIsExempt.EMP_IsExempt,
                     emp_id = ApplicantIsExempt.EMP_EmployeeID

                });
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "NotificationDetailModel NotificationDetailsforMeetingDateTime(string EmpId)", "Exception while getting the details of applicant", obj.NotificationId);
                throw;
            }
            
        }


    }
}
