using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.TCAPIP
{

    
    //public class AssetModel
    //{
    //    public EmployeeDetailsModel EmployeeDetailsModel { get; set; }
    //    public List<EmployeeAssetDetails> EmployeeAssetDetails { get; set; }  
       

    //}

    public class EmployeeDetailsModel
    {
        public List<EmployeeAssetDetails> EmployeeAssetDetails { get; set; }
        public string empid { get; set; }
        public string employee_name { get; set; }
        public string employee_photo { get; set; }
        public string OperationHead { get; set; }
        public string man_id { get; set; }
        public string emp_email { get; set; }
    }
    public class EmployeeAssetDetails
    {
        public long AssetRowId { get; set; }
        public string AssetName { get; set; }
        public string AssetDetails { get; set; }
        public Nullable<DateTime> ReturnDate { get; set; }
        public string AssetStatus { get; set; }
        

    }
        

    
}
