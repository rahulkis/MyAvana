using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MyavanaAdminModels
{
    public class HairStateModel
    {
        [JsonProperty(PropertyName = "HairStateId")]
        public int HairStateId { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
