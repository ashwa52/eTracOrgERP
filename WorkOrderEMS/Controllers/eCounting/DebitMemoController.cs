using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces.eCounting;

namespace WorkOrderEMS.Controllers.eCounting
{
    public class DebitMemoController : Controller
    {
        private readonly IDebitMemo _IDebitMemo;

        // GET: DebitMemo
        /// <summary>
        /// Created By: Er.Ro.R
        /// Created Date : 8-OCT-2019
        /// Created For : To get Debit Memo List as per Vendor
        /// </summary>
        public ActionResult Index()
        {

            return View();
        }
    }
}