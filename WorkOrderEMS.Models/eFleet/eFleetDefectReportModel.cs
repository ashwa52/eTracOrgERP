using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class eFleetDefectReportModel
    {
        public DateTime CreatedOn { get; set; }
        public long WorkOrderRequestId { get; set; }
        public long UserId { get; set; }

        public long? VehicleID { get; set; }
        public string VehicleNumber { get; set; }

        public long LocationId { get; set; }

        public string UserName { get; set; }

        public string QrcodeId { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public bool DamageStatus { get; set; }
        public string AllPictures { get; set; }
        public string ProjectDescripton { get; set; }
        public string InspectionType { get; set; }
        public long DefectId { get; set; }
        public string DefectIDForDownload { get; set; }
        public string  DefectType { get; set; }
        public List<eFleetEmergencyAccessoriesModel> FilterList { get; set; }

    }
}
