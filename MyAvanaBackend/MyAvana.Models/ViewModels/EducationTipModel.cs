using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Models.ViewModels
{
    public class EducationTipModel
    {
        public int EducationTipsId { get; set; }
        public string Description { get; set; }
        public bool ShowOnMobile { get; set; }
        public bool IsActive { get; set; }
        public int TotalRecords { get; set; }
    }
}
