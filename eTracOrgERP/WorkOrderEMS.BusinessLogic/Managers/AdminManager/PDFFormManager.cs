using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class PDFFormManager : IPDFFormManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-April-2019
        /// Created For : To get module list for PDF forms
        /// </summary>
        /// <returns></returns>
        public List<ModuleListModel> GetModuleList()
        {
            try
            {
                var lstModule = new List<ModuleListModel>();
                lstModule = _workorderems.spGetModule().Select(x => new ModuleListModel()
                {
                    ModuleName = x.MDL_ModuleName,
                    ModuleId = x.MDL_Id
                }).ToList();
                return lstModule;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ModuleListModel> GetModuleList()", "Exception While getting list of module.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By:Ashwajit bansod
        /// Created For : To get form details list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public PDFFormModelDetails GetPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var VendorTypeModelDetails = new PDFFormModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGet_eForms()
                    .Select(a => new PDFFormModel()
                    {
                     FormId = a.EFM_Id,
                     FormName = a.EFM_eFormName,
                     FormPath = a.EFM_eFormLink,
                     ModuleId = a.EFM_MDL_Id ,
                     IsActive = a.EFM_IsActive == "Y" ? "Active":"Deactive"
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PDFFormModelDetails GetPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of PDF Form.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By: Ashwajit bansod
        /// Created Date : 19-Feb-2019
        /// Created For : To save PDF form
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SavePDFForm(PDFFormModel Obj)
        {
            bool IsSaved = false;
            try
            {
                string action = "";
                if (Obj.FormId == 0)
                {
                    action = "I";
                    var saveVendorType = _workorderems.spSet_eForms(action, null, Obj.ModuleId, Obj.FormName,
                                                                     Obj.FormPath,"N");

                    //var saveVendorType = _workorderems.spSetFormMaster(action,null,Obj.FormType,Obj.FormName,
                    //                                                  Obj.FormPath,"Y");
                    IsSaved = true;
                }
                else
                {
                    action = "U";
                    var saveVendorType = _workorderems.spSet_eForms(action, null, Obj.ModuleId, Obj.FormName,
                                                                     Obj.FormPath, "Y");
                    //var saveVendorType = _workorderems.spSetFormMaster(action, null, Obj.FormType, Obj.FormName,
                    //                                                  Obj.FormPath, "Y");
                    IsSaved = true;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SavePDFForm(PDFFormModel Obj)", "Exception While Saving PDF Form.", Obj);
                throw;
            }
            return IsSaved;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-April-2019
        /// Created For : To get all form data to download PDF forms
        /// </summary>
        /// <param name="formID"></param>
        /// <returns></returns>
        public PDFFormModel GetePDFFormDetailsById(long formID)
        {
            try
            {
                var db = new workorderEMSEntities();

                var editFormDetails = new PDFFormModel();
                var FormDetails = db.eForms.Where(u => u.EFM_Id == formID).Select(
                a => new PDFFormModel()
                {
                   FormPath = a.EFM_eFormLink
                }).FirstOrDefault();
                return FormDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public PDFFormModel GetePDFFormDetailsById(long formID)", "Exception While getting Form detail by id.", null);
                throw;
            }
        }

        public bool ActiveFormById(long Id, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (Id > 0)
                {
                    var getDetails = _workorderems.eForms.Where(x => x.EFM_Id == Id)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSet_eForms(action, getDetails.EFM_Id, getDetails.EFM_MDL_Id,
                                                                 getDetails.EFM_eFormName, getDetails.EFM_eFormLink, IsActive);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ActiveFormById(long Id, long UserId, string IsActive)", "Exception While activating forms.", UserId);
                throw;
            }
            return result;
        }
    }
}
