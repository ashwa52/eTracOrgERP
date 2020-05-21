﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
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
                            ProfilePhoto = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                            IsOrientation = x.OrientationStatus
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
                            UserModel.EmployeeCurrentStatus = selfData.EMP_EmploymentStatus;
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
        //public IEnumerable<UploadedFiles> GetUploadedFilesOfUser(string EmployeeId)
        //{
        //    var lst = new List<UploadedFiles>();
        //    var objworkorderEMSEntities = new workorderEMSEntities();
        //    try
        //    {
        //        var ePeopleRepository = new ePeopleRepository();
        //        if (EmployeeId != null)
        //        {
        //            var getFileList = ePeopleRepository.GetUploadFilesList(EmployeeId).ToList();
        //            if (getFileList.Count() > 0)
        //            {
        //                lst = getFileList.Select(x => new UploadedFiles()
        //                {
        //                    FileName = x.FLU_FileName == null ? null : x.FLU_FileName,
        //                    FileTypeName = x.FLT_FileType == null ? null : x.FLT_FileType,
        //                    AttachedFileName = x.FLU_FileAttached == null ? null : x.FLU_FileAttached,
        //                    FileId = x.FLU_FLT_Id > 0 ? 0 : x.FLU_FLT_Id,
        //                    AttachedFileLink = x.FLU_FileAttached == null ? null : HostingPrefix + FilePath.Replace("~", "") + x.FLU_FileAttached
        //                }).ToList();
        //            }
        //        }
        //        return lst;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public UploadedFiles GetUploadedFilesOfUser(string EmployeeId)", "Exception While getting list of files by user id.", EmployeeId);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-Oct-2019
        /// Created For : To get uploaded files data
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public List<UploadedFiles> GetUploadedFilesOfUserTesting(string EmployeeId)
        {
            var lst = new List<UploadedFiles>();
            var model = new UploadedFiles();
            var objworkorderEMSEntities = new workorderEMSEntities();
            try
            {
                var ePeopleRepository = new ePeopleRepository();
                if (EmployeeId != null)
                {
                    var getFileList = ePeopleRepository.GetUploadFilesListTesting(EmployeeId);
                    if (getFileList.Count() > 0)
                    {
                        //foreach (var item in getFileList)
                        //{
                        //    model.FileName = item.FLU_FileName;
                        //    model.FileTypeName = item.FLT_FileType;
                        //    model.AttachedFileName = item.FLU_FileAttached;
                        //    //model.FileId1 = Convert.ToInt32(item.FLU_FLT_Id);
                        //    model.FileId = item.FLU_FileId;
                        //    //model.FileId = item.FLU_FLT_Id;
                        //    model.AttachedFileLink = item.FLU_FileAttached == null ? null : HostingPrefix + FilePath.Replace("~", "") + item.FLU_FileAttached;
                        //    lst.Add(model);
                        //}
                        lst = getFileList.Select(x => new UploadedFiles()
                        {
                            FileName = x.FLU_FileName,
                            FileTypeName = x.FLT_FileType,
                            AttachedFileName = x.FLU_FileAttached,
                            //FileId = x.FLU_FLT_Id,
                            FileId = x.FLU_FileId,
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
        /// <summary>
        /// Created By : Ashwajit Bnasod
        /// Created Date : 18-Nov-2019
        /// Created For : To send backgroud check for approve reject via mail.
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="IsActive"></param>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public bool SendForAssessment(string Status, string IsActive, long ApplicantId, long UserId)
        {
            var ePeopleRepository = new ePeopleRepository();
            bool isSend = false;
            try
            {
                var sendForAssessment = ePeopleRepository.SendForAssessment(Status, IsActive, ApplicantId);
                isSend = true;
                #region Email
                if (ApplicantId > 0)
                {
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    if (isSend == true)
                    {
                        bool IsSent = false;
                        var objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = "assessment.check@gmail.com";
                        //objEmailHelper.AcceptAssessmentLink = HostingPrefix + "api/ServiceApi/GetAssessmentList?ApplicantId="+ ApplicantId+"&";
                        objEmailHelper.AcceptAssessmentLink = HostingPrefix + "GetMailData/GetAssessmentStatus?ApplicantId=" + ApplicantId + "&Status="+"Y";
                        //objEmailHelper.InfractionStatus = obj.Status;
                        objEmailHelper.MailType = "SENDFORASSESSMENT";
                        objEmailHelper.SentBy = UserId;
                        objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                        IsSent = objEmailHelper.SendEmailWithTemplate();

                        //if (IsSent == true)
                        //{
                        //    var objEmailog = new EmailLog();
                        //    try
                        //    {
                        //        objEmailog.CreatedBy = userData.u.UserId;
                        //        objEmailog.CreatedDate = DateTime.UtcNow;
                        //        objEmailog.DeletedBy = null;
                        //        objEmailog.DeletedOn = null;
                        //        objEmailog.LocationId = userData.q.LPOD_LocationId;
                        //        objEmailog.ModifiedBy = null;
                        //        objEmailog.ModifiedOn = null;
                        //        objEmailog.SentBy = userData.u.UserId;
                        //        objEmailog.SentEmail = getRuleData.Email;
                        //        objEmailog.Subject = objEmailHelper.Subject;
                        //        objEmailog.SentTo = getRuleData.UserId;
                        //        objListEmailog.Add(objEmailog);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        throw;
                        //    }
                        //}

                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges();
                        }
                        #endregion Email
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool SendForAssessment(string Status, string IsActive, long ApplicantId)", "Exception While sending assessment", ApplicantId);
                throw;
            }
            return isSend;
        }
        /// <summary>
        /// Created By : Ashwajit Bnasod
        /// Created Date : 18-Nov-2019
        /// Created For : To send assessment for approve reject via mail.
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="IsActive"></param>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public bool SendForBackgroundCheck(string Status, string IsActive, long ApplicantId, long UserId)
        {
            var ePeopleRepository = new ePeopleRepository();
            var _workorderems = new workorderEMSEntities();
            bool isSend = false;
            try
            {
                var sendForBackgroundCheck = ePeopleRepository.SendForAssessment(Status, IsActive, ApplicantId);
                isSend = true;
                #region Email
                if (ApplicantId > 0)
                {
                    var getEMPData = _workorderems.ApplicantInfoes.Where(x => x.API_ApplicantId == ApplicantId).FirstOrDefault();
                    var objEmailLogRepository = new EmailLogRepository();
                    var objEmailReturn = new List<EmailToManagerModel>();
                    var objListEmailog = new List<EmailLog>();
                    var objTemplateModel = new TemplateModel();
                    if (isSend == true)
                    {
                        bool IsSent = false;
                        var objEmailHelper = new EmailHelper();
                        objEmailHelper.emailid = "background360.check@gmail.com";
                        objEmailHelper.Name = getEMPData.API_FirstName + " " + getEMPData.API_LastName;
                        //objEmailHelper.AcceptAssessmentLink = HostingPrefix + "api/ServiceApi/GetAssessmentList?ApplicantId="+ ApplicantId+"&";
                        objEmailHelper.AcceptAssessmentLink = HostingPrefix + "GetMailData/GetBackGroundStatus?ApplicantId=" + ApplicantId + "&Status=" + "Y";
                        //objEmailHelper.InfractionStatus = obj.Status;
                        objEmailHelper.MailType = "SENDFORBACKGROUNDCHECK";
                        objEmailHelper.SentBy = UserId;
                        //objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                        IsSent = objEmailHelper.SendEmailWithTemplate();

                        //if (IsSent == true)
                        //{
                        //    var objEmailog = new EmailLog();
                        //    try
                        //    {
                        //        objEmailog.CreatedBy = userData.u.UserId;
                        //        objEmailog.CreatedDate = DateTime.UtcNow;
                        //        objEmailog.DeletedBy = null;
                        //        objEmailog.DeletedOn = null;
                        //        objEmailog.LocationId = userData.q.LPOD_LocationId;
                        //        objEmailog.ModifiedBy = null;
                        //        objEmailog.ModifiedOn = null;
                        //        objEmailog.SentBy = userData.u.UserId;
                        //        objEmailog.SentEmail = getRuleData.Email;
                        //        objEmailog.Subject = objEmailHelper.Subject;
                        //        objEmailog.SentTo = getRuleData.UserId;
                        //        objListEmailog.Add(objEmailog);
                        //    }
                        //    catch (Exception)
                        //    {
                        //        throw;
                        //    }
                        //}

                        using (var context = new workorderEMSEntities())
                        {
                            context.EmailLogs.AddRange(objListEmailog);
                            context.SaveChanges();
                        }
                        #endregion Email
                    }
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "bool SendForAssessment(string Status, string IsActive, long ApplicantId)", "Exception While sending assessment", ApplicantId);
                throw;
            }
            return isSend;
        }
        #endregion Status Change
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To Make assessmennt , offer accept or active or deny, send mail when offer is accepted or counter
        /// Created Date : 20-03-2020
        /// </summary>
        /// <param name="IsActive"></param>
        /// <param name="ActionVal"></param>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public bool ClearedOrNot(string IsActive, string ActionVal, long ApplicantId)
        {
            bool isCleared = false;
            var ePeopleRepository = new ePeopleRepository();
            var _ICommonMethod = new CommonMethodManager();
            string employeeID = string.Empty;
            string Status = string.Empty;
            try
            {
                if (IsActive != null && ActionVal != null && ApplicantId > 0)
                {
                    if (ActionVal == "Assessment")
                    {
                        if(IsActive == "Y")
                        Status = IsActive == "Y" ? ApplicantStatus.Hired : ApplicantIsActiveStatus.Pass;
                        else
                            Status = IsActive == "N" ? ApplicantStatus.Assessment : ApplicantIsActiveStatus.Fail;
                        var isSaved = ePeopleRepository.SendForAssessment(Status, IsActive, ApplicantId);
                        var getApplicantDetails =  objworkorderEMSEntities.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
                        string message = IsActive == "N" ? DarMessage.AssessmentReject(getApplicantDetails.JBT_JobTitle) : DarMessage.AssessmentClear(getApplicantDetails.JBT_JobTitle);
                        var saveNotification = objworkorderEMSEntities.spSetNotification("I", null, message,
                                                       ModuleSubModule.ePeople, ModuleSubModule.AssessmentStatus, ApplicantId.ToString(), getApplicantDetails.HiringManagerEmployeeId, getApplicantDetails.HiringManagerEmployeeId, true, false, Priority.Medium, null, false, "Y");
                    }
                    else if (ActionVal == "Offer")
                    {
                        var isSaved = ePeopleRepository.SendForAssessment(ApplicantStatus.Offer, IsActive, ApplicantId);
                        
                        #region Email
                        var getEMPData = objworkorderEMSEntities.spGetApplicantAllDetails(ApplicantId).FirstOrDefault();
                        var getLocationCode = objworkorderEMSEntities.LocationMasters.Where(x => x.LocationId == getEMPData.LocationId).FirstOrDefault().Address2.Substring(0,3).ToUpper();
                        var getLastEmp_Id = objworkorderEMSEntities.Employees.OrderByDescending(x => x.EMP_Id).FirstOrDefault().EMP_Id;
                        if (getEMPData != null && getLocationCode != null && getLastEmp_Id > 0)
                        {
                            if (IsActive == ApplicantIsActiveStatus.OfferAccepted)
                            {
                                //Make Employee by last Id and Location code
                                var value = getLastEmp_Id + 1;
                                employeeID = getLocationCode + "000" + value;
                                var saveEployee = objworkorderEMSEntities.spSetEmployee("I", null, employeeID, ApplicantId, getEMPData.API_FirstName, getEMPData.API_MidName,
                                                                          getEMPData.API_LastName, getEMPData.ACI_eMail, getEMPData.ACI_PhoneNo, null, null,null, getEMPData.ALA_Photo, null, 9,
                                                                          null, getEMPData.HiringManagerEmployeeId, getEMPData.APT_DateOfJoining, getEMPData.LocationId,
                                                                          getEMPData.UserId, DateTime.Now, "Y", Convert.ToInt64(UserType.GuestUser), null, null, null, null, null);
                                
                            }
                            string applicantName = getEMPData.API_FirstName + getEMPData.API_LastName;
                            string message = IsActive == ApplicantIsActiveStatus.OfferAccepted ? DarMessage.AddAssetsForHiredApplicant(applicantName, getEMPData.HiringManagerName, getEMPData.LocationName) : IsActive == "C" ? DarMessage.OfferCouterByAppicant(applicantName, getEMPData.JBT_JobTitle, getEMPData.LocationName) : DarMessage.OfferRejectByAppicant(applicantName, getEMPData.JBT_JobTitle, getEMPData.LocationName);
                            var saveNotification = objworkorderEMSEntities.spSetNotification("I", null, message,
                                                        "ePeople", ModuleSubModule.OnBoarding, ApplicantId.ToString(), getEMPData.HiringManagerEmployeeId, getEMPData.HiringManagerEmployeeId, true, false, "H", null, false, "Y");
                            if (ApplicantId > 0 && IsActive == ApplicantIsActiveStatus.OfferAccepted)
                            {
                                //var objEmailLogRepository = new EmailLogRepository();
                                var objEmailReturn = new List<EmailToManagerModel>();
                                var objListEmailog = new List<EmailLog>();
                                var objTemplateModel = new TemplateModel();
                                if (getEMPData != null)
                                {
                                    bool IsSent = false;
                                    var objEmailHelper = new EmailHelper();
                                    objEmailHelper.emailid = getEMPData.ACI_eMail;
                                    objEmailHelper.Name = getEMPData.API_FirstName + " "+ getEMPData.API_LastName;
                                    //By Default password will be this will change it if requirment changes
                                    objEmailHelper.Password = "Elite@123";
                                    objEmailHelper.UserName = employeeID;
                                    objEmailHelper.emailid = getEMPData.ACI_eMail;
                                    objEmailHelper.JobTitle = getEMPData.JBT_JobTitle;
                                    //objEmailHelper.AcceptAssessmentLink = HostingPrefix + "api/ServiceApi/GetAssessmentList?ApplicantId="+ ApplicantId+"&";
                                    objEmailHelper.AcceptAssessmentLink = HostingPrefix + "GetMailData/LoginForOnboarding?ApplicantId=" + ApplicantId;
                                    objEmailHelper.MailType = IsActive == ApplicantIsActiveStatus.OfferAccepted ? "OFFERACCEPTED": IsActive == "C"?"OFFERCOUNTER":"OFFERREJECTED";
                                    objEmailHelper.Subject = IsActive == ApplicantIsActiveStatus.OfferAccepted ? "eTrac : Thanks for accepting offer letter": IsActive == "C"? "eTrac : Thanks for Countering offer" :"eTrac : Offer Rejected";
                                    objEmailHelper.SentBy = Convert.ToInt64(getEMPData.UserId);
                                    objEmailHelper.TimeAttempted = DateTime.UtcNow.ToMobileClientTimeZone(objTemplateModel.TimeZoneName, objTemplateModel.TimeZoneOffset, objTemplateModel.IsTimeZoneinDaylight, false).ToString();
                                    IsSent = objEmailHelper.SendEmailWithTemplate();
                                    if (IsSent == true)
                                    {
                                        var objEmailog = new EmailLog();
                                        try
                                        {
                                            objEmailog.CreatedBy = Convert.ToInt64(getEMPData.UserId);
                                            objEmailog.CreatedDate = DateTime.UtcNow;
                                            objEmailog.DeletedBy = null;
                                            objEmailog.DeletedOn = null;
                                            objEmailog.LocationId = getEMPData.LocationId;
                                            objEmailog.ModifiedBy = null;
                                            objEmailog.ModifiedOn = null;
                                            objEmailog.SentBy = getEMPData.UserId;
                                            objEmailog.SentEmail = getEMPData.ACI_eMail;
                                            objEmailog.Subject = objEmailHelper.Subject;
                                            objEmailog.SentTo = ApplicantId;
                                            objListEmailog.Add(objEmailog);
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                    using (var context = new workorderEMSEntities())
                                    {
                                        context.EmailLogs.AddRange(objListEmailog);
                                        context.SaveChanges();
                                    }
                                    #endregion Email
                                }
                            }
                        }
                    }
                    //if(ActionVal == "Background")
                    //{
                    //    Status = "F";
                    //}
                    //else
                    //{
                    //    Status = "E";
                    //}
                    
                    isCleared = true;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool ClearedOrNot(string IsActive, string ActionVal, long ApplicantId)", "Exception While cleared or not clear employee", ApplicantId);
                throw;
            }
            return isCleared;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-March-2020
        /// Created For : To schedule interview for applicant
        /// </summary>
        /// <param name="IPT_Id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void ScheduleInterviewOfApplicant(long IPT_Id, string status)
        {
            var common_B = new Common_B();
            if (IPT_Id > 0 && status != null)
            {
                objworkorderEMSEntities.spSetScheduleInterview_ForMailFromApplicant(IPT_Id, status);
                var getDetails = objworkorderEMSEntities.InterviewProposalTimes.Where(x => x.IPT_Id == IPT_Id).
                    Select(a => new ApplicantDetails()
                    {
                        JobId = a.IPT_JPS_JobPostingId,
                        ApplicantId = a.IPT_APT_ApplicantId,
                        HiringManagerId = a.IPT_EMP_EmployeeID_HM,
                        IPT_Id = a.IPT_Id
                    }).FirstOrDefault();
                var getJobDetails = objworkorderEMSEntities.spGetMyOpening(getDetails.JobId).FirstOrDefault().JBT_JobTitle;
                var message = status == "A"?  DarMessage.InterviewAcceptByApplicant(getJobDetails,status == "A"?"Accept":"Reject"): DarMessage.InterviewDenyByApplicant(getJobDetails);
                var saveNotification = objworkorderEMSEntities.spSetNotification("I", null, message,
                                                        ModuleSubModule.ePeople, ModuleSubModule.InterviewerAcceptDeny, getDetails.IPT_Id.ToString(), getDetails.HiringManagerId, getDetails.HiringManagerId, true, false, Priority.Medium, null, false, "Y");
            }
        }

        /// <summary>
        /// Created y  :Ashwajit Bansod
        /// Created Date : 11-04-2020
        /// Created For : get benifit list
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public BenefitList GetBenifitList(long ApplicantId)
        {
            var clientUrl = new Helper.CommonHTTPClient();
            var convertedClass = new CommonAPIJsonConvertToClass<object>();
            if (ApplicantId > 0)
            {
                var objCommon = new CommonMethodManager();              
                //var getStringLogin = objCommon.GetJsoSerializeDataForAPI(APIName.FloridaBlueAuthentication, null);                
                var getOutputData = clientUrl.FloridaBlueAuthentication(APIName.FloridaBlueAuthenticationLink);
                var getClassData = Newtonsoft.Json.JsonConvert.DeserializeObject<FloridaBlueAuthentication>(getOutputData);
                //Florida blue benefit list
                var getListData = clientUrl.FloridaBluePost(APIName.FloridaBlueGetLink, getClassData.access_token);
                var getListDataModel = Newtonsoft.Json.JsonConvert.DeserializeObject<BenefitList>(getListData);
                return getListDataModel;
            }
            else
                return new BenefitList();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 13-05-2020
        /// Created For : To get Employee status
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public string GetApplicantStatus(long ApplicantId)
        {
            string Status = string.Empty;
            try
            {
                if (ApplicantId > 0)
                {
                    var getAptDetails = objworkorderEMSEntities.spGetMyOpening(null).Where(x => x.APT_ApplicantId == ApplicantId).FirstOrDefault();
                    if (getAptDetails.APT_Status != null)
                    {
                        Status = getAptDetails.APT_Status == "Applied" ? "Applied"
                             : getAptDetails.APT_Status == "Screened" ? "Screened"
                             : getAptDetails.APT_Status == "IntervieweSchedule" ? "Interview Schedule"
                             : getAptDetails.APT_Status == "InterviewCanceled" ? "Interview Canceled"
                             : getAptDetails.APT_Status == "Shortlisted" ? "Shortlisted"
                             : getAptDetails.APT_Status == "AssessmentSend" ? "Assessment Send"
                             : getAptDetails.APT_Status == "AssessmentPass" ? "Assessment Pass"
                             : getAptDetails.APT_Status == "AssessmentFail" ? "Assessment Fail"
                             : getAptDetails.APT_Status == "OnHold" ? "On Hold"
                             : getAptDetails.APT_Status == "Hired" ? "Hired"
                             : getAptDetails.APT_Status == "OfferSent" ? "Offer Sent"
                             : getAptDetails.APT_Status == "OfferAccepted" ? "Offer Accepted"
                             : getAptDetails.APT_Status == "OfferCountered" ? "Offer Countered"
                             : getAptDetails.APT_Status == "OfferDeclined" ? "Offer Declined"
                             : getAptDetails.APT_Status == "OfferCancled" ? "Offer Cancled"
                             : getAptDetails.APT_Status == "Onboarding" ? "On boarding"
                             : getAptDetails.APT_Status == "Onboarded" ? "On boarded"
                             : getAptDetails.APT_Status == "OnboardingDrop" ? "Onboarding Drop"
                             : getAptDetails.APT_Status == "BackgroundCheckSend" ? "Background Check Send"
                             : getAptDetails.APT_Status == "BackgroundCheckPass" ? "Background Check Pass"
                             : getAptDetails.APT_Status == "BackgroundCheckFail" ? "Background Check Fail"
                             : getAptDetails.APT_Status == "OrientationSchedule" ? "Orientation Schedule"
                             : getAptDetails.APT_Status == "OrientationDone" ? "Orientation Done"
                             : getAptDetails.APT_Status == "OrientationNotDone" ? "Orientation Not Done"
                             : "No Entry";
                             
                    }
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string GetEmployeeStatus(long ApplicantId)", "Exception While getting applicant status", ApplicantId);
                throw;
            }
            return Status;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14-05-2020
        /// Created For : To set job status by job id
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="JobStatus"></param>
        /// <returns></returns>
        public bool UpdateCloseHoldOpenJob(long JobId, string JobStatus)
        {
            bool isUpdate = false;
            try
            {
                if (JobId > 0 && JobStatus != null)
                {
                    var getJobDetails = objworkorderEMSEntities.JobPostings.Where(x => x.JPS_JobPostingId == JobId).FirstOrDefault();
                    var update = objworkorderEMSEntities.spSetJobPosting("U", JobId, getJobDetails.JPS_JobPostingIdRecruitee, getJobDetails.JPS_JobTitleID,
                        getJobDetails.JPS_HiringManagerID, getJobDetails.JPS_LocationId, getJobDetails.JPS_NumberOfPost, getJobDetails.JPS_DrivingType, JobStatus);
                    isUpdate = true;
                }
                else
                    isUpdate = false;
            }
            catch (Exception ex)
            {
                isUpdate = false;
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool UpdateCloseHoldOpenJob(long JobId, string JobStatus)", "Exception While updating job status", JobId);
                throw;
            }
            return isUpdate;
        }
    }
}
