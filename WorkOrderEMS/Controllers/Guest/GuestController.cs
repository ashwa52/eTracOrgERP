using System;
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
			if (ModelState.IsValid) { 
			var isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfo(model);
			if(isSaveSuccess)
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
			if(ModelState.IsValid)
			{
				var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
				_IGuestUserRepository.SetDirectDepositeFormData(model, ObjLoginModel.UserId);
				Session["etrac_isDirectDepositeSaved"] = false;
				return Json(true,JsonRequestBehavior.AllowGet);
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
	}
}