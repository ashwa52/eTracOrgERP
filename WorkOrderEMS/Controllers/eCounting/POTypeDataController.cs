using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Controllers.QuickBookData;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class POTypeDataController : Controller
    {
        private readonly IPOTypeDetails _IPOTypeDetails;
        private readonly IBillDataManager _IBillDataManager;
        private readonly IVendorManagement _IVendorManagement;
        public POTypeDataController(IPOTypeDetails _IPOTypeDetails, IBillDataManager _IBillDataManager, IVendorManagement _IVendorManagement)
        {
            this._IPOTypeDetails = _IPOTypeDetails;
            this._IBillDataManager = _IBillDataManager;
            this._IVendorManagement = _IVendorManagement;
        }
        // GET: POTypeData
        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();
        public ActionResult Index()
        {
            var objPOTypeModel = new POTypeDataModel();
            try
            {                
                // ViewBag.VendorList = _IPOTypeDetails.GetCompany_VendorList();
                ViewBag.PONumber = _IPOTypeDetails.PONumberData();
                ViewBag.POType = _IPOTypeDetails.POTypeList();
            }
            catch (Exception ex)
            {                
            }
            return View(objPOTypeModel);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Sept-2018
        /// Created for : To get All Company List
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public JsonResult GetVendorList(long Location, bool IsRecurring)
        {
            try
            {
                var result = _IPOTypeDetails.GetCompany_VendorList(Location, IsRecurring);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Sept-2018
        /// Created For : To get All company details as per company id
        /// </summary>
        /// <param name="VendorId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCompanyDetailsList(long VendorId)
        {
            try
            {
                long Vendor = 0;
                //if (!string.IsNullOrEmpty(VendorId))
                //{
                //    VendorId = Cryptography.GetDecryptedData(VendorId, true);
                //    long.TryParse(VendorId, out Vendor);
                //}
                var result = _IPOTypeDetails.GetCompanyDetailsListByCompanyId(VendorId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : Sept-17-2018
        /// Created For : To get List of Company facility
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComapnyFacilityList(string _search, long? UserId, long? VendorId, long? Location, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (Location == null)
                {
                    Location = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var companyFacilityList = _IPOTypeDetails.GetPOTypeDetailsOfCompanyFacilityList(UserId, Location, VendorId, rows, TotalRecords, sidx, sord);
                return Json(companyFacilityList, JsonRequestBehavior.AllowGet);
                //foreach (var compFacility in companyFacilityList.rows)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(Convert.ToString(compFacility.COM_FacilityId), true);
                //    row.cell = new string[12];
                //    row.cell[0] = compFacility.COM_Facility_Desc;
                //    row.cell[1] = compFacility.Quantity == 0?"": compFacility.Quantity.ToString();
                //    row.cell[2] = compFacility.UnitPrice.ToString();
                //    row.cell[3] = compFacility.TotalPrice.ToString();
                //    row.cell[4] = compFacility.Tax.ToString();
                //   // row.cell[5] = compFacility.Resourse.ToList().ToString();
                //    row.cell[5] = compFacility.Status;
                //    row.cell[6] = compFacility.CostCode.ToString();
                //    row.cell[7] = compFacility.CFM_CMP_Id.ToString();
                //    row.cell[8] = compFacility.COM_FacilityId.ToString();
                //    row.cell[9] = compFacility.RemainingAmt.ToString();
                //    row.cell[10] = compFacility.LastRemainingAmount.ToString();
                //    row.cell[11] = compFacility.StatusCalculation.ToString();
                //    rowss.Add(row);
                //}
                //result.rows = rowss.ToArray();
                //result.page = Convert.ToInt32(page);
                //result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                //result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 29-Sept-2018
        /// Created For : To save PO details 
        /// </summary>
        /// <param name="objPOTypeDataModel"></param>
        /// <param name="obj"></param>
        /// <param name="objQuestioAsnwerList"></param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult SavePOTypeDetails(POTypeDataModel objPOTypeDataModel, List<GridDataPO> obj,List<QuestionAnswerModel> objQuestioAsnwerList)//, HttpPostedFileBase file)//(POTypeDataModel objPOTypeDataModel)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    HttpFileCollectionBase files = Request.Files;
        //    bool savedStatus = false;
        //    if (Session["eTrac"] != null)
        //    { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
        //    try
        //    {
        //        objPOTypeDataModel.UserId = ObjLoginModel.UserId;
        //        if (objPOTypeDataModel != null && objPOTypeDataModel.POId == 0)
        //        {
        //            if (objPOTypeDataModel.POD_EmergencyPODocumentFile != null)
        //            {
        //                string AttachmentName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objPOTypeDataModel.POD_EmergencyPODocumentFile.FileName);
        //                CommonHelper.StaticUploadImage(objPOTypeDataModel.POD_EmergencyPODocumentFile, Server.MapPath(ConfigurationManager.AppSettings["EmergencyDocuments"]), AttachmentName);
        //                objPOTypeDataModel.POD_EmergencyPODocument = AttachmentName;
        //            }
        //            bool IsManager = true;
        //            savedStatus = _IPOTypeDetails.SavePODetails(objPOTypeDataModel, obj, objQuestioAsnwerList, IsManager);
        //            if (savedStatus == true)
        //            {
        //                ViewBag.Message = CommonMessage.SaveSuccessMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;                        
        //            }
        //            else
        //            {
        //                ViewBag.Message = CommonMessage.FailureMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //            }
        //        }
        //        else
        //        {
        //            bool IsManager = true;
        //            savedStatus = _IPOTypeDetails.SavePODetails(objPOTypeDataModel, obj, objQuestioAsnwerList, IsManager);
        //            if (savedStatus == true)
        //            {
        //                ViewBag.Message = CommonMessage.UpdateSuccessMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
        //            }
        //            else
        //            {
        //                ViewBag.Message = CommonMessage.FailureMessage();
        //                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
        //            }
        //        }
        //        var objModel = new POTypeDataModel();
        //        return Json(savedStatus, JsonRequestBehavior.AllowGet); ;
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = ex;
        //        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ViewBag.PONumber = _IPOTypeDetails.PONumberData();
        //        ViewBag.POType = _IPOTypeDetails.POTypeList(); 
        //    }
        //}

        [HttpPost]
        public JsonResult SavePOTypeDetails(POTypeDataModel objPOTypeDataModel, List<GridDataPO> obj, List<QuestionAnswerModel> objQuestioAsnwerList)//, HttpPostedFileBase file)//(POTypeDataModel objPOTypeDataModel)
        {
            eTracLoginModel ObjLoginModel = null;
            HttpFileCollectionBase files = Request.Files;
            bool savedStatus = false;
            var resultSave = new PurchaseOrder();
            if (Session["eTrac"] != null)
            { ObjLoginModel = (eTracLoginModel)(Session["eTrac"]); }
            try
            {
                objPOTypeDataModel.UserId = ObjLoginModel.UserId;
                if (objPOTypeDataModel != null && objPOTypeDataModel.POId == 0)
                {
                    if (objPOTypeDataModel.POD_EmergencyPODocumentFile != null)
                    {
                        string AttachmentName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + Convert.ToString(objPOTypeDataModel.POD_EmergencyPODocumentFile.FileName);
                        CommonHelper.StaticUploadImage(objPOTypeDataModel.POD_EmergencyPODocumentFile, Server.MapPath(ConfigurationManager.AppSettings["EmergencyDocuments"]), AttachmentName);
                        objPOTypeDataModel.POD_EmergencyPODocument = AttachmentName;
                    }
                    bool IsManager = true;


                    string realmId = CallbackController.RealMId.ToString();//Session["realmId"].ToString();
                    if (realmId != null)
                        {
                            try
                            {
                                string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                                string refreshToken = CallbackController.RefreshToken;
                                var principal = User as ClaimsPrincipal;
                                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);
                                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                                DataService commonServiceQBO = new DataService(serviceContext);
                                // Create a QuickBooks QueryService using ServiceContext
                                Account account = new Account();

                                QueryService<Account> querySvcCompany = new QueryService<Account>(serviceContext);
                                List<Account> listAccount = querySvcCompany.ExecuteIdsQuery("SELECT * FROM Account MaxResults 1000")
                                    .ToList();

                                QueryService<Vendor> querySvc = new QueryService<Vendor>(serviceContext);
                                List<Vendor> vendorList = querySvc.ExecuteIdsQuery("SELECT * FROM Vendor MaxResults 1000").ToList();

                                //var Vendor = _IVendorManagement.GetVendorId(objListData.CompanyName);
                                var VendorDetails = _IVendorManagement.GetCompanyQuickBookId(Convert.ToInt64(objPOTypeDataModel.Vendor));
                                ReferenceType parentReference = new ReferenceType();
                                var purchaseOrder = new PurchaseOrder();
                                //var lineDetailType = new LineDetailTypeEnum();
                                if (VendorDetails > 0)
                                {
                                    var vendorData = vendorList.Where(x => x.Id == VendorDetails.ToString()).FirstOrDefault();
                                    //Vendor Reference                                   
                                    purchaseOrder.VendorRef = new ReferenceType()
                                    {
                                        name = vendorData.DisplayName,
                                        Value = vendorData.Id
                                    };
                                }
                                var LocationName = _IBillDataManager.GetLocationDataByLocId(Convert.ToInt64(objPOTypeDataModel.Location));
                                purchaseOrder.DepartmentRef = new ReferenceType()
                                {
                                    name = LocationName.LocationName,
                                    Value = LocationName.QBK_Id.ToString()
                                };
                                purchaseOrder.POStatus = PurchaseOrderStatusEnum.Open;
                                //Line line = new Line();
                                List<Line> lineList = new List<Line>();
                                var accountRef = new AccountBasedExpenseLineDetail();
                                purchaseOrder.APAccountRef = new ReferenceType()
                                {
                                    name = "Accounts Payable (A/P)",
                                    Value = "33"
                                };

                                foreach (var item in obj)
                                {
                                    var line = new Line();
                                    line.DetailType = LineDetailTypeEnum.AccountBasedExpenseLineDetail;
                                    long CostCodeId = Convert.ToInt64(item.CostCode);
                                    var costCodeName = _IBillDataManager.GetCostCodeData(CostCodeId);
                                    var dataget = listAccount.Where(x => x.Name == costCodeName.Description).FirstOrDefault();
                                if (dataget !=null) {
                                    accountRef.AccountRef = new ReferenceType()
                                    {
                                        name = dataget.Name,
                                        Value = dataget.Id
                                    };
                                }
                                    line.AnyIntuitObject = accountRef;
                                    line.DetailTypeSpecified = true;
                                    line.Amount = Convert.ToDecimal(item.Quantity) * Convert.ToDecimal(item.UnitPrice);
                                    line.AmountSpecified = true;
                                    line.Description = item.COM_Facility_Desc;
                                    lineList.Add(line);
                                }
                                purchaseOrder.Line = lineList.ToArray();
                                resultSave = commonServiceQBO.Add(purchaseOrder) as PurchaseOrder;
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Message = ex.Message;
                                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                            }
                        }                    
                    if (resultSave.Id != null)
                    {
                        objPOTypeDataModel.QBK_Id = Convert.ToInt64(resultSave.Id);
                    }
                    else
                    {
                        objPOTypeDataModel.QBK_Id = 0;
                    }
                    savedStatus = _IPOTypeDetails.SavePODetails(objPOTypeDataModel, obj, objQuestioAsnwerList, IsManager);
                    if (savedStatus == true)
                    {
                        ViewBag.Message = CommonMessage.SaveSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return Json(savedStatus, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                        return Json(savedStatus, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    bool IsManager = true;
                    savedStatus = _IPOTypeDetails.SavePODetails(objPOTypeDataModel, obj, objQuestioAsnwerList, IsManager);
                    if (savedStatus == true)
                    {
                        ViewBag.Message = CommonMessage.UpdateSuccessMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Success;
                        return Json(savedStatus, JsonRequestBehavior.AllowGet);
                        //return RedirectToAction("AllPOList", "POTypeData");
                        
                        //return View("AllPOList");
                    }
                    else
                    {
                        ViewBag.Message = CommonMessage.FailureMessage();
                        ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;// store the failure message in tempdata to display in view.
                        //return RedirectToAction("AllPOList", "POTypeData");
                        return Json(savedStatus, JsonRequestBehavior.AllowGet);
                        //return View("AllPOList");
                    }
                }
                //var objModel = new POTypeDataModel();
                //return View("AllPOList");
                //return Json(savedStatus, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
                throw ex;
            }
            finally
            {
                ViewBag.PONumber = _IPOTypeDetails.PONumberData();
                ViewBag.POType =   _IPOTypeDetails.POTypeList();        
            }
            //return View("AllPOList");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created for : To save PO Image to folder
        /// Created Date : To
        /// </summary>
        /// <param name="File"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadedImagePO(HttpPostedFileBase File)
        {
            eTracLoginModel ObjLoginModel = null;

            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (File != null)
                {
                    string ImageName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + File.FileName.ToString();
                    CommonHelper obj_CommonHelper = new CommonHelper();
                    var res = obj_CommonHelper.UploadImage(File, Server.MapPath(ConfigurationManager.AppSettings["EmergencyDocuments"]), ImageName);
                    ViewBag.ImageUrl = res;
                    if (res)
                    {

                        return Json(ImageName);
                    }
                    else { return Json(""); }
                }
                return Json("");
            }
            else
            {
                return Json("");
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created By : 29-Sept-2018
        /// Created For : To Open List Page of PO
        /// </summary>
        /// <returns></returns>
        public ActionResult AllPOList()
        {
            return View();
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 29-Sept-2018
        /// Created For : To Get All PO List Details as per location Id
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllPOList(string _search, long? UserId,  long? LocationId,string status, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
       {
            eTracLoginModel ObjLoginModel = null;
            long UserTypeId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ObjLoginModel != null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
                UserTypeId = ObjLoginModel.UserType;
            }
            //long _lpoid;
            //long.TryParse(txtSearch, out _lpoid);
            //JQGridResults result = new JQGridResults();
            //List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
             
            //txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                if (txtSearch !=null && txtSearch != "")
                {
                    var AllPOList1 = _IPOTypeDetails.GetAllPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord).Where(n => n.DisplayLogPOId == txtSearch).ToList(); 
                    return Json(AllPOList1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var AllPOList = _IPOTypeDetails.GetAllPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord);
                    return Json(AllPOList, JsonRequestBehavior.AllowGet);
                }
                //foreach (var poList in AllPOList.rows)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(Convert.ToString(poList.LogPOId), true);
                //    row.cell = new string[11];
                //    row.cell[0] = "PO" + poList.LogPOId.ToString();
                //    row.cell[1] = poList.POType.ToString();
                //    row.cell[2] = (poList.CompanyName == null) ? "N/A" : poList.CompanyName.ToString();
                //    row.cell[3] = poList.LocationName.ToString();
                //    row.cell[4] = poList.PODate.ToString("MM/dd/yyyy");
                //    row.cell[5] = (poList.DeliveryDate == null)?"N/A": poList.DeliveryDate.ToString("MM/dd/yyyy");
                //    row.cell[6] = (poList.POStatus == null)?"Not Approved": poList.POStatus.ToString();
                //    row.cell[7] = (poList.POStatusToDisplay == "W") ? "Waiting" : (poList.POStatusToDisplay == "Y")? "Approved" : "Reject";
                //    row.cell[8] = poList.LogId.ToString();
                //    row.cell[9] = (poList.Total == null)?"N/A": poList.Total.ToString(); 
                //    row.cell[10] = poList.CreatedBy.ToString();
                //    rowss.Add(row);
                //}
                //result.rows = rowss.ToArray();
                //result.page = Convert.ToInt32(page);
                //result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                //result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-Oct-2018
        /// Created For : To Approve or Reject PO using PO Number
        /// </summary>
        /// <param name="objPOApproveRejectModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApprovePO(POApproveRejectModel objPOApproveRejectModel, POListModel objListData, List<POTypeDataModel> ProductListData)
        {
            var ObjLoginModel = new eTracLoginModel();
            long LocationId= 0;string result="";
            long POId = 0;
            try
            {
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (objPOApproveRejectModel.LocationId == 0)
                    {
                        LocationId = ObjLoginModel.LocationID;
                    }                    
                }
                if (!string.IsNullOrEmpty(objPOApproveRejectModel.POApproveRemoveId))
                {
                    objPOApproveRejectModel.POApproveRemoveId = Cryptography.GetDecryptedData(objPOApproveRejectModel.POApproveRemoveId, true);
                    long.TryParse(objPOApproveRejectModel.POApproveRemoveId, out POId);
                }
                objPOApproveRejectModel.UserId = ObjLoginModel.UserId;
                objPOApproveRejectModel.POModifiedId = POId;
                if (objPOApproveRejectModel.POModifiedId > 0)
                {
                    //var getResponseForQB = _IPOTypeDetails.GetApprovalResponseToSaveQBKId(objPOApproveRejectModel, objListData);                  
                    result = _IPOTypeDetails.ApprovePOByPOId(objPOApproveRejectModel, objListData);
                    if (objPOApproveRejectModel.Comment != null)
                    {
                        string realmId = CallbackController.RealMId.ToString();//Session["realmId"].ToString();
                        if (realmId != null)
                        {
                            try
                            {
                                string AccessToken = CallbackController.AccessToken.ToString();// Session["access_token"].ToString();
                                string refreshToken = CallbackController.RefreshToken;
                                var principal = User as ClaimsPrincipal;
                                OAuth2RequestValidator oauthValidator = new OAuth2RequestValidator(AccessToken);
                                ServiceContext serviceContext = new ServiceContext(realmId, IntuitServicesType.QBO, oauthValidator);
                                serviceContext.IppConfiguration.MinorVersion.Qbo = "23";
                                DataService commonServiceQBO = new DataService(serviceContext);
                                QueryService<PurchaseOrder> querySvcCompany = new QueryService<PurchaseOrder>(serviceContext);
                                List<PurchaseOrder> listPO = querySvcCompany.ExecuteIdsQuery("SELECT * FROM PurchaseOrder MaxResults 1000")
                                    .ToList();
                                var getResponseForQB = _IPOTypeDetails.GetApprovalResponseToSaveQBKId(objPOApproveRejectModel, objListData);
                                if (getResponseForQB != null)
                                {
                                    var dataget = listPO.Where(x => x.Id == getResponseForQB.POD_QBKId.ToString()).FirstOrDefault();
                                    if (dataget != null)
                                    {
                                        dataget.POStatus = PurchaseOrderStatusEnum.Closed;
                                        var update = commonServiceQBO.Update(dataget) as PurchaseOrder;
                                    }
                                }
                                else
                                {
                                    result = CommonMessage.NoRecordMessage();
                                }
                            }
                            catch (Exception ex)
                            {
                                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                            }
                        }
                    } 
                }
                else
                {
                    result = "Not getting PO Number";
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Ashwajit Bansod
        /// Created Date : 03-Oct-2018
        /// Created For : To get Facility Item by PO Id
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="POId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllPOFacilityByPOIdList(string _search, long? UserId, string POId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long LocationId = 0;
            long Id = 0;
            decimal? Calculation = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            if (!string.IsNullOrEmpty(POId))
            {
                POId = Cryptography.GetDecryptedData(POId, true);
                long.TryParse(POId, out Id);
            }
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            decimal? grandTotal = 0;
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                var AllPOFacilityList = _IPOTypeDetails.GetAllPOFacilityByPOIdList(UserId, Id, rows, TotalRecords, sidx, sord);
                foreach (var poFacilityList in AllPOFacilityList.rows)
                {
                    grandTotal += poFacilityList.UnitPrice * poFacilityList.Quantity;
                    poFacilityList.TotalPrice = grandTotal;
                    poFacilityList.TotalPrice = poFacilityList.UnitPrice * poFacilityList.Quantity;
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(poFacilityList.COM_FacilityId), true);
                    row.cell = new string[10];
                    row.cell[0] = poFacilityList.COM_FacilityId.ToString();
                    row.cell[1] = poFacilityList.CostCode.ToString();
                    row.cell[2] = poFacilityList.FacilityType.ToString();
                    row.cell[3] = poFacilityList.COM_Facility_Desc.ToString();
                    row.cell[4] = poFacilityList.UnitPrice.ToString();
                    row.cell[5] = poFacilityList.Tax.ToString();
                    row.cell[6] = poFacilityList.Quantity.ToString();
                    row.cell[7] = poFacilityList.Total.ToString();
                    row.cell[8] = poFacilityList.TotalPrice.ToString(); 
                    row.cell[9] = poFacilityList.CostCodeName.ToString();
                    rowss.Add(row);
                }
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Ashwajit bansod
        /// Created Date : 03-Oct-2018
        /// Created For: to edit PO as per POId
        /// </summary>
        /// <param name="POId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public ActionResult EditPOByPOId(string POId,long LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            long Id = 0;
            var objPOTypeModel = new POTypeDataModel();
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (!string.IsNullOrEmpty(POId))
                {
                    ViewBag.UpdateMode = true;
                    POId = Cryptography.GetDecryptedData(POId, true);
                    long.TryParse(POId, out Id);
                }
                if(Id > 0)
                { 
                    var DataPO = _IPOTypeDetails.GetPODetailsById(Id);
                    ViewBag.Vendor = DataPO.Vendor;
                    ViewBag.POTypeDate = DataPO.POType;
                    ViewBag.PONumberData = DataPO.PONumber;
                    ViewBag.POType = _IPOTypeDetails.POTypeList();
                    ViewBag.VendorList = _IPOTypeDetails.GetCompany_VendorList(LocationId, false);
                    return View("Index", DataPO);
                }
                else
                {
                    ViewBag.AlertMessageClass = new AlertMessageClass().Danger;
                    ViewBag.Message = Result.DoesNotExist;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.AlertMessageClass = ObjAlertMessageClass.Danger;
            }
            return View("Index");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 04-Oct-2018
        /// Created For : Grid for edit, we cannot edit the previous grid so this grid use for edit
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="POId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPOFacilityListForEditByPOId(string _search, long? UserId, string POId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long Location = 0;
            long Id = 0;
            string FacId = "";
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (Location == null)
                {
                    Location = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            if (!string.IsNullOrEmpty(POId))
            {
                ViewBag.UpdateMode = true;
                POId = Cryptography.GetDecryptedData(POId, true);
                long.TryParse(POId, out Id);
            }
            ViewBag.UpdateMode = true;
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);  
             
            try
            {
                var companyFacilityList = _IPOTypeDetails.GetPOFacilityListForEditByPOId(UserId, Id, rows, TotalRecords, sidx, sord);
                return Json(companyFacilityList, JsonRequestBehavior.AllowGet);
                //foreach (var compFacility in companyFacilityList.rows)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(Convert.ToString(compFacility.COM_FacilityId), true);
                //    FacId = Cryptography.GetEncryptedData(Convert.ToString(compFacility.COM_FacilityId), true);
                //    row.cell = new string[13];
                //    row.cell[0] = compFacility.COM_Facility_Desc;
                //    row.cell[1] = compFacility.Quantity.ToString();
                //    row.cell[2] = compFacility.UnitPrice.ToString();
                //    row.cell[3] = compFacility.TotalPrice.ToString();
                //    row.cell[4] = compFacility.Tax.ToString();
                //    // row.cell[5] = compFacility.Resourse.ToList().ToString();
                //    row.cell[5] = compFacility.Status;
                //    row.cell[6] = compFacility.CostCode.ToString();
                //    row.cell[7] = compFacility.CFM_CMP_Id.ToString();
                //    row.cell[8] = compFacility.COM_FacilityId.ToString();
                //    row.cell[9] = compFacility.RemainingAmt.ToString();
                //    row.cell[10] = compFacility.LastRemainingAmount.ToString();
                //    row.cell[11] = compFacility.StatusCalculation.ToString();
                //    row.cell[12] = compFacility.POId.ToString();
                //    rowss.Add(row);
                //}
                //result.rows = rowss.ToArray();
                //result.page = Convert.ToInt32(page);
                //result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                //result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
           
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 02-Nov-2018
        /// Created For : To Send mail when Cost remaining aount over budgeted
        /// </summary>
        /// <param name="CalculateRemainingAmt"></param>
        /// <param name="CostCodeData"></param>
        /// <param name="LocationData"></param>
        /// <param name="Vendordata"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendMailForMoreBudget(decimal CalculateRemainingAmt, long CostCodeData, long LocationData, long Vendordata)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            string result = "";
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationData == null)
                {
                    LocationData = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (CalculateRemainingAmt > 0 && CostCodeData > 0 && LocationData > 0 && Vendordata > 0)
                {
                    result = _IPOTypeDetails.SendMailToManagerForBudget(UserId, CalculateRemainingAmt, CostCodeData, LocationData, Vendordata);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = "Check parameters";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-Mar-2019
        /// Created For : To get all PO list of User by user id.
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="LocationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAllSelfCreatedPOList(string _search, long? UserId, long? LocationId, string status, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserTypeId = 0;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
                }
                UserId = ObjLoginModel.UserId;
                UserTypeId = ObjLoginModel.UserType;
            }
            JQGridResults result = new JQGridResults();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "UserEmail" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            { 
                    if (txtSearch!=null && txtSearch !="")
                    {
                    var AllPOList = _IPOTypeDetails.GetAllSelfPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord).Where(x => x.DisplayLogPOId == txtSearch);
                    return Json(AllPOList, JsonRequestBehavior.AllowGet);
                         
                    }
                    else
                    {
                    var AllPOList1 = _IPOTypeDetails.GetAllSelfPOList(UserId, LocationId, status, UserTypeId, rows, TotalRecords, sidx, sord);
                        return Json(AllPOList1, JsonRequestBehavior.AllowGet);
                    }
                    
                //foreach (var poList in AllPOList.rows)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(Convert.ToString(poList.LogPOId), true);
                //    row.cell = new string[11];
                //    row.cell[0] = "PO" + poList.LogPOId.ToString();
                //    row.cell[1] = poList.POType;
                //    row.cell[2] = (poList.CompanyName == null) ? "N/A" : poList.CompanyName.ToString();
                //    row.cell[3] = poList.LocationName;
                //    row.cell[4] = poList.UserName == null?"N/A": poList.UserName;
                //    row.cell[5] = poList.PODate.ToString("MM/dd/yyyy");
                //    row.cell[6] = (poList.DeliveryDate == null) ? "N/A" : poList.DeliveryDate.ToString("MM/dd/yyyy");
                //    row.cell[7] = (poList.POStatus == null) ? "Not Approved" : poList.POStatus.ToString();
                //    row.cell[8] = (poList.POStatusToDisplay == "W") ? "Waiting" : (poList.POStatusToDisplay == "Y") ? "Approved" : "Reject";
                //    row.cell[9] = poList.LogId.ToString();
                //    row.cell[10] = (poList.Total == null) ? "N/A" : poList.Total.ToString();

                //    rowss.Add(row);
                //}
                //result.rows = rowss.ToArray();
                //result.page = Convert.ToInt32(page);
                //result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                //result.records = Convert.ToInt32(TotalRecords.Value);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
          
        }

    }
}