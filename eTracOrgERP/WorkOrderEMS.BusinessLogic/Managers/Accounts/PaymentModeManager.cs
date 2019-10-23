using System;
using System.Linq;
using WorkOrderEMS.BusinessLogic.Interfaces.Accounts;
using WorkOrderEMS.Data.DataRepository.AdminSection;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models.AccountModels;

namespace WorkOrderEMS.BusinessLogic.Managers.Accounts
{
    public class PaymentModeManager : IPaymentMode
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
        public PaymentModeDetails GetPaymentModeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objPaymentModeDetails = new PaymentModeDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetPaymentMode()
                    .Select(a => new PaymentModeModel()
                    {
                        PMD_Id = a.PMD_Id,
                        PMD_PaymentMode = a.PMD_PaymentMode,
                        PMD_IsActive = a.PMD_IsActive
                    }).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objPaymentModeDetails.pageindex = pageindex;
                objPaymentModeDetails.total = totalPages;
                objPaymentModeDetails.records = totRecords;
                objPaymentModeDetails.rows = Results.ToList();
                return objPaymentModeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PaymentModeDetails GetPaymentModeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of payment mode.", null);
                throw;
            }
        }

        public PaymentModeModel SavePaymentMode(PaymentModeModel objPaymentModeModel)
        {
            var objPaymentModeRepository = new PaymentModeRepository();
            var objPaymentMode = new PaymentMode();
            string action = "";
            try
            {
                if (objPaymentModeModel.PMD_Id == 0)
                {
                    action = "I";
                    var savePaymentMode = _workorderems.spSetPymentMode(action, null, objPaymentModeModel.PMD_PaymentMode, objPaymentModeModel.PMD_IsActive);
                }
                else
                {
                    action = "U";
                    var savePaymentMode = _workorderems.spSetPymentMode(action, objPaymentModeModel.PMD_Id, objPaymentMode.PMD_PaymentMode, objPaymentMode.PMD_IsActive);
                }


                objPaymentModeModel.Result = Result.Completed;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PaymentModeDetails SavPaymentModeList(String Action, int? PMD_Id, string PaymentMode, bool IsActive)", "Exception While Saving payment mode.", null);
                throw;
            }

            return objPaymentModeModel;
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 19-Nov-2018
        /// Created For : To activate Payment mode by payment mode Id.
        /// </summary>
        /// <param name="PaymentModeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActivePaymentModeById(long PaymentModeId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (PaymentModeId > 0)
                {
                    var getDetails = _workorderems.PaymentModes.Where(x => x.PMD_Id == PaymentModeId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetPymentMode(action, PaymentModeId, getDetails.PMD_PaymentMode,
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
