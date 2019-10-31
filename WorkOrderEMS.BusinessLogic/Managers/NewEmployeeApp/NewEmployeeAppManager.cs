using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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

namespace WorkOrderEMS.BusinessLogic
{
    public class NewEmployeeAppManager : INewEmployeeAppManager
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string WorkOrderImagePath = ConfigurationManager.AppSettings["WorkOrderImage"];
        private string ProfileImagePath = ConfigurationManager.AppSettings["ProfilePicPath"];
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-Jan-2019
        /// Created For : To get employee task list by userId and Location Id
        /// </summary>
        /// <param name="objEmpManagerAppModel"></param>
        /// <returns></returns>
        public List<UnAssignedWorkOrderModel> TaskListForEmployee(EmployeeManagerModel objEmpManagerAppModel)
        {

            var listTaskForEmployee = new List<UnAssignedWorkOrderModel>();
            var ObjUserRepository = new UserRepository();
            try
            {
                long UnAssignedId = Convert.ToInt64(DashboardWidget.UnAssignedWorkOrder);
                long PriorityLevel = Convert.ToInt64(WorkRequestPriority.Level1Urgent);
                long ProjectType = Convert.ToInt64(WorkRequestProjectType.ContinuousRequest);
                var userData = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == objEmpManagerAppModel.ServiceAuthKey
                                                                            && x.IsDeleted == false);
                //var userData = _workorderEMSEntities.UserRegistrations.Where(x => x.ServiceAuthKey == objEmpManagerAppModel.ServiceAuthKey
                //                                                            && x.IsDeleted == false).FirstOrDefault();
                if (userData != null)
                {
                    listTaskForEmployee = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.LocationID == objEmpManagerAppModel.LocationId
                                                                                                && x.IsDeleted == false
                                                                                                && x.AssignToUserId == userData.UserId
                                                                                                && x.PriorityLevel != PriorityLevel 
                                                                                                && x.EndTime == null
                                                                                                && x.WorkRequestProjectType != ProjectType).Select(a => new UnAssignedWorkOrderModel
                                                                                                {
                                                                                                    LocationId = a.LocationID,
                                                                                                    LocationName = a.LocationMaster.LocationName,
                                                                                                    Description = a.ProblemDesc,
                                                                                                    PriorityLevel = a.GlobalCode.CodeName,
                                                                                                    WorkOrderCode = a.WorkOrderCode + a.WorkOrderCodeID,
                                                                                                    WorkOrderCodeID = a.WorkOrderCodeID,
                                                                                                    WorkRequestImage = a.AssignedWorkOrderImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.AssignedWorkOrderImage,
                                                                                                    WorkRequestId = a.WorkRequestAssignmentID,
                                                                                                    WorkRequestType = a.WorkRequestType,
                                                                                                    WorkOrderStatus = a.WorkRequestStatus,
                                                                                                    //WorkRequestProjectType = a.WorkRequestProjectType
                                                                                                }).OrderByDescending(x => x.WorkRequestId).ToList();
                    userData.Latitute = objEmpManagerAppModel.Lat;
                    userData.Longitude = objEmpManagerAppModel.Long;
                    userData.ModifiedBy = userData.UserId;
                    userData.ModifiedDate = DateTime.UtcNow;
                    ObjUserRepository.Update(userData);
                }
                else
                {
                    listTaskForEmployee = null;
                }
                ////var DeviceId = "cZ65vzu0q2c:APA91bGASmJfEv5GT4quc9vBMhsBTGxFj4rEbGKIy88Z3zAYc15l8jtewm2eiphKNu1UgwNhJKeukssj5grifXFzbpTxGhNNe0Nv75xsRj3j1mEUXD8j39iVBmCSewQgL7GvS07xGStQ";
                //EmailHelper objEmailHelper = new EmailHelper();
                //objEmailHelper.MailType = "EMAINTENANCE";
                //objEmailHelper.LocationID = objEmpManagerAppModel.LocationId;
                //objEmailHelper.LocationName = "JAX";
                //objEmailHelper.UserName = "Thomas Hardy";
                //if (userData.DeviceId != null)
                //{
                //    PushNotificationFCM.FCMAndroid("Testing Notification ", userData.DeviceId, objEmailHelper);
                //}
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UnAssignedWorkOrderModel> TaskListForEmployee(ManagerAppModel objManagerAppModel)", "Exception While getting list of task assign to employee.", objEmpManagerAppModel);
                throw;
            }
            return listTaskForEmployee;
        }

        public List<UnAssignedWorkOrderModel> FacilityTaskListForEmployee(EmployeeManagerModel objEmpManagerAppModel)
        {

            var listTaskForEmployee = new List<UnAssignedWorkOrderModel>();
            var ObjUserRepository = new UserRepository();
            try
            {
                long UnAssignedId = Convert.ToInt64(DashboardWidget.UnAssignedWorkOrder);
                long PriorityLevel = Convert.ToInt64(WorkRequestPriority.Level1Urgent);
                long ProjectType = Convert.ToInt64(WorkRequestProjectType.FacilityRequest);
                var userData = ObjUserRepository.GetSingleOrDefault(x => x.ServiceAuthKey == objEmpManagerAppModel.ServiceAuthKey
                                                                            && x.IsDeleted == false);
                if (userData != null)
                {
                    listTaskForEmployee = _workorderEMSEntities.WorkRequestAssignments
                        //.Join(_workorderEMSEntities.QRCMasters, z => z.UserType, gc => gc.GlobalCodeId, (z, gc) => new { z, gc })
                        .Where(x => x.LocationID == objEmpManagerAppModel.LocationId
                                                                                && x.IsDeleted == false
                                                                                && x.PriorityLevel == PriorityLevel
                                                                                && x.EndTime == null
                                                                                && x.AssignToUserId == userData.UserId
                                                                                && x.WorkRequestProjectType == ProjectType).Select(a => new UnAssignedWorkOrderModel
                                                                                {
                                                                                    WorkRequestId = a.WorkRequestAssignmentID,
                                                                                    AssetID = a.AssetID,
                                                                                    WorkRequestTypeName = a.GlobalCode.CodeName,
                                                                                    QRCName = a.GlobalCode.QRCMasters.Where(x => x.QRCID == a.AssetID).FirstOrDefault().QRCName,
                                                                                    WorkRequestType = a.WorkRequestType,
                                                                                    Description = a.ProblemDesc,
                                                                                    ProjectDescription = a.ProjectDesc,
                                                                                    WorkOrderStatus = a.WorkRequestStatus,
                                                                                    WorkRequestStatusName = a.WorkRequestStatus == 14 ? "Pending" : a.WorkRequestStatus == 15 ? "In Progress" : "Complete",
                                                                                    WorkRequestProjectType = a.WorkRequestProjectType,
                                                                                    WorkRequestProjectTypeName = "Facility Request",
                                                                                    PriorityLevel = a.PriorityLevel.ToString(),
                                                                                    SafetyHazard = a.SafetyHazard,
                                                                                    LocationId = a.LocationID,
                                                                                    LocationName = a.LocationMaster.LocationName,
                                                                                    AssignedByUserId = a.AssignByUserId,
                                                                                    RequestBy = a.RequestBy,
                                                                                    //RequestedName = a.
                                                                                    CreatedDate = a.CreatedDate.ToString(),
                                                                                    WorkRequestImage = a.WorkRequestImage == null ? HostingPrefix + WorkOrderImagePath.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkOrderImagePath.Replace("~", "") + a.WorkRequestImage,
                                                                                    WorkOrderCode = a.WorkOrderCode + a.WorkOrderCodeID.ToString(),
                                                                                    FrCurrentLocation = a.CurrentLocation,
                                                                                    CustomerContact = a.CustomerContact,
                                                                                    CustomerName = a.CustomerName,
                                                                                    DriverLicenseNo = a.DriverLicenseNo,
                                                                                    VehicleColor = a.VehicleColor,
                                                                                    VehicleMake1 = a.VehicleMake,
                                                                                    VehicleModel1 = a.VehicleModel,
                                                                                    VehicleYear = a.VehicleYear.ToString(),
                                                                                    AddressFacilityReq = a.Address,
                                                                                    LicensePlateNo = a.LicensePlateNo,
                                                                                    FacilityRequest = a.FacilityRequestId.ToString(),
                                                                                    UserType = userData.UserType,
                                                                                    AssignToUserId = a.AssignToUserId,
                                                                                    AssignByUserName = userData.FirstName + " " + userData.LastName
                                                                                }).OrderByDescending(x => x.WorkRequestId).ToList();
                    userData.Latitute = objEmpManagerAppModel.Lat;
                    userData.Longitude = objEmpManagerAppModel.Long;
                    userData.ModifiedBy = userData.UserId;
                    userData.ModifiedDate = DateTime.UtcNow;
                    ObjUserRepository.Update(userData);
                }
                else
                {
                    listTaskForEmployee = null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<UnAssignedWorkOrderModel> TaskListForEmployee(ManagerAppModel objManagerAppModel)", "Exception While getting list of task assign to employee.", objEmpManagerAppModel);
                throw;
            }
            return listTaskForEmployee;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<spGetMyOpening_Result1> GetMyOpenings()
		{
			return _workorderEMSEntities.spGetMyOpening().ToList();
		}
    }
}
