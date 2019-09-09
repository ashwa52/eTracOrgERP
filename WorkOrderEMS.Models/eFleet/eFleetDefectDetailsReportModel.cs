using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetDefectDetailsReportModel
    {
        public long DefectID { get; set; }
        public long VehicleID { get; set; }
        public long WorkRequestAssignmentID { get; set; }
        public string VehicleNumber { get; set; }
        public string QRCodeID { get; set; }
        public string Status { get; set; }
        public string DefectType { get; set; }
        public long InspectionType { get; set; }
        public string DamageFile { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }
}
