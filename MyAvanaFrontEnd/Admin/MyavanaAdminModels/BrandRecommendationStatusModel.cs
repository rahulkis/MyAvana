using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class BrandRecommendationStatusModel
    {
        [JsonProperty(PropertyName = "BrandRecommendationStatusId")]
        public int BrandRecommendationStatusId { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
