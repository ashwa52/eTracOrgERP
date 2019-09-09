using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class ContractDropdownDetailsModel
    {
        public long ContractTypeId { get; set; }
        public string ContractType { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public long ClientInvoicingId { get; set; }
        public string ClientInvoicingName { get; set; }
    }
}
