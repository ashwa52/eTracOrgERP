using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class ContactModel
    {        
        public long? ContactNo { get; set; }
        public string EmailId { get; set; }
        public long ContactId { get; set; }
        public string IsChecked { get; set; }
        public long? ACI_APT_ApplicantId { get; set; }
    }
    public class DocumentUpload
    {
        public string License { get; set; }
        public string SSNNo { get; set; }
        public long ApplicantId { get; set; }
    }
    public class ContactListModel
    {
        public List<ContactModel> ContactModel { get; set; }
        public ContactModel ContactModelData { get; set; }
        public DocumentUpload DocumentUpload { get; set; }

    }
}
