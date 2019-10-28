using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WorkOrderEMS.Models
{
    public class AddChartModel
    {
        [Display(Name = "Seating Name")]
        public string SeatingName { get; set; }
        public long? Id { get; set; }
        public long? JobTitleId { get; set; }
        public long? parentId { get; set; }
        [Display(Name = "Superior")]
        public long Superior { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        public string  JobDesc { get; set; }
        [AllowHtml]
        [Display(Name = "Roles And Responsibility")]
        public string RolesAndResponsibility { get; set; }
        [Display(Name = "Department")]
        public long? Department { get; set; }
        public string Action { get; set; }
        public string IsActive { get; set; }
        public string DepartmentName { get; set; }
        public string Image { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleDesc { get; set; }
        public string  JobTitleLabel { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentClassification { get; set; }
        public decimal? RateOfPay { get; set; }
        public long RequisitionId { get; set; }
        public string EmployeeId { get; set; }
        public string RequisitionType { get; set; }
        public string ActionStatus { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public int? JobTitleCount { get; set; }
        //public string[] JDSplitedString { get; set; }
        //public long? MyProperty { get; set; }
    }

    public class BindDropDownList
    {
        public List<AddChartModel> listSuperiour { get; set; }
        public List<DepartmentModel> listDepartment { get; set; }
    }
    public class JobTitleModel
    {
        public long JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public int? JobTitleCount { get; set; }
        public int? JobTitleLastCount { get; set; }
    }
}
