using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class StatusTrackerModel
    {
        [JsonProperty(PropertyName = "StatusTrackerId")]
        public int StatusTrackerId { get; set; }
        [JsonProperty(PropertyName = "CustomerName")]
        public string CustomerName { get; set; }
        [JsonProperty(PropertyName = "HairAnalysisStatus")]
        public string HairAnalysisStatus { get; set; }
        [JsonProperty(PropertyName = "LastUpdatedOn")]
        public DateTime LastUpdatedOn { get; set; }
        [JsonProperty(PropertyName = "LastModifiedBy")]
        public string LastModifiedBy { get; set; }
        [JsonProperty(PropertyName = "HairAnalysisStatusId")]
        public int HairAnalysisStatusId { get; set; }
        [JsonProperty(PropertyName = "CustomerId")]
        public string CustomerId { get; set; }
        [JsonProperty(PropertyName = "CustomerEmail")]
        public string CustomerEmail { get; set; }
        [JsonProperty(PropertyName = "KitSerialNumber")]
        public string KitSerialNumber { get; set; }
    }
    public class HairAnalysisStatusModel
    {
        public int HairAnalysisStatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class HairAnalysisStatusHistoryModel
    {
        public string OldStatusName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedByUser { get; set; }
        public string CreatedDate { get; set; }
    }
}
