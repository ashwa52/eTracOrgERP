using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface ICostCode
    {
        CostCodeModel SaveCostCode(CostCodeModel objCostCodeModel);
        CostCodeDetails GetListCostCode(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        SubCostCodeDetails GetListOfSubCostCode(long _CostCodeId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType);
        bool ActiveCostCodeById(long CostCodeId, long UserId, string IsActive);
        List<string> GetCategoryList();
        List<string> GetSubCategoryList(string CategoryName);
    }
}
