using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Helper
{
    public class PushNotificationMessages
    {

        public static string ItemMissing(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Item missing at {0}", locationName);
        }
        public static string VehiclePartDamage(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Vehicle part is damaged at {0}", locationName);
        }
        public static string WeeklyVehicleCheck(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Weekly checked vehicle part is damaged at {0}", locationName);
        }
        public static string CellphoneScreenCracked(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Cellphone Screen is cracked at {0}", locationName);
        }
        public static string CellphoneButtonsNotPresent(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Cellphone buttons are not present at {0}", locationName);
        }
        public static string CellphoneNotFunctional(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Cellphone not functional at at {0}", locationName);
        }
        public static string NewFacilityRequest(string facilityRequestId)
        {
            return string.Format(CultureInfo.InvariantCulture, "New facility request {0} is genrated", facilityRequestId);
        }
        public static string VehicleMileage(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Vehicle mileage change at  {0}", locationName);
        }
        public static string EmployeeIdle(string locationName,string userName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Employee: {0} is idle from last 30 min at Location:{0}",userName, locationName);
        }
        public static string FacilityRequestIdle(string locationName, string workordercodeId)
        {
            return string.Format(CultureInfo.InvariantCulture, "Facility request: {0} is not accepted by any employee at Location:{1}", workordercodeId, locationName);
        }
        public static string CheckOut(string locationName, string qrcodeId,string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Check In not done for QRC {1} at Location {2} by {0}",UserName, qrcodeId, locationName);
        }
        public static string ElevatorInspection(string locationName, string qrcodeId, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "No Elevator inspection certificate posted for QRC {1} at Location {2} by {0}", UserName, qrcodeId, locationName);
        }
        public static string ElevatorCapacity(string locationName, string qrcodeId, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "No Elevator maximum capacity posted for QRC {1} at Location {2} by {0}", UserName, qrcodeId, locationName);
        }
        public static string ShuttlePartDamage(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Shuttle part is damaged at {0}", locationName);
        }
        public static string ShuttleTirePartDamage(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Shuttle tire part is damaged at {0}", locationName);
        }
        public static string ShuttleMileage(string locationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Shuttle mileage change at  {0}", locationName);
        }
        public static string eFleetInspectionReported(string locationName, string QRCodeID, string vehicleNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Inspection reported for QRC {1}, vehicle number {2} at location {0}", locationName, QRCodeID, vehicleNumber);
        }
        public static string eFleetIncidentForServiceReported(string locationName, string QRCodeID, string vehicleNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Vehicle Incident reported for QRC {1}, vehicle number {2} at location {0}", locationName, QRCodeID, vehicleNumber);
        }
        public static string eFleetFuelingReported(string locationName, string QRCodeID, string vehicleNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Fueling reported for QRC {1}, vehicle number {2} at location {0}", locationName, QRCodeID, vehicleNumber);
        }
        public static string eFleetPreventativeMaintenanceReported(string locationName, string QRCodeID, string vehicleNumber)
        {
            return string.Format(CultureInfo.InvariantCulture, "eFleet Preventative Maintenance reported for QRC {1}, vehicle number {2} at location {0}", locationName, QRCodeID, vehicleNumber);
        }
        public static string POApprovedReject(string locationName, string POId, string Status, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "PO {1} is {2} by {3} at Location {0}", locationName, POId, Status, UserName);
        }
        public static string MiscellaneousApprovedReject(string locationName, string MISNO, string Status, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Miscellaneous {1} is {2} by {3} at Location {0}", locationName, MISNO, Status, UserName);
        }
        public static string BillApprovedReject(string locationName, long BillNo, string Status, string UserName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Bill No {1} is {2} by {3} at Location {0}", locationName, BillNo, Status, UserName);
        }
        public static string POCreate(string PONumber, string UserName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Please approve PO {0}  created by {1} at Location {2}", PONumber, UserName, LocationName);
        }
        public static string POApproved(string PONumber, string UserName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Approved PO {0}  created by {1} at Location {2}", PONumber, UserName, LocationName);
        }
        public static string LastPOApproved(string PONumber, string UserName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "The PO {0} is successfully approved by {1} at Location {2}", PONumber, UserName, LocationName);
        }
        public static string BillCreate(string BillNumber, string UserName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Please approve Bill {0}  created by {1} at Location {2}", BillNumber, UserName, LocationName);
        }
        public static string MiscellaneousCreate(string MiscNumber, string UserName, string LocationName)
        {
            return string.Format(CultureInfo.InvariantCulture, "Please approve Bill {0}  created by {1} at Location {2}", MiscNumber, UserName, LocationName);
        }
    }
}
