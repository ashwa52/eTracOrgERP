using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Controllers.Guest
{

    public class GuestController : Controller
    {
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ICompanyAdmin _ICompanyAdmin;
        private readonly IGuestUser _IGuestUserRepository;
        private readonly IApplicantManager _IApplicantManager;
        private readonly IFillableFormManager _IFillableFormManager;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ApplicantSignature = ConfigurationManager.AppSettings["ApplicantSignature"];
        public GuestController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICompanyAdmin _ICompanyAdmin, IGuestUser _GuestUserRepository, IApplicantManager _IApplicantManager, IFillableFormManager _IFillableFormManager)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._ICompanyAdmin = _ICompanyAdmin;
            this._IGuestUserRepository = _GuestUserRepository;
            this._IApplicantManager = _IApplicantManager;
            this._IFillableFormManager = _IFillableFormManager;
        }
        //
        // GET: /Guest/
        public ActionResult Index()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            CommonApplicantModel model = new CommonApplicantModel();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            var employee = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
            var commonModel = _IGuestUserRepository.GetApplicantAllDetails(ObjLoginModel.UserId);
            model.ApplicantId = employee.ApplicantId;
            model.ApplicantPersonalInfo = new List<Models.ApplicantPersonalInfo>();
            Models.ApplicantPersonalInfo a1 = new Models.ApplicantPersonalInfo();
            a1.API_APT_ApplicantId = employee.ApplicantId;
            a1.API_FirstName = employee.FirstName;
            a1.API_LastName = employee.LastName;
            a1.API_SSN = employee.SocialSecurityNumber;
            model.ApplicantPersonalInfo.Add(a1);
            model.ApplicantAddress = new List<ApplicantAddress>();
            ApplicantAddress a2 = new ApplicantAddress();
            a2.APA_APT_ApplicantId = employee.ApplicantId;
            a2.APA_StreetAddress = employee.Address;
            a2.APA_City = employee.City;
            a2.APA_Apartment = employee.APIUnit;
            a2.APA_State = employee.State;
            a2.APA_YearsAddressFrom = employee.YearsAtAddrss;
            a2.APA_APT_ApplicantId = employee.ApplicantId;
            model.ApplicantAddress.Add(a2);

            model.AplicantAcadmicDetails = new List<AplicantAcadmicDetails>();
            AplicantAcadmicDetails aad1 = new AplicantAcadmicDetails();
            aad1.AAD_APT_ApplicantId = employee.ApplicantId;
            model.AplicantAcadmicDetails.Add(aad1);
            model.ApplicantBackgroundHistory = new List<ApplicantBackgroundHistory>();
            ApplicantBackgroundHistory abh1 = new ApplicantBackgroundHistory();
            abh1.ABH_ApplicantId = employee.ApplicantId;
            model.ApplicantBackgroundHistory.Add(abh1);
            model.ApplicantAccidentRecord = new List<ApplicantAccidentRecord>();
            ApplicantAccidentRecord aar1 = new ApplicantAccidentRecord();
            aar1.AAR_ApplicantId = employee.ApplicantId;
            model.ApplicantAccidentRecord.Add(aar1);

            model.ApplicantPositionTitle = new List<ApplicantPositionTitle>();
            ApplicantPositionTitle pt1 = new ApplicantPositionTitle();
            pt1.APT_ApplicantId = employee.ApplicantId;
            model.ApplicantPositionTitle.Add(pt1);

            model.ApplicantContactInfo = new List<Models.ApplicantContactInfo>();
            var c1 = new Models.ApplicantContactInfo();
            c1.ACI_APT_ApplicantId = employee.ApplicantId;
            c1.ACI_eMail = employee.Email;
            c1.ACI_PhoneNo = employee.Phone.Value;
            model.ApplicantContactInfo.Add(c1);
            model.ApplicantTrafficConvictions = new List<ApplicantTrafficConvictions>();
            ApplicantTrafficConvictions obj = new ApplicantTrafficConvictions();
            obj.ATC_APT_ApplicantId = employee.ApplicantId;
            ApplicantTrafficConvictions obj2 = new ApplicantTrafficConvictions();
            obj2.ATC_APT_ApplicantId = employee.ApplicantId;
            ApplicantTrafficConvictions obj3 = new ApplicantTrafficConvictions();
            obj3.ATC_APT_ApplicantId = employee.ApplicantId;
            model.ApplicantTrafficConvictions.Add(obj);
            model.ApplicantTrafficConvictions.Add(obj2);
            model.ApplicantTrafficConvictions.Add(obj3);

            model.ApplicantLicenseHeald = new List<ApplicantLicenseHeald>();
            ApplicantLicenseHeald obj4 = new ApplicantLicenseHeald();
            obj4.ALH_ApplicantId = employee.ApplicantId;
            ApplicantLicenseHeald obj5 = new ApplicantLicenseHeald();
            obj5.ALH_ApplicantId = employee.ApplicantId;
            ApplicantLicenseHeald obj6 = new ApplicantLicenseHeald();
            obj6.ALH_ApplicantId = employee.ApplicantId;
            model.ApplicantLicenseHeald.Add(obj4);
            model.ApplicantLicenseHeald.Add(obj5);
            model.ApplicantLicenseHeald.Add(obj6);

            model.ApplicantAdditionalInfo = new List<ApplicantAdditionalInfo>();
            ApplicantAdditionalInfo ad1 = new ApplicantAdditionalInfo();
            ad1.AAI_ApplicantId = employee.ApplicantId;
            model.ApplicantAdditionalInfo.Add(ad1);

            model.ApplicantVehiclesOperated = new List<ApplicantVehiclesOperated>();
            ApplicantVehiclesOperated vo = new ApplicantVehiclesOperated();
            vo.AVO_ApplicantId = employee.ApplicantId;
            model.ApplicantVehiclesOperated.Add(vo);
            Session["ApplicantId"] = employee.ApplicantId; model.ApplicantId = employee.ApplicantId;
            return View("~/Views/Guest/Index1.cshtml", model);
        }
        [HttpPost]
        public ActionResult Index(CommonApplicantModel CommonApplicantModel)
        {
            ApplicantManager _IApplicantManager = new ApplicantManager();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            var getDetails = _IApplicantManager.SaveApplicantData(CommonApplicantModel);
            if (ModelState.IsValid)
            {
                var isSaveSuccess = true;
                if (isSaveSuccess)
                    return RedirectToAction("_W4Form");
                else
                {
                    ViewBag.message = "Something went wrong!!!";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult PersonalFile(bool? isSaved)
        {
            return View();
        }
        [Route("welcome")]
        [HttpGet]
        public ActionResult LandingPage()
        {
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult _DirectDepositeForm()
        {
            DirectDepositeFormModel model = new DirectDepositeFormModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
            return PartialView("_directDepositeForm", model);
        }
        [HttpPost]
        public ActionResult _DirectDepositeForm(DirectDepositeFormModel model)
        {
            if (model != null)
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                var save = _IGuestUserRepository.SetDirectDepositeFormData(model, ObjLoginModel.UserId);
                return Json(save, JsonRequestBehavior.AllowGet);
                //return PartialView("PartialView/_CommonModals", new ContactListModel());
            }
            ViewBag.NotSaved = true;
            return Json(true, JsonRequestBehavior.AllowGet);
            //return PartialView("PartialView/_CommonModals", new ContactListModel());

        }
        [HttpGet]
        public PartialViewResult _EmployeeHandbook()
        {
            EmployeeHandbookModel model = new EmployeeHandbookModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetEmployeeHandBookByUserId(ObjLoginModel.UserId);
            return PartialView("_employeeHandbook", model);
        }
        [HttpPost]
        public ActionResult _EmployeeHandbook(EmployeeHandbookModel model)
        {
            if (ModelState.IsValid)
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                _IGuestUserRepository.SetEmployeeHandbookData(model, ObjLoginModel.UserId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.NotSaved = true;
            return PartialView("_employeeHandbook", model);
        }
        [HttpGet]
        public PartialViewResult _I9Form()
        {
            var getI9Info = new I9FormModel();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (applicantId > 0)
            {
                getI9Info = _IApplicantManager.GetI9FormData(applicantId, ObjLoginModel.UserId);
                //getI9Info.IsSignature = isSignature;
                return PartialView("_I9Form", getI9Info); ;
            }
            return PartialView("_I9Form", getI9Info);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To save I9 form Details of applicant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _I9Form(I9FormModel model)
        {
            var _model = new EmergencyContectForm();
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                var saved = _IApplicantManager.SetI9Form(objloginmodel.UserId, applicantId, model);
                if (saved)
                    return RedirectToAction("_emergencyContactForm");
                else
                    return RedirectToAction("_I9Form");
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_emergencyContactForm");
            //return PartialView("_emergencyContactForm", _model);
            //return PartialView("_I9Form");
        }
        [HttpGet]
        public PartialViewResult _W4Form()
        {
            W4FormModel model = new W4FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
            ViewBag.IsSignature = false;//To filup form no need to display signature button so we make it hide
            return PartialView("_W4Form", model);
        }
        //[HttpPost]
        //public ActionResult _W4Form(W4FormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var objloginmodel = (eTracLoginModel)(Session["etrac"]);
        //        _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    return PartialView("_W4Form", model);
        //}

        [HttpPost]
        public ActionResult _W4Form(W4FormModel model)
        {
            if (model != null)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
                return RedirectToAction("_I9Form", model.IsSignature);
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_I9Form",model.IsSignature);
            //return PartialView("_I9Form", model);
        }
        [HttpGet]
        public PartialViewResult _PhotoReleaseForm()
        {
            PhotoRelease model = new PhotoRelease();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
            model.Name = d;
            return PartialView("_PhotoReleaseForm", model);
        }
        [HttpPost]
        public ActionResult _photoreleaseform(PhotoRelease model)
        {
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);


                _IGuestUserRepository.SetPhotoRelease(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.notsaved = true;
            return PartialView("_photoreleaseform", model);
        }

        [HttpGet]
        public PartialViewResult _EducationVarificationForm()
        {
            EducationVarificationModel model = new EducationVarificationModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId);
            return PartialView("_EducationVarificationForm", model);
        }
        [HttpPost]
        public ActionResult _EducationVarificationForm(EducationVarificationModel model)
        {
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetEducationVerificationForm(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            model.IsSave = false;
            return PartialView("_EducationVarificationForm", model);
        }
        [HttpGet]
        public PartialViewResult _ConfidentialityAgreementForm()
        {
            return PartialView("_ConfidentialityAgreementForm");
        }
        [HttpPost]
        public ActionResult _ConfidentialityAgreementForm(ConfidenialityAgreementModel model)
        {
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetConfidenialityAgreementForm(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_ConfidentialityAgreementForm", model);
        }
        [HttpGet]
        public PartialViewResult _CreditCardAuthorizationForm()
        {
            return PartialView("_CreditCardAuthorizationForm");
        }
        [HttpGet]
        public PartialViewResult _PreviousEmployeement()
        {
            return PartialView("_PreviousEmployeement");
        }
        [HttpGet]
        public PartialViewResult _emergencyContactForm()
        {
            var model = new EmergencyContectForm();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
            return PartialView("_emergencyContactForm", model);
        }
        //[HttpPost]
        //public ActionResult _emergencyContactForm(EmergencyContectForm model)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		var objloginmodel = (eTracLoginModel)(Session["etrac"]);
        //		_IGuestUserRepository.SetEmergencyForm(objloginmodel.UserId, model);
        //		return Json(true, JsonRequestBehavior.AllowGet);
        //	}
        //	return PartialView("_emergencyContactForm", model);
        //}
        [HttpPost]
        public ActionResult _emergencyContactForm(EmergencyContectForm model)
        {
            var _model = new DirectDepositeFormModel();
            if (model != null)
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                _IGuestUserRepository.SetEmergencyForm(ObjLoginModel.UserId, model);
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("_directDepositeForm");
            }
            return RedirectToAction("_directDepositeForm");
            //return PartialView("_directDepositeForm", _model);
        }
        [HttpGet]
        public ActionResult GetFormsStatus()
        {
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var data = _IGuestUserRepository.GetFormsStatus(objloginmodel.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Create For : TO open Contact Modal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _ContactInfo()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var getApplicantContact = _IApplicantManager.GetContactListByApplicantId(getApplicantId);
            return PartialView("PartialView/_CommonModals", getApplicantContact);
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 11-Feb-2020
        /// Created For : To update Contact Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ContactSavedForm(ContactListModel model, List<ContactModel> lstModel)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                if (lstModel.Count() > 0)
                {
                    var updateContact = _IApplicantManager.UpdateContactDetailsApplicant(model, lstModel);
                    if (updateContact)
                    {
                        return RedirectToAction("_BackGroundCheckForm");
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_BackGroundCheckForm");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To Get Applicant details by applicant Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _BackGroundCheckForm()
        {
            var getApplicant = new BackgroundCheckForm();
            try
            {
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                getApplicant = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }

            catch (Exception ex)
            {
                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To send Applicant Details For Backgroud check and Create for same
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _BackGroundCheckForm(BackgroundCheckForm model)
        {
            var _model = new ContactListModel();
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                model.ApplicantPersonalInfo.API_APT_ApplicantId = getApplicantId;
                model.UserId = ObjLoginModel.UserId;
                var sendForBackgroundCheck = _IApplicantManager.SendApplicantInfoForBackgrounddCheck(model);
                var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                if (sendForBackgroundCheck == true)
                {
                    //return PartialView("PartialView/_UploadDocuments", _model);
                    return Json(true, JsonRequestBehavior.AllowGet);
                    //return PartialView("PartialView/_UploadDocuments");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("PartialView/_CommonModals");
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To upload applicant file .
        /// </summary>
        /// <param name="filesLicense"></param>
        /// <param name="filesSSN"></param>
        /// <returns></returns>
        public ActionResult UploadFilesApplicant(bool isLicense)
        {
            var Obj = new UploadedFiles();
            var _db = new workorderEMSEntities();

            eTracLoginModel ObjLoginModel = null;
            //Serves as the base class for classes that provide access to files that were uploaded by a client.
            HttpFileCollectionBase files = Request.Files;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            if (files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        var getUser = _db.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (getUser != null)
                        {
                            if (fname != null)
                            {
                                string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["ApplicantFiles"]), FName);
                                Obj.AttachedFileName = FName;
                                Obj.FileName = fname;
                                Obj.FileEmployeeId = getUser.EmployeeID;
                                Obj.FileId = Convert.ToInt64(Helper.FileType.FileType);
                                var IsSaved = _IFillableFormManager.SaveFile(Obj, Obj.FileEmployeeId);
                            }
                        }
                    }
                    // Returns message that successfully uploaded  
                    if (isLicense == false)
                    {
                        return RedirectToAction("BenifitSection");
                    }
                    else { return Json("File Uploaded Successfully!"); }
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-Feb-2020
        /// Created For : To get signature by applicant Id
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSignature()
        {
            var getSignature = new Desclaimer();
            var url = string.Empty;
            try
            {
                var signature = string.Empty;
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                if (getApplicantId > 0)
                {
                    getSignature = _IApplicantManager.GetSignature(getApplicantId);
                    if (getSignature != null)
                    {
                        url = HostingPrefix + ApplicantSignature.Replace("~", "") + getSignature.Signature + ".jpg";
                        return Json(new { name = getSignature.Signature, imagePath = url }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { name = getSignature.Signature, imagePath = url }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Feb-2020
        /// Created For :  To save and update signature by using update condition.
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public JsonResult SaveSignature(bool isUpdate)
        {
            var Obj = new Desclaimer();
            var _db = new workorderEMSEntities();
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            eTracLoginModel ObjLoginModel = null;
            if (isUpdate == true)
            {
                HttpFileCollectionBase files = Request.Files;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string fname;
                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            var getUser = _db.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                            if (getUser != null)
                            {
                                if (fname != null)
                                {
                                    string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                    CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"]), FName);
                                    Obj.Signature = FName;
                                    Obj.Signature = fname;
                                    Obj.EmployeeId = getUser.EmployeeID;
                                    var IsSaved = _IApplicantManager.SaveDesclaimerData(Obj);
                                }
                            }
                        }
                        // Returns message that successfully uploaded  
                        return Json("File Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("No files selected.");
                }
            }
            else
            {

                var getDetails = _IApplicantManager.GetSignature(getApplicantId);
                if (getDetails != null)
                {
                    Obj.ApplicantId = getDetails.ApplicantId;
                    Obj.ASG_Date = getDetails.ASG_Date;
                    Obj.EmployeeId = getDetails.EmployeeId;
                    Obj.IsActive = getDetails.IsActive;
                    Obj.Sing_Id = getDetails.Sing_Id;
                    Obj.Signature = getDetails.Signature;
                    var set = _IApplicantManager.SaveDesclaimerData(Obj);
                }
                return Json("Signature is added successfully.");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Feb-2020
        /// Created For  : To return partial view of Benifi section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult BenifitSection()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            //var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
            return PartialView("PartialView/_BenifitSectionFloridaBlue");
        }
        /// <summary>
        /// Created by  :Ashwajit Bansod
        /// Created Date : 20-Feb-2020
        /// Created For : To save Benifits from florida blue
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BenifitSection(BenifitSectionModel obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            return  Json(true,JsonRequestBehavior.AllowGet);
            //return RedirectToAction("SelfIdentificationForm");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Feb-2020
        /// Created For : To open self identification form if applciant want to make there data confedential
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelfIdentificationForm()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            //var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
            return PartialView("PartialView/_SelfIdentificationForm");
        }
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : TO save self identification form of applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSelfIdentificationForm(SelfIdentificationModel obj)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        obj.EmployeeId = ObjLoginModel.EmployeeID;
                        var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                    }
                }
                if (obj != null)
                {
                    var isSaved = _IApplicantManager.SaveSelfIdentification(obj);
                    if(isSaved)
                        return RedirectToAction("ApplicantFunFacts");
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(false,JsonRequestBehavior.AllowGet); //RedirectToAction("ApplicantFunFacts");
            }
            return RedirectToAction("ApplicantFunFacts");
        }
        [HttpGet]
        public ActionResult ApplicantFunFacts()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            //var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
            return PartialView("PartialView/_ApplicantFunFact");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : to save fun fact questions and anwers
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplicantFunFact(ApplicantFunFactModel Obj)
        {
            try
            {
                W4FormModel model = new W4FormModel();
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        Obj.Employee_Id = ObjLoginModel.EmployeeID;
                        Obj.Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
                    }
                }
                if (Obj != null)
                {
                    var isSaved = _IApplicantManager.SaveApplicantFunFacts(Obj);
                    if (isSaved)
                    {
                        var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                        model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
                        ViewBag.IsSignature = true;//To filup form no need to display signature button so we make it hide
                       // ViewBag.IsSignature = 
                        return PartialView("_W4Form", model);                        
                    }
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet); //RedirectToAction("ApplicantFunFacts");
            }
            return RedirectToAction("ApplicantFunFacts");
        }
    }
}