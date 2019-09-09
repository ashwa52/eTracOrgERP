using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IPDFFormManager
    {
        PDFFormModelDetails GetPDFFormList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy);
        bool SavePDFForm(PDFFormModel Obj);
        List<ModuleListModel> GetModuleList();
        PDFFormModel GetePDFFormDetailsById(long formID);
        bool ActiveFormById(long Id, long UserId, string IsActive);
    }
}
