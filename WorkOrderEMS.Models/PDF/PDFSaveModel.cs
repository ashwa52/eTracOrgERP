using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class PDFSaveModel
    {
        public long UserId { get; set; }
        public long LocationId { get; set; }
        public string FileName { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public long ModuleId { get; set; }
        public long FormId { get; set; }
    }
}
