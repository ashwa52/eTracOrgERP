using System;
using System.Collections.Generic;
using System.Globalization;
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
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
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
        public List<UserModelList> GetUserHeirarchyList(long? LocationId, long? UserId)
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
                }).FirstOrDefault();
                return null;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetUserHeirarchyList(long? LocationId)", "Exception While getting list User.", LocationId);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 05-oct-2019
        /// Created For : To get User Details fro Tree View
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserListViewEmployeeManagementModel> GetUserTreeViewList(long UserId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            var lstDetails = new List<UserListViewEmployeeManagementModel>();
            var UserModel = new UserListViewEmployeeManagementModel();
            try
            {
                if (UserId > 0)
                {
                    var getUserDetails = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId && x.IsEmailVerify == true && x.IsDeleted == false).FirstOrDefault();
                    if (getUserDetails != null)
                    {
                        var selfData = _ePeopleRepository.GetUserSelfDetailsByUserId(getUserDetails.EmployeeID);
                        if (selfData != null)
                        {
                            UserModel.DepartmentName = selfData.DepartmentName;
                            UserModel.EmployeeId = selfData.EMP_EmployeeID;
                            UserModel.EmployeeName = selfData.EmployeeName;
                            UserModel.ProfilePhoto = selfData.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + selfData.EMP_Photo;
                            lstDetails.Add(UserModel);
                        }
                        var data = _ePeopleRepository.UserTreeViewDetails(getUserDetails.EmployeeID).Select(x => new UserListViewEmployeeManagementModel()
                        {
                            EmployeeId = x.EMP_EmployeeID,
                            EmployeeName = x.EmployeeName,
                            JobTitle = x.JBT_JobTitle,
                            LocationId = x.EMP_LocationId,
                            JobTitleId = x.EMP_JobTitleId,
                            ProfilePhoto = x.EMP_Photo
                        }).ToList();
                        foreach (var item in data)
                        {
                            item.ProfilePhoto = item.ProfilePhoto == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfilePhoto;
                            lstDetails.Add(item);
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetUserHeirarchyList(long? LocationId)", "Exception While getting list User.", UserId);
                throw;
            }
            return lstDetails;
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created For : To get User Details By User id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserListViewEmployeeManagementModel> GetUserListByUserId(long? LocationId, long? UserId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            var lstDetails = new List<UserListViewEmployeeManagementModel>();
            var UserModel = new UserListViewEmployeeManagementModel();
            try
            {
                if (UserId > 0)
                {
                    var getUserDetails = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId && x.IsEmailVerify == true && x.IsDeleted == false).FirstOrDefault();
                    if (getUserDetails != null)
                    {
                        var selfData = _ePeopleRepository.GetUserSelfDetailsByUserId(getUserDetails.EmployeeID);
                        if (selfData != null)
                        {
                            UserModel.DepartmentName = selfData.DepartmentName;
                            UserModel.EmployeeId = selfData.EMP_EmployeeID;
                            UserModel.EmployeeName = selfData.EmployeeName;
                            UserModel.JobDesc = selfData.VST_JobDescription;
                            UserModel.JobTitle = selfData.JBT_JobTitle;
                            UserModel.LocationId = selfData.EMP_LocationId;
                            UserModel.LocationName = selfData.LocationName;
                            UserModel.JobTitleId = selfData.EMP_JobTitleId;
                            UserModel.ProfilePhoto = selfData.EMP_Photo;
                            string[] JobTitleList = UserModel.JobDesc.Split('|');
                            List<string> myCollection = new List<string>();
                            foreach (string title in JobTitleList)
                            {
                                if (title != " " && title != "")
                                {
                                    myCollection.Add(title);
                                }
                                UserModel.JobDescList = myCollection;
                            }
                            lstDetails.Add(UserModel);
                            //lstDetails.Add(item);
                        }                      
                        var data  = _ePeopleRepository.GetUserListByUserId(getUserDetails.EmployeeID).Select(x => new UserListViewEmployeeManagementModel()
                        {
                            DepartmentName = x.DepartmentName,
                            EmployeeId = x.EMP_EmployeeID,
                            EmployeeName = x.EmployeeName,
                            JobDesc =x.VST_JobDescription,
                            JobTitle = x.JBT_JobTitle,
                            LocationId = x.EMP_LocationId,
                            LocationName = x.LocationName,
                            JobTitleId = x.EMP_JobTitleId,
                            ProfilePhoto = x.EMP_Photo
                        }).ToList();
                        foreach (var item in data)
                        {
                            string[] JobTitleList = item.JobDesc.Split('|');
                            List<string> myCollection = new List<string>();
                            foreach (string title in JobTitleList)
                            {
                                
                                if (title != " " && title != "")
                                {
                                    myCollection.Add(title);
                                    
                                }
                                item.JobDescList = myCollection;
                            }

                            lstDetails.Add(item);
                        }
                    }
                    return lstDetails;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserListViewEmployeeManagementModel> GetUserList(long? LocationId, long? UserId)", "Exception While getting list User.", LocationId);
                throw;
            }
            return lstDetails;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Oct-2019
        /// Created
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public UserListViewEmployeeManagementModel GetVCSPositionByUserId(long? UserId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            var getDetails = new UserListViewEmployeeManagementModel();
            try
            {
                if (UserId > 0)
                {
                    var getUserDetails = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId && x.IsEmailVerify == true && x.IsDeleted == false).FirstOrDefault();
                    if (getUserDetails != null)
                    {
                        var data = _ePeopleRepository.UserPositionVCS(getUserDetails.EmployeeID);
                        if(data != null)
                        {
                            getDetails.JobTitleId = data.EMP_JobTitleId;
                            getDetails.VSTId = data.JBT_VST_Id;
                            getDetails.EmployeeId = data.EMP_EmployeeID;
                        }
                    }
                }
                return getDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetUserHeirarchyList(long? LocationId)", "Exception While getting list User.", UserId);
                throw;
            }
        }
    }
}
