using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.CommonModels
{
    public class UploadEmployeeFiles
    {
        public long Id { get; set; }
        public string UploadFile { get; set; }
        public string FileType { get; set; }
        //public string FileType { get; set; }
    }
}
