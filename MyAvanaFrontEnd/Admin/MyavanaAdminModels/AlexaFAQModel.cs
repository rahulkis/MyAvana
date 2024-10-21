using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class AlexaFAQModel
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "ShortResponse")]
        public string ShortResponse { get; set; }

        [JsonProperty(PropertyName = "DetailedResponse")]
        public string DetailedResponse { get; set; }

        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "TotalRecords")]
        public int TotalRecords { get; set; }

    }
    
}
