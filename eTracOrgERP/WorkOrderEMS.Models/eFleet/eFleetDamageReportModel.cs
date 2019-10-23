using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetDamageReportModel
    {
        public string eFleetVehicleLogId { get; set; }
        public long UserId { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }      
        public string DriverName { get; set; }
        public string LocationName { get; set; }
        public string StrCreatedDate { get; set; }
        public string VehicleIdentificationNumber { get; set; }
        public string LicensePlateNumber { get; set; }
        public string CroppedPicture { get; set; }
        public string CapturedPicture { get; set; }
        public string CroppedPicture1 { get; set; }
        public string CapturedPicture1 { get; set; }
        public string CroppedPicture2 { get; set; }
        public string CapturedPicture2 { get; set; }
        public string CroppedPicture3 { get; set; }
        public string CapturedPicture3 { get; set; }
        public string CroppedPicture4 { get; set; }
        public string CapturedPicture4 { get; set; }
        public string DamageDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
