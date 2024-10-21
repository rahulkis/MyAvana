using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class ToolsModel
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "ToolName")]
        public string ToolName { get; set; }
        [JsonProperty(PropertyName = "ActualName")]
        public string ActualName { get; set; }
        [JsonProperty(PropertyName = "BrandName")]
        public string BrandName { get; set; }
        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }
        [JsonProperty(PropertyName = "ToolLink")]
        public string ToolLink { get; set; }
        [JsonProperty(PropertyName = "ToolDetails")]
        public string ToolDetails { get; set; }
        [JsonProperty(PropertyName = "Price")]
        public decimal Price { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime CreatedOn { get; set; }
        public string ActualPrice { get; set; }
        public string DecimalPrice { get; set; }

        public bool IsActive { get; set; }

    }
}
