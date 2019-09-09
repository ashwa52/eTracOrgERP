using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class PDFFormRepository
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public List<PDFFormModel> GetFormName()
        {
            try
            {
                return _workorderems.spGet_eForms().Select(x => new PDFFormModel()
                {
                    FormName = x.EFM_eFormName,
                    FormId = x.EFM_Id
                }).ToList();
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
