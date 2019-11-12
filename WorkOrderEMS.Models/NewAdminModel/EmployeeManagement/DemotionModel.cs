using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class DemotionModel
    {
        public DateTime? EffectiveDate { get; set; }
        public string Position { get; set; }
        public long IsTempDate { get; set; }
        public DateTime? TempDate { get; set; }
        public string Image { get; set; }
        public string EmpId { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string EmploymentStatus { get; set; }
        public int VSC_Id { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public long LocationId { get; set; }
        public string TransferType { get; set; }
        public long JobTitleId { get; set; }
        public long TempDays { get; set; }
        public string StatusAction { get; set; }
    }
}
