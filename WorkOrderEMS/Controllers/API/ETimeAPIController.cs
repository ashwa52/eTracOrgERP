using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.Controllers.Administrator;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Service;
using WorkOrderEMS.Models.Employee;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace WorkOrderEMS.Controllers
{
    public class eTimeAPIController : ApiController
    {
        DBUtilities DBU = new DBUtilities();
        ARService ARS = new ARService();
        public JsonResult API_DTLogin_DetailResp(APILogin API)
        {
            bool Approval = false;
            string AlternateEmail = "", Message = "", UniqueDeviceId = "";
            int Result = 0, UserId = 0, UserType = 0;
            DataTable DT_Login = new DataTable();
            MemoryStream memoryStream = new MemoryStream();
            var jsonObj = new JsonResult();
            DataTable dt = new DataTable();
            try
            {
                string Pass = Cryptography.GetEncryptedData(API.Password, true);
                string SQRY = "exec USP_User_Login_API '" + API.Userid + "','" + Pass + "'";
                dt = DBU.GetDTResponse(SQRY);

                UserId = Convert.ToInt32(dt.Rows[0]["UserId"].ToString());
                AlternateEmail = dt.Rows[0]["AlternateEmail"].ToString();
                UserType = Convert.ToInt32(dt.Rows[0]["UserType"].ToString());
                UniqueDeviceId = dt.Rows[0]["UniqueDeviceId"].ToString();
                Approval = Convert.ToBoolean(dt.Rows[0]["Approval"].ToString());
                Message = dt.Rows[0]["Message"].ToString();
                Result = 1;
            }
            catch (Exception ex)
            {
                Result = 0;
                Message = ex.Message;
            }
            return new JsonResult() { Data = new { Result = Result, UserId = UserId, AlternateEmail = AlternateEmail, UserType = UserType, Message = Message, UniqueDeviceId = UniqueDeviceId, Approval = Approval } };
        }
        public JsonResult API_USPHolidayListAPI()
        {
            DataTable DT_Login = new DataTable();
            var SearchList = new object();
            DataTable dt = new DataTable();
            try
            {
                string SQRY = "exec USP_Holiday_List_API";
                dt = DBU.GetDTResponse(SQRY);
                List<HolidayManagment> Holiday = DataRowToObject.CreateListFromTable<HolidayManagment>(dt);
                var ListHoliday = Holiday;
                SearchList = (from e in ListHoliday
                              select new
                              {
                                  Id = e.Id,
                                  HolidayName = e.HolidayName,
                                  HolidayDate = e.HolidayDate,

                              }).Distinct().ToList();
            }
            catch (Exception ex)
            {
            }
            return new JsonResult() { Data = new { Result = 1, List = SearchList, Message = "Done" } };
        }
        private JsonResult Json(List<object> searchList, JsonRequestBehavior allowGet)
        {
            throw new NotImplementedException();
        }
        public JsonResult API_USPLeaveList(Tbl_Employee_Leave_Management API)
        {
            int Result = 0;
            string Message = "";
            DataTable DT_Login = new DataTable();
            var SearchList = new object();
            DataTable dt = new DataTable();
            try
            {
                if (API.Id > 0)
                {
                    string SQRY = "exec USP_Leave_List_API";
                    dt = DBU.GetDTResponse(SQRY);
                    List<Tbl_Employee_Leave_Management> Leave = DataRowToObject.CreateListFromTable<Tbl_Employee_Leave_Management>(dt);
                    Leave = Leave.Where(x => x.Id == API.Id).ToList();
                    var ListLeave = Leave;
                    SearchList = (from e in ListLeave
                                  select new
                                  {
                                      Id = e.Id,
                                      EmployeeId = e.EmployeeId,
                                      FromDate = e.FromDate,
                                      ToDate = e.ToDate,
                                      LeaveReason = e.LeaveReason,
                                      LeaveType = e.LeaveType,
                                      IsApproved = e.IsApproved,
                                      IsRejected = e.IsRejected,
                                      ApprovedRejectBy = e.ApprovedRejectBy,
                                      RejectReason = e.RejectReason,
                                      RejectDate = e.RejectDate,
                                      ApprovedDate = e.ApprovedDate,

                                  }).Distinct().ToList();
                    Message = Leave.Count().ToString() + " Records Found";
                    Result = 1;
                }
                else
                {
                    Message = "Employee Id Not Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, List = SearchList, Message = Message } };
        }
        public JsonResult API_Insert_Leavemanagement(int Id, DateTime FromDate, DateTime ToDate, string EmployeeId, string LeaveReason, string LeaveType, int UserId)
        {
            DataTable DT_Login = new DataTable();
            string Message = "";
            DataTable dt = new DataTable();
            try
            {
                string SQRY = "EXEC API_INSERT_LEAVE_MANAGEMENT '" + Id + "','" + FromDate + "','" + ToDate + "','" + EmployeeId + "','" + LeaveReason + "','" + LeaveType + "','" + UserId + "'";
                DataTable DT = DBU.GetDTResponse(SQRY);
                if (DT.Rows.Count > 0)
                {
                    Message = DT.Rows[0]["Result"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return new JsonResult() { Data = new { Result = Message } };
        }
        public JsonResult API_EmployeeDeviceMapping(DeviceManager API)
        {
            string Message = "";
            bool Result = false;
            int response = 0;
            DataTable DT_Login = new DataTable();
            MemoryStream memoryStream = new MemoryStream();
            var jsonObj = new JsonResult();
            DataTable dt = new DataTable();
            try
            {
                string SQRY = "exec USP_AddDeviceWithUserMapping '" + API.DeviceCategory + "','" + API.Description + "','" + API.UniqueDeviceId + "','" + API.NotificationTokenId + "','" + API.CreatedBy + "','" + API.EmpId + "','" + API.DeviceDisplayName + "'";
                dt = DBU.GetDTResponse(SQRY);
                if (dt != null)
                {
                    Result = Convert.ToBoolean(dt.Rows[0]["Result"].ToString());
                    Message = dt.Rows[0]["Message"].ToString();
                    response = Result ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Message = "Wrong User Name & Password !!!";
            }
            return new JsonResult() { Data = new { Result = response, Message = Message } };
        }
        public JsonResult API_GetDeviceTypes(DeviceManager DeviceData)
        {
            string Message = "Done";
            var SearchList = new object();
            try
            {
                List<GlobalCode> ListCode = ARS.GetGlobalCodes(0, DeviceData.DeviceCategory);
                SearchList = (from e in ListCode
                              select new
                              {
                                  CodeName = e.CodeName,
                                  Description = e.Description
                              }).Distinct().ToList();
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
            }
            return new JsonResult() { Data = new { Result = 1, List = SearchList, Message = Message } };
        }

        public JsonResult API_EmployeeAttendanceListMonthWise(Employee_APIAttendanceModel API)
        {
            int Result = 0;
            string Message = "";
            List<Employee_AttendanceModel> lstChart = new List<Employee_AttendanceModel>();
            try
            {

                if (API.UserId > 0 && API.Month > 0)
                {
                    string SQRY = "EXEC SP_GetEmployeeAttendanceList N'" + API.LocationId + "','" + API.UserId + "', '" + API.Month + "'";
                    DataTable DT = DBU.GetDTResponse(SQRY);
                    lstChart = DataRowToObject.CreateListFromTable<Employee_AttendanceModel>(DT);
                    Message = lstChart.Count.ToString() + " Records Found.";
                    Result = 1;
                }
                else
                {
                    Message = "Invalid Data Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, List = lstChart, Message = Message } };
        }
        public JsonResult API_EmployeeAttendanceStatusDaily(Employee_APIAttendanceModel API)
        {
            int Result = 0;
            string Message = "";
            List<Employee_AttendanceModel> lstChart = new List<Employee_AttendanceModel>();
            try
            {
                if (API.UserId > 0)
                {
                    string SQRY = "EXEC SP_GetEmployeeAttendanceByDate N'" + API.LocationId + "','" + API.UserId + "', '" + API.FromDate.ToString("dd MMM yyyy") + "', '" + API.ToDate.ToString("dd MMM yyyy") + "'";
                    DataTable DT = DBU.GetDTResponse(SQRY);
                    lstChart = DataRowToObject.CreateListFromTable<Employee_AttendanceModel>(DT);
                    Message = lstChart.Count.ToString() + " Records Found.";
                    Result = 1;
                }
                else
                {
                    Message = "Invalid Data Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, List = lstChart, Message = Message } };
        }
        public JsonResult API_EmployeeLogin(Employee_APIPunchInOutModel API)
        {
            int Result = 0;
            string Message = "";
            try
            {
                if (API.UserId > 0)
                {
                    string SQRY = "EXEC INSERT_Employee_Attendance_ClockIn '" + API.UserId + "','" + API.AttendanceType + "' ";
                    DBU.SetWOResponse(SQRY);
                    Message = "Attendance Records Updated.";
                    Result = 1;
                }
                else
                {
                    Message = "Invalid Data Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, Message = Message } };
        }

        public JsonResult API_EmployeeLogOut(Employee_APIPunchInOutModel API)
        {
            int Result = 0;
            string Message = "";
            try
            {
                if (API.UserId > 0)
                {
                    string SQRY = "EXEC INSERT_Employee_Attendance_ClockOut '" + API.UserId + "','" + API.AttendanceType + "' ";
                    DBU.SetWOResponse(SQRY);
                    Message = "Attendance Records Updated.";
                    Result = 1;
                }
                else
                {
                    Message = "Invalid Data Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, Message = Message } };
        }
        public JsonResult API_EmployeeProfile(Employee_APIPunchInOutModel API)
        {
            int Result = 0;
            string Message = "";
            Employee_ProfileModel Model = new Employee_ProfileModel();
            try
            {
                if (API.UserId > 0)
                {
                    string SQRY = "EXEC Get_EmployeeProfile_API '" + API.UserId + "'";
                    DataTable DT = DBU.GetDTResponse(SQRY);
                    Model = DataRowToObject.CreateItemFromRow<Employee_ProfileModel>(DT.Rows[0]);
                    DBU.SetWOResponse(SQRY);
                    Message = "Records Updated.";
                    Result = 1;
                }
                else
                {
                    Message = "Invalid Data Request Found";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, Profile = Model, Message = Message } };
        }

        public JsonResult GetLocationListApi()
        {
            DataTable DT_Login = new DataTable();
            var SearchList = new object();
            DataTable dt = new DataTable();
            try
            {
                string SQRY = "SELECT LocationId,LocationName FROM LocationMaster WHERE IsDeleted=0";
                dt = DBU.GetDTResponse(SQRY);
                List<LocationModel> Location = DataRowToObject.CreateListFromTable<LocationModel>(dt);
                var ListLocation = Location;
                SearchList = (from e in ListLocation
                              select new
                              {
                                  LocationId = e.LocationId,
                                  LocationName = e.LocationName,

                              }).Distinct().ToList();
            }
            catch (Exception ex)
            {
            }
            return new JsonResult() { Data = new { Result = 1, List = SearchList, Message = "Done" } };
        }

        public JsonResult API_EmployeeAttendanceStatusAprroval(AttendanceApproval AAM)
        {
            int Result = 0;
            string Message = "";
            try
            {
                if (!string.IsNullOrEmpty(AAM.UserName) && AAM.Status > 0 && !string.IsNullOrEmpty(AAM.TimeCardIds))
                {
                    string SQRY = "EXEC SP_API_EmployeeAttendanceStatusAprroval N'" + AAM.TimeCardIds + "','" + AAM.Status + "', '" + AAM.UserName + "'";
                    DataTable DT = DBU.GetDTResponse(SQRY);
                    Message = DT.Rows[0]["ListCount"].ToString() + " Records " + DT.Rows[0]["Message"].ToString();
                    Result = 1;
                }
                else
                {
                    Message = "Somthing Went Wrong In Request !!!";
                    Result = 0;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message.ToString();
                Result = 0;
            }
            return new JsonResult() { Data = new { Result = Result, Message = Message } };
        }
    }
}
