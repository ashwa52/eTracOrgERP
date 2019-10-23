using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class ManagerAppeMaintenanceManager : IManagerAppeMaintenance
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string WorkOrderImagePath = ConfigurationManager.AppSettings["WorkOrderImage"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        public List<UnAssignedWorkOrderModel> UnassignedWorkOrderList(ManagerAppModel objManagerAppModel)
        {
            
               workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
               var listUnAssignedWorkOrder = new List<UnAssignedWorkOrderModel>();
               try
               {
                   long UnAssignedId = Convert.ToInt64(DashboardWidget.UnAssignedWorkOrder);
                   long PriorityLevel = Convert.ToInt64(WorkRequestPriority.Level1Urgent);
                   var userData = _workorderEMSEntities.UserRegistrations.Where(x => x.ServiceAuthKey == objManagerAppModel.ServiceAuthKey
                                                                               && x.IsDeleted == false).FirstOrDefault();
                   if (userData != null)
                   {
                       listUnAssignedWorkOrder = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.LocationID == objManagerAppModel.LocationId
                                                                                                   && x.IsDeleted == false 
                                                                                                   && x.AssignToUserId == null
                                                                                                   && x.PriorityLevel != PriorityLevel).Select(a => new UnAssignedWorkOrderModel
                                                                                                   {
                                                                                                       LocationId = a.LocationID,
                                                                                                       Description = a.ProblemDesc,
                                                                                                       PriorityLevel = a.GlobalCode.CodeName,
                                                                                                       WorkOrderCode = a.WorkOrderCode+ a.WorkOrderCodeID,
                                                                                                       WorkOrderCodeID = a.WorkOrderCodeID,
                                                                                                       WorkRequestImage = a.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.WorkRequestImage,
                                                                                                       WorkRequestId = a.WorkRequestAssignmentID,
                                                                                                       WorkRequestType = a.WorkRequestType
                                                                                                   }).ToList();                
                   }               
               }
               catch(Exception ex)
               {
                   Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UnAssignedWorkOrderModel> UnassignedWorkOrderList(ManagerAppModel objManagerAppModel)", "Exception While Listing UnAssigned Work Order.", objManagerAppModel.LocationId);
                   throw;
               }
               return listUnAssignedWorkOrder;
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date: Aug-13-2018
        /// Created For : To fetch Employee Data from User Registration Table
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        public List<EmployeeListModel> EmployeeList(ManagerAppModel objManagerAppModel)
        {

            workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
            var listEmployee = new List<EmployeeListModel>();
            try
            {
                long UnAssignedId = Convert.ToInt64(DashboardWidget.UnAssignedWorkOrder);
                long PriorityLevel = Convert.ToInt64(WorkRequestPriority.Level1Urgent);
                var empManager = new EmployeeManager();
                var userData = _workorderEMSEntities.UserRegistrations.Where(x => x.ServiceAuthKey == objManagerAppModel.ServiceAuthKey
                                                                            && x.IsDeleted == false).FirstOrDefault();
                if (userData != null)
                {
                    //var UserInRole = _workorderEMSEntities.UserRegistrations.
                    //Join(_workorderEMSEntities.AdminLocationMappings, u => u.UserId, uir => uir.AdminUserId,
                    //(u, uir) => new { u, uir }).
                    //Join(_workorderEMSEntities.LoginLogs, r => r.uir.AdminUserId, ro => ro.UserID, (r, ro) => new { r, ro })
                    //.Where(m => m.r.u.UserType == 3
                    //       && m.r.u.IsEmailVerify == true
                    //       && m.r.u.IsDeleted == false
                    //       && m.r.u.IsLoginActive == true
                    //       && m.r.uir.LocationId == objManagerAppModel.LocationId
                    //       && m.ro.IsActive == true)
                    //.Select(ls => new EmployeeListModel()
                    //{
                    //    EmployeeId = ls.r.u.EmployeeID,
                    //    LocationId = ls.r.uir.LocationId,
                    //    ProfileImage = ls.r.u.ProfileImage,
                    //    UserId = ls.r.u.UserId,
                    //    UserName = ls.r.u.FirstName + " " + ls.r.u.LastName
                    //}).ToList();
                    EmployeeLocationMappingRepository obj_EmployeeLocationMappingRepository = new EmployeeLocationMappingRepository();
                    listEmployee = obj_EmployeeLocationMappingRepository.GetActiveEmployeeByLocDetailed(objManagerAppModel.LocationId)
                        .Select(ls => new EmployeeListModel()
                        {
                             EmployeeId = ls.EmployeeID,
                             LocationId = objManagerAppModel.LocationId,
                            ProfileImage = ls.ProfileImage == null ? HostingPrefix + ProfileImagePath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfileImagePath.Replace("~", "") + ls.ProfileImage,
                            UserId = ls.UserId,
                             UserName = ls.FirstName+" "+ls.LastName
                        }).ToList();
         
                    //         listEmployee = empManager.GetEmployeeByLocDetailed(objManagerAppModel.LocationId)
                    //.Select( ls => new EmployeeListModel()
                    //{
                    //    EmployeeId  = ls.EmployeeID,
                    //    LocationId = ls.LocationId,
                    //    ProfileImage = ls.ProfileImage,
                    //    UserId = ls.UserId,
                    //    UserName = ls.FirstName + " " + ls.LastName
                    //}).ToList();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<EmployeeListModel> EmployeeList(ManagerAppModel objManagerAppModel)", "Exception While Listing Employee For Assigning Work order.", objManagerAppModel.LocationId);
                throw;
            }
            return listEmployee;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-13-2018
        /// Created For : To save assigned employee to workrequestassignment table 
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        public ServiceResponseModel<string> AssignedEmployee(ManagerAppModel objManagerAppModel)
        {
            var objReturnModel = new ServiceResponseModel<string>();
            WorkRequestAssignmentRepository objWorkRequestAssignmentRepository = new WorkRequestAssignmentRepository();
            var obj = new WorkRequestAssignment();
            workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
            try
            {
                var userData = _workorderEMSEntities.UserRegistrations.Where(x => x.ServiceAuthKey == objManagerAppModel.ServiceAuthKey
                                                                            && x.IsDeleted == false).FirstOrDefault();
                if (userData != null)
                {
                    var workOrderData = objWorkRequestAssignmentRepository.GetAll(w => w.IsDeleted == false 
                                                                          && w.WorkOrderCodeID == objManagerAppModel.WorkRequestCodeId
                                                                          && w.LocationID == objManagerAppModel.LocationId
                                                                          ).FirstOrDefault();

                    if (objManagerAppModel.AssignedTimeInterval != "")
                    {
                        workOrderData.AssignedTime = Convert.ToDateTime(objManagerAppModel.AssignedTimeInterval);
                    }

                    workOrderData.AssignByUserId = userData.UserId;
                    workOrderData.AssignToUserId = objManagerAppModel.UserId;
                    //workOrderData.AssignedTime = objManagerAppModel.AssignedTime.Value.ConvertClientTZtoUTC();
                    workOrderData.ModifiedBy = userData.UserId;
                    workOrderData.ModifiedDate = DateTime.UtcNow;
                    objWorkRequestAssignmentRepository.Update(workOrderData);
                    objWorkRequestAssignmentRepository.SaveChanges();
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.SuccessfullyAssigned();
                }
                else
                {
                    objReturnModel.Response = Convert.ToInt32(ServiceResponse.NoRecord, CultureInfo.InvariantCulture);
                    objReturnModel.Message = CommonMessage.FailureMessage();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UnAssignedWorkOrderModel> UnassignedWorkOrderList(ManagerAppModel objManagerAppModel)", "Exception While Listing UnAssigned Work Order.", objManagerAppModel.LocationId);
                throw;
            }
            return objReturnModel;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-17-2018
        /// Created For : To Get Dashboard Count 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ServiceResponseModel<DashboardCountModel> GetCountOfDashboardForManager(ManagerAppModel obj)
        {
            var ObjServiceResponseModel = new ServiceResponseModel<DashboardCountModel>();
            var result = new DashboardCountModel();
            var objCommonRepository = new CommonRepository();
            var ObjUserRepository = new UserRepository();
            try
            {
                var authuser = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == obj.ServiceAuthKey && x.IsDeleted == false);
                if (authuser != null && authuser.UserId > 0)
                {
                    result = objCommonRepository.DashboardCountForManager(authuser.UserId, obj.LocationId);
                    if (result != null)
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.SuccessResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        ObjServiceResponseModel.Message = CommonMessage.Successful();
                    }
                    else
                    {
                        ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.ExeptionResponse, CultureInfo.CurrentCulture);
                        ObjServiceResponseModel.Data = result;
                        ObjServiceResponseModel.Message = CommonMessage.WrongParameterMessage();
                    }
                }
                else
                {
                    ObjServiceResponseModel.Response = Convert.ToInt32(ServiceResponse.InvalidSessionResponse, CultureInfo.CurrentCulture);
                    ObjServiceResponseModel.Message = CommonMessage.InvalidUser();
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "ServiceResponseModel<ServiceDashboardModel> GetCountforDashboard(ServiceDashboardModel obj)", "While get count for dashboard", obj);
                throw ex;
            }
            return ObjServiceResponseModel;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Aug-27-2018
        /// Created To get All continues request.
        /// </summary>
        /// <param name="objManagerAppModel"></param>
        /// <returns></returns>
        public List<UnAssignedWorkOrderModel> AllContinuesWorkOrderList(ManagerAppModel objManagerAppModel)
        {

            workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
            var listOfCompletedContinuesWotkOreder = new List<UnAssignedWorkOrderModel>();
            var listOfInProgressContinuesWotkOreder = new List<UnAssignedWorkOrderModel>();
            var listOfMissedContinuesWotkOreder = new List<UnAssignedWorkOrderModel>();
            var listOfPendingContinuesWotkOreder = new List<UnAssignedWorkOrderModel>();
            var commonList = new List<UnAssignedWorkOrderModel>();
            try
            {
                var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
                //flag status for if user filter record in time span so to date is till midnight. 
                bool isUTCDay = true;
                DateTime _fromDate = objManagerAppModel.FromDate ?? clientdt.Date;
                DateTime _toDate = objManagerAppModel.ToDate ?? clientdt.AddDays(1).Date;

                //maintaining flag  if interval date come then need to fetch record till midnight of todate day
                if (objManagerAppModel.ToDate != null)
                {
                    if (objManagerAppModel.ToDate.Value.ToLongTimeString() == "12:00:00 AM")
                        isUTCDay = false;
                }
                if (_fromDate != null && _toDate != null)
                {
                    ////if interval date come then need to fetch record till midnight of todate day
                    if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                    {
                        _toDate = _toDate.AddDays(1).Date;
                    }
                    if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                    {
                        _toDate = _toDate.AddDays(1).Date;
                    }
                }
                long UnAssignedId = Convert.ToInt64(DashboardWidget.UnAssignedWorkOrder);
                long Completed = Convert.ToInt64(WorkRequestStatus1.Complete);
                long InProgress = Convert.ToInt64(WorkRequestStatus1.InProgress);
                long Missed = Convert.ToInt64(WorkRequestStatus1.Missed);
                long ContinuesWO = Convert.ToInt64(WorkRequestProjectType.ContinuousRequest);
                long PendingWO = Convert.ToInt64(WorkRequestStatus1.Pending);
                TimeSpan TodayTime = DateTime.Now.TimeOfDay;
                var userData = _workorderEMSEntities.UserRegistrations.Where(x => x.ServiceAuthKey == objManagerAppModel.ServiceAuthKey
                                                                            && x.IsDeleted == false).FirstOrDefault();
                if (userData != null)
                {
                    var PendingWorkOrder = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                                                      Where(x => x.q.LocationID == objManagerAppModel.LocationId
                                                                      && x.q.IsDeleted == false
                                                                      && x.q.WorkRequestStatus == PendingWO
                                                                      && x.q.WorkRequestProjectType == ContinuesWO
                                                                      && (x.q.StartDate <= _fromDate && x.q.EndDate >= _fromDate)).ToList();

                    listOfPendingContinuesWotkOreder = PendingWorkOrder.Select(a => new UnAssignedWorkOrderModel
                    {
                        LocationId = a.q.LocationID,
                        Description = a.q.ProjectDesc,
                        PriorityLevel = a.q.GlobalCode.CodeName,
                        WorkOrderCode = a.q.WorkOrderCode + a.q.WorkOrderCodeID,
                        WorkOrderCodeID = a.q.WorkOrderCodeID,
                        WorkRequestImage = a.q.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.q.WorkRequestImage,
                        WorkRequestId = a.q.WorkRequestAssignmentID,
                        WorkRequestType = a.q.WorkRequestType,
                        WorkOrderStatus = a.q.WorkRequestStatus,
                        UserName = a.u.FirstName + " " + a.u.LastName,
                        StartTime = a.q.ConStartTime.Value.GetClientDateTimeForMobileNow(objManagerAppModel.TimeZoneName).ToString("hh:mm tt"),
                        LocationName = a.q.LocationMaster.LocationName
                        
                    }).ToList();

                    var CompleteWO =  _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                                                      Where(x => x.q.LocationID == objManagerAppModel.LocationId
                                                                      && x.q.IsDeleted == false
                                                                      && x.q.WorkRequestStatus == Completed
                                                                      && x.q.WorkRequestProjectType == ContinuesWO
                                                                      && (x.q.StartDate <= _fromDate && x.q.EndDate >= _fromDate)).ToList();
                    listOfCompletedContinuesWotkOreder = CompleteWO.Select(a => new UnAssignedWorkOrderModel
                                                                    {
                                                                       LocationId = a.q.LocationID,
                                                                       Description = a.q.ProjectDesc,
                                                                       PriorityLevel = a.q.GlobalCode.CodeName,
                                                                       WorkOrderCode = a.q.WorkOrderCode + a.q.WorkOrderCodeID,
                                                                       WorkOrderCodeID = a.q.WorkOrderCodeID,
                                                                       WorkRequestImage = a.q.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.q.WorkRequestImage,
                                                                       WorkRequestId = a.q.WorkRequestAssignmentID,
                                                                       WorkRequestType = a.q.WorkRequestType,
                                                                       WorkOrderStatus = a.q.WorkRequestStatus,
                                                                       UserName = a.u.FirstName + " " + a.u.LastName,
                                                                       StartTime = a.q.ConStartTime.Value.GetClientDateTimeForMobileNow(objManagerAppModel.TimeZoneName).ToString("hh:mm tt")   ,
                                                                       LocationName = a.q.LocationMaster.LocationName
                    }).ToList();
                    var InProgressWO = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                                                      Where(x => x.q.LocationID == objManagerAppModel.LocationId
                                                                      && x.q.IsDeleted == false
                                                                      && x.q.WorkRequestStatus == InProgress
                                                                      && x.q.WorkRequestProjectType == ContinuesWO
                                                                      && (x.q.StartDate <= _fromDate && x.q.EndDate >= _fromDate)).ToList();
                    listOfInProgressContinuesWotkOreder = InProgressWO.Select(a => new UnAssignedWorkOrderModel
                                                                    {
                                                                       LocationId = a.q.LocationID,
                                                                       Description = a.q.ProjectDesc,
                                                                       PriorityLevel = a.q.GlobalCode.CodeName,
                                                                       WorkOrderCode = a.q.WorkOrderCode + a.q.WorkOrderCodeID,
                                                                       WorkOrderCodeID = a.q.WorkOrderCodeID,
                                                                       WorkRequestImage = a.q.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.q.WorkRequestImage,
                                                                       WorkRequestId = a.q.WorkRequestAssignmentID,
                                                                       WorkRequestType = a.q.WorkRequestType,
                                                                       WorkOrderStatus = a.q.WorkRequestStatus,
                                                                       UserName = a.u.FirstName + " " + a.u.LastName,
                                                                       StartTime = a.q.ConStartTime.Value.GetClientDateTimeForMobileNow(objManagerAppModel.TimeZoneName).ToString("hh:mm tt"),
                                                                       LocationName = a.q.LocationMaster.LocationName
                    }).ToList();
                    var MissedWO = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                                                     Where(x => x.q.LocationID == objManagerAppModel.LocationId
                                                                     && x.q.IsDeleted == false
                                                                     && x.q.WorkRequestStatus == Missed
                                                                     && x.q.WorkRequestProjectType == ContinuesWO
                                                                     && (x.q.StartDate <= _fromDate && x.q.EndDate >= _fromDate)).ToList();
                    listOfMissedContinuesWotkOreder = MissedWO.Select(a => new UnAssignedWorkOrderModel
                                                                                                {
                                                                     LocationId = a.q.LocationID,
                                                                     Description = a.q.ProjectDesc,
                                                                     PriorityLevel = a.q.GlobalCode.CodeName,
                                                                     WorkOrderCode = a.q.WorkOrderCode + a.q.WorkOrderCodeID,
                                                                     WorkOrderCodeID = a.q.WorkOrderCodeID,
                                                                     WorkRequestImage = a.q.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.q.WorkRequestImage,
                                                                     WorkRequestId = a.q.WorkRequestAssignmentID,
                                                                     WorkRequestType = a.q.WorkRequestType,
                                                                     WorkOrderStatus = a.q.WorkRequestStatus,
                                                                     UserName = a.u.FirstName + " " + a.u.LastName,
                                                                     StartTime = a.q.ConStartTime.Value.GetClientDateTimeForMobileNow(objManagerAppModel.TimeZoneName).ToString("hh:mm tt"),
                                                                     LocationName = a.q.LocationMaster.LocationName
                    }).ToList();
                    commonList = listOfCompletedContinuesWotkOreder.Union(listOfInProgressContinuesWotkOreder).Union(listOfMissedContinuesWotkOreder).Union(listOfPendingContinuesWotkOreder).Distinct().ToList();//Union(listOfPendingContinuesWotkOreder)
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UnAssignedWorkOrderModel> UnassignedWorkOrderList(ManagerAppModel objManagerAppModel)", "Exception While Listing UnAssigned Work Order.", objManagerAppModel.LocationId);
                throw;
            }
            return commonList;
        }
    }
}
