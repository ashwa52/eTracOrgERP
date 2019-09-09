using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.WorkOrderAssignment
{
    public class WorkOrderController : Controller
    {
        //GET: WorkOrder
        private readonly IUnclosedWOManager _IUnclosedWOManager;

        public WorkOrderController(IUnclosedWOManager _IUnclosedWOManager)
        {
            this._IUnclosedWOManager = _IUnclosedWOManager;
        }

        [HttpGet]
        public JsonResult UnclosedWorkOrder(string _search, long? LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            ViewBag.AccountSection = true;
            if (Session != null && Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = ObjLoginModel.LocationID;
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
                var AllUnClosedWOList = _IUnclosedWOManager.GetAllUnOrderList(LocationId, rows, TotalRecords, sidx, sord);
                foreach (var workOrder in AllUnClosedWOList.rows)
                {
                    JQGridRow row = new JQGridRow();
                    row.id = Cryptography.GetEncryptedData(Convert.ToString(workOrder.WorkRequestId), true);
                    row.cell = new string[4];                   
                    row.cell[0] = workOrder.WorkOrder == null ? "N/A" : workOrder.WorkOrder;
                    row.cell[1] = workOrder.AssignedTo == null ? "N/A" : workOrder.AssignedTo;
                    row.cell[2] = workOrder.StartTime == null ? "N/A" : workOrder.StartTime;
                    row.cell[3] = workOrder.LocationName == null ? "N/A" : workOrder.LocationName;
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

    }
}