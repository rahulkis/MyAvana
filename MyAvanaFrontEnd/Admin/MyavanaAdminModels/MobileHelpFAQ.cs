using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class MobileHelpFAQ
    {
        [JsonProperty(PropertyName = "MobileHelpFAQId")]
        public int MobileHelpFAQId { get; set; }
        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "Videolink")]
        public string Videolink { get; set; }
        [JsonProperty(PropertyName = "ImageLink")]
        public string ImageLink { get; set; }
        [JsonProperty(PropertyName = "VideoThumbnail")]
        public string VideoThumbnail { get; set; }
        [JsonProperty(PropertyName = "IsActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        [JsonProperty(PropertyName = "CreatedBy")]
        public string CreatedBy { get; set; }
        [JsonProperty(PropertyName = "LastModifiedOn")]
        public DateTime? LastModifiedOn { get; set; }
        [JsonProperty(PropertyName = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonProperty(PropertyName = "MobileHelpTopicId")]
        public int? MobileHelpTopicId { get; set; }
        [JsonProperty(PropertyName = "MobileHelpTopicDescription")]
        public string MobileHelpTopicDescription { get; set; }
    }

    public class MobileHelpTopic
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
