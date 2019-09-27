using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class ePeopleManager : IePeopleManager
    {
        ePeopleRepository _ePeopleRepository = new ePeopleRepository();
        public List<UserModelList> GetUserList(long? LocationId)
        {
            try
            {
                var data = _ePeopleRepository.GetUserListByLocation(LocationId).Select(x => new UserModelList() {
                    Name = x.FirstName + " " + x.LastName,
                    UserType = x.GlobalCode.CodeName,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    ProfileImage = x.ProfileImage
                }).ToList();
                return data;
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetUserList(long? LocationId)", "Exception While getting list User.", LocationId);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 23-Sept-2019
        /// Created For : To get User list by Location id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public UserModelList GetUserHeirarchyList(long? LocationId, long? UserId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            try
            {
                var data = _ePeopleRepository.GetUserListByLocation(LocationId).Where(x => x.UserId == UserId).Select(x => new UserModelList()
                {
                    Name = x.FirstName + " " + x.LastName,
                    UserType = x.GlobalCode.CodeName,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    ProfileImage = x.ProfileImage,
                    ChildrenList = GetChildrenUser(x.UserId, LocationId).ToList()
                }).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetUserHeirarchyList(long? LocationId)", "Exception While getting list User.", LocationId);
                throw;
            }
        }
        /// <summary>
        /// Get Child User data
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        private List<UserModelList> GetChildrenUser(long UserId, long? LocationId)
        {
            workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
            var data = objworkorderEMSEntities.UserRegistrations.Where(x => x.ManagerForUser == UserId).Select(x => new UserModelList()
            {
                Name = x.FirstName + " " + x.LastName,
                UserType = x.GlobalCode.CodeName,
                UserId = x.UserId,
                UserEmail = x.UserEmail,
                ProfileImage = x.ProfileImage,
                ChildrenList = GetChildrenUser(x.UserId, LocationId).ToList()
            }).ToList();
            return data;
        }

    }
}
