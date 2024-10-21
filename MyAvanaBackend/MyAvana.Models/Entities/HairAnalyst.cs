using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class HairAnalyst
    {
        [Key]
        public int HairAnalystId { get; set; }
        public string AnalystName { get; set; }
        public string AnalystImage { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
