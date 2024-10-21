using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class EducationalTipModel
    {
        [JsonProperty(PropertyName = "EducationTipsId")]
        public int EducationTipsId { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "ShowOnMobile")]
        public bool ShowOnMobile { get; set; }

        [JsonProperty(PropertyName = "IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "TotalRecords")]
        public int TotalRecords { get; set; }

    }
    
}
