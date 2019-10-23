using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetMileageReportModel
    {     
        public string FuelTypeName { get; set; }       
        public long UserId { get; set; }
        public long VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public long LocationID { get; set; }
        public string Mileage { get; set; }    
        public long FuelType { get; set; }      
        public string DriverName { get; set; }      
        public string LocationName { get; set; }
        public string CreatedDate { get; set; }
        public string VehicleIdentificationNumber { get; set; }
        public string LicensePlateNumber { get; set; }
    }
}
