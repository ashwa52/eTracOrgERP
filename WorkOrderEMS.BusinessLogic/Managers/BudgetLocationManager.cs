using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class BudgetLocationManager : IBudgetLocationManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Aug-2018
        /// Created For : To fetch cost code data as per locationId
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <param name="locationId"></param>
        /// <param name="textSearch"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public BudgetDetails GetListBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objBudgetDetails = new BudgetDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<BudgetForLocationModel>();
                var obj = new BudgetForLocationModel();
               var DataGrid = db.spGetBudgetCostCodeMapping(LocationId).Select(a => new BudgetForLocationModel()
                {
                    CostCode = a.CLM_CCD_CostCode,
                    AssignedAmount = a.BCM_BudgetAmount,
                    Description = a.CCD_Description,
                    AssignedPercent = a.BCM_BudgetPercent,
                    RemainingAmount = a.BCM_BalanceAmount,
                    Year = a.BCM_BudgetYear,
                    BudgetAmount = a.BLM_BudgetAmount,
                    BCM_Id = a.BCM_Id,
                    CLM_Id = a.CLM_Id,
                    BudgetStatus = "O",
                    BudgetSource = a.BCM_BudgetSource
                 }).ToList();
                Results = DataGrid.Where(a => a.BudgetSource == "BUDGETED" || a.BudgetSource == null).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objBudgetDetails.pageindex = pageindex;
                objBudgetDetails.total = totalPages;
                objBudgetDetails.records = totRecords;
                objBudgetDetails.rows = Results.ToList();
                return objBudgetDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BudgetDetails GetListBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Cost Code.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : August-22-2018
        /// Created For : To save Budget amount
        /// </summary>
        /// <param name="BudgetAmount"></param>
        /// <param name="LocationId"></param>
        /// <param name="BudgetYear"></param>
        /// <returns></returns>
        public bool SaveBudgetAmount(decimal BudgetAmount, long LocationId, int BudgetYear)
        {
            bool IsSaved = false;
            string action = "";
            try
            {
                if (BudgetAmount > 0 && LocationId > 0 && BudgetYear > 0)
                {
                    var budgetAmtData = _workorderems.BudgetLocationMappings.Where(x => x.BLM_LocationId == LocationId).FirstOrDefault();
                    if (budgetAmtData != null && budgetAmtData.BLM_LocationId > 0)
                    {
                        action = "U";
                        var saveBudget = _workorderems.spSetBudgetLocationMapping(action, LocationId, BudgetAmount, BudgetYear);
                        IsSaved = true;
                    }
                    else
                    {
                        action = "I";
                        var saveBudget = _workorderems.spSetBudgetLocationMapping(action, LocationId, BudgetAmount, BudgetYear);
                        IsSaved = true;
                    }
                }
                else
                {
                    IsSaved = false;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveBudgetAmount(decimal BudgetAmount, long LocationId, int BudgetYear)", "Exception While saving Budget for this Location.", null);
                throw;
            }
            return IsSaved;
        }

        /// <summary>
        /// Created by : Ashwajit bansod
        /// created Date : Aug-23-2018
        /// Created For : To save all calculated grid data of cost code
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        public bool SaveAllGridBudgetData(List<BudgetForLocationModel> obj, long LocationId, int Year)
        {
            bool IsSaved = false;
            string action = "";
            try
            {
                if (obj.Count > 0 && LocationId > 0)
                {
                    foreach (var item in obj)
                    {
                        if (item.BCM_Id != null)
                        {
                            action = "U";
                            var saveBudget = _workorderems.spSetBudgetCostCodeMapping(action, item.BCM_Id, item.CLM_Id,
                                                                                      item.AssignedPercent, item.AssignedAmount,
                                                                                      item.BudgetStatus, item.Year);
                        }
                        else
                        {
                            item.Year = Year;
                            action = "I";
                            var saveBudget = _workorderems.spSetBudgetCostCodeMapping(action, item.BCM_Id, item.CLM_Id,
                                                                                      item.AssignedPercent, item.AssignedAmount,
                                                                                      item.BudgetStatus, item.Year);
                        }
                    }                    
                    IsSaved = true;
                }
                else
                {
                    IsSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveBudgetAmount(decimal BudgetAmount, long LocationId, int BudgetYear)", "Exception While saving Budget for this Location.", null);
                throw;
            }
            return IsSaved;
        }


        public List<BudgetForLocationModel> ListOfCostCode(long LocationId, decimal BudgetAmount, long CLM_Id)
        {
            var listCostCode = new List<BudgetForLocationModel>();            
            try
            {
                if(LocationId > 0 && BudgetAmount > 0)
                {
                   var dataCostCode = _workorderems.spGetBudgetCostCodeMappingForTransfer(LocationId, CLM_Id, BudgetAmount).ToList();
                   if(dataCostCode.Count > 0)
                    {
                        listCostCode = dataCostCode.Select(x => new BudgetForLocationModel()
                        {
                            Description = x.CCD_Description,
                            CostCode = x.CLM_CCD_CostCode,
                            BCM_Id = x.BCM_Id,
                            CLM_Id = x.CLM_Id,
                            RemainingAmount=x.BCM_BalanceAmount,
                           
                        }).ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<BudgetForLocationModel> ListOfCostCode(long LocationId, decimal BudgetAmount)", "Exception While getting List of Cost Code for this Location.", null);
                throw;
            }
            return listCostCode;
        }

        public BudgetDetails GetListOfTransferCostCodeForLocationDetails(long? LocationId, decimal RemainingAmt,long CLMId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objBudgetDetails = new BudgetDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<BudgetForLocationModel>();
                 var  DataGrid = db.spGetBudgetCostCodeMappingForTransfer(LocationId, CLMId,  RemainingAmt)
                .Select(a => new BudgetForLocationModel()
                {
                    CostCode = a.CLM_CCD_CostCode,
                    //AssignedAmount = a.,
                    Description = a.CCD_Description,
                    AssignedPercent = 0,
                    RemainingAmount = a.BCM_BalanceAmount,
                    Year = a.BCM_BudgetYear,
                    //BudgetAmount = a.,
                    BCM_Id = a.BCM_Id,
                    CLM_Id = a.CLM_Id,
                    BudgetStatus = "O"
                }).ToList();
                Results = DataGrid.Where(a => a.BudgetSource == "BUDGETED" || a.BudgetSource == null).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objBudgetDetails.pageindex = pageindex;
                objBudgetDetails.total = totalPages;
                objBudgetDetails.records = totRecords;
                objBudgetDetails.rows = Results.ToList();
                return objBudgetDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BudgetDetails GetListOfTransferCostCodeForLocationDetails(long? LocationId, decimal RemainingAmt, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Transfer Cost Code Amount.", null);
                throw;
            }
        }
        
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To Save Transfer amount of cost code to database
        /// Created Date : Aug-28-2018
        /// </summary>
        /// <param name="objBudgetForLocationModel"></param>
        /// <returns></returns>
        public bool SaveTransferAmount(BudgetForLocationModel objBudgetForLocationModel)
        {
            bool IsSaved = false;
            string Action = "";
            try
            {
                if(objBudgetForLocationModel.LocationId > 0  && objBudgetForLocationModel.BudgetAmount > 0 && objBudgetForLocationModel.Year > 0 )
                {
                    if (objBudgetForLocationModel.BudgetSource == "OVERDUE")
                    {Action = "O";}
                    if (objBudgetForLocationModel.BudgetSource == "INTRA")
                    { Action = "I"; }
                    if (objBudgetForLocationModel.BudgetSource == "TRANSFERED")
                    { Action = "T"; }
                    var savedData = _workorderems.spSetBudgetCostCodeMappingForTransfer(Action, objBudgetForLocationModel.BCM_Id, objBudgetForLocationModel.CLM_Id,
                                                                                       objBudgetForLocationModel.BCM_CLM_TransferId,
                                                                                     objBudgetForLocationModel.AssignedPercent, objBudgetForLocationModel.BudgetAmount,
                                                                                     objBudgetForLocationModel.BudgetSource, objBudgetForLocationModel.Year);
                    IsSaved = true;
                }
                else
                {
                    IsSaved = false;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCostCodeTransferAmount(BudgetForLocationModel objBudgetForLocationModel)", "Exception While saving Budget Amount to transfer to cost code.", null);
                throw;
            }
            return IsSaved;
        }

        public TranferAmountForCostCodeModel GetCostCodeDetailsByCostCodeId(long CostCodeId, long LocationId)
        {
            var obj = new TranferAmountForCostCodeModel();
            try
            {
                if (CostCodeId > 0 && LocationId > 0)
                {
                    obj = _workorderems.BudgetCostCodeMappings.Join(_workorderems.CostCodeLocationMappings, b => b.BCM_CLM_Id, c => c.CLM_Id, (b, c) => new { b, c }).
                    Where(x => x.c.CLM_CCD_CostCode == CostCodeId && x.c.CLM_LocationId == LocationId).
                    Select(a => new TranferAmountForCostCodeModel()
                    {
                        BCM_Id = a.b.BCM_Id,
                        CLM_Id =a.b.BCM_CLM_Id,
                        CostCode = a.c.CLM_CCD_CostCode,
                        AssignedPercent = a.b.BCM_BudgetPercent,
                        Year = a.b.BCM_BudgetYear,
                        LocationIdToTransfer = a.c.CLM_LocationId
                    }).FirstOrDefault();
                }
            }
           catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveCostCodeTransferAmount(BudgetForLocationModel objBudgetForLocationModel)", "Exception While saving Budget Amount to transfer to cost code.", null);
                throw;
            }
            return obj;
        }


        public BudgetDetails GetListOfCostCodeAfterTransferBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)
        {
            try
            {
                var db = new workorderEMSEntities();
                var objBudgetDetails = new BudgetDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = new List<BudgetForLocationModel>();
                var obj = new BudgetForLocationModel();
                var DataGrid = db.spGetBudgetCostCodeMapping(LocationId).Select(a => new BudgetForLocationModel()
                {
                    CostCode = a.CLM_CCD_CostCode,
                    AssignedAmount = a.BCM_BudgetAmount,
                    Description = a.CCD_Description,
                    AssignedPercent = a.BCM_BudgetPercent,
                    RemainingAmount = a.BCM_BalanceAmount,
                    Year = a.BCM_BudgetYear,
                    BudgetAmount = a.BLM_BudgetAmount,
                    BCM_Id = a.BCM_Id,
                    CLM_Id = a.CLM_Id,
                    BudgetStatus = "O",
                    BudgetSource = a.BCM_BudgetSource
                }).ToList();
                Results = DataGrid.Where(a => a.BudgetSource != "BUDGETED" && a.BudgetSource != null).ToList();
                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                //Results = Results.OrderByDescending(s => s.CostCode);
                objBudgetDetails.pageindex = pageindex;
                objBudgetDetails.total = totalPages;
                objBudgetDetails.records = totRecords;
                objBudgetDetails.rows = Results.ToList();
                return objBudgetDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public BudgetDetails GetListBudgetDetails(long? LocationId, long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy, long? locationId, string textSearch, string statusType)", "Exception While Getting List of Cost Code.", null);
                throw;
            }
        }
        public string GetLocationName(long LocationId)
        {
            var tt= _workorderems.LocationMasters.Where(x => x.LocationId == LocationId).FirstOrDefault();
            return tt.LocationName;
        }

    }
}
