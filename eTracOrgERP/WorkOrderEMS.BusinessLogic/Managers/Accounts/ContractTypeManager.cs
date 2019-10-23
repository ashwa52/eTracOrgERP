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
    public class ContractTypeManager : IContractType
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
        public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objContractTypeDetails = new ContractTypeDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetContractType()
                    .Select(a => new ContractTypeModel()
                    {
                        CTT_Id = a.CTT_Id,
                        CTT_ContractType = a.CTT_ContractType,
                        CTT_Discription = a.CTT_Discription,
                        CTT_IsActive =a.CTT_IsActive
                    }).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objContractTypeDetails.pageindex = pageindex;
                objContractTypeDetails.total = totalPages;
                objContractTypeDetails.records = totRecords;
                objContractTypeDetails.rows = Results.ToList();
                return objContractTypeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }

        public ContractTypeModel SaveContractType(ContractTypeModel objContractTypeModel)
        {
            var objContractTypeRepository = new ContractTypeRepository();
            var objContractType = new ContractType();
            string action = "";
            try
            {
                if (objContractTypeModel.CTT_Id == 0)
                {
                    action = "I";
                    var saveContractType = _workorderems.spSetContractType(action, null, objContractTypeModel.CTT_ContractType, objContractTypeModel.CTT_Discription, objContractTypeModel.CTT_IsActive);
                }
                else
                {
                    action = "U";
                    var saveContractType = _workorderems.spSetContractType(action, objContractTypeModel.CTT_Id, objContractTypeModel.CTT_ContractType, objContractTypeModel.CTT_Discription, objContractTypeModel.CTT_IsActive);
                }


                objContractTypeModel.Result = Result.Completed;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeModel SaveContractType(String Action, int? CTT_Id, string CTT_ContractType, string CTT_Discription, bool IsActive)", "Exception While Saving Contract Type.", null);
                throw;
            }
            return objContractTypeModel;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Nov-2019
        /// Created For : To Activate 
        /// </summary>
        /// <param name="ContractTypeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActiveContractTypeById(long ContractTypeId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (ContractTypeId > 0)
                {
                    var getDetails = _workorderems.ContractTypes.Where(x => x.CTT_Id == ContractTypeId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetContractType(action, ContractTypeId, getDetails.CTT_ContractType,
                                                                             getDetails.CTT_Discription, IsActive);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActiveContractTypeById(long ContractTypeId, long UserId)", "Exception While activating contract type.", null);
                throw;
            }
            return result;
        }
    }
}
