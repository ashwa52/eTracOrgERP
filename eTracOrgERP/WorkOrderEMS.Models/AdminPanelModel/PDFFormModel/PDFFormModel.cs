using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WorkOrderEMS.Models
{
    public class PDFFormModel
    {
        public long FormId { get; set; }
        public string FormType { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string FormName { get; set; }
        public string FormPath { get; set; }
        public HttpPostedFileBase FormPathFile { get; set; }
        public string IsActive { get; set; }
    }
    public class PDFFormModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<PDFFormModel> rows { get; set; }
    }
}
