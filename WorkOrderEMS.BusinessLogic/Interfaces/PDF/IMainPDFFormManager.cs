using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface IMainPDFFormManager
    {
        PDFFormModelDetails GetMainPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        List<PDFFormModel> GetPDFFormNameList();
        bool SavePDFData(PDFSaveModel obj);
    }
}
