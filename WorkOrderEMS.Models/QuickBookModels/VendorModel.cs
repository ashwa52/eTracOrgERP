using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class VendorModel
    {
        public long VendorId { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNo { get; set; }
        public string Website { get; set; }
        public string AccountNo { get; set; }
        public string BalanceAmount { get; set; }
    }
}
