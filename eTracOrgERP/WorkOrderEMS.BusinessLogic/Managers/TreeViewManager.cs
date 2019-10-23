using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class TreeViewManager : ITreeViewManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        /// <summary>
        /// Crated By : Ashwajit Bansod
        /// Created Date : Aug-19-2018
        /// Created For : To Fetch Cost Code Master List  
        /// </summary>
        /// <returns></returns>
        public List<TreeViewModel> ListTreeViewCostCode(long LocationId)
        {
            var records = new List<TreeViewModel>();
            try
            {

                string action = "M";
                var Results = _workorderems.spGetCostCodeLocationMapping(action, LocationId ,null)
                    .Select(l => new TreeViewModel//_workorderems.spGetCostCode(action, null).Select(l => new TreeViewModel
                    {
                        id = l.CCM_CostCode,
                        name = l.CCM_Description,
                        @checked = false
                    }).ToList();
                records = Results
                   .Select(l => new TreeViewModel
                   {
                       id = l.id,
                       name = l.name,
                       @checked = false,
                       item = GetChildrenCostCode(Results, l.id, LocationId)
                   }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<TreeViewModel> ListTreeViewCostCode()", "Exception While Listing UnAssigned Work Order.", null);
                throw;
            }
            return records;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-19-2018
        /// Created For : TO fetch Cost code as per master cost code Id.
        /// </summary>
        /// <param name="ChildCostCode"></param>
        /// <param name="MasteCostCodeId"></param>
        /// <returns></returns>
        private List<TreeViewModel> GetChildrenCostCode(List<TreeViewModel> ChildCostCode, long? MasteCostCodeId, long LocationId)
        {
            string action = "C";
            return ChildCostCode = _workorderems.spGetSubCostCodeLocationMapping(action, LocationId, MasteCostCodeId). //_workorderems.spGetCostCode(action, MasteCostCodeId).Select(l => new TreeViewModel
            Select(l => new TreeViewModel
            {
                id = l.CCD_CostCode,
                name = l.CCD_Description,
                @checked = false
                //Children = GetChildrenCostCode(ChildCostCode, l.CostCode)
            }).ToList();
            //return ChildCostCode.Where(l => l. == parentId).OrderBy(l => l.OrderNumber)
            //    .Select(l => new TreeViewModel
            //    {
            //        id = l.ID,
            //        text = l.Name,
            //        population = l.Population,
            //        flagUrl = l.FlagUrl,
            //        @checked = l.Checked,
            //        children = GetChildren(locations, l.ID)
            //    }).ToList();
        }
        public bool SaveCostCodeIds(List<long> CostCodeIds, long LocationId, long UserId)
        {
            bool IsSaved = false;
            string action = "I";
            string result = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            List<string> CostCodedata = new List<string>();
            try
            {
                if (CostCodeIds.Count > 0)
                {
                    foreach (var item in CostCodeIds)
                    {
                        var ChildCostCode = _workorderems.CostCodes.Where(x => x.CCD_CostCode == item).FirstOrDefault();                        
                        if (ChildCostCode == null)
                        {
                        }
                        else
                        {
                            var saveCostCodeId = _workorderems.spSetCostCodeLocationMapping(action, LocationId, ChildCostCode.CCD_CCM_CostCode, item);
                            IsSaved = true;
                            CostCodedata.Add(ChildCostCode.CCD_Description);
                        }
                    }
                }
                var locationName = _workorderems.LocationMasters.Where(x => x.LocationId == LocationId && x.IsDeleted == false).FirstOrDefault();
                result = string.Join(",", CostCodedata.ToArray());
                #region Save DAR
                objDAR.ActivityDetails = DarMessage.AllocateCostCodeForLocation(locationName.LocationName, result);
                objDAR.TaskType = (long)TaskTypeCategory.AllocateCostcode;
                objDAR.UserId = UserId;
                objDAR.CreatedBy = UserId;
                objDAR.LocationId = LocationId;
                objDAR.DeletedOn = DateTime.UtcNow;
                CommonManager.SaveDAR(objDAR);
                #endregion DAR
        }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCostCodeIds(List<long> CostCodeIds, long LocationId)", "Exception While Saving Cost Code to database", LocationId);
                throw;
            }
            return IsSaved;
        }
    }
}
