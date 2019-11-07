using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WorkOrderEMS.Data.Classes;
 
using WorkOrderEMS.Data.EntityModel;
 
using WorkOrderEMS.Models;
 
using WorkOrderEMS.Models.GlobalSettings;

namespace WorkOrderEMS.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ARService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ARService.svc or ARService.svc.cs at the Solution Explorer and start debugging.
    public class ARService : IARService
    {
        public void DoWork()
        {
        }
        DBUtilities DB = new DBUtilities();

        #region AR Rules
        public List<ARRules> GetAllARRules(int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string RuleStastus = null, int RuleId = 0)
        {
            if (page == null)
                page = 1;
            if (rows == null)
                rows = 10;

            string SQRY = "EXEC SP_GetAllARRules N'" + RuleStastus + "','" + txtSearch + "','" + RuleId + "','" + page + "', '" + rows + "', '" + sidx + "','" + sord + "'";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return RuleList;
        }

        public DataTable InsertUpdateARRules(string XML1, string EntryType)
        {
            string SQRY = "exec USP_GlobalARRule_InsertUpdate '" + XML1 + "' ,'" + EntryType + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }

 
        #endregion


        #region Bank Account

        public List<BankAccount> GetAllBankAccount( int BankUserId = 0, int Id = 0, int BankId = 0)
        {
           

            string SQRY = "EXEC USP_BankAccountMapping_List N'" + Id + "','" + BankUserId + "','" + BankId + "'";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<BankAccount> ItemList = DataRowToObject.CreateListFromTable<BankAccount>(DT);
            return ItemList;
        }

        public DataTable  BankAccountMapping_InsertUpdate(string XML1, string EntryType)
        {
            string SQRY = "exec USP_Invoices_InsertUpdate '" + XML1 + "' ,'" + EntryType + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }

        #endregion


        #region Customer Invoice 

        public List<Invoices> GetAllInvoice(DateTime? FromDate, DateTime? ToDate ,string InvoiceNo = "", int InvoiceType = 0, int InvoiceCriteria = 0, int Id = 0)
        {
            string SQRY = "EXEC USP_Invoices_List N'" + InvoiceNo + "','" + InvoiceType + "','" + InvoiceCriteria + "','" + FromDate + "','" + ToDate + "','" + Id + "'";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<Invoices> ItemList = DataRowToObject.CreateListFromTable<Invoices>(DT);
            return ItemList;
        }

        public DataTable  Invoices_InsertUpdate(string XML1, string EntryType)
        {
            string SQRY = "exec USP_Invoices_InsertUpdate '" + XML1 + "' ,'" + EntryType + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }


        public DataTable InvoicesApproveReject(int Id, int AppRejType,string ApprovedBy)
        {
            string SQRY = "exec USP_InvoicesApproveReject '" + Id + "' ,'" + AppRejType + "','" + ApprovedBy + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }

        #endregion

        public List<GlobalCode> GetGlobalCodes(  int Id = 0,string Category="")
        {
            string SQRY = "EXEC USP_GetGlobalCode N'" + Id + "','" + Category + "' ";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<GlobalCode> ItemList = DataRowToObject.CreateListFromTable<GlobalCode>(DT);
            return ItemList;
        }
 
        #region Customer Master

        public DataTable InsertUpdateCustomerDetailsSubmit(string Action, string xmlBasicDetails, string xmlVehicleDetails, string xmlPaymentAccountDetails
            , long ModifiedBy, long ApprovedBy, string IsActive)
        {
            string QueryString = "";
            QueryString = "EXEC USP_InsertUpdate_CustomerBasicDetails '"
                            + Action.Trim() + "','"
                            + xmlBasicDetails.Trim() + "','"
                            + xmlVehicleDetails.Trim() + "','"
                            + xmlPaymentAccountDetails + "','"
                            + ModifiedBy + "','"
                            + ApprovedBy + "','"
                            + IsActive + "'";

            return DB.GetDTResponse(QueryString);
        }
        public List<CustomerSetupManagementModel> Get_CustomerList(long? locationId = 0, string search = null)
        {
            string QueryString = "EXEC USP_Get_CustomerList '" + locationId + "','" + search + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<CustomerSetupManagementModel> ItemList = DataRowToObject.CreateListFromTable<CustomerSetupManagementModel>(dataTable);
            return ItemList;
        }

        public List<CustomerAllViewDataModel> GetCustomerAllDetailForEditApproval(long? CustomerId)
        {
            string QueryString = "EXEC USP_Get_CustomerAllDetailForEditApproval '" + CustomerId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<CustomerAllViewDataModel> ItemList = DataRowToObject.CreateListFromTable<CustomerAllViewDataModel>(dataTable);
            return ItemList;
        }
        public List<CustomerAllViewDataModel> GetCustomerAllDetailForApproval(long? CustomerId)
        {
            string QueryString = "EXEC USP_Get_CustomerAllDetailForApproval '" + CustomerId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<CustomerAllViewDataModel> ItemList = DataRowToObject.CreateListFromTable<CustomerAllViewDataModel>(dataTable);
            return ItemList;
        }
        public List<CustomerVehicleDetails> GetCustomerVehicleDetails(long? CustomerId)
        {
            string QueryString = "EXEC USP_GetCustomerVehicleDetails '" + CustomerId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<CustomerVehicleDetails> ItemList = DataRowToObject.CreateListFromTable<CustomerVehicleDetails>(dataTable);
            return ItemList;
        }

        public DataTable InsertUpdateCustomerDetailsSubmit(long cmp_Id, string comment, string isApprove, long approvedBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_Set_ApprovalForCustomerAllDetail '"
                            + cmp_Id + "','"
                            + (string.IsNullOrEmpty(comment)==true ? "" : comment) + "','"
                            + isApprove.Trim() + "','"
                            + approvedBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public CustomerSetupManagementModel Get_CustomerDetail_ForEdit(long? CustomerId)
        {
            CustomerSetupManagementModel ItemList = new CustomerSetupManagementModel();
            ItemList.CustomerPaymentModel = new CustomerPaymentInformationModel();
            //ItemList.CustomerVehicleModel = new CustomerVehicleInformationModel();
            //ItemList.CustomerVehicleModel.CustomerVehicleDetails = new List<CustomerVehicleDetails>();
            var lstVehicleData = new CustomerVehicleInformationModel();
            string QueryString = "EXEC USP_Get_CustomerDetail_ForEdit '" + CustomerId + "'";
            DataSet dataSet = DB.GetDSResponse(QueryString);
            if (dataSet.Tables.Count > 0) {
                ItemList = DataRowToObject.CreateListFromTable<CustomerSetupManagementModel>(dataSet.Tables[0]).FirstOrDefault();
                var PaymentDetails = DataRowToObject.CreateListFromTable<CustomerPaymentInformationModel>(dataSet.Tables[1]);
                var vehicleList = DataRowToObject.CreateListFromTable<CustomerVehicleDetails>(dataSet.Tables[2]);
                ItemList.CustomerPaymentModel = PaymentDetails.FirstOrDefault();
                //ItemList.CustomerVehicleModel.CustomerVehicleDetails = vehicleList;
                lstVehicleData.CustomerVehicleDetails = vehicleList;
            }
            ItemList.CustomerVehicleModel = lstVehicleData;
            return ItemList;
        }

        public DataTable GetMonthlyPriceFromLocationSetting(long? Id)
        {
            string QueryString = "EXEC USP_GetMonthlyPriceFromLocationSetting '" + Id + "'";
            return DB.GetDTResponse(QueryString);
        }
        #endregion

    }
}
