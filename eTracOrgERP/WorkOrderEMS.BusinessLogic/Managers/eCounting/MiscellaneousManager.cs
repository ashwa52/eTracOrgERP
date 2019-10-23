using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class MiscellaneousManager : IMiscellaneousManager
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string MiscellaneousImagePath = ConfigurationManager.AppSettings["MiscellaneousImage"];
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 16-OCT-2018
        /// Created For : To get location list assign to user 
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<List<LocationServiceModel>> GetLocationAssignedListByUserId(ServiceBaseModel ObjServiceBaseModel, bool IsManager)
        {
            try
            {
                var lstLocation = new List<LocationServiceModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<LocationServiceModel>>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false).FirstOrDefault();
                    //_workorderems.EmployeeLocationMappings.Join(_workorderems.LocationMasters, q => q.LocationId, u => u.LocationId, (q, u) => new { q, u }).
                    //    Where(x => x.q.)
                    if (userData != null)
                    {
                        if (IsManager == false)
                        {
                            lstLocation = _workorderems.EmployeeLocationMappings.Where(x => x.EmployeeUserId == userData.UserId).Select(a => new LocationServiceModel()
                            {
                                LocationId = a.LocationId,
                                LocationName = a.LocationMaster.LocationName
                            }).ToList();
                        }
                        else
                        {
                            lstLocation = _workorderems.ManagerLocationMappings.Where(x => x.ManagerUserId == userData.UserId).Select(a => new LocationServiceModel()
                            {
                                LocationId = a.LocationId,
                                LocationName = a.LocationMaster.LocationName
                            }).ToList();
                        }
                        if (lstLocation.Count > 0)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = lstLocation;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<LocationServiceModel>> GetLocationAssignedListByUserId(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting List of Location Assigned to user.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2018
        /// Created For : To get miscellaneous Cost code List 
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<List<MiscellaneousModel>> GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel)
        {
            try
            {
                var lstMiscellaneous = new List<MiscellaneousModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousModel>>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false).FirstOrDefault();
                    if (userData != null)
                    {
                        lstMiscellaneous = _workorderems.CostCodes.Join(_workorderems.CostCodeMasters, q => q.CCD_CCM_CostCode, u => u.CCM_CostCode, (q, u) => new { q, u }).
                            Where(x => x.u.CCM_Description == "Miscellaneous" && x.q.CCD_IsActive == "Y").
                            Select(x => new MiscellaneousModel()
                            {
                                CostCodeId = x.q.CCD_CostCode,
                                CostCodeName = x.q.CCD_Description
                            }).ToList();

                        if (lstMiscellaneous.Count > 0)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = lstMiscellaneous;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<MiscellaneousModel>> GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting List of Miscellaneous Cost code.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To get miscellaneous number
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<miscellaneousNumberModel> GetMiscellaneousNumberData(ServiceBaseModel ObjServiceBaseModel)
        {
            try
            {
                var MiscellaneousNumber = new miscellaneousNumberModel();
                var ObjServiceResponseModel = new ServiceResponseModel<miscellaneousNumberModel>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false
                                                                        && x.IsEmailVerify == true).FirstOrDefault();
                    if (userData != null)
                    {
                        var number = _workorderems.spGetMisNumber().FirstOrDefault();
                        MiscellaneousNumber.MISNumber = "MIS" + number;
                        if (MiscellaneousNumber.MISNumber != null)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = MiscellaneousNumber;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<miscellaneousNumberModel> GetMiscellaneousNumberData(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting Miscellaneous number.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To save Miscellaneous data
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<MiscellaneousDetails> SaveMiscellaneous(MiscellaneousDetails Obj)
        {
            bool IsSaved = false;
            string Action = "";
            var objDAR = new DARModel();
            List<string> LocationData = new List<string>();
            var CommonManager = new CommonMethodManager();
            string MISID = "";
            string LocationName = "";
            long UserId = 0;
            long Mis_Id = 0;
            decimal TotalAmount = 0;
            try
            {
                var data = new MiscellaneousDetails();
                var ObjServiceResponseModel = new ServiceResponseModel<MiscellaneousDetails>();
                if (Obj.MiscellaneousDetailsmodel.Count > 0)
                {
                    Action = "I";
                    foreach (var item in Obj.MiscellaneousDetailsmodel)
                    {
                        var SaveMiscData = _workorderems.spSetMiscellaneous(Action, item.MISId, item.CostCode, item.LocationId,
                                                                      item.VendorId, item.Discription, item.InvoiceAmount,
                                                                      item.InvoiceDate, item.InvoiceDocument, item.UserId,
                                                                      null, "Y");
                        var locData = _workorderems.LocationMasters.Where(x => x.LocationId == item.LocationId && x.IsDeleted == false).FirstOrDefault();
                        LocationData.Add(locData.LocationName);
                        MISID = "MIS" + item.MISId.ToString();
                        Mis_Id = item.MISId;
                        UserId = item.UserId;
                        TotalAmount += Convert.ToDecimal(item.InvoiceAmount);
                        IsSaved = true;
                    }

                    if (IsSaved == true)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.SaveSuccessMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    #region Save DAR
                    LocationName = string.Join(",", LocationData.ToArray());
                    objDAR.ActivityDetails = DarMessage.MiscellaneousCreated(LocationName, MISID);
                    objDAR.TaskType = (long)TaskTypeCategory.MiscellaneousCreated;
                    objDAR.UserId = UserId;
                    objDAR.CreatedBy = UserId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    CommonManager.SaveDAR(objDAR);
                    #endregion DAR


                    #region Notification
                    var objNotify = new NotificationDetailModel();
                    var _ICommonMethod = new CommonMethodManager();
                    var objModel = new CommonApproval<ApprovalInput>();
                    var approvalInput = new ApprovalInput();
                    approvalInput.Amount = TotalAmount;
                    approvalInput.UserId = UserId;
                    approvalInput.ModuleName = "eCounting";
                    var getRuleData = objModel.GetApprovalRuleData(approvalInput);
                    if (getRuleData != null)
                    {
                        objNotify.CreatedBy = UserId;
                        objNotify.CreatedDate = DateTime.UtcNow;
                        objNotify.AssignTo = getRuleData.UserId;
                        if (MISID != null)
                        {
                            //var id = Convert.ToInt64(MISID);
                            objNotify.MiscellaneousID = Mis_Id;
                        }
                        var saveDataForNotification = _ICommonMethod.SaveNotificationDetail(objNotify);
                    }
                    if (getRuleData.DeviceId != null)
                    {
                        var objEmailHelper = new EmailHelper();
                        var objTemplateModel = new TemplateModel();
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == UserId &&
                                                                             x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        objEmailHelper.emailid = getRuleData.Email;
                        objEmailHelper.ManagerName = getRuleData.ManagerName;
                        objEmailHelper.LocationName = LocationName;
                        if (userData != null)
                        {
                            objEmailHelper.UserName = userData.FirstName + " " + userData.LastName;
                        }
                        objEmailHelper.MISId = MISID;
                        //objEmailHelper.InfractionStatus = obj.Status;
                        objEmailHelper.MailType = "APPROVEMISCELLANEOUS";
                        objEmailHelper.SentBy = UserId;
                        objEmailHelper.LocationID = objDAR.LocationId;
                        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                        objEmailHelper.IsWorkable = true;
                        if (getRuleData.DeviceId != null)
                        {
                            string message = PushNotificationMessages.MiscellaneousCreate(objNotify.BillID.ToString(), objEmailHelper.UserName, objEmailHelper.LocationName);
                            PushNotificationFCM.FCMAndroid(message, getRuleData.DeviceId, objEmailHelper);
                        }
                    }
                    #endregion Notification

                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<miscellaneousNumberModel> GetMiscellaneousNumberData(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting Miscellaneous number.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-OCT-2018
        /// Created For : To get all Miscellaneous list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Location"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public List<MiscellaneousListModel> GetListMiscellaneous(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {

            var result = new List<MiscellaneousListModel>();

            try
            {
                /*
                var objDetails = new MiscellaneousListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                */
                result = _workorderems.spGetMiscellaneousList(Location)
                   .Select(a => new MiscellaneousListModel()
                   {
                       MISId = "MIS" + a.LMIS_MIS_Id,
                       LocationName = a.LocationName,
                       VendorName = a.CMP_NameLegal,
                       InvoiceAmount = a.MISAmount,
                       MISDate = a.MISDate,
                       UserName = a.Employee_Name,
                       Status = a.Status == "W" ? "Pendig" : "Approved"
                   }).ToList();
                /*int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                */
                //Results = Results.OrderByDescending(s => s.CostCode);
                /*objDetails.pageindex = pageindex;
                objDetails.total = totalPages;
                objDetails.records = totRecords;
                objDetails.rows = Results.ToList();
                */
                return result;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public MiscellaneousListDetails GetListMiscellaneous(long? UserId,long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Miscellaneous details.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 23-OCT-2018
        /// Created For : To get all miscellaneous details as per misid.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="MiscId"></param>
        /// <param name="Location"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public List<MiscellaneousListModel> GetListMiscellaneousByMiscId(long? UserId, long? MiscId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {

            var result = new List<MiscellaneousListModel>();

            try
            {
                /*
                var objDetails = new MiscellaneousListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                */
                // var Results = _workorderems.spGetMiscellaneousDetail(MiscId)
                result = _workorderems.spGetMiscellaneousDetail(MiscId)
                    .Select(a => new MiscellaneousListModel()
                    {
                        Status = a.Status,// "w" ? "Pending" : "Approved",
                        MiscStatus = a.Status == "Y" ? true : false,
                        MISId = "MIS" + a.LMIS_MIS_Id,
                        LocationName = a.LocationName,
                        VendorName = a.CMP_NameLegal,
                        InvoiceAmount = a.MISAmount,
                        MISDate = a.MISDate,
                        UserName = a.Employee_Name,
                        Document = a.LMIS_InvoiceDocument,
                        Comment = a.LMIS_Comment,
                        MId = a.LMIS_Id,
                        Vendor = a.LMIS_ModifiedBy,
                        LocationId = a.LMIS_LocationId
                    }).ToList();
                /*
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objDetails.pageindex = pageindex;
                objDetails.total = totalPages;
                objDetails.records = totRecords;
                objDetails.rows = Results.ToList();
                return objDetails;
                */
                return result;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public MiscellaneousListDetails GetListMiscellaneous(long? UserId,long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Miscellaneous details.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-OCT-2018
        /// Created for : To approve miscellaneous data.
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool ApproveMiscellaneous(List<MiscellaneousListModel> Obj, string UserName, long UserId, long LocationId, long MiscQbkId, long VendorDetailsId)
        {
            bool IsApproved = false;
            long MISNumber = 0;
            decimal? calculatedAmt = 0;
            string MISID = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            DateTime date = new DateTime();
            long CreatedBy = 0;
            long Location = 0;
            bool GetOnceData = false;
            var UserDetail = new UserModel();
            var LocationDetail = new LocationMaster();
            string Status = "Approved";
            try
            {
                if (Obj.Count > 0)
                {
                    foreach (var item in Obj)
                    {
                        MISID = item.MISId;
                        string id = item.MISId.Split('S')[1];
                        MISNumber = Convert.ToInt64(id);
                        calculatedAmt += item.InvoiceAmount;
                        Location = item.LocationId;
                        date = Convert.ToDateTime(item.MISDate);
                        CreatedBy = item.Vendor;
                        objDAR.TaskType = (long)TaskTypeCategory.MiscellaneousReject;
                        if (item.Status == "N")
                        {
                            Status = "Reject";
                            calculatedAmt -= item.InvoiceAmount;
                            objDAR.TaskType = (long)TaskTypeCategory.MiscellaneousReject;
                        }
                        var saveApprove = _workorderems.spSetApprovalForMiscellaneous(item.MId, item.Comment, item.Status, UserId);
                        if (GetOnceData == false)
                        {
                            GetOnceData = true;
                            UserDetail = _workorderems.UserRegistrations.Join(_workorderems.LogMiscellaneous, u => u.UserId, m => m.LMIS_ModifiedBy, (u, m) => new { u, m }).
                            Where(x => x.m.LMIS_MIS_Id == MISNumber).
                            Select(x => new UserModel()
                            {
                                UserName = x.u.FirstName + " " + x.u.LastName,
                                Email = x.u.UserEmail,
                                Location = x.m.LMIS_LocationId
                            }).FirstOrDefault();
                            LocationDetail = _workorderems.LocationMasters.Where(x => x.LocationId == UserDetail.Location
                                                               && x.IsDeleted == false).FirstOrDefault();
                        }

                        #region Save DAR
                        objDAR.ActivityDetails = DarMessage.MiscellaneousApproveReject(LocationDetail.LocationName, MISID, Status);
                        objDAR.UserId = UserId;
                        objDAR.CreatedBy = UserId;
                        objDAR.LocationId = UserDetail.Location;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        CommonManager.SaveDAR(objDAR);
                        #endregion DAR
                    }
                    string Action = "I";
                    var saveBill = _workorderems.spSetBill(Action, MiscQbkId, null, MISNumber, null, VendorDetailsId, Location, "MIS", calculatedAmt,
                                                          calculatedAmt, date, null, CreatedBy, null, "Y");
                    IsApproved = true;

                }
                else
                {
                    IsApproved = true;
                }
                if (IsApproved == true)
                {
                    #region Email
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    if (IsApproved == true)
                    {
                        var userData = _workorderems.UserRegistrations.Join(_workorderems.LogMiscellaneous, u => u.UserId, m => m.LMIS_ModifiedBy, (u, m) => new { u, m }).
                        Where(x => x.m.LMIS_MIS_Id == MISNumber).FirstOrDefault();
                        //Select(x => new UserModel()
                        //{
                        //    UserName = x.u.FirstName + " " + x.u.LastName,
                        //    Email = x.u.UserEmail,
                        //    Location = x.m.LMIS_LocationId,

                        //}).FirstOrDefault();
                        var locationName = _workorderems.LocationMasters.Where(x => x.LocationId == userData.m.LMIS_LocationId
                                                           && x.IsDeleted == false).FirstOrDefault();
                        if (userData != null)
                        {

                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = userData.u.SubscriptionEmail;
                            objEmailHelper.LocationName = locationName.LocationName;
                            objEmailHelper.UserName = userData.u.FirstName + " " + userData.u.LastName;
                            objEmailHelper.MISId = MISID;
                            objEmailHelper.MailType = "APPROVEMISCELLANEOUS";
                            objEmailHelper.SentBy = UserId;
                            objEmailHelper.LocationID = userData.m.LMIS_LocationId;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();

                            //Push Notification     
                            if (userData.u.DeviceId != null)
                            {
                                string message = PushNotificationMessages.MiscellaneousApprovedReject(objEmailHelper.LocationName, MISID, Status, objEmailHelper.UserName);
                                PushNotification.GCMAndroid(message, userData.u.DeviceId, objEmailHelper);
                            }
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = userData.m.LMIS_LocationId;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = UserId;
                                    objEmailog.SentEmail = userData.u.SubscriptionEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = userData.u.UserId;
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
                                context.SaveChanges();
                            }
                            #endregion Email
                        }



                    }
                    else
                    {
                        IsApproved = true;
                    }
                }
                return IsApproved;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public MiscellaneousListDetails GetListMiscellaneous(long? UserId,long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Miscellaneous details.", null);
                throw;
            }
        }

        public ServiceResponseModel<List<LocationServiceModel>> GetLocationList(ServiceBaseModel ObjServiceBaseModel)
        {
            try
            {
                var lstLocation = new List<LocationServiceModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<LocationServiceModel>>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false).FirstOrDefault();
                    //_workorderems.EmployeeLocationMappings.Join(_workorderems.LocationMasters, q => q.LocationId, u => u.LocationId, (q, u) => new { q, u }).
                    //    Where(x => x.q.)
                    if (userData != null)
                    {
                        lstLocation = _workorderems.LocationMasters.Where(x => x.IsDeleted == false).Select(a => new LocationServiceModel()
                        {
                            LocationId = a.LocationId,
                            LocationName = a.LocationName
                        }).ToList();

                        if (lstLocation.Count > 0)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = lstLocation;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<LocationServiceModel>> GetLocationAssignedListByUserId(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting List of Location Assigned to user.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : Nov-13-2018
        /// Created For : To get list of all miscellaneous saved by employee.
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<List<MiscellaneousListModel>> GetAllMiscellaneousListForManager(MiscellaneousServiceModel ObjServiceBaseModel)
        {
            try
            {
                var lstMiscellaneous = new List<MiscellaneousListModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousListModel>>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false).FirstOrDefault();
                    if (userData != null)
                    {
                        lstMiscellaneous = _workorderems.spGetMiscellaneousList(ObjServiceBaseModel.LocationId)
                        .Select(a => new MiscellaneousListModel()
                        {
                            MISId = "MIS" + a.LMIS_MIS_Id,
                            LocationName = a.LocationName,
                            VendorName = a.CMP_NameLegal,
                            InvoiceAmount = a.MISAmount,
                            MISDate = a.MISDate,
                            UserName = a.Employee_Name,
                            Status = a.Status == "W" ? "Pending" : a.Status == "N" ? "Pending" : "Approved",
                        }).ToList();
                        if (lstMiscellaneous.Count > 0)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = lstMiscellaneous;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<MiscellaneousModel>> GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting List of Miscellaneous Cost code.", null);
                throw;
            }
        }

        public ServiceResponseModel<List<MiscellaneousListModel>> GetAllChildeMiscellaneousListForManagerByMiscId(MiscellaneousServiceModel ObjServiceBaseModel)
        {
            try
            {
                var lstMiscellaneous = new List<MiscellaneousListModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<MiscellaneousListModel>>();
                if (ObjServiceBaseModel != null && ObjServiceBaseModel.ServiceAuthKey != null)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey
                                                                        && x.IsDeleted == false).FirstOrDefault();
                    if (userData != null)
                    {
                        lstMiscellaneous = _workorderems.spGetMiscellaneousDetail(ObjServiceBaseModel.MISCID)
                        .Select(a => new MiscellaneousListModel()
                        {
                            Status = a.Status,// "w" ? "Pending" : "Approved",
                            MISId = "MIS" + a.LMIS_MIS_Id,
                            LocationName = a.LocationName,
                            VendorName = a.CMP_NameLegal,
                            InvoiceAmount = a.MISAmount,
                            MISDate = a.MISDate,
                            UserName = a.Employee_Name,
                            Document = a.LMIS_InvoiceDocument == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + MiscellaneousImagePath.Replace("~", "") + a.LMIS_InvoiceDocument,
                            Comment = a.LMIS_Comment,
                            MId = a.LMIS_Id,
                            Vendor = a.LMIS_ModifiedBy,
                            LocationId = a.LMIS_LocationId
                        }).ToList();
                        if (lstMiscellaneous.Count > 0)
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.Successful();
                            ObjServiceResponseModel.Data = lstMiscellaneous;
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.NoRecordMessage();
                        ObjServiceResponseModel.Data = null;
                    }
                    return ObjServiceResponseModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<MiscellaneousModel>> GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel)", "Exception While Getting List of Miscellaneous Cost code.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 27-Dec-2018
        /// Created For : To get miscellaneous cost code by misc Id.
        /// </summary>
        /// <param name="MISNumber"></param>
        /// <returns></returns>
        public MiscellaneousDetailsmodel MiscellaneoousDataById(long MISNumber)
        {
            var data = new MiscellaneousDetailsmodel();
            try
            {
                if (MISNumber > 0)
                {
                    data = _workorderems.Miscellaneous.Where(x => x.MIS_Id == MISNumber).Select(a => new MiscellaneousDetailsmodel()
                    {
                        CostCode = a.MIS_CCD_CostCode
                    }).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string GetCostCodeData(long CostCodeId)", "Exception While Getting data of cost code.", null);
                throw;
            }
            return data;
        }
    }
}
