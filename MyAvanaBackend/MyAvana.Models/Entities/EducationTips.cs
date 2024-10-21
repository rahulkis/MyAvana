using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyAvana.Models.Entities
{
    public class EducationTips
    {
        [Key]
        public int EducationTipsId { get; set; }
        public string Description { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool IsActive { get; set; }
    }
}
