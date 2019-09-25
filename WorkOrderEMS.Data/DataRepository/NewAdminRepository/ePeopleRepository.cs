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
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
