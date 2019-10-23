using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IBudgetLocationManager
    {
        BudgetDetails GetListBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        bool SaveBudgetAmount(decimal BudgetAmount, long LocationId, int BudgetYear);
        bool SaveAllGridBudgetData(List<BudgetForLocationModel> obj, long LocationId, int Year);
        List<BudgetForLocationModel> ListOfCostCode(long LocationId, decimal BudgetAmount, long CLM_Id);
        BudgetDetails GetListOfTransferCostCodeForLocationDetails(long? LocationId, decimal RemainingAmt, long CLMId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        bool SaveTransferAmount(BudgetForLocationModel objBudgetForLocationModel);
        // bool SaveTransferAmount(decimal BudgetAmount, long Location, int Year, int? BCM_Id, int? CLM_Id, int? BCM_CLM_TransferId, int? AssignedPercent, string BudgetSource);
        TranferAmountForCostCodeModel GetCostCodeDetailsByCostCodeId(long CostCodeId, long LocationId);
        BudgetDetails GetListOfCostCodeAfterTransferBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        string GetLocationName(long LocationId);
    }
}
