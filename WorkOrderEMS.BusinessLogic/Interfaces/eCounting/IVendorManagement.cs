using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IVendorManagement
    {
        List<VendorTypeData> ListVendorType();
        List<PaymentModeList> PaymentModeList();
        List<PaymentTermList> PaymentTermList();
        List<CostCodeListData> ListAllCostCode();
        VendorSetupManagementModel ProcessVendorSetup(VendorSetupManagementModel Obj);
        CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        VendorAllViewDataModel GetAllVendorData(long VendorId, string Status = null);
        string ApproveVendorByVendorId(ApproveRejectVendorModel ObjApproveRejectVendorModel);
        VendorSetupManagementModel GetVendorDetailsByVendorId(long VendorId);
        VendorSetupManagementModel SaveVendorAccount(VendorSetupManagementModel Obj);
        VendorSetupManagementModel SaveVendorInsuranceLicense(VendorSetupManagementModel Obj);
        List<LocationListServiceModel> ListAllocatedLocatioForVender(long VendorId);
        List<VendorInsuranceModel> GetAllInsuranceDataList(long? VendorId, long? LocationId, bool VendorStatus, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        //InsuranceLicenseListDetails GetAllInsuranceDataList(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        VendorInsuranceModel GetDetailsById(long Id);
        List<VendorInsuranceModel> GetAllLicenseDataList(long? VendorId, long? LocationId, bool status, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        //InsuranceLicenseListDetails GetAllLicenseDataList(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        bool ActiveInsuranceLicenseById(long InsuranceId, long UserId, string IsActive, string IsInsuranceLicense);
        List<VendorAccountDetailsModel> GetAllAccountsDataList(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);

        bool ActiveAccountsById(long AccountsId, long UserId, string IsActive);
        VendorAccountDetailsModel GetAccountDetailsById(long Id);

        bool SaveQuickBookId(string QuickBookVendorId, long? VendorId);

        long GetCompanyInfo(long Id);

        VendorSetupManagementModel GetCompanyDetails(long Id);
        long GetCompanyQuickBookId(long Id);

        VendorAccountDetailsModel GetAccountDetailsByVendorId(long VendorId);
        VendorInsuranceModel GetInsuranceLicenseCompanyDetails(long Id, string modelStatus);
        string ListAllAlocatedLocatioForVender(long VendorId);
        List<VendorFacilityModel> GetFacilityListCompanyDetails(long? VendorId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        bool SaveFacilityDetails(VendorFacilityModel obj);

        long GetVendorId(string CompanyName);
        bool TaxNumberIsExists(string taxNumber, long VendorId);
        VendorCompanyContractDocument GetCompany_ContractDocument(long VendorId);
        bool InsPolicyNumberIsExists(string InsPolicyNumber);
        IList<VendorSetupManagementModel> GetAllCompanyDataList1(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        bool SaveContractAllocation(ContractLocationAllocation obj);
        CompanyCountForGraph GetCompanyCountForGraph();
        List<LocationAllocationCompanyCountForGraph> GetCompanyAllocationLocationCountForGraph();
      void  SetPrimaryAccount(long VendorId, long AccountId);
        /// <summary>
        /// Created By : Ajay Kumar
        /// Created Date : 10-Oct-2019
        /// Crated For : To check duplicate Point Of Contact  for vendor
        /// </summary>
        /// <param name="taxNumber"></param>
        /// <returns></returns>
         bool PointOfContactIsExists(string txtPointOfContact, long VendorId);
    }
}
