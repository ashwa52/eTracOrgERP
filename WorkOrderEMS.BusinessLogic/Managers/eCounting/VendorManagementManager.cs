using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class VendorManagementManager : IVendorManagement
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string Path = ConfigurationManager.AppSettings["CompanyDocument"];

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-Oct-2018
        /// Created For : To get list of vendor type
        /// </summary>
        /// <returns></returns>
        public List<VendorTypeData> ListVendorType()
        {
            var lstVendorType = new List<VendorTypeData>();
            try
            {
                lstVendorType = _workorderems.VendorTypes.Where(x => x.VDT_IsActive == "Y").Select
                (x => new VendorTypeData()
                {
                    VendorTypeId = x.VDT_Id,
                    VendorTypeName = x.VDT_VendorType
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorTypeData ListVendorType()", "Exception While Getting List of Vendor Type.", null);
                throw;
            }
            return lstVendorType;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-Oct-2018
        /// Created For : To get list of Payment mode
        /// </summary>
        /// <returns></returns>
        public List<PaymentModeList> PaymentModeList()
        {
            var lstVendorType = new List<PaymentModeList>();
            try
            {
                lstVendorType = _workorderems.spGetPaymentMode().Where(x => x.PMD_IsActive == "Y").Select
                (x => new PaymentModeList()
                {
                    PaymentModeId = x.PMD_Id,
                    PaymentModeName = x.PMD_PaymentMode
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PaymentModeList> PaymentModeList()", "Exception While Getting List of Payment Mode Type.", null);
                throw;
            }
            return lstVendorType;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-Oct-2018
        /// Created For : To get list of Payment Term
        /// </summary>
        /// <returns></returns>
        public List<PaymentTermList> PaymentTermList()
        {
            var lstVendorType = new List<PaymentTermList>();
            try
            {
                lstVendorType = _workorderems.spGetPaymentTerm().Where(x => x.PTM_IsActive == "Y").Select
                (x => new PaymentTermList()
                {
                    PaymentTermId = x.PTM_Id,
                    PaymentTermName = x.PTM_Term
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PaymentTermList> PaymentTermList()", "Exception While Getting List of Payment Term .", null);
                throw;
            }
            return lstVendorType;
        }
        /// <summary>
        /// Created By:Ashwajit Bansod
        /// Created Date : 10-Oct-2018
        /// Created For : To get list of costcode
        /// </summary>
        /// <returns></returns>
        public List<CostCodeListData> ListAllCostCode()
        {
            var lstVendorType = new List<CostCodeListData>();
            try
            {
                lstVendorType = _workorderems.CostCodes.Where(x => x.CCD_IsActive == "Y" && x.CCD_CCM_CostCode != 16000).Select
                (x => new CostCodeListData()
                {
                    CostCodeId = x.CCD_CostCode,
                    CostCodeName = x.CCD_Description
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<CostCodeListData> ListAllCostCode()", "Exception While Getting List of Cost Code.", null);
                throw;
            }
            return lstVendorType;
        }

        /// <summary>
        /// Created By Ashwajit Bansod
        /// Created Date : 20-Nov-2018
        /// Created For : To get all allocated location for vendor
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public List<LocationListServiceModel> ListAllocatedLocatioForVender(long VendorId)
        {
            var lstLocation = new List<LocationListServiceModel>();
            try
            {
                lstLocation = _workorderems.spGetLocationCompanyMappingForApproval(VendorId).Select
                (x => new LocationListServiceModel()
                {
                    LocationId = x.LLCM_LocationId,
                    LocationName = x.LocationName
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<CostCodeListData> ListAllCostCode()", "Exception While Getting List of Cost Code.", null);
                throw;
            }
            return lstLocation;
        }
        public string ListAllAlocatedLocatioForVender(long VendorId)
        {
            string lstLocation = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                var data = _workorderems.spGetLocationCompanyMappingForApproval(VendorId);
                foreach (var item in data)
                {
                    //lstLocation = item.LCM_CMP_Id.ToString();
                    sb.Append(item + ", ");
                    //lstLocation = String.Join(",", item.LCM_CMP_Id);
                }
                lstLocation = sb.ToString().TrimEnd(new char[] { ',' }); ;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<CostCodeListData> ListAllCostCode()", "Exception While Getting List of Cost Code.", null);
                throw;
            }
            return lstLocation;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-OCT-2018
        /// Created For : To save vendor
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public VendorSetupManagementModel ProcessVendorSetup(VendorSetupManagementModel Obj)
        {
            var objVendorManagement = new VendorSetupManagementModel();
            int? VendorId = 0;
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            string LocName = "";
            string annualValue = string.Empty; string costPeriod = string.Empty;
            string latefine = string.Empty; string minimumBill = string.Empty;
            long COT_ID = 0;
            long Primarymode = 0;
            if (Obj.CompanyType == 2 || Obj.CompanyType == 3)
            {
                COT_ID = Convert.ToInt64(Obj.CompanyType);
                Obj.VendorType = Convert.ToInt64(VendorTypeValue.VendorType);
                Primarymode = Obj.VendorAccountDetailsModel.PaymentMode;
            }
            else
            {
                Primarymode = Obj.VendorContractModel.PrimaryPaymentMode;
                COT_ID = Convert.ToInt64(eCounting.COT_ID);
            }
            string Action = "";
            try
            {
                if (Obj.VendorId == 0)
                {
                    Action = "I";
                    var Vendor = _workorderems.spSetCompany(Action, null, Obj.CompanyNameLegal, Obj.CompanyNameDBA,
                                                          Obj.VendorType, COT_ID, Obj.CompanyDocuments, Obj.UserId,
                                                          null, "N");
                    //Vendor.FirstOrDefault();
                    VendorId = Vendor.FirstOrDefault();
                    Obj.VendorId = VendorId;
                    if (VendorId > 0)
                    {
                        var SaveCompanyDetails = _workorderems.spSetCompanyDetail(Action, null, VendorId, Obj.PointOfContact,
                                                                    Obj.JobTile, null, Obj.Address1, Obj.Address1City,
                                                                    Obj.Address1State, 1, Obj.Address2, Obj.Address2City,
                                                                    Obj.StateAfterIsSame, 1, Obj.Phone1, Obj.Phone2,
                                                                    Obj.VendorEmail, Obj.Website, Obj.UserId, null, "N");

                        var SaveTax = _workorderems.spSetTaxDetail(Action, null, VendorId, Obj.TaxNo, null, Obj.UserId, null, "N");

                        if (Obj.VendorContractModel != null)
                        {
                            if (Obj.VendorContractModel.ContractExecutedBy == null)
                            {
                                Obj.VendorContractModel.ContractExecutedBy = Obj.VendorContractModel.ContractIssuedBy;
                            }
                            if (Obj.VendorContractModel.AnnualValueOfAggrimentForView != null)
                            {
                                annualValue = Obj.VendorContractModel.AnnualValueOfAggrimentForView.Replace(",", "");
                                Obj.VendorContractModel.AnnualValueOfAggriment = Convert.ToInt64(annualValue);
                            }
                            if (Obj.VendorContractModel.CostDuringPeriodForView != null)
                            {
                                costPeriod = Obj.VendorContractModel.CostDuringPeriodForView.Replace(",", "");
                                Obj.VendorContractModel.CostDuringPeriod = Convert.ToInt32(costPeriod);
                            }
                            if (Obj.VendorContractModel.LateFineForView != null)
                            {
                                latefine = Obj.VendorContractModel.LateFineForView.Replace(",", "");
                                Obj.VendorContractModel.LateFine = Convert.ToDecimal(latefine);
                            }
                            if (Obj.VendorContractModel.MinimumBillAmountForView != null)
                            {
                                minimumBill = Obj.VendorContractModel.MinimumBillAmountForView.Replace(",", "");
                                Obj.VendorContractModel.MinimumBillAmount = Convert.ToDecimal(minimumBill);
                            }
                            //var annualValue =  Obj.VendorContractModel.AnnualValueOfAggrimentForView.Replace(",", "");
                            //var costPeriod = Obj.VendorContractModel.CostDuringPeriodForView.Replace(",", "");
                            //var latefine = Obj.VendorContractModel.LateFineForView.Replace(",", "");
                            //var minimumBill = Obj.VendorContractModel.MinimumBillAmountForView.Replace(",", "");
                            //Obj.VendorContractModel.AnnualValueOfAggriment = Convert.ToInt64(annualValue);
                            //Obj.VendorContractModel.CostDuringPeriod = Convert.ToInt32(costPeriod);
                            //Obj.VendorContractModel.LateFine = Convert.ToDecimal(latefine);
                            //Obj.VendorContractModel.MinimumBillAmount = Convert.ToDecimal(minimumBill);
                            var saveContract = _workorderems.spSetContract(Action, null, Obj.VendorContractModel.FirstCompany, VendorId,
                                                                         Obj.VendorContractModel.ContractType, Obj.VendorContractModel.ContractExecutedBy,
                                                                         Obj.VendorContractModel.ContractIssuedBy, Obj.VendorContractModel.PaymentTerm,
                                                                         Obj.VendorContractModel.PrimaryPaymentMode, Obj.VendorContractModel.GracePeriod,
                                                                         Obj.VendorContractModel.InvoicingFrequecy, Obj.VendorContractModel.CostDuringPeriod,
                                                                         Obj.VendorContractModel.AllocationNeeded, Obj.VendorContractModel.StartDate,
                                                                         Obj.VendorContractModel.EndDate, Obj.VendorContractModel.AnnualValueOfAggriment,
                                                                         Obj.VendorContractModel.MinimumBillAmount, Obj.VendorContractModel.BillDueDate,
                                                                         Obj.VendorContractModel.LateFine, Obj.VendorContractModel.ContractDocuments,
                                                                         Obj.UserId, null, "Y", Obj.VendorContractModel.ReccurenceForPO);
                        }
                        if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.InsuranceCarries != null)
                        {
                            var saveInsuranceLicense = _workorderems.spSetInsurance(Action, null, VendorId,
                                                                     Obj.VendorInsuranceModel.InsuranceCarries, Obj.VendorInsuranceModel.PolicyNumber,
                                                                     Obj.VendorInsuranceModel.InsuranceExpirationDate, Obj.VendorInsuranceModel.InsuranceDocument,
                                                                     Obj.UserId, null, "Y");
                        }
                        if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.LicenseName != null)
                        {
                            var saveInsuranceLicense = _workorderems.spSetLicense(Action, null, VendorId, Obj.VendorInsuranceModel.LicenseName,
                                                                     Obj.VendorInsuranceModel.LicenseNumber, Obj.VendorInsuranceModel.LicenseExpirationDate,
                                                                     Obj.VendorInsuranceModel.LicenseDocument, Obj.UserId, null, "Y");
                        }
                        if (Obj.VendorAccountDetailsModel != null)
                        {
                            var saveAccountDetails = _workorderems.spSetCompanyAccountDetail(Action, null, VendorId, Primarymode,
                                                                          Obj.VendorAccountDetailsModel.BankName, Obj.VendorAccountDetailsModel.BankLocation,
                                                                          Obj.VendorAccountDetailsModel.AccountNumber, Obj.VendorAccountDetailsModel.CardNumber,
                                                                          Obj.VendorAccountDetailsModel.IFSCCode, Obj.VendorAccountDetailsModel.SwiftOICCode,
                                                                          Obj.VendorAccountDetailsModel.AccountDocuments, Obj.UserId, null, "Y", Obj.VendorAccountDetailsModel.BalanceAmount, Obj.VendorAccountDetailsModel.QuickbookAcountId);
                        }
                        if (Obj.VendorFacilityListModel != null)
                        {
                            foreach (var item in Obj.VendorFacilityListModel)
                            {
                                var unitcost = item.UnitCostForView.Replace(",", "");
                                item.UnitCost = Convert.ToDecimal(unitcost);
                                var saveVendorFacility = _workorderems.spSetCompanyFacilityMapping(Action, null, VendorId, item.Costcode,
                                                                          item.ProductServiceType, item.ProductServiceName, item.UnitCost, item.Tax, Obj.UserId,
                                                                          null, "Y");

                                Obj.Result = Result.Completed;
                            }
                        }



                        string LocIds = "";
                        if (Obj.SelectedLcation != null)
                        {
                            string[] LoctionIds = Obj.SelectedLcation.Split(',');
                            for (int i = 0; i < LoctionIds.Length; i++)
                            {
                                if (LoctionIds[i] != null && !string.IsNullOrEmpty(LoctionIds[i]) && Convert.ToInt64(LoctionIds[i], CultureInfo.InvariantCulture) > 0)
                                {
                                    long LocId = Convert.ToInt64(LoctionIds[i]);
                                    var locData = _workorderems.LocationMasters.Where(x => x.LocationId == LocId && x.IsDeleted == false).FirstOrDefault();
                                    LocName = String.Join(",", locData.LocationName);
                                    LocIds = String.Join(",", LocId);
                                    var saveLocationAllocation = _workorderems.spSetLocationCompanyMapping(Action, null, LocId, VendorId, Obj.UserId, null, "Y");
                                }
                            }
                        }
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        if (Obj.Result == Result.Completed)
                        {

                            bool IsSent = false;
                            var objEmailHelper = new EmailHelper();
                            objEmailHelper.emailid = Obj.VendorEmail;
                            objEmailHelper.LocationName = LocName;
                            objEmailHelper.VendorId = Obj.VendorId.ToString();
                            objEmailHelper.VendorName = Obj.CompanyNameLegal;
                            objEmailHelper.MailType = "VENDORCREATE";
                            objEmailHelper.SentBy = Obj.UserId;
                            objEmailHelper.LocationIdsVendor = LocIds;
                            objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                            IsSent = objEmailHelper.SendEmailWithTemplate();
                            if (Obj.Result == Result.Completed)
                            {
                                var objEmailog = new EmailLog();
                                try
                                {
                                    objEmailog.CreatedBy = Obj.UserId; ;
                                    objEmailog.CreatedDate = DateTime.UtcNow;
                                    objEmailog.DeletedBy = null;
                                    objEmailog.DeletedOn = null;
                                    objEmailog.LocationId = null;
                                    objEmailog.ModifiedBy = null;
                                    objEmailog.ModifiedOn = null;
                                    objEmailog.SentBy = Obj.UserId;
                                    objEmailog.SentEmail = Obj.VendorEmail;
                                    objEmailog.Subject = objEmailHelper.Subject;
                                    objEmailog.SentTo = Obj.UserId;
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
                    #region Save DAR
                    objDAR.ActivityDetails = DarMessage.CreateVendor(LocName);
                    objDAR.TaskType = (long)TaskTypeCategory.CreateVendor;
                    #endregion Save DAR
                }
                else
                {
                    var context = new workorderEMSEntities();
                    Action = "U";
                    var Vendor = context.spSetCompany(Action, Obj.VendorId, Obj.CompanyNameLegal, Obj.CompanyNameDBA,
                                                         Obj.VendorType, COT_ID, Obj.CompanyDocuments, Obj.UserId,
                                                         null, "N").FirstOrDefault();
                    var companyDetais = context.CompanyDetails.Where(x => x.COD_CMP_Id == Obj.CompanyId && x.COD_IsActive == "Y").FirstOrDefault();
                    if(companyDetais != null)
                    { 
                    var SaveCompanyDetails = context.spSetCompanyDetail(Action, companyDetais.COD_Id, Obj.CompanyId, Obj.PointOfContact,
                                                               Obj.JobTile, null, Obj.Address1, Obj.Address1City,
                                                               Obj.Address1State, 1, Obj.Address2, Obj.Address2City,
                                                               Obj.StateAfterIsSame, 1, Obj.Phone1, Obj.Phone2,
                                                               Obj.VendorEmail, Obj.Website, Obj.UserId, null, "N");
                    ///var SaveTax = _workorderems.spSetTaxDetail(Action, null, Obj.VendorId, Obj.TaxNo, null, Obj.UserId, null, "N");
                    }

                    string LocIds = "";
                    if (Obj.SelectedLcation != null)
                    {
                        string[] LoctionIds = Obj.SelectedLcation.Split(',');
                        for (int i = 0; i < LoctionIds.Length; i++)
                        {
                            if (LoctionIds[i] != null && !string.IsNullOrEmpty(LoctionIds[i]) && Convert.ToInt64(LoctionIds[i], CultureInfo.InvariantCulture) > 0)
                            {
                                long LocId = Convert.ToInt64(LoctionIds[i]);
                                var lmcId = _workorderems.LocationCompanyMappings.Where(x => x.LCM_LocationId == LocId && x.LCM_CMP_Id == Obj.CompanyId).Select(x=>x.LCM_Id).FirstOrDefault();
                                var saveLocationAllocation = _workorderems.spSetLocationCompanyMapping(Action, lmcId, LocId, Obj.VendorId, Obj.UserId, null, "Y");
                            }
                        }
                    }

                    Obj.Result = Result.UpdatedSuccessfully;
                    #region Save DAR
                    objDAR.ActivityDetails = DarMessage.UpdateVendor();
                    objDAR.TaskType = (long)TaskTypeCategory.UpdateVendor;
                    #endregion Save DAR
                }
                #region DAR
                objDAR.UserId = Obj.UserId;
                objDAR.CreatedBy = Obj.UserId;
                objDAR.CreatedOn = DateTime.UtcNow;
                CommonManager.SaveDAR(objDAR);
                #endregion DAR
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel ProcessVendorSetup(VendorSetupManagementModel Obj)", "Exception While Saving All Vendor Data.", Obj);
                throw;
            }
            return Obj;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-OCT-2018
        /// Created For : To Get all company or vendor list as per LocationId
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objCompanyListDetails = new CompanyListDetails();
                var Results = new List<VendorSetupManagementModel>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                if (LocationId > 0)
                {
                    Results = _workorderems.spGetCompanyList(LocationId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                   .Select(a => new VendorSetupManagementModel()
                   {
                       VendorId = a.CMP_Id,
                       CompanyNameLegal = a.CMP_NameLegal,
                       Address1 = a.Address1,
                       Phone1 = a.COD_Phone1,
                       PointOfContact = a.COD_PointOfContact,
                       VendorTypeData = a.VDT_VendorType,
                       Status = a.Status,
                       AccountStatus = a.AccountStatus,
                       InsuranceStatus = a.InsuranceStatus,
                       LicenseStatus = a.LicenseStatus
                   }).OrderByDescending(x => x.VendorId).ToList();
                }
                else
                {
                    Results = _workorderems.spGetCompanyList(LocationId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorSetupManagementModel()
                    {
                        VendorId = a.CMP_Id,
                        CompanyNameLegal = a.CMP_NameLegal,
                        Address1 = a.Address1,
                        Phone1 = a.COD_Phone1,
                        PointOfContact = a.COD_PointOfContact,
                        VendorTypeData = a.VDT_VendorType,
                        Status = a.Status,
                        AccountStatus = a.AccountStatus,
                        InsuranceStatus = a.InsuranceStatus,
                        LicenseStatus = a.LicenseStatus
                    }).Where(x => x.Status == "W").OrderByDescending(x => x.VendorId).ToList();
                }
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objCompanyListDetails.pageindex = pageindex;
                objCompanyListDetails.total = totalPages;
                objCompanyListDetails.records = totRecords;
                objCompanyListDetails.rows = Results.ToList();
                return objCompanyListDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }
        public IList<VendorSetupManagementModel> GetAllCompanyDataList1(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            var Results = new List<VendorSetupManagementModel>();
            try
            {
                var objCompanyListDetails = new CompanyListDetails();

                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                if (LocationId > 0)
                {
                    Results = _workorderems.spGetCompanyList(LocationId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                   .Select(a => new VendorSetupManagementModel()
                   {
                       id=Cryptography.GetEncryptedData(Convert.ToString(a.CMP_Id),true),
                       VendorId = a.CMP_Id,
                       CompanyNameLegal = a.CMP_NameLegal,
                       Address1 = a.Address1,
                       Phone1 = a.COD_Phone1,
                       PointOfContact = a.COD_PointOfContact,
                       VendorTypeData = a.VDT_VendorType,
                       Status = a.Status,
                       AccountStatus = a.AccountStatus,
                       InsuranceStatus = a.InsuranceStatus,
                      LicenseStatus = a.InsuranceStatus, 
                     
                   }).Where(x => x.Status == "Y").OrderByDescending(x => x.VendorId).ToList();
                }
                else
                {
                    Results = _workorderems.spGetCompanyList(LocationId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorSetupManagementModel()
                    {
                        VendorId = a.CMP_Id,
                        id = Cryptography.GetEncryptedData(Convert.ToString(a.CMP_Id), true),
                        CompanyNameLegal = a.CMP_NameLegal,
                        Address1 = a.Address1,
                        Phone1 = a.COD_Phone1,
                        PointOfContact = a.COD_PointOfContact,
                        VendorTypeData = a.VDT_VendorType,
                        Status = a.Status == "W" ? "Waiting" : a.Status == "N" ? "Rejected":"Y",
                    // = a.Status == "W" ? "Waiting" : "Rejected",
                        AccountStatus = a.AccountStatus,
                        InsuranceStatus = a.InsuranceStatus,
                        LicenseStatus = a.InsuranceStatus,
                    }).Where(x => x.Status != "Y").OrderByDescending(x => x.VendorId).ToList();
                }
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objCompanyListDetails.pageindex = pageindex;
                //objCompanyListDetails.total = totalPages;
                //objCompanyListDetails.records = totRecords;
                //objCompanyListDetails.rows = Results.ToList();
                return Results.ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-OCT-2018
        /// Created For : To get all vendor data and company faciliy list as per vendor id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public VendorAllViewDataModel GetAllVendorData(long VendorId)
        {
            var vendorData = new VendorAllViewDataModel();
            try
            {

                vendorData = _workorderems.spGetVendorAllDetailForApproval(VendorId).Select
                (x => new VendorAllViewDataModel()
                {
                    CompanyNameLegal = x.CMP_NameLegal,
                    CompanyNameDBA = x.CMP_NameDBA,
                    Address1 = x.COD_Address1 + "-" + x.COD_Addr1City,
                    Address2 = x.COD_Address2 + "-" + x.COD_Addr2City,
                    Email = x.COD_Email == null ? "N/A" : x.COD_Email,
                    Website = x.COD_Website == null ? "N/A" : x.COD_Website,
                    Phone1 = x.COD_Phone1,
                    Phone2 = x.COD_Phone2 == null ? "N/A" : x.COD_Phone2,
                    PointOfContact = x.COD_PointOfContact,
                    JobTile = x.COD_JobTitle,
                    VendorTypeData = x.VDT_VendorType,

                    //this is for Insurance and License
                    LicenseNumber = x.LNC_LicenseNumber == null ? "N/A" : x.LNC_LicenseNumber,
                    LicenseExpirationDate = x.LNC_ExpirationDate == null ? "N/A" : x.LNC_ExpirationDate.ToString("yyyy/MM/dd"),
                    InsuranceCarries = x.INS_IncuranceCarrier == null ? "N/A" : x.INS_IncuranceCarrier,
                    InsuranceExpirationDate = x.INS_ExpirationDate == null ? "N/A" : x.INS_ExpirationDate.ToString("yyyy/MM/dd"),
                    PolicyNumber = x.INS_PolicyNumber == null ? "N/A" : x.INS_PolicyNumber,

                    //This is for Contract
                    SecondaryCompany = x.CNT_CMP_IdSecondParty.ToString(),
                    ContractType = x.CTT_ContractType,
                    ContractIssuedBy = x.CNT_IssuedBy,
                    ContractExecutedBy = x.CNT_ExcutedBy,
                    PrimaryPaymentMode = x.PMD_PaymentMode,
                    PaymentTerm = x.PTM_Term,
                    GracePeriod = x.CNT_GracePeriod,
                    InvoicingFrequecy = x.CNT_invoicingFrequency,
                    StartDate = x.CNT_StartDate.ToString("yyyy/MM/dd"),
                    EndDate = x.CNT_EndDate.ToString("yyyy/MM/dd"),
                    CostDuringPeriod = x.CNT_CostDuringPeriod == 0 ? 0 : x.CNT_CostDuringPeriod,
                    AnnualValueOfAggriment = x.CNT_AnnualValueOfAggreement == 0 ? 0 : x.CNT_AnnualValueOfAggreement,
                    MinimumBillAmount = x.CNT_MinimumBillAmount == null ? 0 : x.CNT_MinimumBillAmount,
                    BillDueDate = x.CNT_BillDueDate == null ? "N/A" : x.CNT_BillDueDate.ToString("yyyy/MM/dd"),
                    LateFine = x.CNT_LateFeeFine == null ? 0 : x.CNT_LateFeeFine,
                    //This is for account Details
                    AccountNumber = x.CAD_AccountNumber == null ? "N/A" : x.CAD_AccountNumber,
                    BankName = x.CAD_CardOrBankName == null ? "N/A" : x.CAD_CardOrBankName,
                    BankLocation = x.CAD_BankLocation == null ? "N/A" : x.CAD_BankLocation,
                    IFSCCode = x.CAD_IFSCcode == null ? "N/A" : x.CAD_IFSCcode,
                    SwiftOICCode = x.CAD_SwiftBICcode == null ? "N/A" : x.CAD_SwiftBICcode,
                    CardNumber = x.CAD_CreditCardNumber == null ? "N/A" : x.CAD_CreditCardNumber,
                    PolicyNumberAccount = x.INS_PolicyNumber == null ? "N/A" : x.INS_PolicyNumber,
                    CardHolderName = x.CAD_CardOrBankName == null ? "N/A" : x.CAD_CardOrBankName,
                    //ExpirationDate = x.
                }).FirstOrDefault();
                vendorData.VendorFacilityModel = _workorderems.spGetCompanyFacilityMappingForApproval(VendorId).Select
                (x => new VendorFacilityModel()
                {
                    Costcode = x.CFM_CCD_CostCode,
                    //FacilityId= x.,
                    ProductServiceName = x.CFM_Discription,
                    ProductServiceType = x.CFM_FacilityType,
                    Tax = x.CFM_Tax,
                    UnitCost = x.CFM_Rate,
                }).ToList();
                vendorData.LocationAssignedModel = _workorderems.spGetLocationCompanyMappingForApproval(VendorId).Select
                (x => new LocationDataModel()
                {
                    LocationName = x.LocationName,
                    LocationId = x.LLCM_LocationId,
                    LLCM_Id = x.LLCM_Id
                }).ToList();

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorAllViewDataModel ListAllCostCode()", "Exception While Getting All vendor Details.", null);
                throw;
            }
            return vendorData;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2018
        /// Created For : To Approve or Reject Vendor By Using vendor Id
        /// </summary>
        /// <param name="ObjApproveRejectVendorModel"></param>
        /// <returns></returns>
        public string ApproveVendorByVendorId(ApproveRejectVendorModel ObjApproveRejectVendorModel)
        {
            bool isSaved = false;
            string returnvalue = "";
            string ApproveRemoveSatus = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            string Status = "";
            try
            {
                if (ObjApproveRejectVendorModel.Vendor > 0)
                {
                    var userData = _workorderems.UserRegistrations.Where(x => x.UserId == ObjApproveRejectVendorModel.UserId
                                                    && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    if (userData != null)
                    {
                        if (ObjApproveRejectVendorModel.Comment == null)
                        {
                            returnvalue = CommonMessage.VendorApprove();
                            ApproveRemoveSatus = "Appoved";
                            Status = "Y";
                        }
                        else
                        {
                            returnvalue = CommonMessage.VendorReject();
                            ApproveRemoveSatus = "Removed";
                            Status = "N";
                        }



                        var IsApprove = _workorderems.spSetApprovalForVendorAllDetail(ObjApproveRejectVendorModel.Vendor,
                                                                             ObjApproveRejectVendorModel.Comment, Status, ObjApproveRejectVendorModel.UserId);
                        #region "TEQ"
                        var _locationList = _workorderems.LocationCompanyMappings.Where(n => n.LCM_CMP_Id == ObjApproveRejectVendorModel.Vendor).ToList();
                        if (_locationList != null)
                        {
                            foreach (var item in _locationList)
                            {
                                long LocId = Convert.ToInt64(item.LCM_Id);
                                var result = _workorderems.LocationCompanyMappings.Where(n => n.LCM_Id == LocId).FirstOrDefault();
                                if (result != null)
                                {
                                    result.LCM_IsActive = Status;
                                    _workorderems.SaveChanges();
                                }
                              
                            } 
                        }
                        #endregion
                        //if (ObjApproveRejectVendorModel.LLCM_Id != null)
                        //{
                        //    string[] LLCM_Id = ObjApproveRejectVendorModel.LLCM_Id.Split(',');
                        //    for (int i = 0; i < LLCM_Id.Length; i++)
                        //    {
                        //        if (LLCM_Id[i] != null && !string.IsNullOrEmpty(LLCM_Id[i]) && Convert.ToInt64(LLCM_Id[i], CultureInfo.InvariantCulture) > 0)
                        //        {
                        //            long LocId = Convert.ToInt64(LLCM_Id[i]);
                        //            var locationApprove = _workorderems.spSetApprovalForLocationCompanyMapping(LocId,
                        //                                                                           ObjApproveRejectVendorModel.Comment,
                        //                                                                           Status, ObjApproveRejectVendorModel.UserId);

                        //        }
                        //    }
                        //}


                        var facilityApprove = _workorderems.spSetApprovalForCompanyFacilityMapping(ObjApproveRejectVendorModel.Vendor,
                                                                                                   ObjApproveRejectVendorModel.Comment,
                                                                                                   Status, ObjApproveRejectVendorModel.UserId);
                        isSaved = true;

                    }
                    else { isSaved = false; }
                    if (isSaved == true)
                    {
                        #region Email
                        var objEmailLogRepository = new EmailLogRepository();
                        var objEmailReturn = new List<EmailToManagerModel>();
                        var objListEmailog = new List<EmailLog>();
                        var objTemplateModel = new TemplateModel();
                        if (isSaved == true)
                        {
                            var vendorDetail = _workorderems.spGetVendorAllDetail(ObjApproveRejectVendorModel.Vendor).FirstOrDefault();
                            var locationDetails = _workorderems.LocationMasters.Where(x => x.LocationId == ObjApproveRejectVendorModel.LocationId
                                                                                 && x.IsDeleted == false).FirstOrDefault();
                            if (vendorDetail != null)
                            {
                                bool IsSent = false;
                                var objEmailHelper = new EmailHelper();
                                objEmailHelper.emailid = vendorDetail.COD_Email;
                                objEmailHelper.VendorName = vendorDetail.CMP_NameLegal;
                                objEmailHelper.LocationName = locationDetails.LocationName;
                                objEmailHelper.VendorId = ObjApproveRejectVendorModel.Vendor.ToString();
                                objEmailHelper.ApproveRemoveStatus = ApproveRemoveSatus;
                                objEmailHelper.MailType = "VENDORAPPROVEDREJECT";
                                objEmailHelper.SentBy = ObjApproveRejectVendorModel.UserId;
                                objEmailHelper.LocationID = ObjApproveRejectVendorModel.LocationId;
                                objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                IsSent = objEmailHelper.SendEmailWithTemplate();

                                //Push Notification     
                                if (IsSent == true)
                                {
                                    var objEmailog = new EmailLog();
                                    try
                                    {
                                        objEmailog.CreatedBy = ObjApproveRejectVendorModel.UserId;
                                        objEmailog.CreatedDate = DateTime.UtcNow;
                                        objEmailog.DeletedBy = null;
                                        objEmailog.DeletedOn = null;
                                        objEmailog.LocationId = ObjApproveRejectVendorModel.LocationId;
                                        objEmailog.ModifiedBy = null;
                                        objEmailog.ModifiedOn = null;
                                        objEmailog.SentBy = ObjApproveRejectVendorModel.LocationId;
                                        objEmailog.SentEmail = vendorDetail.COD_Email;
                                        objEmailog.Subject = objEmailHelper.Subject;
                                        objEmailog.SentTo = ObjApproveRejectVendorModel.Vendor;
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
                            #region Save DAR
                            objDAR.ActivityDetails = DarMessage.VendorApprovedCancel(vendorDetail.CMP_NameLegal, locationDetails.LocationName, ApproveRemoveSatus);
                            objDAR.TaskType = (long)TaskTypeCategory.PaymentApporveCancel;
                            objDAR.UserId = ObjApproveRejectVendorModel.UserId;
                            objDAR.CreatedBy = ObjApproveRejectVendorModel.UserId;
                            objDAR.LocationId = ObjApproveRejectVendorModel.LocationId;
                            objDAR.CreatedOn = DateTime.UtcNow;
                            CommonManager.SaveDAR(objDAR);
                            #endregion DAR

                        }
                        else
                        {
                            returnvalue = CommonMessage.FailureMessage();
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
        /// Created By: Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Company or vendor details by vendor id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public VendorSetupManagementModel GetVendorDetailsByVendorId(long VendorId)
        {
            var vendorDetails = new VendorSetupManagementModel();
            try
            {
                if (VendorId > 0)
                {
                    var data = _workorderems.spGetCompanyDetailEdit(VendorId).FirstOrDefault();
                    //.Select(x => new VendorSetupManagementModel()
                    //{
                    //VendorType = x.CMP_VDT_Id,
                    //Address1 = x.COD_Address1,
                    //Address2 = x.COD_Address2,
                    //Address1City = x.COD_Addr1City,
                    //Address2City = x.COD_Addr2City,
                    //Address1State = x.COD_Addr1StateId,
                    //Address2State = x.COD_Addr2StateId,
                    //CompanyNameLegal = x.CMP_NameLegal,
                    //CompanyNameDBA = x.CMP_NameDBA,
                    //Phone1 = x.COD_Phone1,
                    //Phone2 = x.COD_Phone2,
                    //PointOfContact = x.COD_PointOfContact,
                    //VendorEmail = x.COD_Email,
                    //Website = x.COD_Website,
                    //JobTile = x.COD_JobTitle,
                    //VendorId = x.COD_CMP_Id,
                    //CompanyDocEdit = x.CMP_CompanyDocument,
                    //CompanyId = x.COD_Id,
                    //TaxNo = x.TXD_TaxIdNumber,
                    //CompanyDocuments = x.CMP_CompanyDocument == null ? "" : HostingPrefix + Path.Replace("~", "") + x.CMP_CompanyDocument
                    // }).FirstOrDefault();
                    vendorDetails.VendorType = data.CMP_VDT_Id;
                    vendorDetails.Address1 = data.COD_Address1;
                    vendorDetails.Address2 = data.COD_Address2;
                    vendorDetails.Address1City = data.COD_Addr1City;
                    vendorDetails.Address2City = data.COD_Addr2City;
                    vendorDetails.Address1State = data.COD_Addr1StateId;
                    vendorDetails.Address2State = data.COD_Addr2StateId;
                    vendorDetails.CompanyNameLegal = data.CMP_NameLegal;
                    vendorDetails.VendorEmail = data.COD_Email;
                    vendorDetails.CompanyNameDBA = data.CMP_NameDBA;
                    vendorDetails.Phone1 = data.COD_Phone1;
                    vendorDetails.Phone2 = data.COD_Phone2;
                    vendorDetails.PointOfContact = data.COD_PointOfContact;
                    vendorDetails.VendorEmail = data.COD_Email;
                    vendorDetails.Website = data.COD_Website;
                    vendorDetails.JobTile = data.COD_JobTitle;
                    vendorDetails.VendorId = data.COD_CMP_Id;
                    vendorDetails.CompanyDocEdit = data.CMP_CompanyDocument;
                    vendorDetails.CompanyId = data.CMP_Id;
                    vendorDetails.TaxNo = data.TXD_TaxIdNumber;
                    vendorDetails.CompanyDocuments = data.CMP_CompanyDocument == null ? "" : HostingPrefix + Path.Replace("~", "") + data.CMP_CompanyDocument;
                    //vendorDetails.COD_ID =Convert.ToInt64( data.COD_Id);

                    if (vendorDetails.Address1City == vendorDetails.Address2City
                        && vendorDetails.Address1 == vendorDetails.Address2 &&
                        vendorDetails.Address1State == vendorDetails.Address2State)
                    {
                        vendorDetails.IsAddress2Same = true;
                    }
                    else
                    {
                        vendorDetails.IsAddress2Same = false;
                    }
                }
                else
                {
                    return vendorDetails;
                }
                return vendorDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetDriverModel GetDriverDetailsById(long DriverId)", "Exception While Editing Driver.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 28-OCT-2018
        /// Created For : To save newly added account for vendor
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public VendorSetupManagementModel SaveVendorAccount(VendorSetupManagementModel Obj)
        {
            var objVendorManagement = new VendorSetupManagementModel();
            int? VendorId = 0;
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            long COT_ID = 0;
            long PrimaryMode = 0;
            if (Obj.CompanyType == 2 || Obj.CompanyType == 3)
            {
                COT_ID = Convert.ToInt64(Obj.CompanyType);
                Obj.VendorType = 6;
            }
            else
            {
                COT_ID = Convert.ToInt64(eCounting.COT_ID);
            }
            string Action = "";
            try
            {
                if (Obj.VendorId > 0)
                {
                    Action = "I";
                    if (Obj.VendorAccountDetailsModel != null)
                    {
                        var saveAccountDetails = _workorderems.spSetCompanyAccountDetail(Action, null, Obj.VendorId, Obj.VendorAccountDetailsModel.PaymentMode,
                                                                      Obj.VendorAccountDetailsModel.BankName, Obj.VendorAccountDetailsModel.BankLocation,
                                                                      Obj.VendorAccountDetailsModel.AccountNumber, Obj.VendorAccountDetailsModel.CardNumber,
                                                                      Obj.VendorAccountDetailsModel.IFSCCode, Obj.VendorAccountDetailsModel.SwiftOICCode,
                                                                      Obj.VendorAccountDetailsModel.AccountDocuments, Obj.UserId, null, "Y", Obj.VendorAccountDetailsModel.BalanceAmount, Obj.VendorAccountDetailsModel.QuickbookAcountId);
                        objVendorManagement.Result = Result.Completed;
                    }
                    var userData = _workorderems.UserRegistrations.Where(x => x.UserId == Obj.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    var VendorData = _workorderems.spGetVendorAllDetail(Obj.VendorId).FirstOrDefault();
                    var LocatioVendorData = _workorderems.spGetLocationCompanyMappingForApproval(VendorId).Select
                    (x => new LocationDataModel()
                    {
                        LocationName = x.LocationName
                    }).ToList();
                    if (userData != null && VendorData != null)
                    {
                        #region Save DAR
                        objDAR.ActivityDetails = DarMessage.AddBankAccountForVendor(Obj.CompanyNameLegal, userData.FirstName + "" + userData.LastName);
                        objDAR.TaskType = (long)TaskTypeCategory.AddBankAccount;
                        objDAR.UserId = Convert.ToInt64(Obj.VendorId);
                        objDAR.CreatedBy = Obj.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        CommonManager.SaveDAR(objDAR);
                        #endregion DAR
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel SaveVendorAccount(VendorSetupManagementModel Obj)", "Exception While adding account ddetails.", null);
                throw;
            }
            return objVendorManagement;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public VendorSetupManagementModel SaveVendorInsuranceLicense(VendorSetupManagementModel Obj)
        {
            var objVendorManagement = new VendorSetupManagementModel();
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            int? VendorId = 0;
            long COT_ID = Convert.ToInt64(eCounting.COT_ID);
            string Action = "";
            try
            {
                if (Obj.VendorId > 0)
                {
                    if (Obj.VendorInsuranceModel.LicenseId == 0 && Obj.VendorInsuranceModel.InsuranceID == 0)
                    {
                        Action = "I";
                        if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.InsuranceCarries != null)
                        {
                            var saveInsuranceLicense = _workorderems.spSetInsurance(Action, null, Obj.VendorId,
                                                                     Obj.VendorInsuranceModel.InsuranceCarries, Obj.VendorInsuranceModel.PolicyNumber,
                                                                     Obj.VendorInsuranceModel.InsuranceExpirationDate, Obj.VendorInsuranceModel.InsuranceDocument,
                                                                     Obj.UserId, null, "Y");
                        }
                        if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.LicenseName != null)
                        {
                            var saveInsuranceLicense = _workorderems.spSetLicense(Action, null, Obj.VendorId, Obj.VendorInsuranceModel.LicenseName,
                                                                     Obj.VendorInsuranceModel.LicenseNumber, Obj.VendorInsuranceModel.LicenseExpirationDate,
                                                                     Obj.VendorInsuranceModel.LicenseDocument, Obj.UserId, null, "Y");
                        }
                        var userData = _workorderems.UserRegistrations.Where(x => x.UserId == Obj.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        var VendorData = _workorderems.spGetVendorAllDetail(Obj.VendorId).FirstOrDefault();
                        var LocatioVendorData = _workorderems.spGetLocationCompanyMappingForApproval(VendorId).Select
                        (x => new LocationDataModel()
                        {
                            LocationName = x.LocationName
                        }).ToList();
                        if (userData != null && VendorData != null)
                        {
                            #region Save DAR
                            objDAR.ActivityDetails = DarMessage.AddInsuranceAndLicense(Obj.CompanyNameLegal, userData.FirstName + "" + userData.LastName);
                            objDAR.TaskType = (long)TaskTypeCategory.AddInsuranceLicese;
                            objDAR.UserId = Convert.ToInt64(Obj.VendorId);
                            objDAR.CreatedBy = Obj.UserId;
                            objDAR.CreatedOn = DateTime.UtcNow;
                            CommonManager.SaveDAR(objDAR);
                            #endregion DAR
                        }
                    }
                    else if (Obj.VendorInsuranceModel.LicenseId > 0 || Obj.VendorInsuranceModel.InsuranceID > 0)
                    {
                        Action = "U";
                        if (Obj.VendorInsuranceModel.InsuranceID > 0)
                        {
                            if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.InsuranceCarries != null)
                            {
                                var Data = _workorderems.LogInsurances.Where(x => x.LINS_INS_Id == Obj.VendorInsuranceModel.InsuranceID).FirstOrDefault();
                                var saveInsuranceLicense = _workorderems.spSetInsurance(Action, Data.LINS_Id, Obj.VendorId,
                                                                         Obj.VendorInsuranceModel.InsuranceCarries, Obj.VendorInsuranceModel.PolicyNumber,
                                                                         Obj.VendorInsuranceModel.InsuranceExpirationDate, Obj.VendorInsuranceModel.InsuranceDocument,
                                                                         Obj.UserId, Data.LINS_ApprovedBy, "Y");
                            }
                        }
                        if (Obj.VendorInsuranceModel.LicenseId > 0)
                        {
                            if (Obj.VendorInsuranceModel != null && Obj.VendorInsuranceModel.LicenseName != null)
                            {
                                var data = _workorderems.LogLicenses.Where(x => x.LLNC_LNC_Id == Obj.VendorInsuranceModel.LicenseId).FirstOrDefault();

                                var saveInsuranceLicense = _workorderems.spSetLicense(Action, data.LLNC_Id, Obj.VendorId, Obj.VendorInsuranceModel.LicenseName,
                                                                         Obj.VendorInsuranceModel.LicenseNumber, Obj.VendorInsuranceModel.LicenseExpirationDate,
                                                                         Obj.VendorInsuranceModel.LicenseDocument, Obj.UserId, data.LLNC_ApprovedBy, "Y");
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel SaveVendorAccount(VendorSetupManagementModel Obj)", "Exception While adding account ddetails.", null);
                throw;
            }
            return objVendorManagement;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 06-Dec-2018
        /// Created For : To get all Insurance List by Vendor Id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<VendorInsuranceModel> GetAllInsuranceDataList(long? VendorId, long? LocationId, bool VendorStatus, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                //var objDetails = new List<VendorInsuranceModel>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<VendorInsuranceModel>();
                if (VendorStatus == true)
                {
                    Results = _workorderems.spGetInsurance(VendorId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorInsuranceModel()
                    {
                        InsuranceID = a.INS_Id,
                        Id=Cryptography.GetEncryptedData(Convert.ToString(a.INS_Id), true),
                        VendorListId = a.INS_CMP_Id,
                        InsuranceCarries = a.INS_IncuranceCarrier,
                        InsuranceExpirationDate = a.INS_ExpirationDate,
                        DisplayLicenseExpirationDate = a.INS_ExpirationDate.ToString("MM/dd/yyyy"),
                        PolicyNumber = a.INS_PolicyNumber,
                        InsuranceDocument = a.INS_InsuranceDocument,
                        Status = a.INS_IsActive == "E" ? "Expired" : a.INS_IsActive == "N" ? "Deactivated" : "Activated"
                    }).ToList();
                }
                else
                {
                    Results = _workorderems.spGetInsurance(VendorId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorInsuranceModel()
                    {
                        InsuranceID = a.INS_Id,
                        Id = Cryptography.GetEncryptedData(Convert.ToString(a.INS_Id), true),
                        VendorListId = a.INS_CMP_Id,
                        InsuranceCarries = a.INS_IncuranceCarrier,
                        InsuranceExpirationDate = a.INS_ExpirationDate,
                        DisplayLicenseExpirationDate = a.INS_ExpirationDate.ToString("MM/dd/yyyy"),
                        PolicyNumber = a.INS_PolicyNumber,
                        InsuranceDocument = a.INS_InsuranceDocument,
                        Status = a.INS_IsActive == "E" ? "Expired" : a.INS_IsActive == "N" ? "Deactivated" : "Activated"
                    }).ToList();
                }
                
                return Results.ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 06-Dec-2018
        /// Created For : To get 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VendorInsuranceModel GetDetailsById(long Id)
        {
            try
            {
                var ObjInsuranceModel = new VendorInsuranceModel();
                if (Id > 0)
                {
                    ObjInsuranceModel = _workorderems.Insurances.Where(u => u.INS_Id == Id).
                       Select(x => new VendorInsuranceModel()
                       {
                           InsuranceDocument = x.INS_InsuranceDocument,
                       }).FirstOrDefault();
                    if (ObjInsuranceModel == null)
                    {
                        ObjInsuranceModel = _workorderems.Licenses.Where(u => u.LNC_Id == Id).
                        Select(x => new VendorInsuranceModel()
                        {
                            InsuranceDocument = x.LNC_LicenseDocument,
                        }).FirstOrDefault();
                    }
                }
                return ObjInsuranceModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorInsuranceModel GetDetailsById(long Id)", "Exception While getting Insurance/License detail by VendorId.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwaji Bansod
        /// Created Date : 07-Dec-2018
        /// Created For : To get All license list by vendor Id.
        /// </summary>
        /// <param name="VendorId"></param>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<VendorInsuranceModel> GetAllLicenseDataList(long? VendorId, long? LocationId, bool status, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objDetails = new InsuranceLicenseListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<VendorInsuranceModel>();
                if (status == true)
                {

                    var _result = _workorderems.Licenses.Where(x => x.LNC_CMP_Id == VendorId).ToList();

                    Results = _result.Select(a => new VendorInsuranceModel()
                    {
                       
                        LicenseId = a.LNC_Id,
                        Id = Cryptography.GetEncryptedData(Convert.ToString(a.LNC_Id), true),
                        LicenseName = a.LNC_LicenseName,
                        LicenseNumber = a.LNC_LicenseNumber,
                        LicenseExpirationDate = a.LNC_ExpirationDate,
                        DisplayLicenseExpirationDate= a.LNC_ExpirationDate.ToString("MM/dd/yyyy"),
                        VendorListId = a.LNC_CMP_Id,
                        LicenseDocument = a.LNC_LicenseDocument,
                        Status = a.LNC_IsActive == "E" ? "Expired" : a.LNC_IsActive == "N" ? "Deactivated" : "Activated"
                    }).ToList();//.Where(x => x.Status == "Y").
                }
                else
                {
                    Results = _workorderems.spGetLicense(VendorId).Where(x => x.Status == "N")  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorInsuranceModel()
                    {

                        LicenseId = a.LNC_Id,
                        Id = Cryptography.GetEncryptedData(Convert.ToString(a.LNC_Id), true),
                        LicenseName = a.LNC_LicenseName,
                        LicenseNumber = a.LNC_LicenseNumber,
                        LicenseExpirationDate = a.LNC_ExpirationDate,
                        DisplayLicenseExpirationDate = a.LNC_ExpirationDate.ToString("MM/dd/yyyy"),
                        VendorListId = a.LNC_CMP_Id,
                        LicenseDocument = a.LNC_LicenseDocument,
                        Status = a.Status == "E" ? "Expired" : a.Status == "N" ? "Deactivated" : "Activated"
                    }).Where(x => x.Status == "N").ToList();
                }
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //objDetails.pageindex = pageindex;
                //objDetails.total = totalPages;
                //objDetails.records = totRecords;
                //objDetails.rows = Results.ToList();
                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }
   
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Dec-2018
        /// Created For : To Activate/Deactivate Insurance/License
        /// </summary>
        /// <param name="InsuranceId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public bool ActiveInsuranceLicenseById(long InsuranceLicenseId, long UserId, string IsActive, string IsInsuranceLicense)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (InsuranceLicenseId > 0)
                {
                    if (IsInsuranceLicense == "Insurance")
                    {
                        var getDetails = _workorderems.LogInsurances.Where(x => x.LINS_INS_Id == InsuranceLicenseId)//(action, null)
                            .FirstOrDefault();
                        var Update = _workorderems.spSetInsurance(action, InsuranceLicenseId, getDetails.LINS_CMP_Id, getDetails.LINS_IncuranceCarrier,
                                                                          getDetails.LINS_PolicyNumber, getDetails.LINS_ExpirationDate,
                                                                          getDetails.LINS_InsuranceDocument, UserId, getDetails.LINS_ApprovedBy, IsActive);
                    }
                    else
                    {
                        var getDetails = _workorderems.LogLicenses.Where(x => x.LLNC_LNC_Id == InsuranceLicenseId)//(action, null)
                             .FirstOrDefault();
                        var Update = _workorderems.spSetLicense(action, InsuranceLicenseId, getDetails.LLNC_CMP_Id, getDetails.LLNC_LicenseName,
                                                                          getDetails.LLNC_LicenseNumber, getDetails.LLNC_ExpirationDate,
                                                                           getDetails.LLNC_LicenseDocument, UserId, getDetails.LLNC_ApprovedBy, IsActive);
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActivePaymentModeById(long PaymentModeId, long UserId)", "Exception While activating Payment Mode.", null);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To get all Accounts of vendor by vendor Id.
        /// </summary>
        /// <param name="VendorId"></param>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<VendorAccountDetailsModel> GetAllAccountsDataList(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objDetails = new List<VendorAccountDetailsModel>();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                  objDetails = _workorderems.spGetCompanyAccountDetail(VendorId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorAccountDetailsModel()
                    {
                        AccountID = a.CAD_Id,
                        Id= Cryptography.GetEncryptedData(Convert.ToString(a.CAD_Id), true),
                        AccountDocuments = a.CAD_AccountDocument,
                        AccountNumber = a.CAD_AccountNumber,
                        BankLocation = a.CAD_BankLocation,
                        BankName = a.CAD_CardOrBankName,
                        CardNumber = a.CAD_CreditCardNumber,
                        IFSCCode = a.CAD_IFSCcode,
                        SwiftOICCode = a.CAD_SwiftBICcode,
                        Status = a.CAD_IsActive == "E" ? "Expired" : a.CAD_IsActive == "N" ? "Deactivated" : "Activated"
                    }).ToList();

               
                return objDetails.ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorAccountDetails GetAllAccountsDataList(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all accounts.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To Activate / Deactivate accounts details by account Id.
        /// </summary>
        /// <param name="AccountsId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        public bool ActiveAccountsById(long AccountsId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (AccountsId > 0)
                {

                    var getDetails = _workorderems.LogCompanyAccountDetails.Where(x => x.LCAD_CAD_Id == AccountsId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetCompanyAccountDetail(action, AccountsId, getDetails.LCAD_CMP_Id, getDetails.LCAD_PMD_Id,
                                                                      getDetails.LCAD_CardOrBankName, getDetails.LCAD_BankLocation, getDetails.LCAD_AccountNumber,
                                                                      getDetails.LCAD_CreditCardNumber, getDetails.LCAD_IFSCcode, getDetails.LCAD_SwiftBICcode, getDetails.LCAD_AccountDocument, UserId, getDetails.LCAD_ApprovedBy, IsActive, getDetails.LCAD_Balance, null);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActivePaymentModeById(long PaymentModeId, long UserId)", "Exception While activating Payment Mode.", null);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 10-Dec-2018
        /// Created For : To get account details by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VendorAccountDetailsModel GetAccountDetailsById(long Id)
        {
            try
            {
                var ObjAccountsModel = new VendorAccountDetailsModel();
                if (Id > 0)
                {
                    ObjAccountsModel = _workorderems.CompanyAccountDetails.Where(u => u.CAD_Id == Id).
                       Select(x => new VendorAccountDetailsModel()
                       {
                           AccountDocuments = x.CAD_AccountDocument,
                       }).FirstOrDefault();
                }
                return ObjAccountsModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorAccountDetailsModel GetAccountDetailsById(long Id)", "Exception While getting Account detail by id.", null);
                throw;
            }
        }

        /// <summary>
        ///Created By : Ashwajit Bansod
        ///Creted Date : 14-Dec-2018
        ///Created For : To save Quick book Id to database.
        /// </summary>
        /// <param name="QuickBookVendorId"></param>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public bool SaveQuickBookId(string QuickBookVendorId, long? VendorId)
        {
            bool IsSavedId = false;
            try
            {
                if (QuickBookVendorId != null && VendorId > 0)
                {
                    long QBKId = Convert.ToInt64(QuickBookVendorId);
                    var SaveData = _workorderems.spSetCompanyQBK(QBKId, VendorId);
                    IsSavedId = true;
                }
                else
                {
                    IsSavedId = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveQuickBookId(string QuickBookVendorId, long VendorId)", "Exception While Savign Quickbook Id.", null);
                throw;
            }
            return IsSavedId;
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 07-Jan-2019
        /// Created For : To get company Id to compare with quickbook Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public long GetCompanyInfo(long Id)
        {
            long getId = 0;
            try
            {
                if (Id > 0)
                {
                    var getData = _workorderems.CompanyQBKs.Where(x => x.QBK_RefId == Id).FirstOrDefault();
                    if (getData == null)
                    {
                        getId = 0;
                    }
                    else
                    {
                        getId = getData.QBK_RefId;
                    }
                }
                else
                {
                    getId = 0;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public long GetCompanyInfo(long Id)", "Exception While getting data.", null);
                throw;
            }
            return getId;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-Jan-2018
        /// Created For : To get comppany data by Company id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public VendorSetupManagementModel GetCompanyDetails(long Id)
        {
            var model = new VendorSetupManagementModel();
            try
            {
                if (Id > 0)
                {
                    model = _workorderems.spGetCompanyDetailEdit(Id).Select(
                        x => new VendorSetupManagementModel()
                        {
                            CompanyType = x.CMP_COT_Id
                        }).FirstOrDefault();
                }
                else
                {
                    model = null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel GetCompanyDetails(long Id)", "Exception While getting data.", null);
                throw;
            }
            return model;
        }

        /// <summary>
        /// Created Date : 09-Jan-2019
        /// Created By : Ashwajit Bansod
        /// Created For : To get quickbook Id of Vendor
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public long GetCompanyQuickBookId(long Id)
        {
            long getId = 0;
            try
            {
                if (Id > 0)
                {
                    var getData = _workorderems.CompanyQBKs.Where(x => x.QBK_CMP_Id == Id).FirstOrDefault();
                    if (getData == null)
                    {
                        getId = 0;
                    }
                    else
                    {
                        getId = getData.QBK_RefId;
                    }
                }
                else
                {
                    getId = 0;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public long GetCompanyQuickBookId(long Id)", "Exception While getting quick book id.", null);
                throw;
            }
            return getId;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-April-2019
        /// Created For : To get company Id by Company Name
        /// </summary>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        public long GetVendorId(string CompanyName)
        {
            try
            {
                var getData = _workorderems.Companies.Where(x => x.CMP_NameLegal == CompanyName && x.CMP_IsActive == "Y").FirstOrDefault();
                if (getData != null)
                {
                    return getData.CMP_Id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public long GetVendorId(string CompanyName)", "Exception While getting Vendor id.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 10-Jan-2018
        /// Created For : To get QuickBook Account Id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public VendorAccountDetailsModel GetAccountDetailsByVendorId(long CadId)
        {
            try
            {
                var ObjAccountsModel = new VendorAccountDetailsModel();
                if (CadId > 0)
                {
                    ObjAccountsModel = _workorderems.CompanyAccountDetails.Where(u => u.CAD_Id == CadId).
                       Select(x => new VendorAccountDetailsModel()
                       {
                           QuickbookAcountId = x.CAD_QBKId
                       }).FirstOrDefault();
                }
                return ObjAccountsModel;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorAccountDetailsModel GetAccountDetailsByVendorId(long VendorId)", "Exception While getting Account detail by vendor id.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25 Feb 2019
        /// Created For : To get License and Insurance details by using id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modelStatus"></param>
        /// <returns></returns>
        public VendorInsuranceModel GetInsuranceLicenseCompanyDetails(long Id, string modelStatus)
        {
            var model = new VendorInsuranceModel();
            try
            {
                if (Id > 0 && modelStatus == "License")
                {
                    model = _workorderems.Licenses.Where(x => x.LNC_Id == Id && x.LNC_IsActive == "Y").Select(
                        x => new VendorInsuranceModel()
                        {
                            LicenseId = x.LNC_Id,
                            LicenseDocument = x.LNC_LicenseDocument,
                            LicenseExpirationDate = x.LNC_ExpirationDate,
                            LicenseNumber = x.LNC_LicenseNumber,
                            LicenseName = x.LNC_LicenseName,
                        }).FirstOrDefault();
                }
                else
                {
                    model = _workorderems.Insurances.Where(x => x.INS_Id == Id && x.INS_IsActive == "Y").Select(
                        x => new VendorInsuranceModel()
                        {
                            InsuranceCarries = x.INS_IncuranceCarrier,
                            InsuranceDocument = x.INS_InsuranceDocument,
                            InsuranceExpirationDate = x.INS_ExpirationDate,
                            InsuranceID = x.INS_Id,
                            PolicyNumber = x.INS_PolicyNumber
                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorSetupManagementModel GetInsuranceLicenseCompanyDetails(long Id, string modelStatus)", "Exception While getting data.", null);
                throw;
            }
            return model;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 05-March-2018
        /// Created For : To get all facility of vendor as per vendor Id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <param name="LocationId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public List<VendorFacilityModel> GetFacilityListCompanyDetails(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
               var objDetails = new List<VendorFacilityModel>();
                
                objDetails = _workorderems.spGetCompanyFacilityMapping(LocationId, VendorId)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                    .Select(a => new VendorFacilityModel()
                    {
                        Costcode = a.CFM_CCD_CostCode,
                        FacilityId = a.CFM_Id,
                        Tax = a.CFM_Tax,
                        UnitCost = a.CFM_Rate,
                        ProductServiceName = a.CFM_Discription,
                        VendorId = a.CFM_CMP_Id,
                        ProductServiceType = a.CFM_FacilityType == "1" ? "Product" : "Services",
                        Amount = a.BCM_BalanceAmount
                    }).ToList();

                
                 
                return objDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyFacilityModelDetails GetFacilityListCompanyDetails(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all accounts.", VendorId);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 07-March-2019
        /// Crated For : To save facility for vendor
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SaveFacilityDetails(VendorFacilityModel obj)
        {
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            long COT_ID = Convert.ToInt64(eCounting.COT_ID);
            string Action = "";
            bool IsSaved = false;
            try
            {
                if (obj != null && obj.VendorId > 0)
                {

                    Action = "I";
                    var saveVendorFacility = _workorderems.spSetCompanyFacilityMapping(Action, null, obj.VendorId, obj.Costcode,
                                                                         obj.ProductServiceType, obj.ProductServiceName, obj.UnitCost, obj.Tax, obj.UserId,
                                                                         obj.UserId, "Y");
                    IsSaved = true;
                    var userData = _workorderems.UserRegistrations.Where(x => x.UserId == obj.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    var VendorData = _workorderems.spGetVendorAllDetail(obj.VendorId).FirstOrDefault();
                    var LocatioVendorData = _workorderems.spGetLocationCompanyMappingForApproval(obj.VendorId).Select
                    (x => new LocationDataModel()
                    {
                        LocationName = x.LocationName
                    }).ToList();
                    if (userData != null && VendorData != null)
                    {
                        #region Save DAR
                        objDAR.ActivityDetails = DarMessage.AddCompanyFacility(obj.VendorName, userData.FirstName + "" + userData.LastName);
                        objDAR.TaskType = (long)TaskTypeCategory.AddFacility;
                        objDAR.UserId = Convert.ToInt64(obj.VendorId);
                        objDAR.CreatedBy = obj.UserId;
                        objDAR.CreatedOn = DateTime.UtcNow;
                        CommonManager.SaveDAR(objDAR);
                        #endregion DAR
                    }
                }
                else
                {
                    IsSaved = false;
                    return IsSaved;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveFacilityDetails(VendorFacilityModel obj)", "Exception While adding facility details.", obj);
                throw;
            }
            return true;
        }

        #region "Ajay Kumar"
        /// <summary>
        /// Created By : Ajay Kumar
        /// Created Date : 19-Sep-2019
        /// Crated For : To check duplicate TXD_TaxIdNumber  for vendor
        /// </summary>
        /// <param name="taxNumber"></param>
        /// <returns></returns>
        public bool TaxNumberIsExists(string taxNumber,long VendorId)
        {
            bool result = false;
            if (VendorId > 0)
            {
                var status = _workorderems.TaxDetails.Any(u => u.TXD_TaxIdNumber.ToLower() == taxNumber.Trim().ToLower() && u.TXD_Id != VendorId);
                result = status == true ? result = true : result = false;
            }
            else
            {
                var status = _workorderems.TaxDetails.Any(u => u.TXD_TaxIdNumber.ToLower() == taxNumber.Trim().ToLower());
                result = status == true ? result = false : result = true;
            }
           
            return result;
        }

        /// <summary>
        /// Created By : Ajay Kumar
        /// Created Date : 19-Sep-2019
        /// Crated For : To check duplicate INS_PolicyNumber  for vendor
        /// </summary>
        /// <param name="InsPolicyNumber"></param>
        /// <returns></returns>
        public bool InsPolicyNumberIsExists(string InsPolicyNumber)
        {
            bool result = false;
            var status = _workorderems.Insurances.Any(u => u.INS_PolicyNumber.ToLower() == InsPolicyNumber.Trim().ToLower());
            result = status == true ? result = false : result = true;
            return result;
        }
        #endregion
    }
}
