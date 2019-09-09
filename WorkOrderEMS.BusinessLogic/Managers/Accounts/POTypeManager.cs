using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.Accounts;
using WorkOrderEMS.Data.DataRepository.AdminSection;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Managers.Accounts
{
    public class POTypeManager : IPOType
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : August 25 2018
        /// Created For : List for payment mode.
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
        public POTypeModelDetails GetPOTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objPOTypeModelDetails = new POTypeModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetPOType()
                    .Select(a => new POTypeModel()
                    {
                        POT_Id = a.POT_Id,
                        POT_POName = a.POT_POType,
                        POT_IsActive = a.POT_IsActive
                    }).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objPOTypeModelDetails.pageindex = pageindex;
                objPOTypeModelDetails.total = totalPages;
                objPOTypeModelDetails.records = totRecords;
                objPOTypeModelDetails.rows = Results.ToList();
                return objPOTypeModelDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }

        public POTypeModel SavePOType(POTypeModel objPOTypeModel)
        {

            string action = "";
            try
            {
                if (objPOTypeModel.POT_Id == 0)
                {
                    action = "I";
                    var savePOType = _workorderems.spSetPOType(action, null, objPOTypeModel.POT_POName, objPOTypeModel.POT_IsActive);
                }
                else
                {
                    action = "U";
                    var savePOType = _workorderems.spSetPOType(action, objPOTypeModel.POT_Id, objPOTypeModel.POT_POName, objPOTypeModel.POT_IsActive);
                }

                objPOTypeModel.Result = Result.Completed;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POTypeModel SavePOType(String Action, int? POT_Id, string POT_POName, bool IsActive)", "Exception While Saving PO Type.", null);
                throw;
            }
            return objPOTypeModel;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Nov-2018
        /// Created For : To activte PO type by PO Type Id.
        /// </summary>
        /// <param name="POTypeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActivePOTypeById(long POTypeId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (POTypeId > 0)
                {
                    var getDetails = _workorderems.POTypes.Where(x => x.POT_Id == POTypeId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetPOType(action, POTypeId, getDetails.POT_POType,
                                                                               IsActive);
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
    }
}
