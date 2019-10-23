using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class MainPDFFormManager : IMainPDFFormManager
    {
        public string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By  : Ashwajit Bansod
        /// Created Date : 09-April-2019
        /// Created For : To get details of pdf form
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>

        public PDFFormModelDetails GetMainPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var VendorTypeModelDetails = new PDFFormModelDetails();
                var pdfRepository = new PDFFormRepository();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<PDFFormModel>();
                Results = _workorderems.spGeteFormTrack(UserId).Select(x => new PDFFormModel()
                {
                    //FormName = x.,
                }).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                VendorTypeModelDetails.pageindex = pageindex;
                VendorTypeModelDetails.total = totalPages;
                VendorTypeModelDetails.records = totRecords;
                VendorTypeModelDetails.rows = Results.ToList();
                return VendorTypeModelDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PDFFormModelDetails GetMainPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of PDF Form.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-April-2019
        /// Created For : To get all form name
        /// </summary>
        /// <returns></returns>
        public List<PDFFormModel> GetPDFFormNameList()
        {
            var result = new List<PDFFormModel>();
            var getData = new PDFFormRepository();
            try
            {
                var data = getData.GetFormName();
                foreach (var item in data)
                {
                    if(item.FormId == Convert.ToUInt32(PDFForm.CharitableContributionRequestForm))
                    {
                        item.FormPath = HostingPrefix + "PDFData/Index";
                    }
                    result.Add(item);
                }
                return result;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PDFFormModel GetPDFFormNameList()", "Exception While Getting List of PDF Form name.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-April-2019
        /// Created For : to save pdf form details.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SavePDFData(PDFSaveModel obj)
        {
            var objModel = new PDFSaveModel();
            bool isSaved = false;
            try
            {
                if(obj != null)
                {
                    string Action = "I";
                    var saveData = _workorderems.spSeteformTrack(Action, null, obj.ModuleId, obj.FormId, obj.LocationId,
                                                                 obj.UserId, null, obj.FileName, null, "Y");
                    isSaved = true;
                }
                else
                {
                    isSaved = false; 
                }                
            }
            catch(Exception ex)
            {

            }
            return isSaved;
        }
    }
}
