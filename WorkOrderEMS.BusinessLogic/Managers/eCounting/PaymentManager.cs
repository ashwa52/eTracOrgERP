using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interface;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class PaymentManager : IPaymentManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();

        /// <summary>
        /// Created By :  Ashwajit Bansod
        /// Created Date : 25-OCT-2018
        /// Created For : To get Payment bill list by location
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LocationID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public List<PaymentModel> GetListPaymentByLocationId(long? UserId, long? LocationID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType,string BillTypeId)
        {
            try
            {
                var resultList = new List<PaymentModel>();

                //var objPaymentDetails = new PaymentModelDetails();
                //var obj = new PaymentModel();
                //int pageindex = Convert.ToInt32(pageIndex) - 1;
                //int pageSize = Convert.ToInt32(numberOfRows);
                  resultList = _workorderems.spPaymentDesk(LocationID, BillTypeId)
                    .Select(a => new PaymentModel()
                    {
                     LLBL_ID = a.LBLL_Id,
                     BillNo = a.LBLL_BLL_Id,
                     BillAmount = a.LBLL_InvoiceAmount,
                     BillDate = a.BillDate,
                     BillType = a.LBLL_BillType,
                     GracePeriod = a.CNT_GracePeriod,
                     PaymentMode = a.PMD_PaymentMode,
                     LocationName = a.LocationName,                     
                     VendorName = a.CMP_NameLegalBeneficiary,
                     Description = a.CAT_Discription,
                     Status = a.Bill_Status,
                     VendorId = a.CMP_IdBeneficiary,
                     ChequeNo = a.CAT_ChequeNo,
                     OperatingCompany = a.CMP_NameLegalRemitter == null?"N/A": a.CMP_NameLegalRemitter,
                     OperatingCompanyId = a.CMP_IdRemitter > 0 ? a.CMP_IdRemitter :0 ,
                     LocationId = a.LocationId,
                     DisplayDate = a.BillDate == null ? "" : a.BillDate.ToString("MMM dd,yyyy"),
                     PaymentNote = a.LBLL_Comment
                    }).Where(x => x.Status == "W" || x.Status == "Y").ToList();
               // Results.Where(x => x.Status == "Y").ToList();
                //foreach (var item in Results)
                //{
                //    if(item.Status == "Y" && item.BillType=="ManualBill")
                //    {
                //     obj.LLBL_ID = item.
                //      obj.BillNo= a.LBLL_BLL_Id,
                //      obj.= item.LBLL_InvoiceAmount,
                //      obj.= item.BillDate,
                //      obj.= item.LBLL_BillType,
                //      obj.= item.CNT_GracePeriod,
                //      obj.= item.PMD_PaymentMode,
                //      obj.= item.LocationName,
                //      obj.= item.CMP_NameLegalBeneficiary,
                //      obj.= item.CAT_Discription,
                //      obj.= item.Bill_Status,
                //      obj.= item.CMP_IdBeneficiary,
                //      obj.= item.CMP_NameLegalRemitter == null ? "N/A" : a.CMP_NameLegalRemitter,
                //      obj.= item.CMP_IdRemitter > 0 ? 0 : a.CMP_IdRemitter,
                //      obj.= item.LocationId
                //        Results.Add()
                //    }
                //}
                //Results.Where(x => x.BillType == "ManualBill" && x.Status == "Y").ToList();
                //Results.Where(x => x. != "X").ToList();
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                //objPaymentDetails.pageindex = pageindex;
                //objPaymentDetails.total = totalPages;
                //objPaymentDetails.records = totRecords;
                //objPaymentDetails.rows = Results.ToList();
                return resultList;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public GetListPaymentByLocationId GetListPaymentByLocationId(long? UserId,long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Miscellaneous details.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 25-OCT-2018
        /// Created For :  To get Paid bill list by location Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LocationID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public List<PaymentModel> GetListPaidtByLocationId(long? UserId, long? LocationID, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType,string BillTypeId)
        {
            try
            {
                //var objPaymentDetails = new PaymentModel();
                //int pageindex = Convert.ToInt32(pageIndex) - 1;
                //int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spPaymentDesk(LocationID, BillTypeId)
                    .Select(a => new PaymentModel()
                    {
                        BillNo = a.LBLL_BLL_Id,
                        BillAmount = a.LBLL_InvoiceAmount,
                        BillDate = a.BillDate,
                        BillType = a.LBLL_BillType,
                        GracePeriod = a.CNT_GracePeriod,
                        PaymentMode = a.PMD_PaymentMode,
                        LocationName = a.LocationName,
                        VendorName = a.CMP_NameLegalBeneficiary,
                        Description = a.CAT_Discription,
                        Status = a.Bill_Status,
                        OperatingCompany = a.CMP_NameLegalRemitter == null ? "N/A" : a.CMP_NameLegalRemitter,
                        OperatingCompanyId = a.CMP_IdRemitter > 0 ? 0 : a.CMP_IdRemitter,
                        LocationId = a.LocationId,
                        DisplayDate = a.BillDate == null ? "" : a.BillDate.ToString("MMM dd,yyyy"),
                        PaymentNote = a.LBLL_Comment,
                        ActionDoneOn = a.LBLL_ApprovedOn.ToString("MMM dd,yyyy"),
                        ActionDoneBy = a.ApprovedBy,
                        //AccountNo = a.acc
                        //ChequeNo
                        //CARDNo
                    }).Where(x => x.Status == "P" || x.Status == "X").ToList();


                //foreach (var item in Results)
                //{
                //    item.Status = item.Status == "P" ? "Pending" : "Cancelled";
                //}
                 //Results.Where(x => x.Status == "X").ToList();
                //int totRecords = Results.Count();
                //var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                //objPaymentDetails.pageindex = pageindex;
                //objPaymentDetails.total = totalPages;
               // objPaymentDetails.records = totRecords;
                //objPaymentDetails.rows = Results.ToList();
                return Results;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public MiscellaneousListDetails GetListMiscellaneous(long? UserId,long? Location, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Miscellaneous details.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created For : To get Account Details
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        public List<PaymentModel> GetAccountDetails(long VendorId, long OperatingCompanyId)
        {
            try
            {
                var result = new List<PaymentModel>();
                long OperatorCADId = 0;
                if (VendorId > 0)
                {
                    var data = _workorderems.spGetCompanyAccountDetailForPayment(OperatingCompanyId).FirstOrDefault();
                    if(data != null)
                    {
                        OperatorCADId = data.CAD_Id;
                    }
                    
                     result = _workorderems.spGetCompanyAccountDetailForPayment(OperatingCompanyId).
                        Select(x => new PaymentModel()
                        {
                            AccountNo = x.CAD_AccountNumber ?? "",
                            CARDNo = x.CAD_CreditCardNumber ?? "",
                            VendorId = x.CAD_CMP_Id,
                            CompanyAccountId = x.CAD_Id,
                            PaymentMode = x.CAD_PMD_Id.ToString(),
                            //VendorName = _workorderems.Companies.Where(a => a.CMP_Id == VendorId).FirstOrDefault().CMP_NameLegal,
                            OpeartorCAD_Id = OperatorCADId
                        }).ToList();
                    foreach (var item in result)
                    {
                        item.VendorName = _workorderems.Companies.Where(a => a.CMP_Id == VendorId).FirstOrDefault().CMP_NameLegal;
                    }
                }                
                else
                {
                    return null;
                }
                return result;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PaymentModel> GetAccountDetails(long VendorId)", "Exception While Getting List of Account details.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-OCT-2018
        /// Created for : To save Payment data to database.
        /// </summary>
        /// <param name="objPaymentModel"></param>
        /// <param name="ObjData"></param>
        /// <returns></returns>
        public string MakePayment(PaymentModel objPaymentModel, PaymentModel ObjData)
        {
            string result = "";
            string Action = "";
            string Status = "Y";
            string PaidStatus = "";
            var objDAR = new DARModel();
            var CommonManager = new CommonMethodManager();
            try
            {
                if (objPaymentModel != null && ObjData != null)
                {

                    if (objPaymentModel.PaymentMode == "Cash") {
                    
                    }
                    else if (objPaymentModel.PaymentMode == "Wired")
                    {

                    }else if (objPaymentModel.PaymentMode == "Card")
                    {

                    }else if (objPaymentModel.PaymentMode == "Cheque")
                    {

                    }

                    var ChequeNo = Convert.ToInt32(objPaymentModel.ChequeNo);
                    var PaymentMode = Convert.ToInt32(objPaymentModel.PaymentMode);
                    PaidStatus = "Paid";
                    if (objPaymentModel.ChequeNo != null)
                    {
                        objPaymentModel.Comment = ChequeNo.ToString();
                    }
                    else if(objPaymentModel.AccNo != null && objPaymentModel.IsCancel == false)
                    {
                        ObjData.VendorId = 0;
                        objPaymentModel.Comment = objPaymentModel.AccNo;
                    }
                    else if(objPaymentModel.CARDNo != null && objPaymentModel.IsCancel == false)
                    {
                        objPaymentModel.Comment = objPaymentModel.CARDNo;
                    }
                    else if(objPaymentModel.AccountNo != null && objPaymentModel.IsCancel == false)
                    {
                        objPaymentModel.Comment = objPaymentModel.AccountNo;
                    }
                    else if(objPaymentModel.Comment != null && objPaymentModel.IsCancel == true)
                    {
                        objPaymentModel.Comment = objPaymentModel.Comment;
                        Status = "X";
                        PaidStatus = "Canceled";
                    }
                    Action = "I";
                    if (objPaymentModel.IsCancel == false)
                    {
                        if (objPaymentModel.PaymentByCash == "wired") 
                        {
                            var savePayment = _workorderems.spSetCompanyAccountTransaction(Action, null, ObjData.VendorId, ObjData.OperatingCompanyId, objPaymentModel.CompanyAccountId,
                                                                                        ObjData.BillNo, ObjData.BillAmount, ChequeNo, PaymentMode,
                                                                                        objPaymentModel.Comment, objPaymentModel.UserId, ObjData.LocationId,
                                                                                        ObjData.BillType, Status);

                        }
                        if (objPaymentModel.PaymentByCash == "Card")
                        {
                            var savePayment = _workorderems.spSetCompanyAccountTransaction(Action, null, ObjData.VendorId, ObjData.OperatingCompanyId, objPaymentModel.CompanyAccountId,
                                                                                        ObjData.BillNo, ObjData.BillAmount, ChequeNo, PaymentMode,
                                                                                        objPaymentModel.Comment, objPaymentModel.UserId, ObjData.LocationId,
                                                                                        ObjData.BillType, Status);

                        }
                        if (objPaymentModel.PaymentByCash == "Cash")
                        {
                            var savePayment = _workorderems.spSetCompanyAccountTransaction(Action, null, ObjData.VendorId, ObjData.OperatingCompanyId, objPaymentModel.CompanyAccountId,
                                                                                     ObjData.BillNo, ObjData.BillAmount, ChequeNo, PaymentMode,
                                                                                     objPaymentModel.Comment, objPaymentModel.UserId, ObjData.LocationId,
                                                                                     ObjData.BillType, Status);
                        }
                        if (objPaymentModel.PaymentByCash == "Cheque")
                        {
                            var savePayment = _workorderems.spSetCompanyAccountTransaction(Action, null, ObjData.VendorId, ObjData.OperatingCompanyId, objPaymentModel.CompanyAccountId,
                                                                                     ObjData.BillNo, ObjData.BillAmount, ChequeNo, PaymentMode,
                                                                                     objPaymentModel.Comment, objPaymentModel.UserId, ObjData.LocationId,
                                                                                     ObjData.BillType, Status);
                        }                        
                    }
                    if(Status == "Y")
                    {
                        Status = "P";
                    }
                    var changeStatus = _workorderems.spSetPaymentStatusForBill(ObjData.LLBL_ID, objPaymentModel.Comment, Status,
                                                                              objPaymentModel.UserId);
                    result = CommonMessage.PaymentSave();
                }
                else
                {
                    result = CommonMessage.PaymentError();
                }
                #region Save DAR
                var userData = _workorderems.UserRegistrations.Where(x => x.UserId == objPaymentModel.UserId &&
                                                                         x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                var locationData = _workorderems.LocationMasters.Where(x => x.LocationId == ObjData.LocationId
                                                                        && x.IsDeleted == false).FirstOrDefault();
                objDAR.ActivityDetails = DarMessage.PaymentPaidCancel(userData.FirstName + "" + userData.LastName, locationData.LocationName, PaidStatus, ObjData.BillNo);
                long userId = Convert.ToInt64(objPaymentModel.UserId);
                objDAR.TaskType = (long)TaskTypeCategory.PaymentApporveCancel;
                objDAR.UserId = userId;
                objDAR.CreatedBy = userId;
                objDAR.LocationId = Convert.ToInt64(objPaymentModel.LocationId);
                objDAR.CreatedOn = DateTime.UtcNow;
                CommonManager.SaveDAR(objDAR);
                #endregion DAR
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public string MakePayment(PaymentModel objPaymentModel)", "Exception While Saving Payment.", objPaymentModel);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-April-2019
        /// Created For : To get the details of PO by PO Id.
        /// </summary>
        /// <param name="objPaymentModel"></param>
        /// <param name="ObjData"></param>
        /// <returns></returns>
        public POApproveRejectModel GetPODetails(PaymentModel objPaymentModel, PaymentModel ObjData)
        {
            var obj = new POApproveRejectModel();
            try
            {
                if(ObjData != null)
                {
                    var getBill = _workorderems.LogBills.Where(x => x.LBLL_BLL_Id == ObjData.BillNo).FirstOrDefault();
                    if (getBill.LBLL_POD_Id > 0)
                    {
                        obj = _workorderems.PODetails.Where(x => x.POD_Id == getBill.LBLL_POD_Id ).
                        Select(a => new POApproveRejectModel()
                        {
                            POId = a.POD_Id,
                            QuickBookPOId = a.POD_QBKId
                        }).FirstOrDefault();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public POApproveRejectModel GetPODetails(PaymentModel objPaymentModel, PaymentModel ObjData)", "Exception While geting PO details.", objPaymentModel);
                throw;
            }
            return obj;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-July-2019
        /// Created For : To get miscellaneous number
        /// </summary>
        /// <param name="MiscId"></param>
        /// <returns></returns>
        public long? GetMiscellaneousNumber(long MiscId)
        {
            long? MiscellaneousNumber = 0;
            try
            {
                if (MiscId > 0)
                {
                    var data = _workorderems.Bills.Where(x => x.BLL_Id == MiscId).FirstOrDefault();
                    if (data.BLL_MIS_Id != null)
                    {
                        MiscellaneousNumber = data.BLL_MIS_Id;
                    }
                    else
                    {
                        MiscellaneousNumber = 0;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return MiscellaneousNumber;
        }
    }
}
