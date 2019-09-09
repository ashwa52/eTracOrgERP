using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class CostCodeManager : ICostCode
    {
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July 18 2018
        /// Created For : To save Cost code to database.
        /// </summary>
        /// <param name="objCostCodeModel"></param>
        /// <returns></returns>
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public CostCodeModel SaveCostCode(CostCodeModel objCostCodeModel)
        {
            var objCostCodeRepository = new CostCodeRepository();
            var objCostCode = new CostCode();
            string action = "";
            if (objCostCodeModel.CostCodeId == 0 && objCostCodeModel.CostCode == null)
            {
                 action = "I";
                var saveMasterCostCode = _workorderems.spSetCostCodeMaster(action, objCostCodeModel.QuickBookCostCodeMasterId, objCostCodeModel.CostCode,
                                                                          objCostCodeModel.Description, objCostCodeModel.ModifiedBy,
                                                                          objCostCodeModel.ApprovedBy, objCostCodeModel.IsActive);
                objCostCodeModel.Result = Result.Completed;
            }
            else if(objCostCodeModel.CostCode != null)
            {
                action = "I";
                var saveMasterCostCode = _workorderems.spSetCostCode(action, objCostCodeModel.QuickBookCostCodeId, null, objCostCodeModel.CostCode,
                                                                          objCostCodeModel.Description,null, objCostCodeModel.ModifiedBy,
                                                                          objCostCodeModel.ApprovedBy, objCostCodeModel.IsActive);
                objCostCodeModel.Result = Result.Completed;
            }
            else
            {
                action = "U";
                var saveMasterCostCode = _workorderems.spSetCostCodeMaster(action,null, objCostCodeModel.CostCode,
                                                                          objCostCodeModel.Description, objCostCodeModel.ModifiedBy,
                                                                          objCostCodeModel.ApprovedBy, objCostCodeModel.IsActive);
                objCostCodeModel.Result = Result.UpdatedSuccessfully;
            }
            return objCostCodeModel;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July 18 2018
        /// Created For : To fetch cost code data from costcode table to display cost code list.
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public CostCodeDetails GetListCostCode(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var objCostCodeDetails = new CostCodeDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                string action = "M";
                var Results = _workorderems.spGetCostCode(action, null)
                    .Select(a => new CostCodeModel()
                    {
                        CostCode = a.CCM_CostCode,
                        Description = a.CCM_Description,
                        IsActive = a.CCM_IsActive
                    }).ToList();                
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);                
                //Results = Results.OrderByDescending(s => s.CostCode);
                objCostCodeDetails.pageindex = pageindex;
                objCostCodeDetails.total = totalPages;
                objCostCodeDetails.records = totRecords;
                objCostCodeDetails.rows = Results.ToList();
                return objCostCodeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CostCodeDetails GetListCostCode(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Cost Code.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : July-23-2018
        /// Created For : To fetch Sub Cost Code as per cost code Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public SubCostCodeDetails GetListOfSubCostCode(long CCM_CostCode, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objCostCodeDetails = new SubCostCodeDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                string action = "C";
                var Results = _workorderems.spGetSubCostCode(action, CCM_CostCode)
                    .Select(a => new SubCostCodeModel()
                    {
                        CCM_CostCode = a.CCD_CostCode,
                        Description = a.CCD_Description,
                        IsActive = a.CCD_IsActive
                    }).ToList<SubCostCodeModel>();               
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);               
               // Results = Results.OrderByDescending(s => s.CostCode);
                objCostCodeDetails.pageindex = pageindex;
                objCostCodeDetails.total = totalPages;
                objCostCodeDetails.records = totRecords;
                objCostCodeDetails.rows = Results.ToList();
                return objCostCodeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CostCodeDetails GetListCostCode(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Cost Code.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Nov-2018
        /// Created For : To Active Cost code as per cost code Id.
        /// </summary>
        /// <param name="CostCodeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActiveCostCodeById(long CostCodeId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (CostCodeId > 0)
                {
                    var getCostCodeDetails = _workorderems.CostCodes.Where(x => x.CCD_CostCode == CostCodeId)//(action, null)
                        .FirstOrDefault();
                    var updateMasterCostCode = _workorderems.spSetCostCode(action,getCostCodeDetails.CCD_QBKId, CostCodeId, getCostCodeDetails.CCD_CCM_CostCode,
                                                                              getCostCodeDetails.CCD_Description,null, UserId,
                                                                              null, IsActive);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActiveCostCodeById(long CostCodeId, long UserId)", "Exception While activating Cost Code.", null);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Created By  : Ashwajit Bansod
        /// Created  date : 07-Jan-2018
        /// Created For : To get all category List 
        /// </summary>
        /// <returns></returns>
        public List<string> GetCategoryList()
        {
            var objModel = new List<string>();
            var Action = "ACC";
            try
            {
                var data = _workorderems.spGetAccountCategory(Action, null);
                objModel = data.ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CostCodeModel GetCategoryList()", "Exception While getting the list of category.", null);
                throw;
            }
            return objModel;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-Jan-2018
        /// Created For : To get sub category by category name
        /// </summary>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public List<string> GetSubCategoryList(string CategoryName)
        {
            var objModel = new List<string>();
            var Action = "ADC";
            try
            {
                var data = _workorderems.spGetAccountCategory(Action, CategoryName);
                objModel = data.ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<string> GetSubCategoryList(string CategoryName)", "Exception While getting the list of sub category.", CategoryName);
                throw;
            }
            return objModel;
        }
    }
}
