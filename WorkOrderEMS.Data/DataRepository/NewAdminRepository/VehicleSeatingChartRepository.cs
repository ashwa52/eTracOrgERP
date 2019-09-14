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
        public List<spGetVehicleSeating_Result1> GetSuperiorList()
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
                                                                       Obj.Superior.ToString(), Obj.Superior, Obj.Department,Obj.IsActive);
                isSave = true;
            }
            catch (Exception ex)
            {
                isSave = false;
                throw;

            }
            return isSave;
        }

        public List<spGetVehicleSeating_Result1> GetVSCList(long? LocationId)
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
                var save = objworkorderEMSEntities.spSetJobTitle(Obj.Action, Obj.Id, Obj.JobTitleDesc, Obj.parentId, Obj.IsActive);
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
        public List<spGetVehicleSeating_Result1> GetChartDetails()
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
    }
}
