using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class AssetsAllocationModel
    {        
        public string EmployeeId { get; set; }
        public long ApplicantId { get; set; }
        public long AssetsId { get; set; }
        public string Action { get; set; }
        public ComputerAssets ComputerAssets { get; set; }
        public bool IsComputerAssets { get; set; }
        public MiscAssets MiscAssets { get; set; }
        public bool IsMiscAssets { get; set; }
        public CellPhoneAssets CellPhoneAssets { get; set; }
        public bool IsCellPhoneAssets { get; set; }
        public PrinterAssets PrinterAssets { get; set; }
        public bool IsPrinterAssets { get; set; }
        public OfficePhone OfficePhone { get; set; }
        public bool IsOfficePhone { get; set; }
        public Nullable<DateTime> AssignDate { get; set; }
        public string ReturnAcceptBy { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }
        public string ReturnStatus { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public string IsActive { get; set; }
        public string EmployeeName { get; set; }
        public string OperationalHead { get; set; }
        public string employeephoto { get; set; }
    }
    public class ComputerAssets
    {
        public string AssetsName { get; set; }
        public string AssetsType { get; set; }
        public string AssetDescription { get; set; }
        public string SerialNumber { get; set; }
        public DateTime Make { get; set; }
        public string Model { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class CellPhoneAssets
    {
        public string AssetsName { get; set; }
        public string AssetsType { get; set; }
        public string AssetDescription { get; set; }
        public DateTime Make { get; set; }
        public string Model { get; set; }
        public string Number { get; set; }
        public string VoiceMailCode { get; set; }
        public string SerialNumber { get; set; }
    }
    public class PrinterAssets
    {
        public string AssetsName { get; set; }
        public string AssetsType { get; set; }
        public string AssetDescription { get; set; }
        public DateTime Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
    }
    public class OfficePhone
    {
        public string AssetsName { get; set; }
        public string AssetsType { get; set; }
        public string AssetDescription { get; set; }
        public DateTime Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string VoiceMailCode { get; set; }
    }
    public class MiscAssets
    {
        public string AssetsName { get; set; }
        public string AssetsType { get; set; }
        public string AssetDescription { get; set; }
        public DateTime Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Equipment { get; set; }
    }
}
