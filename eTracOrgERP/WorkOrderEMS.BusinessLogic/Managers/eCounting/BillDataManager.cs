using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class BillDataManager : IBillDataManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 12-OCT-2018
        /// Created For : To Save Bill
        /// </summary>
        /// <param name="objBillDataServiceModel"></param>
        /// <returns></returns>
        public bool SaveBillDetails(BillDataServiceModel objBillDataServiceModel)
        {
            bool isBillSaved = false;
            string Action = "";
            string DeliverStatus = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            string LocationName = string.Empty;
            string UserName = string.Empty;
            try
            {
                if (objBillDataServiceModel != null && objBillDataServiceModel.VendorId > 0
                     && objBillDataServiceModel.LocationId > 0  && objBillDataServiceModel.UserId > 0)
                {
                    Action = "I";
                    if (objBillDataServiceModel.PODId > 0)
                    {
                            DeliverStatus = "Y";
                            var saveBillData = _workorderems.spSetBill(Action, objBillDataServiceModel.QuickBookBillId, objBillDataServiceModel.PODId, null,null,
                                                                       objBillDataServiceModel.VendorId, objBillDataServiceModel.LocationId,
                                                                       objBillDataServiceModel.BillType, objBillDataServiceModel.PoMisBdaAmount,
                                                                       objBillDataServiceModel.InvoiceAmount, objBillDataServiceModel.InvoiceDate, objBillDataServiceModel.InvoiceDocument,
                                                                       objBillDataServiceModel.UserId, null, DeliverStatus);

                        string[] Quantity = { };
                        string[] POF = { };
                        string[] ActiveId = objBillDataServiceModel.IsActive.Split(',');
                        if(objBillDataServiceModel.Quantity != null)
                        {
                            Quantity = objBillDataServiceModel.Quantity.Split(',');
                        }
                        if(objBillDataServiceModel.POF_ID != null)
                        {
                             POF = objBillDataServiceModel.POF_ID.Split(',');
                        }
                       
                        if (objBillDataServiceModel.IsActive != "")
                        {
                            for (int i = 0; i < ActiveId.Length; i++)
                            {
                                if (ActiveId[i] != null)
                                {
                                    //ActiveId[i] = "N";
                                    Action = "U";
                                    long POF_ID = Convert.ToInt64(POF[i]);
                                    long QantityData = Convert.ToInt64(Quantity[i]);
                                    long FacilityId = Convert.ToInt64(ActiveId[i]);
                                    DeliverStatus = "N";
                                    var saveFacilityData = _workorderems.spSetPOFacilityItem(Action, POF_ID, objBillDataServiceModel.PODId, FacilityId,
                                                                         QantityData, DeliverStatus);
                                }
                            }
                        }
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objBillDataServiceModel.UserId &&
                                                                             x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == objBillDataServiceModel.LocationId
                                                                            && x.IsDeleted == false).FirstOrDefault();
                        #region Save DAR
                        LocationName = locationData.LocationServices.ToString();
                        UserName = userData.FirstName + " " + userData.LastName;
                        objDAR.ActivityDetails = DarMessage.BillCreated(userData.FirstName+""+ userData.LastName, locationData.LocationName, objBillDataServiceModel.PODId);
                        objDAR.TaskType = (long)TaskTypeCategory.BillCreated;
                        objDAR.UserId = objBillDataServiceModel.UserId;
                        objDAR.CreatedBy = objBillDataServiceModel.UserId;
                        objDAR.LocationId = objBillDataServiceModel.LocationId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        CommonManager.SaveDAR(objDAR);
                        #endregion DAR
                    }
                    else
                    {
                        var savePreBill = _workorderems.spSetPreBill(Action, objBillDataServiceModel.BillNumber, objBillDataServiceModel.LocationId,
                                                                  objBillDataServiceModel.VendorId, objBillDataServiceModel.Comment,
                                                                  objBillDataServiceModel.InvoiceAmount, objBillDataServiceModel.InvoiceDate,
                                                                  objBillDataServiceModel.InvoiceDocument, objBillDataServiceModel.UserId,
                                                                  null,"N");
                        if(objBillDataServiceModel.FacilityListForManualBill != null && objBillDataServiceModel.FacilityListForManualBill.Count() > 0)
                        {
                            foreach (var item in objBillDataServiceModel.FacilityListForManualBill)
                            {
                                if (item.UnitPrice > 0)
                                {
                                    var saveFacility = _workorderems.spSetBillFacilityItem(Action, null, objBillDataServiceModel.BillNumber,
                                                                                  item.COM_FacilityId, item.Quantity, item.UnitPrice, item.Status);
                                }
                            }
                        }

                        //var saveBillData = _workorderems.spSetBill(Action,null, null, null,
                        //                                        objBillDataServiceModel.VendorId, objBillDataServiceModel.LocationId,
                        //                                        objBillDataServiceModel.BillType, null,
                        //                                        objBillDataServiceModel.InvoiceAmount, objBillDataServiceModel.InvoiceDate, objBillDataServiceModel.InvoiceDocument,
                        //                                        objBillDataServiceModel.UserId, null, "Y");
                    }

                    var objNotify = new NotificationDetailModel();
                    var _ICommonMethod = new CommonMethodManager();
                    var objModel = new CommonApproval<ApprovalInput>();
                    var approvalInput = new ApprovalInput();
                    approvalInput.Amount = Convert.ToDecimal(objBillDataServiceModel.InvoiceAmount);
                    approvalInput.UserId = objBillDataServiceModel.UserId;
                    approvalInput.ModuleName = "eCounting";
                    var getRuleData = objModel.GetApprovalRuleData(approvalInput);
                    if(getRuleData != null)
                    {
                        objNotify.CreatedBy = objBillDataServiceModel.UserId;
                        objNotify.CreatedDate = DateTime.UtcNow;
                        objNotify.AssignTo = getRuleData.UserId;
                        if (objBillDataServiceModel.PODId > 0)
                        {
                            objNotify.BillID = objBillDataServiceModel.PODId;
                        }
                        else
                        {
                            objNotify.BillID = objBillDataServiceModel.BillNumber;
                        }
                        var saveDataForNotification = _ICommonMethod.SaveNotificationDetail(objNotify);
                    }
                 

                    if (getRuleData.DeviceId != null)
                    {
                        var objEmailHelper = new EmailHelper();
                        var objTemplateModel = new TemplateModel();
                        objEmailHelper.emailid = getRuleData.Email;
                        objEmailHelper.ManagerName = getRuleData.ManagerName;
                        objEmailHelper.LocationName = LocationName;
                        objEmailHelper.UserName = UserName;
                        objEmailHelper.BillId = objNotify.BillID.ToString();
                        //objEmailHelper.InfractionStatus = obj.Status;
                        objEmailHelper.MailType = "BILLAPPROVE";
                        objEmailHelper.SentBy = objBillDataServiceModel.UserId;
                        objEmailHelper.LocationID = objDAR.LocationId;
                        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();

                        objEmailHelper.IsWorkable = true;
                        string message = PushNotificationMessages.BillCreate(objNotify.BillID.ToString(), objEmailHelper.UserName, objEmailHelper.LocationName);
                        PushNotificationFCM.FCMAndroid(message, getRuleData.DeviceId, objEmailHelper);
                    }
                    isBillSaved = true;
                }
                else
                {
                    isBillSaved = false;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BillDataServiceModel SaveBillDetails(BillDataServiceModel objBillDataServiceModel)", "Exception While Saving Bill Details.", null);
                throw;
            }
            return isBillSaved;
        }

        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date:19-OCT-2018
        /// Created For : to get list of bill
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
        //public BillListApproveDetails GetListBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        //{
        //    try
        //    {
        //        var objBillListApproveDetails = new BillListApproveDetails();
        //        int pageindex = Convert.ToInt32(pageIndex) - 1;
        //        int pageSize = Convert.ToInt32(numberOfRows);
        //        var Results = _workorderems.spGetBillList(Location)
        //            .Select(a => new BillListApproveModel()
        //            {
        //                LBLL_Id = a.LBLL_Id,
        //                BillId = a.LBLL_BLL_Id,
        //                BillAmount = a.LBLL_InvoiceAmount,
        //                BillDate = a.BillDate,
        //                InvoiceDate = a.LBLL_InvoiceDate,
        //                Comment = a.LBLL_Comment,
        //                Status = a.Bill_Status,
        //                VendorName = a.CMP_NameLegal,
        //                VendorType = a.VDT_VendorType,
        //                //BillType = a.LBLL_BillType,  
        //                BillImage = a.LBLL_InvoiceDocument,
        //                LocationName = a.LocationName
        //            }).OrderByDescending(x => x.BillId).ToList();
        //        int totRecords = Results.Count();
        //        var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
        //        //Results = Results.OrderByDescending(s => s.CostCode);
        //        objBillListApproveDetails.pageindex = pageindex;
        //        objBillListApproveDetails.total = totalPages;
        //        objBillListApproveDetails.records = totRecords;
        //        objBillListApproveDetails.rows = Results.ToList();
        //        return objBillListApproveDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BillListApproveDetails GetListBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Bill details.", null);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date:19-OCT-2018
        /// Created For : to get list of bill
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
        public List<BillListApproveModel> GetListPreBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var resultList = new List<BillListApproveModel>();

                //var objBillListApproveDetails = new BillListApproveDetails();
                //int pageindex = Convert.ToInt32(pageIndex) - 1;
                //int pageSize = Convert.ToInt32(numberOfRows);
                resultList = _workorderems.spGetPreBillList(Location)
                    .Select(a => new BillListApproveModel()
                    {
                        BillId = a.LPBL_PBL_Id,
                        BillAmount = a.PBLAmount,
                        VendorName = a.VendorName,
                        EmployeeName = a.Employee_Name,
                        LocationName  = a.LocationName,
                        Comment = a.LPBL_Comment,
                        VendorType = a.VDT_VendorType,
                        BillImage = a.LPBL_InvoiceDocument,
                        BillDate = a.PBLDate,
                        Status = a.Status,
                        VendorId = a.VendorId,
                        LBLL_Id = a.LPBL_Id
                    }).OrderByDescending(x => x.BillId).ToList(); // 
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                ////Results = Results.OrderByDescending(s => s.CostCode);
                //objBillListApproveDetails.pageindex = pageindex;
                //objBillListApproveDetails.total = totalPages;
                //objBillListApproveDetails.records = totRecords;
                //objBillListApproveDetails.rows = Results.ToList();
                return resultList;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BillListApproveDetails GetListBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Bill details.", null);
                throw;
            }
        }


        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-OCT-2018
        /// Created For : To approve and reject bill
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="UserName"></param>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public string ApproveBill(BillListApproveModel Obj, string UserName, long UserId,long LocationId)
        {
            bool IsApproved = false;
            string result = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            string ApproveStatus = "";
            try
            {
                var userDataItem = _workorderems.UserRegistrations.Where(x => x.UserId == UserId &&
                                                                             x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                if (Obj != null)
                {
                    string Action = "I";
                    if (Obj.Comment == null)
                    {
                        Obj.Status = "Y";
                        ApproveStatus = "Approved";
                    }
                    else
                    {
                        Obj.Status = "N";
                        ApproveStatus = "Rejected";
                    }
                    var saveBillApprove = _workorderems.spSetApprovalForPreBill(Obj.LBLL_Id, Obj.Comment,Obj.Status, UserId);
                    //var saveApprove = _workorderems.spSetApprovalForBill(Obj.LBLL_Id, Obj.Comment,Obj.Status,UserId);
                    Obj.InvoiceDate = Convert.ToDateTime(Obj.BillDate);
                    var saveSetBill = _workorderems.spSetBill(Action,Obj.QuickBookBillId,null,null, Obj.BillId, Obj.VendorId,LocationId,
                                                               "Manual Bill",null, Obj.BillAmount,Obj.InvoiceDate,
                                                               Obj.BillImage,UserId,UserId,"Y");
                   
                    IsApproved = true;
                    if (Obj.Comment == null)
                    {
                        result = CommonMessage.BillApprove();
                    }
                    else
                    {
                        result = CommonMessage.BillReject();
                    }
                    #region Save DAR
                    
                    var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == LocationId
                                                                            && x.IsDeleted == false).FirstOrDefault();
                    objDAR.ActivityDetails = DarMessage.BillApprovedReject(userDataItem.FirstName + "" + userDataItem.LastName, locationData.LocationName, ApproveStatus);
                    objDAR.TaskType = (long)TaskTypeCategory.BillApprovedReject;
                    objDAR.UserId = UserId;
                    objDAR.CreatedBy = UserId;
                    objDAR.LocationId = LocationId;
                    objDAR.CreatedOn = DateTime.UtcNow;
                    CommonManager.SaveDAR(objDAR);
                    #endregion DAR
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
                        var userData = _workorderems.spGetVendorAllDetail(Obj.VendorId).
                            Select(x => new BillListApproveModel()
                            {
                                Email = x.COD_Email,
                            }).FirstOrDefault();
                        if (userData != null)
                        {
                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = userData.Email;
                            objEmailHelper.LocationName = Obj.LocationName;
                            objEmailHelper.VendorName = Obj.VendorName;
                            objEmailHelper.UserName = userDataItem.FirstName + " " + userDataItem.LastName;
                            objEmailHelper.MailType = "APPROVEBILL";
                            objEmailHelper.SentBy = UserId;
                            objEmailHelper.LocationID = LocationId;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
                            var userDataApprove = _workorderems.LogBills.Join(_workorderems.UserRegistrations, q => q.LBLL_ModifiedBy, u => u.UserId, (q, u) => new { q, u }).
                            Where(x => x.q.LBLL_Id == Obj.LBLL_Id).FirstOrDefault();
                            //Push Notification
                            if (userDataApprove != null)
                            {
                                if (userDataApprove.u.DeviceId != null)
                                {
                                    string message = PushNotificationMessages.BillApprovedReject(objEmailHelper.LocationName, Obj.LBLL_Id, ApproveStatus, objEmailHelper.UserName);
                                    PushNotification.GCMAndroid(message, userDataApprove.u.DeviceId, objEmailHelper);
                                }
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
                                    objEmailog.LocationId = LocationId;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = UserId;
                                    objEmailog.SentEmail = userData.Email;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = Obj.VendorId;
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
                        result = "Somethinsg goes wrong.";
                    }
                }
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
        /// Created Date : 19-Dec-2018
        /// Created For : To get cos code data by cost code Id
        /// </summary>
        /// <param name="CostCodeId"></param>
        /// <returns></returns>
        public CostCodeModel GetCostCodeData(long CostCodeId)
        {
            var data = new CostCodeModel();
            try
            {
                if(CostCodeId > 0)
                {
                    data = _workorderems.CostCodes.Where(x => x.CCD_CostCode == CostCodeId).Select(a => new CostCodeModel
                    {
                        Description = a.CCD_Description
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

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Dec-2018
        /// Created For : To get Pre Bill Number 
        /// </summary>
        /// <param name="ObjServiceBaseModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<BillNumberData> GetPreBillNumberData(ServiceBaseModel ObjServiceBaseModel)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<BillNumberData>();
            var objCommonRepository = new CommonRepository();
            var ObjUserRepository = new UserRepository();
            var billNumberValue = new BillNumberData();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == ObjServiceBaseModel.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    var BillNumber = _workorderems.spGetPreBillNumber().FirstOrDefault();
                    billNumberValue.BillNumber = BillNumber;
                    if (BillNumber != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = billNumberValue;
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = billNumberValue;
                        ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    }
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<int?> GetPreBillNumberData(ServiceBaseModel ObjServiceBaseModel)", "Exception while gettig Bill number", ObjServiceBaseModel);
                throw ex;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>
        /// Created By : ashwajit Bansod
        /// Created For : To get all bill facility list by Bill id.
        /// created Date : 24-Dec-2018
        /// </summary>
        /// <param name="BillId"></param>
        /// <returns></returns>
        public List<BillFacilityModel> GetAllBillFacilityListById(long BillId)
        {
            var listBill = new List<BillFacilityModel>();
            try
            {
                if (BillId > 0)
                {
                    listBill = _workorderems.spGetBillFacilityItemForApproval(BillId).Select(a => new BillFacilityModel
                    {
                        BillFacilityId = a.BFI_Id,
                        CompanyFacilityId = a.CFM_Id,
                        CostCodeId = a.CFM_CCD_CostCode,
                        FacilityType = a.CFM_FacilityType == "1" ? "Product":"Service",
                        Amount = a.CFM_Rate,
                        Tax = a.CFM_Tax,
                        Description = a.CFM_Discription,
                        BillUnit = a.BFI_Unit,
                        IsActive = a.BFI_IsActive,
                        CostCodeQuickBookId = a.CCD_QBKId,
                        CostCoseDescription = a.CCD_Description
                    }).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<BillFacilityModel> GetAllBillFacilityListById(long BillId)", "Exception While Getting list of Bill.", BillId);
                throw;
            }
            return listBill;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get location data a per location Id
        /// Created Date : 27-Dec-2018
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public LocationDataModel GetLocationDataByLocId(long LocationId)
        {
            var data = new LocationDataModel();
            try
            {
                if (LocationId > 0)
                {
                    data = _workorderems.LocationMasters.Where(x => x.LocationId == LocationId && x.IsDeleted == false).Select(a => new LocationDataModel()
                    {
                         LocationId = a.LocationId,
                        LocationName = a.LocationName,
                        QBK_Id = a.QuickBookLocId
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


        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-Jan-2018
        /// Created For  : To get facility Data as per facility Id
        /// </summary>
        /// <param name="FacilityId"></param>
        /// <returns></returns>
        public FacilityListData GetFacilityDataByFacilityId(string FacilityId)
        {
            var listPOFacility = new FacilityListData();
            try
            {
                if (FacilityId != null)
                {
                    long Fac_Id = 150097; //Convert.ToInt64(FacilityId);
                    listPOFacility = _workorderems.CompanyFacilityMappings.Join(_workorderems.POFacilityItems, q => q.CFM_Id, u => u.POF_CFM_Id, (q, u) => new { q, u }).
                            Where(x => x.q.CFM_Id == Fac_Id).Select(a => new FacilityListData
                            //listPOFacility = _workorderems.CompanyFacilityMappings.Where(x => x.CFM_Id == Fac_Id).Select(a => new FacilityListData
                    {
                        COM_FacilityId = a.q.CFM_Id,
                        COM_Facility_Desc = a.q.CFM_Discription,
                        CostCode = a.q.CFM_CCD_CostCode,
                        UnitPrice = a.q.CFM_Rate,
                        Tax = a.q.CFM_Tax,
                        FacilityType = a.q.CFM_FacilityType,
                        Quantity = a.u.POF_Unit
                    }).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public FacilityListData GetFacilityDataByFacilityId(string FacilityId)", "Exception While Getting data of po facility Item.", FacilityId);
                throw;
            }
            return listPOFacility;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Jan-2018
        /// Created For : To get Quick book bill id.
        /// </summary>
        /// <param name="BillId"></param>
        /// <returns></returns>
        public long GetBillQBKId(long BillId)
        {
            long bllQbk_Id = 0;
            var data = new BillDataServiceModel();
            try
            {
                if (BillId > 0)
                {
                    data = _workorderems.Bills.Where(x => x.BLL_Id == BillId).Select(a => new BillDataServiceModel
                    {
                       QuickBookBillId = a.BLL_QBKId
                    }).FirstOrDefault();
                    bllQbk_Id = data.QuickBookBillId;
                }
                else
                {
                    return bllQbk_Id;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public long GetBillQBKId(long BillId)", "Exception While Getting quickbook Id of bill.", BillId);
                throw;
            }
            return bllQbk_Id;
        }
    }
}
