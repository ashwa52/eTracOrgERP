using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetPassengerTrackingModel
    {
        public long RouteID { get; set; }
        public long ServiceType { get; set; }
        public string RouteName { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public System.DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public System.DateTime EndDate { get; set; }
        public Nullable<long> LocationID { get; set; }
        [Required(ErrorMessage = "PickUp point is required")]
        public string PickUpPoint { get; set; }
        [Required(ErrorMessage = "Drop point is required")]
        public string DropPoint { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ServiceTypeName { get; set; }

        public string PickupList { get; set; }
        public string DropList { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    public class eFleetPassengerTrackingCountModel
    {
        public long PassengerCountID { get; set; }
        public string RouteName { get; set; }
        public long ServiceType { get; set; }
        public string ServiceTypeName { get; set; }
        public string VehicleNumber { get; set; }
        public string PickUpPoint { get; set; }
        public string DropPoint { get; set; }       
        public Nullable<long> PassengerCount { get; set; }
        public string EmployeeName { get; set; }    
        public System.DateTime CreatedDate { get; set; }


        public string WeekDays { get; set; }
        public long WeekNumber { get; set; }
       public List<Nullable<long>> CountPassenger { get; set; }
    }
}
