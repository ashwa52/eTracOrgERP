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
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class POTypeDetailsManager : IPOTypeDetails
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string POEmeregencyImagePath = ConfigurationManager.AppSettings["POEmeregencyImage"];
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public List<VendorSetupManagementModel> GetCompany_VendorList(long Location, bool IsRecurring)
        {
            try
            {
                var lstVendor = new List<VendorSetupManagementModel>();
                if (IsRecurring == false)
                {
                    lstVendor = _workorderems.spGetCompanyList(Location).Select(x => new VendorSetupManagementModel()
                    {
                        CompanyNameLegal = x.CMP_NameLegal,
                        CompanyId = x.CMP_Id,
                        IsReccoring = x.CNT_IsReoccurring
                    }).ToList();
                }
                else
                {
                    lstVendor = _workorderems.spGetCompanyList(Location).Select(x => new VendorSetupManagementModel()
                    {
                        CompanyNameLegal = x.CMP_NameLegal,
                        CompanyId = x.CMP_Id,
                        IsReccoring = x.CNT_IsReoccurring
                    }).Where(x => x.IsReccoring != null).ToList();
                }
                return lstVendor;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<VendorSetupManagementModel> GetCompany_VendorList()", "Exception While Getting List of Company.", null);
                throw;
            }
        }

        public List<POTyeDetailsModelService> POTypeList()
        {
            try
            {
                var lstPotype = new List<POTyeDetailsModelService>();
                lstPotype = _workorderems.spGetPOType().Select(a => new POTyeDetailsModelService()
                {
                    POType = a.POT_Id,
                    POTypeName = a.POT_POType
                }).ToList();
                return lstPotype;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<POTyeDetailsModelService> POTypeList()", "Exception While Getting List of PO Type.", null);
                throw;
            }

        }
        public List<VendorSetupManagementModel> GetCompanyDetailsListByCompanyId(long VendorId)
        {
            try
            {
                var lstVendor = new List<VendorSetupManagementModel>();
                lstVendor = _workorderems.spGetCompanyDetailForReoccurringPO(VendorId).Select(x => new VendorSetupManagementModel()
                {
                    Address2 = x.COD_Address2,
                    PointOfContact = x.COD_PointOfContact,
                    CostDuringPeriod = x.CNT_CostDuringPeriod,
                    InvoicingFrequency = x.CNT_invoicingFrequency
                }).ToList();
                return lstVendor;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<VendorSetupManagementModel> GetCompany_VendorList()", "Exception While Getting List of Company.", null);
                throw;
            }
        }
        public string PONumberData()
        {
            string poNumber = "";
            var poNum = _workorderems.spGetPONumber().FirstOrDefault();
            poNumber = "PO" + poNum.ToString();
            return poNumber;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-17-2018
        /// Created For : To Get PO type list and location list
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        public POTypeServiceModel AllPOTypeList(ServiceBaseModel objServiceBaseModel)
        {
            try
            {
                var lstVendor = new POTypeServiceModel();
                var lstPoType = new POTypeServiceModel();
                string poNumber = "";
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                        && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objServiceBaseModel.UserId
                                                                       && x.IsDeleted == false).FirstOrDefault();
                    var poNum = _workorderems.spGetPONumber().FirstOrDefault();
                    poNumber = "PO" + poNum.ToString();
                    if (userData != null)
                    {
                        lstPoType.PONumber = poNumber;
                        lstPoType.POTypeListServiceModel = _workorderems.spGetPOType().Select(x => new POTyeDetailsModelService()
                        {
                            POType = x.POT_Id,
                            POTypeName = x.POT_POType,
                        }).ToList();

                        lstPoType.LocationListServiceModel = _workorderems.LocationMasters.Where(x => x.IsDeleted == false).
                            Select(ll => new LocationListServiceModel()
                            {
                                LocationId = ll.LocationId,
                                LocationName = ll.LocationName
                            }).ToList();

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return lstPoType;
                }
                return lstPoType;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<POTypeServiceModel> AllPOTypeList(ServiceBaseModel objServiceBaseModel)", "Exception While Getting List of Company & Location.", null);
                throw;
            }

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-17-2018
        /// Created For : To get Company list by location Id
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        public List<VendorTypeListServiceModel> GetCompany_VendorListByLocationId(ServiceBaseModel objServiceBaseModel)
        {
            try
            {
                //var lstVendor = new List<VendorTypeListServiceModel>();
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                        && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    var lstVendor = _workorderems.spGetCompanyList(objServiceBaseModel.LocationID).Select(x => new VendorTypeListServiceModel()
                    {
                        CompanyNameLegal = x.CMP_NameLegal,
                        CompanyId = x.CMP_Id,
                    }).ToList();
                    return lstVendor;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<VendorTypeListServiceModel> GetCompany_VendorListByLocationId(ServiceBaseModel objServiceBaseModel)", "Exception While Getting List of Company Facility.", null);
                throw;
            }

        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-17-2018
        /// Created For : To get Company facility list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<POTypeDataModel> GetPOTypeDetailsOfCompanyFacilityList(long? UserId, long? LocationID, long? VendorId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                List<ResourceData> Resource = new List<ResourceData>();

                // Add parts to the list.
                Resource.Add(new ResourceData() { ResourceId = 1, ResourceName = "M" });
                Resource.Add(new ResourceData() { ResourceId = 2, ResourceName = "L" });
                Resource.Add(new ResourceData() { ResourceId = 3, ResourceName = "S" });
                Resource.Add(new ResourceData() { ResourceId = 4, ResourceName = "O" });
                Resource.Add(new ResourceData() { ResourceId = 5, ResourceName = "E" });

                var objPOTypeModelDetails = new List<POTypeDetails>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetPOFacilityItem(LocationID, VendorId).Select(x => new POTypeDataModel()
                {
                    COM_FacilityId = x.CFM_Id,
                    COM_Facility_Desc = x.CFM_Discription,
                    UnitPrice = x.CFM_Rate,
                    Tax = x.CFM_Tax,
                    Quantity = 0,
                    TotalPrice = 0,
                    //Resourse = Resource,
                    Status = "S",
                    CostCode = x.CFM_CCD_CostCode,
                    CFM_CMP_Id = x.CFM_CMP_Id,
                    RemainingAmt = x.BCM_BalanceAmount,
                    LastRemainingAmount = "0",
                    StatusCalculation = "0"
                }).ToList();
                //var Results = _workorderems.spGetCompanyFacilityMapping(LocationID, VendorId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                //    .Select(a => new POTypeDataModel()
                //    {
                //        COM_FacilityId = a.CFM_Id,
                //        COM_Facility_Desc = a.CFM_Discription,
                //        UnitPrice = a.CFM_Rate,
                //        Tax = a.CFM_Tax,
                //        Quantity = 0,
                //        TotalPrice = 0,
                //        //Resourse = Resource,
                //        Status = "S",
                //        CostCode = a.CFM_CCD_CostCode,
                //        CFM_CMP_Id = a.CFM_CMP_Id,
                //        RemainingAmt = a.BCM_BalanceAmount,
                //        LastRemainingAmount = "0",
                //        StatusCalculation = "0"
                //    }).ToList();

                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objPOTypeModelDetails.pageindex = pageindex;
                //objPOTypeModelDetails.total = totalPages;
                //objPOTypeModelDetails.records = totRecords;
                //objPOTypeModelDetails.rows = Results.ToList();
                return Results.ToList();  
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }

        public bool SavePODetails(POTypeDataModel objPOTypeDataModel, List<GridDataPO> obj, List<QuestionAnswerModel> objQuestioAsnwerList, bool IsManager)
        {
            bool IsSaved = false;
            var objDAR = new DARModel();
            //var getRule = new ApprovalCommonModel<Approval>();
            var objModel = new CommonApproval<ApprovalInput>();
            var approvalInput = new ApprovalInput();
            try
            {
                string action = "I";
                string result = objPOTypeDataModel.PONumber.Split('O')[1];
                long PONumber = Convert.ToInt64(result);
                approvalInput.Amount = Convert.ToDecimal(objPOTypeDataModel.Total);
                approvalInput.UserId = objPOTypeDataModel.UserId;
                approvalInput.ModuleName = "eCounting";
                if (objPOTypeDataModel.POId == 0)
                {
                    var getDetails = _workorderems.PODetails.Where(x => x.POD_Id == PONumber).FirstOrDefault();
                    if (getDetails == null)
                    {
                        var getRuleData = objModel.GetApprovalRuleData(approvalInput);
                        getRuleData.CurrentLevel = "1";
                        if (objPOTypeDataModel.POType == 1)
                        {
                            var saveNormalPO = _workorderems.spSetPODetail(action, PONumber, objPOTypeDataModel.Location,
                                                                         objPOTypeDataModel.POType, objPOTypeDataModel.Vendor, objPOTypeDataModel.DeliveryDate,
                                                                          objPOTypeDataModel.Total, null, null, objPOTypeDataModel.UserId, getRuleData.UserId, "Y", getRuleData.RuleId
                                                                          , getRuleData.RuleLevel, getRuleData.CurrentLevel, objPOTypeDataModel.QBK_Id,null);
                            if (obj != null)
                            {
                                foreach (var item in obj)
                                {
                                    if (item.Quantity > 0)
                                    {
                                        var saveGridData = _workorderems.spSetPOFacilityItem(action, null, PONumber, item.COM_FacilityId, item.Quantity, item.Status);
                                    }
                                }
                            }
                        }
                        if (objPOTypeDataModel.POType == 2)
                        {
                            var saveReoccuringPO = _workorderems.spSetPODetail(action, PONumber, objPOTypeDataModel.Location,
                                                                         objPOTypeDataModel.POType, objPOTypeDataModel.Vendor, null,
                                                                          objPOTypeDataModel.Total, objPOTypeDataModel.BillDate, null, objPOTypeDataModel.UserId, getRuleData.UserId, "Y", getRuleData.RuleId
                                                                          , getRuleData.RuleLevel, getRuleData.CurrentLevel, objPOTypeDataModel.QBK_Id, POReccuringStaus.Pending.ToString());
                            if (obj != null)
                            {
                                foreach (var item in obj)
                                {
                                    if (item.Quantity > 0)
                                    {
                                        var saveGridData = _workorderems.spSetPOFacilityItem(action, null, PONumber, item.COM_FacilityId, item.Quantity, item.Status);
                                    }
                                }
                            }
                        }
                        if (objPOTypeDataModel.POType == 3)
                        {
                            if (objPOTypeDataModel.IsVendorRegister == "1")
                            {
                                var saveEmeregencyPO = _workorderems.spSetPODetail(action, PONumber, objPOTypeDataModel.Location,
                                                                        objPOTypeDataModel.POType, objPOTypeDataModel.Vendor, null, objPOTypeDataModel.Total,
                                                                         null, objPOTypeDataModel.POD_EmergencyPODocument, objPOTypeDataModel.UserId, getRuleData.UserId, "Y", getRuleData.RuleId
                                                                          , getRuleData.RuleLevel, getRuleData.CurrentLevel, objPOTypeDataModel.QBK_Id,null);
                                if (obj != null)
                                {
                                    foreach (var item in obj)
                                    {
                                        if (item.Quantity > 0)
                                        {
                                            var saveGridData = _workorderems.spSetPOFacilityItem(action, null, PONumber, item.COM_FacilityId, item.Quantity, item.Status);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var saveEmergenyUnregisterData = _workorderems.spSetPODetailEmergency(action, PONumber, objPOTypeDataModel.Location,
                                                                                               objPOTypeDataModel.POType, objPOTypeDataModel.VendorName,
                                                                                               objPOTypeDataModel.Comment, objPOTypeDataModel.Amount,
                                                                                              null, objPOTypeDataModel.POD_EmergencyPODocument,
                                                                                              objPOTypeDataModel.UserId, null,
                                                                                              "Y", objPOTypeDataModel.QBK_Id);
                            }
                            foreach (var item in objQuestioAsnwerList)
                            {
                                var saveQuestionData = _workorderems.spSetPOQuestion(action, null, PONumber, item.QuestionId, item.Answer, "Y");
                            }
                        }
                        IsSaved = true;
                        if (objPOTypeDataModel.POStatus == "ApprovePO")
                        {
                            #region Email
                            var objEmailLogRepository = new EmailLogRepository();
                            var objEmailReturn = new List<EmailToManagerModel>();
                            var objListEmailog = new List<EmailLog>();
                            var objTemplateModel = new TemplateModel();
                            var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objPOTypeDataModel.UserId
                                                                                && x.IsDeleted == false).FirstOrDefault();
                            if (userData != null && IsSaved == true)
                            {
                                var location = objPOTypeDataModel.Location;
                                var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == location && x.IsDeleted == false).FirstOrDefault();
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = getRuleData.Email;
                                objEmailHelper.ManagerName = getRuleData.ManagerName;
                                objEmailHelper.LocationName = locationData.LocationName;
                                objEmailHelper.UserName = userData.FirstName + " " + userData.LastName;
                                objEmailHelper.PONumber = objPOTypeDataModel.PONumber;
                                //objEmailHelper.InfractionStatus = obj.Status;
                                objEmailHelper.MailType = "POAPPROVEDREJECT";
                                objEmailHelper.SentBy = userData.UserId;
                                objEmailHelper.LocationID = Convert.ToInt64(location);
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                var objNotify = new NotificationDetailModel();
                                var _ICommonMethod = new CommonMethodManager();
                                objNotify.CreatedBy = objPOTypeDataModel.UserId;
                                objNotify.CreatedDate = DateTime.UtcNow;
                                objNotify.AssignTo = getRuleData.UserId;
                                objNotify.POID = PONumber;
                                var saveDataForNotification = _ICommonMethod.SaveNotificationDetail(objNotify);

                                if (getRuleData.DeviceId != null)
                                {
                                    objEmailHelper.IsWorkable = true;
                                    // objEmailHelper.LogPOId = 
                                    string message = PushNotificationMessages.POCreate(objPOTypeDataModel.PONumber, objEmailHelper.UserName, objEmailHelper.LocationName);
                                    PushNotificationFCM.FCMAndroid(message, getRuleData.DeviceId, objEmailHelper);
                                }
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
                                        objEmailog.LocationId = location;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = userData.UserId;
                                        objEmailog.SentEmail = getRuleData.Email;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = getRuleData.UserId;
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
                            }
                            else if (userData.UserType == 3 && IsSaved == true)
                            {
                                long location = Convert.ToInt64(objPOTypeDataModel.Location);
                                objEmailReturn = objEmailLogRepository.SendEmailToManagerForApprovePO(location, objPOTypeDataModel.UserId);

                                if (objEmailReturn.Count > 0)
                                {
                                    foreach (var item in objEmailReturn)
                                    {
                                        bool IsSent = false;
                                        var objEmailHelper = new EmailHelper();
                                        objEmailHelper.emailid = item.ManagerEmail;
                                        objEmailHelper.ManagerName = item.ManagerName;
                                        objEmailHelper.LocationName = item.LocationName;
                                        objEmailHelper.UserName = item.UserName;
                                        objEmailHelper.PONumber = objPOTypeDataModel.PONumber;
                                        //objEmailHelper.InfractionStatus = obj.Status;
                                        objEmailHelper.MailType = "POAPPROVEDREJECT";
                                        objEmailHelper.SentBy = item.RequestBy;
                                        objEmailHelper.LocationID = item.LocationID;
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
                                            catch (Exception)
                                            {
                                                throw;
                                            }
                                        }
                                    }
                                    using (var context = new workorderEMSEntities())
                                    {
                                        context.EmailLogs.AddRange(objListEmailog);
                                        context.SaveChanges(); ;
                                    }
                                    #endregion Email
                                }
                            }
                        }
                    }
                    else
                    {
                        IsSaved = true;
                    }
                }
                else
                    {
                        if (obj != null)
                        {
                            action = "U";
                            foreach (var item in obj)
                            {
                                if (item.Quantity > 0)
                                {
                                    var saveGridData = _workorderems.spSetPOFacilityItem(action, item.COM_FacilityId, PONumber, item.COM_FacilityId, item.Quantity, item.Status);
                                }
                            }
                        }
                        IsSaved = true;
                    }

                #region Save DAR
                var CommonManager = new CommonMethodManager();
                long LocId = Convert.ToInt64(objPOTypeDataModel.Location);
                var LocationName = _workorderems.LocationMasters.Where(x => x.LocationId == LocId && x.IsDeleted == false).FirstOrDefault();

                objDAR.ActivityDetails = DarMessage.CreatePO(LocationName.LocationName, PONumber);
                objDAR.TaskType = (long)TaskTypeCategory.CreatePO;
                objDAR.UserId = objPOTypeDataModel.UserId;
                objDAR.CreatedBy = objPOTypeDataModel.UserId;
                objDAR.LocationId = LocId;
                objDAR.CreatedOn = DateTime.UtcNow;
                CommonManager.SaveDAR(objDAR);
                #endregion DAR
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POTypeDataModel SavePODetails(POTypeDataModel objPOTypeDataModel)", "Exception While Saving PO.", null);
                throw;
            }
            return IsSaved;
        }

        public CompanyFacilityListServiceModel GetCompanyFacilityByVendoeId(POCommonServiceModel objPOCommonServiceModel)
        {
            try
            {
                var lstCompanyFacility = new CompanyFacilityListServiceModel();
                List<ResourceData> Resource = new List<ResourceData>();

                // Add parts to the list.
                Resource.Add(new ResourceData() { ResourceId = 1, ResourceName = "M" });
                Resource.Add(new ResourceData() { ResourceId = 2, ResourceName = "L" });
                Resource.Add(new ResourceData() { ResourceId = 3, ResourceName = "S" });
                Resource.Add(new ResourceData() { ResourceId = 4, ResourceName = "O" });
                Resource.Add(new ResourceData() { ResourceId = 5, ResourceName = "E" });
                if (objPOCommonServiceModel != null && objPOCommonServiceModel.VendorId > 0
                        && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objPOCommonServiceModel.UserId
                                                                       && x.IsDeleted == false).FirstOrDefault();
                    var VendorId = _workorderems.CompanyDetails.Where(x => x.COD_CMP_Id == objPOCommonServiceModel.VendorId).FirstOrDefault();
                    if (userData != null && VendorId != null)
                    {
                        lstCompanyFacility = _workorderems.spGetCompanyDetailForReoccurringPO(objPOCommonServiceModel.VendorId).Select(x => new CompanyFacilityListServiceModel()
                        {
                            Address = x.COD_Address2,
                            PointOfContact = x.COD_PointOfContact,
                            CostDuringPeriod = x.CNT_CostDuringPeriod,
                            InvoicingFrequency = x.CNT_invoicingFrequency,
                            AnnualValueOfAggreement = x.CNT_AnnualValueOfAggreement,
                            IdSecondParty = x.CNT_CMP_IdSecondParty,
                            ContractCompanyTypeId = x.CNT_CTT_Id,
                            PhoneNumber = x.COD_Phone1,
                            Email = x.COD_Email,
                            City = x.COD_Addr2City,
                            ContractId = x.CNT_Id
                        }).FirstOrDefault();

                        //lstCompanyFacility.CompanyFacility = _workorderems.spGetCompanyFacilityMapping(objPOCommonServiceModel.LocationId, objPOCommonServiceModel.VendorId).Select(x => new CompanyFacility()
                        //{
                        //    COM_FacilityId = x.CFM_Id,
                        //    COM_Facility_Desc = x.CFM_Discription,
                        //    UnitPrice = x.CFM_Rate,
                        //    Tax = x.CFM_Tax,
                        //    Quantity = 0,
                        //    TotalPrice = 0,
                        //    CostCode = x.CFM_CCD_CostCode,
                        //    CompanyId = x.CFM_CMP_Id,
                        //    RemainingAmount = x.BCM_BalanceAmount
                        //}).ToList();
                        lstCompanyFacility.CompanyFacility = _workorderems.spGetPOFacilityItem(objPOCommonServiceModel.LocationId, objPOCommonServiceModel.VendorId).Select(x => new CompanyFacility()
                        {
                            COM_FacilityId = x.CFM_Id,
                            COM_Facility_Desc = x.CFM_Discription,
                            UnitPrice = x.CFM_Rate,
                            Tax = x.CFM_Tax,
                            Quantity = 0,
                            TotalPrice = 0,
                            CostCode = x.CFM_CCD_CostCode,
                            CompanyId = x.CFM_CMP_Id,
                            RemainingAmount = x.BCM_BalanceAmount
                        }).ToList();
                        lstCompanyFacility.Resourse = Resource;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
                return lstCompanyFacility;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<POTypeServiceModel> AllPOTypeList(ServiceBaseModel objServiceBaseModel)", "Exception While Getting List of Company & Location.", null);
                throw;
            }

        }

        public ServiceResponseModel<List<QuestionsEmergencyPO>> GetQuestionList(POCommonServiceModel objPOCommonServiceModel)
        {
            try
            {
                var lstQuestions = new List<QuestionsEmergencyPO>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<QuestionsEmergencyPO>>();
                if (objPOCommonServiceModel != null
                        && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0)
                {
                    lstQuestions = _workorderems.spGetQuestionAnswer("POQ").Select(x => new QuestionsEmergencyPO()
                    {
                        Question = x.QNA_Question,
                        QuestionId = x.QNA_Id,
                    }).ToList();
                    if (lstQuestions.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = lstQuestions;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<QuestionsEmergencyPO>> GetQuestionList(POCommonServiceModel objPOCommonServiceModel)", "Exception While Getting List of Question.", null);
                throw;
            }

        }

        public List<POListModel> GetAllPOList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            long Type = Convert.ToInt64(UserType.Employee);
            //LocationId = 0; //To get all PO for user to approve for all location as per SQL Developer will change if client want to change
            var Results = new List<POListModel>();
            try
            {
               // var objPOTypeModelDetails = new POListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                if (UserTypeId != Type)
                {
                    Results = _workorderems.spGetPOList(LocationId, UserId, status)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                   .Select(a => new POListModel()
                   {
                       LogPOId = a.LPOD_POD_Id,
                       DisplayLogPOId="PO"+ a.LPOD_POD_Id,
                       DisplayPODate = a.LPOD_PODate!=null?a.LPOD_PODate.ToString("yyyy/MM/dd") : "N/A",
                       LocationName = a.LocationName,
                       DisplayDeliveryDate = a.LPOD_DeliveryDate != null ? a.LPOD_DeliveryDate.ToString("yyyy/MM/dd") : "N/A",
                       CompanyName = a.CMP_NameLegal,
                       POStatus = a.PO_Status,
                       POType = a.POT_POType,
                       POStatusToDisplay = (a.PO_Status == "W") ? "Waiting" : (a.PO_Status == "Y") ? "Approved" : "Reject",
                       LogId = a.LPOD_Id,
                       Total = a.LPOD_POAmount,
                       CreatedBy = a.LPOD_ApprovedBy,
                       id= Cryptography.GetEncryptedData(Convert.ToString(a.LPOD_POD_Id), true),
                   }).OrderByDescending(x => x.LogPOId).ToList();
                }
                else
                {
                    Results = _workorderems.spGetPOList(LocationId, UserId,status)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new POListModel()
                    {
                        LogPOId = a.LPOD_POD_Id,
                        DisplayLogPOId = "PO" + a.LPOD_POD_Id,
                        DisplayPODate = a.LPOD_PODate != null ? a.LPOD_PODate.ToString("yyyy/MM/dd") : "N/A",
                        LocationName = a.LocationName,
                        DisplayDeliveryDate = a.LPOD_DeliveryDate != null ? a.LPOD_DeliveryDate.ToString("yyyy/MM/dd") : "N/A",
                        CompanyName = a.CMP_NameLegal,
                        POStatus = a.PO_Status,
                        POType = a.POT_POType,
                        POStatusToDisplay = (a.PO_Status == "W") ? "Waiting" : (a.PO_Status == "Y") ? "Approved" : "Reject",
                        LogId = a.LPOD_Id,
                        Total = a.LPOD_POAmount,
                        CreatedBy = a.LPOD_ApprovedBy,
                        id = Cryptography.GetEncryptedData(Convert.ToString(a.LPOD_POD_Id), true),
                    }).Where(x => x.CreatedBy == UserId).OrderByDescending(x => x.LogPOId).ToList();
                }

                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objPOTypeModelDetails.pageindex = pageindex;
                //objPOTypeModelDetails.total = totalPages;
                //objPOTypeModelDetails.records = totRecords;
                //objPOTypeModelDetails.rows = Results.ToList();
                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 01-OCT-2018
        /// Created For : To approve PO by using PO number
        /// </summary>
        /// <param name="objPOApproveRejectModel"></param>
        /// <param name="objListData"></param>
        /// <returns></returns>
        public string ApprovePOByPOId(POApproveRejectModel objPOApproveRejectModel, POListModel objListData)
        {
            bool isSaved = false;
            string returnvalue = "";
            string LocName = "";
            string ApptoveRemoveSatus = "";
            var objDAR = new DARModel();
            var objModel = new CommonApproval<ApprovalInput>();
            var approvalInput = new ApprovalInput();
            var getRuleData = new Approval();
            var getPODetails = new LogPODetail();
            string Status = string.Empty;
            string StatusApproval = string.Empty;
            try
            {
                if (objPOApproveRejectModel.POId > 0)
                {
                    approvalInput.Amount = Convert.ToDecimal(objListData.Total);
                    approvalInput.UserId = objPOApproveRejectModel.UserId;
                    approvalInput.ModuleName = "eCounting";
                    var userData = _workorderems.LogPODetails.Join(_workorderems.UserRegistrations, q => q.LPOD_ModifiedBy, u => u.UserId, (q, u) => new { q, u }).
                        Where(x => x.q.LPOD_POD_Id == objPOApproveRejectModel.POModifiedId
                                         ).FirstOrDefault();
                    //var userDataCreatedPO = _workorderems.LogPODetails.Where(x => x.LPOD_POD_Id == objPOApproveRejectModel.POId
                    //                                                 && x.LPOD_IsActive == "Y").FirstOrDefault();
                    var getCompanyDataByName = _workorderems.Companies.Where(x => x.CMP_NameLegal == objListData.CompanyName
                                                              && x.CMP_IsActive == "Y").FirstOrDefault();

                    //getPODetails = _workorderems.LogPODetails.Where(x => x.LPOD_Id == objPOApproveRejectModel.POId).OrderByDescending(x => x.LPOD_Id).FirstOrDefault();
                    getPODetails = _workorderems.LogPODetails.Where(x => x.LPOD_Id == objPOApproveRejectModel.POId).FirstOrDefault();
                    if (userData != null)
                    {
                        if (objPOApproveRejectModel.Comment == null)
                        {
                            getRuleData = objModel.GetApprovalRuleData(approvalInput);
                            if (getRuleData.RuleLevel == "0") // 0 is for self approve if amount is less than 50$
                            {
                                ApptoveRemoveSatus = "Appoved";
                                Status = "Y";
                                StatusApproval = "Y";
                                getRuleData.CurrentLevel = "0";
                                returnvalue = CommonMessage.POSelfApprove();
                            }
                            else
                            {
                                returnvalue = CommonMessage.POApproved();
                                ApptoveRemoveSatus = "Appoved";
                                //long Calculation = Convert.ToInt64(getPODetails.LPOD_RUL_CurrentLevel) + 1;
                                //getRuleData.CurrentLevel = Calculation.ToString();
                                getRuleData.CurrentLevel = getPODetails.LPOD_RUL_CurrentLevel;
                                if (getRuleData.ManagerName != null && getRuleData.UserId > 0 &&
                                getPODetails.LPOD_RUL_CurrentLevel != getRuleData.RuleLevel)
                                {
                                    Status = "Y";
                                    StatusApproval = "Y";
                                }
                                //else part will change in future so for now leave it as it is.
                                else
                                {
                                    Status = "Y";
                                    StatusApproval = "Y";
                                }
                            }
                        }
                        else
                        {
                            returnvalue = CommonMessage.PORemoved();
                            ApptoveRemoveSatus = "Reject";
                            Status = "N";
                            StatusApproval = "N";
                        }

                        //var getPODetails = _workorderems.spGetPODetail(objPOApproveRejectModel.POId).FirstOrDefault();
                        isSaved = true;
                        //+1 is use to increase current level as we need to send approval request to their manager
                        #region To set Current Status

                        string Action = "U";
                        var IsApprove = _workorderems.spSetApprovalForPODetail(objPOApproveRejectModel.POId,
                                                                           objPOApproveRejectModel.Comment, StatusApproval, objPOApproveRejectModel.UserId);

                        if (getRuleData.CurrentLevel != getRuleData.RuleLevel)
                        {
                            // var Id = Convert.ToGlobalAdmin.UserId;
                            long Calculation = 0;
                         
                            if (getRuleData.UserId == 3 && getRuleData.ManagerName == "Dane") // Checking for Dane gray GLobal Admin
                            {
                                Calculation = Convert.ToInt64(getPODetails.LPOD_RUL_Level);
                            }
                            else
                            {
                                Calculation = Convert.ToInt64(getPODetails.LPOD_RUL_CurrentLevel) + 1;
                            }
                            getRuleData.CurrentLevel = Calculation.ToString();
                             var updatePO = _workorderems.spUpdatePODetail(objPOApproveRejectModel.POId, getRuleData.RuleId, getRuleData.RuleLevel, getRuleData.CurrentLevel,"W");
                            //var updatePO = _workorderems.spSetPODetail(Action, objPOApproveRejectModel.POModifiedId, getPODetails.LPOD_LocationId,
                            //                                       Convert.ToInt64(getPODetails.LPOD_POT_Id), getPODetails.LPOD_CMP_Id, getPODetails.LPOD_DeliveryDate,
                            //                                       getPODetails.LPOD_POAmount, getPODetails.LPOD_ReoccourringBillDate, getPODetails.LPOD_EmergencyPODocument, getPODetails.LPOD_ModifiedBy,
                            //                                       getRuleData.UserId, Status, getRuleData.RuleId
                            //                                       , getRuleData.RuleLevel, getRuleData.CurrentLevel, objPOApproveRejectModel.QuickBookPOId,null);//Added getRuleData  instead of  objPOApproveRejectModel for approve by
                        }
                        else
                        {
                            var objEmailLogRepository = new EmailLogRepository();
                            var objEmailReturn = new List<EmailToManagerModel>();
                            var objListEmailog = new List<EmailLog>();
                            var objTemplateModel = new TemplateModel();
                            if (isSaved == true)
                            {
                                long location = Convert.ToInt64(userData.q.LPOD_LocationId);
                                var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == location && x.IsDeleted == false).FirstOrDefault();
                                LocName = locationData.LocationName;
                                var userDetails = _workorderems.UserRegistrations.Where(x => x.UserId == getPODetails.LPOD_ModifiedBy &&
                                                                                     x.IsEmailVerify == true).FirstOrDefault(); 
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = getRuleData.Email;
                                objEmailHelper.ManagerName = getRuleData.ManagerName;
                                objEmailHelper.LocationName = locationData.LocationName;
                                objEmailHelper.UserName = userData.u.FirstName + " " + userData.u.LastName;
                                objEmailHelper.PONumber = "PO" + objPOApproveRejectModel.POModifiedId.ToString();
                                //objEmailHelper.InfractionStatus = obj.Status;
                                objEmailHelper.MailType = "POAPPROVEDREJECT";
                                objEmailHelper.SentBy = getRuleData.UserId;
                                objEmailHelper.LocationID = location;
                                objEmailHelper.IsCancel = objPOApproveRejectModel.Comment;
                                objEmailHelper.LogPOId = getPODetails.LPOD_Id.ToString();
                                if (getPODetails != null && getPODetails.LPOD_CMP_Id > 0)
                                {
                                    var companyData = _workorderems.Companies.Where(x => x.CMP_Id == getPODetails.LPOD_CMP_Id && x.CMP_IsActive == "Y").FirstOrDefault();
                                    if (companyData != null)
                                    {
                                        objEmailHelper.CompanyName = companyData.CMP_NameLegal;
                                    }
                                }

                                //Push Notification     
                                ///Comment For Future purpose will be maintain heirachy
                                if (getRuleData.CurrentLevel == getRuleData.RuleLevel)
                                {
                                    var objNotify = new NotificationDetailModel();
                                    var _ICommonMethod = new CommonMethodManager();
                                    objNotify.POID = objPOApproveRejectModel.POModifiedId;
                                    objNotify.ApproveStatus = false;
                                    objNotify.AssignTo = getPODetails.LPOD_ModifiedBy;
                                    var update = _ICommonMethod.UpdateNotificationDetail(objNotify);
                                    if (userDetails.DeviceId != null)
                                    {
                                        objEmailHelper.IsWorkable = false;
                                        string message = PushNotificationMessages.LastPOApproved(objPOApproveRejectModel.POModifiedId.ToString(), objEmailHelper.UserName, objEmailHelper.LocationName);
                                        PushNotificationFCM.FCMAndroid(message, userDetails.DeviceId, objEmailHelper);
                                    }
                                }
                            }
                        }
                        #endregion To set Current Status
                    }
                    else
                    {
                        isSaved = false;
                    }
                    if (isSaved == true)
                    {
                        #region Email
                        if (getRuleData.ManagerName != null && getRuleData.UserId > 0 &&
                            getPODetails.LPOD_RUL_CurrentLevel != getRuleData.RuleLevel && getRuleData.RuleLevel != "0")
                        {
                            var objEmailLogRepository = new EmailLogRepository();
                            var objEmailReturn = new List<EmailToManagerModel>();
                            var objListEmailog = new List<EmailLog>();
                            var objTemplateModel = new TemplateModel();
                            if (isSaved == true)
                            {
                                long location = Convert.ToInt64(userData.q.LPOD_LocationId);
                                var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == location && x.IsDeleted == false).FirstOrDefault();
                                LocName = locationData.LocationName;
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = getRuleData.Email;
                                objEmailHelper.ManagerName = getRuleData.ManagerName;
                                objEmailHelper.LocationName = locationData.LocationName;
                                objEmailHelper.UserName = userData.u.FirstName + " " + userData.u.LastName;
                                objEmailHelper.PONumber = "PO" + objPOApproveRejectModel.POModifiedId.ToString();
                                //objEmailHelper.InfractionStatus = obj.Status;
                                objEmailHelper.MailType = "POAPPROVEDREJECT";
                                objEmailHelper.SentBy = getRuleData.UserId;
                                objEmailHelper.LocationID = location;
                                objEmailHelper.IsCancel = objPOApproveRejectModel.Comment;
                                objEmailHelper.LogPOId = getPODetails.LPOD_Id.ToString();
                                if (getPODetails != null && getPODetails.LPOD_CMP_Id > 0)
                                {
                                    var companyData = _workorderems.Companies.Where(x => x.CMP_Id == getPODetails.LPOD_CMP_Id && x.CMP_IsActive == "Y").FirstOrDefault();
                                    if (companyData != null)
                                    {
                                        objEmailHelper.CompanyName = companyData.CMP_NameLegal;
                                    }
                                }
                                
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                //Push Notification     
                                ///Comment For Future purpose will be maintain heirachy
                                //if (getRuleData.CurrentLevel != getRuleData.RuleLevel)
                                //{
                                    var objNotify = new NotificationDetailModel();
                                    var _ICommonMethod = new CommonMethodManager();
                                    objNotify.POID = objPOApproveRejectModel.POModifiedId;
                                    objNotify.ApproveStatus = false;
                                    objNotify.AssignTo = getPODetails.LPOD_ApprovedBy;
                                    var update = _ICommonMethod.UpdateNotificationDetail(objNotify);
                                    if (userData.u.DeviceId != null)
                                    {
                                        objEmailHelper.IsWorkable = false;
                                        string message = PushNotificationMessages.POApproved(objPOApproveRejectModel.POModifiedId.ToString(), objEmailHelper.UserName, objEmailHelper.LocationName);                                       
                                        PushNotificationFCM.FCMAndroid(message, getRuleData.DeviceId, objEmailHelper);
                                    }
                                //}                                   
                                if (IsSent == true)
                                {
                                    var objEmailog = new EmailLog();
                                    try
                                    {
                                        objEmailog.CreatedBy = userData.u.UserId;
                                        objEmailog.CreatedDate = DateTime.UtcNow;
                                        objEmailog.DeletedBy = null;
                                        objEmailog.DeletedOn = null;
                                        objEmailog.LocationId = userData.q.LPOD_LocationId;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = userData.u.UserId;
                                        objEmailog.SentEmail = getRuleData.Email;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = getRuleData.UserId;
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

                                #region Save DAR
                                var CommonManager = new CommonMethodManager();
                                objDAR.ActivityDetails = DarMessage.ApproveRejectPO(LocName, objPOApproveRejectModel.POId, ApptoveRemoveSatus);
                                objDAR.TaskType = (long)TaskTypeCategory.CreatePO;
                                objDAR.UserId = objPOApproveRejectModel.UserId;
                                objDAR.CreatedBy = objPOApproveRejectModel.UserId;
                                objDAR.LocationId = objPOApproveRejectModel.LocationId;
                                objDAR.CreatedOn = DateTime.UtcNow;
                                CommonManager.SaveDAR(objDAR);
                                #endregion DAR

                            }
                            else
                            {
                                returnvalue = CommonMessage.POApprovedFailure();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string ApprovePOByPOId(long Id)", "Exception While Approving PO.", null);
                throw;
            }
            return returnvalue;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-April-2019
        /// Created For : To get Approval status to save Quickbook.
        /// </summary>
        /// <param name="objPOApproveRejectModel"></param>
        /// <param name="objListData"></param>
        /// <returns></returns>
        public PODetail GetApprovalResponseToSaveQBKId(POApproveRejectModel objPOApproveRejectModel, POListModel objListData)
        {
            var getPODetails = new PODetail();
            try
            {
                getPODetails = _workorderems.PODetails.Where(x => x.POD_Id == objListData.LogPOId).FirstOrDefault();                             
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool GetApprovalResponseToSaveQBKId(POApproveRejectModel objPOApproveRejectModel)", "Exception While Getting Approving PO.", objPOApproveRejectModel);
                throw;
            }
            return getPODetails;
        } 

        public POTypeDataModel GetPODetailsById(long POId)
        {
            try
            {
                var PODetailsData = new POTypeDataModel();
                if (POId > 0)
                {
                    
                    PODetailsData = _workorderems.spGetPODetail(POId).
                    Select(x => new POTypeDataModel()
                    {
                        PONumber ="PO"+ x.POD_Id,
                        Location = x.POD_LocationId,
                        POType = x.POD_POT_Id,
                        Vendor = x.POD_CMP_Id,
                        DeliveryDate = x.POD_DeliveryDate,
                        POD_EmergencyPODocument = x.POD_EmergencyPODocument,
                        IssueDate = x.POD_PODate,
                        BillDate = x.POD_ReoccourringBillDate, 
                        InvoicingFrequency = x.CNT_invoicingFrequency,
                        CostDuringPeriod = x.CNT_CostDuringPeriod.ToString(),
                        Comment=x.POE_EmergencyComment,
                        Amount  = x.POE_POAmount,
                        VendorName = x.POE_VendorName,
                        PointOfContactAddress = x.COD_Address,
                        PointOfContactName = x.COD_PointOfContact,
                        Total = x.POD_POAmount,
                       
                    }).FirstOrDefault();
                }
                return PODetailsData;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POTypeDataModel GetPODetailsById(long POId)", "Exception While Editing PO Detais.", null);
                throw;
            }
        }

        public List<POTypeDataModel> GetAllPOFacilityByPOIdList(long? UserId, long? POId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                //List<ResourceData> Resource = new List<ResourceData>();
                //var objPOTypeModelDetails = new POTypeDetails();
                //int pageindex = Convert.ToInt32(pageIndex) - 1;
                //int pageSize = Convert.ToInt32(numberOfRows);                
                var Results = _workorderems.spGetPOFacilityItemForApproval(POId). // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)      
                    Select(a => new POTypeDataModel()
                    {
                        COM_FacilityId = a.CFM_Id,
                        COM_Facility_Desc = a.CFM_Discription,
                        UnitPrice = a.CFM_Rate,
                        Tax = a.CFM_Tax,
                        FacilityType = a.CFM_FacilityType,
                        CostCode = a.CFM_CCD_CostCode,
                        Quantity = a.POF_Unit,
                        CostCodeName = a.CCD_Description
                        //Total = a.CFM_Rate * a.POF_Unit,
                        //TotalPrice = GradTotal
                }).ToList();               
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objPOTypeModelDetails.pageindex = pageindex;
                //objPOTypeModelDetails.total = totalPages;
                //objPOTypeModelDetails.records = totRecords;
                //objPOTypeModelDetails.rows = Results.ToList();
                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POTypeDetails GetAllPOFacilityByPOIdList(long? UserId, long? POId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of PO.", null);
                throw;
            }
        }

        public List<POTypeDataModel> GetPOFacilityListForEditByPOId(long? UserId, long? POId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objPOTypeModelDetails = new List<POTypeDataModel>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetPOFacilityItemForEdit(POId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new POTypeDataModel()
                    {
                        COM_FacilityId = a.CFM_Id,
                        COM_Facility_Desc = a.CFM_Discription,
                        UnitPrice = a.CFM_Rate,
                        Tax = a.CFM_Tax,
                        Quantity = a.POF_Unit,
                        TotalPrice = a.POF_Unit == null?0: a.CFM_Rate * a.POF_Unit,
                        Status = a.POF_Unit == null?"":"Y",
                        CostCode = a.CFM_CCD_CostCode,
                        CFM_CMP_Id = a.CFM_CMP_Id,
                        RemainingAmt = a.BCM_BalanceAmount,
                        LastRemainingAmount = "0",
                        StatusCalculation = "0",
                        POId = a.CFM_Id
                    }).ToList();

                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objPOTypeModelDetails.pageindex = pageindex;
                //objPOTypeModelDetails.total = totalPages;
                //objPOTypeModelDetails.records = totRecords;
                objPOTypeModelDetails  = Results.ToList();
                return objPOTypeModelDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }

        public ServiceResponseModel<List<POListModel>> GetAllPOListForMobile(POCommonServiceModel objPOCommonServiceModel)
        {
            try
            {
                var lstAllPO = new List<POListModel>();
                var ObjServiceResponseModel = new ServiceResponseModel<List<POListModel>>();
                if (objPOCommonServiceModel != null
                        && objPOCommonServiceModel.LocationId > 0 && objPOCommonServiceModel.UserId > 0
                        )
                {
                    lstAllPO = _workorderems.spGetPOList(objPOCommonServiceModel.LocationId, objPOCommonServiceModel.UserId, objPOCommonServiceModel.Status).Select(a => new POListModel()
                    {
                        LogPOId = a.LPOD_POD_Id,
                        PODate = a.LPOD_PODate,
                        LocationName = a.LocationName,
                        DeliveryDate = a.LPOD_DeliveryDate,
                        CompanyName = a.CMP_NameLegal,
                        POStatus = a.PO_Status,
                        POType = a.POT_POType,
                        LogId = a.LPOD_Id,
                        Total = a.LPOD_POAmount
                    }).ToList();
                    if (lstAllPO.Count > 0)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                        ObjServiceResponseModel.Data = lstAllPO;
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<POListModel>> GetAllPOListForMobile(POCommonServiceModel objPOCommonServiceModel)", "Exception While Getting List of All PO.", null);
                throw;
            }
        }


        public ServiceResponseModel<PODetailsServiceModel> GetAllPODetailsByIdForMobile(POCommonServiceModel objPOCommonServiceModel)
        {
            try
            {
                var lstAllPODetails = new PODetailsServiceModel();
                var ObjServiceResponseModel = new ServiceResponseModel<PODetailsServiceModel>();
                if (objPOCommonServiceModel != null && objPOCommonServiceModel.LocationId > 0
                    && objPOCommonServiceModel.POId > 0)
                {
                    var checkPONumberData = _workorderems.LogPODetails.Where(x => x.LPOD_POD_Id == objPOCommonServiceModel.POId
                                                                   ).OrderByDescending(x => x.LPOD_Id).FirstOrDefault();
                    if (checkPONumberData != null)
                    {
                        if (checkPONumberData.LPOD_IsApprove == "Y")
                        {
                            lstAllPODetails = _workorderems.spGetPODetail(objPOCommonServiceModel.POId).Select(x => new PODetailsServiceModel()
                            {
                                PONumber = "PO" + x.POD_Id,
                                Location = x.LocationName,
                                POType = x.POT_POType,
                                Vendor = x.CMP_NameLegal,
                                DeliveryDate = x.POD_DeliveryDate,
                                POD_EmergencyPODocument = x.POD_EmergencyPODocument == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + POEmeregencyImagePath.Replace("~", "") + x.POD_EmergencyPODocument,
                                IssueDate = x.POD_PODate,
                                BillDate = x.POD_ReoccourringBillDate,
                                InvoicingFrequency = x.CNT_invoicingFrequency,
                                CostDuringPeriod = x.CNT_CostDuringPeriod.ToString(),
                                Comment = x.POE_EmergencyComment,
                                Amount = x.POE_POAmount,
                                VendorName = x.POE_VendorName,
                                Address = x.COD_Address,
                                PointOfContactName = x.COD_PointOfContact,
                                Total = x.POD_POAmount,
                                VendorId = x.POD_CMP_Id,
                                LocationId = x.POD_LocationId,
                                POTypeId = x.POD_POT_Id
                            }).FirstOrDefault();
                            lstAllPODetails.FacilityListData = _workorderems.spGetPOFacilityItemForApproval(objPOCommonServiceModel.POId)
                               .Select(a => new FacilityListData()
                               {
                                   COM_FacilityId = a.CFM_Id,
                                   COM_Facility_Desc = a.CFM_Discription,
                                   UnitPrice = a.CFM_Rate,
                                   Tax = a.CFM_Tax,
                                   FacilityType = a.CFM_FacilityType,
                                   CostCode = a.CFM_CCD_CostCode,
                                   Quantity = a.POF_Unit,
                                   POF_ID = a.POF_Id
                               }).ToList();
                            lstAllPODetails.QuestionAnswerData = _workorderems.spGetPOQuestion(objPOCommonServiceModel.POId).
                                Select(x => new QuestionAnswerData()
                                {
                                    Answer = x.POQ_Answer,
                                    POId = x.POQ_POD_Id,
                                    POQId = x.POQ_Id,
                                    QuestionId = x.POQ_QNA_Id
                                }).ToList();
                            if (lstAllPODetails != null)
                            {
                                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                                ObjServiceResponseModel.Message = CommonMessage.Successful();
                                ObjServiceResponseModel.Data = lstAllPODetails;
                            }
                            else
                            {
                                ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.CurrentCulture);
                                ObjServiceResponseModel.Message = CommonMessage.Successful();
                                ObjServiceResponseModel.Data = null;
                            }
                        }
                        else
                        {
                            ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                            ObjServiceResponseModel.Message = CommonMessage.PONumberIsNotApprove();
                            ObjServiceResponseModel.Data = null;
                        }
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Message = CommonMessage.PONumberNotExist();
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ServiceResponseModel<List<POListModel>> GetAllPOListForMobile(POCommonServiceModel objPOCommonServiceModel)", "Exception While Getting List of All PO.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : Oct-23-2018
        /// Created For : To Get Waiting PO type list and location list
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        public List<POListModel> WaitingPODetailList(ServiceBaseModel objServiceBaseModel)
        {
            try
            {
                var lstPoDetail = new List<POListModel>();
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                        && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    lstPoDetail = _workorderems.spGetPOList(objServiceBaseModel.LocationID, objServiceBaseModel.UserId, objServiceBaseModel.Status).Where(x=> x.PO_Status == "W").Select(a => new POListModel()
                    {
                        LogPOId = a.LPOD_POD_Id,
                        PODate = a.LPOD_PODate,
                        LocationName = a.LocationName,
                        DeliveryDate = a.LPOD_DeliveryDate,
                        CompanyName = a.CMP_NameLegal,
                        POStatus = a.PO_Status,
                        POType = a.POT_POType,
                        LogId = a.LPOD_Id,
                        Total = a.LPOD_POAmount
                    }).ToList();

                    //lstPoDetail = _workorderems.PODetails.Select(x => new PODetailsServiceModelManager()
                    //{
                    //    POD_CMP_Id = x.POD_CMP_Id,
                    //    POD_DeliveryDate = x.POD_DeliveryDate,
                    //    POD_EmergencyPODocument = x.POD_EmergencyPODocument,
                    //    POD_Id = x.POD_Id,
                    //    POD_IsActive = x.POD_IsActive,
                    //    POD_LocationId = x.POD_LocationId,
                    //    POD_PODate = x.POD_PODate,
                    //    POD_POT_Id = x.POD_POT_Id,
                    //    POD_ReoccourringBillDate = x.POD_ReoccourringBillDate

                    //}).Where(x => x.POD_IsActive.ToLower() == "W" && x.POD_LocationId == objServiceBaseModel.LocationID).ToList();
                }
                else
                {
                    return lstPoDetail;
                }
                return lstPoDetail;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PODetailsServiceModel> WaitingPODetailList(ServiceBaseModel objServiceBaseModel)", "Exception While Getting List of Waiting PO details.", null);
                throw;
            }

        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : Oct-23-2018
        /// Created For : To approve PO details
        /// </summary>
        /// <param name="objServiceBaseModel"></param>
        /// <returns></returns>
        public int PODetailsApproval(ServiceBaseModel objServiceBaseModel)
        {
            try
            {
                string action = "I";
                int approvePo = 1;
                string poNumber = "";
                if (objServiceBaseModel != null && objServiceBaseModel.ServiceAuthKey != null
                        && objServiceBaseModel.LocationID > 0 && objServiceBaseModel.UserId > 0)
                {
                    approvePo = _workorderems.spSetApprovalForPODetail(objServiceBaseModel.POId, objServiceBaseModel.Comments, objServiceBaseModel.IsApprove, objServiceBaseModel.UserId);                    
                }
                else
                {
                    return approvePo;
                }
                return approvePo;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool PODetailsApproval(ServiceBaseModel objServiceBaseModel)", "Exception While approving the PO.", null);
                throw;
            }

        }

        public string SendMailToManagerForBudget(long UserId, decimal CalculateRemainingAmt, long CostCodeData, long LocationData, long Vendordata)
        {
            bool isSaved = false;
            string result = "";
            try
            {
                if (CalculateRemainingAmt > 0 && CostCodeData > 0 && LocationData > 0 && Vendordata > 0)
                {
                    isSaved = true;
                    #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        bool IsSent = false;
                    string UserLink = "GlobalAdmin/BudgetAllocation/";
                    if (isSaved == true)
                        {
                            var ManagerData = objEmailLogRepository.SendEmailToManagerForApprovePO(LocationData,UserId);
                            var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == LocationData && x.IsDeleted == false).FirstOrDefault();
                            var VendorAllData = _workorderems.spGetVendorAllDetail(Vendordata).FirstOrDefault();
                            var CostCodeAllData = _workorderems.CostCodes.Where(x => x.CCD_CostCode == CostCodeData && x.CCD_IsActive == "Y").FirstOrDefault();
                            var objEmailHelper = new EmailHelper();
                           foreach (var item in ManagerData)
                           {
                            string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                            HostingPrefix = HostingPrefix + UserLink + "?loc=" + Cryptography.GetEncryptedData(LocationData.ToString(CultureInfo.InvariantCulture), true);
                            objEmailHelper.RegistrationLink = HostingPrefix;
                            objEmailHelper.emailid = item.ManagerEmail;
                            objEmailHelper.ManagerName = item.ManagerName;
                            objEmailHelper.LocationName = locationData.LocationName;
                            objEmailHelper.UserName = item.UserName;
                            objEmailHelper.VendorId = Vendordata.ToString();
                            objEmailHelper.VendorName = VendorAllData.CMP_NameLegal;
                            objEmailHelper.CalculatedAmount = CalculateRemainingAmt.ToString();
                            objEmailHelper.CostCode = CostCodeAllData.CCD_Description.ToString();
                            objEmailHelper.MailType = "SENDMAILFORMOREBUDGET";
                            objEmailHelper.SentBy = UserId;
                            objEmailHelper.LocationID = item.LocationID;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();

                            //Push Notification     
                            //if (userData.u.DeviceId != null)
                            //{
                            //    string message = PushNotificationMessages.POApprovedReject(objEmailHelper.LocationName, objEmailHelper.PONumber);
                            //    PushNotification.GCMAndroid(message, userData.u.DeviceId, objEmailHelper);
                            //}
                            if (IsSent == true)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = UserId;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = LocationData;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = UserId;
                                    objEmailog.SentEmail = item.ManagerEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = item.ManagerUserId;
                                    objListEmailog.Add(objEmailog);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                result = CommonMessage.SendMailForBudget();
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
                        result = CommonMessage.FailureSendMailForBudget(); 
                        }
                    
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string ApprovePOByPOId(long Id)", "Exception While Approving PO.", null);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-March-2019
        /// Created For : To get Self PO List.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <param name="UserTypeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<POListModel> GetAllSelfPOList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            long Type = Convert.ToInt64(UserType.Employee);
            var Results = new List<POListModel>();
            try
            { 
                Results = _workorderems.spGetSelfPOList(UserId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                .Select(a => new POListModel()
                {
                    id= Cryptography.GetEncryptedData(Convert.ToString(a.LPOD_POD_Id), true),
                    LogPOId = a.LPOD_POD_Id,
                    DisplayLogPOId="PO"+ a.LPOD_POD_Id,
                    DisplayPODate = a.LPOD_PODate != null ? a.LPOD_PODate.ToString("yyyy/MM/dd") : "N/A",
                    LocationName = a.LocationName,
                    DeliveryDate = a.LPOD_DeliveryDate,
                    DisplayDeliveryDate= a.LPOD_DeliveryDate != null ? a.LPOD_DeliveryDate.ToString("yyyy/MM/dd") : "N/A",
                    CompanyName = a.CMP_NameLegal,
                    POStatus = a.PO_Status,
                    POType = a.POT_POType,
                    POStatusToDisplay = a.PO_Status,
                    LogId = a.LPOD_Id,
                    Total = a.LPOD_POAmount,
                    UserName = a.Employee_Name
                }).OrderByDescending(x => x.LogPOId).ToList(); 
                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POListDetails GetAllSelfPOList(long? UserId, long? LocationId, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of self PO.", null);
                throw;
            }
        }


        /// <summary>
        /// Created By : Ajay kumar
        /// Created Date : Oct-30-2019
        /// Created For : To Get PO Details For Graphs.
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public POApproveRejectModel GetPODetailsForGraphs(long Userid)
        {
            POApproveRejectModel model = new POApproveRejectModel();
            try
            {
               

                var _result = _workorderems.SP_GetAllDetailsOfPOForGraphs(Userid).FirstOrDefault();
                if (_result !=null) 
                {
                    model.UnauthorizePoCount = _result.UnauthorizePoCount;
                    model.OpenPOCount = _result.OpenPOCount;
                    model.NotApprovedPoCount = _result.NotApprovedPoCount;

                }  
              
            }
            catch (Exception ex)
            {
                
            }
            return model;
        }

        public List<GetBudgetDetailsForPOSGraphs> GetAllBudgetDetailsForPOGraphs()
        {
            List<GetBudgetDetailsForPOSGraphs> modellist = new List<GetBudgetDetailsForPOSGraphs>();
            var result = _workorderems.spGetBudgetDetailsForPOGraphs().ToList();
            if (result.Count() > 0)
            {
                Random r = new Random();
                foreach (var item in result)
                {

                    GetBudgetDetailsForPOSGraphs model = new GetBudgetDetailsForPOSGraphs();
                    model.BudgetAmount = item.BudgetAmount;
                    model.Years = item.Years;
                    model.colour = String.Format("#{0:X6}", r.Next(0x1000000)); // = "#A197B9";
                    modellist.Add(model);
                }

            }
            return modellist;
        }

    }
}
