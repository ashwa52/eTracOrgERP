using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class CustomerManagementManager : ICustomerManagement
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string Path = ConfigurationManager.AppSettings["CompanyDocument"];
        private string DocFacilityPath = ConfigurationManager.AppSettings["VendorImageFacility"];

        public VendorAllViewDataModel GetAllCustomerData(long CustmerID, string Status)
        {
            return new VendorAllViewDataModel();
        }

        public string GetFirstCompanyByVendorId(int firstcompanyId)
        {
            string _Name = "";
            try
            {
                var _result = _workorderems.Companies.Where(x => x.CMP_Id == firstcompanyId).FirstOrDefault();
                if (_result != null)
                {
                    _Name = _result.CMP_NameLegal;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string ApprovePOByPOId(long Id)", "Exception While Approving PO.", null);
                throw;
            }
            return _Name;
        }
    }
}