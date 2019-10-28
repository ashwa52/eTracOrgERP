using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class DepartmentRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        //Save Department Data
        public bool SaveDepartmentRepository(DepartmentModel Obj)
        {
            bool isSave = false;
            try
            {               
                var save = objworkorderEMSEntities.spSetDepartment(Obj.Action, Obj.DeptId, Obj.DepartmentName, Obj.IsActive) ;
                isSave = true;
            }
            catch(Exception ex)
            {
                isSave = false;
                throw;

            }
            return isSave;
        }
        //Get Department List
        public List<spGetDepartment_Result> GetDepartmentList(string txt, long? LocationId, long? UserId)
        {
            try
            {
                var data = objworkorderEMSEntities.spGetDepartment().ToList();
                return data;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        //To delete Department
        public bool IsDepetmentDeleted(DepartmentModel Obj)
        {
            bool isSave = false;
            try
            {
                var save = objworkorderEMSEntities.spSetDepartment(Obj.Action, Obj.DeptId, Obj.DepartmentName, Obj.IsActive);
                isSave = true;
            }
            catch (Exception ex)
            {
                isSave = false;
                throw;

            }
            return isSave;
        }
    }
}
