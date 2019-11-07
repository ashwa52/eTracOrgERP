using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.GlobalSettings
{
    public class BankAccount
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankUserId { get; set; }
        public string BankPin { get; set; }
        public bool Status { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
