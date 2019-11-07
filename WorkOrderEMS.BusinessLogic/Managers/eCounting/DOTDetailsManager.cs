using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.ServiceModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DOTDetailsManager : IDOTDetails
    {
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private string POEmeregencyImagePath = ConfigurationManager.AppSettings["POEmeregencyImage"];
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        public List<DOTManagementViewDataModel> GetAllDOTList(long? UserId, long? LocationId, string status, long? UserTypeId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            long Type = Convert.ToInt64(UserType.Employee);
            //LocationId = 0; //To get all PO for user to approve for all location as per SQL Developer will change if client want to change
            var Results = new List<DOTManagementViewDataModel>();
            try
            {
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
               // Results = _workorderems.spGetDriverList()
               //.Select(a => new DOTManagementViewDataModel()
               //{
               //    //FirstName = a.Id,
               //    //LastName = "PO" + a.LPOD_POD_Id,
               //    //ExpirationDate = a.LPOD_PODate != null ? a.LPOD_PODate.ToString("yyyy/MM/dd") : "N/A",
               //    //LicenseNumber = a.LocationName,
               //    //State = a.LPOD_DeliveryDate != null ? a.LPOD_DeliveryDate.ToString("yyyy/MM/dd") : "N/A",
               //    //LicenseExpirationDate = a.LicenseExpirationDate,
               //}).OrderByDescending(x => x.Id).ToList();

                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public ContractTypeDetails GetContractTypeList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of contract types.", null);
                throw;
            }
        }
    }
}
