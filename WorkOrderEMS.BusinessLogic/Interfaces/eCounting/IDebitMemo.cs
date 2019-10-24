using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.eCounting.DebitMemo;

namespace WorkOrderEMS.BusinessLogic.Interfaces.eCounting
{
    public interface IDebitMemo
    {
        List<DebitMemoModel> GetDebitList();
    }
}
