using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class VehicleSeatingChartRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date  :27-Aug-2019
        /// Created For : TO get Superior Data 
        /// </summary>
        /// <returns></returns>
        public List<spGetVehicleSeating_Result> GetSuperiorList()
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeating().ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date  :27-Aug-2019
        /// Created For : TO save chart Data 
        /// </summary>
        /// <returns></returns>
        public bool SaveVSCRepository(AddChartModel Obj)
        {
            bool isSave = false;
            try
            {
                var save = objworkorderEMSEntities.spSetVehicleSeating(Obj.Action, Obj.Id,Obj.SeatingName,Obj.JobDesc,Obj.RolesAndResponsibility,
                                                                       Obj.Superior.ToString(), Obj.Superior, Obj.Department, Obj.EmploymentStatus, Obj.EmploymentClassification, Obj.RateOfPay,Obj.IsActive);
                isSave = true;
            }
            catch (Exception ex)
            {
                isSave = false;
                throw;

            }
            return isSave;
        }

        public List<spGetVehicleSeating_Result> GetVSCList(long? LocationId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeating().ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : To save Job Title
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SaveJobTitleRepository(AddChartModel Obj)
        {
            bool isSave = false;
            try
            {
                var save = objworkorderEMSEntities.spSetJobTitle(Obj.Action, Obj.Id, Obj.JobTitleDesc, Obj.JobTitleCount, null, Obj.parentId, Obj.IsActive);
                isSave = true;
            }
            catch (Exception ex)
            {
                isSave = false;
                throw;

            }
            return isSave;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-Sept-2019
        /// Created For : TO get Job title list by VCS Id
        /// </summary>
        /// <param name="CSVChartId"></param>
        /// <returns></returns>
        public List<spGetJobTitle_Result> GetJobTitleList(long CSVChartId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetJobTitle(CSVChartId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        public List<spGetVehicleSeating_Result> GetChartDetails()
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeating().ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public List<spGetVehicleSeatingPermission_Result> GetAccessPermissionList(long VST_Id)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeatingPermission(VST_Id).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;

            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 29-Oct-2019
        /// Created For : To save Job posting
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int SaveJob(JobPostingModel Obj)
        {
            try
            {
                var data = objworkorderEMSEntities.spSetJobPosting(Obj.AddChartModel.Action,null, Obj.RecruiteeId, Obj.AddChartModel.JobTitleId, Obj.HiringManager, Obj.LocationId, Obj.NumberOfPost,Obj.DOT_Status,"Y");
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Created Date : 30-Oct-2019
        /// Created By : Ashwajit Bansod
        /// Created For: To get Hiring manager list
        /// </summary>
        /// <param name="VSCId"></param>
        /// <returns></returns>
        public List<spGetHiringManager_Result> GetHiringManagerList(long VSCId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetHiringManager(VSCId).ToList();
                return data;
            }
            catch (Exception ex)
            {   
                throw;
            }
        }
    }
}
