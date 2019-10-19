using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;

namespace WorkOrderEMS.Data
{
    public class ePeopleRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 23-Sept-2019
        /// Created For : To get User List
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public List<UserRegistration> GetUserListByLocation(long? Location)
        {
            try
            {
                var data = objworkorderEMSEntities.UserRegistrations.Where(x => x.IsDeleted == false &&
                                                                          x.IsEmailVerify == true).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Creted By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get User Details by their manager Id 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetOrgnizationListview_Result> GetUserListByUserId(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationListview(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created For : To get self details by Employee Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetOrgnizationCommonview_Result GetUserSelfDetailsByUserId(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationCommonview(EmployeeId).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To get Position in VCS Chart by  Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetOrgnizationPositionView_Result UserPositionVCS(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationPositionView(EmployeeId).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get user data list
        /// Created Date : 05-oct-2019
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetOrgnizationUserView_Result1> UserTreeViewDetails(string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetOrgnizationUserView(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Creted By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To Get User Details by their manager Id 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        /// 
        public List<spGetVehicleSeating_Result2> GetVSCDetails(long VSCId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetVehicleSeating().Where(x => x.VST_Id == VSCId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 17-Oct-2019
        /// Created For : To get employee management list by login employee id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<spGetEmployeeManagementList_Result1> GetEmployeeManagementListData(long LocationId, string EmployeeId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetEmployeeManagementList(EmployeeId).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
