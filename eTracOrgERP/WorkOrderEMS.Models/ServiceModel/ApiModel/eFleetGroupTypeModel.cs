using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetDamageTireModel : BaseTimeZoneParameter
    {
        public string ServiceAuthKey { get; set; }

        public long UserId { get; set; }

        public long VehicleID { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public bool DamageStatus { get; set; }

        public ShuttleDamage Damage { get; set; }

        public Tires Tires { get; set; }

        public string AllPictures { get; set; }

        public int WorkOrderRequestId { get; set; }
    }

    public class eFleetInteriorMileageDriverModel : BaseTimeZoneParameter
    {
        public string ServiceAuthKey { get; set; }

        public long UserId { get; set; }

        public long VehicleID { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public bool DamageStatus { get; set; }

        public ShuttleMileage Mileage { get; set; }

        public Interior Interior { get; set; }

        public DriversCabin DriversCabin { get; set; }

        public string AllPictures { get; set; }

        public int WorkOrderRequestId { get; set; }
    }

    public class eFleetEngineExteriorModel : BaseTimeZoneParameter
    {
        public string ServiceAuthKey { get; set; }

        public long UserId { get; set; }

        public long VehicleID { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public bool DamageStatus { get; set; }

        public Engine Engine { get; set; }

        public Exterior Exterior { get; set; }

        public string AllPictures { get; set; }

        public int WorkOrderRequestId { get; set; }
    }

    public class eFleetEmergencyAccessoriesModel : BaseTimeZoneParameter
    {
        public string ServiceAuthKey { get; set; }

        public long UserId { get; set; }

        public long VehicleID { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public bool DamageStatus { get; set; }

        public Accessories Accessories { get; set; }

        public Emergency Emergency { get; set; }

        public string AllPictures { get; set; }

        public int WorkOrderRequestId { get; set; }
    }
    public class BaseTimeZoneParameter
    {

        public string TimeZoneName { get; set; }

        public long TimeZoneOffset { get; set; }

        public bool IsTimeZoneinDaylight { get; set; }
    }

    public class Engine
    {
        
        public bool IsFluidLeak { get; set; }

        public bool IsFluidLeakClicked { get; set; }
        
        public long FluidWorkOrderId { get; set; }
        
        public long FluidDarId { get; set; }

        
        public bool IsLooseWire { get; set; }

        public bool IsLooseWireClicked { get; set; }
        
        public long LooseWorkOrderId { get; set; }

        
        public long BeltCondition { get; set; }
        
        public long BeltWorkOrderId { get; set; }

        
        public long OilLevel { get; set; }
        
        public long OilLevelWorkOrderId { get; set; }

        
        public bool IsRadiatorClicked { get; set; }
        
        public long IsRadiator { get; set; }
        
        public long RadiatorWorkOrderId { get; set; }

        
        public long IsEngineNoise { get; set; }
        
        public bool IsEngineNoiseClicked { get; set; }
        
        public long EngNoiseWorkOrderId { get; set; }
    }

    public class Exterior
    {
        
        public long Headlights { get; set; }
        
        public long HeadlightsWorkOrderId { get; set; }
        
        public bool IsFlashers { get; set; }
        
        public bool IsFlashersClicked { get; set; }
        
        public long FlashersWorkOrderId { get; set; }
        
        public bool IsReflectors { get; set; }
        
        public bool IsReflectorsClicked { get; set; }
        
        public long ReflectorsWorkOrderId { get; set; }
        
        public long Window { get; set; }
        
        public long WindowWorkOrderId { get; set; }
        
        public bool IsExhaust { get; set; }
        
        public bool IsExhaustClicked { get; set; }
        
        public long ExhaustWorkOrderId { get; set; }
        
        public bool IsTailPipe { get; set; }
        
        public bool IsTailPipeClicked { get; set; }
        
        public long TailPipeWorkOrderId { get; set; }
        
        public bool IsWindShields { get; set; }
        
        public bool IsWindShieldsClicked { get; set; }
        
        public long WindShieldsWorkOrderId { get; set; }

        
        public long FuelLevel { get; set; }
        
        public long FuelLevelWorkOrderId { get; set; }
    }
}
