using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class IngredientsModel
    {
        public int IngedientsEntityId { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }  
        [JsonProperty(PropertyName = "Image")]
        public string Image { get; set; }
        [JsonProperty(PropertyName = "ImageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty(PropertyName = "Description")]        
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Challenges")]
        public string Challenges { get; set; }

        public IFormFile File { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool? IsActive { get; set; }

    }
}
