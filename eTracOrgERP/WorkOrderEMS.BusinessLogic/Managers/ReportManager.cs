using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helper.SerializationHelper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.UserModels;
using static WorkOrderEMS.Models.eFleetDriverModel;

namespace WorkOrderEMS.BusinessLogic
{

    public class ReportManager : IReportManager
    {
        workorderEMSEntities _workorderEMSEntities = new workorderEMSEntities();
        QRCScanLogRepository objQRCScanLogRepository;
        ReportRepository objReportRepository = new ReportRepository();
        public List<CleaningModel> NoCleaningDone()
        {
            try
            {
                return objReportRepository.NoCleaningDone();
            }
            catch (Exception ex)
            {
                List<CleaningModel> lstRoutine = new List<CleaningModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<CleaningModel> NoCleaningDone", "lstRoutine", lstRoutine);
                return lstRoutine;
            }
        }

        public List<WorkOrderIssueedModel> GetWorkOrderIssued(long ProjectId)
        {
            try
            {
                return objReportRepository.GetWorkOrderIssued(ProjectId);
            }
            catch (Exception ex)
            {
                List<WorkOrderIssueedModel> lstRoutine = new List<WorkOrderIssueedModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderIssued", "ProjectId", ProjectId);
                return lstRoutine;
            }
        }

        public List<WorkOrderIssueedModel> GetWorkOrderListForLocation(long? LocationID, long? UserId, long LoginUserId, string FromDate, string ToDate, string WorkRequestProjectType, string textSearch)
        {
            try
            {
                return objReportRepository.GetWorkOrderIssuedForLocation(LocationID, UserId, LoginUserId, FromDate, ToDate, WorkRequestProjectType, textSearch);
            }
            catch (Exception ex)
            {
                List<WorkOrderIssueedModel> lstRoutine = new List<WorkOrderIssueedModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderListForLocation(long? LocationID, string FromDate, string ToDate, string WorkRequestProjectType, string textSearch)", "LocationID", LocationID);
                return lstRoutine;
            }
        }

        public List<WorkOrderIssueedModel> GetWorkOrderInProgressListForLocation(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? UserId, string textSearch)
        {
            try
            {
                return objReportRepository.GetWorkOrderInProgressForLocation(LoginUserID, LocationID, FromDate, ToDate, WorkRequestProjectType, UserId, textSearch);
            }
            catch (Exception ex)
            {
                List<WorkOrderIssueedModel> lstRoutine = new List<WorkOrderIssueedModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderInProgressListForLocation(long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? UserId, string textSearch)", "LocationID", LocationID);
                return lstRoutine;
            }
        }

        public List<WorkOrderIssueedModel> GetWorkOrderAssignedListForLocationItem(long LoginUserID, long? LocationID, string FromDate, string ToDate, long? QrcId, long? ReqType, int? WorkRequestProjectType, long? safetyHuzzard, long? priorityLevel, long? userId, string textSearch)
        {
            try
            {
                return objReportRepository.GetWorkOrderIssuedForAssignedLocationItem(LoginUserID, LocationID, FromDate, ToDate, QrcId, ReqType, WorkRequestProjectType, safetyHuzzard, priorityLevel, userId, textSearch);
            }
            catch (Exception ex)
            {
                List<WorkOrderIssueedModel> lstRoutine = new List<WorkOrderIssueedModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderAssignedListForLocationItem(long? LocationID, string FromDate, string ToDate, long? QrcId, long? ReqType, int? WorkRequestProjectType, long? safetyHuzzard, long? priorityLevel, long? userId, string textSearch)", "LocationID", LocationID);
                return lstRoutine;
            }
        }

        /// <summary>Get List Of Asset by location
        /// <CreatedBy>Bhushan Dod</CreatedBy>
        /// <Craeted For>Get Asset List from  QRCMaster </Craeted>
        /// <ModifiedOn>March-11-2015</ModifiedOn>
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetAssetListForReportingDDL(long? LocationId)
        {
            List<SelectListItem> lstAsset = new List<SelectListItem>();
            QRCMasterRepository objQRCMasterRepository = new QRCMasterRepository();
            try
            {
                if (LocationId == 0)
                {
                    lstAsset = objQRCMasterRepository.GetAll(r => r.IsDeleted == false).Select(e => new SelectListItem()
                    {
                        Value = Convert.ToString(e.QRCID, CultureInfo.InvariantCulture),
                        Text = e.QRCName,

                    }).ToList();
                }
                else
                {
                    lstAsset = objQRCMasterRepository.GetAll(r => r.IsDeleted == false && r.LocationId == LocationId).Select(e => new SelectListItem()
                    {
                        Value = Convert.ToString(e.QRCID, CultureInfo.InvariantCulture),
                        Text = e.QRCName,

                    }).ToList();
                }


                return lstAsset;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<SelectListItem> GetAssetListForReportingDDL(long? LocationId)", "Exception while fetching latest record for GetAssetListForReportingDDL ", LocationId);
                throw;
            }
        }

        public List<WorkOrderIssueedModel> GetWorkOrderIssuedListFixedTime(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, string textSearch)
        {
            try
            {
                return objReportRepository.GetWorkOrderIssuedListFixedTime(LoginUserID, LocationID, FromDate, ToDate, WorkRequestProjectType, textSearch);
            }
            catch (Exception ex)
            {
                List<WorkOrderIssueedModel> lstRoutine = new List<WorkOrderIssueedModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderIssuedListFixedTime(long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, string textSearch)", "LocationID", LocationID);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 15/04/2015
        ///  Get details of QRC Scan for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetRoutineCheckList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    var TemplstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && x.q.GlobalCode.CodeName == name
                                                    && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                    && x.u.ClientTypeID == null
                                                    ).ToList();
                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        if (filterlst.u.QRCTYPE == (long)QrcType.TrashCan && filterlst.u.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Elevator && filterlst.u.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.Clean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.MovingWalkway && filterlst.u.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Escalators && filterlst.u.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.ParkingFacility && filterlst.u.QRCTypeDetails != null)
                        {
                            if (filterlst.u.QRCTypeDetails != null)
                            {
                                var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                                if (tt.Routine == false)
                                {
                                    TemplstRoutine.Remove(filterlst);
                                }
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }

                    }
                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.ScanUserId).Select(x => new ReportChart()
                    {
                        ScanUserId = x.Key,
                        ScanUserCount = x.Count(),
                        ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                    }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetRoutineCheckList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    var TemplstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                         Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
                                                    && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                    && (x.q.QrcType == 37 || x.q.QrcType == 38 || x.q.QrcType == 43 || x.q.QrcType == 44 || x.q.QrcType == 101)
                                                      && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                     && x.u.ClientTypeID == null
                                               ).ToList();
                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        if (filterlst.u.QRCTYPE == (long)QrcType.TrashCan && filterlst.u.QRCTypeDetails != null)
                        {

                            var tt = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }

                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Elevator && filterlst.u.QRCTypeDetails != null)
                        {

                            var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.Clean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.MovingWalkway && filterlst.u.QRCTypeDetails != null)
                        {

                            var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }

                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Escalators && filterlst.u.QRCTypeDetails != null)
                        {

                            var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.ParkingFacility && filterlst.u.QRCTypeDetails != null)
                        {


                            var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.q.QRCMaster.QRCTypeDetails);

                            if (tt.Routine == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }

                            //else
                            //{
                            //    filterlst.CleaningDescription = tt.Cleaning.CleanDescription;
                            //}
                        }

                    }
                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.QrcType).Select(x => new ReportChart()
                    {
                        QrcType = x.Key,
                        QrcTypeCount = x.Count(),
                        QrcTypeName = x.FirstOrDefault().q.GlobalCode.CodeName
                    }).ToList<ReportChart>();


                    return lstRoutine;

                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetRoutineCheckList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 17/04/2015
        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> RoutineDoneListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {


                //lstRoutine = _workorderEMSEntities.QRCMasterLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                //        Where(x => ((x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName) == userName)
                //                                   && ((LocationId == 0 ? null : LocationId) == null || x.u.LocationId == LocationId)
                //                                   && x.u.GlobalCode5.CodeName == qrcName
                //                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                //                                   && x.u.ClientTypeID == null
                //                                   ).Select(x => new ReportChart()
                //                                   {
                //                                       ScanUserId = x.q.UserId,
                //                                       ScanUserName = x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName,
                //                                       CreatedDate = x.q.CreatedOn,
                //                                       QrcName = x.u.QRCName
                //                                   }).ToList<ReportChart>();
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                ////Converted to UTC because datetime in utc in db.
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.QRCMasterLogs.
                            Where(x => ((x.UserRegistration.FirstName + " " + x.UserRegistration.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                   && x.QRCMaster.GlobalCode5.CodeName == qrcName
                                                  && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                   && x.QRCMaster.ClientTypeID == null
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.UserId,
                                                       ScanUserName = x.UserRegistration.FirstName + " " + x.UserRegistration.LastName,
                                                       CreatedDate = x.CreatedOn,
                                                       QrcName = x.QRCMaster.QRCName,
                                                       QrcTypeName = x.QRCTypeDetails,
                                                       QrcType = x.QRCMaster.QRCTYPE,
                                                       QrCodeId = x.QRCMaster.QRCodeID,
                                                       KeyName = x.QRCMaster.GlobalCode5.CodeName
                                                   }).ToList();

                foreach (var filterlst in lstRoutine.ToList())
                {
                    if (filterlst.QrcType == (long)QrcType.Vehicle && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.Elevator && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.GateArm && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcGateArmModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.TicketSpitter && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcTicketSplitterModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.BusStation && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcBusStationModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.EmergencyPhoneSystems && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcPhoneSystemModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.MovingWalkway && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.Escalators && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }

                    if (filterlst.QrcType == (long)QrcType.Bathroom && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcBathroomModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.ParkingFacility && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.QrcTypeName);
                        if (tt.Cleaning.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }

                }

                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    CreatedDate = x.CreatedDate,
                    QrcName = x.QrcName,
                    QrCodeId = x.QrCodeId,
                    Description = x.Description,
                    //CleaningDescription = x.CleaningDescription,
                    KeyName = x.KeyName
                }).ToList();

                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> RoutineDoneListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 03/04/2015
        /// Get details of Routine Check for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        //public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        //{
        //    DateTime _fromDate = FromDate ?? DateTime.UtcNow.Date;
        //    DateTime _toDate = ToDate ?? DateTime.UtcNow;
        //    //_fromDate = DateTime.Parse(_fromDate.ToString("dd/MMM/yyyy hh:mm:ss tt"));
        //    //_toDate= DateTime.Parse(_toDate.ToString("dd/MMM/yyyy hh:mm:ss tt"));
        //    if (name != null && name.Trim() != "")
        //    {
        //        List<ReportChart> lstRoutine = new List<ReportChart>();
        //        try
        //        {


        //            var TemplstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
        //                Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
        //                                           && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
        //                                           && x.q.GlobalCode.CodeName == name
        //                                           && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
        //                                           && x.u.ClientTypeID == null
        //                                           ).ToList();

        //            lstRoutine = TemplstRoutine.GroupBy(x => x.q.ScanUserId).Select(x => new ReportChart()
        //            {
        //                ScanUserId = x.Key,
        //                ScanUserCount = x.Count(),
        //                ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
        //            }).ToList<ReportChart>();
        //            return lstRoutine;
        //        }
        //        catch (Exception ex)
        //        {
        //            Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
        //            return lstRoutine;
        //        }
        //    }
        //    else
        //    {
        //        objQRCScanLogRepository = new QRCScanLogRepository();
        //        List<ReportChart> lstRoutine = new List<ReportChart>();
        //        try
        //        {
        //            var TemplstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
        //                Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
        //                                           && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
        //                                           && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
        //                                            && x.u.ClientTypeID == null
        //                                      ).ToList();
        //            lstRoutine = TemplstRoutine.GroupBy(x => x.q.QrcType).Select(x => new ReportChart()
        //            {
        //                QrcType = x.Key,
        //                QrcTypeCount = x.Count(),
        //                QrcTypeName = x.FirstOrDefault().q.GlobalCode.CodeName
        //            }).ToList<ReportChart>();

        //            return lstRoutine;
        //        }
        //        catch (Exception ex)
        //        {
        //            Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
        //            return lstRoutine;
        //        }
        //    }
        //}

        public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    //Converted to UTC because datetime in utc in db.

                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    //Converting datetime from userTZ to UTC
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();

                    lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                       Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
                                                  && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                  && x.q.GlobalCode.CodeName == name
                                                  && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                  && x.u.ClientTypeID == null
                                                  ).GroupBy(x => x.q.ScanUserId).Select(x => new ReportChart()
                                                  {
                                                      ScanUserId = x.Key,
                                                      ScanUserCount = x.Count(),
                                                      ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                                                  }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    //   bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;

                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();

                    lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.ScanUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                    && x.u.ClientTypeID == null
                                              ).GroupBy(x => x.q.QrcType).Select(x => new ReportChart()
                                              {
                                                  QrcType = x.Key,
                                                  QrcTypeCount = x.Count(),
                                                  QrcTypeName = x.FirstOrDefault().q.GlobalCode.CodeName
                                              }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 17/04/2015
        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> QrcTypeScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {

            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            //DateTime _fromDate = FromDate ?? DateTime.UtcNow.Date;
            //DateTime _toDate = ToDate ?? DateTime.UtcNow;

            // var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();           
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;

                //Converted to UTC because datetime in utc in db.
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //_fromDate = _fromDate.ToClientTimeZoneinDateTime();
                //_toDate = _toDate.ToClientTimeZoneinDateTime();
                //_fromDate = Convert.ToDateTime(_fromDate).ToClientTimeZoneinDateTime();
                //_toDate = Convert.ToDateTime(_toDate).ToClientTimeZoneinDateTime();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u })
                                                                .Where(x => ((x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName) == userName)
                                                                    && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                                    && x.q.GlobalCode.CodeName == qrcName
                                                                    && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                   // && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                   && x.u.ClientTypeID == null
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.q.ScanUserId,
                                                       ScanUserName = x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName,
                                                       CreatedDate = x.q.CreatedOn,
                                                       QrcName = x.u.QRCName,
                                                       QrCodeId = x.u.QRCodeID,
                                                       LocationName = x.u.LocationMaster.LocationName,
                                                       QrcTypeName = x.q.GlobalCode.CodeName

                                                   }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    CreatedDate = x.CreatedDate,
                    QrcName = x.QrcName,
                    QrCodeId = x.QrCodeId,
                    LocationName = x.LocationName,
                    QrcTypeName = x.QrcTypeName
                }).ToList();

                //lstRoutine = TemplstRoutine.GroupBy(x => x.ScanUserId).Select(x => new ReportChart()
                //{
                //    ScanUserId = x.Key,                  
                //    ScanUserName = x.FirstOrDefault().UserRegistration.FirstName + " " + x.FirstOrDefault().UserRegistration.LastName,
                //    CreatedDate = x.Select(y=>y.CreatedOn).ToString(),                   
                //}).ToList<ReportChart>();
                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> QrcTypeScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>
        /// Created by Ankit Choudhary on 01/02/2017
        /// Routine scandetails for table 
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="qrcName"></param>
        /// <param name="userName"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> RoutineScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            // var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();           
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {

                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();


                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.QRCScanLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && x.q.GlobalCode.CodeName == qrcName
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                   && x.u.ClientTypeID == null).Select(x => new ReportChart()
                                                   {
                                                       ScanId = x.q.QRCScanLogId,
                                                       ScanUserId = x.q.ScanUserId,
                                                       ScanUserName = x.q.UserRegistration.FirstName + " " + x.q.UserRegistration.LastName,
                                                       CreatedDate = x.q.CreatedOn,
                                                       QrcName = x.u.QRCName,
                                                       QrCodeId = x.u.QRCodeID,
                                                       QrcTypeName = x.u.QRCTypeDetails,
                                                       QrcType = x.u.QRCTYPE,
                                                       KeyName = x.q.GlobalCode.CodeName
                                                   }).ToList();

                foreach (var filterlst in lstRoutine.ToList())
                {
                    if (filterlst.QrcType == (long)QrcType.TrashCan && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QrcTypeName);

                        if (tt.Routine.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.Elevator && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.QrcTypeName);

                        if (tt.Routine.Clean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.MovingWalkway && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.QrcTypeName);

                        if (tt.Routine.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }
                    }
                    if (filterlst.QrcType == (long)QrcType.Escalators && filterlst.QrcTypeName != null)
                    {
                        var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.QrcTypeName);

                        if (tt.Routine.IsClean == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }

                    }
                    if (filterlst.QrcType == (long)QrcType.ParkingFacility && filterlst.QrcTypeName != null)
                    {

                        var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.QrcTypeName);

                        if (tt.Routine == false)
                        {
                            lstRoutine.Remove(filterlst);
                        }

                        //else
                        //{
                        //   rc.RoutineDescription = tt.Routine.RoutineDescription;
                        //}
                    }
                }
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    CreatedDate = x.CreatedDate,
                    QrcName = x.QrcName,
                    QrCodeId = x.QrCodeId,
                    //RoutineDescription = (x.RoutineDescription == null) ? "Not Available" : x.RoutineDescription,
                    KeyName = x.KeyName
                }).ToList();

                //lstRoutine = TemplstRoutine.GroupBy(x => x.ScanUserId).Select(x => new ReportChart()
                //{
                //    ScanUserId = x.Key,                  
                //    ScanUserName = x.FirstOrDefault().UserRegistration.FirstName + " " + x.FirstOrDefault().UserRegistration.LastName,
                //    CreatedDate = x.Select(y=>y.CreatedOn).ToString(),                   
                //}).ToList<ReportChart>();
                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> QrcTypeScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return null;
            }
        }

        /// <summary>Created by Bhushan Dod on 22/04/2015
        /// Get details of Cleaning done for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetCleaningDoneList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            //var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();
            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {


                    //var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                    //    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                    //                               && ((LocationId == 0 ? null : LocationId) == null || x.u.LocationId == LocationId)
                    //                               && x.u.GlobalCode5.CodeName == name
                    //                               && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                    //                                && x.u.ClientTypeID == null
                    //                               ).ToList();


                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                   && x.QRCMaster.GlobalCode5.CodeName == name
                                                    && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                    && x.QRCMaster.ClientTypeID == null
                                                   ).ToList();
                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.Vehicle && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.Elevator && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.GateArm && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcGateArmModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.TicketSpitter && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcTicketSplitterModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.BusStation && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcBusStationModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.EmergencyPhoneSystems && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcPhoneSystemModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.MovingWalkway && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.Escalators && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }

                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.Bathroom && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcBathroomModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.QRCMaster.QRCTYPE == (long)QrcType.ParkingFacility && filterlst.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }

                    }
                    lstRoutine = TemplstRoutine.GroupBy(x => x.UserId).Select(x => new ReportChart()
                    {
                        ScanUserId = x.Key,
                        ScanUserCount = x.Count(),
                        //ScanUserName = usertemp.Where(y=>y.UserId==x.Key).Select(z=>z.FirstName).ToString(),
                        ScanUserName = x.FirstOrDefault().UserRegistration.FirstName + " " + x.FirstOrDefault().UserRegistration.LastName,
                    }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetCleaningDoneList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }

            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {

                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.u.LocationId == LocationId)
                                                   && (x.u.QRCTYPE == 36 || x.u.QRCTYPE == 38 || x.u.QRCTYPE == 39 || x.u.QRCTYPE == 40 || x.u.QRCTYPE == 41 ||
                                                       x.u.QRCTYPE == 42 || x.u.QRCTYPE == 43 || x.u.QRCTYPE == 44 || x.u.QRCTYPE == 45 || x.u.QRCTYPE == 101)
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                    && x.u.ClientTypeID == null
                                              ).ToList();
                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        if (filterlst.u.QRCTYPE == (long)QrcType.Vehicle && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Elevator && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcElevatorModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.GateArm && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcGateArmModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.TicketSpitter && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcTicketSplitterModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.BusStation && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcBusStationModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.EmergencyPhoneSystems && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcPhoneSystemModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.MovingWalkway && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcMovingWalkwayModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.Escalators && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcEscalatorsModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }

                        if (filterlst.u.QRCTYPE == (long)QrcType.Bathroom && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcBathroomModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }
                        if (filterlst.u.QRCTYPE == (long)QrcType.ParkingFacility && filterlst.q.QRCTypeDetails != null)
                        {
                            var tt = GenericDataContractSerializer<ServiceQrcParkingModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (tt.Cleaning.IsClean == false)
                            {
                                TemplstRoutine.Remove(filterlst);
                            }
                        }

                    }

                    lstRoutine = TemplstRoutine.GroupBy(x => x.u.QRCTYPE).Select(x => new ReportChart()
                    {
                        QrcType = x.Key,
                        QrcTypeCount = x.Count(),
                        QrcTypeName = x.FirstOrDefault().u.GlobalCode5.CodeName
                    }).ToList<ReportChart>();

                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetCleaningDoneList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 24/04/2015
        /// Get details of trash levels for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetTrashLevelList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
                    isUTCDay = false;
            }
            if (_fromDate != null && _toDate != null)
            {
                //if interval date come then need to fetch record till midnight of todate day
                if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
                if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
                {
                    _toDate = _toDate.AddDays(1).Date;
                }
            }
            var extractXML = new List<ServiceQrcTrashcanModel>();
            var returnlist = new List<ReportChart>();
            var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();
            if (name != null && name.Trim() != "")
            {
                var lstRoutine = new List<ReportChart>();
                var lstRoutineTrashName = new List<ReportChart>();

                List<ReportChart> objlst = new List<ReportChart>();
                ReportChart objReportJsonResult;// = new ReportChart();

                TrashData1 objT;
                List<TrashData1> objTList = new List<TrashData1>();
                try
                {
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //Converted to UTC because datetime in utc in db.
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                    var listTrash = _workorderEMSEntities.QRCMasters.Select(x => x).ToList();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                   && (x.QRCMaster.QRCTYPE == (long)QrcType.TrashCan)//x.QRCMaster.GlobalCode5.CodeName == name
                                                   && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                    && x.QRCMaster.ClientTypeID == null
                                                   ).ToList();

                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        /// <summary>
                        /// Code Updated By Ankit Choudhary on 18jan2017
                        /// previously this code add the data after deserialing the xml 
                        /// but not this code check the condition if IsReported = true then only it count as a trash reported.
                        /// </summary>

                        if (filterlst.QRCTypeDetails != null)
                        {
                            var deserialize1 = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (deserialize1.IsReported == true)
                            {
                                extractXML.Add(deserialize1);
                            }
                        }
                    }

                    if (extractXML.Count > 0)
                    {      //This list Get all Trash Count with user name             
                        lstRoutine = extractXML.Where(x => x.Routine.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == name && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId))
                                       .GroupBy(x => new { x.UserId, x.QrcId }).Select(x => new ReportChart()
                                       {
                                           ScanUserCount = x.Count(),
                                           ScanUserName = listuser.Where(f => f.UserId == x.Key.UserId).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key.UserId).FirstOrDefault().LastName,
                                           QrcName = listTrash.Where(f => f.QRCID == x.Key.QrcId).FirstOrDefault().QRCName
                                       }).ToList<ReportChart>();

                        //This list Get all Trash name    
                        lstRoutineTrashName = extractXML.Where(x => x.Routine.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == name && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId))
                                       .GroupBy(x => new { x.QrcId }).Select(x => new ReportChart()
                                       {
                                           QrcName = listTrash.Where(f => f.QRCID == x.Key.QrcId).FirstOrDefault().QRCName
                                       }).ToList<ReportChart>();
                    }
                    foreach (var i in lstRoutineTrashName)
                    {
                        objReportJsonResult = new ReportChart();
                        objTList = new List<TrashData1>();
                        foreach (var j in lstRoutine)
                        {
                            if (i.QrcName == j.QrcName)
                            {
                                objT = new TrashData1();
                                objT.ReportCount = j.ScanUserCount;
                                objT.ReportUserName = j.ScanUserName;
                                objTList.Add(objT);
                            }
                        }
                        objReportJsonResult.KeyName = i.QrcName;
                        objReportJsonResult.ValueUser = objTList;
                        objlst.Add(objReportJsonResult);
                    }

                    return objlst;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetTrashLevelList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return objlst;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();

                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.u.LocationId == LocationId)
                                                   && (x.u.QRCTYPE == (long)QrcType.TrashCan)
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                    && x.u.ClientTypeID == null
                                              ).ToList();
                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        /// <summary>
                        /// Code Updated By Ankit Choudhary on 18jan2017
                        /// previously this code add the data after deserialing the xml 
                        /// but not this code check the condition if IsReported = true then only it count as a trash reported.
                        /// </summary>

                        if (filterlst.q.QRCTypeDetails != null)
                        {
                            var deserialize = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.q.QRCTypeDetails);

                            if (deserialize.IsReported == true)
                            {

                                extractXML.Add(deserialize);
                            }
                        }


                    }
                    if (extractXML.Count > 0)
                    {
                        lstRoutine = extractXML.GroupBy(x => x.Routine.TrashLevels).Select(x => new ReportChart()
                        {
                            QrcType = x.Key,
                            QrcTypeCount = x.Count(),
                            //ScanUserName = usertemp.Where(y=>y.UserId==x.Key).Select(z=>z.FirstName).ToString(),
                            QrcTypeName = listGlobalCodes.Where(f => f.GlobalCodeId == x.Key).FirstOrDefault().CodeName,
                            //.Select(c=>c.CodeName).ToString()
                        }).ToList<ReportChart>();
                    }
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetTrashLevelList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 28/04/2015
        /// Get details of Employee reported trash levels
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> ReportedTrashLevelListByEmployee(long? LocationId, string trashname, string userName, string qrctrashlevel, DateTime? FromDate, DateTime? ToDate, string TrashType)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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

            List<ReportChart> lstRoutine = new List<ReportChart>();
            var extractXML = new List<ServiceQrcTrashcanModel>();
            var temp = new List<ServiceQrcTrashcanModelTemp>();
            try
            {

                // bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                //Converted to UTC because datetime in utc in db.
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();

                //var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                //                                   && (x.QRCMaster.QRCName == trashname)
                //                                   && (x.QRCMaster.QRCTYPE == 37)
                //                                   && (isDateOnly == true ? (DbFunctions.TruncateTime(x.CreatedOn) >= _fromDate.Date && (DbFunctions.TruncateTime(x.CreatedOn) <= _toDate.Date)) : (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate))
                //                                   && x.QRCMaster.ClientTypeID == null
                //                                   ).ToList();
                var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((x.UserRegistration.FirstName + " " + x.UserRegistration.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                   && (x.QRCMaster.QRCName == trashname)
                                                   && (x.QRCMaster.QRCTYPE == (long)QrcType.TrashCan)
                                                   && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                   && x.QRCMaster.ClientTypeID == null
                                                   ).ToList();

                foreach (var filterlst in TemplstRoutine.ToList())
                {
                    var temp1 = new ServiceQrcTrashcanModelTemp();
                    if (filterlst.QRCTypeDetails != null)
                    {
                        temp1.ServiceQrcTrashcanProp = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QRCTypeDetails);
                        if (temp1.ServiceQrcTrashcanProp.IsReported)
                        {
                            temp1.QrclogId = filterlst.QRCLogId;
                            temp1.QrCodeId = filterlst.QRCMaster.QRCodeID;
                            temp.Add(temp1);
                        }
                    }

                }
                if (TrashType == "RoutineTrashLevel")
                {
                    if (temp.Count > 0)
                    {
                        lstRoutine = temp.Where(x => x.ServiceQrcTrashcanProp.Routine.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == qrctrashlevel && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId)).Select(x => new ReportChart()
                        {
                            QrcName = qrctrashlevel,
                            CreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.QrclogId).FirstOrDefault().CreatedOn,

                            ScanUserName = userName,
                            QrcTypeName = trashname,
                            ScanUserId = x.ServiceQrcTrashcanProp.UserId,
                            QrCodeId = x.QrCodeId
                        }).ToList<ReportChart>();
                    }
                }
                else
                {
                    lstRoutine = temp.Where(x => x.ServiceQrcTrashcanProp.TrashRemoval.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == qrctrashlevel && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId)).Select(x => new ReportChart()
                    {
                        QrcName = qrctrashlevel,
                        CreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.QrclogId).FirstOrDefault().CreatedOn,

                        ScanUserName = userName,
                        QrcTypeName = trashname,
                        ScanUserId = x.ServiceQrcTrashcanProp.UserId,
                        QrCodeId = x.QrCodeId

                    }).ToList<ReportChart>();
                }
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    //StrCreatedDate = x.CreatedDate.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StrCreatedDate = x.CreatedDate.ToClientTimeZone(true),
                    QrcName = x.QrcName,
                    QrcTypeName = x.QrcTypeName,
                    QrCodeId = x.QrCodeId,
                    CreatedDate = x.CreatedDate
                }).ToList();

                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> ReportedTrashLevelListByEmployee(long? LocationId, string trashname, string userName, string qrctrashlevel, DateTime? FromDate, DateTime? ToDate, string TrashType)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 27/04/2015
        /// Get details of trash picked up for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetTrashPickedUpList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            var extractXML = new List<ServiceQrcTrashcanModel>();
            var returnlist = new List<ReportChart>();

            var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();
            if (name != null && name.Trim() != "")
            {
                var lstRoutine = new List<ReportChart>();
                var lstRoutineTrashName = new List<ReportChart>();

                List<ReportChart> objlst = new List<ReportChart>();
                ReportChart objReportJsonResult;// = new ReportChart();

                TrashData1 objT;
                List<TrashData1> objTList = new List<TrashData1>();
                try
                {
                    var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                    var listTrash = _workorderEMSEntities.QRCMasters.Select(x => x).ToList();

                    // bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //Converted to UTC because datetime in utc in db.
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                             && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                             && (x.QRCMaster.QRCTYPE == (long)QrcType.TrashCan)//x.QRCMaster.GlobalCode5.CodeName == name
                                             && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                              && x.QRCMaster.ClientTypeID == null
                                             ).ToList();



                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        /// <summary>
                        /// Code Updated By Ankit Choudhary on 18jan2017
                        /// previously this code add the data after deserialing the xml 
                        /// but not this code check the condition if IsReported = true then only count as a trash Reported.
                        /// </summary>
                        if (filterlst.QRCTypeDetails != null)
                        {
                            var deserialize = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            if (deserialize.IsReported == true)
                            {
                                extractXML.Add(deserialize);
                            }
                        }
                        //extractXML.Add(GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QRCTypeDetails));
                    }

                    if (extractXML.Count > 0)
                    {      //This list Get all Trash Count with user name             
                        lstRoutine = extractXML.Where(x => x.TrashRemoval.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == name && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId))
                                       .GroupBy(x => new { x.UserId, x.QrcId }).Select(x => new ReportChart()
                                       {
                                           ScanUserCount = x.Count(),
                                           ScanUserName = listuser.Where(f => f.UserId == x.Key.UserId).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key.UserId).FirstOrDefault().LastName,
                                           QrcName = listTrash.Where(f => f.QRCID == x.Key.QrcId).FirstOrDefault().QRCName
                                       }).ToList<ReportChart>();

                        //This list Get all Trash name    
                        lstRoutineTrashName = extractXML.Where(x => x.TrashRemoval.TrashLevels == (listGlobalCodes.Where(m => m.CodeName == name && m.Category == "QRCTYPETRASHCAN").FirstOrDefault().GlobalCodeId))
                                       .GroupBy(x => new { x.QrcId }).Select(x => new ReportChart()
                                       {
                                           QrcName = listTrash.Where(f => f.QRCID == x.Key.QrcId).FirstOrDefault().QRCName
                                       }).ToList<ReportChart>();
                    }
                    foreach (var i in lstRoutineTrashName)
                    {
                        objReportJsonResult = new ReportChart();
                        objTList = new List<TrashData1>();
                        foreach (var j in lstRoutine)
                        {
                            if (i.QrcName == j.QrcName)
                            {
                                objT = new TrashData1();
                                objT.ReportCount = j.ScanUserCount;
                                objT.ReportUserName = j.ScanUserName;
                                objTList.Add(objT);

                            }
                        }
                        objReportJsonResult.KeyName = i.QrcName;
                        objReportJsonResult.ValueUser = objTList;
                        objlst.Add(objReportJsonResult);
                    }

                    return objlst;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetTrashPickedUpList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return objlst;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    //dynamic TemplstRoutine;
                    // bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //Converted to UTC because datetime in utc in db.
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Join(_workorderEMSEntities.QRCMasters, q => q.QRCID, u => u.QRCID, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.u.LocationId == LocationId)
                                                   && (x.u.QRCTYPE == (long)QrcType.TrashCan)
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                   && x.u.ClientTypeID == null).ToList();

                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        /// <summary>
                        /// Code Updated By Ankit Choudhary on 18jan2017
                        /// previously this code add the data after deserialing the xml 
                        /// but not this code check the condition if IsReported = true then only count as a trash Reported.
                        /// </summary>
                        if (filterlst.q.QRCTypeDetails != null)
                        {
                            var deserialized = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.q.QRCTypeDetails);
                            if (deserialized.IsReported == true && deserialized.TrashRemoval.TrashLevels == (long)TrashLevelValue.Full)
                            {
                                deserialized.QRCName = filterlst.u.QRCName;
                                extractXML.Add(deserialized);
                            }
                        }
                    }
                    if (extractXML.Count > 0)
                    {
                        lstRoutine = extractXML.GroupBy(x => x.QRCName).Select(x => new ReportChart()
                        {
                            QrcName = x.Key,
                            QrcNameCount = x.Count(),
                        }).ToList<ReportChart>();
                    }
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetTrashPickedUpList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 27/04/2015
        /// Get details of trash picked up for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetTrashPickedUpListDetails(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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

            List<ReportChart> lstRoutine = new List<ReportChart>();
            var extractXML = new List<ServiceQrcTrashcanModel>();
            var temp = new List<ServiceQrcTrashcanModelTemp>();
            //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
            //Converted to UTC because datetime in utc in db.
            _fromDate = _fromDate.ConvertClientTZtoUTC();
            _toDate = _toDate.ConvertClientTZtoUTC();
            //if (FromDate != null)
            //{
            //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
            //    {
            //        TimeSpan ts = new TimeSpan(00, 00, 00);
            //        _fromDate = _fromDate.Date + ts;
            //    }
            //}
            try
            {
                var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();
                var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                   && (x.QRCMaster.QRCTYPE == (long)QrcType.TrashCan)
                                                  && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                   && x.QRCMaster.ClientTypeID == null
                                                   ).ToList();

                foreach (var filterlst in TemplstRoutine.ToList())
                {
                    var temp1 = new ServiceQrcTrashcanModelTemp();
                    // var deserialize = ;
                    if (filterlst.QRCTypeDetails != null)
                    {
                        temp1.ServiceQrcTrashcanProp = GenericDataContractSerializer<ServiceQrcTrashcanModel>.DeserializeXml(filterlst.QRCTypeDetails);

                        /// <summary>
                        /// Code Updated By Ankit Choudhary on 18jan2017
                        /// previously this code add the data after deserialing the xml 
                        /// but not this code check the condition if IsReported = true then only it count as a trash reported.
                        /// </summary>
                        if (temp1.ServiceQrcTrashcanProp.IsReported == true && temp1.ServiceQrcTrashcanProp.TrashRemoval.TrashLevels == (long)TrashLevelValue.Full && filterlst.QRCMaster.QRCName == name)
                        {
                            temp1.QrclogId = filterlst.QRCLogId;
                            temp1.QrcName = filterlst.QRCMaster.QRCName;
                            temp1.QrCodeId = filterlst.QRCMaster.QRCodeID;
                            temp.Add(temp1);
                        }
                    }
                }

                if (temp.Count > 0)
                {
                    lstRoutine = temp.Where(x => x.ServiceQrcTrashcanProp.TrashRemoval.TrashLevels == (long)TrashLevelValue.Full).Select(x => new ReportChart()
                    {
                        QrcName = name,
                        CreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.QrclogId).FirstOrDefault().CreatedOn,
                        StrCreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.QrclogId).FirstOrDefault().CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),
                        ScanUserName = listuser.Where(f => f.UserId == x.ServiceQrcTrashcanProp.UserId).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.ServiceQrcTrashcanProp.UserId).FirstOrDefault().LastName,
                        QrcTypeName = x.QrcName,
                        ScanUserId = x.ServiceQrcTrashcanProp.UserId,
                        QrCodeId = x.QrCodeId

                    }).ToList<ReportChart>();
                }

                return lstRoutine;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetTrashPickedUpListDetails(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Get report on Get Expiration Date for warranty and insurance
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>May-04-2015</CreatedOn>
        /// <CreatedFor> GetExpirationDate</CreatedFor>
        /// </summary>
        /// <param name="LocationID,ExpirationType"></param>
        ///   /// <param name="FromDate"></param>
        /// <returns></returns>
        public List<QRCModel> GetExpirationDateList(long? LocationID, int ExpirationType)
        {
            try
            {
                return objReportRepository.GetExpirationDate(LocationID, ExpirationType);
            }
            catch (Exception ex)
            {
                List<QRCModel> lstRoutine = new List<QRCModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<QRCModel> GetExpirationDateList(long? LocationID, int ExpirationType)", "lstRoutine", lstRoutine);
                return lstRoutine;
            }
        }

        /// <summary>Get report on purchase type list
        /// <CreatedBy>Bhushan Dod</CreatedBY>
        /// <CreatedOn>May-05-2015</CreatedOn>
        /// <CreatedFor> PurchaseTypeList</CreatedFor>
        /// </summary>
        /// <param name="LocationID,OwnedType"></param>
        /// <returns></returns>
        public List<QRCModel> PurchaseTypeList(long? LocationID, int OwnedType)
        {
            try
            {
                //var listuser = _workorderEMSEntities.UserRegistrations.ToList();
                //var qrcmaster = _workorderEMSEntities.QRCMasters.Where(x => ((LocationID == 0 ? null : LocationID) == null || x.LocationId == LocationID)
                //                                            && x.PurchaseType == OwnedType).ToList();
                //lstQRCList = qrcmaster.Select(x => new QRCModel()
                //                                             {
                //                                                 QRCodeID = x.QRCodeID,
                //                                                 QRCName = x.QRCName,
                //                                                 QRCTYPECaption = x.GlobalCode5.CodeName,
                //                                                 UserName = listuser.Where(y => y.UserId == x.CreatedBy).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault(),
                //                                                 CreatedDate = x.CreatedDate,
                //                                                 CreatedOn = x.CreatedDate.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                //                                                 SpecialNotes = x.SpecialNotes == null ? "Not Available" : x.SpecialNotes,
                //                                                 VendorName = x.VendorName == null ? "Not Available" : x.VendorName,
                //                                                 //AssetPicture = (x.AssetPicture == null) ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : (
                //                                                 //File.Exists(ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture)==false?
                //                                                 //ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture :ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png"),
                //                                                 AssetPicture = checkImageExists(x.AssetPicture, "Content/Images/ProjectLogo/"),
                //                                                 PurchaseTypeRemark = x.PurchaseTypeRemark
                //                                             }).ToList<QRCModel>();

                var qrcmaster = _workorderEMSEntities.QRCMasters.Join(_workorderEMSEntities.UserRegistrations, qm => qm.CreatedBy, u => u.UserId, (qm, u) => new { qm, u }).Where(x => ((LocationID == 0 ? null : LocationID) == null || x.qm.LocationId == LocationID)
                                                       && x.qm.PurchaseType == OwnedType).Select(x => new QRCModel()
                                                       {
                                                           QRCodeID = x.qm.QRCodeID,
                                                           QRCName = x.qm.QRCName,
                                                           QRCTYPECaption = x.qm.GlobalCode5.CodeName,
                                                           UserName = x.u.FirstName + " " + x.u.LastName,// listuser.Where(y => y.UserId == x.CreatedBy).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault(),
                                                           //UserName = listuser.Where(y => y.UserId == x.CreatedBy).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault(),
                                                           CreatedDate = x.qm.CreatedDate,
                                                           // CreatedOn = x.qm.CreatedDate.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                                                           SpecialNotes = x.qm.SpecialNotes == null ? "Not Available" : x.qm.SpecialNotes,
                                                           VendorName = x.qm.VendorName == null ? "Not Available" : x.qm.VendorName,
                                                           //AssetPicture = (x.AssetPicture == null) ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : (
                                                           //File.Exists(ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture)==false?
                                                           //ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture :ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png"),
                                                           AssetPicture = x.qm.AssetPicture,
                                                           PurchaseTypeRemark = x.qm.PurchaseTypeRemark
                                                       }).ToList<QRCModel>();

                qrcmaster = qrcmaster.Select(x => new QRCModel()
                {
                    QRCodeID = x.QRCodeID,
                    QRCName = x.QRCName,
                    QRCTYPECaption = x.QRCTYPECaption,
                    UserName = x.UserName,
                    CreatedDate = x.CreatedDate,
                    CreatedOn = x.CreatedDate.ToClientTimeZone(true), //    .ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    SpecialNotes = x.SpecialNotes == null ? "Not Available" : x.SpecialNotes,
                    VendorName = x.VendorName == null ? "Not Available" : x.VendorName,
                    //AssetPicture = (x.AssetPicture == null) ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : (
                    //File.Exists(ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture)==false?
                    //ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.AssetPicture :ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png"),
                    AssetPicture = checkImageExists(x.AssetPicture, "Content/Images/ProjectLogo/"),
                    PurchaseTypeRemark = x.PurchaseTypeRemark
                }).ToList<QRCModel>();

                return qrcmaster;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<QRCModel> PurchaseTypeList(long? LocationID, int OwnedType)", "LocationID", LocationID);
                return null;
            }
        }
        //shubham 08112016
        public string checkImageExists(string filename, string specificPath)
        {
            var url = String.Empty;
            filename = filename == null ? "" : filename;
            if (filename != "")
                url = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + specificPath + filename;
            else
                url = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + specificPath + "defaultImage.png";

            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException)
            {
                if (response != null)
                    response.Close();
                else
                    url = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + specificPath + "defaultImage.png";
            }
            return url;
        }
        //end 08112016
        /// <summary>Created by Bhushan Dod on 06/05/2015
        /// Get details of Damage in veghicle for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetDamageVehicleList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            var extractXML = new List<ServiceQrcVehicleModel>();

            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                var temp = new List<ServiceQrcVehicleModelTemp>();
                try
                {
                    // bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //Converted to UTC because datetime in utc in db.
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    //if (FromDate != null)
                    //{
                    //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                    //    {
                    //        TimeSpan ts = new TimeSpan(00, 00, 00);
                    //        _fromDate = _fromDate.Date + ts;
                    //    }
                    //}
                    var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                                       && (x.QRCMaster.QRCTYPE == (long)QrcType.Vehicle)
                                                       && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                                       && x.QRCMaster.ClientTypeID == null
                                                       ).ToList();

                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        var temp1 = new ServiceQrcVehicleModelTemp();
                        if (filterlst.QRCTypeDetails != null)
                        {
                            temp1.ServiceQrcVehicleProp = GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(filterlst.QRCTypeDetails);
                            temp1.QrclogId = filterlst.QRCLogId;
                            temp1.QrcName = filterlst.QRCMaster.QRCName;
                            temp.Add(temp1);
                        }
                    }
                    if (temp.Count > 0)
                    {
                        if (name.Replace(" ", "") == "CheckOut")
                        {
                            lstRoutine = temp.Join(_workorderEMSEntities.QRCMasters, q => q.ServiceQrcVehicleProp.QrcId, u => u.QRCID, (q, u) => new { q, u }).Where(x => x.q.ServiceQrcVehicleProp.CheckingOut.IsDamage == true).Select(x => new ReportChart()
                            {
                                QrcName = x.q.QrcName,
                                CreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.q.QrclogId).FirstOrDefault().CreatedOn,
                                StrCreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.q.QrclogId).FirstOrDefault().CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),
                                ScanUserName = listuser.Where(f => f.UserId == x.q.ServiceQrcVehicleProp.UserId).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.q.ServiceQrcVehicleProp.UserId).FirstOrDefault().LastName,
                                QrcTypeName = x.u.GlobalCode5.CodeName,
                                KeyName = x.u.QRCodeID,
                                CroppedPicture = x.q.ServiceQrcVehicleProp.CheckingOut.CroppedPicture,
                                CapturedPicture = x.q.ServiceQrcVehicleProp.CheckingOut.CapturedImage,

                            }).OrderByDescending(q => q.StrCreatedDate).ToList<ReportChart>();
                        }
                        if (name.Replace(" ", "") == "WeeklyCheckOut")
                        {
                            lstRoutine = temp.Join(_workorderEMSEntities.QRCMasters, q => q.ServiceQrcVehicleProp.QrcId, u => u.QRCID, (q, u) => new { q, u }).Where(x => x.q.ServiceQrcVehicleProp.VehicleCheck.IsDamage == true).Select(x => new ReportChart()
                            {
                                QrcName = x.q.QrcName,
                                CreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.q.QrclogId).FirstOrDefault().CreatedOn,
                                StrCreatedDate = TemplstRoutine.Where(f => f.QRCLogId == x.q.QrclogId).FirstOrDefault().CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),
                                ScanUserName = listuser.Where(f => f.UserId == x.q.ServiceQrcVehicleProp.UserId).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.q.ServiceQrcVehicleProp.UserId).FirstOrDefault().LastName,
                                QrcTypeName = x.u.GlobalCode5.CodeName,
                                KeyName = x.u.QRCodeID,
                                CroppedPicture = x.q.ServiceQrcVehicleProp.VehicleCheck.CroppedPicture,
                                CapturedPicture = x.q.ServiceQrcVehicleProp.VehicleCheck.CapturedImage,

                            }).OrderByDescending(q => q.StrCreatedDate).ToList<ReportChart>();
                        }

                    }
                    if (lstRoutine.Count > 0)
                    {
                        var defaultimage = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png";
                        foreach (var t in lstRoutine)
                        {
                            if (t.CroppedPicture != null && t.CroppedPicture.Replace(" ", "") != "" && t.CroppedPicture.Trim() != "")
                            {
                                t.CroppedPicture = t.CroppedPicture.Replace(" ", "");
                                var list1 = t.CroppedPicture.Split(',').ToList();
                                if (list1.Count != 0 && list1.Count == 1)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    //t.CroppedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    t.CroppedPicture2 = defaultimage;
                                    t.CroppedPicture3 = defaultimage;
                                    t.CroppedPicture4 = defaultimage;
                                }
                                if (list1.Count != 0 && list1.Count == 2)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");

                                    //t.CroppedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CroppedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    t.CroppedPicture3 = defaultimage;
                                    t.CroppedPicture4 = defaultimage;
                                }
                                if (list1.Count != 0 && list1.Count == 3)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture3 = checkImageExists(list1[2], "Content/Images/QRCVehicle/");

                                    //t.CroppedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CroppedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    //t.CroppedPicture3 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[2];
                                    t.CroppedPicture4 = defaultimage;
                                }
                                if (list1.Count != 0 && list1.Count == 4)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture3 = checkImageExists(list1[2], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture4 = checkImageExists(list1[3], "Content/Images/QRCVehicle/");

                                    //t.CroppedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CroppedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    //t.CroppedPicture3 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[2];
                                    //t.CroppedPicture4 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[3];
                                }
                                if (list1.Count != 0 && list1.Count > 4)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture3 = checkImageExists(list1[2], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture4 = checkImageExists(list1[3], "Content/Images/QRCVehicle/");
                                    //t.CroppedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    //t.CroppedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[2];
                                    //t.CroppedPicture3 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[3];
                                    //t.CroppedPicture4 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[4];
                                }
                            }//shubham 27102016
                            else if (t.CroppedPicture == "")
                            {
                                t.CroppedPicture1 = defaultimage;
                                t.CroppedPicture2 = defaultimage;
                                t.CroppedPicture3 = defaultimage;
                                t.CroppedPicture4 = defaultimage;
                            }
                            if (t.CapturedPicture != null && t.CapturedPicture.Replace(" ", "") != "" && t.CapturedPicture.Trim() != "")
                            {
                                t.CapturedPicture = t.CapturedPicture.Replace(" ", "");
                                var list1 = t.CapturedPicture.Split(',').ToList();
                                if (list1.Count != 0 && list1.Count == 1)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    //t.CapturedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    t.CapturedPicture2 = defaultimage;
                                    t.CapturedPicture3 = defaultimage;
                                    t.CapturedPicture4 = defaultimage;
                                }
                                if (list1.Count != 0 && list1.Count == 2)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    //t.CapturedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CapturedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    t.CapturedPicture3 = defaultimage;
                                    t.CapturedPicture4 = defaultimage;
                                }
                                if (list1.Count != 0 && list1.Count == 3)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture3 = checkImageExists(list1[2], "Content/Images/QRCVehicle/");

                                    //t.CapturedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CapturedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    //t.CapturedPicture3 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[2];
                                    t.CapturedPicture4 = defaultimage;
                                }//shubham 27102016
                                if (list1.Count != 0 && list1.Count == 4)
                                {
                                    t.CroppedPicture1 = checkImageExists(list1[0], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture2 = checkImageExists(list1[1], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture3 = checkImageExists(list1[2], "Content/Images/QRCVehicle/");
                                    t.CroppedPicture4 = checkImageExists(list1[3], "Content/Images/QRCVehicle/");
                                    //t.CapturedPicture1 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[0];
                                    //t.CapturedPicture2 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[1];
                                    //t.CapturedPicture3 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[2];
                                    //t.CapturedPicture4 = ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/QRCVehicle/" + list1[3];
                                }
                            }//shubham 27102016
                            else if (t.CapturedPicture == "")
                            {
                                t.CapturedPicture1 = defaultimage;
                                t.CapturedPicture2 = defaultimage;
                                t.CapturedPicture3 = defaultimage;
                                t.CapturedPicture4 = defaultimage;
                            }
                        }
                    }

                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDamageVehicleList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                ReportChart objReportChart;
                try
                {
                    // bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //Converted to UTC because datetime in utc in db.
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.QRCMasterLogs.Where(x => ((UserId == 0 ? null : UserId) == null || x.UserId == UserId)
                                              && ((LocationId == 0 ? null : LocationId) == null || x.QRCMaster.LocationId == LocationId)
                                              && (x.QRCMaster.QRCTYPE == (long)QrcType.Vehicle)
                                              && (x.CreatedOn >= _fromDate && x.CreatedOn <= _toDate)
                                               && x.QRCMaster.ClientTypeID == null
                                              ).ToList();

                    foreach (var filterlst in TemplstRoutine.ToList())
                    {
                        if (filterlst.QRCTypeDetails != null)
                        {
                            extractXML.Add(GenericDataContractSerializer<ServiceQrcVehicleModel>.DeserializeXml(filterlst.QRCTypeDetails));
                        }
                    }
                    if (extractXML.Count > 0)
                    {
                        var checkout = extractXML.GroupBy(x => x.CheckingOut.IsDamage == true).Select(x => new ReportChart()
                        {
                            QrcTypeCount = x.Count(),
                            QrcTypeName = "Check Out",
                            KeyName = x.Key.ToString()
                        }).ToList<ReportChart>();

                        var weekcheckout = extractXML.GroupBy(x => x.VehicleCheck.IsDamage == true).Select(x => new ReportChart()
                        {
                            QrcTypeCount = x.Count(),
                            QrcTypeName = "Weekly Check Out",
                            KeyName = x.Key.ToString()
                        }).ToList<ReportChart>();

                        foreach (var i in checkout)
                        {
                            objReportChart = new ReportChart();
                            if (i.KeyName == "True")
                            {
                                objReportChart.QrcTypeCount = i.QrcTypeCount;
                                objReportChart.QrcTypeName = i.QrcTypeName;
                                lstRoutine.Add(objReportChart);
                            }

                        }
                        foreach (var i in weekcheckout)
                        {
                            objReportChart = new ReportChart();
                            if (i.KeyName == "True")
                            {
                                objReportChart.QrcTypeCount = i.QrcTypeCount;
                                objReportChart.QrcTypeName = i.QrcTypeName;
                                lstRoutine.Add(objReportChart);
                            }

                        }
                    }

                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDamageVehicleList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 11/05/2015
        /// Get details of all qrc type
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetDetailsOfAllQrc(long? LocationId, long? UserId, string name)
        {
            if (name != null && name.Trim() != "")
            {
                var lstRoutine = new List<ReportChart>();
                try
                {
                    lstRoutine = _workorderEMSEntities.QRCMasters.Join(_workorderEMSEntities.UserRegistrations, q => q.CreatedBy, u => u.UserId, (q, u) => new { q, u }).
                        Where(x => ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && x.q.GlobalCode5.CodeName == name
                                                   && x.q.ClientTypeID == null
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.q.CreatedBy,
                                                       ScanUserName = x.u.FirstName + " " + x.u.LastName,
                                                       CreatedDate = x.q.CreatedDate,
                                                       QrcName = x.q.QRCName,
                                                       KeyName = x.q.QRCodeID,
                                                       QrcTypeName = x.q.SpecialNotes,
                                                       CroppedPicture = x.q.AssetPicture,
                                                       CapturedPicture = x.q.LocationPicture
                                                   }).OrderByDescending(x => x.CreatedDate).ToList<ReportChart>();
                    var tmp = lstRoutine.Select(x => new ReportChart()
                    {
                        ScanUserId = x.ScanUserId,
                        ScanUserName = x.ScanUserName,
                        StrCreatedDate = x.CreatedDate.ToClientTimeZone(true), // ..ToString("MM'/'dd'/'yyyy hh:mm tt"),
                        QrcName = x.QrcName,
                        KeyName = x.KeyName,
                        CroppedPicture = checkImageExists(x.CroppedPicture, "Content/Images/ProjectLogo/"),//x.CroppedPicture == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.CroppedPicture,
                        CapturedPicture = checkImageExists(x.CapturedPicture, "Content/Images/ProjectLogo/"),//x.CapturedPicture == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/" + x.CapturedPicture,
                        QrcTypeName = x.QrcTypeName == null ? "N/A" : x.QrcTypeName,
                        CreatedDate = x.CreatedDate,

                    }).ToList();


                    return tmp;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDetailsOfAllQrc(long? LocationId, long? UserId, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                try
                {
                    var TemplstRoutine = _workorderEMSEntities.QRCMasters.Where(x => ((LocationId == 0 ? null : LocationId) == null || x.LocationId == LocationId)
                                         && x.ClientTypeID == null).GroupBy(x => x.QRCTYPE).
                                          Select(x => new ReportChart()
                                          {
                                              QrcType = x.Key,
                                              QrcTypeCount = x.Count(),
                                              QrcTypeName = x.FirstOrDefault().GlobalCode5.CodeName//listGlobalCodes.Where(f => f.GlobalCodeId == x.Key).FirstOrDefault().CodeName,
                                          }).ToList();
                    return TemplstRoutine;
                }
                catch (Exception ex)
                {
                    List<ReportChart> lstRoutine = new List<ReportChart>();
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDetailsOfAllQrc(long? LocationId, long? UserId, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        //public List<WorkOrderIssueedModel> GetWorkOrderMissedTime(long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType,long? PriorityLevel, long? UserId, string textSearch)
        //{
        //    try
        //    {
        //        return objReportRepository.GetWorkOrderAcceptedandCompleted(LocationID, FromDate, ToDate, WorkRequestProjectType, PriorityLevel, UserId, textSearch);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<WorkOrderIssueedModel> GetWorkOrderMissedTime(long? LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? PriorityLevel, long? UserId, string textSearch)
        {
            List<WorkOrderIssueedModel> lstWorkOrder = new List<WorkOrderIssueedModel>();
            try
            {
                ReportRepository objReport = new ReportRepository();
                lstWorkOrder = objReport.GetWorkOrderAcceptedandCompleted(LoginUserID, LocationID, FromDate, ToDate, WorkRequestProjectType, PriorityLevel, UserId, textSearch).Select(r => new WorkOrderIssueedModel()
                {
                    //AssignBy = r.AssignBy,
                    AssignTo = r.AssignTo,
                    // CreatedDate = Convert.ToDateTime(r.CreatedDate).ToClientTimeZone(true),
                    CreatedDate = Convert.ToDateTime(r.CreatedDate).ToString("MM/dd/yyyy hh:mm tt"),
                    PriorityLevel = r.PriorityLevel,
                    ProblemDesc = r.ProblemDesc,
                    ProjectDesc = r.ProjectDesc,
                    RequestBy = r.RequestBy,
                    //WorkRequestType = r.WorkRequestType,
                    StartTime = r.StartTime.ToString(),
                    EndTime = r.EndTime.ToString(),
                    FixedTime = r.TimeFrame,
                    MissedTime = r.MissedTime,
                    AssignedTime = r.AssignedTime,
                    CodeID = r.CodeID

                }).ToList();

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<WorkOrderIssueedModel> GetWorkOrderMissedTime(long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? PriorityLevel, long? UserId, string textSearch)", "LocationID", LocationID);
                return lstWorkOrder;
            }
            return lstWorkOrder;
        }

        /// <summary>Created by Bhushan Dod on 17/06/2015
        /// Get details of all DAR Customer Assistance(DAR Code - 22)
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<DARModel> GetDetailsOfDARCustomerAssistance(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (UserId == 0)
            {
                UserId = null;
            }
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                var AssistanceList = _workorderEMSEntities.DARDetails.Join(_workorderEMSEntities.UserRegistrations, q => q.UserId, u => u.UserId, (q, u) => new { q, u }).
                    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                               && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                               && (x.q.TaskType == (long)TaskTypeCategory.CustomerAssistance)
                                               && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)).ToList();
                var tmp = AssistanceList.Select(r => new DARModel()
                {

                    ActivityDetails = r.q.ActivityDetails,
                    UserName = r.u.FirstName + " " + r.u.LastName,
                    StartTime = r.q.StartTime == null ? "Not Available" : r.q.StartTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"), //ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    EndTime = r.q.EndTime == null ? "Not Available" : r.q.EndTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),//  ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StartTimeImage = checkImageExists(r.q.StartTimeImage, "Content/Images/DarImage/"),//r.q.StartTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.StartTimeImage,
                    EndTimeImage = checkImageExists(r.q.EndTimeImage, "Content/Images/DarImage/"),//r.q.EndTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.EndTimeImage,
                    CreatedDate = r.q.CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"), // ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    CreatedOn = r.q.CreatedOn
                }).ToList<DARModel>();

                return tmp;
            }
            catch (Exception ex)
            {
                List<DARModel> lstWorkOrder = new List<DARModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<DARModel> GetDetailsOfDARCustomerAssistance(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstWorkOrder;
            }

        }

        /// <summary>Created by Bhushan Dod on 22/06/2015
        /// Get details of all DAR Jump Start(DAR Code - 20)
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<DARModel> GetDetailsOfDARJumpStarts(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (UserId == 0)
            {
                UserId = null;
            }
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                var JumpstartList = _workorderEMSEntities.DARDetails.Join(_workorderEMSEntities.UserRegistrations, q => q.UserId, u => u.UserId, (q, u) => new { q, u }).
                    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                               && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                               && (x.q.TaskType == (long)TaskTypeCategory.CustomerJumpStart)
                                                && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)).ToList();
                var tmp = JumpstartList.Select(r => new DARModel()
                {
                    ActivityDetails = r.q.ActivityDetails,
                    UserName = r.u.FirstName + " " + r.u.LastName,
                    StartTime = r.q.StartTime == null ? "Not Available" : r.q.StartTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    EndTime = r.q.EndTime == null ? "Not Available" : r.q.EndTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),//    .ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StartTimeImage = checkImageExists(r.q.StartTimeImage, "Content/Images/DarImage/"),//r.q.StartTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.StartTimeImage,
                    EndTimeImage = checkImageExists(r.q.EndTimeImage, "Content/Images/DarImage/"),//r.q.EndTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.EndTimeImage,
                    CreatedDate = r.q.CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"), //  ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    CreatedOn = r.q.CreatedOn
                }).ToList<DARModel>();

                return tmp;
            }
            catch (Exception ex)
            {
                List<DARModel> lstWorkOrder = new List<DARModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<DARModel> GetDetailsOfDARJumpStarts(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstWorkOrder;
            }

        }

        /// <summary>Created by Bhushan Dod on 22/06/2015
        /// Get details of all DAR Tire Inflation(DAR Code - 20)
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<DARModel> GetDetailsOfDARTireInflation(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (UserId == 0)
            {
                UserId = null;
            }
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "412:00:00 AM") ? true : false;
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                var JumpstartList = _workorderEMSEntities.DARDetails.Join(_workorderEMSEntities.UserRegistrations, q => q.UserId, u => u.UserId, (q, u) => new { q, u }).
                   Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                              && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                              && (x.q.TaskType == (long)TaskTypeCategory.Customertireinflation)
                                              && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)).ToList();
                var tmp = JumpstartList.Select(r => new DARModel()
                {
                    ActivityDetails = r.q.ActivityDetails,
                    UserName = r.u.FirstName + " " + r.u.LastName,
                    StartTime = r.q.StartTime == null ? "Not Available" : r.q.StartTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"), // ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    EndTime = r.q.EndTime == null ? "Not Available" : r.q.EndTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),// ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StartTimeImage = checkImageExists(r.q.StartTimeImage, "Content/Images/DarImage/"),//r.q.StartTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.StartTimeImage,
                    EndTimeImage = checkImageExists(r.q.EndTimeImage, "Content/Images/DarImage/"),//r.q.EndTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.EndTimeImage,
                    CreatedDate = r.q.CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),   //ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    CreatedOn = r.q.CreatedOn
                }).ToList<DARModel>();

                return tmp;
            }
            catch (Exception ex)
            {
                List<DARModel> lstWorkOrder = new List<DARModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<DARModel> GetDetailsOfDARTireInflation(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstWorkOrder;
            }

        }

        /// <summary>Created by Bhushan Dod on 22/06/2015
        /// Get details of all DAR Space Count(DAR Code - 26)
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<DARModel> GetDetailsOfDARSpaceCount(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (UserId == 0)
            {
                UserId = null;
            }
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                var JumpstartList = _workorderEMSEntities.DARDetails.Join(_workorderEMSEntities.UserRegistrations, q => q.UserId, u => u.UserId, (q, u) => new { q, u }).
                    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserId == UserId)
                                               && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                               && (x.q.TaskType == (long)TaskTypeCategory.SpaceCount)
                                               && (x.q.StartTime != null)
                                               && (x.q.EndTime != null)
                                                && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)).ToList();
                var tmp = JumpstartList.Select(r => new DARModel()
                {
                    ActivityDetails = r.q.ActivityDetails,
                    UserName = r.u.FirstName + " " + r.u.LastName,
                    StartTime = r.q.StartTime == null ? "Not Available" : r.q.StartTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"), //.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    EndTime = r.q.EndTime == null ? "Not Available" : r.q.EndTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),   //.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StartTimeImage = checkImageExists(r.q.StartTimeImage, "Content/Images/DarImage/"),//r.q.StartTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.StartTimeImage,
                    EndTimeImage = checkImageExists(r.q.EndTimeImage, "Content/Images/DarImage/"),//r.q.EndTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/defaultImage.png" : ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/DarImage/" + r.q.EndTimeImage,
                    CreatedDate = r.q.CreatedOn.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),   //ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    CreatedOn = r.q.CreatedOn,
                    Description = r.q.Description
                }).OrderByDescending(x => x.CreatedOn).ToList<DARModel>();

                return tmp;
            }
            catch (Exception ex)
            {
                List<DARModel> lstWorkOrder = new List<DARModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<DARModel> GetDetailsOfDARSpaceCount(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstWorkOrder;
            }

        }

        /// <summary>Created by Bhushan Dod on 26/06/2015
        ///  Get details of DAR Codes for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetDARCodesList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (UserId == 0) { UserId = null; }
            if (name != null)
            {
                List<ReportChart> lstCodes = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();

                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    var Templst = _workorderEMSEntities.DARDetails
                                    .Join(_workorderEMSEntities.UserRegistrations, d => d.UserId, u => u.UserId, (d, u) => new { d, u })
                                    .Join(_workorderEMSEntities.GlobalCodes, (z => z.d.TaskType), (t => t.GlobalCodeId), (z, t) => new { z, t })
                                    .Where(x => ((UserId == 0 ? null : UserId) == null || x.z.d.UserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.z.d.LocationId == LocationId)
                                                    && (x.z.d.CreatedOn >= _fromDate && x.z.d.CreatedOn <= _toDate)
                                                   //&& (x.z.d.CreatedOn >= _fromDate && x.z.d.CreatedOn <= _toDate)
                                                   && x.t.CodeName == name
                                                   && x.z.d.TaskType != (long)TaskTypeCategory.ShiftEnd // This line is harcoded due to shift end is not DAR Codes
                                                   ).ToList();

                    lstCodes = Templst.GroupBy(x => x.z.d.UserId).Select(x => new ReportChart()
                    {
                        ScanUserId = x.Key,
                        ScanUserCount = x.Count(),
                        ScanUserName = x.FirstOrDefault().z.u.FirstName + " " + x.FirstOrDefault().z.u.LastName
                    }).ToList<ReportChart>();

                    return lstCodes;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDARCodesList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstCodes;
                }
            }
            else
            {
                List<ReportChart> lstCodes = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    var TemplstRoutine = _workorderEMSEntities.DARDetails
                                            .Join(_workorderEMSEntities.UserRegistrations, d => d.UserId, u => u.UserId, (d, u) => new { d, u })
                                            .Join(_workorderEMSEntities.GlobalCodes, (z => z.d.TaskType), (t => t.GlobalCodeId), (z, t) => new { z, t })
                                              .Where(x => ((UserId == 0 ? null : UserId) == null || x.z.d.UserId == UserId)
                                                    && ((LocationId == 0 ? null : LocationId) == null || x.z.d.LocationId == LocationId)
                                                     && (x.z.d.CreatedOn >= _fromDate && x.z.d.CreatedOn <= _toDate)
                                                    && (x.z.d.TaskType == (long)TaskTypeCategory.GateRepair
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.CustomerVehicleLocate
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.CustomerJumpStart
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.Customertireinflation
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.CustomerAssistance
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.WorkBreak
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.SpecialProject
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.RoutineChecks
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.SpaceCount
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.LicensePlateInventory
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.Emergency
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.Facilitycleaning
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.FacilitySpillResponse
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.SnowRemoval
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.TicketSpitterRepair
                                                        || x.z.d.TaskType == (long)TaskTypeCategory.MiscellaneousEvent)
                                              //|| x.z.d.TaskType == 280)
                                              ).ToList();

                    lstCodes = TemplstRoutine.GroupBy(x => x.z.d.TaskType).Select(x => new ReportChart()
                    {
                        QrcType = x.Key,
                        QrcTypeCount = x.Count(),
                        QrcTypeName = x.FirstOrDefault().t.CodeName
                    }).ToList<ReportChart>();

                    return lstCodes;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetDARCodesList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstCodes;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 26/06/2015
        /// Get details of Employee  DAR Codes submitted
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> DARSubmittedListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            string DARPath = ConfigurationManager.AppSettings["hostingprefix"];

            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.DARDetails
                                                     .Join(_workorderEMSEntities.UserRegistrations, q => q.UserId, u => u.UserId, (q, u) => new { q, u })
                                                     .Join(_workorderEMSEntities.GlobalCodes, (z => z.q.TaskType), (t => t.GlobalCodeId), (z, t) => new { z, t })
                                                     .Where(x => ((x.z.u.FirstName + " " + x.z.u.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.z.q.LocationId == LocationId)
                                                   && x.t.CodeName == qrcName
                                                    && (x.z.q.CreatedOn >= _fromDate && x.z.q.CreatedOn <= _toDate)
                                                   // && (x.z.q.CreatedOn >= _fromDate && x.z.q.CreatedOn <= _toDate)
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.z.q.UserId,
                                                       ScanUserName = x.z.u.FirstName + " " + x.z.u.LastName,
                                                       CreatedDate = x.z.q.CreatedOn,
                                                       QrcName = x.t.CodeName,
                                                       Description = x.z.q.Description,
                                                       StartTime = x.z.q.StartTime,
                                                       EndTime = x.z.q.EndTime,
                                                       StartTimeImage = x.z.q.StartTimeImage,
                                                   }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    CreatedDate = x.CreatedDate,
                    StrCreatedDate = x.CreatedDate.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    QrcName = x.QrcName,
                    StrStartTime = x.StartTime == null ? "N/A" : x.StartTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),    //.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    StrEndTime = x.EndTime == null ? "N/A" : x.EndTime.Value.GetClientDateTimeNow().ToString("MM'/'dd'/'yyyy hh:mm tt"),//  .ToString("MM'/'dd'/'yyyy hh:mm tt"),
                    Description = x.Description == null ? "N/A" : x.Description,
                    StartTimeImage = checkImageExists(x.StartTimeImage, "Content/Images/DarImage/"),//x.StartTimeImage == null ? ConfigurationManager.AppSettings["hostingPrefix"].ToString() + "Content/Images/ProjectLogo/defaultImage.png" : DARPath + "Content/Images/DarImage/" + x.StartTimeImage
                }).ToList();
                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> DARSubmittedListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 27/07/2015
        /// Get details of Completed Work Order for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetCompletedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                    var TemplstRoutine = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.GlobalCodes, q => q.WorkRequestProjectType, u => u.GlobalCodeId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.AssignToUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationID == LocationId)
                                                   && (x.q.WorkRequestStatus == (long)WorkRequestStatus1.Complete)
                                                   && (x.u.CodeName == name)
                                                    && (x.q.EndTime != null)
                                                    && (x.q.EndTime >= _fromDate && x.q.EndTime <= _toDate)
                                                   ).ToList();

                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.AssignToUserId).Select(x => new ReportChart()
                    {
                        ScanUserId = x.Key.Value,
                        ScanUserCount = x.Count(),
                        ScanUserName = listuser.Where(f => f.UserId == x.Key.Value).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key.Value).FirstOrDefault().LastName,
                        //   ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                    }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetCompletedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    ////Converted to UTC because datetime in utc in db.
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.GlobalCodes, q => q.WorkRequestProjectType, u => u.GlobalCodeId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.AssignToUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationID == LocationId)
                                                   && (x.q.WorkRequestStatus == (long)WorkRequestStatus1.Complete)
                                                   && (x.q.EndTime != null)
                                                    && (x.q.EndTime >= _fromDate && x.q.EndTime <= _toDate)
                                              ).ToList();
                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.WorkRequestProjectType).Select(x => new ReportChart()
                    {
                        QrcType = x.Key,
                        QrcTypeCount = x.Count(),
                        QrcTypeName = x.FirstOrDefault().u.CodeName
                    }).ToList<ReportChart>();

                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetCompletedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 28/07/2015
        /// Get details of Employee work order 
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> WorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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

            // var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();           
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {

                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                ////Converted to UTC because datetime in utc in db.
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.WorkRequestAssignments.
                                        Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                        Join(_workorderEMSEntities.GlobalCodes, t => t.q.WorkRequestProjectType, s => s.GlobalCodeId, (t, s) => new { t, s }).
                                        Where(x => ((x.t.u.FirstName + " " + x.t.u.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.t.q.LocationID == LocationId)
                                                   && (x.t.q.WorkRequestStatus == (long)WorkRequestStatus1.Complete)
                                                   && x.s.CodeName == qrcName
                                                    && (x.t.q.EndTime != null)
                                                   && (x.t.q.EndTime >= _fromDate && x.t.q.EndTime <= _toDate)
                                                   // && (x.t.q.EndTime != null)
                                                   //&& (isDateOnly == true ? (DbFunctions.TruncateTime(x.t.q.EndDate) >= _fromDate.Date && (DbFunctions.TruncateTime(x.t.q.EndDate) <= _toDate.Date)) : (x.t.q.EndDate >= _fromDate && x.t.q.EndDate <= _toDate))
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.t.q.AssignToUserId.Value,
                                                       ScanUserName = x.t.u.FirstName + " " + x.t.u.LastName,
                                                       CompletedDate = x.t.q.EndTime.Value,
                                                       QrcName = x.s.CodeName,
                                                       WorkOrderCode = x.t.q.WorkOrderCode + x.t.q.WorkOrderCodeID,
                                                       ProblemDescription = (x.t.q.ProblemDesc == null) ? "Not Available" : x.t.q.ProblemDesc,
                                                       ProjectDescription = (x.t.q.ProjectDesc == null) ? "Not Available" : x.t.q.ProjectDesc,
                                                       CreatedDate = x.t.q.CreatedDate
                                                   }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.GetClientDateTimeNow().ToString(),
                    QrcName = x.QrcName,
                    WorkOrderCode = x.WorkOrderCode,
                    ProblemDescription = x.ProblemDescription,
                    ProjectDescription = x.ProjectDescription,
                    CreatedDate = x.CreatedDate,
                    StrCompletedDate = x.CompletedDate.GetClientDateTimeNow().ToString()
                }).ToList();
                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> WorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 27/07/2015
        /// Get details of Issued Work Order for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetIssuedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            objQRCScanLogRepository = new QRCScanLogRepository();
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                ////Converted to UTC because datetime in utc in db.
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                var TemplstRoutine = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.GlobalCodes, q => q.WorkRequestProjectType, u => u.GlobalCodeId, (q, u) => new { q, u }).
                    Where(x => ((LocationId == 0 ? null : LocationId) == null || x.q.LocationID == LocationId)
                                                && ((UserId == 0 ? null : UserId) == null || x.q.CreatedBy == UserId)
                                                //&& (x.q.WorkRequestProjectType == 128 || x.q.WorkRequestProjectType == 129)
                                                && (x.q.CreatedDate >= _fromDate && x.q.CreatedDate <= _toDate)
                                          ).ToList();
                lstRoutine = TemplstRoutine.GroupBy(x => x.q.WorkRequestProjectType).Select(x => new ReportChart()
                {
                    QrcType = x.Key,
                    QrcTypeCount = x.Count(),
                    QrcTypeName = x.FirstOrDefault().u.CodeName
                }).ToList<ReportChart>();

                return lstRoutine;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetIssuedWorkOrder(long? LocationId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        /// <summary>Created by Bhushan Dod on 29/07/2015
        /// Get details of Assigned Work Order for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            if (name != null && name.Trim() != "")
            {
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    ////Converted to UTC because datetime in utc in db.
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                    var TemplstRoutine = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.GlobalCodes, q => q.WorkRequestProjectType, u => u.GlobalCodeId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.AssignToUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationID == LocationId)
                                                   && (x.q.AssignToUserId != null)
                                                   && (x.u.CodeName == name)
                                                   && (x.q.CreatedDate >= _fromDate && x.q.CreatedDate <= _toDate)

                                                   ).ToList();

                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.AssignToUserId).Select(x => new ReportChart()
                    {
                        ScanUserId = x.Key.Value,
                        ScanUserCount = x.Count(),
                        ScanUserName = listuser.Where(f => f.UserId == x.Key.Value).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key.Value).FirstOrDefault().LastName,
                        //   ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                    }).ToList<ReportChart>();
                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
            else
            {
                objQRCScanLogRepository = new QRCScanLogRepository();
                List<ReportChart> lstRoutine = new List<ReportChart>();
                try
                {
                    //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                    ////Converted to UTC because datetime in utc in db.
                    //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                    //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                    _fromDate = _fromDate.ConvertClientTZtoUTC();
                    _toDate = _toDate.ConvertClientTZtoUTC();
                    var TemplstRoutine = _workorderEMSEntities.WorkRequestAssignments.Join(_workorderEMSEntities.GlobalCodes, q => q.WorkRequestProjectType, u => u.GlobalCodeId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.AssignToUserId == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationID == LocationId)
                                                   && (x.q.AssignToUserId != null)
                                                   && (x.q.CreatedDate >= _fromDate && x.q.CreatedDate <= _toDate)
                                              ).ToList();
                    lstRoutine = TemplstRoutine.GroupBy(x => x.q.WorkRequestProjectType).Select(x => new ReportChart()
                    {
                        QrcType = x.Key,
                        QrcTypeCount = x.Count(),
                        QrcTypeName = x.FirstOrDefault().u.CodeName
                    }).ToList<ReportChart>();

                    return lstRoutine;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                    return lstRoutine;
                }
            }
        }

        /// <summary>Created by Bhushan Dod on 29/07/2015
        /// Get details of assigned work order employee list
        /// </summary>
        /// <param name="LocationId,qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<ReportChart> AssignedWorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)
        {
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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

            // var listGlobalCodes = _workorderEMSEntities.GlobalCodes.Select(x => x).ToList();           
            List<ReportChart> lstRoutine = new List<ReportChart>();
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                //bool isDateOnly = (_toDate.ToLongTimeString() == "12:00:00 AM") ? true : false;
                ////Converted to UTC because datetime in utc in db.
                //_fromDate = _fromDate.ToClientTimeZoneinDateTimeReports();
                //_toDate = _toDate.ToClientTimeZoneinDateTimeReports();
                //if (FromDate != null)
                //{
                //    if (FromDate.Value.ToLongTimeString() == "12:00:00 AM")
                //    {
                //        TimeSpan ts = new TimeSpan(00, 00, 00);
                //        _fromDate = _fromDate.Date + ts;
                //    }
                //}
                lstRoutine = _workorderEMSEntities.WorkRequestAssignments.
                                        Join(_workorderEMSEntities.UserRegistrations, q => q.AssignToUserId, u => u.UserId, (q, u) => new { q, u }).
                                        Join(_workorderEMSEntities.GlobalCodes, t => t.q.WorkRequestProjectType, s => s.GlobalCodeId, (t, s) => new { t, s }).
                                        Where(x => ((x.t.u.FirstName + " " + x.t.u.LastName) == userName)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.t.q.LocationID == LocationId)
                                                   && x.s.CodeName == qrcName
                                                   && x.t.q.AssignToUserId != null
                                                   && (x.t.q.CreatedDate >= _fromDate && x.t.q.CreatedDate <= _toDate)
                                                   // && (x.t.q.CreatedDate >= _fromDate && x.t.q.CreatedDate <= _toDate)
                                                   ).Select(x => new ReportChart()
                                                   {
                                                       ScanUserId = x.t.q.AssignToUserId.Value,
                                                       ScanUserName = x.t.u.FirstName + " " + x.t.u.LastName,
                                                       CreatedDate = x.t.q.CreatedDate,
                                                       QrcName = x.s.CodeName,
                                                       WorkOrderCode = x.t.q.WorkOrderCode + x.t.q.WorkOrderCodeID,
                                                       ProblemDescription = x.t.q.ProblemDesc,
                                                       ProjectDescription = x.t.q.ProjectDesc
                                                   }).ToList<ReportChart>();
                var tmp = lstRoutine.Select(x => new ReportChart()
                {
                    ScanUserId = x.ScanUserId,
                    ScanUserName = x.ScanUserName,
                    StrCreatedDate = x.CreatedDate.GetClientDateTimeNow().ToString(),
                    QrcName = x.QrcName,
                    WorkOrderCode = x.WorkOrderCode,
                    CreatedDate = x.CreatedDate,
                    ProblemDescription = (x.ProblemDescription == null) ? "Not Available" : x.ProblemDescription,
                    ProjectDescription = (x.ProjectDescription == null) ? "Not Available" : x.ProjectDescription
                }).ToList();
                return tmp;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> AssignedWorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
                return lstRoutine;
            }
        }

        #region eFleet report

        #region Fueling 
        public List<FuelJQGridList> GetFuelingReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)
        {
            try
            {
                //return objReportRepository.GetWorkOrderIssuedForAssignedLocationItem(LoginUserID, LocationID, FromDate, ToDate, QrcId, ReqType, WorkRequestProjectType, safetyHuzzard, priorityLevel, userId, textSearch);
                List<FuelJQGridList> lstFueling = new List<FuelJQGridList>();
                try
                {
                    lstFueling = _workorderEMSEntities.eFleetFuelings.Where(x => ((userId == 0 ? null : userId) == null || x.CreatedBy == userId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || x.LocationID == LocationId)
                                                      && ((FuelType == 0 ? null : FuelType) == null || x.FuelType == FuelType)
                                                       && (x.CreatedDate >= FromDate && x.CreatedDate <= ToDate)
                                                       ).Select(a => new FuelJQGridList()
                                                       {
                                                           VehicleNumber = a.VehicleNumber,
                                                           QRCodeID = a.QRCodeID,
                                                           Mileage = a.Mileage,
                                                           // CurrentFuel = a.CurrentFuel,
                                                           FuelTypeName = a.GlobalCode.CodeName,
                                                           //ReceiptNo = a.ReceiptNo,
                                                           FuelingDate = a.FuelingDate,
                                                           Gallons = a.Gallons,
                                                           PricePerGallon = a.PricePerGallon,
                                                           Total = a.Total,
                                                           GasStatioName = a.GasStatioName,
                                                           //CardNo = a.CardNo,
                                                           DriverName = a.DriverName,
                                                           FuelReceiptImage = a.FuelReceiptImage
                                                       }).ToList<FuelJQGridList>();
                    var tmp = lstFueling.Select(a => new FuelJQGridList()
                    {
                        VehicleNumber = a.VehicleNumber,
                        QRCodeID = a.QRCodeID,
                        Mileage = a.Mileage,
                        // CurrentFuel = a.CurrentFuel,
                        FuelTypeName = a.FuelTypeName,
                        //ReceiptNo = a.ReceiptNo,
                        FuelDate = a.FuelingDate.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                        Gallons = a.Gallons,
                        PricePerGallon = a.PricePerGallon,
                        Total = a.Total,
                        GasStatioName = a.GasStatioName,
                        //CardNo = a.CardNo,
                        DriverName = a.DriverName
                    }).ToList();
                    return tmp;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<FuelJQGridList> GetFuelingReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)", "LocationId", LocationId);
                    return lstFueling;
                }
            }
            catch (Exception ex)
            {
                List<FuelJQGridList> lstFueling = new List<FuelJQGridList>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<FuelJQGridList> GetFuelingReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)", "LocationID", LocationId);
                return lstFueling;
            }
        }
        #endregion Fueling

        #region Driver Details 
        public List<eFleetDriverReportModel> GetDriverDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)
        {
            try
            {
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                try
                {
                    lstDriverdetails = _workorderEMSEntities.eFleetDrivers.Join(_workorderEMSEntities.MasterStates, ed => ed.StateId, ms => ms.StateId, (ed, ms) => new { ed, ms }).
                     Where(a => a.ed.IsDeleted == false
                     && ((userId == 0 ? null : userId) == null || a.ed.EmployeeName == userId)
                         && (((textSearch == null ? null : textSearch) == null || a.ed.DriverLicenseNo.Contains(textSearch))

                                   //|| ((textSearch == null ? null : textSearch) == null || a.ur.FirstName.Contains(textSearch))
                                   || ((textSearch == null ? null : textSearch) == null || (a.ed.UserRegistration.FirstName + " " + a.ed.UserRegistration.LastName).Contains(textSearch))
                                 )
                 ).Select(a => new eFleetDriverReportModel()
                 {
                     DriverID = a.ed.DriverID,
                     DriverLicenseNo = a.ed.DriverLicenseNo,
                     CDLType = a.ed.CDLType,
                     EmployeeNameList = a.ed.UserRegistration.FirstName + " " + a.ed.UserRegistration.LastName,
                     CDLExpiration = a.ed.CDLExpiration,
                     MedicleCardExpiration = a.ed.MedicleCardExpiration,
                     MVRExpiration = a.ed.MVRExpiration,
                     DriverImage = (a.ed.UserRegistration.ProfileImage == null) ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + a.ed.UserRegistration.ProfileImage,
                     StateName = a.ms.StateName,
                     DOH = a.ed.UserRegistration.DOB
                     //(from em in db.UserRegistrations where em.UserId == a.EmployeeName select em.ProfileImage == null ? "" : HostingPrefix + ProfilePicPath.Replace("~", "") + em.ProfileImage).FirstOrDefault(),
                 }).ToList<eFleetDriverReportModel>();

                    //var driverList = lstDriverdetails.Select(x => new eFleetDriverReportModel()
                    //{
                    //    DriverID = x.DriverID,
                    //    DriverLicenseNo = x.DriverLicenseNo,
                    //    CDLType = x.CDLType,
                    //    EmployeeNameList = x.EmployeeNameList,
                    //    CDLExpiration = x.CDLExpiration,
                    //    MedicleCardExpiration = x.MedicleCardExpiration,
                    //    MVRExpiration = x.MVRExpiration,
                    //    DriverImage =  x.DriverImage,
                    //    StateName = x.StateName,
                    //    DOH = x.DOH.ToString("MM'/'dd'/'yyyy hh: mm tt")
                    //}).ToList();

                    return lstDriverdetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetDriverModel> GetDriverDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)", "LocationId", LocationId);
                    return lstDriverdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetDriverReportModel> lstDriverdetails = new List<eFleetDriverReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetDriverModel> GetDriverDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)", "LocationID", LocationId);
                return lstDriverdetails;
            }
        }
        #endregion Driver Details 

        #region eMaintenance 
        public List<eFleetMaintenanceModel> GeteMaintenanceDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)
        {
            try
            {
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
                List<eFleetMaintenanceModel> lstMaintenancedetails = new List<eFleetMaintenanceModel>();
                try
                {
                    lstMaintenancedetails = _workorderEMSEntities.eFleetMaintenances.Where(a => a.IsDeleted == false && a.LocationID == LocationId).Select(a => new eFleetMaintenanceModel()
                    {
                        PmID = a.PmID,
                        VehicleID = a.VehicleID,
                        MaintenanceID = a.MaintenanceID,
                        //QRCodeID = a.QRCodeID,
                        VehicleNumber = a.VehicleNumber,
                        MaintenanceTypeList = a.GlobalCode.CodeName,
                        DriverName = a.DriverName,
                        //ListReminderMetric = (from em in db.eFleetMeters where em.ID == a.ReminderMetric select em.MeterValue).FirstOrDefault(),//db.eFleetMeters.Where(rm => rm.ID == a.ReminderMetric).FirstOrDefault().MeterValue,                                                                                                                                       // (from rm in db.eFleetMeters where rm.ID == a.ReminderMetric select rm.MeterValue).FirstOrDefault(),
                        //ScheduledPM = a.ScheduledPM,
                        // Category = a.GlobalCode1.GlobalCodeId,      //(from gc in db.GlobalCodes where gc.GlobalCodeId == a.Category select gc.CodeName).FirstOrDefault(),
                        DaysOutOfService = a.DaysOutOfService,
                        ReminderMetricDesc = a.ReminderMetricDesc,
                        MaintenanceDate = a.MaintenanceDate,
                        TotalCost = a.TotalCost,
                        Miles = a.Miles,
                        Note = a.Note,
                        LabourCost = a.LabourCost,
                        PartsCost = a.PartsCost,
                        MaintenanceAttachment = a.MaintenanceAttachment,
                        OthersCosts = a.OthersCosts
                    }).ToList<eFleetMaintenanceModel>();
                    return lstMaintenancedetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetMaintenanceModel> GeteMaintenanceDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationId", LocationId);
                    return lstMaintenancedetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetMaintenanceModel> lstMaintenancedetails = new List<eFleetMaintenanceModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetMaintenanceModel> GeteMaintenanceDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationID", LocationId);
                return lstMaintenancedetails;
            }
        }
        #endregion eMaintenance 

        #region Incident 
        public List<eFleetReportIncidentDetails> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)
        {
            try
            {
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
                List<eFleetReportIncidentDetails> lstIncidentdetails = new List<eFleetReportIncidentDetails>();
                try
                {
                    lstIncidentdetails = _workorderEMSEntities.eFleetVehicleIncidents.Join(_workorderEMSEntities.MasterStates, vi => vi.StateId, ms => ms.StateId, (vi, ms) => new { vi, ms }).
                                                        Where(a => a.vi.IsDeleted == false
                                                        && ((userId == 0 ? null : userId) == null || a.vi.CreatedBy == userId)
                                                        && ((LocationId == 0 ? null : LocationId) == null || a.vi.LocationID == LocationId)
                                                        // && ((FuelType == 0 ? null : FuelType) == null || a.FuelType == FuelType)
                                                        && (a.vi.CreatedDate >= FromDate && a.vi.CreatedDate <= ToDate)).
                                            Select(a => new eFleetReportIncidentDetails()
                                            {
                                                VehicleID = a.vi.VehicleID,
                                                QRCodeID = a.vi.QRCodeID,
                                                IncidentID = a.vi.IncidentID,
                                                AccidentDate = a.vi.AccidentDate,
                                                VehicleNumber = a.vi.VehicleNumber,
                                                DriverName = a.vi.DriverName,
                                                Preventability = (bool)a.vi.Preventability,
                                                NumberOfInjuries = a.vi.NumberOfInjuries,
                                                // ListFuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == a.FuelType select gc.CodeName).FirstOrDefault(),
                                                Description = a.vi.Description,
                                                IncidentImage = a.vi.IncidentImage,
                                                StateName = a.ms.StateName,
                                                Fatalities = a.vi.NumberOfFatalities,
                                                City = a.vi.City
                                            }).ToList();
                    var tt = lstIncidentdetails.Select(a => new eFleetReportIncidentDetails()
                    {
                        VehicleID = a.VehicleID,
                        QRCodeID = a.QRCodeID,
                        IncidentID = a.IncidentID,
                        Accident = a.AccidentDate.ToString("MM'/'dd'/'yyyy hh:mm tt"),
                        VehicleNumber = a.VehicleNumber,
                        DriverName = a.DriverName,
                        Prevent = (a.Preventability == true) ? "Yes" : "No",
                        NumberOfInjuries = a.NumberOfInjuries,
                        // ListFuelType = (from gc in db.GlobalCodes where gc.GlobalCodeId == a.FuelType select gc.CodeName).FirstOrDefault(),
                        Description = a.Description,
                        IncidentImage = a.IncidentImage,
                        StateName = a.StateName,
                        Fatalities = a.Fatalities,
                        City = a.City
                    }).ToList();
                    return tt;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationId", LocationId);
                    return lstIncidentdetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetReportIncidentDetails> lstIncidentdetails = new List<eFleetReportIncidentDetails>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationID", LocationId);
                return lstIncidentdetails;
            }
        }
        #endregion Incident 

        #region Vehicle 
        public List<eFleetVehicleModel> GetVehicleDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)
        {
            try
            {
                string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
                string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
                List<eFleetVehicleModel> lstvehicledetails = new List<eFleetVehicleModel>();
                try
                {
                    lstvehicledetails = _workorderEMSEntities.eFleetVehicles.Where(a => (a.IsDeleted == false)
                                                       && ((userId == 0 ? null : userId) == null || a.CreatedBy == userId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || a.LocationID == LocationId)
                                                       && ((FuelType == 0 ? null : FuelType) == null || a.FuelType == FuelType)).
                                                        // && (a.CreatedDate >= FromDate && a.CreatedDate <= ToDate)).
                                                        Select(a => new eFleetVehicleModel()
                                                        {
                                                            VehicleID = a.VehicleID,
                                                            QRCodeID = a.QRCodeID,
                                                            VehicleIdentificationNo = a.VehicleIdentificationNo,
                                                            VehicleLicensePlateNo = a.VehicleLicensePlateNo,
                                                            VehicleNumber = a.VehicleNumber,
                                                            Make = a.Make,
                                                            ListFuelType = a.GlobalCode.CodeName,
                                                            Model = a.Model,
                                                            GVWR = a.GVWR,
                                                            Year = a.Year,
                                                            StorageAddress = a.StorageAddress,
                                                            VehicleImage = a.VehicleImage,
                                                            DummyField = a.DummyField,
                                                            ExpirationDate = a.ExpirationDate
                                                        }).OrderByDescending(s => s.QRCodeID).ToList();

                    return lstvehicledetails;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationId", LocationId);
                    return lstvehicledetails;
                }
            }
            catch (Exception ex)
            {
                List<eFleetVehicleModel> lstvehicledetails = new List<eFleetVehicleModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationID", LocationId);
                return lstvehicledetails;
            }
        }
        #endregion Vehicle

        #region Mileage 
        public List<eFleetMileageReportModel> GetMileageReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch)
        {
            try
            {
                List<eFleetMileageReportModel> recentmileagelst = new List<eFleetMileageReportModel>();
                try
                {
                    var lstfromfueling = (from lst in _workorderEMSEntities.eFleetFuelings.Where(a => (a.IsDeleted == false)
                                                       && ((userId == 0 ? null : userId) == null || a.CreatedBy == userId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || a.LocationID == LocationId)
                                                       && ((FuelType == 0 ? null : FuelType) == null || a.FuelType == FuelType)
                                                       && (a.CreatedDate >= FromDate && a.CreatedDate <= ToDate))
                                          group lst by new
                                          {
                                              y = lst.CreatedDate.Year,
                                              m = lst.CreatedDate.Month,
                                              d = lst.CreatedDate.Day,
                                              lst.VehicleID,
                                          }
                                         into groups
                                          select groups.OrderByDescending(p => p.CreatedDate).FirstOrDefault());
                    var lstfromvehiclemasterlog = (from lst in _workorderEMSEntities.eFleetVehicleMasterLogs.Where(a => (a.IsDeleted == false)
                                                       && ((userId == 0 ? null : userId) == null || a.CreatedBy == userId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || a.eFleetVehicle.LocationID == LocationId)
                                                       && ((FuelType == 0 ? null : FuelType) == null || a.eFleetVehicle.FuelType == FuelType)
                                                       && (a.CreatedOn >= FromDate && a.CreatedOn <= ToDate)
                                                       && (a.Action == "423"))
                                                   group lst by new
                                                   {
                                                       lst.VehicleID,
                                                       y = lst.CreatedOn.Year,
                                                       m = lst.CreatedOn.Month,
                                                       d = lst.CreatedOn.Day,
                                                       // DbFunctions.TruncateTime(lst.CreatedDate),
                                                   } into groups
                                                   select groups.OrderByDescending(p => p.CreatedOn).FirstOrDefault());
                    foreach (var item in lstfromfueling)
                    {
                        eFleetMileageReportModel objeFleetMileageReportModel = new eFleetMileageReportModel();
                        objeFleetMileageReportModel.CreatedDate = item.CreatedDate.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        objeFleetMileageReportModel.DriverName = item.UserRegistration.FirstName + " " + item.UserRegistration.LastName;
                        objeFleetMileageReportModel.FuelType = item.FuelType;
                        objeFleetMileageReportModel.FuelTypeName = item.GlobalCode.CodeName;
                        objeFleetMileageReportModel.LocationID = item.LocationID;
                        objeFleetMileageReportModel.Mileage = item.Mileage;
                        objeFleetMileageReportModel.UserId = item.CreatedBy;
                        objeFleetMileageReportModel.VehicleID = item.VehicleID;
                        objeFleetMileageReportModel.VehicleNumber = item.VehicleNumber;
                        objeFleetMileageReportModel.VehicleIdentificationNumber = item.eFleetVehicle.VehicleIdentificationNo;
                        objeFleetMileageReportModel.LicensePlateNumber = item.eFleetVehicle.VehicleLicensePlateNo;
                        objeFleetMileageReportModel.QRCodeID = item.eFleetVehicle.QRCodeID;
                        recentmileagelst.Add(objeFleetMileageReportModel);
                    }

                    foreach (var item in lstfromvehiclemasterlog)
                    {
                        var objInteriorMileageDriverDetails = new eFleetInteriorMileageDriverModel();
                        objInteriorMileageDriverDetails = GenericDataContractSerializer<eFleetInteriorMileageDriverModel>.DeserializeXml(item.VehicleTypeDetails);
                        if (objInteriorMileageDriverDetails.Mileage.IsChShuttleMileageClicked == true && objInteriorMileageDriverDetails.Mileage.ChShuttleMileage == true
                             && objInteriorMileageDriverDetails.Mileage.ChShDescription != null)
                        {
                            eFleetMileageReportModel objeFleetMileageReportModel = new eFleetMileageReportModel();
                            objeFleetMileageReportModel.CreatedDate = item.CreatedOn.ToString("MM'/'dd'/'yyyy hh:mm tt");
                            objeFleetMileageReportModel.DriverName = item.UserRegistration.FirstName + " " + item.UserRegistration.LastName;
                            objeFleetMileageReportModel.FuelType = item.eFleetVehicle.FuelType.Value;
                            objeFleetMileageReportModel.FuelTypeName = item.eFleetVehicle.GlobalCode.CodeName;
                            objeFleetMileageReportModel.LocationID = objInteriorMileageDriverDetails.LocationId;
                            objeFleetMileageReportModel.Mileage = objInteriorMileageDriverDetails.Mileage.ChShDescription;
                            objeFleetMileageReportModel.UserId = item.UserId;
                            objeFleetMileageReportModel.VehicleID = item.VehicleID;
                            objeFleetMileageReportModel.VehicleNumber = item.eFleetVehicle.VehicleNumber;
                            objeFleetMileageReportModel.VehicleIdentificationNumber = item.eFleetVehicle.VehicleIdentificationNo;
                            objeFleetMileageReportModel.LicensePlateNumber = item.eFleetVehicle.VehicleLicensePlateNo;
                            objeFleetMileageReportModel.QRCodeID = item.eFleetVehicle.QRCodeID;
                            recentmileagelst.Add(objeFleetMileageReportModel);
                        }
                    }

                    var filterlist = recentmileagelst.OrderByDescending(x => x.CreatedDate).ToList();
                    return recentmileagelst;
                }
                catch (Exception ex)
                {
                    Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationId", LocationId);
                    return recentmileagelst;
                }
            }
            catch (Exception ex)
            {
                List<eFleetMileageReportModel> recentmileagelst = new List<eFleetMileageReportModel>();
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetVehicleIncidentModel> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch)", "LocationID", LocationId);
                return recentmileagelst;
            }
        }
        #endregion Mileage 

        #region Vehicle Damage
        /// <summary>Created by Bhushan Dod on 10/01/2018
        /// Get details of Damage in vehicle
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<eFleetDamageReportModel> GeteFleetDamageList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)
        {
            List<eFleetDamageReportModel> lstDamage = new List<eFleetDamageReportModel>();
            try
            {
                var TemplstRoutine = _workorderEMSEntities.eFleetVehicleMasterLogs.Where(a => (a.IsDeleted == false)
                                                       // && ((UserId == 0 ? null : UserId) == null || a.CreatedBy == UserId)
                                                       && ((LocationId == 0 ? null : LocationId) == null || a.eFleetVehicle.LocationID == LocationId)
                                                       && ((vehicle == 0 ? null : vehicle) == null || a.VehicleID == vehicle)
                                                       && (a.CreatedOn >= FromDate && a.CreatedOn <= ToDate)
                                                       && (a.Action == "422")).ToList();

                foreach (var item in TemplstRoutine.ToList())
                {
                    var objeFleetDamageTireModelDetails = new eFleetDamageTireModel();
                    objeFleetDamageTireModelDetails = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(item.VehicleTypeDetails);
                    if (objeFleetDamageTireModelDetails.Damage.IsDamageClicked == true && objeFleetDamageTireModelDetails.Damage.IsDamage == true
                         && objeFleetDamageTireModelDetails.Damage.DamageDarId > 0)
                    {
                        eFleetDamageReportModel objeFleetDamageReportModel = new eFleetDamageReportModel();
                        objeFleetDamageReportModel.StrCreatedDate = item.CreatedOn.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        objeFleetDamageReportModel.CreatedDate = item.CreatedOn;
                        objeFleetDamageReportModel.DriverName = item.UserRegistration.FirstName + " " + item.UserRegistration.LastName;
                        objeFleetDamageReportModel.LocationID = objeFleetDamageTireModelDetails.LocationId;
                        objeFleetDamageReportModel.DamageDescription = objeFleetDamageTireModelDetails.Damage.DamageDesc;
                        objeFleetDamageReportModel.UserId = item.UserId;
                        objeFleetDamageReportModel.VehicleID = item.VehicleID;
                        objeFleetDamageReportModel.VehicleNumber = item.eFleetVehicle.VehicleNumber;
                        objeFleetDamageReportModel.VehicleIdentificationNumber = item.eFleetVehicle.VehicleIdentificationNo;
                        objeFleetDamageReportModel.LicensePlateNumber = item.eFleetVehicle.VehicleLicensePlateNo;
                        objeFleetDamageReportModel.QRCodeID = item.eFleetVehicle.QRCodeID;
                        objeFleetDamageReportModel.eFleetVehicleLogId = Cryptography.GetEncryptedData(Convert.ToString(item.eFleetVehicleLogId), true);
                        lstDamage.Add(objeFleetDamageReportModel);
                    }
                }
                return lstDamage;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetDamageReportModel> GeteFLeetDamageList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)", "LocationId", LocationId);
                return lstDamage; ;
            }
        }

        public eFleetDamageReportModel GeteFleetVehicleMasterLogDetailsById(long logID)
        {
            try
            {
                var db = new workorderEMSEntities();
                var efleetVehiclelogDetails = db.eFleetVehicleMasterLogs.Where(u => u.eFleetVehicleLogId == logID).FirstOrDefault();
                if (efleetVehiclelogDetails.eFleetVehicleLogId > 0)
                {
                    var objeFleetDamageTireModelDetails = new eFleetDamageTireModel();
                    objeFleetDamageTireModelDetails = GenericDataContractSerializer<eFleetDamageTireModel>.DeserializeXml(efleetVehiclelogDetails.VehicleTypeDetails);
                    if (objeFleetDamageTireModelDetails.Damage.IsDamageClicked == true && objeFleetDamageTireModelDetails.Damage.IsDamage == true
                         && objeFleetDamageTireModelDetails.Damage.DamageDarId > 0)
                    {
                        eFleetDamageReportModel objeFleetDamageReportModel = new eFleetDamageReportModel();
                        objeFleetDamageReportModel.StrCreatedDate = efleetVehiclelogDetails.CreatedOn.ToString("MM'/'dd'/'yyyy hh:mm tt");
                        objeFleetDamageReportModel.CreatedDate = efleetVehiclelogDetails.CreatedOn;
                        objeFleetDamageReportModel.DriverName = efleetVehiclelogDetails.UserRegistration.FirstName + " " + efleetVehiclelogDetails.UserRegistration.LastName;
                        objeFleetDamageReportModel.LocationID = objeFleetDamageTireModelDetails.LocationId;
                        objeFleetDamageReportModel.DamageDescription = objeFleetDamageTireModelDetails.Damage.DamageDesc;
                        objeFleetDamageReportModel.CroppedPicture = objeFleetDamageTireModelDetails.Damage.CroppedPicture;
                        objeFleetDamageReportModel.CapturedPicture = objeFleetDamageTireModelDetails.Damage.CapturedImage;
                        objeFleetDamageReportModel.UserId = efleetVehiclelogDetails.UserId;
                        objeFleetDamageReportModel.VehicleID = efleetVehiclelogDetails.VehicleID;
                        objeFleetDamageReportModel.VehicleNumber = efleetVehiclelogDetails.eFleetVehicle.VehicleNumber;
                        objeFleetDamageReportModel.VehicleIdentificationNumber = efleetVehiclelogDetails.eFleetVehicle.VehicleIdentificationNo;
                        objeFleetDamageReportModel.LicensePlateNumber = efleetVehiclelogDetails.eFleetVehicle.VehicleLicensePlateNo;
                        objeFleetDamageReportModel.QRCodeID = efleetVehiclelogDetails.eFleetVehicle.QRCodeID;
                        return objeFleetDamageReportModel;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "eFleetVehicleMasterLog GeteFleetVehicleMasterLogDetailsById(long logID)", "Exception While getting vehicle detail by id.", null);
                throw;
            }
        }
        #endregion Vehicle Damage

        #region Inspection

        /// <summary>Created by Bhushan Dod on 29/01/2018
        /// Get details of Inspection in vehicle
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        public List<InspectionVehicleModel> GeteFleetInspectionList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)
        {
            List<InspectionVehicleModel> lstInspection = new List<InspectionVehicleModel>();
            try
            {
                var lstinspectionlog = _workorderEMSEntities.eFleetVehicleInspectionLogs.Where(a => ((LocationId == 0 ? null : LocationId) == null || a.eFleetVehicle.LocationID == LocationId)
                                                       && ((vehicle == 0 ? null : vehicle) == null || a.VehicleID == vehicle)
                                                       && (a.CreatedDate >= FromDate && a.CreatedDate <= ToDate)).ToList();

                foreach (var item in lstinspectionlog.ToList())
                {
                    InspectionVehicleModel objInspectionVehicleModel = new InspectionVehicleModel();
                    objInspectionVehicleModel.InspectedDate = item.CreatedDate.ToString("MM'/'dd'/'yyyy hh:mm tt");
                    objInspectionVehicleModel.CreatedDate = item.CreatedDate;
                    objInspectionVehicleModel.InspectedBy = item.UserRegistration.FirstName + " " + item.UserRegistration.LastName;
                    //objInspectionVehicleModel.LocationName = item.LocationMaster.LocationName;
                    objInspectionVehicleModel.PostInspection = item.PostInspection;
                    objInspectionVehicleModel.PreInspection = item.PreInspection;
                    objInspectionVehicleModel.VehicleIdentificationNo = item.eFleetVehicle.VehicleIdentificationNo;
                    objInspectionVehicleModel.VehicleNumber = item.eFleetVehicle.VehicleNumber;
                    //objInspectionVehicleModel.VehicleID = Cryptography.GetEncryptedData(Convert.ToString(item.VehicleID), true);
                    objInspectionVehicleModel.VehicleImage = item.eFleetVehicle.VehicleImage;
                    objInspectionVehicleModel.QRCodeID = item.eFleetVehicle.QRCodeID;
                    objInspectionVehicleModel.InspectionLogNo = Cryptography.GetEncryptedData(Convert.ToString(item.InspectionLogID), true);
                    lstInspection.Add(objInspectionVehicleModel);

                }
                return lstInspection;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<InspectionVehicleModel> GeteFleetInspectionList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)", "LocationId", LocationId);
                return lstInspection; ;
            }
        }
        #endregion Inspection

        #region Defect
        /// <summary>
        /// Created By Ashwajit Bansod
        /// Created Date : Feb 13 2018
        /// Created For : To Create a report for Defect
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public List<eFleetDefectReportModel> GeteFleetDefectList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)
        {
            List<eFleetDefectReportModel> lstDefect = new List<eFleetDefectReportModel>();

            try
            {

                var ObjVehicleDetail = _workorderEMSEntities.DefectReportDetails.Where(q => q.IsDeleted == false
                                                                                         && ((vehicle == 0 ? null : vehicle) == null || q.VehicleID == vehicle)
                                                                                            && (q.CreatedDate >= FromDate && q.CreatedDate <= ToDate)).Select(x => new eFleetDefectReportModel()
                                                                                            {

                                                                                                CreatedOn = x.CreatedDate,
                                                                                                Action = x.DefectType,
                                                                                                QrcodeId = x.QRCodeID,
                                                                                                UserName = x.UserRegistration.FirstName + " " + x.UserRegistration.LastName,
                                                                                                WorkOrderRequestId = x.WorkRequestAssignmentID,
                                                                                                Status = x.Status,
                                                                                                VehicleNumber = x.eFleetVehicle.VehicleNumber,
                                                                                                ProjectDescripton = x.WorkRequestAssignment.ProblemDesc,
                                                                                                AllPictures = x.DamageFile,
                                                                                                InspectionType = x.InspectionType,
                                                                                                DefectId = x.DefectID
                                                                                            }).ToList<eFleetDefectReportModel>();

                //var tmp = ObjVehicleDetail.Select(x => new eFleetDefectReportModel()
                //{
                //    DefectIDForDownload = Cryptography.GetDecryptedData(Convert.ToString(x.DefectId), true) ,
                //    Action = x.Action,
                //    AllPictures = x.AllPictures,
                //    CreatedOn = x.CreatedOn,
                //    InspectionType = x.InspectionType,
                //    ProjectDescripton = x.ProjectDescripton,
                //    QrcodeId = x.QrcodeId,
                //    UserName = x.UserName,
                //    VehicleNumber = x.VehicleNumber,
                //    WorkOrderRequestId = x.WorkOrderRequestId,
                //    Status = x.Status
                //}).ToList();
                if (ObjVehicleDetail.Count == 0)
                { throw new Exception("Record not found."); }
                return ObjVehicleDetail;

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetDefectReportModel> GeteFleetDefectList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)", "LocationId", LocationId);
                return lstDefect; ;
            }
        }
        /// <summary>
        /// Created By :  Ashwajit Bansod
        /// Created Date : Feb-14-2018
        /// Created For : To Fetch the Defect Details record from DefectDetails table by DefectId
        /// </summary>
        /// <param name="logID"></param>
        /// <returns></returns>
        public eFleetDefectReportModel GeteFleetDefectDetailsById(long logID)
        {
            try
            {
                var db = new workorderEMSEntities();
                var efleetDefectDetails = db.DefectReportDetails.Where(u => u.DefectID == logID).FirstOrDefault();
                if (efleetDefectDetails.DefectID > 0)
                {
                    eFleetDefectReportModel objeFleetDefectReportModel = new eFleetDefectReportModel();

                    objeFleetDefectReportModel.CreatedOn = efleetDefectDetails.CreatedDate;
                    objeFleetDefectReportModel.UserName = efleetDefectDetails.UserRegistration.FirstName + " " + efleetDefectDetails.UserRegistration.LastName;
                    objeFleetDefectReportModel.ProjectDescripton = efleetDefectDetails.WorkRequestAssignment.ProjectDesc;
                    objeFleetDefectReportModel.AllPictures = efleetDefectDetails.DamageFile;
                    objeFleetDefectReportModel.UserId = efleetDefectDetails.CreatedBy;
                    objeFleetDefectReportModel.VehicleID = efleetDefectDetails.VehicleID;
                    objeFleetDefectReportModel.VehicleNumber = efleetDefectDetails.eFleetVehicle.VehicleNumber;
                    objeFleetDefectReportModel.QrcodeId = efleetDefectDetails.QRCodeID;
                    objeFleetDefectReportModel.DefectType = efleetDefectDetails.DefectType;
                    return objeFleetDefectReportModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public eFleetDefectReportModel GeteFleetDefectDetailsById(long logID)", "Exception While getting Defect detail by id.", null);
                throw;
            }
        }
        #endregion Defect
        #region Passenger Count Report
        public List<eFleetPassengerCountReportModel> GeteFleetPassengerCountHoursOfWeekDays(long? LocationId, long? UserId, DateTime FromDate, DateTime? ToDate, int curretWeek)
        {
            var countList = new eFleetPassengerCountReportModel();
            List<eFleetPassengerCountReportModel> lstDefect = new List<eFleetPassengerCountReportModel>();
            try
            {

                DateTime currenteDate = DateTime.UtcNow;
                DateTime start = currenteDate.Date.AddDays(-(int)currenteDate.DayOfWeek), // prev sunday 00:00  
                end = start.AddDays(5);
                DateTime startDateFromMonday = start.AddDays(1);
                DateTime weekend = start.AddDays(6);
                var weekCountData = _workorderEMSEntities.eFleetPassengerTrackingCounts.Where(a => a.LocationID == LocationId && a.IsDeleted == false && a.CreatedDate > startDateFromMonday && a.CreatedDate < weekend).ToList();
                var tt = from t in weekCountData.AsEnumerable()
                         group t by new { weekday = t.CreatedDate.DayOfWeek, hourdata = t.CreatedDate.Hour } into ut  // WeekNumber = t.CreatedDate.Day / 7,
                         select new
                         {
                             weekday = ut.Key.weekday,
                             hourdata = ut.Key.hourdata,
                             passengerCount = ut.Select(a => a.PassengerCount)
                         };

                foreach (var item in tt)
                {
                    var lst = new eFleetPassengerCountReportModel();
                    //lst.Hour=
                }
                if (lstDefect == null)
                { throw new Exception("Record not found."); }
                return lstDefect;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "List<eFleetDefectReportModel> GeteFleetDefectList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle)", "LocationId", LocationId);
                return lstDefect; ;
            }
        }
        #endregion Passenger Count Report
        #endregion eFleet report

        #region Idle Employee
        public List<IdleEmployeeModel> IdleEmployeeList(long? LocationId, long? UserId, DateTime _fromDate, DateTime? _toDate, DateTime? fromMonth, DateTime? toMonth)
        {
            List<IdleEmployeeModel> lstidleReport = new List<IdleEmployeeModel>();
            try
            {
                var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                string fromString = fromMonth.ToString();
                string toString = toMonth.ToString();
                if (fromString != "1/1/0001 12:00:00 AM" && fromString != "1/1/0001 12:00:00 AM")
                {
                    var employeeIdleListForMonth = _workorderEMSEntities.TrackEmployeeStatus.Join(_workorderEMSEntities.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u }).
                    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserID == UserId)
                                               && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                               && (x.q.CreatedOn >= fromMonth && x.q.CreatedOn <= toMonth)
                                               ).ToList();
                    lstidleReport = employeeIdleListForMonth.GroupBy(x => x.q.CreatedOn.Month).Select(x => new IdleEmployeeModel()
                    {
                        MonthData = (x.Key.ToString() == "1") ? "January" : (x.Key.ToString() == "2") ? "February" : (x.Key.ToString() == "3") ? "March" : (x.Key.ToString() == "4") ? "April" : (x.Key.ToString() == "5") ? "May" : (x.Key.ToString() == "6") ? "June" : (x.Key.ToString() == "7") ? "July" : (x.Key.ToString() == "8") ? "August" : (x.Key.ToString() == "9") ? "September" : (x.Key.ToString() == "10") ? "October" : (x.Key.ToString() == "11") ? "November" : "December",
                        IdleCount = x.Count(),
                    }).ToList<IdleEmployeeModel>();
                }
                else
                {
                    var employeeIdleList = _workorderEMSEntities.TrackEmployeeStatus.Join(_workorderEMSEntities.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserID == UserId)
                                                   && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                                   && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                                   ).ToList();
                    lstidleReport = employeeIdleList.GroupBy(x => x.q.UserID).Select(x => new IdleEmployeeModel()
                    {
                        UserID = x.Key,
                        IdleCount = x.Count(),
                        EmployeeName = listuser.Where(f => f.UserId == x.Key).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key).FirstOrDefault().LastName
                        //   ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                    }).ToList<IdleEmployeeModel>();
                }
                return lstidleReport;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                return lstidleReport;
            }
        }

        public List<IdleEmployeeModel> IdleEmployeeListForWeek(long? LocationId, long? UserId, int weekNumber)
        {
            List<IdleEmployeeModel> lstidleReport = new List<IdleEmployeeModel>();
            try
            {
                int countofWeeks = 0;
                DateTime reference = DateTime.Now;
                Calendar calendar = CultureInfo.CurrentCulture.Calendar;

                IEnumerable<int> daysInMonth = Enumerable.Range(1, calendar.GetDaysInMonth(reference.Year, reference.Month));

                List<Tuple<DateTime, DateTime>> weeks = daysInMonth.Select(day => new DateTime(reference.Year, reference.Month, day))
                    .GroupBy(d => calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFullWeek, 0
                    ))
                    .Select(g => new Tuple<DateTime, DateTime>(g.First(), g.Last()))
                    .ToList();

                weeks.ForEach(x => Console.WriteLine("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}", x.Item1, x.Item2));
                foreach (var item in weeks)
                {

                    int count = countofWeeks + 1;
                    if (count == weekNumber)
                    {
                        var employeeFisrtWeekData = _workorderEMSEntities.TrackEmployeeStatus.Join(_workorderEMSEntities.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u }).
                        Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserID == UserId)
                                              && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                              && (x.q.CreatedOn >= item.Item1 && x.q.CreatedOn <= item.Item2)
                                              ).ToList();
                        lstidleReport = employeeFisrtWeekData.GroupBy(x => x.q.CreatedOn.DayOfWeek).Select(x => new IdleEmployeeModel()
                        {
                            WeekData = x.Key.ToString(),
                            IdleCount = x.Count()
                            // EmployeeName = listuser.Where(f => f.UserId == x.Key).FirstOrDefault().FirstName + " " + listuser.Where(f => f.UserId == x.Key).FirstOrDefault().LastName
                            //   ScanUserName = x.FirstOrDefault().q.UserRegistration.FirstName + " " + x.FirstOrDefault().q.UserRegistration.LastName
                        }).ToList<IdleEmployeeModel>();

                    }
                    countofWeeks++;
                }


                return lstidleReport;
            }

            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                return lstidleReport;
            }
        }
        public List<IdleEmployeeModel> IdleEmployeeListForSelectedDateRange(long? LocationId, long? UserId, DateTime _fromDate, DateTime? _toDate)
        {
            List<IdleEmployeeModel> lstidleReport = new List<IdleEmployeeModel>();
            try
            {

                var listuser = _workorderEMSEntities.UserRegistrations.Select(x => x).ToList();
                var employeeIdleList = _workorderEMSEntities.TrackEmployeeStatus.Join(_workorderEMSEntities.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u }).
                    Where(x => ((UserId == 0 ? null : UserId) == null || x.q.UserID == UserId)
                                               && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
                                               && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
                                               ).ToList();

                lstidleReport = employeeIdleList.GroupBy(x => x.q.CreatedOn.Hour).Select(x => new IdleEmployeeModel()
                {
                    Hoursdata = x.Key.ToString(),
                    IdleCount = x.Count(),

                }).ToList<IdleEmployeeModel>();
                //IEnumerable<IdleEmployeeModel> tt = lstidleReport.OrderBy(a => a.Hoursdata);
                return lstidleReport;
            }

            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name)", "LocationId", LocationId);
                return lstidleReport;
            }
        }

        public List<ReportChartIdleEmployee> IdleEmployeeListDataByEmployee(long? userId, string UserName, long? LocationId, DateTime? FromDate, DateTime? ToDate)
        {

            // Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            DateTime _fromDate = FromDate ?? clientdt.Date;
            DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;
            DateTime diff;
            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null)
            {
                if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
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
            var lstRoutine = new List<ReportChartIdleEmployee>();
            var obj = new ReportChartIdleEmployee();
            try
            {
                _fromDate = _fromDate.ConvertClientTZtoUTC();
                _toDate = _toDate.ConvertClientTZtoUTC();
                DateTime todaysdate = DateTime.UtcNow;
                TimeSpan? CalculatedDifferenceTime = new TimeSpan();
                string DifferenceTime;
                string StartTime;
                string EndTime;
                var locationamedata = _workorderEMSEntities.LocationMasters.Where(x => x.LocationId == LocationId).FirstOrDefault();
                var userdata = _workorderEMSEntities.UserRegistrations.Where(x => (x.FirstName + " " + x.LastName) == UserName).FirstOrDefault();
                userId = userdata.UserId;
                 var CalculatedData = _workorderEMSEntities.IdleEmployees.Where(x => x.IdleEmployeeId == userId && x.LocationId == LocationId
                                                                                && x.IdleTime >= _fromDate && x.IdleTime <= _toDate).ToList();
                foreach (var item in CalculatedData)
                {
                    CalculatedDifferenceTime = item.EndTime - item.StartTime;
                    //obj.DifferenceTime = CalculatedDifferenceTime.ToString();
                    //obj.UserName = userdata.FirstName + " " + userdata.LastName;
                    //obj.StartTime = item.StartTime.ToString();
                    //obj.EndTime = item.EndTime.ToString();
                    //obj.LocationName = locationamedata.LocationName;
                    lstRoutine.Add(new ReportChartIdleEmployee() { DifferenceTime = CalculatedDifferenceTime.ToString(),
                                                                    UserName = userdata.FirstName + " " + userdata.LastName,
                                                                   StartTime = item.StartTime.ToString(),
                                                                   EndTime = item.EndTime.ToString(),
                                                                   LocationName = locationamedata.LocationName
                                                                  });
                }
                //lstRoutine = CalculatedData.Select(ls => new ReportChartIdleEmployee()
                //{
                //    UserName = userdata.FirstName + " " + userdata.LastName,
                //    StartTime = ls.StartTime.ToString(),
                //    EndTime = ls.EndTime.ToString(),
                //    DifferenceTime = ls.EndTime - ls.StartTime,

                //}).ToList();
            }
            catch(Exception ex)
            {

            }
            return lstRoutine;
       }
                /// <summary>
                /// Created By : Ashwajit Bansod
                /// Created Date:May-09-2018
                /// Created For: TO fetch idle Employee data from Database
                /// </summary>
                /// <param name="LocationId"></param>
                /// <param name="userName"></param>
                /// <param name="FromDate"></param>
                /// <param name="ToDate"></param>
                /// <returns></returns>
        //        public List<ReportChartIdleEmployee> IdleEmployeeListByEmployee(long? userId, string UserName, long? LocationId, DateTime? FromDate, DateTime? ToDate)
        //{

        //    // Getting client date time. 
        //    var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
        //    //flag status for if user filter record in time span so to date is till midnight. 
        //    bool isUTCDay = true;
        //    DateTime _fromDate = FromDate ?? clientdt.Date;
        //    DateTime _toDate = ToDate ?? clientdt.AddDays(1).Date;
        //    DateTime diff;
        //    //maintaining flag  if interval date come then need to fetch record till midnight of todate day
        //    if (ToDate != null)
        //    {
        //        if (ToDate.Value.ToLongTimeString() == "12:00:00 AM")
        //            isUTCDay = false;
        //    }
        //    if (_fromDate != null && _toDate != null)
        //    {
        //        ////if interval date come then need to fetch record till midnight of todate day
        //        if ((_fromDate.Date != _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
        //        {
        //            _toDate = _toDate.AddDays(1).Date;
        //        }
        //        if ((_fromDate.Date == _toDate.Date) && (_toDate.ToLongTimeString() == "12:00:00 AM"))
        //        {
        //            _toDate = _toDate.AddDays(1).Date;
        //        }
        //    }
        //    List<ReportChartIdleEmployee> lstRoutine = new List<ReportChartIdleEmployee>();
        //    var obj = new ReportChartIdleEmployee();
        //    try
        //    {
        //        _fromDate = _fromDate.ConvertClientTZtoUTC();
        //        _toDate = _toDate.ConvertClientTZtoUTC();
        //        DateTime todaysdate = DateTime.UtcNow;
        //        DateTime? StartTimeReocrd;
        //        TimeSpan AddTimeIfNull = new TimeSpan();
        //        long TrackIdForIdle = 0;
        //        var userdata = _workorderEMSEntities.UserRegistrations.Where(x => (x.FirstName + " " + x.LastName) == UserName).FirstOrDefault();
        //        userId = userdata.UserId;
        //        var employeeIdleList = _workorderEMSEntities.TrackEmployeeStatus.Join(_workorderEMSEntities.UserRegistrations, q => q.UserID, u => u.UserId, (q, u) => new { q, u }).
        //                                       Where(x => ((x.q.UserID == 0 ? null : userId) == null || x.q.UserID == userId)
        //                                       && ((LocationId == 0 ? null : LocationId) == null || x.q.LocationId == LocationId)
        //                                       && (x.q.CreatedOn >= _fromDate && x.q.CreatedOn <= _toDate)
        //                                       ).ToList();
        //        if (employeeIdleList.Count > 0)
        //        {
        //            foreach (var item in employeeIdleList)
        //            {
        //                var locationamedata = _workorderEMSEntities.LocationMasters.Where(x => x.LocationId == LocationId).FirstOrDefault();
        //                DateTime MinusthirtyMin = item.q.CreatedOn.AddMinutes(-30);
        //                DateTime PlusthityMin = item.q.CreatedOn.AddMinutes(30);

        //                #region Get End time 
        //                //Get DAR End time before Idle
        //                var darLastRecordBeforeIdle = _workorderEMSEntities.DARDetails.Where(x => x.CreatedOn <= item.q.CreatedOn || x.CreatedOn <= item.q.ModifiedOn &&
        //                                                                             x.CreatedBy == userdata.UserId && x.LocationId == LocationId
        //                                                                             && x.TaskType != 23).OrderByDescending(a => a.CreatedOn).Select(
        //                    d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                    {
        //                        EndTime = d.CreatedOn
        //                    }).FirstOrDefault();
        //                // Get Work Order End time before Idle
        //                var WorkRequestLastRecordBeforeIdle = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.CreatedDate <= item.q.CreatedOn || x.CreatedDate <= item.q.ModifiedOn
        //                                                                             && x.CreatedBy == userdata.UserId
        //                                                                             && x.LocationID == LocationId
        //                                                                             ).OrderByDescending(a => a.CreatedDate).Select(
        //                    d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                    {
        //                        EndTime = d.CreatedDate
        //                    }).FirstOrDefault();

        //                // Get QRC Last time before Idle
        //                var qrcScanLastRecordBeforeIdle = _workorderEMSEntities.QRCScanLogs.Where(x => x.CreatedOn <= item.q.CreatedOn || x.CreatedOn <= item.q.ModifiedOn &&
        //                                                                             x.CreatedBy == userdata.UserId && x.LocationId == LocationId
        //                                                                             ).OrderByDescending(a => a.CreatedOn).ThenByDescending(a => a.ModifiedOn).Select(
        //                    d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                    {
        //                        EndTime = d.CreatedOn
        //                    }).FirstOrDefault();
        //                #endregion Get Start time 

        //                #region Get First Time After Idle
        //                //Get DAR Details after 30 Min.
        //                var darFirstRecordAfterIdleForThirtyMin = _workorderEMSEntities.DARDetails.Where(x => x.CreatedOn <= PlusthityMin && x.CreatedOn >= item.q.CreatedOn || x.ModifiedOn <= PlusthityMin
        //                                                                               && x.ModifiedOn >= item.q.CreatedOn &&
        //                                                                              x.CreatedBy == userdata.UserId && x.LocationId == LocationId).OrderBy(a => a.CreatedOn).Select(
        //                  d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                  {
        //                      StartTime = d.CreatedOn,

        //                  }).FirstOrDefault();
        //                //Get WO Details After 30 Min.
        //                var WorkOrderFirstRecordAfterIdleForThirtyMin = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.CreatedDate <= PlusthityMin && x.CreatedDate >= item.q.CreatedOn || x.ModifiedDate <= PlusthityMin
        //                                                                                     && x.ModifiedDate >= item.q.CreatedOn &&
        //                                                                              x.CreatedBy == userdata.UserId && x.LocationID == LocationId).OrderBy(a => a.CreatedDate).Select(
        //                  d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                  {
        //                      StartTime = d.CreatedDate
        //                  }).FirstOrDefault();
        //                //Get QRC Details After 30 Min.
        //                var qrcScanFirstRecordAfterIdleForThirtyMin = _workorderEMSEntities.QRCScanLogs.Where(x => x.CreatedOn <= PlusthityMin && x.CreatedOn >= item.q.CreatedOn || x.ModifiedOn <= PlusthityMin
        //                                                                               && x.ModifiedOn >= item.q.CreatedOn &&
        //                                                                              x.CreatedBy == userdata.UserId && x.LocationId == LocationId).OrderBy(a => a.CreatedOn).Select(
        //                  d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                  {
        //                      StartTime = d.CreatedOn
        //                  }).FirstOrDefault();

        //                // Get DAR First time After Idle
        //                var darFirstRecordAfterIdle = _workorderEMSEntities.DARDetails.Where(x => (x.CreatedOn >= item.q.CreatedOn ? x.CreatedOn : DateTime.UtcNow) == null || x.CreatedOn >= item.q.ModifiedOn &&
        //                                                                            x.CreatedBy == userdata.UserId && x.LocationId == LocationId).OrderBy(a => a.CreatedOn).Select(
        //                   d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                   {
        //                       StartTime = d.CreatedOn
        //                   }).FirstOrDefault();
        //                //Get Work Order First time After Idle
        //                var WorkOrderFirstRecordAfterIdle = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.CreatedDate >= item.q.CreatedOn || x.CreatedDate >= item.q.ModifiedOn &&
        //                                                                             x.CreatedBy == userdata.UserId && x.LocationID == LocationId).OrderBy(a => a.CreatedDate).Select(
        //                  d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                  {
        //                      StartTime = d.CreatedDate
        //                  }).FirstOrDefault();

        //                var qrcScanFirstRecordAfterIdle = _workorderEMSEntities.QRCScanLogs.Where(x => (x.CreatedOn >= item.q.CreatedOn ? x.CreatedOn : DateTime.UtcNow) == x.CreatedOn || x.CreatedOn >= item.q.ModifiedOn &&
        //                                                                           x.CreatedBy == userdata.UserId && x.LocationId == LocationId).OrderBy(a => a.CreatedOn).Select(
        //                  d => new StartEndAndDifferenceTimeForIdleEmployee()
        //                  {
        //                      StartTime = d.CreatedOn
        //                  }).FirstOrDefault();

        //                #endregion Get First Time After Idle
        //                DateTime forNull = DateTime.UtcNow;
        //                bool trackStatus = false;
        //                var EndTimeRecord = (darLastRecordBeforeIdle.EndTime > WorkRequestLastRecordBeforeIdle.EndTime ? (darLastRecordBeforeIdle.EndTime > qrcScanLastRecordBeforeIdle.EndTime ? darLastRecordBeforeIdle.EndTime : qrcScanLastRecordBeforeIdle.EndTime) : (WorkRequestLastRecordBeforeIdle.EndTime > qrcScanLastRecordBeforeIdle.EndTime ? WorkRequestLastRecordBeforeIdle.EndTime : qrcScanLastRecordBeforeIdle.EndTime));
        //                //var StartTimeRecord = (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? (darFirstRecordAfterIdle.StartTime < qrcScanFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : qrcScanFirstRecordAfterIdle.StartTime) : (WorkOrderFirstRecordAfterIdle.StartTime < qrcScanFirstRecordAfterIdle.EndTime ? WorkOrderFirstRecordAfterIdle.StartTime : qrcScanFirstRecordAfterIdle.StartTime));

        //                if (darFirstRecordAfterIdleForThirtyMin == null && WorkOrderFirstRecordAfterIdleForThirtyMin == null && qrcScanFirstRecordAfterIdleForThirtyMin == null)
        //                {
        //                    AddTimeIfNull = AddTimeIfNull + item.q.CreatedOn.TimeOfDay;
        //                    var calculation = AddTimeIfNull - EndTimeRecord.TimeOfDay;

        //                    var lastrecord = lstRoutine.LastOrDefault();
        //                    lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.trackStatus = true);
        //                    //lstRoutine.Where(x => x.TrackId == lastrecord.TrackId).Select(x => new ReportChartIdleEmployee()
        //                    //{
        //                    //    trackStatus = true
        //                    //}).ToList();
        //                    //newly added code
        //                    if (lstRoutine.Count == 0 && darFirstRecordAfterIdle == null && WorkOrderFirstRecordAfterIdle == null && qrcScanFirstRecordAfterIdle == null)
        //                    {
        //                        lstRoutine = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                       .Select(x => new ReportChartIdleEmployee()
        //                       {
        //                           EndTime = forNull.ToString(),
        //                           StartTime = EndTimeRecord.ToString(),
        //                           LocationId = x.ResponseLocationId,
        //                           DifferenceTime = calculation.ToString(),
        //                           UserName = UserName,
        //                           LocationName = locationamedata.LocationName,
        //                           TrackId = item.q.TrackId,
        //                           trackStatus = true
        //                       }).ToList();
        //                    }
        //                    if (lastrecord != null && darFirstRecordAfterIdle == null && WorkOrderFirstRecordAfterIdle == null && qrcScanFirstRecordAfterIdle == null)
        //                    {
        //                        var previousTime = forNull - Convert.ToDateTime(lastrecord.EndTime);
        //                        var TimeCalculation = previousTime + calculation;
        //                        var result = string.Format("{0:D2}:{1:D2}", previousTime.Hours, previousTime.Minutes);

        //                        lstRoutine.Where(x => x.UserName == userdata.FirstName + " " + userdata.LastName && x.trackStatus == true).Select(s => new ReportChartIdleEmployee
        //                        {
        //                            EndTime = DateTime.UtcNow.ToString(),
        //                            StartTime = lastrecord.EndTime.ToString(),
        //                            DifferenceTime = result.ToString(),
        //                            UserName = UserName,
        //                            LocationName = locationamedata.LocationName,
        //                            TrackId = item.q.TrackId,
        //                            trackStatus = false
        //                        });
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.DifferenceTime = result);
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.StartTime = lastrecord.EndTime.ToString());
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.EndTime = forNull.ToString());

        //                    }
        //                    else if (lastrecord != null)
        //                    {

        //                        //eed a logic here like calculate first record time and calculate difference from last record and first record
        //                        var previousTime = Convert.ToDateTime(lastrecord.EndTime) - Convert.ToDateTime(lastrecord.StartTime);
        //                        // var hm = new DateTime(0,0,0,previousTime.Day, previousTime.Hour, previousTime.Minute);
        //                        // var totaltime = Convert.ToDateTime(calculation);
        //                        var TimeCalculation = previousTime + calculation;
        //                        var result = string.Format("{0:D2}:{1:D2}", TimeCalculation.Hours, TimeCalculation.Minutes);
        //                        var exactEndTimeAfterIdle = TimeCalculation + EndTimeRecord.TimeOfDay;
        //                        var stringTime = exactEndTimeAfterIdle.ToString();
        //                        var ConvertexactEndTimeAfterIdleTODateTime = Convert.ToDateTime(stringTime);
        //                        //var TimeCalculation = result + calculation;
        //                        //lastrecord.DifferenceTime = TimeCalculation.ToString();
        //                        lstRoutine.Where(x => x.UserName == userdata.FirstName + " " + userdata.LastName && x.trackStatus == true).Select(s => new ReportChartIdleEmployee
        //                        {
        //                            EndTime = ConvertexactEndTimeAfterIdleTODateTime.ToString(),
        //                            StartTime = EndTimeRecord.ToString(),
        //                            DifferenceTime = result.ToString(),
        //                            UserName = UserName,
        //                            LocationName = locationamedata.LocationName,
        //                            TrackId = item.q.TrackId,
        //                            trackStatus = false
        //                        });
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.DifferenceTime = result);
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.EndTime = ConvertexactEndTimeAfterIdleTODateTime.ToString());
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.trackStatus = false);
        //                        lstRoutine.Where(w => w.TrackId == lastrecord.TrackId).ToList().ForEach(i => i.StartTime = EndTimeRecord.ToString());
        //                    }


        //                    AddTimeIfNull = new TimeSpan();
        //                    TrackIdForIdle = item.q.TrackId;
        //                }
        //                else
        //                {
        //                    if (qrcScanFirstRecordAfterIdle == null && WorkOrderFirstRecordAfterIdle == null && darFirstRecordAfterIdle == null)
        //                    {

        //                        var DiffenrenceTimeQRC = forNull - EndTimeRecord;
        //                        var result = string.Format("{0:D2}:{1:D2}", DiffenrenceTimeQRC.Hours, DiffenrenceTimeQRC.Minutes);

        //                        var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                        .Select(x => new ReportChartIdleEmployee()
        //                        {
        //                            EndTime = forNull.ToString(),
        //                            StartTime = EndTimeRecord.ToString(),
        //                            LocationId = x.ResponseLocationId,
        //                            DifferenceTime = result.ToString(),
        //                            UserName = UserName,
        //                            LocationName = locationamedata.LocationName,
        //                            TrackId = item.q.TrackId
        //                        }).ToList();
        //                        lstRoutine.AddRange(getIdleData);
        //                    }
        //                    else if (qrcScanFirstRecordAfterIdle != null)
        //                    {
        //                        var StartTimeRecordFirst = qrcScanFirstRecordAfterIdle.StartTime;
        //                        if (WorkOrderFirstRecordAfterIdle != null || darFirstRecordAfterIdle != null)
        //                        {
        //                            if (WorkOrderFirstRecordAfterIdle != null && darFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) < qrcScanFirstRecordAfterIdle.StartTime ? (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) : qrcScanFirstRecordAfterIdle.StartTime;
        //                            }
        //                            else if (WorkOrderFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (qrcScanFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? qrcScanFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime);
        //                            }
        //                            else if (darFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (qrcScanFirstRecordAfterIdle.StartTime < darFirstRecordAfterIdle.StartTime ? qrcScanFirstRecordAfterIdle.StartTime : darFirstRecordAfterIdle.StartTime);
        //                            }
        //                            TimeSpan tt = new TimeSpan();
        //                            var FinalDifference = StartTimeRecordFirst - EndTimeRecord;
        //                            // var DiffenrenceTimeQRC = ((WorkOrderFirstRecordAfterIdle != null) ? WorkOrderFirstRecordAfterIdle.StartTime - EndTimeRecord : darFirstRecordAfterIdle.StartTime - EndTimeRecord);
        //                            var result = string.Format("{0:D2}:{1:D2}", FinalDifference.Value.Hours, FinalDifference.Value.Minutes);
        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                            .Select(x => new ReportChartIdleEmployee()
        //                            {
        //                                EndTime = StartTimeRecordFirst.ToString(),
        //                                StartTime = EndTimeRecord.ToString(),
        //                                LocationId = x.ResponseLocationId,
        //                                DifferenceTime = result.ToString(),
        //                                UserName = UserName,
        //                                LocationName = locationamedata.LocationName,
        //                                TrackId = item.q.TrackId
        //                            }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                        else
        //                        {
        //                            var DiffenrenceTimeQRC = qrcScanFirstRecordAfterIdle.StartTime - EndTimeRecord;
        //                            var result = string.Format("{0:D2}:{1:D2}", DiffenrenceTimeQRC.Value.Hours, DiffenrenceTimeQRC.Value.Minutes);

        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                                .Select(x => new ReportChartIdleEmployee()
        //                                {

        //                                    EndTime = qrcScanFirstRecordAfterIdle.StartTime.ToString(),
        //                                    StartTime = EndTimeRecord.ToString(),
        //                                    LocationId = x.ResponseLocationId,
        //                                    DifferenceTime = result.ToString(),
        //                                    UserName = UserName,
        //                                    LocationName = locationamedata.LocationName,
        //                                    TrackId = item.q.TrackId
        //                                }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                    }
        //                    else if (WorkOrderFirstRecordAfterIdle != null)
        //                    {
        //                        var StartTimeRecordFirst = WorkOrderFirstRecordAfterIdle.StartTime;
        //                        if (qrcScanFirstRecordAfterIdle != null || darFirstRecordAfterIdle != null)
        //                        {
        //                            if (qrcScanFirstRecordAfterIdle != null && darFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) < qrcScanFirstRecordAfterIdle.StartTime ? (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) : qrcScanFirstRecordAfterIdle.StartTime;
        //                            }
        //                            else if (darFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (qrcScanFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? qrcScanFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime);
        //                            }
        //                            else if (qrcScanFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (qrcScanFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? qrcScanFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime);
        //                            }
        //                            var FinalDifference = StartTimeRecordFirst - EndTimeRecord;
        //                            var DiffenrenceTimeQRC = ((qrcScanFirstRecordAfterIdle != null) ? qrcScanFirstRecordAfterIdle.StartTime - EndTimeRecord : darFirstRecordAfterIdle.StartTime - EndTimeRecord);
        //                            var result = string.Format("{0:D2}:{1:D2}", FinalDifference.Value.Hours, FinalDifference.Value.Minutes);

        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                            .Select(x => new ReportChartIdleEmployee()
        //                            {
        //                                EndTime = StartTimeRecordFirst.ToString(),
        //                                StartTime = EndTimeRecord.ToString(),
        //                                LocationId = x.ResponseLocationId,
        //                                DifferenceTime = result.ToString(),
        //                                UserName = UserName,
        //                                LocationName = locationamedata.LocationName,
        //                                TrackId = item.q.TrackId
        //                            }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                        else
        //                        {
        //                            var DiffenrenceTimeQRC = WorkOrderFirstRecordAfterIdle.StartTime - EndTimeRecord;
        //                            var result = string.Format("{0:D2}:{1:D2}", DiffenrenceTimeQRC.Value.Hours, DiffenrenceTimeQRC.Value.Minutes);

        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                                .Select(x => new ReportChartIdleEmployee()
        //                                {
        //                                    EndTime = WorkOrderFirstRecordAfterIdle.StartTime.ToString(),
        //                                    StartTime = EndTimeRecord.ToString(),
        //                                    LocationId = x.ResponseLocationId,
        //                                    DifferenceTime = result.ToString(),
        //                                    UserName = UserName,
        //                                    LocationName = locationamedata.LocationName,
        //                                    TrackId = item.q.TrackId
        //                                }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                    }
        //                    else if (darFirstRecordAfterIdle != null)
        //                    {
        //                        var StartTimeRecordFirst = darFirstRecordAfterIdle.StartTime;
        //                        if (qrcScanFirstRecordAfterIdle != null || WorkOrderFirstRecordAfterIdle != null)
        //                        {
        //                            if (qrcScanFirstRecordAfterIdle != null && WorkOrderFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) < qrcScanFirstRecordAfterIdle.StartTime ? (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : WorkOrderFirstRecordAfterIdle.StartTime) : qrcScanFirstRecordAfterIdle.StartTime;
        //                            }
        //                            else if (qrcScanFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (qrcScanFirstRecordAfterIdle.StartTime < darFirstRecordAfterIdle.StartTime ? qrcScanFirstRecordAfterIdle.StartTime : darFirstRecordAfterIdle.StartTime);
        //                            }
        //                            else if (WorkOrderFirstRecordAfterIdle != null)
        //                            {
        //                                StartTimeRecordFirst = (WorkOrderFirstRecordAfterIdle.StartTime < darFirstRecordAfterIdle.StartTime ? WorkOrderFirstRecordAfterIdle.StartTime : darFirstRecordAfterIdle.StartTime);
        //                            }
        //                            var FinalDifference = StartTimeRecordFirst - EndTimeRecord;
        //                            var DiffenrenceTimeQRC = ((qrcScanFirstRecordAfterIdle != null) ? qrcScanFirstRecordAfterIdle.StartTime - EndTimeRecord : WorkOrderFirstRecordAfterIdle.StartTime - EndTimeRecord);
        //                            var result = string.Format("{0:D2}:{1:D2}", FinalDifference.Value.Hours, FinalDifference.Value.Minutes);

        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                            .Select(x => new ReportChartIdleEmployee()
        //                            {
        //                                EndTime = StartTimeRecordFirst.ToString(),
        //                                StartTime = EndTimeRecord.ToString(),
        //                                LocationId = x.ResponseLocationId,
        //                                DifferenceTime = result.ToString(),
        //                                UserName = UserName,
        //                                LocationName = locationamedata.LocationName,
        //                                TrackId = item.q.TrackId
        //                            }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                        else
        //                        {
        //                            var DiffenrenceTimeQRC = darFirstRecordAfterIdle.StartTime - EndTimeRecord;
        //                            var result = string.Format("{0:D2}:{1:D2}", DiffenrenceTimeQRC.Value.Hours, DiffenrenceTimeQRC.Value.Minutes);

        //                            var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                                .Select(x => new ReportChartIdleEmployee()
        //                                {
        //                                    EndTime = darFirstRecordAfterIdle.StartTime.ToString(),
        //                                    StartTime = EndTimeRecord.ToString(),
        //                                    LocationId = x.ResponseLocationId,
        //                                    DifferenceTime = result.ToString(),
        //                                    UserName = UserName,
        //                                    LocationName = locationamedata.LocationName,
        //                                    TrackId = item.q.TrackId
        //                                }).ToList();
        //                            lstRoutine.AddRange(getIdleData);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        StartTimeReocrd = (darFirstRecordAfterIdle.StartTime < WorkOrderFirstRecordAfterIdle.StartTime ? (darFirstRecordAfterIdle.StartTime < qrcScanFirstRecordAfterIdle.StartTime ? darFirstRecordAfterIdle.StartTime : qrcScanFirstRecordAfterIdle.StartTime) : (WorkOrderFirstRecordAfterIdle.StartTime < qrcScanFirstRecordAfterIdle.StartTime ? WorkOrderFirstRecordAfterIdle.StartTime : qrcScanFirstRecordAfterIdle.StartTime));
        //                        var DiffenrenceTime = StartTimeReocrd - EndTimeRecord;
        //                        var result = string.Format("{0:D2}:{1:D2}", DiffenrenceTime.Value.Hours, DiffenrenceTime.Value.Minutes);

        //                        var getIdleData = _workorderEMSEntities.sp_GetIdleCalculationTimeOfEmployee(LocationId, userdata.UserId, MinusthirtyMin, item.q.CreatedOn)
        //                        .Select(x => new ReportChartIdleEmployee()
        //                        {
        //                            EndTime = StartTimeReocrd.ToString(),
        //                            StartTime = EndTimeRecord.ToString(),
        //                            LocationId = x.ResponseLocationId,
        //                            DifferenceTime = result.ToString(),
        //                            UserName = UserName,
        //                            LocationName = locationamedata.LocationName,
        //                            TrackId = item.q.TrackId
        //                        }).ToList();
        //                        lstRoutine.AddRange(getIdleData);

        //                    }
        //                }
        //            }
        //        }
        //        //if (employeeIdleList.Count > 0)
        //        //{
        //        //    var locationamedata = _workorderEMSEntities.LocationMasters.Where(x => x.LocationId == LocationId).FirstOrDefault();
        //        //   var 
        //        //    foreach (var item in employeeIdleList)
        //        //    {

        //        //        #region LastWork 
        //        //        var lastWorkTimeToCreateDAR = _workorderEMSEntities.DARDetails.Where(x => x.CreatedOn <= item.q.CreatedOn &&
        //        //                                                                        x.CreatedBy == userdata.UserId && x.LocationId == LocationId
        //        //                                                                        && x.TaskType != 23).OrderByDescending(a => a.CreatedOn).FirstOrDefault();
        //        //        if(lastWorkTimeToCreateDAR == null)
        //        //        {
        //        //           var lastWorkTimeForDAR = _workorderEMSEntities.DARDetails.Where(x => (x.StartTime <= item.q.CreatedOn || x.EndTime <= item.q.CreatedOn)
        //        //                                                                           &&x.CreatedBy == userdata.UserId && x.LocationId == LocationId
        //        //                                                                           && x.TaskType != 23).OrderByDescending(a => a.StartTime).FirstOrDefault();
        //        //            var startTimeandEndTime = (lastWorkTimeForDAR.EndTime != null ?:)
        //        //            var DarLastTime = (lastWorkTimeToCreateDAR == null ? lastWorkTimeForDAR.s : lastWorkTimeForDAR.s);

        //        //        }
        //        //        var LastWorkTimeToCreateWR = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.CreatedDate <= item.q.CreatedOn
        //        //                                                                     && x.CreatedBy == userdata.UserId
        //        //                                                                     && x.LocationID == LocationId
        //        //                                                                     ).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        //        //        if (LastWorkTimeToCreateWR == null)
        //        //        {
        //        //            var lastWorkTimeForWR = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.StartTime <= item.q.CreatedOn || x.EndDate <= item.q.CreatedOn
        //        //                                                                         && x.CreatedBy == userdata.UserId
        //        //                                                                         && x.LocationID == LocationId
        //        //                                                                         ).OrderByDescending(a => a.StartTime).FirstOrDefault();

        //        //        }
        //        //        var LastWorkTimeForQRScan = _workorderEMSEntities.QRCScanLogs.Where(x => x.CreatedOn <= item.q.CreatedOn
        //        //                                                                    && x.CreatedBy == userdata.UserId
        //        //                                                                    && x.LocationId == LocationId
        //        //                                                                    ).OrderByDescending(a => a.CreatedOn).FirstOrDefault();
        //        //        #endregion LastWork
        //        //        #region First Work After Idle
        //        //        var firstWorkTimeToCreateDAR = _workorderEMSEntities.DARDetails.Where(x => x.CreatedOn >= item.q.CreatedOn &&
        //        //                                                                     x.CreatedBy == userdata.UserId && x.LocationId == LocationId && x.TaskType != 23).OrderBy(a => a.CreatedOn).FirstOrDefault();
        //        //       if(firstWorkTimeToCreateDAR == null)
        //        //        {
        //        //            var firstWorkTimeForeDAR = _workorderEMSEntities.DARDetails.Where(x => (x.StartTime >= item.q.CreatedOn || x.EndTime >= item.q.CreatedOn) &&
        //        //                                                                    x.CreatedBy == userdata.UserId && x.LocationId == LocationId && x.TaskType != 23).OrderBy(a => a.CreatedOn).FirstOrDefault();
        //        //        }
        //        //        var firstWorkTimeToCreateWR = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.CreatedDate >= item.q.CreatedOn
        //        //                                                                     && x.CreatedBy == userdata.UserId
        //        //                                                                     && x.LocationID == LocationId
        //        //                                                                     ).OrderBy(a => a.CreatedDate).FirstOrDefault();
        //        //        if (firstWorkTimeToCreateWR != null)
        //        //        {
        //        //            var firstWorkTimeForWR = _workorderEMSEntities.WorkRequestAssignments.Where(x => x.StartTime >= item.q.CreatedOn || x.EndDate >= item.q.CreatedOn
        //        //                                                                         && x.CreatedBy == userdata.UserId
        //        //                                                                         && x.LocationID == LocationId
        //        //                                                                         ).OrderByDescending(a => a.StartTime).FirstOrDefault();

        //        //        }
        //        //        var firstWorkTimeForQRScan = _workorderEMSEntities.QRCScanLogs.Where(x => x.CreatedOn >= item.q.CreatedOn
        //        //                                                                    && x.CreatedBy == userdata.UserId
        //        //                                                                    && x.LocationId == LocationId
        //        //                                                                    ).OrderByDescending(a => a.CreatedOn).FirstOrDefault();
        //        //        #endregion First Work After Idle
        //        //        var EndTimeRecord = (darLastRecordBeforeIdle.EndTime > WorkRequestLastRecordBeforeIdle.EndTime ? (darLastRecordBeforeIdle.EndTime > qrcScanLastRecordBeforeIdle.EndTime ? darLastRecordBeforeIdle.EndTime : qrcScanLastRecordBeforeIdle.EndTime) : (WorkRequestLastRecordBeforeIdle.EndTime > qrcScanLastRecordBeforeIdle.EndTime ? WorkRequestLastRecordBeforeIdle.EndTime : qrcScanLastRecordBeforeIdle.EndTime));

        //        //    }
        //        //}

        //        return lstRoutine;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<ReportChartIdleEmployee> IdleEmployeeListByEmployee(long? LocationId, string userName, DateTime? FromDate, DateTime? ToDate)", "LocationId", LocationId);
        //        return null;
        //    }
           
        //}

        #endregion Idle Empolyee
    }
}
