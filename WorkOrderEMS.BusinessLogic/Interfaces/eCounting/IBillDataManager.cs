using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces.eCounting
{
    public interface IBillDataManager
    {
        bool SaveBillDetails(BillDataServiceModel objBillDataServiceModel);
        //BillListApproveDetails GetListBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        string ApproveBill(BillListApproveModel Obj, string UserName, long UserId, long LocationId);
        CostCodeModel GetCostCodeData(long CostCodeId);
        ServiceResponseModel<BillNumberData> GetPreBillNumberData(ServiceBaseModel ObjServiceBaseModel);
        BillListApproveDetails GetListPreBill(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        List<BillFacilityModel> GetAllBillFacilityListById(long BillId);
        LocationDataModel GetLocationDataByLocId(long LocationId);
        FacilityListData GetFacilityDataByFacilityId(string FacilityId);
        long GetBillQBKId(long BillId);
    }
}
