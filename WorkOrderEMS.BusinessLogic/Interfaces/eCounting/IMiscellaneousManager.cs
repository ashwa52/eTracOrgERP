using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IMiscellaneousManager
    {
        ServiceResponseModel<List<LocationServiceModel>> GetLocationAssignedListByUserId(ServiceBaseModel ObjServiceBaseModel, bool IsManager);
        ServiceResponseModel<List<MiscellaneousModel>> GetMiscellaneousList(ServiceBaseModel ObjServiceBaseModel);
        ServiceResponseModel<miscellaneousNumberModel> GetMiscellaneousNumberData(ServiceBaseModel ObjServiceBaseModel);
        ServiceResponseModel<MiscellaneousDetails> SaveMiscellaneous(MiscellaneousDetails Obj);
        MiscellaneousListDetails GetListMiscellaneous(long? UserId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        MiscellaneousListDetails GetListMiscellaneousByMiscId(long? UserId, long? MiscId, long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        bool ApproveMiscellaneous(List<MiscellaneousListModel> Obj, string UserName, long UserId, long LocationId, long MiscQbkId, long VendorDetailsId);
        //bool ApproveMiscellaneous(List<MiscellaneousListModel> Obj, string UserName, long UserId, long LocationId, long MiscQbkId);
        ServiceResponseModel<List<LocationServiceModel>> GetLocationList(ServiceBaseModel ObjServiceBaseModel);
        ServiceResponseModel<List<MiscellaneousListModel>> GetAllMiscellaneousListForManager(MiscellaneousServiceModel ObjServiceBaseModel);
        ServiceResponseModel<List<MiscellaneousListModel>> GetAllChildeMiscellaneousListForManagerByMiscId(MiscellaneousServiceModel ObjServiceBaseModel);
        MiscellaneousDetailsmodel MiscellaneoousDataById(long MISNumber);
    }
}
