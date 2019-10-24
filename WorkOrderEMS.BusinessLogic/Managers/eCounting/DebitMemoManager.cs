using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.eCounting.DebitMemo;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DebitMemoManager : IDebitMemo
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        public List<DebitMemoModel> GetDebitList()
        {
            try
            {
                var result = new List<DebitMemoModel>();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }    
}
