using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.BusinessLogic
{
    public class ePeopleManager : IePeopleManager
    {
        ePeopleRepository _ePeopleRepository = new ePeopleRepository();
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"];
        public List<UserModelList> GetUserList(long? LocationId)
        {
            try
            {
                var data = _ePeopleRepository.GetUserListByLocation(LocationId).Select(x => new UserModelList()
                {
                    Name = x.FirstName + " " + x.LastName,
                    UserType = x.GlobalCode.CodeName,
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    ProfileImage = x.ProfileImage
                }).ToList();
                return data;
            }
            catch (Exception ex)
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

        public List<UserListViewEmployeeManagementModel> GetUserTreeViewListTesting(long UserId)
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
                        lstDetails = _ePeopleRepository.UserTreeViewDetails(getUserDetails.EmployeeID).Select(x => new UserListViewEmployeeManagementModel()
                        {
                            EmployeeId = x.EMP_EmployeeID,
                            EmployeeName = x.EmployeeName,
                            JobTitle = x.JBT_JobTitle,
                            LocationId = x.EMP_LocationId,
                            JobTitleId = x.EMP_JobTitleId,
                            UserId = Cryptography.GetEncryptedData(x.UserId.ToString(), true),
                            ProfilePhoto = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo
                        }).ToList();
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
                        var data = _ePeopleRepository.GetUserListByUserId(getUserDetails.EmployeeID).Select(x => new UserListViewEmployeeManagementModel()
                        {
                            DepartmentName = x.DepartmentName,
                            EmployeeId = x.EMP_EmployeeID,
                            EmployeeName = x.EmployeeName,
                            JobDesc = x.VST_JobDescription,
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
                        if (data != null)
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
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-OCT-2019
        /// Created For : To get VSC list to delete
        /// </summary>
        /// <returns></returns>
        public List<AddChartModel> GetVSCList()
        {
            try
            {
                var lst = objworkorderEMSEntities.spGetVehicleSeating().Select(x => new AddChartModel()
                {
                    SeatingName = x.VST_Title,
                    Id = x.VST_Id
                }).ToList();

                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AddChartModel> GetVSCList()", "Exception While getting list VSC.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Oct-2019
        /// Created For : To get VSC details by VSC Id
        /// </summary>
        /// <param name="VSCId"></param>
        /// <returns></returns>
        public AddChartModel GetVSCDetailsById(long VSCId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            try
            {
                var lst = _ePeopleRepository.GetVSCDetails(VSCId).Select(x => new AddChartModel()
                {
                    DepartmentName = x.DPT_Name,
                    EmploymentClassification = x.VST_IsExempt,
                    EmploymentStatus = x.VST_EmploymentStatus,
                    JobDescription = x.VST_JobDescription,
                    SeatingName = x.VST_Title,
                    RateOfPay = x.VST_RateOfPay,
                    RolesAndResponsibility = x.VST_RolesAndResponsiblities,
                    // x.
                }).FirstOrDefault();

                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AddChartModel> GetVSCDetailsById(long VSCId)", "Exception While getting details VSC.", VSCId);
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 17-Oct-2019
        /// Created For : To get Employee Management List
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserModelList> GetEmployeeMgmList(long LocationId, long UserId)
        {
            var _ePeopleRepository = new ePeopleRepository();
            var lst = new List<UserModelList>();
            try
            {
                if (UserId > 0)
                {
                    var userDetails = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId && x.IsEmailVerify == true && x.IsDeleted == false).FirstOrDefault();
                    if (userDetails != null)
                    {
                        lst = _ePeopleRepository.GetEmployeeManagementListData(LocationId, userDetails.EmployeeID).Select(x => new UserModelList()
                        {
                            id = Cryptography.GetEncryptedData(x.UserId.ToString(), true),
                            UserId = x.UserId,
                            UserEmail = x.EMP_Email,
                            Name = x.EmployeeName,
                            HiringDate = x.EMP_DateOfJoining,
                            UserType = x.JBT_JobTitle,
                            ProfileImage = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                        }).ToList();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UserModelList> GetEmployeeMgmList(long LocationId, long UserId)", "Exception While getting employee Management List.", UserId);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Creaed Date : 18-Oct-2019
        /// Created For : To get Personal data to view
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DemotionModel GetEMployeeData(long UserId)
        {
            var UserModel = new DemotionModel();
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
                            UserModel.Name = selfData.EmployeeName;
                            UserModel.EmpId = selfData.EMP_EmployeeID;
                            UserModel.LocationName = selfData.LocationName;
                            UserModel.LocationIdCurrent = selfData.EMP_LocationId;
                            UserModel.JobTitleCurrent = selfData.EMP_JobTitleId;
                            UserModel.Image = selfData.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + selfData.EMP_Photo;
                        }
                    }
                }
                else
                {
                    UserModel = null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public UserListViewEmployeeManagementModel GetEMployeeData(long UserId)", "Exception While getting employee Details.", UserId);
                throw;
            }
            return UserModel;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To save for approval requisition
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool ApprovalRequisition(AddChartModel Obj)
        {
            var _workorderEMS = new workorderEMSEntities();
            bool isSaved = false;
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                string Action = string.Empty;

                var getEmpDetails = _workorderEMS.UserRegistrations.Where(x => x.UserId == Obj.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                if (getEmpDetails != null)
                {
                    Obj.EmployeeId = getEmpDetails.EmployeeID;
                    if (Obj.IsDeleted == false)
                    {
                        if (Obj != null && Obj.SeatingName != null)
                        {
                            if (Obj.RequisitionId == 0)
                            {
                                Obj.Action = "I";
                                Obj.ActionStatus = "Y";
                                Obj.IsActive = "Y";
                                Obj.RequisitionType = "Add Seat";
                                isSaved = ePeopleRepository.SendForApproval(Obj);
                            }
                            else
                            {
                                Obj.Action = "U";
                                Obj.IsActive = "Y";
                                isSaved = ePeopleRepository.SendForApproval(Obj);
                            }
                        }
                    }
                    else
                    {
                        Obj.ActionStatus = "X";
                        Obj.RequisitionType = "Remove Seat";
                        isSaved = ePeopleRepository.SendForApproval(Obj);
                    }
                    isSaved = true;
                }
                else
                {
                    isSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ApprovalRequisition(AddChartModel Obj)", "Exception While Saving Vehicle seating chart.", Obj);
                throw;
            }
            return isSaved;
        }
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 20-Oct-2019
        /// Created For : To get Requsition list
        /// </summary>
        /// <returns></returns>
        public List<AddChartModel> GetRequisitionlist()
        {
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                var lst = ePeopleRepository.GetRequisitionlist().Select(x => new AddChartModel()
                {
                    RequisitionId = x.RQS_Id,
                    Id = x.RQS_ActivityId,
                    RequisitionType = x.RQS_RequizationType,
                    ActionStatus = x.RQS_ApprovalStatus == "W" ? "Waiting" : x.RQS_ApprovalStatus == "A" ? "Approved" : "Reject",
                    SeatingName = x.Activity
                }).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AddChartModel> GetRequisitionlist()", "Exception While getting lis of Requisition.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 21-Oct-2019
        /// Created For : TO approve/ reject requisition
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ApproveRejectAction(long Id, string Status, long UserId)
        {
            var _workorderEMS = new workorderEMSEntities();
            bool IsApproveReject = false;
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                if (UserId > 0)
                {
                    var getEmpDetails = _workorderEMS.UserRegistrations.Where(x => x.UserId == UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    if (getEmpDetails != null)
                    {
                        if (Id > 0 && Status != null)
                        {
                            var Statusdata = ePeopleRepository.ApproveRejectRequisition(Id, Status, getEmpDetails.EmployeeID);
                            IsApproveReject = true;
                        }
                        else
                        {
                            IsApproveReject = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ApprovalRequisition(AddChartModel Obj)", "Exception While Saving Vehicle seating chart.", Id);
                throw;
            }
            return IsApproveReject;
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 21-oct-2019
        /// Created For : To get job title cout list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<AddChartModel> GetJobTitleCountForRequistion(long VSCId)
        {
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                var lst = ePeopleRepository.GetJobTitleCount(VSCId).Select(x => new AddChartModel()
                {
                    JobTitleCount = x.JBT_JobCount,
                    Id = x.JBT_Id,
                    JobTitle = x.JBT_JobTitle
                }).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<AddChartModel> GetJobTitleCountForRequistion(long VSCId)", "Exception While getting lis of job tilte.", null);
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 22-oct-2019
        /// Created For : To get job details by job Id
        /// </summary>
        /// <param name="JobId"></param>
        /// <returns></returns>
        public JobTitleModel GetJobTitleCount(long JobId)
        {
            var details = new JobTitleModel();
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                var data = ePeopleRepository.GetJobCount(JobId);
                if (data != null)
                {

                    details.JobTitleCount = data.JBT_JobCount;
                    details.JobTitleId = data.JBT_Id;
                    details.JobTitle = data.JBT_JobTitle;
                    details.JobTitleLastCount = data.JBT_JobCount;
                }
                return details;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public JobTitleModel GetJobTitleCount(long JobId)", "Exception While getting details of job tilte.", JobId);
                throw;
            }
        }

        /// <summary>
        /// Created By  :Ashwajit bansod
        /// Created Date : 22-Oct-2019
        /// Created For : To send job title for approval
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendJobTitleForApproval(JobTitleModel model)
        {
            var _workorderEMS = new workorderEMSEntities();
            bool isSaved = false;
            var Obj = new AddChartModel();
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                var getEmpDetails = _workorderEMS.UserRegistrations.Where(x => x.UserId == model.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                if (getEmpDetails != null)
                {
                    if (model != null)
                    {
                        Obj.JobTitleCount = model.JobTitleCount;
                        Obj.Id = model.JobTitleId;
                        Obj.EmployeeId = getEmpDetails.EmployeeID;
                        if (model.JobTitleCount > model.JobTitleLastCount)
                        {
                            Obj.ActionStatus = "Y";
                            Obj.IsActive = "Y";
                            Obj.RequisitionType = "Add Head Count";
                        }
                        else
                        {
                            Obj.ActionStatus = "Y";
                            Obj.IsActive = "Y";
                            Obj.RequisitionType = "Remove Head Count";

                        }
                        isSaved = ePeopleRepository.SendForApproval(Obj);
                        isSaved = true;
                    }
                }
                else
                    isSaved = false;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, " public bool SendJobTitleForApproval(JobTitleModel model)", "Exception While getting send job title for approval.", model);
                throw;
            }
            return isSaved;
        }

        #region Files
        /// <summary>
        /// Created By
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveDirectDepositeForm(DirectDepositeFormModel model)
        {
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var tt = new GuestUserRepository();
                    var data = tt.GetDirectDepositeDataByEmployeeId(model.EmployeeId);
                    if (data != null)
                        return Context.spSetDirectDepositForm("U", model.EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;

                    return Context.spSetDirectDepositForm("I", model.EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
                            model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount.HasValue ? model.Account1.DepositeAmount.Value : 0, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
                            , model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-Oct-2019
        /// Created For : To get uploaded files data
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<UploadedFiles> GetUploadedFilesOfUser(string EmployeeId)
        {
            var lst = new List<UploadedFiles>();
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                if (EmployeeId != null)
                {
                    var getFileList = ePeopleRepository.GetUploadFilesList(EmployeeId).ToList();
                    if (getFileList.Count() > 0)
                    {
                        lst = getFileList.Select(x => new UploadedFiles()
                        {
                            FileName = x.FLU_FileName == null ? null : x.FLU_FileName,
                            FileTypeName = x.FLT_FileType == null ? null : x.FLT_FileType,
                            AttachedFileName = x.FLU_FileAttached == null ? null : x.FLU_FileAttached,
                            FileId = x.FLU_FLT_Id > 0 ? 0 : x.FLU_FLT_Id,
                            AttachedFileLink = x.FLU_FileAttached == null ? null : HostingPrefix + FilePath.Replace("~", "") + x.FLU_FileAttached
                        }).ToList();
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public UploadedFiles GetUploadedFilesOfUser(string EmployeeId)", "Exception While getting list of files by user id.", EmployeeId);
                throw;
            }
        }
        #endregion Files
        #region GRAPH COUNT
        public List<GraphCountModel> GetEMP_ReqCount()
        {
            var repository = new ePeopleRepository();
            var lst = new List<GraphCountModel>();
            try
            {
                var getEmpCount = repository.GetEmployeeManagementListData(0, null).GroupBy(x => x.JBT_JobTitle)
                    .Select(x => new GraphCountModel()
                    {
                        Employee = x.Count(),
                        JobTitle = x.Key

                    });
                var requisitionlst = repository.GetRequisitionlist().GroupBy(x => x.RQS_RequizationType).
                    Select(x => new GraphCountModel()
                    {
                        RequisitionName = x.Key,
                        Requisition = x.Count()
                    }).ToList();
                lst.AddRange(getEmpCount);
                lst.AddRange(requisitionlst);
                return lst;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<GraphCountModel> GetEMP_ReqCount()", "Exception While getting count list.", null);
                throw;
            }
        }
        #endregion GRAPH COUNT
        #region Status Change
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 11-Nov-2019
        /// Created For : TO get vacant job title
        /// </summary>
        /// <param name="VSC_Id"></param>
        /// <returns></returns>
        public List<JobTitleModel> GetJobTitleVacantList(long VSC_Id)
        {
            var jobTitle = new List<JobTitleModel>();
            var repository = new ePeopleRepository();
            try
            {
                if(VSC_Id> 0)
                {
                    jobTitle =  repository.GetJobTitleVacant(VSC_Id).Select(x => new JobTitleModel() {
                        JobTitle = x.JBT_JobTitle,
                        JobTitleId = x.JBT_Id
                    }).ToList();
                }
                return jobTitle;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<JobTitleModel> GetJobTitleVacantList(long VSC_Id)", "Exception While getting Job title list.", VSC_Id);
                throw;
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 11-Nov-2019
        /// Created For : To save demotion promotion
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public bool SaveCommonStatusOfEmployee(DemotionModel Obj)
        {
            bool isSaved = false;
            var repository = new ePeopleRepository();
            var _workorderEMS = new workorderEMSEntities();
            try
            {
                if(Obj != null)
                {
                    var getEmpDetails = _workorderEMS.UserRegistrations.Where(x => x.UserId == Obj.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    if (getEmpDetails != null)
                    {
                        Obj.CreatedBy = getEmpDetails.EmployeeID;
                        Obj.FromDate = Obj.EffectiveDate;
                        if (Obj.StatusAction == EmployeeStatusChnage.L)
                        {        
                            Obj.ChangeType = "Location Change";
                            Obj.JobTitleCurrent = null;
                            Obj.Action = "I";
                            Obj.JobTitleId = null;
                            Obj.ToDate = Obj.EffectiveDate.Value.AddDays(Convert.ToDouble(Obj.TempDays));
                            var saveLocationTransfer = repository.SaveEmployeeStatus(Obj);
                        }
                        else if (Obj.StatusAction == EmployeeStatusChnage.S)
                        {
                            Obj.ChangeType = "Status Change";
                            Obj.Action = "I";
                            Obj.LocationIdCurrent = null;
                            Obj.JobTitleCurrent = null;
                            Obj.JobTitleId = null;
                            var saveEmployeeStatus = repository.SaveEmployeeStatus(Obj);
                        }
                        else
                        {
                            Obj.ChangeType = "Promotion/Demotion";
                            Obj.Action = "I";
                            Obj.LocationIdCurrent = null;
                            Obj.EmployeeCurrentStatus = null;
                            var saveDemotion = repository.SaveEmployeeStatus(Obj);
                        }
                    }
                    isSaved = true;
                }
                else
                {
                    isSaved = false;
                }
                return isSaved;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SavePromoDemo(DemotionModel Obj)", "Exception While saving promotion demotion.", Obj);
                throw;
            }
        }
        /// <summary>
        /// Created By :Ashwajit Bansod
        /// Created Date : 14-Nov-2019
        /// Created For : To get Employee status List
        /// </summary>
        /// <returns></returns>
        public List<EmployeeStatusList> GetEmployeeStatusList()
        {
            var repository = new ePeopleRepository();
            var lst = new List<EmployeeStatusList>();
            try
            {
                lst = repository.GetEmployeeStatusList().Select(x => new EmployeeStatusList() {
                   ESC_ApprovalStatus= x.ESC_ApprovalStatus == null ? "Not Approved": x.ESC_ApprovalStatus == "A" ? "Approved" :"Reject",
                   ESC_ApprovedBy = x.ESC_ApprovedBy == null?"N/A": x.ESC_ApprovedBy,
                   ESC_ChangeType= x.ESC_ChangeType,
                   ESC_Date= x.ESC_Date.Value.ToString("MM/dd/yyyy"),
                   ESC_EMP_EmployeeId= x.ESC_EMP_EmployeeId,
                   ESC_Id = x.ESC_Id
                }).ToList();
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<EmployeeStatusList> GetEmployeeStatusList()", "Exception While getting list of Employee Status.", null);
                throw;
            }
            return lst;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 15-Nov-2019
        /// Created For  : To approve reject employee status
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Status"></param>
        /// <param name="UserId"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public bool ApproveRejectEmployeeStatus(long Id, string Status, long UserId, string Comment)
        {
            var _workorderEMS = new workorderEMSEntities();
            bool IsApproveReject = false;
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                if (UserId > 0)
                {
                    var getEmpDetails = _workorderEMS.UserRegistrations.Where(x => x.UserId == UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                    if (getEmpDetails != null)
                    {
                        if (Id > 0 && Status != null)
                        {
                            var Statusdata = ePeopleRepository.ApproveRejectEmployeeReject(Id, Status, getEmpDetails.EmployeeID, Comment);
                            IsApproveReject = true;
                        }
                        else
                        {
                            IsApproveReject = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ApproveRejectEmployeeStatus(long Id, string Status, long UserId, string Comment)", "Exception While approve reject employee status", Id);
                throw;
            }
            return IsApproveReject;
        }
        //public bool SendForAssessment(string Status, string IsActive, long ApplicantId)
        //{
        //    var ePeopleRepository = new ePeopleRepository();
        //    try
        //    {
        //        var sendForAssessment = ePeopleRepository.SendForAssessment(Status, IsActive, ApplicantId);
        //    }
        //    catch(Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool SendForAssessment(string Status, string IsActive, long ApplicantId)", "Exception While sending assessment", ApplicantId);
        //        throw;
        //    }
        //}
        #endregion Status Change
    }
}
