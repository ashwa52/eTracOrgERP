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
using WorkOrderEMS.Models.eCounting;
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

        public List<BankAccount> GetAllBankAccount(int BankUserId = 0, int Id = 0, int BankId = 0)
        {


            string SQRY = "EXEC USP_BankAccountMapping_List N'" + Id + "','" + BankUserId + "','" + BankId + "'";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<BankAccount> ItemList = DataRowToObject.CreateListFromTable<BankAccount>(DT);
            return ItemList;
        }

        public DataTable BankAccountMapping_InsertUpdate(string XML1, string EntryType)
        {
            string SQRY = "exec USP_Invoices_InsertUpdate '" + XML1 + "' ,'" + EntryType + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }

        #endregion


        #region Customer Invoice 

        public List<Invoices> GetAllInvoice(DateTime? FromDate, DateTime? ToDate, string InvoiceNo = "", int InvoiceType = 0, int InvoiceCriteria = 0, int Id = 0)
        {
            string SQRY = "EXEC USP_Invoices_List N'" + InvoiceNo + "','" + InvoiceType + "','" + InvoiceCriteria + "','" + FromDate + "','" + ToDate + "','" + Id + "'";
            DataTable DT = DB.GetDTResponse(SQRY);

            List<Invoices> ItemList = DataRowToObject.CreateListFromTable<Invoices>(DT);
            return ItemList;
        }

        public DataTable Invoices_InsertUpdate(string XML1, string EntryType)
        {
            string SQRY = "exec USP_Invoices_InsertUpdate '" + XML1 + "' ,'" + EntryType + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }


        public DataTable InvoicesApproveReject(int Id, int AppRejType, string ApprovedBy)
        {
            string SQRY = "exec USP_InvoicesApproveReject '" + Id + "' ,'" + AppRejType + "','" + ApprovedBy + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            //   List<ARRules> RuleList = DataRowToObject.CreateListFromTable<ARRules>(DT);
            return DT;
        }

        #endregion

        public List<GlobalCode> GetGlobalCodes(int Id = 0, string Category = "")
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
                            + (string.IsNullOrEmpty(comment) == true ? "" : comment) + "','"
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
            if (dataSet.Tables.Count > 0)
            {
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

        #region Client Invocie

        public DataTable GetDataFromLocationMasterSetting(long? Id)
        {
            string QueryString = "EXEC USP_GetDataForInvoiceFromLocationMasterSetting '" + Id + "'";
            return DB.GetDTResponse(QueryString);
        }
        public DataTable GetItemMasterDetails(long? ItemId)
        {
            string QueryString = "EXEC USP_GetItemMasterDetails '" + ItemId + "'";
            return DB.GetDTResponse(QueryString);
            //List<Tbl_Item_Master> ItemList = DataRowToObject.CreateListFromTable<Tbl_Item_Master>(dataTable);
            //return ItemList;
        }

        public DataTable InsertUpdateInvoiceBill(string Action, string xmlInvoiceDetails, string xmlItemDetails
            , long ModifiedBy, long ApprovedBy, string IsDraft)
        {
            string QueryString = "";
            QueryString = "EXEC USP_InsertUpdate_InvoiceBill '"
                            + Action.Trim() + "','"
                            + xmlInvoiceDetails.Trim() + "','"
                            + xmlItemDetails.Trim() + "','"
                            + ModifiedBy + "','"
                            + ApprovedBy + "','"
                            + IsDraft + "'";

            return DB.GetDTResponse(QueryString);
        }

        public List<ClientInvoiceDataModel> Get_InvoiceList(long? locationId = 0, string search = null)
        {
            string QueryString = "EXEC USP_Get_InvoiceList '" + locationId + "','" + search + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ClientInvoiceDataModel> ItemList = DataRowToObject.CreateListFromTable<ClientInvoiceDataModel>(dataTable);
            return ItemList;
        }

        public DataTable SubmissionClientInvoice(long InvoiceId, string comment, long approvedBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_ClientInvoice_Submission '"
                            + InvoiceId + "','"
                            + (string.IsNullOrEmpty(comment) == true ? "" : comment) + "','"
                            + approvedBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public DataSet GetSubmissionMailDetails(long InvoiceId)
        {
            string QueryString = "EXEC USP_Get_InvoiceDetail_ForRecievePayment '" + InvoiceId + "'";
            return DB.GetDSResponse(QueryString);
        }

        public DataTable ApproveDenyClientInvoice(long InvoiceId, string comment, string isApprove, long approvedBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_ApproveDenyClientInvoice '"
                            + InvoiceId + "','"
                            + (string.IsNullOrEmpty(comment) == true ? "" : comment) + "','"
                            + isApprove.Trim() + "','"
                            + approvedBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public DataTable CancelClientInvoice(long InvoiceId, string comment, long cancelledBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_Cancel_ClientInvoice '"
                            + InvoiceId + "','"
                            + (string.IsNullOrEmpty(comment) == true ? "" : comment) + "','"
                            + cancelledBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public List<ClientInvoiceDataModel> GetInvoiceDetailForEditView(long? InvoiceId)
        {
            string QueryString = "EXEC USP_Get_InvoiceDetailsForEditView '" + InvoiceId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ClientInvoiceDataModel> ItemList = DataRowToObject.CreateListFromTable<ClientInvoiceDataModel>(dataTable);
            return ItemList;
        }

        public List<InvoiceItemDetails> GetInvoiceItemDetailsForEditView(long? InvoiceId)
        {
            string QueryString = "EXEC USP_GetInvoiceItemDetailsForEditView '" + InvoiceId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<InvoiceItemDetails> ItemList = DataRowToObject.CreateListFromTable<InvoiceItemDetails>(dataTable);
            return ItemList;
        }

        public ClientInvoiceDataModel Get_InvoiceDetail_ForRecievePayment(long? InvoiceId)
        {
            var ItemList = new ClientInvoiceDataModel();
            string QueryString = "EXEC USP_Get_InvoiceDetail_ForRecievePayment '" + InvoiceId + "'";
            DataSet dataSet = DB.GetDSResponse(QueryString);
            if (dataSet.Tables.Count > 0)
            {
                ItemList = DataRowToObject.CreateListFromTable<ClientInvoiceDataModel>(dataSet.Tables[0]).FirstOrDefault();
                ItemList.ListInvoiceItemDetails = DataRowToObject.CreateListFromTable<InvoiceItemDetails>(dataSet.Tables[1]);
            }
            return ItemList;
        }

        public DataTable RecievePaymentInvoiceBill(string xmlInvoiceDetails, long ModifiedBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_ReceivePayment_InvoiceBill '"
                            + xmlInvoiceDetails.Trim() + "','"
                            + ModifiedBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public ClientInvoiceDataModel GetInvoiceData(string ID, string IsDraft)
        {
            string QueryString = "EXEC USP_Get_InvoiceDetailsForEditView '" + ID + "', '" + IsDraft + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<ClientInvoiceDataModel>(dataTable).FirstOrDefault();
        }

        public List<InvoiceItemDetails> GetInvoiceItemsList(string ID, string IsDraft)
        {
            string QueryString = "EXEC USP_GetInvoiceItemDetailsForEditView '" + ID + "', '" + IsDraft + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<InvoiceItemDetails>(dataTable);
        }

        public DataSet GetInvoiceDataforView(long ID)
        {
            string QueryString = "EXEC USP_Get_InvoiceDetail_ForRecievePayment '" + ID + "'";
            return DB.GetDSResponse(QueryString);
        }

        public DataSet GetDraftDataforView(long ID)
        {
            string QueryString = "EXEC USP_Get_DraftDetailForView '" + ID + "'";
            return DB.GetDSResponse(QueryString);
        }
        public List<InvoiceCountForGraph> GetInvoiceCountByStatus(long? LocationId)
        {
            string QueryString = "EXEC USP_GetInvoiceCountByStatusForGraph '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<InvoiceCountForGraph>(dataTable);
        }
        public List<CreditInvoiceCountForGraph> GetCreditInvoiceCountByStatus(long? LocationId)
        {
            string QueryString = "EXEC USP_GetCreditInvoiceCountByStatusForGraph '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<CreditInvoiceCountForGraph>(dataTable);
        }
        public CreditMemoDataModel GetInvoiceDataForCreditMemo(string ID)
        {
            string QueryString = "EXEC USP_GetInvoiceDataForCreditMemo '" + ID + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<CreditMemoDataModel>(dataTable).FirstOrDefault();
        }

        public List<CreditMemoItemDetails> GetInvoiceItemsListForCreditMemo(string ID)
        {
            string QueryString = "EXEC USP_GetInvoiceItemsListForCreditMemo '" + ID + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<CreditMemoItemDetails>(dataTable);
        }

        public DataTable InsertUpdateCreditMemo(string Action, string xmlInvoiceDetails, string xmlItemDetails
            , long ModifiedBy, long ApprovedBy, string IsDraft)
        {
            string QueryString = "";
            QueryString = "EXEC USP_InsertUpdate_CreditMemo '"
                            + Action.Trim() + "','"
                            + xmlInvoiceDetails.Trim() + "','"
                            + xmlItemDetails.Trim() + "','"
                            + ModifiedBy + "','"
                            + ApprovedBy + "','"
                            + IsDraft + "'";

            return DB.GetDTResponse(QueryString);
        }

        public List<CreditMemoDataModel> Get_CreditInvoiceList(long? locationId = 0, string search = null)
        {
            string QueryString = "EXEC USP_Get_CreditInvoiceList '" + locationId + "','" + search + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<CreditMemoDataModel> ItemList = DataRowToObject.CreateListFromTable<CreditMemoDataModel>(dataTable);
            return ItemList;
        }

        public DataSet GetDraftCreditInvoiceDataforView(long ID)
        {
            string QueryString = "EXEC USP_Get_DraftCreditMemoDetailForView '" + ID + "'";
            return DB.GetDSResponse(QueryString);
        }
        public DataSet GetCreditInvoiceDataforView(long ID)
        {
            string QueryString = "EXEC USP_Get_CreditMemoDetailForView '" + ID + "'";
            return DB.GetDSResponse(QueryString);
        }

        public DataTable ApproveRejectCreditMemo(long Id, string comment, string isApprove, long approvedBy)
        {
            string QueryString = "";
            QueryString = "EXEC USP_ApproveRejectCreditMemo '"
                            + Id + "','"
                            + (string.IsNullOrEmpty(comment) == true ? "" : comment) + "','"
                            + isApprove.Trim() + "','"
                            + approvedBy + "'";

            return DB.GetDTResponse(QueryString);
        }

        public CreditMemoDataModel GetCreditMemoDetailsForEdit(string ID, string IsDraft)
        {
            string QueryString = "EXEC USP_GetCreditMemoDetailsForEdit '" + ID + "', '" + IsDraft + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<CreditMemoDataModel>(dataTable).FirstOrDefault();
        }

        public List<CreditMemoItemDetails> GetCreditMemoItemDetailsForEdit(string ID, string IsDraft)
        {
            string QueryString = "EXEC USP_GetCreditMemoItemDetailsForEdit '" + ID + "', '" + IsDraft + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            return DataRowToObject.CreateListFromTable<CreditMemoItemDetails>(dataTable);
        }

        public DataTable ClientInvoice_AccountTransaction(string CATAction, Nullable<long> CAT_CMP_IdDr, Nullable<long> CAT_CMP_IdCr
            , Nullable<long> CAT_CAD_IdDr, Nullable<long> CAT_CAD_IdCr, Nullable<long> CAT_ClientInvoice_Id, Nullable<decimal> CAT_Amount
            , Nullable<int> CAT_ChequeNo, string CAT_Discription, Nullable<long> CAT_PayBy, Nullable<long> BLL_LocationId,string CAT_PaymentMode
            , string CAT_BillType, string CAT_IsActive)
        {
            string QueryString = "";
            QueryString = "EXEC USP_Set_ClientInvoice_AccountTransaction '"
                                + CATAction + "',"
                                + (CAT_CMP_IdDr == null ? "null" : CAT_CMP_IdDr.ToString()) + ","
                                + (CAT_CMP_IdCr == null ? "null" : CAT_CMP_IdCr.ToString()) + ","
                                + (CAT_CAD_IdDr == null ? "null" : CAT_CAD_IdDr.ToString()) + ","
                                + (CAT_CAD_IdCr == null ? "null" : CAT_CAD_IdCr.ToString()) + ","
                                + CAT_ClientInvoice_Id + ","
                                + CAT_Amount + ","
                                + (CAT_ChequeNo == null ? "null" : CAT_ChequeNo.ToString()) + ",'"
                                + CAT_PaymentMode + "','"
                                + CAT_Discription + "','"
                                + CAT_PayBy + "','"
                                + BLL_LocationId + "','"
                                + CAT_BillType + "','"
                                + CAT_IsActive + "'";

            return DB.GetDTResponse(QueryString);
        }

        #endregion

        #region Item Master
        public List<ItemServiceModel> Get_ItemList(long? ItemCode = 0, string search = null)
        {
            string QueryString = "EXEC USP_Get_ItemList '" + ItemCode + "','" + search + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ItemServiceModel> ItemList = DataRowToObject.CreateListFromTable<ItemServiceModel>(dataTable);
            return ItemList;
        }
        #endregion

        #region EmployeeList For Schedule
        
        public List<ApplicantSchecduleAvaliblity> GetScheduleEmployeeList(long Location,string EmployeeId)
        {
            string SQRY = "EXEC USP_GetScheduleEmployeeList N'" + Location + "','"+ EmployeeId + "'";
            DataTable DT = DB.GetDTResponse(SQRY);
            List<ApplicantSchecduleAvaliblity> ItemList = DataRowToObject.CreateListFromTable<ApplicantSchecduleAvaliblity>(DT);
            return ItemList;
        }

        #endregion
    }
}
