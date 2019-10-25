using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class FormTypeListModel
    {
        public long FileTypeId { get; set; }
        public string FileType { get; set; }
        public string  FileName { get; set; }
        public string IsActive { get; set; }

    }
    public class UploadedFiles
    {
        public long FileId { get; set; }
        public long FileTypeId { get; set; }
        public string FileEmployeeId { get; set; }
        public string FileName { get; set; }
        public string FileTypeName { get; set; }
        public string AttachedFileName { get; set; }
        public string EmployeeId { get; set; }
    }
}
