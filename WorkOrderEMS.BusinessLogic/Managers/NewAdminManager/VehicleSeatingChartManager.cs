using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class VehicleSeatingChartManager : IAdminDashboard
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date: 28-Aug-2019
        /// Created For : To get Superiour list 
        /// </summary>
        /// <returns></returns>
        public List<AddChartModel> ListSuperiour()
        {
            var lstSuperiour = new List<AddChartModel>();
            var _ChartRepository = new VehicleSeatingChartRepository();
            try
            {
                lstSuperiour = _ChartRepository.GetSuperiorList()
                    .Select(x => new AddChartModel()
                    {
                        SeatingName = x.VST_Title,
                        parentId = x.VST_Id
                    }).ToList();
                return lstSuperiour;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AddChartModel> ListSuperiour()", "Exception While getting list Superiour.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 27-Aug-2019
        /// Created For : To Save Vehicle seating chart data to database
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public AddChartModel SaveVSC(AddChartModel Obj)
        {
            var _workorderEMS = new workorderEMSEntities();
            bool isSaved = false;
            try
            {
                var _VSCRepository = new VehicleSeatingChartRepository();
                string Action = string.Empty;
                if (Obj != null && Obj.SeatingName != null)
                {
                    if (Obj.Id == null)
                    {                     
                        isSaved = _VSCRepository.SaveVSCRepository(Obj);
                        Obj.Id = _workorderEMS.VehicleSeatings.OrderByDescending(x => x.VST_Id).FirstOrDefault().VST_Id;
                    }
                    else
                    {
                        Obj.Action = "U";
                        Obj.IsActive = "Y";
                        isSaved = _VSCRepository.SaveVSCRepository(Obj); 
                    }                    
                }
                else
                {
                    Obj = null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveVSC(AddChartModel Obj)", "Exception While Saving Vehicle seating chart.", Obj);
                throw;
            }
            return Obj;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 29-Aug-2019
        /// Created For : To get Vehicle seating chart list, I put location Id for furture purpose
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<AddChartModel> ListVehicleSeatingChart(long? LocationId)
        {
            var _VSCRepository = new VehicleSeatingChartRepository();
            var lstVSC = new List<AddChartModel>();
            try
            {
               var data  = _VSCRepository.GetVSCList(LocationId).Select(x => new AddChartModel() {
                   DepartmentName = x.DPT_Name,
                   Id = x.VST_Id,
                   Department = 0,
                   parentId = x.VST_ParentId,
                   JobDescription = x.VST_JobDescription,
                   RolesAndResponsibility = x.VST_RolesAndResponsiblities,
                   IsActive = x.VST_IsExempt,
                   SeatingName  = x.VST_Title,
                   //Image = HostingPrefix + ProfileImagePath.Replace("~", "") + "no-profile-pic.jpg"
               }).ToList();
                if(data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        if(item.JobDescription != null)
                        {
                            item.JobDescription = item.JobDescription.Replace("|", ",");
                            //item.JDSplitedString = item.JobDescription.Split('|');
                            lstVSC.Add(item);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<AddChartModel> ListVehicleSeatingChart(long? LocationId)", "Exception While getting list Vehicle seating chart.", LocationId);
                throw;
            }
            return lstVSC;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : To save Job Title
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SaveJobTitleVSC(AddChartModel Obj)
        {
            bool isSaved = false;
            try
            {
                var _VSCRepository = new VehicleSeatingChartRepository();
                string Action = string.Empty;
                if (Obj != null && Obj.JobTitleDesc != null)
                {
                    if (Obj.JobTitleId == null)
                    {
                        Obj.Action = "I";
                        Obj.IsActive = "N";
                        Obj.Id = Obj.JobTitleId;
                        if(Obj.JobTitleDesc != null)
                        {
                            string[] JobTitleList = Obj.JobTitleDesc.Split('|');
                            foreach (string title in JobTitleList)
                            {
                                if (title != " " && title != "")
                                {
                                    Obj.JobTitleDesc = title;
                                    isSaved = _VSCRepository.SaveJobTitleRepository(Obj);
                                }
                            }
                        }                        
                    }
                    else
                    {
                        Obj.Action = "U";
                        Obj.IsActive = "Y";
                        Obj.Id = Obj.JobTitleId;
                        //var getData = _VSCRepository.GetJobTitleList(Convert.ToInt64(Obj.parentId));
                        //if(getData != null)
                        //{
                        //    Obj.JobTitleDesc = getData.JBT_JobTitle + Obj.JobTitleDesc;
                        //}
                        if (Obj.JobTitleDesc != null)
                        {
                            string[] JobTitleList = Obj.JobTitleDesc.Split('|');
                            foreach (string title in JobTitleList)
                            {
                                isSaved = _VSCRepository.SaveJobTitleRepository(Obj);
                            }
                        }
                        //isSaved = _VSCRepository.SaveJobTitleRepository(Obj);
                    }
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveJobTitleVSC(AddChartModel Obj)", "Exception While Saving Job Title.", Obj);
                throw;
            }
            return isSaved;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : TO get data of job title
        /// </summary>
        /// <param name="CSVChartId"></param>
        /// <returns></returns>
        public List<AddChartModel> GetJobTitleData(long CSVChartId)
        {
            var data = new List<AddChartModel>();
            try
            {
                var _VSCRepository = new VehicleSeatingChartRepository();               
                string Action = string.Empty;
                if (CSVChartId > 0)
                {
                    data = _VSCRepository.GetJobTitleList(CSVChartId).
                        Select(x => new AddChartModel() {
                          Id = x.JBT_Id,
                          parentId = x.JBT_VST_Id,
                          JobTitleLabel = x.JBT_JobTitle
                        }).ToList();                                   
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public AddChartModel GetJobTitleData(long CSVChartId)", "Exception While getting data of Job Title.", CSVChartId);
                throw;
            }
            return data;
        }

        /// <summary>
        /// Created y  :Ashwajit Bansod
        /// Created Date : 11-Sept-2019
        /// Created For : To get chart details by Id
        /// </summary>
        /// <param name="CSVChartId"></param>
        /// <returns></returns>
        public AddChartModel GetChartData(long CSVChartId)
        {
            var data = new AddChartModel();
            try
            {
                var _VSCRepository = new VehicleSeatingChartRepository();
                string Action = string.Empty;
                if (CSVChartId > 0)
                {
                    data = _VSCRepository.GetChartDetails().Where(x => x.VST_Id == CSVChartId).
                        Select(x => new AddChartModel()
                        {
                            Id = x.VST_Id,
                            parentId = x.VST_ParentId,
                            JobDescription = x.VST_JobDescription,
                            RolesAndResponsibility = x.VST_RolesAndResponsiblities,
                            SeatingName = x.VST_Title,
                            Department = x.DPT_Id
                        }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public AddChartModel GetChartData(long CSVChartId)", "Exception While getting data of Vehicle Chart.", CSVChartId);
                throw;
            }
            return data;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-Sept-2019
        /// Created For : To get Access Permission list
        /// </summary>
        /// <param name="VST_Id"></param>
        /// <returns></returns>
        public List<AccessPermisionTreeViewModel> ListTreeViewAccessPermission(long VST_Id)
        {
            var _VSCRepository = new VehicleSeatingChartRepository();
            var records = new List<AccessPermisionTreeViewModel>();
            workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
            try
            {
                var getModuleList = objworkorderEMSEntities.Modules.Where(x => x.MDL_IsActive == "Y").
                    Select(x => new AccessPermisionTreeViewModel() {
                        ModuleId = x.MDL_Id,
                        ModuleName = x.MDL_ModuleName,
                        @checked = false
                    }).ToList();
                
                var Results = _VSCRepository.GetAccessPermissionList(VST_Id)
                    .Select(l => new AccessPermisionTreeViewModel//_workorderems.spGetCostCode(action, null).Select(l => new TreeViewModel
                    {
                        id = l.SMD_MDL_Id,
                        name = l.MDL_ModuleName,
                        SubModuleId = l.SMD_Id,
                        SubModuleName = l.SMD_SubModuleName,
                        @checked = false
                    }).ToList();
                records = getModuleList
                   .Select(l => new AccessPermisionTreeViewModel
                   {
                       id = l.ModuleId,
                       name = l.ModuleName,
                       @checked = false,
                       item = GetChildrenModule(Results, l.ModuleId)
                   }).ToList();
                //records = Results
                //   .Select(l => new AccessPermisionTreeViewModel
                //   {
                //       id = l.id,
                //       name = l.name,
                //       @checked = false,
                //       item = GetChildrenModule(Results, l.id)
                //   }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AccessPermisionTreeViewModel> ListTreeViewAccessPermission(long VST_Id)", "Exception While Listing Module and sub module tree.", null);
                throw;
            }
            return records;
        }
        private List<AccessPermisionTreeViewModel> GetChildrenModule(List<AccessPermisionTreeViewModel> ChildDataList, long? ModuleId)
        {
            return ChildDataList = ChildDataList.Where(x => x.id == ModuleId). //_workorderems.spGetCostCode(action, MasteCostCodeId).Select(l => new TreeViewModel
            Select(l => new AccessPermisionTreeViewModel
            {
                id = l.SubModuleId,
                name = l.SubModuleName,
                @checked = false
            }).ToList();           
        }

        public bool SaceAccessPermission(AccessPermisionTreeViewModel obj)
        {
            var _VSCRepository = new VehicleSeatingChartRepository();
            bool isSaved = false;
            try
            {
                if(obj != null)
                {
                    if(obj.id > 0)
                    {

                    }
                    else
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaceAccessPermission(AccessPermisionTreeViewModel obj)", "Exception While saving access permission", obj);
                throw;
            }
            return isSaved;
        }
    }
}
