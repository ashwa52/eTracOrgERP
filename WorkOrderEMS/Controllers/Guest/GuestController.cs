using System.Collections.Generic;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Controllers.Guest
{

    public class GuestController : Controller
    {
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ICompanyAdmin _ICompanyAdmin;
        private readonly IGuestUser _IGuestUserRepository;
        public GuestController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICompanyAdmin _ICompanyAdmin, IGuestUser _GuestUserRepository)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._ICompanyAdmin = _ICompanyAdmin;
            this._IGuestUserRepository = _GuestUserRepository;
        }
        //
        // GET: /Guest/
        public ActionResult Index()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            CommonApplicantModel model = new CommonApplicantModel();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            //model = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
            model.ApplicantPersonalInfo = new List<ApplicantPersonalInfo>();
            ApplicantPersonalInfo a1 = new ApplicantPersonalInfo();
            model.ApplicantPersonalInfo.Add(a1);
            model.ApplicantAddress = new List<ApplicantAddress>();
            ApplicantAddress a2 = new ApplicantAddress();
            model.ApplicantAddress.Add(a2);

            model.AplicantAcadmicDetails = new List<AplicantAcadmicDetails>();
            AplicantAcadmicDetails aad1 = new AplicantAcadmicDetails();
            model.AplicantAcadmicDetails.Add(aad1);
            model.ApplicantBackgroundHistory = new List<ApplicantBackgroundHistory>();
            ApplicantBackgroundHistory abh1 = new ApplicantBackgroundHistory();
            model.ApplicantBackgroundHistory.Add(abh1);
            model.ApplicantAccidentRecord = new List<ApplicantAccidentRecord>();
            ApplicantAccidentRecord aar1 = new ApplicantAccidentRecord();
            model.ApplicantAccidentRecord.Add(aar1);

            model.ApplicantContactInfo = new List<ApplicantContactInfo>();
            ApplicantContactInfo c1 = new ApplicantContactInfo();
            model.ApplicantContactInfo.Add(c1);
            model.ApplicantTrafficConvictions = new List<ApplicantTrafficConvictions>();
            ApplicantTrafficConvictions obj = new ApplicantTrafficConvictions();
            ApplicantTrafficConvictions obj2 = new ApplicantTrafficConvictions();
            ApplicantTrafficConvictions obj3 = new ApplicantTrafficConvictions();
            model.ApplicantTrafficConvictions.Add(obj);
            model.ApplicantTrafficConvictions.Add(obj2);
            model.ApplicantTrafficConvictions.Add(obj3);

            model.ApplicantLicenseHeald = new List<ApplicantLicenseHeald>();
            ApplicantLicenseHeald obj4 = new ApplicantLicenseHeald();
            ApplicantLicenseHeald obj5 = new ApplicantLicenseHeald();
            ApplicantLicenseHeald obj6 = new ApplicantLicenseHeald();
            model.ApplicantLicenseHeald.Add(obj4);
            model.ApplicantLicenseHeald.Add(obj5);
            model.ApplicantLicenseHeald.Add(obj6);

            model.ApplicantAdditionalInfo = new List<ApplicantAdditionalInfo>();
            ApplicantAdditionalInfo ad1 = new ApplicantAdditionalInfo();
            model.ApplicantAdditionalInfo.Add(ad1);

            model.ApplicantVehiclesOperated = new List<ApplicantVehiclesOperated>();
            ApplicantVehiclesOperated vo = new ApplicantVehiclesOperated();
            model.ApplicantVehiclesOperated.Add(vo);

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
                    return RedirectToAction("PersonalFile");
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
            if (ModelState.IsValid)
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                _IGuestUserRepository.SetDirectDepositeFormData(model, ObjLoginModel.UserId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.NotSaved = true;
            return PartialView("_directDepositeForm", model);

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
            return PartialView("_I9Form");
        }
        [HttpPost]
        public PartialViewResult _I9Form(W4FormModel model)
        {
            var _model = new EmergencyContectForm();
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);


               // _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
                //return Json(true, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_emergencyContactForm", _model);
            //return PartialView("_I9Form");
        }
        [HttpGet]
        public PartialViewResult _W4Form()
        {
            W4FormModel model = new W4FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);

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
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);


                _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_I9Form", model);
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
			return PartialView("_EducationVarificationForm",model);
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
			return PartialView("_EducationVarificationForm",model);
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
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetEmergencyForm(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return PartialView("_directDepositeForm", _model);
        }
        [HttpGet]
		public ActionResult GetFormsStatus()
		{
			
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);
				var data=_IGuestUserRepository.GetFormsStatus(objloginmodel.UserId);
				return Json(data, JsonRequestBehavior.AllowGet);
			
			
		}
	}
}