using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IPOTypeDetails
    {
        List<VendorSetupManagementModel> GetCompany_VendorList(long Location, bool IsRecurring);
        POTypeServiceModel AllPOTypeList(ServiceBaseModel objServiceBaseModel);
        POTypeDetails GetPOTypeDetailsOfCompanyFacilityList(long? UserId, long? LocationID, long? VendorId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        string PONumberData();
        bool SavePODetails(POTypeDataModel objPOTypeDataModel, List<GridDataPO> obj, List<QuestionAnswerModel> objQuestioAsnwerList, bool IsManager);
        //bool SavePODetails(POTypeDataModel objPOTypeDataModel, List<GridDataPO> obj, List<QuestionAnswerModel> objQuestioAsnwerList);
        CompanyFacilityListServiceModel GetCompanyFacilityByVendoeId(POCommonServiceModel objPOCommonServiceModel);
        List<VendorSetupManagementModel> GetCompanyDetailsListByCompanyId(long VendorId);
        ServiceResponseModel<List<QuestionsEmergencyPO>> GetQuestionList(POCommonServiceModel objPOCommonServiceModel);
        List<POTyeDetailsModelService> POTypeList();
        POListDetails GetAllPOList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        //POListDetails GetAllPOList(long? UserId, long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        string ApprovePOByPOId(POApproveRejectModel objPOApproveRejectModel, POListModel objListData);
        List<POTypeDataModel> GetAllPOFacilityByPOIdList(long? UserId, long? POId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        POTypeDataModel GetPODetailsById(long POId);
        POTypeDetails GetPOFacilityListForEditByPOId(long? UserId, long? POId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        ServiceResponseModel<List<POListModel>> GetAllPOListForMobile(POCommonServiceModel objPOCommonServiceModel);
        string SendMailToManagerForBudget(long UserId, decimal CalculateRemainingAmt, long CostCodeData, long LocationData, long Vendordata);
        POListDetails GetAllSelfPOList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        PODetail GetApprovalResponseToSaveQBKId(POApproveRejectModel objPOApproveRejectModel, POListModel objListData);
    }
}
