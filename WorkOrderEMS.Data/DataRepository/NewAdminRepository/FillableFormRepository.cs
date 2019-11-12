using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Data.DataRepository
{
    public class FillableFormRepository
    {
        workorderEMSEntities _workorderems = new workorderEMSEntities();
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get Education Form details
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetEducationVerificationForm_Result1 GetEducationFormDetails(string EmployeeId)
        {
            try
            {
                return _workorderems.spGetEducationVerificationForm(EmployeeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get direct deposit form
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetDirectDepositForm_Result2 GetDirectDepositeDetails(string EmployeeId)
        {
            try
            {
                return _workorderems.spGetDirectDepositForm(EmployeeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created Fro : To get Emergency Contact Info
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public spGetEmergencyContactForm_Result2 GetEmergencyContactForm(string EmployeeId)
        {
            try
            {
                return _workorderems.spGetEmergencyContactForm(EmployeeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Oct-2019
        /// Created For : To get File type list 
        /// </summary>
        /// <returns></returns>
        public List<FileType> GetFileList()
        {
            try
            {
                return _workorderems.FileTypes.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int SetFiles(UploadedFiles Obj, string EmployeeId)
        {
            try
            {
                return _workorderems.spSetFileUpload(Obj.Action,Obj.Id, Obj.FileEmployeeId, Obj.FileId, Obj.FileName,Obj.AttachedFileName,EmployeeId,"Y");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
