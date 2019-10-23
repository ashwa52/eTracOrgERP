using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IClientManager
    {
        UserModel GetClientById(long userId, string operationName, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, string textSearch);
        Result SaveClient(UserModel objUserModel, out long qrcId, bool isManagerRegistration, DARModel objDARModel);
        Result SaveClient(UserModel objUserModel, out long qrcId, bool isManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action);
        Result CancelWorkOrderByEmployee(long WorOrderID, long iUserId);
        UserModelList GetClientByLocation(long LocationID);
        Result SaveClientNewUserRegistrationforAll(UserModel objUserModel, out long qrcId, bool isManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action);
        Result SaveNewUserRegistration(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, long createdBy, string action);
        Result SaveAdminNewUserRegistration(UserModel objUserModel);
        Result SaveNewClient(UserModel objUserModel, out long qrcId, bool IsManagerRegistration, DARModel objDARModel, long locationId, long createdBy, string action);
        UserModelDetails GetClientList(long? UserId, long? LocationId,  int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        VendorSetupManagementModel GetCompanyDetails(long UserId);
        List<LocationDataModel> GetLocationIds(long? VendorId);
    }
}
