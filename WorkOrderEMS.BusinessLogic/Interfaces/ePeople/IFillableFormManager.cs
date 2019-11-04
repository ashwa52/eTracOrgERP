using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{ 
    public interface IFillableFormManager
    {
       CommonFormModel GetFormDetails(CommonFormModel Obj);
        List<FormTypeListModel> GetFileList(eTracLoginModel obj);
        bool SaveFile(UploadedFiles Obj, string EmployeeId);
        bool SaveFileList(CommonFormModel obj);
    }
}
