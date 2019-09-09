using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models.AccountModels
{
    public class PaymentModeModel
    {
        public long PMD_Id { get; set; }

        [DisplayName("Payment Mode")]
        public string PMD_PaymentMode { get; set; }
        public string PMD_IsActive { get; set; }
        public Result Result { get; set; }
    }
    public class PaymentModeDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<PaymentModeModel> rows { get; set; }
    }
}
