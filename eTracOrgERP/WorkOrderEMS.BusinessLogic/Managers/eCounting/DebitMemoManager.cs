using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.eCounting.DebitMemo;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class DebitMemoManager : IDebitMemo
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string DebitMemoDocumentPath = System.Configuration.ConfigurationManager.AppSettings["DebitMemoDocuments"];
        public List<DebitMemoModel> GetDebitListByVendorId()
        {
            try
            {
                var result = new List<DebitMemoModel>();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<DebitMemoModel> GetDebitListByLocationId(long? LocationId)
        {
            try
            {
                var status = 0;
                var result = new List<DebitMemoModel>();
                result = _workorderems.spGetDebitMemoList(LocationId, status).Select
                    (x => new DebitMemoModel()
                    {
                        DebitId = x.DBM_Id,
                        Location = x.LCM_CMP_Id,// location id company  
                        LocationName = x.LocationName,
                        VendorId = x.CMP_Id,
                        VendorName = x.CMP_NameLegal,
                        PurchaseOrderId = x.DBM_PurchaseOrder,
                        ProductOrderName = "PO"+ x.DBM_PurchaseOrder,
                        DebitAmount = x.DBM_DebitAmount,
                        Note = x.DBM_Note,
                        Status = x.DBM_Status,
                        EditStatus = x.DBM_Status,
                        UploadedEditDocumentName = x.DBM_DocumentName,//to delete old document if new file added in edit
                        UploadedDocumentName = x.DBM_DocumentName == null ? HostingPrefix + DebitMemoDocumentPath.Replace("~","") + "Code20-Emergency.png" : HostingPrefix + DebitMemoDocumentPath.Replace("~","") + x.DBM_DocumentName,
                        DisplayDate = x.DBM_CreatedDate.HasValue ? x.DBM_CreatedDate.Value.ToString("dd/MM/yyyy"):"" //for nullable datetime check hasvalue and .value
                    }).ToList();

                foreach (var item in result)
                {
                    if (item.Status != null) {
                        DebitMemoStatus e = (DebitMemoStatus)item.Status;
                        item.DisplayDebitMemoStatus = e.ToString();
                    }                   
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SaveNewDebitMemo(DebitMemoModel model)
        {
            try
            {
                string action = "";
                DateTime? modifieddate;
                if (model.DebitId == 0)
                {
                    action = "I";
                    modifieddate = null;
                }
                else {
                    action = "U";
                    modifieddate = DateTime.Now;                    
                }
                
                bool IsSaved = false;
                
                var result = _workorderems.spSetDebitMemo(model.DebitId, action,model.Location ,model.VendorId, model.PurchaseOrderId , model.DebitAmount, model.Note, (int)model.DebitMemoStatus, model.UploadedDocumentName, DateTime.Now, modifieddate, null, false, null, null);
                if (result == 1)//return coming in 1 after save
                {
                    IsSaved = true;
                }
                else {
                    IsSaved = false;
                }

                return IsSaved;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }    
}
