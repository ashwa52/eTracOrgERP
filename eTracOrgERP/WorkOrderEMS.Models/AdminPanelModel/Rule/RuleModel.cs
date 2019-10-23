using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class RuleModel
    {
        public long RuleId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9, -/_]+$", ErrorMessage = "Special characters and numbers are not allowed.")]
        public string RuleName { get; set; }

        [Required]
        public string Level { get; set; }

        [Required]
        [RegularExpression("^[0-9,. ]+$", ErrorMessage = "Letters are not allowed.")]
        public decimal? SlabFrom { get; set; }

        [Required]
        [RegularExpression("^[0-9,. ]+$", ErrorMessage = "Letters are not allowed.")]
        public decimal? SlabTo { get; set; }

        
        public long? ByPassUserId { get; set; }

        
        [RegularExpression("^[0-9,]+$", ErrorMessage = "Letters are not allowed.")]
        public string PassLevel { get; set; }
       

        [Required]
        public Nullable<DateTime> Date { get; set; }
        public string IsActive { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z, -]+$", ErrorMessage = "Special characters and numbers are not allowed.")]
        public string ModuleName { get; set; }
        public long ModuleId { get; set; }
    }
    public class RuleModelDetails
    {
        public int total { get; set; }
        public int pageindex { get; set; }
        public int records { get; set; }
        public List<RuleModel> rows { get; set; }
    }
}
