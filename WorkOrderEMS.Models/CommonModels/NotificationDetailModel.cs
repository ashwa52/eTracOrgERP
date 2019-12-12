using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class NotificationDetailModel
    {
        public long? ModuleId { get; set; }
        public long? AssignTo { get; set; }
        public long? WorkOrderID { get; set; }
        public long? eFleetID { get; set; }
        public long? POID { get; set; }
        public long? MiscellaneousID { get; set; }
        public long? BillID { get; set; }
        public bool IsRead { get; set; }
        public long ReadBy { get; set; }
        public DateTime ReadDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long DeletedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public long CommonId { get; set; }
        public long UserId { get; set; }
        public bool ApproveStatus { get; set; }
        public long? eScanQRCID { get; set; }
        public bool? IsDamage { get; set; }
        public bool? IsCheckOut { get; set; }
        public long NotificationId { get; set; }
    }
}
