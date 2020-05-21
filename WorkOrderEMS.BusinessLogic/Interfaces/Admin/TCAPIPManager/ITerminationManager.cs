using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.NewAdminModel.TCAPIP;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface ITerminationManager
    {
        TerminationModel TerminationFormEmployeeDetails(string EmpId);

        TerminationModel EmployeeTerminationFormAlldetails(string EmpId);

        bool SaveTerminationFormDetails(TerminationModel Obj);

        List<TerminationListModel> TerminationlistTeamDetails(string LoginMangerId, DateTime? ToDate,DateTime? FromDate);
        
        List<TerminationListModel> TerminationlistFinalizedetails(long man_id, DateTime? ToDate, DateTime? FromDate);
        
        List<TerminationListModel> TerminationlistPendingdetails(long man_id, DateTime? ToDate, DateTime? FromDate);

        bool SendServeranceEmail(TerminationModel Obj, long LoginEmployeeId);

        bool SendTerminationFormEmail(TerminationModel Obj, long LoginEmployeeId);

        bool SaveTerminationWitnessDetails(WitnessModel Obj,string TEmpId);

        EmployeeDetailsModel TerminationEmployeeDetails(string EmpId);

        List<EmployeeAssetDetails> TerminationEmployeeAssetDetails(string EmpId);

        bool SaveAssetStatusReturnDate(List<EmployeeAssetDetails> obj,string EmpId, string LoginEmployeeId);

        bool SendAssetDetailsEmail(EmployeeDetailsModel obj, List<EmployeeAssetDetails> obj1);

        bool SaveNotificationForHrDenial(WitnessModel obj, string EmpId, string LoginEmployeeId);

        bool SaveHrDenyReasonandComment(WitnessModel obj, string EmpId);

        bool SaveNotificationForHrApproval(string EmpId, string LoginEmployeeId);

        TerminationModel TerminationEmployeeDetailsforReview(string EmpId);

        

    }
}
