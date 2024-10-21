using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class MolecularWeightModel
    {
        [JsonProperty(PropertyName = "MolecularWeightId")]
        public int MolecularWeightId { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
