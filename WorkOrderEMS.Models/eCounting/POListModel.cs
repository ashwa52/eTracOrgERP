using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class POListModel
    {
        public long? LogPOId { get; set; }
        public string LocationName { get; set; }
        public Nullable<DateTime> PODate { get; set; }
        public string POType { get; set; }
        public string CompanyName { get; set; }
        public Nullable<DateTime> DeliveryDate { get; set; }
        public string POStatus { get; set; }
        public long? LogId { get; set; }
        public decimal? Total { get; set; }
        public string POStatusToDisplay { get; set; }
        public long? CreatedBy { get; set; }
        public string UserName { get; set; }
    }

    public class POListDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<POListModel> rows { get; set; }
    }

    public class POApproveRejectModel
    {
        public string POApproveRemoveId { get; set; }
        public long POId { get; set; }
        [Required(ErrorMessage ="Comment is required")]
        public string Comment { get; set; }
        public long LocationId { get; set; }
        public long UserId { get; set; }
        public long POModifiedId { get; set; }
        public long LogId { get; set; }
        public long? QuickBookPOId { get; set; }
        public decimal? Amount { get; set; }
    }
}
