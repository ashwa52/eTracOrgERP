using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DepartmentManager : IDepartment
    {
        /// <summary>
        /// Created By : Ashwajit Bansod 
        /// Created Date : 26- Aug-2019
        /// Created For : To save Department
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SaveDepartment(DepartmentModel Obj)
        {
            bool isSaved = false;
            try
            {
                var _DepartmentRepository = new DepartmentRepository();
                string Action = string.Empty;
                if (Obj.DepartmentName != null)
                {
                    if (Obj.DeptId == null)
                    {
                        Obj.Action = "I";
                        Obj.IsActive = "N";
                        isSaved = _DepartmentRepository.SaveDepartmentRepository(Obj);
                    }
                    else
                    {
                        Obj.Action = "U";
                        Obj.IsActive = "Y";
                        isSaved = _DepartmentRepository.SaveDepartmentRepository(Obj);
                    }
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveDepartment(DepartmentModel Obj)", "Exception While Saving Department.", Obj);
                throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-Aug-2019
        /// Created For : To get All Department list
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<DepartmentModel> ListAllDepartment(string txt, long? LocationId, long? UserId)
        {
            var lstDepartment = new List<DepartmentModel>();
            var _DepartmentRepository = new DepartmentRepository();
            try
            {
                lstDepartment = _DepartmentRepository.GetDepartmentList(txt, LocationId, UserId)
                    .Where(x => x.DPT_IsActive == "Y").Select(x => new DepartmentModel()
                    {
                        DepartmentName = x.DPT_Name,
                        IsActive_Grid = x.DPT_IsActive == "Y"?true:false,
                        DeptId = x.DPT_Id,
                    }).ToList();
                   return lstDepartment;                
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<DepartmentModel> ListAllDepartment(string txt, long? LocationId, long? UserId)", "Exception While getting list Department.", null);
                throw;
            }
        }


        //public DepartmentDetails ListAllDepartment(string txt, long? LocationId, long? UserId, long? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        //{
        //    try
        //    {
        //        var objContractTypeDetails = new DepartmentDetails();
        //        var _DepartmentRepository = new DepartmentRepository();
        //        int pageindex = Convert.ToInt32(pageIndex) - 1;
        //        int pageSize = Convert.ToInt32(numberOfRows);
        //        var Results  = _DepartmentRepository.GetDepartmentList(txt, LocationId, UserId)
        //            .Select(x => new DepartmentModel()
        //            {
        //                DepartmentName = x.DPT_Name,
        //                IsActive_Grid = x.DPT_IsActive == "Y" ? true : false,
        //                DeptId = x.DPT_Id,
        //            }).ToList();

        //        int totRecords = Results.Count();
        //        var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
        //        objContractTypeDetails.pageindex = pageindex;
        //        objContractTypeDetails.total = totalPages;
        //        objContractTypeDetails.records = totRecords;
        //        objContractTypeDetails.rows = Results.ToList();
        //        return objContractTypeDetails;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public DepartmentDetails ListAllDepartment(string txt, long? LocationId, long? UserId, long? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of Department.", null);
        //        throw;
        //    }
        //}
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Cerated Date : 26-Aug-2019
        /// Created For : To get details of department as per dept Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DepartmentModel GetDepartmentData(long Id)
        {
            var _DepartmentRepository = new DepartmentRepository();
            var getDetails = new DepartmentModel();
            string txt = string.Empty; long? LocationId = 0, UserId = 0;
            try
            {
              if(Id > 0)
                {
                    getDetails = _DepartmentRepository.GetDepartmentList(txt, LocationId, UserId).
                        Where(x => x.DPT_Id == Id && x.DPT_IsActive == "Y").Select(a => new DepartmentModel() {
                            DepartmentName = a.DPT_Name,
                            IsActive = a.DPT_IsActive,
                            DeptId = a.DPT_Id,
                            IsActive_Grid = a.DPT_IsActive == "Y"?true:false,                           
                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public DepartmentModel GetDepartmentData(long Id)", "Exception While getting data Department.", Id);
                throw;
            }
            return getDetails;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-Sept-2019
        /// Created For : To delete Department by departmetn Id
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public bool DeleteDepartmentById(DepartmentModel Obj)
        {
            bool isSaved = false;
            try
            {
                var _DepartmentRepository = new DepartmentRepository();
                string Action = string.Empty;
                if (Obj.DeptId > 0)
                {
                    Obj.Action = "U";
                    Obj.IsActive = "X";
                    isSaved = _DepartmentRepository.IsDepetmentDeleted(Obj);                    
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveDepartment(DepartmentModel Obj)", "Exception While deleting Department.", Obj);
                throw;
            }
            return isSaved;
        }
    }
}
