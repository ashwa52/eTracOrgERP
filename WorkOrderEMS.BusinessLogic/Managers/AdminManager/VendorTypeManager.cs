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

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class VendorTypeManager : IVendorType
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : October 19 2018
        /// Created For : List for Vendor Type.
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
        public VendorTypeModelDetails GetVendorTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var VendorTypeModelDetails = new VendorTypeModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                string action = "M";
                var Results = _workorderems.spGetVendorType()
                    .Select(a => new VendorTypeModel()
                    {
                        VendorType = a.VDT_VendorType,
                        Vendor_Id = a.VDT_Id,
                        Vendor_IsActive = a.VDT_IsActive
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "VendorTypeModelDetails GetVendorTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of Vendor Type.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashutosh Dwivedi
        /// Created Date : October 19 2018
        /// Created For : Add Vendor Type.
        /// </summary>
        /// <param name="VendorTypeModel"></param>
        /// <returns></returns>
        public VendorTypeModel SaveVendorType(VendorTypeModel objVendorTypeModel)
        {
            try
            {
                string action = "";
                //if (objVendorTypeModel.Vendor_Id == 0 && objVendorTypeModel.VendorType == null)
                //{
                //    action = "I";
                //    var saveVendorType= _workorderems.spSetVendorType(action, objVendorTypeModel.Vendor_Id,
                //                                                              objVendorTypeModel.VendorType, objVendorTypeModel.Vendor_IsActive);
                //    objVendorTypeModel.Result = Result.Completed;
                //}
                if (objVendorTypeModel.VendorType != null)
                {
                    action = "I";
                    var saveVendorType = _workorderems.spSetVendorType(action, null,
                                                                              objVendorTypeModel.VendorType, objVendorTypeModel.Vendor_IsActive);
                    objVendorTypeModel.Result = Result.Completed;
                }
                else
                {
                    action = "U";
                    var saveVendorType = _workorderems.spSetVendorType(action, objVendorTypeModel.Vendor_Id,
                                                                              objVendorTypeModel.VendorType, objVendorTypeModel.Vendor_IsActive);
                    objVendorTypeModel.Result = Result.UpdatedSuccessfully;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public VendorTypeModel SaveVendorType(VendorTypeModel objVendorTypeModel)", "Exception While Saving Vendor Type.", objVendorTypeModel);
                throw;
            }
            return objVendorTypeModel;
        }

        /// <summary>
        /// Created By Ashutosh Dwivedi 
        /// for Deleting the Vendor Type and set IsDeleted Field to 1
        /// </summary>
        /// <param name="vendorId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Result DeleteVendorType(long vendorId, long loggedInUserId, string location)
        {
            try
            {
                Result result;
                if (vendorId > 0)
                {
                    VendorTypeRepository objVendorTypeRepository = new VendorTypeRepository();
                    var data = objVendorTypeRepository.GetSingleOrDefault(v => v.VDT_Id == vendorId && v.VDT_IsActive == "Y");
                    if (data != null)
                    {
                        data.VDT_IsActive = "X";
                        objVendorTypeRepository.Update(data);
                        objVendorTypeRepository.SaveChanges();
                        return Result.Delete;
                    }
                }
                else
                {
                    return Result.DoesNotExist;
                }
                return Result.Delete;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public Result DeleteVendorType(long vendorId, long loggedInUserId)", "Exception While Deleting Vendor.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Nov-2018
        /// Created For : To active Vendor Type by vendor Id.
        /// </summary>
        /// <param name="VendorTypeId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ActiveVendorTypeById(long VendorTypeId, long UserId, string IsActive)
        {
            bool result = false;
            string action = "U";
            try
            {
                if (VendorTypeId > 0)
                {
                    var getDetails = _workorderems.VendorTypes.Where(x => x.VDT_Id == VendorTypeId)//(action, null)
                        .FirstOrDefault();
                    var Update = _workorderems.spSetVendorType(action, VendorTypeId, getDetails.VDT_VendorType,
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
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public bool ActiveVendorTypeById(long VendorTypeId, long UserId)", "Exception While activating Vendor Type.", null);
                throw;
            }
            return result;
        }
    }
}
