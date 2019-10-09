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
		private readonly IGuestUserRepository _IGuestUserRepository;
		public GuestController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICompanyAdmin _ICompanyAdmin, IGuestUserRepository _GuestUserRepository)
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
			var isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfo(model);
			if(isSaveSuccess)
			return RedirectToAction("PersonalFile");
			else
			{
				ViewBag.message = "Something went wrong!!!";
				return View(model);
			}
		}
		[HttpGet]
		public ActionResult PersonalFile()
		{
			return View();
		}

	}
}