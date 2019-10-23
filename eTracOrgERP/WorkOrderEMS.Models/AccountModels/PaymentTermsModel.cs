using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;

namespace WorkOrderEMS.Models
{
    public class PaymentTermsModel
    {
        public long PTM_Id { get; set; }
        [DisplayName("Payment Term")]
        public string PTM_Term { get; set; }

        [DisplayName("Grace Period")]
        public int PTM_GracePeriod { get; set; }
        public string PTM_IsActive { get; set; }
        public Result Result { get; set; }
    }
    public class PaymentTermsDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<PaymentTermsModel> rows { get; set; }
    }
}
