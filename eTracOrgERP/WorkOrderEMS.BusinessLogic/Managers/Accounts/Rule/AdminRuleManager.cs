using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class AdminRuleManager : IRuleManager
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Jan-2019
        /// Created For  : To get all rule data list
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="numberOfRows"></param>
        /// <param name="sortColumnName"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public RuleModelDetails GetRuleList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)
        {
            try
            {
                var objContractTypeDetails = new RuleModelDetails();
                int pageindex = Convert.ToInt32(pageIndex) - 1;
                int pageSize = Convert.ToInt32(numberOfRows);
                var Results = _workorderems.spGetRule()
                    .Select(a => new RuleModel()
                    {
                       ByPassUserId = a.RUL_ByPass_UserId,
                       Date = a.RUL_Date,
                       Level = a.RUL_Level,
                       RuleName = a.RUL_RuleName,
                       SlabFrom = a.RUL_SlabFrom,
                       SlabTo = a.RUL_SlabTo, 
                       RuleId = a.RUL_Id,
                       IsActive = a.RUL_IsActive,
                       ModuleName = a.MDL_ModuleName
                    }).ToList();

                int totRecords = Results.Count();
                var totalPages = (int)Math.Ceiling((float)totRecords / (float)numberOfRows);
                objContractTypeDetails.pageindex = pageindex;
                objContractTypeDetails.total = totalPages;
                objContractTypeDetails.records = totRecords;
                objContractTypeDetails.rows = Results.ToList();
                return objContractTypeDetails;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public RuleModelDetails GetRuleList(long? UserId, int? pageIndex, int? numberOfRows, string sortColumnName, string sortOrderBy)", "Exception While Getting List of Rules.", null);
                throw;
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To save rule and module to database
        /// Created Date : 22-Jan-2019
        /// </summary>
        /// <param name="objRuleModel"></param>
        /// <returns></returns>
        public bool SaveRule(RuleModel objRuleModel)
        {
            bool IsSaved = false;
            string action = "";
            try
            {
                if (objRuleModel.RuleId == 0)
                {
                    action = "I";
                    var saveModule = _workorderems.spSetModule(action, null, objRuleModel.ModuleName, "Y");
                    var getModuleData = _workorderems.Modules.Where(x => x.MDL_ModuleName == objRuleModel.ModuleName).FirstOrDefault();
                    if (getModuleData != null)
                    {
                        if(objRuleModel.ByPassUserId == null)
                        {
                            objRuleModel.PassLevel = "0";
                            objRuleModel.ByPassUserId = 0;
                        }
                        var saveRule = _workorderems.spSetRule(action, null, getModuleData.MDL_Id, objRuleModel.RuleName, objRuleModel.Level,
                                                                       objRuleModel.SlabFrom, objRuleModel.SlabTo, objRuleModel.ByPassUserId, objRuleModel.PassLevel, "Y");
                        IsSaved = true;
                    }
                    else
                    {
                        IsSaved = false;
                    }
                }
                else
                {
                    action = "U";
                    if (objRuleModel.ByPassUserId == null)
                    {
                        objRuleModel.PassLevel = "0";
                        objRuleModel.ByPassUserId = 0;
                    }
                    var saveRule = _workorderems.spSetRule(action, objRuleModel.RuleId, objRuleModel.ModuleId, objRuleModel.RuleName, objRuleModel.Level,
                                                                   objRuleModel.SlabFrom, objRuleModel.SlabTo, objRuleModel.ByPassUserId, objRuleModel.PassLevel
                                                                   ,"Y");
                    IsSaved = true;
                }              
            }
            catch (Exception ex)
            {
                IsSaved = false;
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public bool SaveRule(RuleModel objRuleModel)", "Exception While Saving Rule and Module.", objRuleModel);
                throw;
                
            }
            return IsSaved;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 22-Jan-2019
        /// Created For : To get all employee user ---for now I get all location employee will change it later
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<EmployeeListModel> GetUserDataList(long LocationId, long UserId)
        {
            var getData =new EmployeeListModel();
            var getUserList = new List<EmployeeListModel>();
            try
            {
                if (LocationId > 0)
                {
                    ObjectParameter totalRecord = new ObjectParameter("TotalRecords", typeof(int));
                    getUserList = _workorderems.SP_GetAllActiveUser(UserId,1,"UserId","Desc",100,"",0, "All Users", totalRecord).Select(a => new EmployeeListModel()
                     {
                        UserId = a.UserId,
                        UserName = a.Name
                     }).ToList();
                }
                else
                {
                    getUserList = null;
                }
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<EmployeeListModel> GetUserDataList(long LocationId, long UserId)", "Exception While getting the list of user.", LocationId);
                throw;
            }
            return getUserList;
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get data for edit rule
        /// created Date : 24-Jan-2018
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public RuleModel GetRuleDetailsById(long ruleId)
        {
            var obj = new RuleModel();
            try
            {
                if(ruleId > 0)
                {
                    obj = _workorderems.Rules.Join(_workorderems.Modules,r => r.RUL_MDL_Id,m => m.MDL_Id,(r,m) =>  new { r,m}).Where(x => x.r.RUL_Id == ruleId && x.r.RUL_IsActive == "Y").Select
                        (a => new RuleModel() {
                            ByPassUserId = a.r.RUL_ByPass_UserId,
                            Date = a.r.RUL_Date,
                            Level = a.r.RUL_Level,
                            ModuleId = a.r.RUL_MDL_Id,
                            PassLevel = a.r.RUL_ByPassLevel,
                            ModuleName = a.m.MDL_ModuleName,
                            RuleId = a.r.RUL_Id,
                            RuleName = a.r.RUL_RuleName,
                            SlabFrom = a.r.RUL_SlabFrom,
                            SlabTo = a.r.RUL_SlabTo
                        }).FirstOrDefault();
                }
                else
                {
                    obj = null;
                }
            }
            catch(Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public RuleModel GetRuleDetailsById(long ruleId)", "Exception While getting the data of rule.", ruleId);
                throw;
            }
            return obj;
        }
    }
}
