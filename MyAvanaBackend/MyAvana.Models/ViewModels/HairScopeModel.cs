using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
namespace MyAvana.Models.ViewModels
{
    public class Bbox
    {
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }    
    public class detectionData
    {
        public string @class { get; set; }
        public double confidence { get; set; }
        public Bbox bbox { get; set; }
        public string color { get; set; }
        public string image { get; set; }
    }

    public class HairScopeModel
    {
        public string sender { get; set; }
        public Guid AccessCode { get; set; }
        public bool hashed { get; set; }
        public List<detectionData> detectionData { get; set; }
        public int? HairProfileId { get; set; }
        public int? QAVersion { get; set; }
    }

    public class HairScopeModelParameters
    {        
        public Guid UserID { get; set; }       
        public int? HairProfileId { get; set; }        
    }
}
