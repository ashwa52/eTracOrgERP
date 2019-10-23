using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
namespace WorkOrderEMS.BusinessLogic
{
    public class AdminCompanyManager : ICompanyAdmin
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public CompanyAdminListDetails GetAllCompanyAdmiDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objCompanyListDetails = new CompanyAdminListDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                LocationId = 0;
                var data = _workorderems.spGetCompanyListForAdmin();//.Where(x => x.c)  // .CompanyFacilityMappings.Where(x => x.CFM_CMP_Id == VendorId)
                var Results = data.Select(a => new AdminCompanyModel()
                    {
                        VendorId = a.CMP_Id > 0? a.CMP_Id :0,
                        CompanyNameLegal = a.CMP_NameLegal == null ?"N/A": a.CMP_NameLegal,
                        Address = a.Address1 == null ? "N/A" : a.Address1,
                        CompanyType = a.COT_CompanyType == null ? "N/A" : a.COT_CompanyType,
                        StateLicDoc = a.LNC_LicenseDocument,
                        StateLicExpDate = a.LNC_ExpirationDate == null ? "N/A" : a.LNC_ExpirationDate.Value.ToString("MM/dd/yyyy"),
                        TaxIdNo = a.TXD_TaxIdNumber == null?"N/A": a.TXD_TaxIdNumber
                }).OrderByDescending(x => x.VendorId).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objCompanyListDetails.pageindex = pageindex;
                objCompanyListDetails.total = totalPages;
                objCompanyListDetails.records = totRecords;
                objCompanyListDetails.rows = Results.ToList();
                return objCompanyListDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public CompanyListDetails GetAllCompanyDataList(long? LocationId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of all company.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-Nov-2018
        /// Created For : To Get License of company by Id
        /// </summary>
        /// <param name="CMPID"></param>
        /// <returns></returns>
        public VendorInsuranceModel GetCompanyDetailsById(long CMPID)
        {
            try
            {
                var data = _workorderems.Licenses.Where(u => u.LNC_CMP_Id == CMPID).Select(x => new VendorInsuranceModel()
                {
                    LicenseDocument = x.LNC_LicenseDocument,
                    LicenseExpirationDate = x.LNC_ExpirationDate
                }).FirstOrDefault();
                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }                
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public VendorInsuranceModel GetCompanyDetailsById(long CMPID)", "Exception While getting License detail by id.", null);
                throw;
            }
        }

        
    }
}
