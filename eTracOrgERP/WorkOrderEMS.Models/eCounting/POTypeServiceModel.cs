using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.CommonModels;

namespace WorkOrderEMS.Models
{
    public class POTypeServiceModel
    {
        public string PONumber { get; set; }
        public List<POTyeDetailsModelService> POTypeListServiceModel { get; set; }
        public List<LocationListServiceModel> LocationListServiceModel { get; set; }
    }
    public class POTyeDetailsModelService
    {
        public long POType { get; set; }
        public string POTypeName { get; set; }
    }

    public class LocationListServiceModel
    {
        public long LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class VendorTypeListServiceModel
    {
        public string PointOfContact { get; set; }
        public string CompanyNameLegal { get; set; }
        public long CompanyId { get; set; }
    }
    public class QuestionsEmergencyPO
    {
        public string Question { get; set; }
        public long QuestionId { get; set; }
    }
    public class PODetailsServiceModelManager
    {
        public long? POD_Id { get; set; }
        public long? POD_LocationId { get; set; }
        public long? POD_POT_Id { get; set; }
        public long? POD_CMP_Id { get; set; }
        public DateTime? POD_PODate { get; set; }
        public DateTime? POD_DeliveryDate { get; set; }
        public DateTime? POD_ReoccourringBillDate { get; set; }
        public string POD_EmergencyPODocument { get; set; }
        public string POD_IsActive { get; set; }

    }
}
