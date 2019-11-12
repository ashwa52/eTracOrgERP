using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IAdminDashboard
    {
        List<AddChartModel> ListSuperiour();
        bool SaveVSC(AddChartModel Obj);
        List<AddChartModel> ListVehicleSeatingChart(long? LocationId);
        bool SaveJobTitleVSC(AddChartModel Obj);
        List<AddChartModel> GetJobTitleData(long CSVChartId);
        AddChartModel GetChartData(long CSVChartId);
        List<AccessPermisionTreeViewModel> ListTreeViewAccessPermission(long VST_Id);
        bool SaceAccessPermission(AccessPermisionTreeViewModel obj);
        bool SaveJobPosting(JobPostingModel Obj);
        List<JobPostingModel> GetChartHiringManager(long VSCId);
    }
}
