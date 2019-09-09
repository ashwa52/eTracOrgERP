using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class TreeViewModel
    {
        public long? id { get; set; }
        public long? MasteCostCodeId { get; set; }
        public long? CostCodeId { get; set; }
        public string name { get; set; }  //text
        public string CostCodeDesc { get; set; }
        public bool @checked { get; set; }
        public List<TreeViewModel> item { get; set; }  //children
    }
    public class Category
    {
        public int ID { get; set; } 
        public string Name { get; set; }
        //Cat Description  
        public string Description { get; set; }
        public int? Pid { get; set; }
        [ForeignKey("Pid")]
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Childs { get; set; }
    }
}
