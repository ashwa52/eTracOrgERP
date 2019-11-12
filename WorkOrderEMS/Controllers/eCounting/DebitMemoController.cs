using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.eCounting.DebitMemo;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class DebitMemoController : Controller
    {
        private readonly IDebitMemo _IDebitMemo;
        private readonly IPOTypeDetails _IPOTypeDetails;

        public DebitMemoController(IDebitMemo _IdebitMemo, IPOTypeDetails _IPOTypeDetails)
        {
            this._IDebitMemo = _IdebitMemo;
            this._IPOTypeDetails = _IPOTypeDetails;
        }

        AlertMessageClass ObjAlertMessageClass = new AlertMessageClass();

        // GET: DebitMemo
        /// <summary>
        /// Created By: Er.Ro.R
        /// Created Date : 8-OCT-2019
        /// Created For : To get Debit Memo List as per Vendor
        /// </summary>
        public ActionResult Index()
        {
            var model = new DebitMemoModel();
            try
            {                
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        if (Convert.ToInt64(Session["eTrac_SelectedDasboardLocationID"]) == 0)//eTrac_UserLocations
                        {
                            (Session["eTrac_SelectedDasboardLocationID"]) = ObjLoginModel.LocationID;
                        }
                    }
                }

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "---Select Vendor---",   
                    Value = "0"
                });
                ViewBag.VendorList = listItems;//sending locationId 0 for all data                
                //ViewBag.VendorList = _IPOTypeDetails.GetCompany_VendorList(0, false);//sending locationId 0 for all data                
                
                model.ActionModeI = "I";
                ViewBag.ProductOrderList = _IPOTypeDetails.GetAllPOList(0, 0, "", 0,0,0, "","");//companyFacilityList  
                
            }
            catch (Exception ex)
            {               
            }
            return View(model);
        }

        public ActionResult GetDebitMemoListByVendorId(string vendorId , string txtSearch)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    var debitMemoList = _IDebitMemo.GetDebitListByVendorId();
                    return Json(debitMemoList.ToList(), JsonRequestBehavior.AllowGet);
                }
                else {
                    var debitMemoList = _IDebitMemo.GetDebitListByVendorId();
                    return Json(debitMemoList.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GetDebitMemoListByLocationId(long? LocationId, string txtSearch)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                long UserId = 0;
                if (Session != null && Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    if (ObjLoginModel != null)
                    {

                        LocationId = ObjLoginModel.LocationID;
                    }
                    UserId = ObjLoginModel.UserId;
                }
                if (!String.IsNullOrEmpty(txtSearch))
                {
                    var debitMemoList = _IDebitMemo.GetDebitListByLocationId(LocationId).Where(x=>!string.IsNullOrEmpty(x.VendorName));
                    var FilterList = debitMemoList.Where(x => x.VendorName.ToLower().Contains(txtSearch.ToLower())).ToList();
                    return Json(FilterList.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var debitMemoList = _IDebitMemo.GetDebitListByLocationId(LocationId);
                    return Json(debitMemoList.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveDebitMemoByVendorId(DebitMemoModel model) //, HttpPostedFileBase File
        {
            try
            {                   
                if (model.DebitMemoFile != null)
                {
                    string AttachmentName = model.VendorId + "_" + DateTime.Now.Ticks.ToString() + "_" + model.DebitMemoFile.FileName.ToString();                    
                    CommonHelper.StaticUploadImage(model.DebitMemoFile, Server.MapPath(ConfigurationManager.AppSettings["DebitMemoDocuments"]), AttachmentName);
                    model.UploadedDocumentName = AttachmentName;
                }

                if (model.DebitId > 0 && model.editNewDocument != null)//to delete old document if exists and replacing new one
                {
                    string AttachmentName = model.DebitId + "_" + DateTime.Now.Ticks.ToString() + "_" + model.editNewDocument.FileName.ToString();
                    CommonHelper.StaticDeleteFIle(null, Server.MapPath(ConfigurationManager.AppSettings["DebitMemoDocuments"]), model.UploadedEditDocumentName);
                    CommonHelper.StaticUploadImage(model.editNewDocument, Server.MapPath(ConfigurationManager.AppSettings["DebitMemoDocuments"]), AttachmentName);
                    model.UploadedDocumentName = AttachmentName;
                    model.DebitMemoStatus = model.DebitMemoStatusEdit;
                }
                //during edit if old document exists and no new file is provided--so persisting old document
                if (model.editNewDocument == null && model.UploadedEditDocumentName != null) {
                    model.UploadedDocumentName = model.UploadedEditDocumentName;
                    model.DebitMemoStatus = model.DebitMemoStatusEdit;
                }
                if (model.DebitId > 0) {//for all edit case
                    model.DebitMemoStatus = model.DebitMemoStatusEdit;
                }

                var debitMemoList = _IDebitMemo.SaveNewDebitMemo(model);
                return Json(debitMemoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}