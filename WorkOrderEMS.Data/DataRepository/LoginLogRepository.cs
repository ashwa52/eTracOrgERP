using System;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data
{
    public class LoginLogRepository : BaseRepository<LoginLog>, ILoginLogRepository
    {

        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        /// <summary>
        /// Created By :Bhushan Dod
        /// Created Date: 26-05-2015
        /// Description : For Save log of login to trac the employee idle state
        /// </summary>
        /// <param name="objLoginLogModel"></param>
        /// <returns>LogId</returns>
        public long SaveLoginLog(LoginLogModel objLoginLogModel)
        {
            LoginLog Obj = new LoginLog();
            try
            {

                Obj.UserID = objLoginLogModel.UserID;
                Obj.LocationId = objLoginLogModel.LocationId;
                Obj.UserType = objLoginLogModel.UserType;
                Obj.CreatedBy = objLoginLogModel.UserID;
                Obj.CreatedOn = DateTime.UtcNow;
                Obj.DeletedBy = null;
                Obj.DeletedOn = null;
                Obj.IsDeleted = false;
                Obj.ModifiedBy = null;
                Obj.ModifiedOn = null;
                Add(Obj);
                long LogId = Obj.LogId;

                return LogId;
            }
            catch (Exception)
            {
                throw;
            }
        }       
        public sp_GetIdleStatusOfEmployee_2_Result IdleStatusOfEmployee(long UserId, long LocationId)
        {
            //sp_GetIdleStatusOfEmployee_Result obj = new sp_GetIdleStatusOfEmployee_Result();
            sp_GetIdleStatusOfEmployee_2_Result obj = new sp_GetIdleStatusOfEmployee_2_Result();
            try
            {
                //obj = _workorderEMSEntities.sp_GetIdleStatusOfEmployee(LocationId, UserId).Select(t => new sp_GetIdleStatusOfEmployee_Result()
                //{
                //    Response = t.Response,
                //    ResponseLocation = t.ResponseLocation,
                //    ResponseMessage = t.ResponseMessage
                //}).FirstOrDefault();
                obj = _workorderEMSEntities.sp_GetIdleStatusOfEmployee_2(LocationId, UserId).Select(t => new sp_GetIdleStatusOfEmployee_2_Result()
                {
                    Response = t.Response,
                    ResponseLocation = t.ResponseLocation,
                    ResponseMessage = t.ResponseMessage,
                    ResponseTimeForNextIdleCall = t.ResponseTimeForNextIdleCall
                    
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }

        public IdleEmployeeResult IdleStatusOfEmployeeNew(long UserId, long LocationId)
        {
            var getFirstLoginLogData = _workorderEMSEntities.LoginLogs.Where(x => x.UserID == UserId && x.LocationId == LocationId 
                                                          && x.IsActive == true && x.IsDeleted == false)
                                                          .OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            var AddPlusThirtyInLoginTime = DateTime.UtcNow.AddMinutes(-30);

            return null;
        }
    }
}
