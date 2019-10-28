using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public class CommonApproval<T>
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        public Approval GetApprovalRuleData(ApprovalInput data)
        {
            var getRuleData = new spGetRule_Result();
            var returnRuleData = new Approval();
            try
            {
                if (data.UserId > 0)
                {
                    if (data.ModuleName != null)
                    {
                        getRuleData = _workorderems.spGetRule().Where(x => x.MDL_ModuleName == data.ModuleName
                                                                       && x.RUL_SlabFrom <= data.Amount && x.RUL_SlabTo >= data.Amount).FirstOrDefault();
                    }
                    //var getManager = _workorderems.spGetManager(data.UserId).FirstOrDefault();
                    if ( getRuleData != null)
                    {
                        var getApplyRule = _workorderems.spApplyRule(data.UserId, getRuleData.RUL_MDL_Id, getRuleData.RUL_RuleName
                                                                        , "Amount",data.Amount,null, null).FirstOrDefault();
                       
                        if (getApplyRule != null)
                        { 
                            //returnRuleData.CurrentLevel = data.CurrentLevel;
                            returnRuleData.Email = getApplyRule.UserEmail;
                            returnRuleData.UserId = Convert.ToInt64(getApplyRule.ApprovedBy);
                            returnRuleData.RuleId = Convert.ToInt64(getRuleData.RUL_Id);
                            returnRuleData.RuleLevel = getApplyRule.RUL_Level;
                            returnRuleData.RuleName = getRuleData.RUL_RuleName;
                            returnRuleData.ManagerName = getApplyRule.FirstName;
                            returnRuleData.DeviceId = getApplyRule.DeviceId;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return returnRuleData;
        }
    }
}
