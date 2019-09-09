using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository.AdminSection;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class PaymentTermsManager : IPaymentTerms
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Bhushan Dod
        /// Created Date : August 25 2018
        /// Created For : List for payment terms.
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
        public PaymentTermsDetails GetPaymentTermsList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objPaymentTermsDetails = new PaymentTermsDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetPaymentTerm()
                    .Select(a => new PaymentTermsModel()
                    {
                        PTM_Id = a.PTM_Id,
                        PTM_Term = a.PTM_Term,
                        PTM_GracePeriod = a.PTM_GracePeriod,
                        PTM_IsActive = a.PTM_IsActive
                    }).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objPaymentTermsDetails.pageindex = pageindex;
                objPaymentTermsDetails.total = totalPages;
                objPaymentTermsDetails.records = totRecords;
                objPaymentTermsDetails.rows = Results.ToList();
                return objPaymentTermsDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CostCodeDetails GetListCostCode(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Cost Code.", null);
                throw;
            }
        }

        public PaymentTermsModel SavePaymentTerms(PaymentTermsModel objPaymentTermsModel)
        {
            var objPaymentTermRepository = new PaymentTermsRepository();
            var objPaymentTerm = new PaymentTerm();
            string action = "";
            try
            {
                if (objPaymentTermsModel.PTM_Id == 0)
                {
                    action = "I";
                    var savePaymentTerms = _workorderems.spSetPymentTerm(action, null, objPaymentTermsModel.PTM_Term, objPaymentTermsModel.PTM_GracePeriod, objPaymentTermsModel.PTM_IsActive);
                }
                else
                {
                    action = "U";
                    var savePaymentTerms = _workorderems.spSetPymentTerm(action, objPaymentTermsModel.PTM_Id, objPaymentTermsModel.PTM_Term, objPaymentTermsModel.PTM_GracePeriod, objPaymentTermsModel.PTM_IsActive);
                }


                objPaymentTermsModel.Result = Result.Completed;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PaymentTermsDetails SavePaymentTerms(String Action, int? PTM_Id, string PTM_Term, int PTM_GracePeriod, bool IsActive)", "Exception While Saving payment term.", null);
                throw;
            }
            return objPaymentTermsModel;
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 19-Nov-2018
        /// Created For : To Active Payment Term
        /// </summary>
        /// <param name="PaymentTermId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActivePaymentTermById(long PaymentTermId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (PaymentTermId > 0)
                {
                    var getDetails = _workorderems.PaymentTerms.Where(x => x.PTM_Id == PaymentTermId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetPymentTerm(action, PaymentTermId, getDetails.PTM_Term,
                                                                              getDetails.PTM_GracePeriod,
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActiveCostCodeById(long CostCodeId, long UserId)", "Exception While activating Cost Code.", null);
                throw;
            }
            return result;
        }
    }
}
