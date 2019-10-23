﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
   public class ServiceInspectionModel
    {        
        public string ServiceAuthKey { get; set; }        
        public long UserId { get; set; }        
        public long QrcId { get; set; }
        
        public long LocationId { get; set; }
        
        public string UserName { get; set; }
        
        public string QrcodeId { get; set; }
        
        public string Action { get; set; }
        
        public string Status { get; set; }
        
        public bool DamageStatus { get; set; }
        
        public ShuttleDamage Damage { get; set; }
        
        public ShuttleMileage Mileage { get; set; }
        
        public Engine Engine { get; set; }
        
        public Interior Interior { get; set; }
        
        public Exterior Exterior { get; set; }
        
        public Tires Tires { get; set; }
        
        public DriversCabin DriversCabin { get; set; }
        
        public Accessories Accessories { get; set; }
        
        public Emergency Emergency { get; set; }
        
        public string AllPictures { get; set; }
        
        public int WorkOrderRequestId { get; set; }
    }

    public class InspectionVehicleModel
    {
        public string InspectionLogNo { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public string VehicleIdentificationNo { get; set; }        
        public string VehicleImage { get; set; }
        public string LocationName { get; set; }
        public string PreInspection { get; set; }
        public string PostInspection { get; set; }
        public string InspectedBy { get; set; }
        public string InspectedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
