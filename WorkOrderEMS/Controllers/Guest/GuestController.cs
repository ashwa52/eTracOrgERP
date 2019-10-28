﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Data.Interfaces;
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
			EmployeeVIewModel model = new EmployeeVIewModel();
			ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
			model = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
			return View(model);
		}
		[HttpPost]
		public ActionResult Index(EmployeeVIewModel model)
		{
			ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
			if (ModelState.IsValid)
			{
				var isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfo(model);
				if (isSaveSuccess)
					return RedirectToAction("PersonalFile");
				else
				{
					ViewBag.message = "Something went wrong!!!";
					return View(model);
				}
			}
			return View(model);
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
		[HttpGet]
		public PartialViewResult _W4Form()
		{
			W4FormModel model = new W4FormModel();
			var objloginmodel = (eTracLoginModel)(Session["etrac"]);
			model= _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
		
			return PartialView("_W4Form",model);
		}
		[HttpPost]
		public ActionResult _W4Form(W4FormModel model)
		{
			if (ModelState.IsValid)
			{
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);


				_IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			
			return PartialView("_W4Form", model);
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
			EmergencyContectForm model = new EmergencyContectForm();
			var objloginmodel = (eTracLoginModel)(Session["etrac"]);
			model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
			return PartialView("_emergencyContactForm", model);
		}
		[HttpPost]
		public ActionResult _emergencyContactForm(EmergencyContectForm model)
		{
			if (ModelState.IsValid)
			{
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);
				_IGuestUserRepository.SetEmergencyForm(objloginmodel.UserId, model);
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			return PartialView("_emergencyContactForm", model);
		}
	}
}