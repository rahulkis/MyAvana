using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvana.Models.Entities
{
    public class EducationTip
    {
        [Key]
        public int EducationTipsId { get; set; }
        public string Description { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool IsActive { get; set; }
    }
}
