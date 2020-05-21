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
        public long? JobTitleId { get; set; }
        public long? TempDays { get; set; }
        public string StatusAction { get; set; }
        public string ChangeType { get; set; }
        public long? JobTitleCurrent { get; set; }
        public long? LocationIdCurrent { get; set; }
        public string EmployeeCurrentStatus { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public long UserId { get; set; }
    }
    public class EmployeeStatusList
    {
        public long ESC_Id { get; set; }
        public string ESC_EMP_EmployeeId { get; set; }
        public string ESC_ChangeType { get; set; }
        public string ESC_ApprovedBy { get; set; }
        public string ESC_ApprovalStatus { get; set; }
        public string ESC_Date { get; set; }
    }
}
