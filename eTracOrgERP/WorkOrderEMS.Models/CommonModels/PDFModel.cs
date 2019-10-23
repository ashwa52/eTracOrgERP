using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PDFModel
    {
        public string EncryptVehicleId { get; set; }
        public string htmlData { get; set; }
        public string InspectionSignatureImage { get; set; }
        public string Username { get; set; }
    }
}
