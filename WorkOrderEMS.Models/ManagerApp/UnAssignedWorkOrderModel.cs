using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class UnAssignedWorkOrderModel
    {
        public long WorkRequestId { get; set; }
        public long LocationId { get; set; }
        public string Description { get; set; }
        public long? WorkRequestType { get; set; }
        public long WorkOrderCodeID { get; set; }
        public string WorkOrderCode { get; set; }
        public string WorkRequestImage { get; set; }
        public string PriorityLevel { get; set; }
        public long? WorkOrderStatus { get; set; }
        public string StartTime { get; set; }
        public string UserName { get; set; }
        public string LocationName { get; set; }
        public long WorkRequestProjectType { get; set; }

        //For Facility
        public string ProjectDescription { get; set; }
        public bool? SafetyHazard { get; set; }
        public long? AssignedByUserId { get; set; }
        public long RequestBy { get; set; }
        public string RequestedName { get; set; }
        public string CreatedDate { get; set; }
        public string FrCurrentLocation { get; set; }
        public string CustomerContact { get; set; }
        public string CustomerName { get; set; }
        public string DriverLicenseNo { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleMake1 { get; set; }
        public string VehicleModel1 { get; set; }
        public string VehicleYear { get; set; }
        public string AddressFacilityReq { get; set; }
        public string LicensePlateNo { get; set; }
        public string FacilityRequest { get; set; }
        public long? AssetID { get; set; }
        public string QRCName { get; set; }
        public string AssetName { get; set; }
        public string WorkRequestTypeName { get; set; }
        public string WorkRequestStatusName { get; set; }
        public string WorkRequestProjectTypeName { get; set; }
        public long? UserType { get; set; }
        public long? AssignToUserId { get; set; }
        public string AssignByUserName { get; set; }
    }
}
