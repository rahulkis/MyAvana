using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class CustomerAIResult
    {
        public int CustomerAIResultId { get; set; }
        public Guid UserId { get; set; }
        public string AIResult { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        public int? HairProfileId { get; set; }

        public virtual HairProfile HairProfile { get; set; }
        public bool? IsVersion2 { get; set; }
        public string UserName { get; set; }
        public string UserAIImage { get; set; }
        public string UserUploadedImage { get; set; }
        public string AIResultNew { get; set; }
        public JObject AIResultNewDecoded { get; set; }
        public JObject AIResultTextureDecoded { get; set; }
        public bool? IsAIV2Mobile { get; set; }
        public JObject AIResultDecoded { get; set; }
        public string HairTextureLabelAIResult { get; set; }
        public string HairTypeLabelAIResult { get; set; }
        public string LabelAIResult { get; set; }
        public int? CountAIResults { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
