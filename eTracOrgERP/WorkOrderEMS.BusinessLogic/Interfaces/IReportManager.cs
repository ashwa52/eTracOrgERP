using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.ServiceModel;
using WorkOrderEMS.Models.UserModels;
using static WorkOrderEMS.Models.eFleetDriverModel;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IReportManager
    {

        List<CleaningModel> NoCleaningDone();
        //List<TrashData> GetTrashData();
        List<ReportChart> GetRoutineCheckList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);
        List<SelectListItem> GetAssetListForReportingDDL(long? LocationId);
        /// <summary>Created by Bhushan Dod on 22/04/2015
        /// Get details of Cleaning done for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<ReportChart> GetCleaningDoneList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        /// <summary>Created by Bhushan Dod on 17/04/2015
        /// Get details of Employee scan qrc log
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<ReportChart> QrcTypeScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);
        /// <summary>Created by Ankit Choudhary on 01/02/2017
        /// Get details of Employee scan qrc log for Routine
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>


        List<WorkOrderIssueedModel> GetWorkOrderListForLocation(long? LocationID, long? UserId, long LoginUserId, string FromDate, string ToDate, string WorkRequestProjectType, string textSearch);


        /// <summary>Created by Bhushan Dod on 15/04/2015
        /// Get details of QRC Scan for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<ReportChart> GetQrcScanList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        /// <summary>Created by Bhushan Dod on 17/04/2015
        /// Get details of work order fixed time
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<WorkOrderIssueedModel> GetWorkOrderIssuedListFixedTime(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, string textSearch);

        /// <summary>Created by Bhushan Dod on 23/04/2015
        /// Get details of Cleanin done qrc log
        /// </summary>
        /// <param name="qrcName,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<ReportChart> RoutineDoneListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);

        List<ReportChart> GetTrashLevelList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        /// <summary>Created by Bhushan Dod on 27/04/2015
        /// Get details of trash picked up for High Charts
        /// </summary>
        /// <param name="LocationId,UserId,FromDate,ToDate"></param>
        /// <returns></returns>
        List<ReportChart> GetTrashPickedUpList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> ReportedTrashLevelListByEmployee(long? LocationId, string trashname, string userName, string qrctrashlevel, DateTime? FromDate, DateTime? ToDate, string TrashType);

        List<ReportChart> GetTrashPickedUpListDetails(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);
        List<QRCModel> GetExpirationDateList(long? LocationID, int ExpirationType);

        List<QRCModel> PurchaseTypeList(long? LocationID, int OwnedType);

        List<ReportChart> GetDamageVehicleList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> GetDetailsOfAllQrc(long? LocationId, long? UserId, string name);

        List<WorkOrderIssueedModel> GetWorkOrderAssignedListForLocationItem(long LoginUserID, long? LocationID, string FromDate, string ToDate, long? QrcId, long? ReqType, int? WorkRequestProjectType, long? safetyHuzzard, long? priorityLevel, long? userId, string textSearch);

        List<WorkOrderIssueedModel> GetWorkOrderInProgressListForLocation(long LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? UserId, string textSearch);

        List<WorkOrderIssueedModel> GetWorkOrderMissedTime(long? LoginUserID, long? LocationID, string FromDate, string ToDate, int? WorkRequestProjectType, long? PriorityLevel, long? UserId, string textSearch);

        List<DARModel> GetDetailsOfDARCustomerAssistance(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate);

        List<DARModel> GetDetailsOfDARJumpStarts(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate);

        List<DARModel> GetDetailsOfDARTireInflation(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate);

        List<DARModel> GetDetailsOfDARSpaceCount(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate);

        List<ReportChart> GetDARCodesList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> DARSubmittedListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);

        List<ReportChart> GetCompletedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> WorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);

        List<ReportChart> GetIssuedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> GetAssignedWorkOrder(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, string name);

        List<ReportChart> AssignedWorkOrderListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);

        List<ReportChart> RoutineScanListByEmployee(long? LocationId, string qrcName, string userName, DateTime? FromDate, DateTime? ToDate);

        List<FuelJQGridList> GetFuelingReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch);

        List<eFleetDriverReportModel> GetDriverDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch);

        List<eFleetMaintenanceModel> GeteMaintenanceDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch);

        List<eFleetReportIncidentDetails> GetIncidentDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? userId, string textSearch);

        List<eFleetVehicleModel> GetVehicleDetailsReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch);

        List<eFleetMileageReportModel> GetMileageReport(long LoginUserID, long? LocationId, DateTime FromDate, DateTime ToDate, long? FuelType, long? userId, string textSearch);

        List<eFleetDamageReportModel> GeteFleetDamageList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle);

        eFleetDamageReportModel GeteFleetVehicleMasterLogDetailsById(long logID);

        List<InspectionVehicleModel> GeteFleetInspectionList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle);

        List<eFleetDefectReportModel> GeteFleetDefectList(long? LocationId, long? UserId, DateTime? FromDate, DateTime? ToDate, long? vehicle);

        eFleetDefectReportModel GeteFleetDefectDetailsById(long logID);

        List<eFleetPassengerCountReportModel> GeteFleetPassengerCountHoursOfWeekDays(long? LocationId, long? UserId, DateTime FromDate, DateTime? ToDate, int curretWeek);

        //Idle Employee Report
        List<IdleEmployeeModel> IdleEmployeeList(long? LocationId, long? UserId, DateTime _fromDate, DateTime? _toDate, DateTime? fromMonth, DateTime? toMonth);
        // List<IdleEmployeeModel> IdleEmployeeList(long? LocationId, long? UserId, DateTime _fromDate, DateTime? _toDate);

        List<IdleEmployeeModel> IdleEmployeeListForSelectedDateRange(long? LocationId, long? UserId, DateTime _fromDate, DateTime? _toDate);

        //List<ReportChartIdleEmployee> IdleEmployeeListByEmployee(long? userId, string UserName, long? LocationId, DateTime? FromDate, DateTime? ToDate);

        List<IdleEmployeeModel> IdleEmployeeListForWeek(long? LocationId, long? UserId, int weekNumber);
        List<ReportChartIdleEmployee> IdleEmployeeListDataByEmployee(long? userId, string UserName, long? LocationId, DateTime? FromDate, DateTime? ToDate);
        //End Idle Employee Report
    }
}
