using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class SalonModel
    {
        [JsonProperty(PropertyName = "SalonId")]
        public int SalonId { get; set; }

        [JsonProperty(PropertyName = "SalonName")]
        public string SalonName { get; set; }

        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "EmailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "IsActive")]
        public bool IsActive { get; set; }
        
        [JsonProperty(PropertyName = "IsPublicNotes")]
        public bool IsPublicNotes { get; set; }

        [JsonProperty(PropertyName = "TotalRecords")]
        public int TotalRecords { get; set; }
        [JsonProperty(PropertyName = "PublicNotes")]
        public string PublicNotes { get; set; }
        [JsonProperty(PropertyName = "SalonLogo")]
        public string SalonLogo { get; set; }

        [JsonIgnore]
        public IFormFile File { get; set; }
    }
    public class SalonTotalRecordModel
    {
        [JsonProperty(PropertyName = "RecordCount")]
        public int RecordCount { get; set; }
    }
    public class SalonLoginDetail
    {
        public string SalonName { get; set; }
        public string SalonLogo { get; set; }
    }

}
